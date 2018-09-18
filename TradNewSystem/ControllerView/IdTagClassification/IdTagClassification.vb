Option Strict On
Option Explicit On

Imports TradNewSystem.Helpers
Imports TradNewSystem.Model
Imports TradNewSystem.PocoClass

Imports System.Runtime.InteropServices
Imports System.Globalization
Imports System.Data

Imports DNWA.BHTCL
Imports MySql.Data.MySqlClient

Public Class frm_IdTagClassification

#Region "Declaration"
    Friend WithEvents myScanner As Scanner

    Protected Friend hdr_NowLoading As NowLoading

    Private mbool_True As Boolean = True
    Private mbool_False As Boolean = False
    Private bool_ProcessKeypadPress As Boolean = True
    Private mstr_PackQty As New List(Of String)
    Private mstr_NewBarcode As New List(Of String)
    Private mstr_NewQR As New List(Of String)
    Private mstr_SplitProdDateList As New List(Of String)

    Private mdate_CurrDate As Date = Date.Now

    Private mint_StandardQty As Int32 = 0
    Private int_DbLoadDuration As Integer
    Private lint_ActQty As Int32
    Private lint_PackTotal As Int32 = 0

    Private lstr_TempQRCode As String = String.Empty
    Private lstr_TempTRINPartNo As String = String.Empty
    Private lstr_Barcode As String = String.Empty
    Private lstr_TRINNo As String = String.Empty
    Private lstr_BarcodeDate As String = String.Empty
    Private lstr_BarcodeLineName As String = String.Empty
    Private lstr_ImageName As String = String.Empty
    Private lstr_TempDateNew As String = String.Empty

    Private mint_SplitFlag As Int16

    Private lstr_LineCode As String
    Private configData As ConfigMgr.ConfigData

    Private Enum QrCodeValues
        BarcodeLine
        BarcodeTag
        ActQty
        TrinPartCode
    End Enum

    Private Enum InsertTagClassificationValues
        FinalID
        BarcodeTag
        ProdDate
        TrinPartNo
        LineCode
        ActQty
        UserID
        QrCode
        ImgFile
        Remarks
    End Enum

    Private Enum InsertStockCardValues
        TrinPartNo
        ActID
        Stock_In
        Remark
        BarcodeTag
        LineCode
        Date_Time
        UserID
    End Enum

#End Region

#Region "Sub New"
    Public Sub New(ByVal _lineCode As String)

        InitializeComponent()

        Me.lstr_LineCode = _lineCode
    End Sub
#End Region

#Region "Form"
    Private Sub frm_IdTagClassification_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        BHTController.DisposeScanner(myScanner)
    End Sub

    Private Sub frm_IdTagClassification_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txt_ScanTag.Focus()

        subInitForm(False)
    End Sub

#End Region

#Region "Scanner Process"
    Private Sub myScanner_OnDone(ByVal sender As Object, ByVal e As System.EventArgs) Handles myScanner.OnDone
        Try
            Try
                If RF.SYNCHRONIZE(RF.SYNC_CHECK) <> 0 Then
                    BHTController.DisposeScanner(myScanner)
                    DisplayMessage.ErrorMsg("Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
                        "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.", "Error")
                    BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                    Exit Sub
                End If
            Catch ex As Exception
                If Err.Number = 5 Then
                    BHTController.DisposeScanner(myScanner)
                    DisplayMessage.ErrorMsg("Koneksi Wifi di HT tertutup." & vbCrLf & _
                        "Tunggu beberapa detik dan ulangi lagi.", "Error")
                    Dim MyRf As RF
                    MyRf = New RF()
                    MyRf.OpenMode = RF.EN_OPEN_MODE.CONTINUOUSLY
                    MyRf.Open = True
                    BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                    Exit Sub
                End If
            End Try

            Dim scn_ScanCode = myScanner.Input(Scanner.ALL_BUFFER)

            Dim str_QrItems As String()

            tmr_DbLoad.Enabled = True
            Dim QrValid As QueryRetValue = ProductionActDB.fncCheckQR(scn_ScanCode)

            Dim productionAct As ProductionAct = ProductionActDB.GetProdDateLineCode(scn_ScanCode, True)

            subCloseNowLoading()

            If QrValid <> QueryRetValue.ValueTrue Then
                BHTController.DisposeScanner(myScanner)
                DisplayMessage.ErrorMsg("QR code tidak valid", "Error")
                BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                Exit Sub
            End If



            If String.IsNullOrEmpty(lstr_TempQRCode) Then
                lstr_TempQRCode = scn_ScanCode
            Else
                If lstr_TempQRCode <> scn_ScanCode Then
                    BHTController.DisposeScanner(myScanner)
                    DisplayMessage.ErrorMsg("Telah melakukan scan, mohon selesaikan proses atau silahkan kembali ke menu utama", "Error")
                    BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                    Exit Sub
                End If

                str_QrItems = lstr_TempQRCode.Split(New Char() {";"c})

                ' Total quantity for given barcode in stockCard > 0?
                'Ver 109
                Dim exceptionMsg As String = String.Empty
                Dim balanceQty As Integer = StockCardSummaryDB.GetStockBalanceQty( _
                    str_QrItems(QrCodeValues.BarcodeTag), _
                    exceptionMsg _
                    )

                If balanceQty <= 0 Then
                    If exceptionMsg.Length > 0 Then
                        BHTController.DisposeScanner(myScanner)
                        DisplayMessage.ErrorMsg(exceptionMsg, "DB Error")
                        BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                        Exit Sub

                    Else
                        Dim errorMsg As String = String.Format( _
                            "Tidak Terdapat Sisa Stock Untuk QR Code {0} di Sistem", _
                            str_QrItems(QrCodeValues.BarcodeTag) _
                            )

                        BHTController.DisposeScanner(myScanner)
                        DisplayMessage.ErrorMsg(errorMsg, "Error")
                        BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                        Exit Sub
                    End If

                End If


                If txt_ScanTag.Focus Then
                    BHTController.SoundOK()

                    txt_ScanTag.Text = str_QrItems(QrCodeValues.BarcodeTag)
                    lint_ActQty = Convert.ToInt32(str_QrItems(QrCodeValues.ActQty))
                    lstr_TempTRINPartNo = str_QrItems(QrCodeValues.TrinPartCode)

                    mint_StandardQty = ProductionActClassificationDB.fncGetStdQty(lstr_TempTRINPartNo)
                    lbl_StandardQtyValue.Text = mint_StandardQty.ToString
                    lstr_Barcode = txt_ScanTag.Text

                    mstr_SplitProdDateList.Add(productionAct.PRODDATE.ToString("yyyy-MM-dd HH:mm:ss"))

                    subQrCheck(scn_ScanCode)
                End If
            End If

        Catch ex As Exception
            BHTController.DisposeScanner(myScanner)
            DisplayMessage.ErrorMsg(ex.Message, "Error")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
        End Try
    End Sub

    Private Sub txt_ScanTag_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_ScanTag.LostFocus
        BHTController.DisposeScanner(myScanner)
    End Sub

    Private Sub txt_ScanTag_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_ScanTag.GotFocus
        BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
    End Sub
#End Region

#Region "DB Load Handler"
    Private Sub tmr_DbLoad_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmr_DbLoad.Tick
        If int_DbLoadDuration < 3 Then
            int_DbLoadDuration += 1
        Else
            tmr_DbLoad.Enabled = False

            int_DbLoadDuration = 0

            subShowNowLoading()
        End If
    End Sub

    Private Sub subShowNowLoading()
        hdr_NowLoading = New NowLoading()
        hdr_NowLoading.Show()
        subEnableControl(False)
    End Sub

    Private Sub subCloseNowLoading()
        tmr_DbLoad.Enabled = False

        int_DbLoadDuration = 0

        If Not IsNothing(hdr_NowLoading) Then
            hdr_NowLoading.Close()
            hdr_NowLoading = Nothing
        End If

        subEnableControl(True)
    End Sub

#End Region

#Region "Control Handler"
    Private Sub subEnableControl(ByVal _bool As Boolean)
        btn_FullScreen.Enabled = _bool
        btn_Split.Enabled = _bool
        btn_Back.Enabled = _bool
        bool_ProcessKeypadPress = _bool
    End Sub

#End Region

#Region "Event Click Handler"
    Private Sub btn_FullScreen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_FullScreen.Click
        ToggleTaskbar.EnableDisable()
    End Sub

    Private Sub btn_Split_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Split.Click
        'add by 9i
        lint_PackTotal = 0
        mstr_PackQty.Clear()

        If fncCountTotalPack() Then
            subSplitHandler()
        End If
    End Sub

    Private Sub btn_Back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Back.Click
        subClose()
    End Sub

#End Region

#Region "Keypress Handler"
    Private Sub subKeypressShortcut(ByVal e As System.Windows.Forms.KeyEventArgs)
        If bool_ProcessKeypadPress Then
            Select Case e.KeyCode
                Case Windows.Forms.Keys.F1
                    If fncCountTotalPack() Then
                        subSplitHandler()
                    End If
                Case Windows.Forms.Keys.F2
                    subClose()
            End Select
        End If
    End Sub

    Private Sub frm_IdTagClassification_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        subKeypressShortcut(e)
    End Sub

    Private Sub btn_Split_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btn_Split.KeyUp
        subKeypressShortcut(e)
    End Sub

    Private Sub btn_Back_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        subKeypressShortcut(e)
    End Sub

    Private Sub btn_FullScreen_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btn_FullScreen.KeyUp
        subKeypressShortcut(e)
    End Sub

    Private Sub txt_ScanTag_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ScanTag.KeyUp
        subKeypressShortcut(e)
    End Sub

    Private Sub txt_Tag1Value_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Tag1Value.KeyPress
        e.Handled = fncCheckTextboxNumeric(e.KeyChar)
    End Sub

    Private Sub txt_Tag1Value_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Tag1Value.KeyUp
        subKeypressShortcut(e)
    End Sub

    Private Sub txt_Tag2Value_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Tag2Value.KeyPress
        e.Handled = fncCheckTextboxNumeric(e.KeyChar)
    End Sub

    Private Sub txt_Tag2Value_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Tag2Value.KeyUp
        subKeypressShortcut(e)
    End Sub

    Private Sub txt_Tag3Value_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Tag3Value.KeyPress
        e.Handled = fncCheckTextboxNumeric(e.KeyChar)
    End Sub

    Private Sub txt_Tag3Value_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Tag3Value.KeyUp
        subKeypressShortcut(e)
    End Sub

    Private Sub txt_Tag4Value_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Tag4Value.KeyPress
        e.Handled = fncCheckTextboxNumeric(e.KeyChar)
    End Sub

    Private Sub txt_Tag4Value_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Tag4Value.KeyUp
        subKeypressShortcut(e)
    End Sub

#End Region

#Region "Function"
    'New Function 9H
    Private Function fncCreateBarcode(ByVal str_BarcodeLine As String, ByVal str_ID As String, ByVal int_ContBarcode As Int32) As String
        Dim str_NewBarcode As String
        Dim str_TempBarcode As String = String.Empty
        Dim str_BarcodeLastOrder As String = String.Empty
        Dim int_QtyLeft As Integer
        Dim int_Order As Int32 = 0

        Dim obj_TempMergeDate As Date = mdate_CurrDate

        tmr_DbLoad.Enabled = True
        Dim lstr_Date As Date = ProductionActIntegrationDB.fncGetBusinessDay()
        subCloseNowLoading()

        Dim str_NewDate As String = lstr_Date.ToString("yyyy/MM/dd")
        str_NewDate = str_NewDate.Replace("/", "")

        lstr_TempDateNew = String.Concat("%", str_NewDate, "%")

        str_BarcodeLastOrder = ProductionActIntegrationDB.fncGetLastBarcodeCountValue(lstr_TempDateNew, str_BarcodeLine, str_ID)
        int_Order = 1
        If Not String.IsNullOrEmpty(str_BarcodeLastOrder) Then
            int_Order = Convert.ToInt32(str_BarcodeLastOrder) + 1
        End If

        int_Order += int_ContBarcode

        Dim lobj_BarcodeBuilder As New System.Text.StringBuilder()
        str_NewBarcode = str_ID & Microsoft.VisualBasic.Right("0000" & int_Order, 5)
        Return lobj_BarcodeBuilder.Append(str_BarcodeLine).Append(str_NewDate).Append(str_NewBarcode).ToString()
    End Function

    Private Sub subClose()
        BHTController.DisposeScanner(myScanner)
        Dim obj_Confirm As Boolean = DisplayMessage.ConfirmationDialog("Kembali ke menu utama?", "Konfirmasi")
        BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)

        If obj_Confirm = True Then
            Close()
        End If
    End Sub

    Private Sub subInitForm(ByVal _bool As Boolean)
        Me.lbl_TRINPartNoTitle.Visible = _bool
        Me.lbl_TRINPartNoValue.Visible = _bool
        Me.lbl_ActualQtyTitle.Visible = _bool
        Me.lbl_ActualQtyValue.Visible = _bool
        Me.lbl_StandardQtyTitle.Visible = _bool
        Me.lbl_StandardQtyValue.Visible = _bool
        Me.lbl_SplitTag.Visible = _bool
        Me.lbl_SplitQty.Visible = _bool
        Me.lbl_Tag1.Visible = _bool
        Me.txt_Tag1Value.Visible = _bool
        Me.txt_Tag1Value.Enabled = _bool
        Me.lbl_Tag2.Visible = _bool
        Me.txt_Tag2Value.Visible = _bool
        Me.txt_Tag2Value.Enabled = _bool
        Me.lbl_Tag3.Visible = _bool
        Me.txt_Tag3Value.Visible = _bool
        Me.txt_Tag3Value.Enabled = _bool
        Me.lbl_Tag4.Visible = _bool
        Me.txt_Tag4Value.Visible = _bool
        Me.txt_Tag4Value.Enabled = _bool
        Me.lbl_TotalTitle.Visible = _bool
        Me.lbl_TotalValue.Visible = _bool
    End Sub

    Private Sub subQrCheck(ByVal str_ProdQr As String)
        tmr_DbLoad.Enabled = True

        Dim int_QrStatus As Integer = ProductionActDB.fncCheckQR(str_ProdQr)

        Dim str_TRINPartNo As String = ProductionActDB.fncGetTRINPartNo(str_ProdQr)

        subCloseNowLoading()

        If String.IsNullOrEmpty(str_ProdQr) Then
            BHTController.DisposeScanner(myScanner)
            DisplayMessage.ErrorMsg("Silahkan Scan QR code", "Error")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
        Else
            If int_QrStatus = 1 Then
                BHTController.DisposeScanner(myScanner)
                txt_ScanTag.Text = String.Empty
                DisplayMessage.ErrorMsg("QR code tidak valid", "Error")
                BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
            ElseIf int_QrStatus = 0 Then
                If String.IsNullOrEmpty(str_TRINPartNo) Then
                    BHTController.DisposeScanner(myScanner)
                    DisplayMessage.ErrorMsg("Nomor TRIN tidak ditemukan!", "Error")
                    BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                Else
                    If Not String.IsNullOrEmpty(lstr_TempTRINPartNo) Then
                        If str_TRINPartNo <> lstr_TempTRINPartNo Then
                            BHTController.DisposeScanner(myScanner)
                            DisplayMessage.ErrorMsg("Nomor TRIN tidak sama", "Error")
                            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                        Else
                            Me.lbl_TRINPartNoValue.Text = str_TRINPartNo
                            Me.lbl_ActualQtyValue.Text = lint_ActQty.ToString
                            lstr_TRINNo = str_TRINPartNo
                            subGetBarcodeInfo(lstr_LineCode)
                            subInitForm(True)
                        End If
                    Else
                        Me.lbl_TRINPartNoValue.Text = str_TRINPartNo
                        Me.lbl_ActualQtyValue.Text = lint_ActQty.ToString
                        lstr_TRINNo = str_TRINPartNo
                        subGetBarcodeInfo(lstr_LineCode)
                        subInitForm(True)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub subGetBarcodeInfo(ByVal str_BarcodeLine As String)
        tmr_DbLoad.Enabled = True
        Dim obj_LineName As String = LineMasterDB.fncGetLineCodes(lstr_LineCode)
        subCloseNowLoading()

        lstr_BarcodeLineName = obj_LineName
        lstr_BarcodeDate = lstr_Barcode.Substring(1, 8)

    End Sub

    Private Function fncCheckTextboxNumeric(ByVal KeyChar As Char) As Boolean
        Dim bool_Check As Boolean = False
        Dim str_AllowedChars As String = "0123456789"

        If str_AllowedChars.IndexOf(KeyChar) = -1 And (Asc(KeyChar)) <> 8 Then
            bool_Check = True
        End If

        Return bool_Check
    End Function

    Private Function fncCheckPackTotal(ByVal int_PackTotal As Int32) As Boolean
        Dim bool_Check As Boolean = False

        If int_PackTotal > lint_ActQty Then
            BHTController.DisposeScanner(myScanner)
            DisplayMessage.ErrorMsg("Qty berlebih", "Error")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
            mstr_PackQty.Clear()
        Else
            If int_PackTotal < lint_ActQty Then
                BHTController.DisposeScanner(myScanner)
                DisplayMessage.ErrorMsg("Qty kurang", "Error")
                BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                mstr_PackQty.Clear()
            Else
                If int_PackTotal = lint_ActQty Then
                    bool_Check = True
                End If
            End If
        End If

        Return bool_Check
    End Function

#End Region

#Region "Count Total"
    Private Function fncTotalQty(ByVal int_Total As Int32) As Int32
        int_Total = 0
        If Not String.IsNullOrEmpty(txt_Tag1Value.Text) Then
            If Convert.ToInt32(txt_Tag1Value.Text) <> 0 Then
                mstr_PackQty.Add(txt_Tag1Value.Text)
            Else
                txt_Tag1Value.Text = "0"
            End If
        End If
        If Not String.IsNullOrEmpty(txt_Tag2Value.Text) Then
            If Convert.ToInt32(txt_Tag2Value.Text) <> 0 Then
                mstr_PackQty.Add(txt_Tag2Value.Text)
            Else
                txt_Tag2Value.Text = "0"
            End If
        End If
        If Not String.IsNullOrEmpty(txt_Tag3Value.Text) Then
            If Convert.ToInt32(txt_Tag3Value.Text) <> 0 Then
                mstr_PackQty.Add(txt_Tag3Value.Text)
            Else
                txt_Tag3Value.Text = "0"
            End If
        End If
        If Not String.IsNullOrEmpty(txt_Tag4Value.Text) Then
            If Convert.ToInt32(txt_Tag4Value.Text) <> 0 Then
                mstr_PackQty.Add(txt_Tag4Value.Text)
            Else
                txt_Tag4Value.Text = "0"
            End If
        End If

        If mstr_PackQty.Count = 0 Then
            BHTController.DisposeScanner(myScanner)
            DisplayMessage.ErrorMsg("Silahkan isi Qty", "Error")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
        Else
            For int_Counter As Int32 = 0 To mstr_PackQty.Count - 1
                int_Total += Convert.ToInt32(mstr_PackQty(int_Counter))
                lbl_TotalValue.Text = int_Total.ToString
            Next
            lint_PackTotal = int_Total
        End If
        Return int_Total
    End Function

    Private Function fncCountTotalPack() As Boolean
        If fncTotalQty(lint_PackTotal) <> 0 Then
            Return fncCheckPackTotal(lint_PackTotal)
        Else
            lint_PackTotal = 0
            Return False
        End If
    End Function
#End Region

#Region "New QR Handler"
    Private Function fncQRCodeBuild(ByVal int_PackQtyUpd As List(Of String)) As List(Of String)
        Dim str_CreatedQR As New List(Of String)

        Dim lstr_NewQRLineCode As String = lstr_LineCode

        Dim lstr_NewBarcodeList As New List(Of String)
        lstr_NewBarcodeList = mstr_NewBarcode

        Dim lstr_NewQRBarcode As String = String.Empty

        Dim lstr_NewQRProdQty As String = String.Empty
        Dim lstr_NewQRTRINPartNo As String = lbl_TRINPartNoValue.Text

        For int_Counter As Integer = 0 To lstr_NewBarcodeList.Count - 1
            lstr_NewQRBarcode = lstr_NewBarcodeList(int_Counter)
            lstr_NewQRProdQty = int_PackQtyUpd(int_Counter).ToString()

            Dim lstr_NewQRStringBuilder As New System.Text.StringBuilder

            lstr_NewQRStringBuilder.Append(lstr_NewQRLineCode)
            lstr_NewQRStringBuilder.Append(";")
            lstr_NewQRStringBuilder.Append(lstr_NewQRBarcode)
            lstr_NewQRStringBuilder.Append(";")
            lstr_NewQRStringBuilder.Append(lstr_NewQRProdQty)
            lstr_NewQRStringBuilder.Append(";")
            lstr_NewQRStringBuilder.Append(lstr_NewQRTRINPartNo)

            str_CreatedQR.Add(lstr_NewQRStringBuilder.ToString)
        Next
        mstr_NewQR = str_CreatedQR
        Return str_CreatedQR
    End Function
#End Region

#Region "New Barcode Handler"
    Private Function fncCheckLastBarcodeOrder(ByVal str_Date As String, ByVal str_ID As String) As Boolean
        Dim bool_Check As Boolean = False

        If String.IsNullOrEmpty(lstr_Barcode) Then
            BHTController.DisposeScanner(myScanner)
            DisplayMessage.ErrorMsg("Silahkan Scan Barcode", "Error")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
        Else

            tmr_DbLoad.Enabled = True
            Dim obj_LastBarcodeCount As String = ProductionActClassificationDB.fncGetLastBarcodeCountValue(str_Date, lstr_LineCode, str_ID)
            subCloseNowLoading()

            If Not obj_LastBarcodeCount Is Nothing Then
                bool_Check = True
            Else
                bool_Check = False
            End If

        End If

        Return bool_Check
    End Function

    Private Function fncGenerateBarcode(ByVal str_BarcodeLine As String, ByVal str_ID As String) As List(Of String)
        Dim str_NewBarcode As New List(Of String)
        Dim str_TempBarcode As String = String.Empty
        Dim str_BarcodeLastOrder As String = String.Empty
        Dim int_Order As Int32 = 0

        Dim obj_TempMergeDate As Date = mdate_CurrDate

        tmr_DbLoad.Enabled = True
        Dim lstr_Date As Date = ProductionActIntegrationDB.fncGetBusinessDay()
        subCloseNowLoading()

        Dim str_NewDate As String = lstr_Date.ToString("yyyy/MM/dd")
        str_NewDate = str_NewDate.Replace("/", "")

        lstr_TempDateNew = String.Concat("%", str_NewDate, "%")

        str_BarcodeLastOrder = ProductionActClassificationDB.fncGetLastBarcodeCountValue(lstr_TempDateNew, lstr_LineCode, str_ID)

        If Not String.IsNullOrEmpty(str_BarcodeLastOrder) Then
            int_Order = Convert.ToInt32(str_BarcodeLastOrder)
        End If

        Try
            For int_Counter As Integer = 0 To mstr_PackQty.Count - 1
                If fncCheckLastBarcodeOrder(str_NewDate, str_ID) Then
                    int_Order += 1

                    Dim str_ModifiedOrder As String = int_Order.ToString()

                    Dim str_NewBarcodeOrder As String = fncBarcodeOrderBuild(str_ModifiedOrder)

                    Dim lobj_BarcodeBuilder As New System.Text.StringBuilder()
                    str_TempBarcode = lobj_BarcodeBuilder.Append(str_BarcodeLine).Append(str_NewDate).Append(str_NewBarcodeOrder).ToString()
                    str_NewBarcode.Add(str_TempBarcode)
                    mstr_NewBarcode.Add(str_TempBarcode)

                    obj_TempMergeDate = obj_TempMergeDate.AddSeconds(1)

                    Dim lstr_MergeProdDate As String = obj_TempMergeDate.ToString("yyyy-MM-dd HH:mm:ss")

                    'mstr_SplitProdDateList.Add(lstr_MergeProdDate)

                End If
            Next

        Catch ex As Exception
            BHTController.DisposeScanner(myScanner)
            DisplayMessage.ErrorMsg("Barcode gagal dibuat", "Error")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
        End Try

        Return str_NewBarcode
    End Function

    Private Function fncBarcodeOrderBuild(ByVal str_BarcodeOrder As String) As String
        Dim str_Res As String = String.Empty

        Select Case str_BarcodeOrder.Length
            Case 1
                str_Res = String.Concat("00000", str_BarcodeOrder)
            Case 2
                str_Res = String.Concat("0000", str_BarcodeOrder)
            Case 3
                str_Res = String.Concat("000", str_BarcodeOrder)
            Case 4
                str_Res = String.Concat("00", str_BarcodeOrder)
            Case 5
                str_Res = String.Concat("0", str_BarcodeOrder)
            Case 6
                str_Res = str_BarcodeOrder
        End Select

        Return str_Res
    End Function

#End Region

#Region "Image file name"
    Private Function fncNewImageFile(ByVal str_TRINNo As String) As String
        Dim lstr_NewImageFileName As String = String.Empty
        Dim lstr_CutTRINNo As String = str_TRINNo.Substring(0, 13)

        lstr_NewImageFileName = String.Concat(lstr_CutTRINNo, ".jpg")

        Return lstr_NewImageFileName
    End Function

#End Region

#Region "Split Process"
    Private Function fncProcessSplit() As Boolean
        Dim bool_ProcessStatus As Boolean = False
        Dim bool_Check As Boolean = False
        Dim bool_SplitInsertCheck As Boolean = False
        Dim bool_StockcardInsertCheck As Boolean = False

        Dim str_RemarksRes As String = String.Empty
        Dim str_RemarksBuilder As New System.Text.StringBuilder

        Dim str_ProdActIntUser As String = String.Empty
        Dim str_ProdActIntDateTime As String = String.Empty

        ConfigData = ConfigMgr.GetConfigData()

        mint_SplitFlag = 0
        mint_SplitFlag = ProductionActClassificationDB.fncGetSplitFlag(lstr_Barcode)

        str_ProdActIntUser = ProductionActClassificationDB.fncGetUser(lstr_Barcode)
        str_ProdActIntDateTime = ProductionActClassificationDB.fncGetDateTime(lstr_Barcode)

        str_RemarksBuilder.Append(lstr_Barcode)
        str_RemarksBuilder.Append(";")
        str_RemarksBuilder.Append(lint_ActQty)
        str_RemarksBuilder.Append(";")
        str_RemarksBuilder.Append(str_ProdActIntUser)
        str_RemarksBuilder.Append(";")
        str_RemarksBuilder.Append(str_ProdActIntDateTime)
        str_RemarksBuilder.Append("<BR>")

        str_RemarksRes = str_RemarksBuilder.ToString()

        Dim int_InsertActId As Integer
        Dim int_InsertActIdList As New List(Of Integer)

        Dim str_NewBarcodeList As New List(Of String)
        Dim str_NewBarcodeList2 As New List(Of String)

        str_NewBarcodeList = fncGenerateBarcode(lstr_LineCode, configData.ID)

        Dim str_NewQrList As New List(Of String)
        str_NewQrList = fncQRCodeBuild(mstr_PackQty)

        Dim lint_SplitFinalId As Int32 = ProductionActClassificationDB.fncGetFinalID(lstr_TempQRCode)

        'modify 9i
        'Dim lstr_SplitUserId As String = TemporaryData.loggedInUserID
        Dim lstr_SplitUserId As String = str_ProdActIntUser

        Dim lstr_SplitLineName As String = String.Empty
        lstr_SplitLineName = LineMasterDB.fncGetLineCodes(lstr_LineCode)

        Dim lstr_SplitProdDate As String = mstr_SplitProdDateList(0)

        If mstr_PackQty.Count <> 0 Then


            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim transaction As IDbTransaction = Nothing

                Try

                    connection.Open()
                    transaction = connection.BeginTransaction()


                    For int_Counter As Integer = 0 To str_NewBarcodeList.Count - 1
                        Dim str_TagClassificationData(9) As String
                        Dim str_StockCardClassificationData(7) As String

                        tmr_DbLoad.Enabled = True

                        Dim lstr_SplitBarcodeTag As String = str_NewBarcodeList(int_Counter)

                        Dim lstr_SplitTRINPartNo As String = lbl_TRINPartNoValue.Text
                        Dim lint_SplitQty As Int32 = Convert.ToInt32(mstr_PackQty(int_Counter))

                        Dim lstr_SplitQRCode As String = str_NewQrList(int_Counter)

                        Dim lstr_SplitImageName As String = fncNewImageFile(lstr_SplitTRINPartNo)

                        subCloseNowLoading()

                        'add 9h
                        Dim lstr_BarcodeReset As String = ""
                        lstr_BarcodeReset = fncCreateBarcode(lstr_LineCode, configData.ID, int_Counter)
                        lstr_SplitBarcodeTag = lstr_BarcodeReset

                        'add 9i
                        str_NewBarcodeList2.Add(lstr_SplitBarcodeTag)

                        Dim tempbarcode() As String = Split(lstr_SplitQRCode, ";")
                        Dim lstr_NewQRStringBuilder As New System.Text.StringBuilder
                        lstr_NewQRStringBuilder.Append(tempbarcode(0))
                        lstr_NewQRStringBuilder.Append(";")
                        lstr_NewQRStringBuilder.Append(lstr_BarcodeReset)
                        lstr_NewQRStringBuilder.Append(";")
                        lstr_NewQRStringBuilder.Append(tempbarcode(2))
                        lstr_NewQRStringBuilder.Append(";")
                        lstr_NewQRStringBuilder.Append(tempbarcode(3))
                        lstr_SplitQRCode = lstr_NewQRStringBuilder.ToString

                        lstr_SplitTRINPartNo = lbl_TRINPartNoValue.Text

                        'Insert into PRODUCTIONACT
                        str_TagClassificationData(InsertTagClassificationValues.FinalID) = lint_SplitFinalId.ToString
                        str_TagClassificationData(InsertTagClassificationValues.BarcodeTag) = lstr_SplitBarcodeTag
                        str_TagClassificationData(InsertTagClassificationValues.ProdDate) = lstr_SplitProdDate
                        str_TagClassificationData(InsertTagClassificationValues.TrinPartNo) = lstr_SplitTRINPartNo
                        str_TagClassificationData(InsertTagClassificationValues.LineCode) = lstr_SplitLineName
                        str_TagClassificationData(InsertTagClassificationValues.ActQty) = lint_SplitQty.ToString
                        str_TagClassificationData(InsertTagClassificationValues.UserID) = lstr_SplitUserId
                        str_TagClassificationData(InsertTagClassificationValues.QrCode) = lstr_SplitQRCode
                        str_TagClassificationData(InsertTagClassificationValues.ImgFile) = lstr_SplitImageName
                        str_TagClassificationData(InsertTagClassificationValues.Remarks) = str_RemarksRes

                        tmr_DbLoad.Enabled = True
                        int_InsertActId = ProductionActClassificationDB.fncInsertTagClassificationUseTransaction(str_TagClassificationData, connection)
                        If int_InsertActId <> 0 Then

                            int_InsertActIdList.Add(int_InsertActId)

                            'insert into STOCK_CARD
                            str_StockCardClassificationData(InsertStockCardValues.TrinPartNo) = lstr_SplitTRINPartNo
                            str_StockCardClassificationData(InsertStockCardValues.ActID) = int_InsertActId.ToString
                            str_StockCardClassificationData(InsertStockCardValues.Stock_In) = lint_SplitQty.ToString
                            str_StockCardClassificationData(InsertStockCardValues.Remark) = str_RemarksRes
                            str_StockCardClassificationData(InsertStockCardValues.BarcodeTag) = lstr_SplitBarcodeTag
                            str_StockCardClassificationData(InsertStockCardValues.LineCode) = lstr_SplitLineName
                            str_StockCardClassificationData(InsertStockCardValues.Date_Time) = Now.ToString("yyyy-MM-dd HH:mm:ss")
                            str_StockCardClassificationData(InsertStockCardValues.UserID) = lstr_SplitUserId

                            If Not ProductionActClassificationDB.fncInsertStockCardTagClassificationUseTransaction(str_StockCardClassificationData, connection, transaction) Then
                                ProductionActClassificationDB.DeleteData(int_InsertActId)
                                subCloseNowLoading()
                                lbl_TotalValue.Text = "0"
                                txt_Tag1Value.Text = String.Empty
                                txt_Tag2Value.Text = String.Empty
                                txt_Tag3Value.Text = String.Empty
                                txt_Tag4Value.Text = String.Empty
                                mstr_NewBarcode.Clear()
                                mstr_PackQty.Clear()
                                mstr_NewQR.Clear()
                                mstr_SplitProdDateList.Clear()
                                ReDim str_StockCardClassificationData(-1)
                                ReDim str_TagClassificationData(-1)
                                Exit For
                            Else
                                bool_StockcardInsertCheck = True
                            End If
                            bool_SplitInsertCheck = True
                        Else
                            subCloseNowLoading()
                            lbl_TotalValue.Text = "0"
                            txt_Tag1Value.Text = String.Empty
                            txt_Tag2Value.Text = String.Empty
                            txt_Tag3Value.Text = String.Empty
                            txt_Tag4Value.Text = String.Empty
                            mstr_NewBarcode.Clear()
                            mstr_PackQty.Clear()
                            mstr_NewQR.Clear()
                            mstr_SplitProdDateList.Clear()
                            ReDim str_StockCardClassificationData(-1)
                            ReDim str_TagClassificationData(-1)
                            Exit For
                        End If
                        subCloseNowLoading()
                        ReDim str_StockCardClassificationData(-1)
                        ReDim str_TagClassificationData(-1)
                    Next


                    transaction.Commit()

                Catch ex As Exception
                    transaction.Rollback()
                End Try
            End Using

            'For int_Counter As Integer = 0 To str_NewBarcodeList.Count - 1
            '    Dim str_TagClassificationData(9) As String
            '    Dim str_StockCardClassificationData(7) As String

            '    tmr_DbLoad.Enabled = True

            '    Dim lstr_SplitBarcodeTag As String = str_NewBarcodeList(int_Counter)

            '    Dim lstr_SplitTRINPartNo As String = lbl_TRINPartNoValue.Text
            '    Dim lint_SplitQty As Int32 = Convert.ToInt32(mstr_PackQty(int_Counter))

            '    Dim lstr_SplitQRCode As String = str_NewQrList(int_Counter)

            '    Dim lstr_SplitImageName As String = fncNewImageFile(lstr_SplitTRINPartNo)

            '    subCloseNowLoading()

            '    'add 9h
            '    Dim lstr_BarcodeReset As String = ""
            '    lstr_BarcodeReset = fncCreateBarcode(lstr_LineCode, configData.ID)
            '    lstr_SplitBarcodeTag = lstr_BarcodeReset

            '    'add 9i
            '    str_NewBarcodeList2.Add(lstr_SplitBarcodeTag)

            '    Dim tempbarcode() As String = Split(lstr_SplitQRCode, ";")
            '    Dim lstr_NewQRStringBuilder As New System.Text.StringBuilder
            '    lstr_NewQRStringBuilder.Append(tempbarcode(0))
            '    lstr_NewQRStringBuilder.Append(";")
            '    lstr_NewQRStringBuilder.Append(lstr_BarcodeReset)
            '    lstr_NewQRStringBuilder.Append(";")
            '    lstr_NewQRStringBuilder.Append(tempbarcode(2))
            '    lstr_NewQRStringBuilder.Append(";")
            '    lstr_NewQRStringBuilder.Append(tempbarcode(3))
            '    lstr_SplitQRCode = lstr_NewQRStringBuilder.ToString

            '    lstr_SplitTRINPartNo = lbl_TRINPartNoValue.Text

            '    'Insert into PRODUCTIONACT
            '    str_TagClassificationData(InsertTagClassificationValues.FinalID) = lint_SplitFinalId.ToString
            '    str_TagClassificationData(InsertTagClassificationValues.BarcodeTag) = lstr_SplitBarcodeTag
            '    str_TagClassificationData(InsertTagClassificationValues.ProdDate) = lstr_SplitProdDate
            '    str_TagClassificationData(InsertTagClassificationValues.TrinPartNo) = lstr_SplitTRINPartNo
            '    str_TagClassificationData(InsertTagClassificationValues.LineCode) = lstr_SplitLineName
            '    str_TagClassificationData(InsertTagClassificationValues.ActQty) = lint_SplitQty.ToString
            '    str_TagClassificationData(InsertTagClassificationValues.UserID) = lstr_SplitUserId
            '    str_TagClassificationData(InsertTagClassificationValues.QrCode) = lstr_SplitQRCode
            '    str_TagClassificationData(InsertTagClassificationValues.ImgFile) = lstr_SplitImageName
            '    str_TagClassificationData(InsertTagClassificationValues.Remarks) = str_RemarksRes

            '    tmr_DbLoad.Enabled = True
            '    int_InsertActId = ProductionActClassificationDB.fncInsertTagClassification(str_TagClassificationData)
            '    If int_InsertActId <> 0 Then

            '        int_InsertActIdList.Add(int_InsertActId)

            '        'insert into STOCK_CARD
            '        str_StockCardClassificationData(InsertStockCardValues.TrinPartNo) = lstr_SplitTRINPartNo
            '        str_StockCardClassificationData(InsertStockCardValues.ActID) = int_InsertActId.ToString
            '        str_StockCardClassificationData(InsertStockCardValues.Stock_In) = lint_SplitQty.ToString
            '        str_StockCardClassificationData(InsertStockCardValues.Remark) = str_RemarksRes
            '        str_StockCardClassificationData(InsertStockCardValues.BarcodeTag) = lstr_SplitBarcodeTag
            '        str_StockCardClassificationData(InsertStockCardValues.LineCode) = lstr_SplitLineName
            '        str_StockCardClassificationData(InsertStockCardValues.Date_Time) = Now.ToString("yyyy-MM-dd HH:mm:ss")
            '        str_StockCardClassificationData(InsertStockCardValues.UserID) = lstr_SplitUserId

            '        If Not ProductionActClassificationDB.fncInsertStockCardTagClassification(str_StockCardClassificationData) Then
            '            ProductionActClassificationDB.DeleteData(int_InsertActId)
            '            subCloseNowLoading()
            '            lbl_TotalValue.Text = "0"
            '            txt_Tag1Value.Text = String.Empty
            '            txt_Tag2Value.Text = String.Empty
            '            txt_Tag3Value.Text = String.Empty
            '            txt_Tag4Value.Text = String.Empty
            '            mstr_NewBarcode.Clear()
            '            mstr_PackQty.Clear()
            '            mstr_NewQR.Clear()
            '            mstr_SplitProdDateList.Clear()
            '            ReDim str_StockCardClassificationData(-1)
            '            ReDim str_TagClassificationData(-1)
            '            Exit For
            '        Else
            '            bool_StockcardInsertCheck = True
            '        End If
            '        bool_SplitInsertCheck = True
            '    Else
            '        subCloseNowLoading()
            '        lbl_TotalValue.Text = "0"
            '        txt_Tag1Value.Text = String.Empty
            '        txt_Tag2Value.Text = String.Empty
            '        txt_Tag3Value.Text = String.Empty
            '        txt_Tag4Value.Text = String.Empty
            '        mstr_NewBarcode.Clear()
            '        mstr_PackQty.Clear()
            '        mstr_NewQR.Clear()
            '        mstr_SplitProdDateList.Clear()
            '        ReDim str_StockCardClassificationData(-1)
            '        ReDim str_TagClassificationData(-1)
            '        Exit For
            '    End If
            '    subCloseNowLoading()
            '    ReDim str_StockCardClassificationData(-1)
            '    ReDim str_TagClassificationData(-1)
            'Next

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim transaction As IDbTransaction = Nothing

                Try
                    connection.Open()
                    transaction = connection.BeginTransaction()

                    For int_RollBack = 0 To int_InsertActIdList.Count - 1
                        If ProductionActIntegrationDB.fncProdactTagIntegrationNewtag _
                       (int_InsertActIdList(int_RollBack), connection, transaction) Then

                            If ProductionActIntegrationDB.fncUpdateStockCardTagIntegrationNewtag _
                                (int_InsertActIdList(int_RollBack), 5, connection, transaction) Then

                            End If

                        End If
                    Next

                    If bool_SplitInsertCheck And bool_StockcardInsertCheck Then
                        If ProductionActClassificationDB.fncUpdateOldTagUseTransaction(lstr_Barcode, mint_SplitFlag, connection, transaction) Then
                            If ProductionActClassificationDB.fncUpdateStockCardTagClassificationUseTransaction(lstr_Barcode, connection, transaction) Then
                                bool_ProcessStatus = True
                                bool_Check = True
                                subCloseNowLoading()
                            End If
                        End If
                    End If

                    'If bool_ProcessStatus = False Then
                    '    For int_RollBack = 0 To int_InsertActIdList.Count - 1
                    '        ProductionActIntegrationDB.DeleteData(int_InsertActIdList(int_RollBack), str_NewBarcodeList2(int_RollBack))
                    '        ProductionActClassificationDB.DeleteDataStockCard(int_InsertActIdList(int_RollBack))
                    '        ProductionActClassificationDB.fncUpdateReturnOldTag(lstr_Barcode, mint_SplitFlag)
                    '    Next
                    '    bool_Check = False
                    'End If

                    transaction.Commit()
                Catch ex As Exception
                    transaction.Rollback()
                End Try
            End Using

        End If
        Return bool_Check
    End Function

    Private Sub subSplitHandler()
        Try
            If RF.SYNCHRONIZE(RF.SYNC_CHECK) <> 0 Then
                BHTController.DisposeScanner(myScanner)
                DisplayMessage.ErrorMsg("Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
                    "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.", "Error")
                BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                Exit Sub
            End If
        Catch ex As Exception
            If Err.Number = 5 Then
                BHTController.DisposeScanner(myScanner)
                DisplayMessage.ErrorMsg("Koneksi Wifi di HT tertutup." & vbCrLf & _
                    "Tunggu beberapa detik dan ulangi lagi.", "Error")
                Dim MyRf As RF
                MyRf = New RF()
                MyRf.OpenMode = RF.EN_OPEN_MODE.CONTINUOUSLY
                MyRf.Open = True
                BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                Exit Sub
            End If
        End Try
        If mstr_PackQty.Count = 0 Then
            BHTController.DisposeScanner(myScanner)
            DisplayMessage.ErrorMsg("Masukkan Split Qty dan coba sekali lagi", "Error")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
        Else
            tmr_DbLoad.Enabled = True

            If Not fncProcessSplit() Then
                BHTController.DisposeScanner(myScanner)
                DisplayMessage.ErrorMsg("Split error", "Error")
                BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
            Else
                BHTController.DisposeScanner(myScanner)
                DisplayMessage.OkMsg("ID Tag telah berhasil dipilah", "Sukses")
                BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)

                lbl_TotalValue.Text = "0"
                txt_ScanTag.Text = String.Empty
                txt_Tag1Value.Text = String.Empty
                txt_Tag2Value.Text = String.Empty
                txt_Tag3Value.Text = String.Empty
                txt_Tag4Value.Text = String.Empty

                '************************************************************
                'New edit for clear scanned QR after successful split process
                lstr_TempQRCode = String.Empty
                '************************************************************

                mstr_NewBarcode.Clear()
                mstr_PackQty.Clear()
                mstr_NewQR.Clear()
                mstr_SplitProdDateList.Clear()

                subInitForm(False)
            End If

            subCloseNowLoading()
        End If
    End Sub

#End Region

End Class
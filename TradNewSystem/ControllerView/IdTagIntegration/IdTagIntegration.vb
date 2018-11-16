Option Strict On
Option Explicit On

Imports TradNewSystem.Helpers
Imports TradNewSystem.Model
Imports TradNewSystem.PocoClass

Imports System.Runtime.InteropServices
Imports System.Globalization
Imports System.Data

Imports DNWA.BHTCL
Imports DataGridCustomColumns
Imports MySql.Data.MySqlClient
Imports log4net

Public Class frm_IdTagIntegration

#Region "Declaration"

    Private lstr_LineCode As String

    Friend WithEvents myScanner As Scanner

    Protected Friend hdr_NowLoading As NowLoading
    Protected Friend ds_Datasource As DataSet
    Protected Friend dt_BarcodeTag As DataTable

    Private pobj_BindingSource As New BindingSource

    Private scn_ScannedBarcodeList As New List(Of String)

    Private int_DbLoadDuration As Integer
    Private processKeypadPress As Boolean = True

    Private mstr_LastCellValue As String = String.Empty
    Private mstr_TempRemarkData As New List(Of String)
    Private mstr_ScannedBarcode As New List(Of String)
    Private mstr_NewBarcode As New List(Of String)
    Private mstr_NewQR As New List(Of String)
    Private mstr_BarcodeLineList As New List(Of String)
    Private mstr_MergeProdDateList As New List(Of String)
    Private mint_PackQtyMod As New List(Of Integer)
    Private marr_PackQtyList As New ArrayList

    Private mdate_CurrDate As Date = Date.Now

    Private lint_StdQty As Int16 = 0
    Private lint_PackQty As Int32
    Private lint_TempActQty As New List(Of Integer)
    Private str_IntegrationBarcodeArr As New List(Of String)
    Private str_StockCardIntegrationBarcodeArr As New List(Of String)
    Private int_DataGridColumnSizes() As Integer

    Private fnt_headerFont As Font

    Private lstr_BarcodeLine As String = String.Empty
    Private lstr_Barcode As String = String.Empty
    Private lint_ProdQty As Int32
    Private lstr_TRINNo As String = String.Empty

    Private lstr_TempTRINPartNo As String = String.Empty
    Private lstr_TempQRCode As String = String.Empty
    Private lstr_TempDateNew As String = String.Empty

    Private mint_SplitFlag As Int16
    Private lint_SplitFlagList As New List(Of Int16)

    Private mint_RevertSplitFlag As Int16

    Private cint_TestVal As Int32 = 3

    Private configData As ConfigMgr.ConfigData

    'add 9i
    Private lstr_UserID As String = String.Empty

    Friend Enum DataGridColumns
        BarcodeTag
        PackedQty
    End Enum

    Private Enum QrCodeValues
        BarcodeLine
        BarcodeTag
        ActQty
        TrinPartCode
    End Enum

    Private Enum InsertTagIntegrationValues
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

    Private Sub frm_IdTagIntegration_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        BHTController.DisposeScanner(myScanner)
    End Sub

    Private Sub frm_IdTagIntegration_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)

        ds_Datasource = New DataSet

        txt_ScanTag.Focus()
        subInitForm(False)
    End Sub

#End Region

#Region "Fill GridView Data"
    Private Sub subSetBarcodeTagData()
        Dim int_PackQtyRes As Int32 = 0

        tmr_DbLoad.Enabled = True

        Dim obj_BarcodeList As List(Of ProductionActIntegration) = ProductionActIntegrationDB.fncGetBarcodeValLine(lstr_TempQRCode)

        subCloseNowLoading()

        fnt_headerFont = Me.fncGetHeaderFont(dgv_ScanData.Font)

        If dt_BarcodeTag Is Nothing Then
            dt_BarcodeTag = New DataTable("BarcodeTag")
            dt_BarcodeTag.Columns.Add("Barcode Tag", Type.GetType("System.String"))
            dt_BarcodeTag.Columns.Add("Packed Qty", Type.GetType("System.Int32"))
        End If

        int_DataGridColumnSizes = New Integer(1) {}

        If Not obj_BarcodeList Is Nothing Then
            Dim dr_DataRow As DataRow = Nothing

            For Each obj_BarcodeListMembers As ProductionActIntegration In obj_BarcodeList
                dr_DataRow = dt_BarcodeTag.NewRow

                dr_DataRow(DataGridColumns.BarcodeTag) = obj_BarcodeListMembers.BARCODETAG
                dr_DataRow(DataGridColumns.PackedQty) = obj_BarcodeListMembers.ACTQTY

                dt_BarcodeTag.Rows.Add(dr_DataRow)
            Next
            dr_DataRow = dt_BarcodeTag.NewRow
        End If

        subSetDataGridColumnSize("Barcode Tag", 0)
        subSetDataGridColumnSize("Packed Qty", 1)

        pobj_BindingSource.DataSource = dt_BarcodeTag

        dgv_ScanData.DataSource = pobj_BindingSource

        subResizeColumnWidth(dt_BarcodeTag)

        lbl_TotalTagValue.Text = marr_PackQtyList.Count.ToString

        lbl_TotalPackedQtyValue.Text = lint_PackQty.ToString

        Me.dgv_ScanData.TabIndex = 1
        Me.dgv_ScanData.Focus()
    End Sub

    Private Sub subClearDataTable()
        Dim dr_DataRow As DataRow
        dt_BarcodeTag.Clear()
        dr_DataRow = dt_BarcodeTag.NewRow
        dgv_ScanData.DataSource = Nothing
        pobj_BindingSource.Clear()
    End Sub

#End Region

#Region "DataGrid Layout"
    Private Function fncMeasureTextWidth(ByVal str_Text As String, ByVal obj_Font As Font) As Integer
        Dim lobj_Instance As Graphics = dgv_ScanData.CreateGraphics

        Return CInt(lobj_Instance.MeasureString(str_Text, obj_Font).Width)
    End Function

    Private Function fncGetHeaderFont(ByVal obj_Font As Font) As Font
        Dim str_FontName As String = obj_Font.Name
        Dim int_FontSize As Single = obj_Font.Size

        Return New Font(str_FontName, int_FontSize, FontStyle.Bold)
    End Function

    Private Sub subSetDataGridColumnSize(ByVal str_Text As String, ByVal int_Index As Integer)
        If int_DataGridColumnSizes Is Nothing Or int_DataGridColumnSizes.Length - 1 < int_Index Then Exit Sub

        Dim int_TextWidth As Integer = fncMeasureTextWidth(str_Text, fnt_headerFont)

        If int_DataGridColumnSizes(int_Index) < int_TextWidth + 58 Then
            int_DataGridColumnSizes(int_Index) = int_TextWidth + 58
        End If
    End Sub

    Private Sub subResizeColumnWidth(ByVal dt_Datatable As DataTable)
        dgv_ScanData.TableStyles.Clear()

        Dim obj_TableStyle As DataGridTableStyle = New DataGridTableStyle

        obj_TableStyle.MappingName = dt_Datatable.TableName

        For Each dt_Items As DataColumn In dt_Datatable.Columns
            Dim obj_TextboxColumn As DataGridTextBoxColumn = New DataGridTextBoxColumn
            Dim str_ColumnName As String = dt_Items.ColumnName

            obj_TextboxColumn.Width = int_DataGridColumnSizes(dt_Items.Ordinal)

            obj_TextboxColumn.MappingName = dt_Items.ColumnName
            obj_TextboxColumn.HeaderText = dt_Items.ColumnName

            obj_TableStyle.GridColumnStyles.Add(obj_TextboxColumn)
        Next
        dgv_ScanData.TableStyles.Add(obj_TableStyle)

    End Sub

#End Region

#Region "Now Loading Handler"

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
        btn_Merge.Enabled = _bool
        btn_Back.Enabled = _bool
        processKeypadPress = _bool
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

#End Region

#Region "Scanner Process"
    Private Sub myScanner_OnDone(ByVal sender As Object, ByVal e As System.EventArgs) Handles myScanner.OnDone
        Try
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")
            Try
                If RF.SYNCHRONIZE(RF.SYNC_CHECK) <> 0 Then
                    BHTController.DisposeScanner(myScanner)
                    log.Info("Start Info WifiConectionCheck Signal Distance Id Tag Integration : Scanner Process")
                    log.Info("Additional Message: Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
                        "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.")
                    log.Info("End Info WifiConectionCheck Signal Distance Id Tag Integration : Scanner Process")

                    DisplayMessage.ErrorMsg("Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
                        "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.", "Error")
                    BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                    Exit Sub
                End If
            Catch ex As Exception
                log.Error("Start Error WifiConectionCheck Id Tag Integration : Scanner Process")
                If Err.Number = 5 Then
                    BHTController.DisposeScanner(myScanner)
                    log.Error("Additional Message: Koneksi Wifi di HT tertutup." & vbCrLf & _
                    "Tunggu beberapa detik dan ulangi lagi.")
                    log.Error("Error Number: " & Err.Number & vbCrLf & "Error Description: " & Err.Description & vbCrLf, ex)
                    log.Error("End Error WifiConectionCheck Id Tag Integration : Scanner Process")

                    DisplayMessage.ErrorMsg("Koneksi Wifi di HT tertutup." & vbCrLf & _
                        "Tunggu beberapa detik dan ulangi lagi.", "Error")
                    Dim MyRf As RF
                    MyRf = New RF()
                    MyRf.OpenMode = RF.EN_OPEN_MODE.CONTINUOUSLY
                    MyRf.Open = True
                    BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                    Exit Sub
                End If
                log.Error("Error Number: " & Err.Number & vbCrLf & "Error Description: " & Err.Description & vbCrLf, ex)
                log.Error("End Error WifiConectionCheck Id Tag Integration : Scanner Process")
            End Try
            LogManager.Shutdown()

            BHTController.SoundOK() ' Febry
            Cursor.Current = Cursors.WaitCursor

            Dim scn_ScanCode = myScanner.Input(Scanner.ALL_BUFFER)

            Dim str_QrItems As String()

            tmr_DbLoad.Enabled = True
            Dim QrValid As QueryRetValue = ProductionActDB.fncCheckQR(scn_ScanCode)



            'modify 9.i
            'Dim productionAct As ProductionAct = ProductionActDB.GetProdDateLineCodeUserID(scn_ScanCode, True) Test Disable by Febry
            Dim productionAct As ProductionAct = ProductionActDB.GetProdDateLineCodeUserID(scn_ScanCode)

            If Not productionAct Is Nothing Then
                lstr_UserID = productionAct.userid
            End If


            subCloseNowLoading()

            If QrValid <> QueryRetValue.ValueTrue Then
                BHTController.DisposeScanner(myScanner)

                Cursor.Current = Cursors.Default

                DisplayMessage.ErrorMsg("QR code tidak valid", "Error")
                BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                Exit Sub
            End If

            str_QrItems = scn_ScanCode.Split(New Char() {";"c})

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

                    Cursor.Current = Cursors.Default

                    DisplayMessage.ErrorMsg(exceptionMsg, "DB Error")
                    BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                    Exit Sub

                Else
                    Dim errorMsg As String = String.Format( _
                        "Tidak Terdapat Sisa Stock Untuk QR Code {0} di Sistem", _
                        str_QrItems(QrCodeValues.BarcodeTag) _
                        )

                    BHTController.DisposeScanner(myScanner)

                    Cursor.Current = Cursors.Default

                    DisplayMessage.ErrorMsg(errorMsg, "Error")
                    BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                    Exit Sub
                End If

            End If

            If scn_ScannedBarcodeList.Count = 0 Then
                lstr_TempQRCode = scn_ScanCode
                scn_ScannedBarcodeList.Add(lstr_TempQRCode)
            Else
                If scn_ScannedBarcodeList.Contains(scn_ScanCode) Then
                    BHTController.DisposeScanner(myScanner)

                    Cursor.Current = Cursors.Default

                    DisplayMessage.ErrorMsg("Silahkan scan QR Code yang berbeda", "Error")
                    BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                    Exit Sub
                Else
                    lstr_TempQRCode = scn_ScanCode
                    scn_ScannedBarcodeList.Add(lstr_TempQRCode)
                End If
            End If




            If Not String.IsNullOrEmpty(lstr_BarcodeLine) Then
                If str_QrItems(QrCodeValues.TrinPartCode) <> lstr_TempTRINPartNo Then
                    BHTController.DisposeScanner(myScanner)

                    Cursor.Current = Cursors.Default

                    DisplayMessage.ErrorMsg("TrinPartNo tidak sama", "Error")
                    BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                Else
                    'BHTController.SoundOK()
                    Cursor.Current = Cursors.Default 'febry

                    mint_SplitFlag = ProductionActIntegrationDB.fncGetSplitFlag(str_QrItems(QrCodeValues.BarcodeTag))
                    lint_SplitFlagList.Add(mint_SplitFlag)

                    lstr_BarcodeLine = lstr_LineCode

                    txt_ScanTag.Text = str_QrItems(QrCodeValues.BarcodeTag)

                    lint_ProdQty = Convert.ToInt32(str_QrItems(QrCodeValues.ActQty))
                    marr_PackQtyList.Add(lint_ProdQty)

                    lstr_TempTRINPartNo = str_QrItems(QrCodeValues.TrinPartCode)

                    lint_StdQty = ProductionActIntegrationDB.fncGetStdQty(lstr_TempTRINPartNo)
                    lbl_StdQtyValue.Text = lint_StdQty.ToString

                    lbl_ActualQtyValue.Text = lint_ProdQty.ToString
                    lstr_Barcode = txt_ScanTag.Text

                    mstr_ScannedBarcode.Add(lstr_Barcode)
                    mstr_BarcodeLineList.Add(lstr_BarcodeLine)
                    mstr_MergeProdDateList.Add(productionAct.PRODDATE.ToString("yyyy-MM-dd HH:mm:ss"))

                    subQrCheck(scn_ScanCode)
                    BHTController.SoundOK()
                End If
            Else
                'BHTController.SoundOK()
                Cursor.Current = Cursors.Default

                mint_SplitFlag = ProductionActIntegrationDB.fncGetSplitFlag(str_QrItems(QrCodeValues.BarcodeTag))
                lint_SplitFlagList.Add(mint_SplitFlag)

                lstr_BarcodeLine = lstr_LineCode

                txt_ScanTag.Text = str_QrItems(QrCodeValues.BarcodeTag)

                lint_ProdQty = Convert.ToInt32(str_QrItems(QrCodeValues.ActQty))
                marr_PackQtyList.Add(lint_ProdQty)

                lstr_TempTRINPartNo = str_QrItems(QrCodeValues.TrinPartCode)

                lint_StdQty = ProductionActIntegrationDB.fncGetStdQty(lstr_TempTRINPartNo)
                lbl_StdQtyValue.Text = lint_StdQty.ToString

                lbl_ActualQtyValue.Text = lint_ProdQty.ToString
                lstr_Barcode = txt_ScanTag.Text

                mstr_ScannedBarcode.Add(lstr_Barcode)
                mstr_BarcodeLineList.Add(lstr_BarcodeLine)
                mstr_MergeProdDateList.Add(productionAct.PRODDATE.ToString("yyyy-MM-dd HH:mm:ss"))



                subQrCheck(scn_ScanCode) 'show data grid
                BHTController.SoundOK()
            End If

        Catch ex As Exception
            BHTController.DisposeScanner(myScanner)

            Cursor.Current = Cursors.Default

            DisplayMessage.ErrorMsg(ex.Message, "Error")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
        End Try
    End Sub

#End Region

#Region "Click Handler"
    Private Sub btn_FullScreen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_FullScreen.Click
        ToggleTaskbar.EnableDisable()
    End Sub

    Private Sub btn_Merge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Merge.Click
        log4net.Config.XmlConfigurator.Configure()
        Dim log As ILog = LogManager.GetLogger("TRADLogger")
        Try
            If RF.SYNCHRONIZE(RF.SYNC_CHECK) <> 0 Then
                BHTController.DisposeScanner(myScanner)
                log.Info("Start Info WifiConectionCheck Signal Distance Id Tag Integration : btn_Merge_Click")
                log.Info("Additional Message: Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
                    "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.")
                log.Info("End Info WifiConectionCheck Signal Distance Tag Integration : btn_Merge_Click")

                DisplayMessage.ErrorMsg("Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
                    "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.", "Error")
                BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                Exit Sub
            End If

        Catch ex As Exception
            log.Error("Start Error WifiConectionCheck Id Tag Integration : btn_Merge_Click")
            If Err.Number = 5 Then
                BHTController.DisposeScanner(myScanner)
                log.Error("Additional Message: Koneksi Wifi di HT tertutup." & vbCrLf & _
                    "Tunggu beberapa detik dan ulangi lagi.")
                log.Error("Error Number: " & Err.Number & vbCrLf & "Error Description: " & Err.Description & vbCrLf, ex)
                log.Error("End Error WifiConectionCheck Id Tag Integration : btn_Merge_Click")

                DisplayMessage.ErrorMsg("Koneksi Wifi di HT tertutup." & vbCrLf & _
                    "Tunggu beberapa detik dan ulangi lagi.", "Error")
                BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                Dim MyRf As RF
                MyRf = New RF()
                MyRf.OpenMode = RF.EN_OPEN_MODE.CONTINUOUSLY
                MyRf.Open = True
                BHTController.DisposeScanner(myScanner)
                Exit Sub
            End If
            log.Error("Error Number: " & Err.Number & vbCrLf & "Error Description: " & Err.Description & vbCrLf, ex)
            log.Error("End Error WifiConectionCheck Id Tag Integration : btn_Merge_Click")
        End Try
        LogManager.Shutdown()

        If Convert.ToInt32(lbl_TotalPackedQtyValue.Text) = 0 And marr_PackQtyList.Count = 0 Then
            BHTController.DisposeScanner(myScanner)
            DisplayMessage.ErrorMsg("Silahkan Masukkan Pack Qty baru", "Error")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
        Else
            If marr_PackQtyList.Count = 0 Then
                BHTController.DisposeScanner(myScanner)
                DisplayMessage.ErrorMsg("Silahkan isi Qty baru", "Error")
                BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
            Else
                If DisplayMessage.ConfirmationDialog("Apakah ingin menggabungkan tag?", "Konfirmasi") = True Then
                    subMergeHandler() ' send print service
                End If
            End If
        End If
    End Sub

    Private Sub btn_Back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Back.Click
        subClose()
    End Sub

#End Region

#Region "Keypad Press"
    Private Sub KeypressShortcut(ByVal e As System.Windows.Forms.KeyEventArgs)
        If processKeypadPress Then
            Select Case e.KeyCode
                Case Windows.Forms.Keys.F1
                    If Convert.ToInt32(lbl_TotalPackedQtyValue.Text) = 0 And marr_PackQtyList.Count = 0 Then
                        BHTController.DisposeScanner(myScanner)
                        DisplayMessage.ErrorMsg("Silahkan Masukkan Pack Qty baru", "Error")
                        BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                    Else
                        If marr_PackQtyList.Count = 0 Then
                            BHTController.DisposeScanner(myScanner)
                            DisplayMessage.ErrorMsg("Silahkan isi Qty baru", "Error")
                            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                        Else
                            If DisplayMessage.ConfirmationDialog("Apakah ingin menggabungkan tag?", "Konfirmasi") = True Then
                                subMergeHandler()
                            End If
                        End If
                    End If
                Case Windows.Forms.Keys.F2
                    subClose()
            End Select
        End If
    End Sub

    Private Sub frm_IdTagIntegration_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        KeypressShortcut(e)
    End Sub

    Private Sub dgv_ScanData_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        KeypressShortcut(e)
    End Sub

    Private Sub btn_FullScreen_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btn_FullScreen.KeyUp
        KeypressShortcut(e)
    End Sub

    Private Sub txt_ScanTag_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ScanTag.KeyUp
        KeypressShortcut(e)
    End Sub

    Private Sub btn_Merge_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btn_Merge.KeyUp
        KeypressShortcut(e)
    End Sub

    Private Sub btn_Back_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btn_Back.KeyUp
        KeypressShortcut(e)
    End Sub

#End Region

#Region "Function"
    'New Function 9H
    Private Function fncCreateBarcode(ByVal str_BarcodeLine As String, ByVal str_ID As String, ByVal CountNewBarcodetag As Int32) As String
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

        str_BarcodeLastOrder = ProductionActIntegrationDB.fncGetLastBarcodeCountValue(lstr_TempDateNew, lstr_BarcodeLine, str_ID)
        int_Order = 1
        If Not String.IsNullOrEmpty(str_BarcodeLastOrder) Then
            int_Order = Convert.ToInt32(str_BarcodeLastOrder) + 1
        End If

        int_Order = int_Order + Convert.ToInt32(CountNewBarcodetag)

        Dim lobj_BarcodeBuilder As New System.Text.StringBuilder()
        str_NewBarcode = str_ID & Microsoft.VisualBasic.Right("0000" & int_Order, 5)
        Return lobj_BarcodeBuilder.Append(str_BarcodeLine).Append(str_NewDate).Append(str_NewBarcode).ToString()
    End Function

    Private Sub subInitForm(ByVal _bool As Boolean)
        Me.lbl_TRINPartNoTitle.Visible = _bool
        Me.lbl_TRINPartNoValue.Visible = _bool
        Me.lbl_ActualQtyTitle.Visible = _bool
        Me.lbl_ActualQtyValue.Visible = _bool
        Me.lbl_StdQtyTitle.Visible = _bool
        Me.lbl_StdQtyValue.Visible = _bool
        Me.lbl_TotalTagTitle.Visible = _bool
        Me.lbl_TotalTagValue.Visible = _bool
        Me.lbl_TotalPackedQtyTitle.Visible = _bool
        Me.lbl_TotalPackedQtyValue.Visible = _bool
        Me.dgv_ScanData.Visible = _bool
        Me.dgv_ScanData.Enabled = _bool
        Me.btn_Merge.Visible = _bool
        Me.btn_Merge.Enabled = _bool
    End Sub

    Private Sub subClose()
        BHTController.DisposeScanner(myScanner)
        Dim obj_Confirm As Boolean = DisplayMessage.ConfirmationDialog("Kembali ke menu utama?", "Konfirmasi")
        BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)

        If obj_Confirm = True Then
            Close()
        End If
    End Sub

    Private Sub subQrCheck(ByVal str_ProdQr As String)
        tmr_DbLoad.Enabled = True

        Dim int_QrStatus As Integer = ProductionActDB.fncCheckQR(str_ProdQr)

        Dim str_TRINPartNo As String = ProductionActDB.fncGetTRINPartNo(str_ProdQr)

        subCloseNowLoading()

        lint_PackQty += lint_ProdQty

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
                            lstr_TRINNo = str_TRINPartNo
                            subSetBarcodeTagData()

                            subInitForm(True)
                        End If
                    Else
                        Me.lbl_TRINPartNoValue.Text = str_TRINPartNo
                        lstr_TRINNo = str_TRINPartNo
                        subSetBarcodeTagData()

                        subInitForm(True)
                    End If
                End If
            End If
        End If
    End Sub

#End Region

#Region "New QR Code Handler"
    Private Function fncQRCodeBuild(ByVal int_PackQtyUpd As List(Of Integer)) As List(Of String)
        Dim str_QRCreate As New List(Of String)

        Dim lstr_NewQRLineCode As String = lstr_BarcodeLine

        Dim lstr_NewQRBarcodeList As New List(Of String)
        lstr_NewQRBarcodeList = mstr_NewBarcode

        Dim lstr_NewQRBarcode As String = String.Empty

        Dim lstr_NewQRProdQty As String = String.Empty
        Dim lstr_NewQRTRINPartNo As String = lbl_TRINPartNoValue.Text

        For int_Counter As Integer = 0 To lstr_NewQRBarcodeList.Count - 1
            lstr_NewQRBarcode = lstr_NewQRBarcodeList(int_Counter)
            lstr_NewQRProdQty = mint_PackQtyMod(int_Counter).ToString

            Dim lstr_NewQRStringBuilder As New System.Text.StringBuilder

            lstr_NewQRStringBuilder.Append(lstr_NewQRLineCode)
            lstr_NewQRStringBuilder.Append(";")
            lstr_NewQRStringBuilder.Append(lstr_NewQRBarcode)
            lstr_NewQRStringBuilder.Append(";")
            lstr_NewQRStringBuilder.Append(lstr_NewQRProdQty)
            lstr_NewQRStringBuilder.Append(";")
            lstr_NewQRStringBuilder.Append(lstr_NewQRTRINPartNo)

            str_QRCreate.Add(lstr_NewQRStringBuilder.ToString)
        Next
        mstr_NewQR = str_QRCreate
        Return str_QRCreate
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
            Dim obj_LastBarcodeCount As String = ProductionActIntegrationDB.fncGetLastBarcodeCountValue(str_Date, lstr_BarcodeLine, str_ID)
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
        Dim int_QtyLeft As Integer
        Dim int_Order As Int32 = 0

        Dim obj_TempMergeDate As Date = mdate_CurrDate

        tmr_DbLoad.Enabled = True
        Dim lstr_Date As Date = ProductionActIntegrationDB.fncGetBusinessDay()
        subCloseNowLoading()

        Dim str_NewDate As String = lstr_Date.ToString("yyyy/MM/dd")
        str_NewDate = str_NewDate.Replace("/", "")

        lstr_TempDateNew = String.Concat("%", str_NewDate, "%")

        str_BarcodeLastOrder = ProductionActIntegrationDB.fncGetLastBarcodeCountValue(lstr_TempDateNew, lstr_BarcodeLine, str_ID)

        If Not String.IsNullOrEmpty(str_BarcodeLastOrder) Then
            int_Order = Convert.ToInt32(str_BarcodeLastOrder)
        End If

        Try
            If fncCheckLastBarcodeOrder(str_NewDate, str_ID) Then
                If lint_PackQty <= lint_StdQty Then
                    mint_PackQtyMod.Add(lint_PackQty)

                    int_Order += 1

                    Dim str_ModifiedOrder As String = int_Order.ToString()

                    Dim str_NewBarcodeOrder As String = fncBarcodeOrderBuild(str_ModifiedOrder, str_ID)

                    Dim lobj_BarcodeBuilder As New System.Text.StringBuilder()
                    str_TempBarcode = lobj_BarcodeBuilder.Append(str_BarcodeLine).Append(str_NewDate).Append(str_NewBarcodeOrder).ToString()
                    str_NewBarcode.Add(str_TempBarcode)
                    mstr_NewBarcode.Add(str_TempBarcode)

                    obj_TempMergeDate = obj_TempMergeDate.AddSeconds(1)

                    Dim lstr_MergeProdDate As String = obj_TempMergeDate.ToString("yyyy-MM-dd HH:mm:ss")

                    'mstr_MergeProdDateList.Add(lstr_MergeProdDate)

                Else
                    int_QtyLeft = lint_PackQty

                    While int_QtyLeft > 0
                        Dim int_TmpQty As Integer = 0

                        If int_QtyLeft > lint_StdQty Then
                            int_TmpQty = lint_StdQty
                        Else
                            int_TmpQty = int_QtyLeft
                        End If

                        mint_PackQtyMod.Add(int_TmpQty)

                        int_Order += 1

                        Dim str_ModifiedOrder As String = int_Order.ToString()

                        Dim str_NewBarcodeOrder As String = fncBarcodeOrderBuild(str_ModifiedOrder, str_ID)

                        Dim lobj_BarcodeBuilder As New System.Text.StringBuilder()
                        str_TempBarcode = lobj_BarcodeBuilder.Append(str_BarcodeLine).Append(str_NewDate).Append(str_NewBarcodeOrder).ToString()
                        str_NewBarcode.Add(str_TempBarcode)
                        mstr_NewBarcode.Add(str_TempBarcode)

                        obj_TempMergeDate = obj_TempMergeDate.AddSeconds(1)

                        Dim lstr_MergeProdDate As String = obj_TempMergeDate.ToString("yyyy-MM-dd HH:mm:ss")

                        'mstr_MergeProdDateList.Add(lstr_MergeProdDate)

                        int_QtyLeft -= lint_StdQty
                    End While

                End If
            End If
        Catch ex As Exception
            BHTController.DisposeScanner(myScanner)
            DisplayMessage.ErrorMsg("Barcode gagal dibuat", "Error")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
        End Try

        Return str_NewBarcode
    End Function

    Private Function fncBarcodeOrderBuild(ByVal str_BarcodeOrder As String, ByVal str_ID As String) As String
        Dim str_Res As String = String.Empty

        'Select Case str_BarcodeOrder.Length
        '    Case 1
        '        str_Res = String.Concat("00000", str_BarcodeOrder)
        '    Case 2
        '        str_Res = String.Concat("0000", str_BarcodeOrder)
        '    Case 3
        '        str_Res = String.Concat("000", str_BarcodeOrder)
        '    Case 4
        '        str_Res = String.Concat("00", str_BarcodeOrder)
        '    Case 5
        '        str_Res = String.Concat("0", str_BarcodeOrder)
        '    Case 6
        '        str_Res = str_BarcodeOrder
        'End Select


        Select Case str_BarcodeOrder.Length
            Case 1
                str_Res = str_ID & String.Concat("00000", str_BarcodeOrder)
            Case 2
                str_Res = str_ID & String.Concat("0000", str_BarcodeOrder)
            Case 3
                str_Res = str_ID & String.Concat("000", str_BarcodeOrder)
            Case 4
                str_Res = str_ID & String.Concat("00", str_BarcodeOrder)
            Case 5
                str_Res = str_ID & String.Concat("0", str_BarcodeOrder)
                'Case 6
                '    str_Res = str_BarcodeOrder
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



#Region "Merge Process"
    Private Function fncProcessMerge() As Boolean
        Dim bool_IntegrateProcess As Boolean = False
        Dim bool_StockCardProcess As Boolean = False
        Dim bool_Status As Boolean = False

        configData = ConfigMgr.GetConfigData()


        Dim str_RemarkData As String = String.Empty
        Dim str_RemarksRes As String = String.Empty
        Dim str_RemarksBuilder As New System.Text.StringBuilder

        Dim str_ProdActIntUser As String = String.Empty
        Dim str_ProdActIntDateTime As String = String.Empty

        mint_SplitFlag = 0
        mint_RevertSplitFlag = 0

        For int_counter As Integer = 0 To mstr_ScannedBarcode.Count - 1
            str_ProdActIntUser = ProductionActIntegrationDB.fncGetUser(mstr_ScannedBarcode(int_counter))
            str_ProdActIntDateTime = ProductionActIntegrationDB.fncGetDateTime(mstr_ScannedBarcode(int_counter))

            str_RemarksBuilder.Append(mstr_ScannedBarcode(int_counter))
            str_RemarksBuilder.Append(";")
            str_RemarksBuilder.Append(marr_PackQtyList(int_counter).ToString)
            str_RemarksBuilder.Append(";")
            str_RemarksBuilder.Append(str_ProdActIntUser)
            str_RemarksBuilder.Append(";")
            str_RemarksBuilder.Append(str_ProdActIntDateTime)
            If int_counter = mstr_ScannedBarcode.Count - 1 Then
                str_RemarkData = str_RemarksBuilder.ToString()
            Else
                str_RemarksBuilder.Append("<BR>")
                str_RemarkData = str_RemarksBuilder.ToString()
            End If
        Next

        str_RemarksRes = str_RemarkData

        Dim lint_FinalId As Int32 = ProductionActIntegrationDB.fncGetFinalID(lstr_TempQRCode)

        'Modify 9i
        'Dim lstr_MergeUserId As String = TemporaryData.loggedInUserID
        Dim lstr_MergeUserId As String = lstr_UserID


        Dim int_InsertActId As Integer
        Dim int_InsertActIdList As New List(Of Integer)

        Dim str_NewBarcodeList As New List(Of String)
        Dim str_NewBarcodeList2 As New List(Of String)
        str_NewBarcodeList = fncGenerateBarcode(lstr_BarcodeLine, configData.ID)

        Dim str_NewQRList As New List(Of String)
        str_NewQRList = fncQRCodeBuild(mint_PackQtyMod)

        Dim lstr_MergeLineName As String = String.Empty
        lstr_MergeLineName = LineMasterDB.fncGetLineCodes(lstr_LineCode)

        Dim lstr_MergeProdDate As String = mstr_MergeProdDateList(0)

        If dgv_ScanData.VisibleRowCount < 2 Then
            BHTController.DisposeScanner(myScanner)
            DisplayMessage.ErrorMsg("Silahkan scan lebih dari satu barcode tag", "Error")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
            Return False
        Else

            If Not ValidateData(mstr_ScannedBarcode) Then
                Return False
            End If
            tmr_DbLoad.Enabled = True


            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim transaction As IDbTransaction = Nothing

                Try
                    connection.Open()
                    transaction = connection.BeginTransaction()


                    For int_Counter As Integer = 0 To str_NewBarcodeList.Count - 1
                        Dim lint_ActQty As Integer = Convert.ToInt32(lbl_ActualQtyValue.Text)

                        Dim lstr_MergeLineCode As String = lstr_BarcodeLine

                        Dim lstr_MergeBarcodeTag As String = String.Empty
                        If str_NewBarcodeList.Count = 1 Then
                            lstr_MergeBarcodeTag = str_NewBarcodeList(0)
                        Else
                            lstr_MergeBarcodeTag = str_NewBarcodeList(int_Counter)
                        End If

                        Dim lstr_MergeTRINPartNo As String = lbl_TRINPartNoValue.Text

                        Dim lint_MergeQty As Int32 = 0
                        If lint_PackQty > lint_StdQty Then
                            lint_MergeQty = Convert.ToInt32(mint_PackQtyMod(int_Counter))
                            'Modify Lutfi
                            lint_PackQty = lint_PackQty - lint_MergeQty
                        Else
                            lint_MergeQty = lint_PackQty
                        End If

                        Dim lstr_MergeQRCode As String = String.Empty
                        If str_NewQRList.Count = 1 Then
                            lstr_MergeQRCode = str_NewQRList(0)
                        Else
                            lstr_MergeQRCode = str_NewQRList(int_Counter)
                        End If

                        Dim lstr_MergeImageName As String = fncNewImageFile(lstr_MergeTRINPartNo)

                        Dim str_tempBarcode As String = frm_IdTagIntegration.DataGridColumns.BarcodeTag.ToString

                        subCloseNowLoading()

                        Dim str_TagIntegrationData(9) As String
                        Dim str_StockCardIntegrationData(7) As String
                        'add 9h
                        Dim lstr_BarcodeReset As String = ""
                        lstr_BarcodeReset = fncCreateBarcode(lstr_BarcodeLine, configData.ID, int_Counter)
                        lstr_MergeBarcodeTag = lstr_BarcodeReset
                        'add 9i
                        str_NewBarcodeList2.Add(lstr_MergeBarcodeTag)

                        Dim tempbarcode() As String = Split(lstr_MergeQRCode, ";")
                        Dim lstr_NewQRStringBuilder As New System.Text.StringBuilder
                        lstr_NewQRStringBuilder.Append(tempbarcode(0))
                        lstr_NewQRStringBuilder.Append(";")
                        lstr_NewQRStringBuilder.Append(lstr_BarcodeReset)
                        lstr_NewQRStringBuilder.Append(";")
                        lstr_NewQRStringBuilder.Append(tempbarcode(2))
                        lstr_NewQRStringBuilder.Append(";")
                        lstr_NewQRStringBuilder.Append(tempbarcode(3))
                        lstr_MergeQRCode = lstr_NewQRStringBuilder.ToString

                        'Insert into PRODUCTIONACT
                        str_TagIntegrationData(InsertTagIntegrationValues.FinalID) = lint_FinalId.ToString
                        str_TagIntegrationData(InsertTagIntegrationValues.BarcodeTag) = lstr_MergeBarcodeTag
                        str_TagIntegrationData(InsertTagIntegrationValues.ProdDate) = lstr_MergeProdDate
                        str_TagIntegrationData(InsertTagIntegrationValues.TrinPartNo) = lstr_MergeTRINPartNo
                        str_TagIntegrationData(InsertTagIntegrationValues.LineCode) = lstr_MergeLineName
                        str_TagIntegrationData(InsertTagIntegrationValues.ActQty) = lint_MergeQty.ToString
                        str_TagIntegrationData(InsertTagIntegrationValues.UserID) = lstr_MergeUserId
                        str_TagIntegrationData(InsertTagIntegrationValues.QrCode) = lstr_MergeQRCode
                        str_TagIntegrationData(InsertTagIntegrationValues.ImgFile) = lstr_MergeImageName
                        str_TagIntegrationData(InsertTagIntegrationValues.Remarks) = str_RemarksRes

                        If str_NewBarcodeList.Count = 0 Then
                            BHTController.DisposeScanner(myScanner)
                            DisplayMessage.ErrorMsg("Silahkan scan QR Code", "Error")
                            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                            Return False
                        Else
                            Dim int_BarcodeArrLength As Integer = str_IntegrationBarcodeArr.Count

                            tmr_DbLoad.Enabled = True

                            int_InsertActId = ProductionActIntegrationDB.fncInsertTagIntegrationUseTransaction(str_TagIntegrationData, connection)

                            If int_InsertActId <> 0 Then
                                int_InsertActIdList.Add(int_InsertActId)
                            Else
                                bool_IntegrateProcess = False
                                Exit For
                            End If

                        End If

                        'insert into STOCK_CARD
                        str_StockCardIntegrationData(InsertStockCardValues.TrinPartNo) = lstr_MergeTRINPartNo
                        str_StockCardIntegrationData(InsertStockCardValues.ActID) = int_InsertActId.ToString
                        str_StockCardIntegrationData(InsertStockCardValues.Stock_In) = lint_MergeQty.ToString
                        str_StockCardIntegrationData(InsertStockCardValues.Remark) = str_RemarksRes
                        str_StockCardIntegrationData(InsertStockCardValues.BarcodeTag) = lstr_MergeBarcodeTag
                        str_StockCardIntegrationData(InsertStockCardValues.LineCode) = lstr_MergeLineName
                        str_StockCardIntegrationData(InsertStockCardValues.Date_Time) = Now.ToString("yyyy-MM-dd HH:mm:ss")
                        str_StockCardIntegrationData(InsertStockCardValues.UserID) = lstr_MergeUserId

                        If ProductionActIntegrationDB.fncInsertStockCardTagIntegrationUsingTransaction(str_StockCardIntegrationData, connection, transaction) Then
                            'If int_Counter = 0 Then
                            '    For int_UpdCount As Integer = 0 To mstr_ScannedBarcode.Count - 1
                            '        If ProductionActIntegrationDB.fncUpdateOldTag(mstr_ScannedBarcode(int_UpdCount), _
                            '                                                      1, _
                            '                                                      0, _
                            '                                                      lint_SplitFlagList(int_UpdCount) _
                            '                                                      ) Then
                            '            bool_IntegrateProcess = True
                            '            If ProductionActIntegrationDB.fncUpdateStockCardTagIntegration(mstr_ScannedBarcode(int_UpdCount), _
                            '                                                                           1, _
                            '                                                                           0 _
                            '                                                                           ) Then
                            '                bool_StockCardProcess = True
                            '            Else
                            '                ProductionActIntegrationDB.fncUpdateOldTag(mstr_ScannedBarcode(int_UpdCount), _
                            '                                                           0, _
                            '                                                           1, _
                            '                                                           lint_SplitFlagList(int_UpdCount) _
                            '                                                           )
                            '                bool_IntegrateProcess = False
                            '                Exit For
                            '            End If
                            '        Else
                            '            bool_IntegrateProcess = False
                            '            Exit For
                            '        End If
                            '        subCloseNowLoading()
                            '    Next
                            'End If
                            'transaction.Rollback()
                            'Exit For
                        Else
                            'bool_IntegrateProcess = False
                            'Exit For
                            transaction.Rollback()
                            Exit For
                        End If
                        subCloseNowLoading()

                        'If int_Counter = str_NewBarcodeList.Count - 1 Then
                        '    bool_Status = True
                        '    bool_IntegrateProcess = True
                        'End If
                    Next

                    transaction.Commit()
                Catch ex As Exception
                    transaction.Rollback()
                    Return False
                End Try
            End Using

            'bool_IntegrateProcess = False
            'If bool_IntegrateProcess = False Then
            'Update Old Barcodetag (Delflag=0=>1)
            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim transaction As IDbTransaction = Nothing

                Try

                    connection.Open()
                    transaction = connection.BeginTransaction()

                    For int_UpdCount As Integer = 0 To mstr_ScannedBarcode.Count - 1
                        If ProductionActIntegrationDB.fncUpdateOldTagUseTrasaction(mstr_ScannedBarcode(int_UpdCount), _
                                                                      1, _
                                                                      0, _
                                                                      lint_SplitFlagList(int_UpdCount), _
                                                                      connection, transaction) Then
                            bool_IntegrateProcess = True
                            If ProductionActIntegrationDB.fncUpdateStockCardTagIntegrationUseTransaction(mstr_ScannedBarcode(int_UpdCount), _
                                                                                           1, _
                                                                                           0, _
                                                                                           connection, transaction) Then
                                bool_StockCardProcess = True
                            End If
                        Else
                            bool_IntegrateProcess = False
                            Exit For
                        End If
                        subCloseNowLoading()
                    Next
                    'End If



                    'Update New Barcodetag (Delflag=1=>0)
                    For int_RollBack = 0 To int_InsertActIdList.Count - 1
                        'modify 9i

                        If ProductionActIntegrationDB.fncProdactTagIntegrationNewtag _
                        (int_InsertActIdList(int_RollBack), connection, transaction) Then

                            If ProductionActIntegrationDB.fncUpdateStockCardTagIntegrationNewtag _
                                (int_InsertActIdList(int_RollBack), 4, connection, transaction) Then

                            End If

                        End If

                    Next

                    transaction.Commit()

                Catch ex As Exception
                    transaction.Rollback()
                    Return False
                End Try
            End Using
        End If

        int_InsertActIdList.Clear()
        marr_PackQtyList.Clear()



        Return True
    End Function

    Private Sub subMergeHandler()
        If String.IsNullOrEmpty(txt_ScanTag.Text) And marr_PackQtyList.Count = 0 Then
            BHTController.DisposeScanner(myScanner)
            DisplayMessage.ErrorMsg("Silahkan Scan QR code", "Error")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
        Else
            tmr_DbLoad.Enabled = True

            If fncProcessMerge() Then
                BHTController.DisposeScanner(myScanner)
                DisplayMessage.OkMsg("Perintah print sedang dikirim ke printer", "Sukses")
                BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)

                txt_ScanTag.Text = String.Empty
                'add by lutfi 9.c
                lint_PackQty = 0
                dt_BarcodeTag.Clear()
                scn_ScannedBarcodeList.Clear()
                mstr_MergeProdDateList.Clear()
                'add lutfi
                mstr_ScannedBarcode.Clear()

                'add lutfi 9.e
                mstr_NewBarcode.Clear()
                mint_PackQtyMod.Clear()
                lint_SplitFlagList.Clear()
                lstr_BarcodeLine = String.Empty
                subInitForm(False)
            End If
            subCloseNowLoading()
        End If
    End Sub

    Public Function ValidateData(ByVal QRBarcodetag As List(Of String)) As Boolean
        For int_counter As Integer = 0 To QRBarcodetag.Count - 1
            Dim QrValid As QueryRetValue = ProductionActDB.fncCheckBarcodetag(QRBarcodetag(int_counter))

            If QrValid <> QueryRetValue.ValueTrue Then
                BHTController.DisposeScanner(myScanner)
                DisplayMessage.ErrorMsg("QR code[" & QRBarcodetag(int_counter) & "]tidak valid", "Error")
                BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                Return False
            End If

        Next
        Return True
    End Function

#End Region

End Class

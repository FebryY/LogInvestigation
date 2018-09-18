Option Strict On
Option Explicit On

Imports TradNewSystem.Helpers
Imports TradNewSystem.Model
Imports TradNewSystem.PocoClass

Imports System.Runtime.InteropServices
Imports System.Globalization
Imports System.Data

Imports DNWA.BHTCL

Public Class frm_ProductModScan

#Region "Declaration"

    Protected Friend hdr_NowLoading As NowLoading
    Protected Friend dt_BarcodeTag As DataTable
    Protected Friend ds_DataSource As DataSet

    Private pobj_BindingSource As New BindingSource
    Private processKeypadPress As Boolean = True

    Private str_TempProdPlanData As New List(Of String())
    Private str_TempQr As String = String.Empty

    Private mstr_LastCellValue As String = String.Empty
    Private marr_PackQtyList As New ArrayList

    Private int_DbLoadDuration As Integer
    Private lstr_BarcodeLine As String
    Private lstr_Barcode As String = String.Empty
    Private lstr_TrinPartNo As String = String.Empty

    Private lint_PackQty As Int32
    Private lint_ProdQty As Int32
    Private date_ProdDate As DateTime
    Private fnt_headerFont As Font
    Private int_DataGridColumnSizes() As Integer

    Private scn_ScannedBarcodeList As New List(Of String)
    Private scn_ScannedQrList As New List(Of String)

    Protected Friend dataSource As DataSet
    Friend WithEvents myScanner As Scanner

    Private lstr_LineCode As String

    Private Enum QrCodeValues
        BarcodeLine
        BarcodeTag
        ActQty
        TrinPartCode
    End Enum

    Private Enum ProdPlanModValues
        LineCode
        ProdDate
        EndTime
        TrinPartNo
        Period
        PlanQty
        ImportDate
        ModBarcode
        OldProdDate
    End Enum

    Friend Enum DataGridColumns
        BarcodeTag
        PackedQty
    End Enum

#End Region

#Region "Sub New"
    Public Sub New(ByVal _lineCode As String)

        InitializeComponent()

        Me.lstr_LineCode = _lineCode
    End Sub
#End Region

#Region "Button Click"

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        subInsertHandler()
    End Sub

    Private Sub btn_FullScreen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_FullScreen.Click
        ToggleTaskbar.EnableDisable()
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
                    subInsertHandler()
                Case Windows.Forms.Keys.F2
                    subClose()
            End Select
        End If
    End Sub

    Private Sub frm_ProductModScan_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        KeypressShortcut(e)
    End Sub

    Private Sub txt_ScanTag_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ScanTag.KeyUp
        KeypressShortcut(e)
    End Sub

    Private Sub cmbNewTRINPartNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbNewTRINPartNo.KeyUp
        KeypressShortcut(e)
    End Sub

    Private Sub btn_FullScreen_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btn_FullScreen.KeyUp
        KeypressShortcut(e)
    End Sub

    Private Sub btn_Save_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btn_Save.KeyUp
        KeypressShortcut(e)
    End Sub

    Private Sub btn_Back_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btn_Back.KeyUp
        KeypressShortcut(e)
    End Sub

#End Region

#Region "Form"

    Private Sub frm_ProductModScan_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
        ds_DataSource = New DataSet

        txt_ScanTag.Focus()
        subInitForm(False)
    End Sub

    Private Sub frm_ProductModScan_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        BHTController.DisposeScanner(myScanner)
    End Sub

#End Region

#Region "Function"
    Private Sub subClose()
        If str_TempProdPlanData.Count > 0 Then
            BHTController.DisposeScanner(myScanner)
            Dim obj_Confirm As Boolean = DisplayMessage.ConfirmationDialog("Data belum disimpan, lanjutkan?", "Konfirmasi")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)

            If obj_Confirm = True Then
                subClearPreparedData()
                Close()
            End If
        Else
            Close()
        End If
    End Sub

    Private Sub subInitForm(ByVal _bool As Boolean)
        Me.lbl_TRINPartNoTitle.Visible = _bool
        Me.lbl_TRINPartNoValue.Visible = _bool
        Me.lbl_ActualQtyTitle.Visible = _bool
        Me.lbl_ActualQtyValue.Visible = _bool
        Me.lbl_LineNameTitle.Visible = _bool
        Me.lbl_LineNameValue.Visible = _bool
        Me.lbl_NewTag.Visible = _bool
        Me.cmbNewTRINPartNo.Visible = _bool
        Me.dgv_ScanResult.Visible = _bool
        Me.dgv_ScanResult.Enabled = _bool
        Me.btn_Save.Visible = _bool
        Me.btn_Save.Enabled = _bool
    End Sub

    Private Sub subQrCheck(ByVal prodQR As String)
        Dim str_TRINPartNo As String = lstr_TrinPartNo

        tmr_DbLoad.Enabled = True

        Dim int_QR As Integer = ProductionActDB.fncCheckQR(prodQR)

        Dim str_OldTRINPartNo As String = ModPartMasterDB.fncGetTRINPartNo(str_TRINPartNo)

        subCloseNowLoading()

        If String.IsNullOrEmpty(prodQR) Then
            BHTController.DisposeScanner(myScanner)
            DisplayMessage.ErrorMsg("Silahkan Scan QR code", "Error")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
        Else
            If int_QR = 1 Then
                txt_ScanTag.Text = String.Empty
                BHTController.DisposeScanner(myScanner)
                DisplayMessage.ErrorMsg("QR code tidak valid", "Error")
                BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
            ElseIf int_QR = 0 Then
                If String.IsNullOrEmpty(str_OldTRINPartNo) Then
                    BHTController.DisposeScanner(myScanner)
                    DisplayMessage.ErrorMsg("Nomor TRIN tidak ditemukan!", "Error")
                    BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                Else
                    If Not String.IsNullOrEmpty(str_TRINPartNo) Then
                        Me.lbl_TRINPartNoValue.Text = str_OldTRINPartNo
                        subFillNewTRIMNo(str_OldTRINPartNo)

                        subSetBarcodeData()
                        subInitForm(True)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub subFillNewTRIMNo(ByVal str_NewTRIN As String)
        tmr_DbLoad.Enabled = True

        Dim newTRINNo As List(Of ModPartMaster) = ModPartMasterDB.fncGetNewTRIMPartNo(str_NewTRIN)

        subCloseNowLoading()

        Dim cmb_Source As New Dictionary(Of String, String)()

        cmb_Source.Add(String.Empty, "Select New TRIN No...")
        If Not newTRINNo Is Nothing Then
            Dim index As Integer = 0

            For Each TRINNo As ModPartMaster In newTRINNo
                cmb_Source.Add(Convert.ToString(index), TRINNo.NEWPARTNO)
                index += 1
            Next
        End If

        cmbNewTRINPartNo.DataSource = New BindingSource(cmb_Source, Nothing)
        cmbNewTRINPartNo.DisplayMember = "Value"
        cmbNewTRINPartNo.ValueMember = "Key"

    End Sub

    Private Sub subClearPreparedData()
        If str_TempProdPlanData.Count = 0 Then
            BHTController.DisposeScanner(myScanner)
            DisplayMessage.OkMsg("Tidak ada perubahan data.", "Pesan")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
        Else
            BHTController.DisposeScanner(myScanner)
            Dim bool_Confirm As Boolean = DisplayMessage.ConfirmationDialog("Apakah ingin membatalkan data yang dihapus?", "Konfirmasi")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)

            If bool_Confirm Then
                subInitForm(False)
                str_TempProdPlanData.Clear()

                txt_ScanTag.Text = ""
            End If
        End If
    End Sub

    Private Sub subSetBarcodeData()
        Dim int_PackQtyCount As Int32 = 0

        tmr_DbLoad.Enabled = True

        Dim obj_BarcodeList As List(Of ProductionPlanMod) = ProductionPlanModDB.fncGetBarcodeValLine(str_TempQr)

        subCloseNowLoading()

        fnt_headerFont = Me.fncGetHeaderFont(dgv_ScanResult.Font)

        If dt_BarcodeTag Is Nothing Then
            dt_BarcodeTag = New DataTable("ModifyTable")
            dt_BarcodeTag.Columns.Add("Barcode Tag", Type.GetType("System.String"))
            dt_BarcodeTag.Columns.Add("Packed Qty", Type.GetType("System.Int32"))
        End If

        int_DataGridColumnSizes = New Integer(1) {}

        If Not obj_BarcodeList Is Nothing Then
            Dim dr_DataRow As DataRow = Nothing

            For Each obj_BarcodeListMembers As ProductionPlanMod In obj_BarcodeList
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

        dgv_ScanResult.DataSource = pobj_BindingSource

        subResizeColumnWidth(dt_BarcodeTag)

        Me.dgv_ScanResult.TabIndex = 1
        Me.dgv_ScanResult.Focus()

    End Sub

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

    Private Function fncMeasureTextWidth(ByVal str_Text As String, ByVal obj_Font As Font) As Integer
        Dim lobj_Instance As Graphics = dgv_ScanResult.CreateGraphics

        Return CInt(lobj_Instance.MeasureString(str_Text, obj_Font).Width)
    End Function

    Private Sub subResizeColumnWidth(ByVal dt_Datatable As DataTable)
        dgv_ScanResult.TableStyles.Clear()

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
        dgv_ScanResult.TableStyles.Add(obj_TableStyle)

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
            Dim productionAct As ProductionAct = ProductionActDB.GetProdDateLineCode(scn_ScanCode, True)

            Dim str_QrItems As String()

            tmr_DbLoad.Enabled = True
            Dim QrValid As QueryRetValue = ProductionActDB.fncCheckQR(scn_ScanCode)
            subCloseNowLoading()

            If QrValid <> QueryRetValue.ValueTrue Then
                BHTController.DisposeScanner(myScanner)
                DisplayMessage.ErrorMsg("QR code tidak valid", "Error")
                BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                Exit Sub
            Else
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

                If scn_ScannedQrList.Count = 0 Then
                    str_TempQr = scn_ScanCode
                    scn_ScannedQrList.Add(str_TempQr)
                Else
                    If scn_ScannedQrList.Contains(scn_ScanCode) Then
                        BHTController.DisposeScanner(myScanner)
                        DisplayMessage.ErrorMsg("Silahkan scan QR Code yang berbeda", "Error")
                        BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                        Exit Sub
                    Else
                        str_TempQr = scn_ScanCode
                        scn_ScannedQrList.Add(str_TempQr)
                    End If
                End If



                If Not String.IsNullOrEmpty(lstr_Barcode) Then
                    If str_QrItems(QrCodeValues.TrinPartCode) <> lstr_TrinPartNo Then
                        BHTController.DisposeScanner(myScanner)
                        DisplayMessage.ErrorMsg("TrinPartNo tidak sama", "Error")
                        BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                    Else
                        BHTController.SoundOK()

                        lstr_Barcode = str_QrItems(QrCodeValues.BarcodeTag)
                        scn_ScannedBarcodeList.Add(lstr_Barcode)

                        txt_ScanTag.Text = lstr_Barcode
                        lstr_BarcodeLine = str_QrItems(QrCodeValues.BarcodeLine)

                        lint_ProdQty = Convert.ToInt32(str_QrItems(QrCodeValues.ActQty))
                        marr_PackQtyList.Add(lint_ProdQty)

                        lstr_TrinPartNo = str_QrItems(QrCodeValues.TrinPartCode)
                        date_ProdDate = productionAct.PRODDATE

                        lbl_ActualQtyValue.Text = lint_ProdQty.ToString

                        tmr_DbLoad.Enabled = True
                        Dim str_LineCode As String = LineMasterDB.fncGetLineCodes(lstr_BarcodeLine)
                        subCloseNowLoading()

                        lbl_LineNameValue.Text = str_LineCode

                        subQrCheck(scn_ScanCode)
                    End If
                Else
                    BHTController.SoundOK()

                    lstr_Barcode = str_QrItems(QrCodeValues.BarcodeTag)
                    scn_ScannedBarcodeList.Add(lstr_Barcode)

                    txt_ScanTag.Text = lstr_Barcode
                    lstr_BarcodeLine = str_QrItems(QrCodeValues.BarcodeLine)

                    lint_ProdQty = Convert.ToInt32(str_QrItems(QrCodeValues.ActQty))
                    marr_PackQtyList.Add(lint_ProdQty)

                    lstr_TrinPartNo = str_QrItems(QrCodeValues.TrinPartCode)
                    date_ProdDate = productionAct.PRODDATE

                    lbl_ActualQtyValue.Text = lint_ProdQty.ToString

                    tmr_DbLoad.Enabled = True
                    Dim str_LineCode As String = LineMasterDB.fncGetLineCodes(lstr_BarcodeLine)
                    subCloseNowLoading()

                    lbl_LineNameValue.Text = str_LineCode

                    subQrCheck(scn_ScanCode)
                End If

            End If
        Catch ex As Exception
            BHTController.DisposeScanner(myScanner)
            DisplayMessage.ErrorMsg(ex.Message, "Scan Error")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
        End Try
    End Sub

#End Region

#Region "Change Process"
    Private Function fncSaveProcess() As Boolean
        tmr_DbLoad.Enabled = True

        Dim int_FinalID As Integer = 0

        Dim int_TotalQty As Integer = 0

        Dim bool_Success As Boolean = False

        Dim str_RemarksRes As String = String.Empty
        Dim str_RemarksBuilder As New System.Text.StringBuilder

        Dim str_ProdActIntUser As String = String.Empty
        Dim str_ProdActIntDateTime As String = String.Empty

        Dim str_OldBarcodes As String = String.Empty

        For int_counter As Integer = 0 To scn_ScannedBarcodeList.Count - 1
            str_ProdActIntUser = ProductionPlanModDB.fncGetUser(scn_ScannedBarcodeList(int_counter))
            str_ProdActIntDateTime = ProductionPlanModDB.fncGetDateTime(scn_ScannedBarcodeList(int_counter))

            str_RemarksBuilder.Append(scn_ScannedBarcodeList(int_counter))
            str_RemarksBuilder.Append(";")
            str_RemarksBuilder.Append(marr_PackQtyList(int_counter).ToString)
            str_RemarksBuilder.Append(";")
            str_RemarksBuilder.Append(str_ProdActIntUser)
            str_RemarksBuilder.Append(";")
            str_RemarksBuilder.Append(str_ProdActIntDateTime)
            If int_counter = scn_ScannedBarcodeList.Count - 1 Then
                str_RemarksRes = str_RemarksBuilder.ToString()
            Else
                str_RemarksBuilder.Append("<BR>")
                str_RemarksRes = str_RemarksBuilder.ToString()
            End If
        Next

        Dim str_LineCodes As String = String.Empty
        str_LineCodes = LineMasterDB.fncGetLineCodes(lstr_LineCode)

        Dim productionPlanMod As ProductionPlanMod = ProductionPlanModDB.fncGetEndDate(lbl_TRINPartNoValue.Text)
        If productionPlanMod Is Nothing Then
            Exit Function
        End If
        Dim lstr_EndDate As DateTime = productionPlanMod.ENDTIME

        'lint_PackQty += Convert.ToInt32(marr_PackQtyList(int_PackQtyCounter))

        subCloseNowLoading()

        Dim obj_SelectedTRINNo As String = DirectCast(cmbNewTRINPartNo.SelectedItem, KeyValuePair(Of String, String)).Value

        Try
            Dim str_CurrProdPlanModData(8) As String

            If Not String.IsNullOrEmpty(str_LineCodes) Then
                For int_InsCount As Integer = 0 To scn_ScannedQrList.Count - 1
                    str_CurrProdPlanModData(ProdPlanModValues.LineCode) = str_LineCodes

                    str_CurrProdPlanModData(ProdPlanModValues.ProdDate) = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    str_CurrProdPlanModData(ProdPlanModValues.EndTime) = lstr_EndDate.ToString("yyyy-MM-dd HH:mm:ss")

                    If String.IsNullOrEmpty(obj_SelectedTRINNo) Then
                        BHTController.DisposeScanner(myScanner)
                        DisplayMessage.ErrorMsg("Silahkan pilih Nomor TRIN baru.", "Error")
                        BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                    Else
                        str_CurrProdPlanModData(ProdPlanModValues.TrinPartNo) = obj_SelectedTRINNo
                    End If

                    str_CurrProdPlanModData(ProdPlanModValues.Period) = "Day"
                    str_CurrProdPlanModData(ProdPlanModValues.PlanQty) = marr_PackQtyList(int_InsCount).ToString
                    str_CurrProdPlanModData(ProdPlanModValues.ImportDate) = Now.ToString("yyyy-MM-dd HH:mm:ss")
                    str_CurrProdPlanModData(ProdPlanModValues.ModBarcode) = str_RemarksRes
                    str_CurrProdPlanModData(ProdPlanModValues.OldProdDate) = date_ProdDate.ToString("yyyy-MM-dd HH:mm:ss")

                    'str_TempProdPlanData.Add(str_CurrProdPlanModData)
                    int_TotalQty += CInt(marr_PackQtyList(int_InsCount).ToString)
                    tmr_DbLoad.Enabled = True

                    If ProductionPlanModDB.fncUpdateProdActMod(scn_ScannedQrList(int_InsCount), 1, 0) Then
                        If ProductionPlanModDB.fncUpdateStockCardTagModification(scn_ScannedBarcodeList(int_InsCount), 1, 0) Then
                            If int_InsCount = scn_ScannedQrList.Count - 1 Then
                                str_CurrProdPlanModData(ProdPlanModValues.PlanQty) = int_TotalQty.ToString
                                str_TempProdPlanData.Add(str_CurrProdPlanModData)
                                int_FinalID = ProductionPlanModDB.fncInsertProdPlanMod(str_CurrProdPlanModData)
                                If int_FinalID <> 0 Then
                                    bool_Success = True
                                End If
                            Else
                                bool_Success = True
                            End If
                        End If
                    End If
                    If bool_Success = False Then
                        ProductionPlanModDB.DeleteData(int_FinalID, obj_SelectedTRINNo)
                        ProductionPlanModDB.fncUpdateProdActMod(scn_ScannedQrList(int_InsCount), 0, 1)
                        ProductionPlanModDB.fncUpdateStockCardTagModification(scn_ScannedBarcodeList(int_InsCount), 0, 1)
                        Exit For
                    End If
                Next
                subCloseNowLoading()
            End If

        Catch ex As Exception
            BHTController.DisposeScanner(myScanner)
            DisplayMessage.ErrorMsg(ex.Message, "Save Error")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
        End Try

        Return bool_Success
    End Function

    Private Sub subInsertHandler()

        Try
            BHTController.DisposeScanner(myScanner)
            If RF.SYNCHRONIZE(RF.SYNC_CHECK) <> 0 Then
                DisplayMessage.ErrorMsg("Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
                    "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.", "Error")
                Exit Sub
            End If
            BHTController.DisposeScanner(myScanner)
        Catch ex As Exception
            If Err.Number = 5 Then
                DisplayMessage.ErrorMsg("Koneksi Wifi di HT tertutup." & vbCrLf & _
                    "Tunggu beberapa detik dan ulangi lagi.", "Error")
                Dim MyRf As RF
                MyRf = New RF()
                MyRf.OpenMode = RF.EN_OPEN_MODE.CONTINUOUSLY
                MyRf.Open = True
                BHTController.DisposeScanner(myScanner)
                Exit Sub
            End If
        End Try

        Dim obj_SelectedTRINNo As String = DirectCast(cmbNewTRINPartNo.SelectedItem, KeyValuePair(Of String, String)).Key

        If String.IsNullOrEmpty(obj_SelectedTRINNo) Then
            BHTController.DisposeScanner(myScanner)
            DisplayMessage.ErrorMsg("Silahkan pilih Nomor TRIN baru.", "Error")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
        Else
            BHTController.DisposeScanner(myScanner)
            Dim bool_Confirm As Boolean = DisplayMessage.ConfirmationDialog("Save data baru ke database?", "Konfirmasi")
            BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)

            If bool_Confirm Then
                tmr_DbLoad.Enabled = True
                Dim SelectedTRINNo As String = DirectCast(cmbNewTRINPartNo.SelectedItem, KeyValuePair(Of String, String)).Value
                Dim IsSuccess As QueryRetValue
                IsSuccess = fncCheckTRIMPartNoMaster(SelectedTRINNo)
                If IsSuccess = QueryRetValue.ValueFalse Then
                    BHTController.DisposeScanner(myScanner)
                    DisplayMessage.ErrorMsg("TRIN baru tidak terdaftar di Part Master.", "Error")
                    BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)

                    subCloseNowLoading()
                    Exit Sub

                End If

                If Not fncSaveProcess() Then
                    BHTController.DisposeScanner(myScanner)
                    DisplayMessage.ErrorMsg("Save Error", "Error")
                    BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
                Else
                    BHTController.DisposeScanner(myScanner)
                    DisplayMessage.OkMsg("Data telah berhasil disimpan", "Pesan")
                    BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)

                    txt_ScanTag.Text = String.Empty
                    lstr_TrinPartNo = String.Empty
                    str_TempQr = String.Empty

                    subInitForm(False)
                End If

                subCloseNowLoading()
            Else
                BHTController.DisposeScanner(myScanner)
                DisplayMessage.ErrorMsg("Tidak ada perubahan data", "Error")
                BHTController.InitialiseScanner(myScanner, ScannerCodeType.QrCode, ScannerReadMode.Momentary)
            End If
        End If
        str_TempProdPlanData.Clear()
        dt_BarcodeTag.Clear()
        scn_ScannedBarcodeList.Clear()

        'add by 9g
        scn_ScannedQrList.Clear()

        lstr_TrinPartNo = String.Empty
        lstr_Barcode = String.Empty
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

#Region "Control Handler"

    Private Sub subEnableControl(ByVal _bool As Boolean)
        btn_FullScreen.Enabled = _bool
        btn_Save.Enabled = _bool
        btn_Back.Enabled = _bool
        processKeypadPress = _bool
    End Sub
#End Region

End Class
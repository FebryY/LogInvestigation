Option Strict On
Option Explicit On

Imports System.Globalization

Imports DNWA.BHTCL

Imports System.Data.SQLite
Imports System.Data
Imports System.Linq

Imports TradNewSystem.Helpers
Imports TradNewSystem.Model
Imports TradNewSystem.PocoClass
Imports MySql.Data.MySqlClient
Imports System.Net
Imports log4net


Public Class ShipmentScan
    Protected Friend nowLoadingPopUp As NowLoading

    Private shipmentList As ShipmentList
    Private trinPartNo As String
    Private customerCode As String
    Private planQty As Integer
    Private _actualQty As Integer
    Private _totalScannedQty As Integer
    Private _totalScannedQtyF As Integer
    Private sid As String
    Private soNumber As String
    Private plantNo As String

    Private _scannedBarcodes As New List(Of String)
    Private _tmpShipActDataInsert As New List(Of String())
    Private _tmpShipActDataUpdate As New List(Of String())
    Private _tmpStockCardDataInsert As New List(Of String())
    Private _tmpStockCardDataUpdate As New List(Of String())
    Private _tmpScannedDate As New List(Of String)
    Private str_tmpSplitQr As New List(Of String())

    Private str_tmpSplitBarcode As New List(Of String)
    Private str_tmpScannedQrcode As New List(Of String)
    Private str_tmpScannedQty As New List(Of String)
    Private str_tmpTrinPartNo As New List(Of String)
    Private int_tmpActIdList As New List(Of Integer)
    Private int_TmpScannedQtyList As New List(Of Integer)
    Private str_TmpDeletedActId As New List(Of String)
    Private str_tmpShipId As New List(Of String)
    Private str_tmpSONumber As New List(Of String)
    Private str_tmpCustCode As New List(Of String)
    Private str_tmpPlantNo As New List(Of String)

    Private _lastScannedBarcode As String
    Private _lastScannedQty As String

    Private currentRowOnDataGrid As Integer
    Private scannerOnDuration As Integer
    Private scannerIsOn As Boolean
    Private scannerIsEnabled As Boolean
    Private processKeypadIsEnabled As Boolean
    Private processScanData As Boolean

    Private mstr_SqliteTable As String = "TempShipmentScan"
    Private mint_InputFlag As Integer = 2
    Private mobj_SqliteData As New Dictionary(Of String, String)()
    Private cint_TmpScannedTotalQty As Integer
    Private bool_TempDataLoad As Boolean = False
    Private bool_Status As Boolean = False


    Friend WithEvents myScanner As Scanner

#Region "Properties"
    Public Property scannedBarcodes() As List(Of String)
        Get
            Return _scannedBarcodes
        End Get

        Set(ByVal value As List(Of String))
            _scannedBarcodes = value
        End Set
    End Property

    Public Property tmpShipActDataInsert() As List(Of String())
        Get
            Return _tmpShipActDataInsert
        End Get

        Set(ByVal value As List(Of String()))
            _tmpShipActDataInsert = value
        End Set
    End Property

    Public Property tmpShipActDataUpdate() As List(Of String())
        Get
            Return _tmpShipActDataUpdate
        End Get

        Set(ByVal value As List(Of String()))
            _tmpShipActDataUpdate = value
        End Set
    End Property

    Public Property tmpStockCardDataInsert() As List(Of String())
        Get
            Return _tmpStockCardDataInsert
        End Get

        Set(ByVal value As List(Of String()))
            _tmpStockCardDataInsert = value
        End Set
    End Property

    Public Property tmpStockCardDataUpdate() As List(Of String())
        Get
            Return _tmpStockCardDataUpdate
        End Get

        Set(ByVal value As List(Of String()))
            _tmpStockCardDataUpdate = value
        End Set
    End Property

    Public Property lastScannedBarcode() As String
        Get
            Return _lastScannedBarcode
        End Get

        Set(ByVal value As String)
            _lastScannedBarcode = value
        End Set
    End Property

    Public Property actualQty() As Integer
        Get
            Return _actualQty
        End Get

        Set(ByVal value As Integer)
            _actualQty = value
        End Set
    End Property

    Public Property totalScannedQty() As Integer
        Get
            Return _totalScannedQty
        End Get

        Set(ByVal value As Integer)
            _totalScannedQty = value
        End Set
    End Property
#End Region

    Public Sub New(ByVal shipmentList As ShipmentList)
        InitializeComponent()

        Me.shipmentList = shipmentList
        currentRowOnDataGrid = shipmentList.DataGrid1.CurrentCell.RowNumber

        customerCode = shipmentList.DataGrid1( _
            currentRowOnDataGrid, _
            shipmentList.DataGridColumns.CustomerCode _
            ).ToString()
        plantNo = shipmentList.DataGrid1( _
            currentRowOnDataGrid, _
            shipmentList.DataGridColumns.PlantNo _
            ).ToString()
        sid = shipmentList.DataGrid1( _
            currentRowOnDataGrid, _
            shipmentList.DataGridColumns.SID _
            ).ToString()
        soNumber = shipmentList.DataGrid1( _
            currentRowOnDataGrid, _
            shipmentList.DataGridColumns.SONumber _
            ).ToString()
        trinPartNo = shipmentList.DataGrid1( _
            currentRowOnDataGrid, _
            shipmentList.DataGridColumns.TrinPartNo _
            ).ToString()

        labelTrinPartCodeValue.Text = trinPartNo
        LoadDefaultDisplayedValues()
    End Sub

    Private Sub LoadDefaultDisplayedValues()
        Dim thousandSeparatedPlanQty As String = shipmentList.DataGrid1( _
            currentRowOnDataGrid, _
            shipmentList.DataGridColumns.PlanQty _
            ).ToString()
        Dim thousandSeparatedActualQty As String = shipmentList.DataGrid1( _
            currentRowOnDataGrid, _
            shipmentList.DataGridColumns.ActQty _
            ).ToString()

        Dim currentCulture As CultureInfo = CultureInfo.CurrentCulture

        planQty = CInt(Convert.ToDouble( _
            thousandSeparatedPlanQty, _
            currentCulture _
            ))
        _actualQty = CInt(Convert.ToDouble( _
            thousandSeparatedActualQty, _
            currentCulture _
            ))

        labelPlanQtyValue.Text = String.Format("{0:n0}", planQty)
        labelActualQtyValue.Text = String.Format("{0:n0}", _actualQty)
        labelRemainingQtyValue.Text = String.Format( _
            "{0:n0}", _
            planQty - _actualQty _
            )

        _lastScannedBarcode = String.Empty
        textBoxScannedTag.Text = String.Empty
    End Sub

    Private Sub ShipmentScan_Load( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles MyBase.Load

        cint_TmpScannedTotalQty = 0

        'add by lutfi 9f
        _totalScannedQtyF = 0

        Dim sql_Query As String = String.Format("select SID, SONumber, CustCode, PlantNo, BarcodeTag, QrCode, TrinPartNo, ActQty " & _
                                                "from {0} where InputFlag = '{1}';", _
                                                mstr_SqliteTable, _
                                                mint_InputFlag _
                                                )

        Dim dt_SqliteData As DataTable = SqliteControllerDB.fncGetDatatable(sql_Query)

        str_tmpShipId = (From dr_Sqlite In dt_SqliteData.AsEnumerable() Select dr_Sqlite.Field(Of String)(0)).ToList()
        str_tmpSONumber = (From dr_Sqlite In dt_SqliteData.AsEnumerable() Select dr_Sqlite.Field(Of String)(1)).ToList()
        str_tmpCustCode = (From dr_Sqlite In dt_SqliteData.AsEnumerable() Select dr_Sqlite.Field(Of String)(2)).ToList()
        str_tmpPlantNo = (From dr_Sqlite In dt_SqliteData.AsEnumerable() Select dr_Sqlite.Field(Of String)(3)).ToList()
        str_tmpSplitBarcode = (From dr_Sqlite In dt_SqliteData.AsEnumerable() Select dr_Sqlite.Field(Of String)(4)).ToList()
        str_tmpScannedQrcode = (From dr_Sqlite In dt_SqliteData.AsEnumerable() Select dr_Sqlite.Field(Of String)(5)).ToList()
        str_tmpTrinPartNo = (From dr_Sqlite In dt_SqliteData.AsEnumerable() Select dr_Sqlite.Field(Of String)(6)).ToList()
        str_tmpScannedQty = (From dr_Sqlite In dt_SqliteData.AsEnumerable() Select dr_Sqlite.Field(Of String)(7)).ToList()

        _scannedBarcodes = (From dr_Sqlite In dt_SqliteData.AsEnumerable() Select dr_Sqlite.Field(Of String)(4)).ToList()

        If dt_SqliteData.Rows.Count > 0 Then
            bool_TempDataLoad = True

            Dim str_Confirmation As String = String.Format("Data SoNumber {0}, TrinPartNo {1} belum diproses, ingin menyimpan?", _
                                                           str_tmpSONumber(0), _
                                                           str_tmpTrinPartNo(0))

            Dim bool_Confirm As Boolean = DisplayMessage.ConfirmationDialog(str_Confirmation, _
                                                                            "Konfirmasi" _
                                                                            )
            If bool_Confirm Then
                bool_Status = True
                int_TmpScannedQtyList = str_tmpScannedQty.ConvertAll(AddressOf Int32.Parse)

                For Each obj_TmpScannedQtyItems In int_TmpScannedQtyList
                    cint_TmpScannedTotalQty += obj_TmpScannedQtyItems
                Next

                For int_Counter As Integer = 0 To str_tmpSplitBarcode.Count - 1

                    processScanData = False

                    Dim splitQRCode As String() = str_tmpScannedQrcode(int_Counter).Split(New Char() {";"c})

                    Dim productionAct As ProductionAct = Nothing
                    Dim actIdOfDeletedShipment As Integer = 0

                    'Update
                    'actIdOfDeletedShipment = ShipmentActDB.GetActIdOfDeletedData(splitQRCode(FinalQrCodeValues.BarcodeTag))
                    actIdOfDeletedShipment = -1

                    If actIdOfDeletedShipment = -1 Then
                        Dim itemHasBeenShipped As Boolean = ( _
                            ScanValidator.HasItemBeenShipped( _
                                myScanner, _
                                timerScanner, _
                                scannerIsOn, _
                                splitQRCode(FinalQrCodeValues.BarcodeTag) _
                                ) _
                            )

                        If itemHasBeenShipped Then
                            CloseNowLoadingAndEnableControls()
                            ResetScannerAndShowErrorMessage( _
                                String.Format( _
                                    "QR Code [{0}] Telah Terdaftar Di Database!", _
                                    splitQRCode(FinalQrCodeValues.BarcodeTag) _
                                    ), _
                                "Error" _
                                )

                            Dim str_SqlWhere As String = String.Format("BarcodeTag = '{0}' and InputFlag = '2'", _
                                                                       str_tmpSplitBarcode(int_Counter) _
                                                                       )

                            SqliteControllerDB.fncDelete(mstr_SqliteTable, str_SqlWhere)

                            str_tmpSplitBarcode.Clear()
                            str_tmpScannedQrcode.Clear()
                            str_tmpScannedQty.Clear()
                            str_tmpTrinPartNo.Clear()
                            ResetScanRelatedData()
                            Close()
                        End If
                    End If

                    Dim exceptionMsg As String = String.Empty
                    Dim balanceQty As Integer = StockCardSummaryDB.GetStockBalanceQty( _
                        splitQRCode(FinalQrCodeValues.BarcodeTag), _
                        exceptionMsg _
                        )

                    If balanceQty <= 0 Then
                        CloseNowLoadingAndEnableControls()

                        If exceptionMsg.Length > 0 Then
                            ResetScannerAndShowErrorMessage( _
                                exceptionMsg, _
                                "DB Error" _
                                )
                        Else
                            Dim errorMsg As String = String.Format( _
                                "Tidak Terdapat Sisa Stock Untuk QR Code {0} di Sistem", _
                                splitQRCode(FinalQrCodeValues.BarcodeTag) _
                                )

                            ResetScannerAndShowErrorMessage( _
                                errorMsg, _
                                "Error" _
                            )

                            Dim str_SqlWhere As String = String.Format("BarcodeTag = '{0}' and InputFlag = '2'", _
                                                                       str_tmpSplitBarcode(int_Counter) _
                                                                       )

                            SqliteControllerDB.fncDelete(mstr_SqliteTable, str_SqlWhere)

                            str_tmpSplitBarcode.Clear()
                            str_tmpScannedQrcode.Clear()
                            str_tmpScannedQty.Clear()
                            str_tmpTrinPartNo.Clear()
                            ResetScanRelatedData()
                            Close()
                            Exit Sub
                        End If

                    End If

                    ' QRCode is registered in productionAct?
                    productionAct = ProductionActDB.GetProdDateLineCode( _
                        String.Join(";", splitQRCode), _
                        True _
                        )

                    Dim qrCodeIsRegistered As Boolean = ( _
                        ScanValidator.IsItemQRCodeRegistered( _
                            myScanner, _
                            timerScanner, _
                            scannerIsOn, _
                            productionAct _
                            ) _
                        )

                    If Not qrCodeIsRegistered Then
                        CloseNowLoadingAndEnableControls()
                        ResetScannerAndShowErrorMessage( _
                            "Barang Sudah Terkirim atau QR Code Tidak Valid!", _
                            "Error" _
                        )
                        str_tmpSplitBarcode.Clear()
                        str_tmpScannedQrcode.Clear()
                        str_tmpScannedQty.Clear()
                        str_tmpTrinPartNo.Clear()
                        Close()
                        Exit Sub
                    End If

                    Dim singleShipActData(13) As String

                    singleShipActData(ShipmentActValues.SID) = str_tmpShipId(int_Counter)
                    singleShipActData(ShipmentActValues.SONumber) = str_tmpSONumber(int_Counter)
                    singleShipActData(ShipmentActValues.CustomerCode) = str_tmpCustCode(int_Counter)
                    singleShipActData(ShipmentActValues.PlantNo) = str_tmpPlantNo(int_Counter)
                    singleShipActData(ShipmentActValues.ShipmentDate) = Now.ToString( _
                        "yyyy-MM-dd HH:mm:ss" _
                        )
                    singleShipActData(ShipmentActValues.UserID) = ( _
                        TemporaryData.loggedInUserID _
                        )
                    singleShipActData(ShipmentActValues.BarcodeTag) = str_tmpSplitBarcode(int_Counter)
                    singleShipActData(ShipmentActValues.ItemProductionDate) = ( _
                        productionAct.PRODDATE.ToString("yyyy-MM-dd HH:mm:ss") _
                        )
                    singleShipActData(ShipmentActValues.TrinPartCode) = str_tmpTrinPartNo(int_Counter)
                    singleShipActData(ShipmentActValues.OKNG) = "1"
                    singleShipActData(ShipmentActValues.ActQty) = str_tmpScannedQty(int_Counter).ToString
                    singleShipActData(ShipmentActValues.ActIdOfDeletedData) = ( _
                        actIdOfDeletedShipment.ToString() _
                        )
                    singleShipActData(ShipmentActValues.LineCode) = ( _
                        productionAct.LINECODE _
                        )
                    singleShipActData(ShipmentActValues.LabelPartName) = ( _
                                            Dns.GetHostEntry(Dns.GetHostName()).AddressList(0).ToString() _
                                            )
                    ' actIdOfDeletedShipment = -1 ->
                    ' previous item shipment has been deleted and re-scanned
                    If actIdOfDeletedShipment = -1 Then
                        _tmpShipActDataInsert.Add(singleShipActData)
                    Else
                        _tmpShipActDataUpdate.Add(singleShipActData)
                    End If

                    If String.Compare(str_tmpTrinPartNo(int_Counter), trinPartNo) = 0 Then
                        _actualQty += CInt(str_tmpScannedQty(int_Counter))
                    End If
                    trinPartNo = str_tmpTrinPartNo(int_Counter)
                Next

                Dim int_TotaRemaining As Integer = CInt(labelRemainingQtyValue.Text) - cint_TmpScannedTotalQty

                If int_TotaRemaining < planQty Then

                    SaveScannedData()
                Else
                    Dim errorMsg As String = String.Format("Actual Qty = {0} Tidak Boleh Melebihi Plan Qty = {1}!" & _
                                                           "Mohon Ulangi Scan Untuk SoNumber {2}.", _
                                                           _actualQty.ToString, _
                                                           planQty, _
                                                           str_tmpSONumber(0) _
                                                           )
                    ResetScannerAndShowErrorMessage(errorMsg, "Error")
                    ResetScanRelatedData()
                    SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '2'")
                End If
            Else
                Dim del_Confirm As Boolean = DisplayMessage.ConfirmationDialog("Yakin untuk menghapus data?", _
                                                                            "Konfirmasi" _
                                                                            )
                If del_Confirm Then
                    SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '2'")
                    str_tmpSplitBarcode.Clear()
                    str_tmpScannedQrcode.Clear()
                    str_tmpScannedQty.Clear()
                    str_tmpTrinPartNo.Clear()
                    ResetScanRelatedData()
                    processKeypadIsEnabled = True
                    EnableScanner()
                Else
                    str_tmpSplitBarcode.Clear()
                    str_tmpScannedQrcode.Clear()
                    str_tmpScannedQty.Clear()
                    str_tmpTrinPartNo.Clear()
                    ResetScanRelatedData()
                    Close()
                    Exit Sub
                End If
            End If
        End If

        processKeypadIsEnabled = True
        processScanData = True
        EnableScanner()
        If bool_Status Then DisableScanner()
    End Sub

    Private Sub ShipmentScan_Closed( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
        ) Handles MyBase.Closed
        timerScanner.Enabled = False
        DisableScanner()
    End Sub

#Region "Key Press"
    Private Sub ActivateCommonShortcut(ByVal e As KeyEventArgs)
        If Not processKeypadIsEnabled Then
            Exit Sub
        End If

        Select Case e.KeyCode
            Case Windows.Forms.Keys.F1
                SaveScannedData()
            Case Windows.Forms.Keys.F2
                SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '2'")
                CloseWindow()
            Case Windows.Forms.Keys.F3
                SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '2'")
                ResetForm()
        End Select
    End Sub

    Private Sub UndoChangeOfTextBoxScanTag()
        textBoxScannedTag.Text = _lastScannedBarcode
        textBoxScannedTag.SelectionStart = 0
        textBoxScannedTag.SelectionLength = 0
    End Sub

    Private Sub ButtonFullScreen_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonFullScreen.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ShipmentScan_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles MyBase.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub TextBoxScanTag_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles textBoxScannedTag.KeyUp
        ActivateCommonShortcut(e)

        If e.KeyCode <> Keys.SCAN _
            Or e.KeyCode <> Keys.SCAN2 _
            Or e.KeyCode <> Keys.SCAN3 Then
            UndoChangeOfTextBoxScanTag()
        End If
    End Sub

    Private Sub TextBoxScanTag_KeyDown( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles textBoxScannedTag.KeyDown
        If e.KeyCode = Keys.SCAN _
            Or e.KeyCode = Keys.SCAN2 _
            Or e.KeyCode = Keys.SCAN3 Then
            If Not scannerIsEnabled Then
                Exit Sub
            End If

            If Not scannerIsOn Then
                SetScannerStatusOn()
            Else
                SetScannerStatusOff()
            End If
        Else
            UndoChangeOfTextBoxScanTag()
        End If
    End Sub

    Private Sub ButtonSave_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles buttonSave.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonBack_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles buttonBack.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonReset_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles buttonReset.KeyUp
        ActivateCommonShortcut(e)
    End Sub
#End Region

#Region "Button Click"
    Private Sub ButtonFullScreen_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonFullScreen.Click
        ToggleTaskbar.EnableDisable()
    End Sub

    Private Sub ButtonSave_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles buttonSave.Click
        SaveScannedData()
    End Sub

    Private Sub ButtonBack_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles buttonBack.Click
        SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '2'")
        CloseWindow()
    End Sub

    Private Sub ButtonReset_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles buttonReset.Click
        SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '2'")
        ResetForm()
    End Sub
#End Region

    Private Sub SaveScannedData()
        'modify 0.9l
        'log4net.Config.XmlConfigurator.Configure()
        'Dim log As ILog = LogManager.GetLogger("TRADLogger")
        If _scannedBarcodes.Count > 0 Then
            Dim confirm As Boolean = ResetScannerAndShowConfirmationMessage( _
                "Save Data Yang Telah Di Scan ke Database?", _
                "Konfirmasi" _
                )

            If confirm = True Then
                Try
                    If RF.SYNCHRONIZE(RF.SYNC_CHECK) <> 0 Then
                        'log.Info("Start Info WifiConectionCheck Signal Distance Shipment Scan method SaveScannedData")
                        'log.Info("Additional Message: Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
                        '    "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.")
                        'log.Info("End Info WifiConectionCheck Signal Distance Shipment Scan method SaveScannedData")

                        ResetScannerAndShowErrorMessage("Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
                            "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.", "Error")
                        Exit Sub
                    End If
                Catch ex As Exception
                    'log.Error("Start Error WifiConectionCheck Shipment Scan method SaveScannedData")
                    If Err.Number = 5 Then
                        '    log.Error("Additional Message: Koneksi Wifi di HT tertutup." & vbCrLf & _
                        '"Tunggu beberapa detik dan ulangi lagi.")
                        '    log.Error("Error Number: " & Err.Number & vbCrLf & "Error Description: " & Err.Description & vbCrLf, ex)
                        '    log.Error("End Error WifiConectionCheck Shipment Filter")

                        ResetScannerAndShowErrorMessage("Koneksi Wifi di HT tertutup." & vbCrLf & _
                            "Tunggu beberapa detik dan ulangi lagi.", "Error")
                        Dim MyRf As RF
                        MyRf = New RF()
                        MyRf.OpenMode = RF.EN_OPEN_MODE.CONTINUOUSLY
                        MyRf.Open = True
                        Exit Sub
                    End If
                    'log.Error("Error Number: " & Err.Number & vbCrLf & "Error Description: " & Err.Description & vbCrLf, ex)
                    'log.Error("End Error WifiConectionCheck Shipment Scan method SaveScannedData")
                End Try

                'Add 9j
                Dim ErrMsg As String = ""
                If Not ValidateData(_scannedBarcodes, ErrMsg) Then
                    ResetScannerAndShowErrorMessage(ErrMsg, "Error")
                    Exit Sub
                End If

                '********************************************************
                'Add Validation Check for when HT scan but not yet saved,
                'and WEB operator delete the shipment plan
                '[Update] : 2017/02/10 _ Adri
                '********************************************************
                If ShipmentActDB.fncCheckSID(sid) Then
                    If ProcessSavingDataToDB() Then
                        SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '2'")
                    End If

                    'add by lutfi
                    DisableScanner()
                    Close()
                Else
                    ResetScannerAndShowErrorMessage("Shipment Plan Telah Di Hapus Dari Sistem, " & _
                                                    "Silahkan Pilih Kembali Plan Baru", _
                                                    "Error" _
                                                   )
                    ResetScanRelatedData()
                    LoadDefaultDisplayedValues()
                End If
                '********************************************************
                'End Update
                '********************************************************
            Else
                If bool_TempDataLoad Then
                    str_tmpSplitBarcode.Clear()
                    str_tmpScannedQrcode.Clear()
                    str_tmpScannedQty.Clear()
                    str_tmpTrinPartNo.Clear()
                    bool_TempDataLoad = False
                    Close()
                End If
            End If
        Else
            ResetScannerAndShowErrorMessage( _
                "Tidak Ada Scan Data Untuk Di Simpan di Database", _
                "Error" _
                )
        End If
        'LogManager.Shutdown()
    End Sub

    Private Sub CloseWindow()
        If _scannedBarcodes.Count = 0 Then
            Close()
        Else
            Dim confirm As Boolean = ResetScannerAndShowConfirmationMessage( _
                "Data Yang Telah Di Scan Belum Tersimpan " & _
                    "di Database. Lanjut?", _
                "Konfirmasi" _
                )

            If confirm = True Then
                Close()
            End If
        End If
    End Sub

    Private Sub ResetForm()
        If _scannedBarcodes.Count = 0 Then
            ResetScannerAndShowErrorMessage( _
                "Tidak Ada Data Untuk Dihapus", _
                "Error" _
                )
        Else
            Dim confirm As Boolean = ResetScannerAndShowConfirmationMessage( _
                "Apakah Anda Ingin Menghapus Data Yang Telah Di Scan?", _
                "Konfirmasi" _
                )

            If confirm = True Then
                ResetScanRelatedData()
                LoadDefaultDisplayedValues()
            End If
        End If
    End Sub

    Private Sub ResetScanRelatedData()
        mobj_SqliteData.Clear()
        int_tmpActIdList.Clear()
        int_TmpScannedQtyList.Clear()
        str_TmpDeletedActId.Clear()
        str_tmpSplitBarcode.Clear()
        str_tmpScannedQrcode.Clear()
        _scannedBarcodes.Clear()
        _tmpShipActDataInsert.Clear()
        _tmpShipActDataUpdate.Clear()
        _tmpStockCardDataInsert.Clear()
        _tmpStockCardDataUpdate.Clear()
        _tmpScannedDate.Clear()
        _totalScannedQty = 0
    End Sub

    Private Sub EnableScanner()
        'myScanner.PortOpen = False
        BHTController.InitialiseScanner( _
            myScanner, _
            ScannerCodeType.QrCode, _
            ScannerReadMode.Alternate _
            )
        scannerIsEnabled = True
    End Sub

    Private Sub DisableScanner()
        BHTController.DisposeScanner(myScanner)
        scannerIsEnabled = False
    End Sub

    Private Sub SetScannerStatusOn()
        scannerIsOn = True
        timerScanner.Enabled = True
        scannerOnDuration = 0
    End Sub

    Private Sub SetScannerStatusOff()
        scannerIsOn = False
        timerScanner.Enabled = False
    End Sub

#Region "Scanned Data Validations"
    Private Function ValidateScannedData( _
        ByVal splitQrCode As String(), _
        ByRef productionAct As ProductionAct, _
        ByRef actIdOfDeletedShipment As Integer _
        ) As Boolean
        ShowNowLoadingAndDisableControls()

        If Not ScanValidator.isValidFinalQrCode(splitQrCode) Then
            CloseNowLoadingAndEnableControls()
            ResetScannerAndShowErrorMessage( _
                "QR Code Tidak Valid!", _
                "Scan Error" _
            )
            Return False
        End If

        ' Match TRIN Part No
        Dim trinCodeIsMatched As Boolean = ( _
            ScanValidator.IsProductTrinCodeMatched( _
                myScanner, _
                timerScanner, _
                scannerIsOn, _
                trinPartNo, _
                splitQrCode(FinalQrCodeValues.TrinPartCode) _
                ) _
            )

        If Not trinCodeIsMatched Then
            CloseNowLoadingAndEnableControls()
            ResetScannerAndShowErrorMessage( _
                "TRIN Part No Tidak Sama!", _
                "Error" _
                )
            Return False
        End If

        ' QR Code has been scanned only
        Dim barcodeHasBeenScanned As Boolean = ( _
            ScanValidator.HasBarcodeTagBeenScanned( _
                myScanner, _
                timerScanner, _
                scannerIsOn, _
                _scannedBarcodes, _
                splitQrCode(FinalQrCodeValues.BarcodeTag) _
                ) _
            )

        If barcodeHasBeenScanned Then
            CloseNowLoadingAndEnableControls()
            ResetScannerAndShowErrorMessage( _
                String.Format( _
                    "QR Code [{0}] Telah Di Scan Sebelumnya!", _
                    splitQrCode(FinalQrCodeValues.BarcodeTag) _
                    ), _
                "Error" _
                )
            Return False
        End If

        ' New ActQty < PlanQty
        Dim newActQty As Integer = ( _
            _actualQty _
            + CInt(splitQrCode(FinalQrCodeValues.ActQty)) _
            )

        If newActQty > planQty Then
            CloseNowLoadingAndEnableControls()

            Dim errorMsg As String = String.Format( _
                "Actual Qty = {0} Tidak Boleh Melebihi Plan Qty = {1}!", _
                newActQty, _
                planQty _
                )
            ResetScannerAndShowErrorMessage(errorMsg, "Error")

            Return False
        End If

        ' QR Code has been scanned and saved?
        'update 9K
        'actIdOfDeletedShipment = ShipmentActDB.GetActIdOfDeletedData( _
        '    splitQrCode(FinalQrCodeValues.BarcodeTag) _
        '    )
        actIdOfDeletedShipment = -1

        If actIdOfDeletedShipment = -1 Then
            Dim itemHasBeenShipped As Boolean = ( _
                ScanValidator.HasItemBeenShipped( _
                    myScanner, _
                    timerScanner, _
                    scannerIsOn, _
                    splitQrCode(FinalQrCodeValues.BarcodeTag) _
                    ) _
                )

            If itemHasBeenShipped Then
                CloseNowLoadingAndEnableControls()
                ResetScannerAndShowErrorMessage( _
                    String.Format( _
                        "QR Code [{0}] Telah Terdaftar Di Database!", _
                        splitQrCode(FinalQrCodeValues.BarcodeTag) _
                        ), _
                    "Error" _
                    )
                Return False
            End If
        End If

        ' Total quantity for given barcode in stockCard > 0?
        Dim exceptionMsg As String = String.Empty
        Dim balanceQty As Integer = StockCardSummaryDB.GetStockBalanceQty( _
            splitQrCode(FinalQrCodeValues.BarcodeTag), _
            exceptionMsg _
            )

        If balanceQty <= 0 Then
            CloseNowLoadingAndEnableControls()

            If exceptionMsg.Length > 0 Then
                ResetScannerAndShowErrorMessage( _
                    exceptionMsg, _
                    "DB Error" _
                    )
            Else
                Dim errorMsg As String = String.Format( _
                    "Tidak Terdapat Sisa Stock Untuk QR Code {0} di Sistem", _
                    splitQrCode(FinalQrCodeValues.BarcodeTag) _
                    )

                ResetScannerAndShowErrorMessage( _
                    errorMsg, _
                    "Error" _
                )
            End If

            Return False
        End If

        ' QRCode is registered in productionAct?
        productionAct = ProductionActDB.GetProdDateLineCode( _
            String.Join(";", splitQrCode), _
            True _
            )

        Dim qrCodeIsRegistered As Boolean = ( _
            ScanValidator.IsItemQRCodeRegistered( _
                myScanner, _
                timerScanner, _
                scannerIsOn, _
                productionAct _
                ) _
            )

        If Not qrCodeIsRegistered Then
            CloseNowLoadingAndEnableControls()
            ResetScannerAndShowErrorMessage( _
                "Barang Sudah Terkirim atau QR Code Tidak Valid!", _
                "Error" _
            )
            Return False
        End If

        CloseNowLoadingAndEnableControls()
        Return True
    End Function

#End Region

    Private Sub ShowNowLoadingAndDisableControls()
        EnableFormControls(False)

        nowLoadingPopUp = New NowLoading()
        nowLoadingPopUp.Show()
    End Sub

    Private Sub CloseNowLoadingAndEnableControls()
        EnableFormControls(True)

        If Not IsNothing(nowLoadingPopUp) Then
            nowLoadingPopUp.Close()
            nowLoadingPopUp = Nothing
        End If
    End Sub

    Private Sub myScanner_OnDone( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles myScanner.OnDone
        textBoxScannedTag.Focus()
        'modify 0.9l
        'log4net.Config.XmlConfigurator.Configure()
        'Dim log As ILog = LogManager.GetLogger("TRADLogger")
        Try
            If RF.SYNCHRONIZE(RF.SYNC_CHECK) <> 0 Then
                'log.Info("Start Info WifiConectionCheck Signal Distance Shipment Scan method myScanner_OnDone")
                'log.Info("Additional Message: Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
                '    "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.")
                'log.Info("End Info WifiConectionCheck Signal Distance Shipment Scan method myScanner_OnDone")

                DisplayMessage.ErrorMsg("Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
                    "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.", "Error")
                Exit Sub
            End If
        Catch ex As Exception
            'log.Error("Start Error WifiConectionCheck Shipment Scan method myScanner_OnDone")
            If Err.Number = 5 Then
                'log.Error("Additional Message: Koneksi Wifi di HT tertutup." & vbCrLf & _
                '    "Tunggu beberapa detik dan ulangi lagi.")
                'log.Error("Error Number: " & Err.Number & vbCrLf & "Error Description: " & Err.Description & vbCrLf, ex)
                'log.Error("End Error WifiConectionCheck Shipment Filter")

                DisplayMessage.ErrorMsg("Koneksi Wifi di HT tertutup." & vbCrLf & _
                    "Tunggu beberapa detik dan ulangi lagi.", "Error")
                Dim MyRf As RF
                MyRf = New RF()
                MyRf.OpenMode = RF.EN_OPEN_MODE.CONTINUOUSLY
                MyRf.Open = True
                Exit Sub
            End If
            'log.Error("Error Number: " & Err.Number & vbCrLf & "Error Description: " & Err.Description & vbCrLf, ex)
            'log.Error("End Error WifiConectionCheck Shipment Scan method myScanner_OnDone")
        End Try
        'LogManager.Shutdown()

        'Dim obj_TempScanDate As Date = Date.Now

        Dim scannedQRCode As String = String.Empty

        'obj_TempScanDate = obj_TempScanDate.AddSeconds(1)
        'Dim lstr_ScannedDate As String = obj_TempScanDate.ToString("yyyy-MM-dd HH:mm:ss")

        Try
            scannedQRCode = myScanner.Input(Scanner.ALL_BUFFER).Trim
        Catch ex As Exception
            ResetScannerAndShowErrorMessage(ex.Message, "Scan Error")
        End Try

        If Not processScanData Or scannedQRCode.Length = 0 Then
            Exit Sub
        End If

        processScanData = False

        Dim splitQRCode As String() = scannedQRCode.Split(New Char() {";"c})

        BHTController.SoundOK()

        str_tmpSplitQr.Add(splitQRCode)

        Dim productionAct As ProductionAct = Nothing
        Dim actIdOfDeletedShipment As Integer = 0

        If Not ValidateScannedData( _
            splitQRCode, _
            productionAct, _
            actIdOfDeletedShipment _
            ) Then
            processScanData = True
            Exit Sub
        End If

        '' Validate success, process scanned data

        scannerOnDuration = 0

        _scannedBarcodes.Add(splitQRCode(FinalQrCodeValues.BarcodeTag))

        Dim singleShipActData(13) As String

        singleShipActData(ShipmentActValues.SID) = sid
        singleShipActData(ShipmentActValues.SONumber) = soNumber
        singleShipActData(ShipmentActValues.CustomerCode) = customerCode
        singleShipActData(ShipmentActValues.PlantNo) = plantNo
        singleShipActData(ShipmentActValues.ShipmentDate) = Now.ToString( _
            "yyyy-MM-dd HH:mm:ss" _
            )
        singleShipActData(ShipmentActValues.UserID) = ( _
            TemporaryData.loggedInUserID _
            )
        singleShipActData(ShipmentActValues.BarcodeTag) = splitQRCode( _
            FinalQrCodeValues.BarcodeTag _
            )
        singleShipActData(ShipmentActValues.ItemProductionDate) = ( _
            productionAct.PRODDATE.ToString("yyyy-MM-dd HH:mm:ss") _
            )
        singleShipActData(ShipmentActValues.TrinPartCode) = trinPartNo
        singleShipActData(ShipmentActValues.OKNG) = "1"
        singleShipActData(ShipmentActValues.ActQty) = splitQRCode( _
            FinalQrCodeValues.ActQty _
            )
        singleShipActData(ShipmentActValues.ActIdOfDeletedData) = ( _
            actIdOfDeletedShipment.ToString() _
            )
        singleShipActData(ShipmentActValues.LineCode) = ( _
            productionAct.LINECODE _
            )
        singleShipActData(ShipmentActValues.LabelPartName) = ( _
           Dns.GetHostEntry(Dns.GetHostName()).AddressList(0).ToString() _
           )

        ' actIdOfDeletedShipment = -1 ->
        ' previous item shipment has been deleted and re-scanned
        If actIdOfDeletedShipment = -1 Then
            _tmpShipActDataInsert.Add(singleShipActData)
        Else
            _tmpShipActDataUpdate.Add(singleShipActData)
        End If

        _lastScannedBarcode = splitQRCode(FinalQrCodeValues.BarcodeTag)
        textBoxScannedTag.Text = _lastScannedBarcode

        _lastScannedQty = splitQRCode(FinalQrCodeValues.ActQty)

        _actualQty += CInt(splitQRCode(FinalQrCodeValues.ActQty))

        'add by lutfi 9f
        _totalScannedQtyF += CInt(_lastScannedQty)

        labelActualQtyValue.Text = String.Format("{0:n0}", _actualQty)
        labelRemainingQtyValue.Text = String.Format( _
            "{0:n0}", _
            planQty - _actualQty _
            )

        '********************************** Add Sqlite process *************************************
        '************************************ Adri_[20170329] **************************************

        Try
            subGetSqliteData(_lastScannedBarcode, _
                             scannedQRCode, _
                             trinPartNo, _
                             _lastScannedQty, _
                             sid, _
                             soNumber, _
                             customerCode, _
                             plantNo _
                             )

            If SqliteControllerDB.fncInsert(mstr_SqliteTable, mobj_SqliteData) Then
                mobj_SqliteData.Clear()
                If _actualQty = planQty Then
                    SetScannerStatusOff()
                    DisableScanner()
                    SaveScannedData()
                End If
                processScanData = True
                Exit Sub
            Else
                DisplayMessage.ErrorMsg("Temporary data save failed!", _
                                        "Error" _
                                        )
                mobj_SqliteData.Clear()
            End If
        Catch ex As SQLiteException
            DisplayMessage.ErrorMsg(ex.Message, "Error")
        End Try

        '*******************************************************************************************

        processScanData = True
    End Sub

    Private Function ProcessSavingDataToDB() As Boolean
        ShowNowLoadingAndDisableControls()

        Dim ActQty As Integer = GetSummarySIDAndActQty(sid)
        'Dim ActQty As Integer = GetSummarySIDAndActQty(soNumber)

        If CInt(labelPlanQtyValue.Text) < (ActQty + _totalScannedQtyF) Then
            ResetScannerAndShowErrorMessage( _
                "Scan Qty sudah melebihi Plan Qty. " & _
                "Semua yang anda scan untuk SO No. ini akan terhapus, tolong scan ulang SO No. ini", _
                "Pesan" _
                )
            SqliteControllerDB.fncDelete(mstr_SqliteTable, "SONumber = '" & soNumber & "'")
            Return False
        End If


        Using connection As IDbConnection = New MySqlConnection( _
               CommonLib.GenerateConnectionString _
               )
            Dim transaction As IDbTransaction = Nothing

            Try
                connection.Open()

                ' Updating data in ShipmentAct table
                If _tmpShipActDataUpdate.Count > 0 Then
                    Dim exceptionMsg As String = String.Empty

                    If Not ShipmentActDB.UpdateData(connection, transaction, _
                                _tmpShipActDataUpdate, _
                                0, _
                                exceptionMsg _
                                ) Then
                        CloseNowLoadingAndEnableControls()

                        If exceptionMsg.Length > 0 Then
                            ResetScannerAndShowErrorMessage(exceptionMsg, "DB Error")
                        End If

                        Return False
                    Else
                        If ShipmentPlanDB.fncCheckStockTakeFlag(sid) = 1 Then
                            fncInsertStockTakeTemp( _
                                _tmpShipActDataUpdate, _
                                 exceptionMsg _
                            )
                        End If
                    End If
                End If

                ' Inserting data to ShipmentAct table
                Dim newActIds As List(Of Integer) = Nothing

                If _tmpShipActDataInsert.Count > 0 Then
                    Dim exceptionMsg As String = String.Empty
                    'xxxx
                    'Dim bolRes As Boolean
                    newActIds = ShipmentActDB.InsertData(connection, transaction, _
                        _tmpShipActDataInsert, _
                        exceptionMsg _
                        )

                    If exceptionMsg.Length > 0 Then

                        CloseNowLoadingAndEnableControls()
                        ResetScannerAndShowErrorMessage(exceptionMsg, "DB Error")

                        exceptionMsg = String.Empty

                        ShowNowLoadingAndDisableControls()

                        If _tmpShipActDataUpdate.Count > 0 Then
                            ' Rollback update on Shipment Act because of failure to 
                            ' update Stock Card table by resetting delFlg to 1
                            ShipmentActDB.UpdateData(connection, transaction, _
                                _tmpShipActDataUpdate, _
                                1, _
                                exceptionMsg _
                                )
                        End If

                        ' Rollback previous insert by deleting inserted data
                        ' Rollback must be done manually
                        ShipmentActDB.DeleteData(newActIds, exceptionMsg)
                        newActIds.Clear()

                        CloseNowLoadingAndEnableControls()

                        If exceptionMsg.Length > 0 Then
                            ResetScannerAndShowErrorMessage(exceptionMsg, "DB Error")
                        End If

                        Return False
                    End If

                    'If ShipmentPlanDB.fncCheckStockTakeFlag(sid) = 1 Then
                    '    fncInsertStockTakeTemp( _
                    '        _tmpShipActDataInsert, _
                    '         exceptionMsg _
                    '    )
                    'End If
                End If

                For index As Integer = 0 To _tmpShipActDataInsert.Count - 1
                    ' When insert is ignored because of duplicate data
                    ' newActId = 0
                    If newActIds(index) = 0 Then
                        _actualQty -= CInt( _
                            _tmpShipActDataInsert(index)(ShipmentActValues.ActQty) _
                            )

                        Continue For
                    End If

                    ' Prepare a list of data to be inserted into Stock Card table
                    Dim stockCardData() As String = CompileStockCardData( _
                        _tmpShipActDataInsert(index), _
                        index, _
                        newActIds(index).ToString() _
                        )
                    _tmpStockCardDataInsert.Add(stockCardData)
                Next

                For index As Integer = 0 To _tmpShipActDataUpdate.Count - 1
                    ' Prepare a list of data to update data in Stock Card table
                    Dim stockCardData() As String = CompileStockCardData( _
                        _tmpShipActDataUpdate(index), _
                        index _
                        )
                    _tmpStockCardDataUpdate.Add(stockCardData)
                Next

                If _tmpShipActDataInsert.Count > 0 Then
                    Dim exceptionMsg As String = String.Empty
                    'xxxx
                    If Not StockCardDB.InsertDataForShipment(connection, transaction, _
                        _tmpStockCardDataInsert, _
                        exceptionMsg _
                        ) Then
                        If exceptionMsg.Length > 0 Then
                            CloseNowLoadingAndEnableControls()
                            ResetScannerAndShowErrorMessage(exceptionMsg, "DB Error")
                            ShowNowLoadingAndDisableControls()
                        End If

                        exceptionMsg = String.Empty

                        'If _tmpShipActDataUpdate.Count > 0 Then
                        '    ' Rollback update on Shipment Act because of failure to 
                        '    ' update Stock Card table by resetting delFlg to 1
                        '    ShipmentActDB.UpdateData(connection, transaction, _
                        '        _tmpShipActDataUpdate, _
                        '        1, _
                        '        exceptionMsg _
                        '        )
                        'End If

                        ' Rollback inserted data in ShipmentAct because failure to
                        ' insert data to Stock Card table
                        'ShipmentActDB.DeleteData(newActIds, exceptionMsg)

                        newActIds.Clear()

                        CloseNowLoadingAndEnableControls()

                        If exceptionMsg.Length > 0 Then
                            ResetScannerAndShowErrorMessage(exceptionMsg, "DB Error")
                        End If

                        ' Clear data to avoid duplicate when user trying to re-save
                        _tmpStockCardDataInsert.Clear()
                        _tmpStockCardDataUpdate.Clear()

                        Return False
                    End If
                End If

                If _tmpShipActDataUpdate.Count > 0 Then
                    Dim exceptionMsg As String = String.Empty

                    If Not StockCardDB.UpdateDataForShipment(connection, transaction, _
                        _tmpStockCardDataUpdate, _
                        exceptionMsg _
                        ) Then
                        If exceptionMsg.Length > 0 Then
                            CloseNowLoadingAndEnableControls()
                            ResetScannerAndShowErrorMessage(exceptionMsg, "DB Error")
                            ShowNowLoadingAndDisableControls()
                        End If

                        exceptionMsg = String.Empty

                        ' Rollback update on Shipment Act because of failure to 
                        ' update Stock Card table by resetting delFlg to 1
                        'ShipmentActDB.UpdateData(connection, transaction, _
                        '    _tmpShipActDataUpdate, _
                        '    1, _
                        '    exceptionMsg _
                        '    )

                        ' Rollback inserted data in ShipmentAct and Stock Card tables 
                        ' because failure to update Stock Card data table 
                        ' by deleting them
                        'ShipmentActDB.DeleteData(newActIds, exceptionMsg)
                        'StockCardDB.DeleteData(newActIds, exceptionMsg)
                        newActIds.Clear()

                        CloseNowLoadingAndEnableControls()

                        If exceptionMsg.Length > 0 Then
                            ResetScannerAndShowErrorMessage(exceptionMsg, "DB Error")
                        End If

                        ' Clear data to avoid duplicate when user trying to re-save
                        _tmpStockCardDataInsert.Clear()
                        _tmpStockCardDataUpdate.Clear()

                        Return False
                    End If
                End If
                'transaction.Commit()
            Catch ex As Exception
                If ex.Message.Length > 0 Then
                    ResetScannerAndShowErrorMessage(ex.Message.ToString, "DB Error")
                End If


                Return False
            End Try
        End Using
        ' Update planQty and ActQty on the data grid in shipmentList window.

        shipmentList.DataGrid1( _
            currentRowOnDataGrid, _
            shipmentList.DataGridColumns.PlanQty _
            ) = String.Format("{0:n0}", planQty)
        shipmentList.DataGrid1( _
            currentRowOnDataGrid, _
            shipmentList.DataGridColumns.ActQty _
            ) = String.Format("{0:n0}", _actualQty)

        Dim shipmentStatus As String = CommonLib.GetStockStatus( _
            planQty, _
            _actualQty _
            )
        shipmentList.DataGrid1( _
            currentRowOnDataGrid, _
            shipmentList.DataGridColumns.Status _
            ) = shipmentStatus
        'MsgBox(shipmentList.DataGrid1.CurrentRowIndex)

        shipmentList.dataSource.AcceptChanges()


        ResetScanRelatedData()

        If planQty > _actualQty Then
            LoadDefaultDisplayedValues()
        End If

        CloseNowLoadingAndEnableControls()

        ResetScannerAndShowInfoMessage( _
            "Data Telah Tersimpan di Database", _
            "Sukses" _
            )

        Return True
    End Function

    Private Function CompileStockCardData( _
        ByVal shipActData As String(), _
        ByVal counter As Integer, _
              Optional ByVal newActID As String = "" _
              ) As String()
        Dim stockCardData(7) As String

        stockCardData(StockCardValues.TrinPartCode) = trinPartNo

        If newActID.Length > 0 Then
            stockCardData(StockCardValues.ActID) = newActID
        Else
            stockCardData(StockCardValues.ActID) = ( _
                shipActData(ShipmentActValues.ActIdOfDeletedData) _
                )
        End If

        stockCardData(StockCardValues.ActQty) = ( _
            shipActData(ShipmentActValues.ActQty) _
            )
        stockCardData(StockCardValues.TypeID) = "2"
        stockCardData(StockCardValues.BarcodeTag) = ( _
            shipActData(ShipmentActValues.BarcodeTag) _
            )
        stockCardData(StockCardValues.LineCode) = ( _
            shipActData(ShipmentActValues.LineCode) _
            )
        stockCardData(StockCardValues.ScanDateTime) = shipActData(ShipmentActValues.ShipmentDate)
        stockCardData(StockCardValues.UserID) = ( _
            TemporaryData.loggedInUserID _
            )

        Return stockCardData
    End Function

    Private Sub timerScanner_Tick( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
        ) Handles timerScanner.Tick
        If scannerOnDuration < 10 Then
            scannerOnDuration += 1
        Else
            SetScannerStatusOff()
            DisableScanner()
            EnableScanner()
        End If
    End Sub

    Private Sub EnableFormControls(ByVal enable As Boolean)
        buttonSave.Enabled = enable
        buttonBack.Enabled = enable
        buttonReset.Enabled = enable
        processKeypadIsEnabled = enable
    End Sub

    Private Sub ResetScannerAndShowInfoMessage( _
        ByVal errorMsg As String, _
        ByVal errorTitle As String _
        )
        SetScannerStatusOff()
        DisableScanner()
        DisplayMessage.OkMsg(errorMsg, errorTitle)
        EnableScanner()
    End Sub

    Private Sub ResetScannerAndShowErrorMessage( _
        ByVal errorMsg As String, _
        ByVal errorTitle As String _
        )
        SetScannerStatusOff()
        DisableScanner()
        DisplayMessage.ErrorMsg(errorMsg, errorTitle)
        EnableScanner()
    End Sub

    Private Function ResetScannerAndShowConfirmationMessage( _
        ByVal confirmationMsg As String, _
        ByVal confirmationTitle As String _
        ) As Boolean
        SetScannerStatusOff()
        DisableScanner()

        Dim confirm As Boolean = DisplayMessage.ConfirmationDialog( _
            confirmationMsg, _
            confirmationTitle _
            )
        EnableScanner()

        Return confirm
    End Function

    Private Sub subGetSqliteData(ByVal str_Barcode As String, _
                                 ByVal str_QrCode As String, _
                                 ByVal str_TrinPartNo As String, _
                                 ByVal str_ScannedQty As String, _
                                 ByVal str_ShipId As String, _
                                 ByVal str_SoNumber As String, _
                                 ByVal str_CustCode As String, _
                                 ByVal str_PlantNo As String _
                                 )
        Try
            mobj_SqliteData.Add("SID", str_ShipId)
            mobj_SqliteData.Add("SONumber", str_SoNumber)
            mobj_SqliteData.Add("CustCode", str_CustCode)
            mobj_SqliteData.Add("PlantNo", str_PlantNo)
            mobj_SqliteData.Add("BarcodeTag", str_Barcode)
            mobj_SqliteData.Add("QrCode", str_QrCode)
            mobj_SqliteData.Add("TrinPartNo", str_TrinPartNo)
            mobj_SqliteData.Add("ActQty", str_ScannedQty)
            mobj_SqliteData.Add("InputFlag", mint_InputFlag.ToString)
        Catch ex As Exception
            DisplayMessage.ErrorMsg(ex.ToString, "Error")
            Exit Sub
        End Try

    End Sub

    Public Function ValidateData(ByVal QRBarcodetag As List(Of String), ByRef ErrMessage As String) As Boolean
        For int_counter As Integer = 0 To QRBarcodetag.Count - 1

            Dim exceptionMsg As String = String.Empty
            Dim balanceQty As Integer = StockCardSummaryDB.GetStockBalanceQty( _
                QRBarcodetag(int_counter), _
                exceptionMsg _
                )

            If balanceQty <= 0 Then
                'CloseNowLoadingAndEnableControls()

                If exceptionMsg.Length > 0 Then
                    ResetScannerAndShowErrorMessage( _
                        exceptionMsg, _
                        "DB Error" _
                        )
                Else
                    Dim errorMsg As String = String.Format( _
                        "Tidak Terdapat Sisa Stock Untuk QR Code {0} di Sistem", _
                        QRBarcodetag(int_counter) _
                        )
                    ErrMessage = errorMsg
                    Return False
                    'ResetScannerAndShowErrorMessage( _
                    '    errorMsg, _
                    '    "Error" _
                    ')

                End If

            End If
        Next
        Return True
    End Function

End Class


'***************  FUTURE RELEASE ***********************

'Option Strict On
'Option Explicit On

'Imports System.Globalization

'Imports DNWA.BHTCL

'Imports System.Data.SQLite
'Imports System.Data
'Imports System.Linq

'Imports TradNewSystem.Helpers
'Imports TradNewSystem.Model
'Imports TradNewSystem.PocoClass


'Public Class ShipmentScan
'    Protected Friend nowLoadingPopUp As NowLoading

'    Private shipmentList As ShipmentList
'    Private trinPartNo As String
'    Private customerCode As String
'    Private planQty As Integer
'    Private _actualQty As Integer
'    Private _totalScannedQty As Integer
'    Private sid As String
'    Private soNumber As String
'    Private plantNo As String

'    Private _scannedBarcodes As New List(Of String)
'    Private _tmpShipActDataInsert As New List(Of String())
'    Private _tmpShipActDataUpdate As New List(Of String())
'    Private _tmpStockCardDataInsert As New List(Of String())
'    Private _tmpStockCardDataUpdate As New List(Of String())
'    Private _tmpScannedDate As New List(Of String)

'    Private str_tmpSplitBarcode As New List(Of String)
'    Private str_tmpScannedQrcode As New List(Of String)
'    Private str_tmpScannedQty As New List(Of String)
'    Private int_tmpActIdList As New List(Of Integer)
'    Private int_TmpScannedQtyList As New List(Of Integer)
'    Private str_TmpDeletedActId As New List(Of String)

'    Private _lastScannedBarcode As String
'    Private _lastScannedQty As String

'    Private currentRowOnDataGrid As Integer
'    Private scannerOnDuration As Integer
'    Private scannerIsOn As Boolean
'    Private scannerIsEnabled As Boolean
'    Private processKeypadIsEnabled As Boolean
'    Private processScanData As Boolean

'    Private mstr_SqliteTable As String = "TempScannedData"
'    Private mint_InputFlag As Integer = 2
'    Private mobj_SqliteData As New Dictionary(Of String, String)()
'    Private cint_TmpScannedTotalQty As Integer

'    Friend WithEvents myScanner As Scanner

'#Region "Properties"
'    Public Property scannedBarcodes() As List(Of String)
'        Get
'            Return _scannedBarcodes
'        End Get

'        Set(ByVal value As List(Of String))
'            _scannedBarcodes = value
'        End Set
'    End Property

'    Public Property tmpShipActDataInsert() As List(Of String())
'        Get
'            Return _tmpShipActDataInsert
'        End Get

'        Set(ByVal value As List(Of String()))
'            _tmpShipActDataInsert = value
'        End Set
'    End Property

'    Public Property tmpShipActDataUpdate() As List(Of String())
'        Get
'            Return _tmpShipActDataUpdate
'        End Get

'        Set(ByVal value As List(Of String()))
'            _tmpShipActDataUpdate = value
'        End Set
'    End Property

'    Public Property tmpStockCardDataInsert() As List(Of String())
'        Get
'            Return _tmpStockCardDataInsert
'        End Get

'        Set(ByVal value As List(Of String()))
'            _tmpStockCardDataInsert = value
'        End Set
'    End Property

'    Public Property tmpStockCardDataUpdate() As List(Of String())
'        Get
'            Return _tmpStockCardDataUpdate
'        End Get

'        Set(ByVal value As List(Of String()))
'            _tmpStockCardDataUpdate = value
'        End Set
'    End Property

'    Public Property lastScannedBarcode() As String
'        Get
'            Return _lastScannedBarcode
'        End Get

'        Set(ByVal value As String)
'            _lastScannedBarcode = value
'        End Set
'    End Property

'    Public Property actualQty() As Integer
'        Get
'            Return _actualQty
'        End Get

'        Set(ByVal value As Integer)
'            _actualQty = value
'        End Set
'    End Property

'    Public Property totalScannedQty() As Integer
'        Get
'            Return _totalScannedQty
'        End Get

'        Set(ByVal value As Integer)
'            _totalScannedQty = value
'        End Set
'    End Property
'#End Region

'    Public Sub New(ByVal shipmentList As ShipmentList)
'        InitializeComponent()

'        Me.shipmentList = shipmentList
'        currentRowOnDataGrid = shipmentList.DataGrid1.CurrentCell.RowNumber

'        customerCode = shipmentList.DataGrid1( _
'            currentRowOnDataGrid, _
'            shipmentList.DataGridColumns.CustomerCode _
'            ).ToString()
'        plantNo = shipmentList.DataGrid1( _
'            currentRowOnDataGrid, _
'            shipmentList.DataGridColumns.PlantNo _
'            ).ToString()
'        sid = shipmentList.DataGrid1( _
'            currentRowOnDataGrid, _
'            shipmentList.DataGridColumns.SID _
'            ).ToString()
'        soNumber = shipmentList.DataGrid1( _
'            currentRowOnDataGrid, _
'            shipmentList.DataGridColumns.SONumber _
'            ).ToString()
'        trinPartNo = shipmentList.DataGrid1( _
'            currentRowOnDataGrid, _
'            shipmentList.DataGridColumns.TrinPartNo _
'            ).ToString()

'        labelTrinPartCodeValue.Text = trinPartNo
'        LoadDefaultDisplayedValues()
'    End Sub

'    Private Sub LoadDefaultDisplayedValues()
'        Dim thousandSeparatedPlanQty As String = shipmentList.DataGrid1( _
'            currentRowOnDataGrid, _
'            shipmentList.DataGridColumns.PlanQty _
'            ).ToString()
'        Dim thousandSeparatedActualQty As String = shipmentList.DataGrid1( _
'            currentRowOnDataGrid, _
'            shipmentList.DataGridColumns.ActQty _
'            ).ToString()

'        Dim currentCulture As CultureInfo = CultureInfo.CurrentCulture

'        cint_TmpScannedTotalQty = 0

'        Dim sql_Query As String = String.Format("select BarcodeTag, QrCode, ScanQty from {0} where TrinPartNo = '{1}' and InputFlag = '{2}';", _
'                                                mstr_SqliteTable, _
'                                                trinPartNo, _
'                                                mint_InputFlag _
'                                                )

'        Dim dt_SqliteData As DataTable = SqliteControllerDB.fncGetDatatable(sql_Query)

'        If dt_SqliteData.Rows.Count > 0 Then
'            Dim productionAct As ProductionAct = Nothing

'            Dim bool_Confirm As Boolean = DisplayMessage.ConfirmationDialog("Terdapat Data yang belum diproses, ingin menggunakan?", _
'                                                                            "Konfirmasi" _
'                                                                            )
'            If bool_Confirm Then
'                str_tmpSplitBarcode = (From dr_Sqlite In dt_SqliteData.AsEnumerable() Select dr_Sqlite.Field(Of String)(0)).ToList()
'                str_tmpScannedQrcode = (From dr_Sqlite In dt_SqliteData.AsEnumerable() Select dr_Sqlite.Field(Of String)(1)).ToList()
'                str_tmpScannedQty = (From dr_Sqlite In dt_SqliteData.AsEnumerable() Select dr_Sqlite.Field(Of String)(2)).ToList()

'                int_TmpScannedQtyList = str_tmpScannedQty.ConvertAll(AddressOf Int32.Parse)

'                For Each obj_TmpScannedQtyItems In int_TmpScannedQtyList
'                    cint_TmpScannedTotalQty += obj_TmpScannedQtyItems
'                Next

'                For int_Counter As Integer = 0 To str_tmpScannedQrcode.Count - 1
'                    productionAct = ProductionActDB.GetProdDateLineCode( _
'                    str_tmpScannedQrcode(int_Counter), _
'                    True _
'                    )
'                Next

'                subSetScannedDataForShipment(productionAct, _
'                                     str_tmpSplitBarcode, _
'                                     int_TmpScannedQtyList _
'                                     )

'            Else
'                SqliteControllerDB.fncClearData(mstr_SqliteTable)
'            End If
'        End If

'        planQty = CInt(Convert.ToDouble( _
'            thousandSeparatedPlanQty, _
'            currentCulture _
'            ))
'        _actualQty = CInt(Convert.ToDouble( _
'            thousandSeparatedActualQty, _
'            currentCulture _
'            ))

'        If str_tmpScannedQty.Count > 0 Then
'            _actualQty += cint_TmpScannedTotalQty
'        End If

'        labelPlanQtyValue.Text = String.Format("{0:n0}", planQty)
'        labelActualQtyValue.Text = String.Format("{0:n0}", _actualQty)
'        labelRemainingQtyValue.Text = String.Format( _
'            "{0:n0}", _
'            planQty - _actualQty _
'            )

'        _lastScannedBarcode = String.Empty
'        textBoxScannedTag.Text = String.Empty
'    End Sub

'    Private Sub ShipmentScan_Load( _
'        ByVal sender As Object, _
'        ByVal e As EventArgs _
'        ) Handles MyBase.Load
'        scannerIsOn = False
'        processKeypadIsEnabled = True
'        processScanData = True
'        EnableScanner()
'    End Sub

'    Private Sub ShipmentScan_Closed( _
'        ByVal sender As Object, _
'        ByVal e As System.EventArgs _
'        ) Handles MyBase.Closed
'        timerScanner.Enabled = False
'        DisableScanner()
'    End Sub

'#Region "Key Press"
'    Private Sub ActivateCommonShortcut(ByVal e As KeyEventArgs)
'        If Not processKeypadIsEnabled Then
'            Exit Sub
'        End If

'        Select Case e.KeyCode
'            Case Windows.Forms.Keys.F1
'                SaveScannedData()
'            Case Windows.Forms.Keys.F2
'                CloseWindow()
'            Case Windows.Forms.Keys.F3
'                ResetForm()
'        End Select
'    End Sub

'    Private Sub UndoChangeOfTextBoxScanTag()
'        textBoxScannedTag.Text = _lastScannedBarcode
'        textBoxScannedTag.SelectionStart = 0
'        textBoxScannedTag.SelectionLength = 0
'    End Sub

'    Private Sub ButtonFullScreen_KeyUp( _
'        ByVal sender As Object, _
'        ByVal e As KeyEventArgs _
'        ) Handles ButtonFullScreen.KeyUp
'        ActivateCommonShortcut(e)
'    End Sub

'    Private Sub ShipmentScan_KeyUp( _
'        ByVal sender As Object, _
'        ByVal e As KeyEventArgs _
'        ) Handles MyBase.KeyUp
'        ActivateCommonShortcut(e)
'    End Sub

'    Private Sub TextBoxScanTag_KeyUp( _
'        ByVal sender As Object, _
'        ByVal e As KeyEventArgs _
'        ) Handles textBoxScannedTag.KeyUp
'        ActivateCommonShortcut(e)

'        If e.KeyCode <> Keys.SCAN _
'            Or e.KeyCode <> Keys.SCAN2 _
'            Or e.KeyCode <> Keys.SCAN3 Then
'            UndoChangeOfTextBoxScanTag()
'        End If
'    End Sub

'    Private Sub TextBoxScanTag_KeyDown( _
'        ByVal sender As Object, _
'        ByVal e As KeyEventArgs _
'        ) Handles textBoxScannedTag.KeyDown
'        If e.KeyCode = Keys.SCAN _
'            Or e.KeyCode = Keys.SCAN2 _
'            Or e.KeyCode = Keys.SCAN3 Then
'            If Not scannerIsEnabled Then
'                Exit Sub
'            End If

'            If Not scannerIsOn Then
'                SetScannerStatusOn()
'            Else
'                SetScannerStatusOff()
'            End If
'        Else
'            UndoChangeOfTextBoxScanTag()
'        End If
'    End Sub

'    Private Sub ButtonSave_KeyUp( _
'        ByVal sender As Object, _
'        ByVal e As KeyEventArgs _
'        ) Handles buttonSave.KeyUp
'        ActivateCommonShortcut(e)
'    End Sub

'    Private Sub ButtonBack_KeyUp( _
'        ByVal sender As Object, _
'        ByVal e As KeyEventArgs _
'        ) Handles buttonBack.KeyUp
'        ActivateCommonShortcut(e)
'    End Sub

'    Private Sub ButtonReset_KeyUp( _
'        ByVal sender As Object, _
'        ByVal e As KeyEventArgs _
'        ) Handles buttonReset.KeyUp
'        ActivateCommonShortcut(e)
'    End Sub
'#End Region

'#Region "Button Click"
'    Private Sub ButtonFullScreen_Click( _
'        ByVal sender As Object, _
'        ByVal e As EventArgs _
'        ) Handles ButtonFullScreen.Click
'        ToggleTaskbar.EnableDisable()
'    End Sub

'    Private Sub ButtonSave_Click( _
'        ByVal sender As Object, _
'        ByVal e As EventArgs _
'        ) Handles buttonSave.Click
'        SaveScannedData()
'    End Sub

'    Private Sub ButtonBack_Click( _
'        ByVal sender As Object, _
'        ByVal e As EventArgs _
'        ) Handles buttonBack.Click
'        CloseWindow()
'    End Sub

'    Private Sub ButtonReset_Click( _
'        ByVal sender As Object, _
'        ByVal e As EventArgs _
'        ) Handles buttonReset.Click
'        ResetForm()
'    End Sub
'#End Region

'    Private Sub SaveScannedData()
'        If str_tmpSplitBarcode.Count > 0 Then
'            Dim confirm As Boolean = ResetScannerAndShowConfirmationMessage( _
'                "Save Data Yang Telah Di Scan ke Database?", _
'                "Konfirmasi" _
'                )

'            If confirm = True Then
'                '********************************************************
'                'Add Validation Check for when HT scan but not yet saved,
'                'and WEB operator delete the shipment plan
'                '[Update] : 2017/02/10 _ Adri
'                '********************************************************
'                If ShipmentActDB.fncCheckSID(sid) Then
'                    ProcessSavingDataToDB()
'                Else
'                    ResetScannerAndShowErrorMessage("Shipment Plan Telah Di Hapus Dari Sistem, " & _
'                                                    "Silahkan Pilih Kembali Plan Baru", _
'                                                    "Error" _
'                                                   )
'                    ResetScanRelatedData()
'                    LoadDefaultDisplayedValues()
'                End If
'                '********************************************************
'                'End Update
'                '********************************************************
'            End If
'        Else
'            ResetScannerAndShowErrorMessage( _
'                "Tidak Ada Scan Data Untuk Di Simpan di Database", _
'                "Error" _
'                )
'        End If
'    End Sub

'    Private Sub CloseWindow()
'        If _scannedBarcodes.Count = 0 Then
'            Close()
'        Else
'            Dim confirm As Boolean = ResetScannerAndShowConfirmationMessage( _
'                "Data Yang Telah Di Scan Belum Tersimpan " & _
'                    "di Database. Lanjut?", _
'                "Konfirmasi" _
'                )

'            If confirm = True Then
'                Close()
'            End If
'        End If
'    End Sub

'    Private Sub ResetForm()
'        If _scannedBarcodes.Count = 0 Then
'            ResetScannerAndShowErrorMessage( _
'                "Tidak Ada Data Untuk Dihapus", _
'                "Error" _
'                )
'        Else
'            Dim confirm As Boolean = ResetScannerAndShowConfirmationMessage( _
'                "Apakah Anda Ingin Menghapus Data Yang Telah Di Scan?", _
'                "Konfirmasi" _
'                )

'            If confirm = True Then
'                ResetScanRelatedData()
'                LoadDefaultDisplayedValues()
'            End If
'        End If
'    End Sub

'    Private Sub ResetScanRelatedData()
'        mobj_SqliteData.Clear()
'        int_tmpActIdList.Clear()
'        int_TmpScannedQtyList.Clear()
'        str_TmpDeletedActId.Clear()
'        str_tmpSplitBarcode.Clear()
'        str_tmpScannedQrcode.Clear()
'        _scannedBarcodes.Clear()
'        _tmpShipActDataInsert.Clear()
'        _tmpShipActDataUpdate.Clear()
'        _tmpStockCardDataInsert.Clear()
'        _tmpStockCardDataUpdate.Clear()
'        _tmpScannedDate.Clear()
'        _totalScannedQty = 0
'    End Sub

'    Private Sub EnableScanner()
'        BHTController.InitialiseScanner( _
'            myScanner, _
'            ScannerCodeType.QrCode, _
'            ScannerReadMode.Alternate _
'            )
'        scannerIsEnabled = True
'    End Sub

'    Private Sub DisableScanner()
'        BHTController.DisposeScanner(myScanner)
'        scannerIsEnabled = False
'    End Sub

'    Private Sub SetScannerStatusOn()
'        scannerIsOn = True
'        timerScanner.Enabled = True
'        scannerOnDuration = 0
'    End Sub

'    Private Sub SetScannerStatusOff()
'        scannerIsOn = False
'        timerScanner.Enabled = False
'    End Sub

'#Region "Scanned Data Validations"
'    Private Function ValidateScannedData( _
'        ByVal splitQrCode As String() _
'        ) As Boolean
'        ShowNowLoadingAndDisableControls()

'        If Not ScanValidator.isValidFinalQrCode(splitQrCode) Then
'            CloseNowLoadingAndEnableControls()
'            ResetScannerAndShowErrorMessage( _
'                "QR Code Tidak Valid!", _
'                "Scan Error" _
'            )
'            Return False
'        End If

'        ' Match TRIN Part No
'        Dim trinCodeIsMatched As Boolean = ( _
'            ScanValidator.IsProductTrinCodeMatched( _
'                myScanner, _
'                timerScanner, _
'                scannerIsOn, _
'                trinPartNo, _
'                splitQrCode(FinalQrCodeValues.TrinPartCode) _
'                ) _
'            )

'        If Not trinCodeIsMatched Then
'            CloseNowLoadingAndEnableControls()
'            ResetScannerAndShowErrorMessage( _
'                "TRIN Part No Tidak Sama!", _
'                "Error" _
'                )
'            Return False
'        End If

'        ' QR Code has been scanned only
'        Dim barcodeHasBeenScanned As Boolean = ( _
'            ScanValidator.HasBarcodeTagBeenScanned( _
'                myScanner, _
'                timerScanner, _
'                scannerIsOn, _
'                str_tmpSplitBarcode, _
'                splitQrCode(FinalQrCodeValues.BarcodeTag) _
'                ) _
'            )

'        If barcodeHasBeenScanned Then
'            CloseNowLoadingAndEnableControls()
'            ResetScannerAndShowErrorMessage( _
'                String.Format( _
'                    "QR Code [{0}] Telah Di Scan Sebelumnya!", _
'                    splitQrCode(FinalQrCodeValues.BarcodeTag) _
'                    ), _
'                "Error" _
'                )
'            Return False
'        End If

'        ' New ActQty < PlanQty
'        Dim newActQty As Integer = ( _
'            _actualQty _
'            + CInt(splitQrCode(FinalQrCodeValues.ActQty)) _
'            )

'        If newActQty > planQty Then
'            CloseNowLoadingAndEnableControls()

'            Dim errorMsg As String = String.Format( _
'                "Actual Qty = {0} Tidak Boleh Melebihi Plan Qty = {1}!", _
'                newActQty, _
'                planQty _
'                )
'            ResetScannerAndShowErrorMessage(errorMsg, "Error")

'            Return False
'        End If

'        CloseNowLoadingAndEnableControls()
'        Return True
'    End Function
'#End Region

'    Private Sub ShowNowLoadingAndDisableControls()
'        EnableFormControls(False)

'        nowLoadingPopUp = New NowLoading()
'        nowLoadingPopUp.Show()
'    End Sub

'    Private Sub CloseNowLoadingAndEnableControls()
'        EnableFormControls(True)

'        If Not IsNothing(nowLoadingPopUp) Then
'            nowLoadingPopUp.Close()
'            nowLoadingPopUp = Nothing
'        End If
'    End Sub

'    Private Sub myScanner_OnDone( _
'        ByVal sender As Object, _
'        ByVal e As EventArgs _
'        ) Handles myScanner.OnDone
'        textBoxScannedTag.Focus()

'        Dim scannedQRCode As String = String.Empty

'        Try
'            scannedQRCode = myScanner.Input(Scanner.ALL_BUFFER).Trim
'        Catch ex As Exception
'            ResetScannerAndShowErrorMessage(ex.Message, "Scan Error")
'        End Try

'        If Not processScanData Or scannedQRCode.Length = 0 Then
'            Exit Sub
'        End If

'        processScanData = False

'        Dim splitQRCode As String() = scannedQRCode.Split(New Char() {";"c})

'        BHTController.SoundOK()

'        If Not ValidateScannedData( _
'            splitQRCode _
'            ) Then
'            processScanData = True
'            Exit Sub
'        End If

'        '' Validate success, process scanned data

'        scannerOnDuration = 0

'        str_tmpScannedQrcode.Add(scannedQRCode)
'        str_tmpSplitBarcode.Add(splitQRCode(FinalQrCodeValues.BarcodeTag))
'        str_tmpScannedQty.Add(splitQRCode(FinalQrCodeValues.ActQty))

'        int_TmpScannedQtyList.Add(CInt(splitQRCode(FinalQrCodeValues.ActQty)))
'        _scannedBarcodes.Add(splitQRCode(FinalQrCodeValues.BarcodeTag))

'        _lastScannedQty = splitQRCode(FinalQrCodeValues.ActQty)
'        _lastScannedBarcode = splitQRCode(FinalQrCodeValues.BarcodeTag)
'        textBoxScannedTag.Text = _lastScannedBarcode

'        _actualQty += CInt(splitQRCode(FinalQrCodeValues.ActQty))
'        labelActualQtyValue.Text = String.Format("{0:n0}", _actualQty)
'        labelRemainingQtyValue.Text = String.Format( _
'            "{0:n0}", _
'            planQty - _actualQty _
'            )

'        '********************************** Add Sqlite process *************************************
'        '************************************ Adri_[20170314] **************************************

'        Try
'            subGetSqliteData(_lastScannedBarcode, scannedQRCode, trinPartNo, _lastScannedQty)

'            If SqliteControllerDB.fncInsert(mstr_SqliteTable, mobj_SqliteData) Then
'                mobj_SqliteData.Clear()
'                If _actualQty = planQty Then
'                    SetScannerStatusOff()
'                    DisableScanner()
'                    SaveScannedData()
'                End If
'                processScanData = True
'                Exit Sub
'            Else
'                DisplayMessage.ErrorMsg("Temporary data save failed!", _
'                                        "Error" _
'                                        )
'                mobj_SqliteData.Clear()
'            End If
'        Catch ex As SQLiteException
'            DisplayMessage.ErrorMsg(ex.Message, "Error")
'        End Try

'        '*******************************************************************************************

'        processScanData = True
'    End Sub

'    Private Function ProcessSavingDataToDB() As Boolean
'        ShowNowLoadingAndDisableControls()

'        Dim productionAct As ProductionAct = Nothing
'        Dim list_DeletedItems As List(Of ShipmentAct)

'        ' QR Code has been scanned and saved?
'        list_DeletedItems = ShipmentActDB.GetActIdOfDeletedData( _
'            str_tmpSplitBarcode _
'            )

'        For Each shipActData As ShipmentAct In list_DeletedItems
'            int_tmpActIdList.Add(shipActData.ACTID)
'        Next

'        If list_DeletedItems.Count = 0 Then
'            For int_Counter As Integer = 0 To str_tmpSplitBarcode.Count - 1
'                Dim itemHasBeenShipped As Boolean = ( _
'                    ScanValidator.HasItemBeenShipped( _
'                        myScanner, _
'                        timerScanner, _
'                        scannerIsOn, _
'                        str_tmpSplitBarcode(int_Counter) _
'                        ) _
'                    )

'                If itemHasBeenShipped Then
'                    CloseNowLoadingAndEnableControls()
'                    ResetScannerAndShowErrorMessage( _
'                        String.Format( _
'                            "QR Code [{0}] Telah Terdaftar Di Database!", _
'                            str_tmpSplitBarcode(int_Counter) _
'                            ), _
'                        "Error" _
'                        )
'                    SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '2'")
'                    ResetScanRelatedData()
'                    LoadDefaultDisplayedValues()
'                    CloseNowLoadingAndEnableControls()
'                    Return False
'                End If
'            Next
'        Else
'            Dim dr_Datarow As DataRow = Nothing

'            For Each obj_DeletedIds As ShipmentAct In list_DeletedItems
'                str_TmpDeletedActId.Add(obj_DeletedIds.ACTID.ToString)
'            Next
'        End If

'        ' Total quantity for given barcode in stockCard > 0?
'        Dim exMsg As String = String.Empty
'        Dim balanceQty As Integer = StockCardSummaryDB.GetStockBalanceQty( _
'            str_tmpSplitBarcode, _
'            exMsg _
'            )

'        If balanceQty <= 0 Then
'            CloseNowLoadingAndEnableControls()

'            If exMsg.Length > 0 Then
'                ResetScannerAndShowErrorMessage( _
'                    exMsg, _
'                    "DB Error" _
'                    )
'            Else
'                Dim errorMsg As String = String.Format( _
'                    "Tidak Terdapat Sisa Stock Untuk QR Code {0} di Sistem", _
'                    str_tmpSplitBarcode _
'                    )
'                SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '2'")
'                ResetScannerAndShowErrorMessage( _
'                    errorMsg, _
'                    "Error" _
'                )
'            End If
'            ResetScanRelatedData()
'            LoadDefaultDisplayedValues()
'            CloseNowLoadingAndEnableControls()
'            Return False
'        End If

'        ' QRCode is registered in productionAct?
'        For int_Counter As Integer = 0 To str_tmpScannedQrcode.Count - 1

'            productionAct = ProductionActDB.GetProdDateLineCode( _
'                str_tmpScannedQrcode(int_Counter), _
'                True _
'                )

'            Dim qrCodeIsRegistered As Boolean = ( _
'                ScanValidator.IsItemQRCodeRegistered( _
'                    myScanner, _
'                    timerScanner, _
'                    scannerIsOn, _
'                    productionAct _
'                    ) _
'                )

'            If Not qrCodeIsRegistered Then
'                CloseNowLoadingAndEnableControls()
'                ResetScannerAndShowErrorMessage( _
'                    "Barang Sudah Terkirim atau QR Code Sudah Dihapus!", _
'                    "Error" _
'                )
'                SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '2'")
'                ResetScanRelatedData()
'                LoadDefaultDisplayedValues()
'                CloseNowLoadingAndEnableControls()
'                Return False
'            End If
'        Next

'        If list_DeletedItems.Count = 0 Then
'            subSetScannedDataForShipment(productionAct, _
'                                     str_tmpSplitBarcode, _
'                                     int_TmpScannedQtyList _
'                                     )
'        Else
'            For int_Counter As Integer = 0 To str_TmpDeletedActId.Count - 1
'                subUpdateShipmentData(productionAct, _
'                                         str_tmpSplitBarcode, _
'                                         int_TmpScannedQtyList, _
'                                         str_TmpDeletedActId(int_Counter) _
'                                         )
'            Next
'        End If


'        Dim scannedShipActs As List(Of ShipmentAct) = ( _
'            ShipmentActDB.GetBarcodeTagsAndActQty(_scannedBarcodes) _
'            )

'        ' Count > 0, some/all barcodes scanned has been saved
'        ' in the database previously
'        If scannedShipActs.Count > 0 Then
'            CloseNowLoadingAndEnableControls()

'            Dim shipmentDuplicateBarcodesWindow As New  _
'                ShipmentDuplicateBarcodes(Me, scannedShipActs)

'            shipmentDuplicateBarcodesWindow.ShowDialog()
'            shipmentDuplicateBarcodesWindow = Nothing

'            ResetScannerAndShowErrorMessage( _
'                "Ulangi Proses Save Sekali Lagi", _
'                "Pesan" _
'                )
'            Return False
'        End If

'        ' Updating data in ShipmentAct table
'        If _tmpShipActDataUpdate.Count > 0 Then
'            Dim exceptionMsg As String = String.Empty

'            If Not ShipmentActDB.UpdateData( _
'                _tmpShipActDataUpdate, _
'                0, _
'                int_tmpActIdList, _
'                exceptionMsg _
'                ) Then
'                CloseNowLoadingAndEnableControls()

'                If exceptionMsg.Length > 0 Then
'                    ResetScannerAndShowErrorMessage(exceptionMsg, "DB Error")
'                End If

'                Return False
'            Else
'                If ShipmentPlanDB.fncCheckStockTakeFlag(sid) = 1 Then
'                    fncInsertStockTakeTemp( _
'                        _tmpShipActDataUpdate, _
'                         exceptionMsg _
'                    )
'                End If
'            End If
'        End If

'        ' Inserting data to ShipmentAct table
'        Dim newActIds As List(Of Integer) = Nothing

'        If _tmpShipActDataInsert.Count > 0 Then
'            Dim exceptionMsg As String = String.Empty
'            newActIds = ShipmentActDB.InsertData( _
'                _tmpShipActDataInsert, _
'                exceptionMsg _
'                )

'            If exceptionMsg.Length > 0 Then
'                CloseNowLoadingAndEnableControls()
'                ResetScannerAndShowErrorMessage(exceptionMsg, "DB Error")

'                exceptionMsg = String.Empty

'                ShowNowLoadingAndDisableControls()

'                If _tmpShipActDataUpdate.Count > 0 Then
'                    ' Rollback update on Shipment Act because of failure to 
'                    ' update Stock Card table by resetting delFlg to 1
'                    ShipmentActDB.UpdateData( _
'                        _tmpShipActDataUpdate, _
'                        1, _
'                        int_tmpActIdList, _
'                        exceptionMsg _
'                        )
'                End If

'                ' Rollback previous insert by deleting inserted data
'                ' Rollback must be done manually
'                ShipmentActDB.DeleteData(newActIds, exceptionMsg)
'                newActIds.Clear()

'                CloseNowLoadingAndEnableControls()

'                If exceptionMsg.Length > 0 Then
'                    ResetScannerAndShowErrorMessage(exceptionMsg, "DB Error")
'                End If

'                Return False
'            End If

'            If ShipmentPlanDB.fncCheckStockTakeFlag(sid) = 1 Then
'                fncInsertStockTakeTemp( _
'                    _tmpShipActDataInsert, _
'                     exceptionMsg _
'                )
'            End If
'        End If

'        For index As Integer = 0 To _tmpShipActDataInsert.Count - 1
'            ' When insert is ignored because of duplicate data
'            ' newActId = 0
'            If newActIds(index) = 0 Then
'                _actualQty -= CInt( _
'                    _tmpShipActDataInsert(index)(ShipmentActValues.ActQty) _
'                    )

'                Continue For
'            End If

'            ' Prepare a list of data to be inserted into Stock Card table
'            Dim stockCardData() As String = CompileStockCardData( _
'                _tmpShipActDataInsert(index), _
'                index, _
'                newActIds(index).ToString() _
'                )
'            _tmpStockCardDataInsert.Add(stockCardData)
'        Next

'        For index As Integer = 0 To _tmpShipActDataUpdate.Count - 1
'            ' Prepare a list of data to update data in Stock Card table
'            Dim stockCardData() As String = CompileStockCardData( _
'                _tmpShipActDataUpdate(index), _
'                index _
'                )
'            _tmpStockCardDataUpdate.Add(stockCardData)
'        Next

'        If _tmpShipActDataInsert.Count > 0 Then
'            Dim exceptionMsg As String = String.Empty

'            If Not StockCardDB.InsertData( _
'                _tmpStockCardDataInsert, _
'                exceptionMsg _
'                ) Then
'                If exceptionMsg.Length > 0 Then
'                    CloseNowLoadingAndEnableControls()
'                    ResetScannerAndShowErrorMessage(exceptionMsg, "DB Error")
'                    ShowNowLoadingAndDisableControls()
'                End If

'                exceptionMsg = String.Empty

'                If _tmpShipActDataUpdate.Count > 0 Then
'                    ' Rollback update on Shipment Act because of failure to 
'                    ' update Stock Card table by resetting delFlg to 1
'                    ShipmentActDB.UpdateData( _
'                        _tmpShipActDataUpdate, _
'                        1, _
'                        int_tmpActIdList, _
'                        exceptionMsg _
'                        )
'                End If

'                ' Rollback inserted data in ShipmentAct because failure to
'                ' insert data to Stock Card table
'                ShipmentActDB.DeleteData(newActIds, exceptionMsg)
'                newActIds.Clear()

'                CloseNowLoadingAndEnableControls()

'                If exceptionMsg.Length > 0 Then
'                    ResetScannerAndShowErrorMessage(exceptionMsg, "DB Error")
'                End If

'                ' Clear data to avoid duplicate when user trying to re-save
'                _tmpStockCardDataInsert.Clear()

'                Return False
'            End If
'        End If

'        If _tmpShipActDataUpdate.Count > 0 Then
'            Dim exceptionMsg As String = String.Empty

'            If Not StockCardDB.UpdateData( _
'                _tmpStockCardDataUpdate, _
'                exceptionMsg _
'                ) Then
'                If exceptionMsg.Length > 0 Then
'                    CloseNowLoadingAndEnableControls()
'                    ResetScannerAndShowErrorMessage(exceptionMsg, "DB Error")
'                    ShowNowLoadingAndDisableControls()
'                End If

'                exceptionMsg = String.Empty

'                ' Rollback update on Shipment Act because of failure to 
'                ' update Stock Card table by resetting delFlg to 1
'                ShipmentActDB.UpdateData( _
'                    _tmpShipActDataUpdate, _
'                    1, _
'                    int_tmpActIdList, _
'                    exceptionMsg _
'                    )

'                ' Rollback inserted data in ShipmentAct and Stock Card tables 
'                ' because failure to update Stock Card data table 
'                ' by deleting them
'                ShipmentActDB.DeleteData(newActIds, exceptionMsg)
'                StockCardDB.DeleteData(newActIds, exceptionMsg)
'                newActIds.Clear()

'                CloseNowLoadingAndEnableControls()

'                If exceptionMsg.Length > 0 Then
'                    ResetScannerAndShowErrorMessage(exceptionMsg, "DB Error")
'                End If

'                ' Clear data to avoid duplicate when user trying to re-save
'                _tmpStockCardDataUpdate.Clear()

'                Return False
'            End If
'        End If

'        ' Update planQty and ActQty on the data grid in shipmentList window.
'        shipmentList.DataGrid1( _
'            currentRowOnDataGrid, _
'            shipmentList.DataGridColumns.PlanQty _
'            ) = String.Format("{0:n0}", planQty)
'        shipmentList.DataGrid1( _
'            currentRowOnDataGrid, _
'            shipmentList.DataGridColumns.ActQty _
'            ) = String.Format("{0:n0}", _actualQty)

'        Dim shipmentStatus As String = CommonLib.GetStockStatus( _
'            planQty, _
'            _actualQty _
'            )
'        shipmentList.DataGrid1( _
'            currentRowOnDataGrid, _
'            shipmentList.DataGridColumns.Status _
'            ) = shipmentStatus

'        shipmentList.dataSource.AcceptChanges()

'        SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '2'")

'        ResetScanRelatedData()
'        LoadDefaultDisplayedValues()
'        CloseNowLoadingAndEnableControls()

'        ResetScannerAndShowInfoMessage( _
'            "Data Telah Tersimpan di Database", _
'            "Sukses" _
'            )

'        Return True
'    End Function

'    Private Function CompileStockCardData( _
'        ByVal shipActData As String(), _
'        ByVal counter As Integer, _
'              Optional ByVal newActID As String = "" _
'              ) As String()
'        Dim stockCardData(7) As String

'        stockCardData(StockCardValues.TrinPartCode) = trinPartNo

'        If newActID.Length > 0 Then
'            stockCardData(StockCardValues.ActID) = newActID
'        Else
'            stockCardData(StockCardValues.ActID) = ( _
'                shipActData(ShipmentActValues.ActIdOfDeletedData) _
'                )
'        End If

'        stockCardData(StockCardValues.ActQty) = ( _
'            shipActData(ShipmentActValues.ActQty) _
'            )
'        stockCardData(StockCardValues.TypeID) = "2"
'        stockCardData(StockCardValues.BarcodeTag) = ( _
'            shipActData(ShipmentActValues.BarcodeTag) _
'            )
'        stockCardData(StockCardValues.LineCode) = ( _
'            shipActData(ShipmentActValues.LineCode) _
'            )
'        stockCardData(StockCardValues.ScanDateTime) = shipActData(ShipmentActValues.ShipmentDate)
'        stockCardData(StockCardValues.UserID) = ( _
'            TemporaryData.loggedInUserID _
'            )

'        Return stockCardData
'    End Function

'    Private Sub timerScanner_Tick( _
'        ByVal sender As Object, _
'        ByVal e As System.EventArgs _
'        ) Handles timerScanner.Tick
'        If scannerOnDuration < 10 Then
'            scannerOnDuration += 1
'        Else
'            SetScannerStatusOff()
'            DisableScanner()
'            EnableScanner()
'        End If
'    End Sub

'    Private Sub EnableFormControls(ByVal enable As Boolean)
'        buttonSave.Enabled = enable
'        buttonBack.Enabled = enable
'        buttonReset.Enabled = enable
'        processKeypadIsEnabled = enable
'    End Sub

'    Private Sub ResetScannerAndShowInfoMessage( _
'        ByVal errorMsg As String, _
'        ByVal errorTitle As String _
'        )
'        SetScannerStatusOff()
'        DisableScanner()
'        DisplayMessage.OkMsg(errorMsg, errorTitle)
'        EnableScanner()
'    End Sub

'    Private Sub ResetScannerAndShowErrorMessage( _
'        ByVal errorMsg As String, _
'        ByVal errorTitle As String _
'        )
'        SetScannerStatusOff()
'        DisableScanner()
'        DisplayMessage.ErrorMsg(errorMsg, errorTitle)
'        EnableScanner()
'    End Sub

'    Private Function ResetScannerAndShowConfirmationMessage( _
'        ByVal confirmationMsg As String, _
'        ByVal confirmationTitle As String _
'        ) As Boolean
'        SetScannerStatusOff()
'        DisableScanner()

'        Dim confirm As Boolean = DisplayMessage.ConfirmationDialog( _
'            confirmationMsg, _
'            confirmationTitle _
'            )
'        EnableScanner()

'        Return confirm
'    End Function

'    Private Sub subSetScannedDataForShipment( _
'                ByRef productionAct As ProductionAct, _
'                ByVal lst_Barcodes As List(Of String), _
'                ByVal lst_ScanQty As List(Of Integer) _
'                )

'        For int_Counter As Integer = 0 To lst_Barcodes.Count - 1
'            Dim singleShipActData(12) As String

'            singleShipActData(ShipmentActValues.SID) = sid
'            singleShipActData(ShipmentActValues.SONumber) = soNumber
'            singleShipActData(ShipmentActValues.CustomerCode) = customerCode
'            singleShipActData(ShipmentActValues.PlantNo) = plantNo
'            singleShipActData(ShipmentActValues.ShipmentDate) = Now.ToString( _
'                "yyyy-MM-dd HH:mm:ss" _
'                )
'            singleShipActData(ShipmentActValues.UserID) = ( _
'                TemporaryData.loggedInUserID _
'                )
'            singleShipActData(ShipmentActValues.BarcodeTag) = lst_Barcodes(int_Counter)
'            singleShipActData(ShipmentActValues.ItemProductionDate) = ( _
'                productionAct.PRODDATE.ToString("yyyy-MM-dd HH:mm:ss") _
'                )
'            singleShipActData(ShipmentActValues.TrinPartCode) = trinPartNo
'            singleShipActData(ShipmentActValues.OKNG) = "1"
'            singleShipActData(ShipmentActValues.ActQty) = CStr(lst_ScanQty(int_Counter))
'            singleShipActData(ShipmentActValues.LineCode) = ( _
'                productionAct.LINECODE _
'                )

'            _tmpShipActDataInsert.Add(singleShipActData)
'        Next
'    End Sub

'    Private Sub subUpdateShipmentData(ByRef productionAct As ProductionAct, _
'                ByVal lst_Barcodes As List(Of String), _
'                ByVal lst_ScanQty As List(Of Integer), _
'                ByVal str_DeletedItems As String _
'                )

'        For int_Counter As Integer = 0 To lst_Barcodes.Count - 1
'            Dim singleShipActData(12) As String

'            singleShipActData(ShipmentActValues.SID) = sid
'            singleShipActData(ShipmentActValues.SONumber) = soNumber
'            singleShipActData(ShipmentActValues.CustomerCode) = customerCode
'            singleShipActData(ShipmentActValues.PlantNo) = plantNo
'            singleShipActData(ShipmentActValues.ShipmentDate) = Now.ToString( _
'                "yyyy-MM-dd HH:mm:ss" _
'                )
'            singleShipActData(ShipmentActValues.UserID) = ( _
'                TemporaryData.loggedInUserID _
'                )
'            singleShipActData(ShipmentActValues.BarcodeTag) = lst_Barcodes(int_Counter)
'            singleShipActData(ShipmentActValues.ItemProductionDate) = ( _
'                productionAct.PRODDATE.ToString("yyyy-MM-dd HH:mm:ss") _
'                )
'            singleShipActData(ShipmentActValues.TrinPartCode) = trinPartNo
'            singleShipActData(ShipmentActValues.OKNG) = "1"
'            singleShipActData(ShipmentActValues.ActQty) = CStr(lst_ScanQty(int_Counter))
'            singleShipActData(ShipmentActValues.ActIdOfDeletedData) = str_DeletedItems
'            singleShipActData(ShipmentActValues.LineCode) = ( _
'                productionAct.LINECODE _
'                )

'            _tmpShipActDataUpdate.Add(singleShipActData)
'        Next
'    End Sub

'    Private Sub subGetSqliteData(ByVal str_Barcode As String, _
'                                 ByVal str_QrCode As String, _
'                                 ByVal str_TrinPartNo As String, _
'                                 ByVal str_ScannedQty As String _
'                                 )
'        Try
'            mobj_SqliteData.Add("BarcodeTag", str_Barcode)
'            mobj_SqliteData.Add("QrCode", str_QrCode)
'            mobj_SqliteData.Add("TrinPartNo", str_TrinPartNo)
'            mobj_SqliteData.Add("InputFlag", mint_InputFlag.ToString)
'            mobj_SqliteData.Add("ScanQty", str_ScannedQty)
'        Catch ex As Exception
'            DisplayMessage.ErrorMsg(ex.ToString, "Error")
'            Exit Sub
'        End Try

'    End Sub

'End Class
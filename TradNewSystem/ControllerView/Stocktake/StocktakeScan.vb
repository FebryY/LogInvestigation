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


Public Class StocktakeScan
    Protected Friend nowLoadingWindow As NowLoading

    Private _stocktakeList As StocktakeList
    Private _trinPartNo As String
    Private _division As String
    Private _totalQty As Integer
    Private _countedQty As Integer
    Private _tagCount As Integer

    Private _tmpBarcodeTagData As New List(Of String)
    Private _tmpStocktakeDataInsert As New List(Of String())
    Private _tmpStocktakeDataUpdate As New List(Of String())
    Private _tmpScannedQty As New List(Of String)
    Private _tmpTrinPartNo As New List(Of String)

    Private totalScanQtyOfDuplicatedBarcodes As Integer
    Private _lastScannedBarcodeTag As String
    Private _lastScannedQty As String

    Private dataGridCurrentRow As Integer
    Private scannerOnDuration As Integer
    Private scannerIsOn As Boolean
    Private scannerIsEnabled As Boolean
    Private processKeypadIsEnabled As Boolean

    Private str_tmpSplitQr As New List(Of String())
    Private int_TmpScannedQtyList As New List(Of Integer)
    Private cint_TmpScannedTotalQty As Integer

    Private mstr_SqliteTable As String = "TempScannedData"
    Private mint_InputFlag As Integer = 1
    Private bool_TempDataLoad As Boolean = False

    Private mobj_SqliteData As New Dictionary(Of String, String)()

    Friend WithEvents myScanner As Scanner

#Region "Properties"
    Public Property tmpBarcodeTagData() As List(Of String)
        Get
            Return _tmpBarcodeTagData
        End Get

        Set(ByVal value As List(Of String))
            _tmpBarcodeTagData = value
        End Set
    End Property

    Public Property tmpScannedQty() As List(Of String)
        Get
            Return _tmpScannedQty
        End Get

        Set(ByVal value As List(Of String))
            _tmpScannedQty = value
        End Set
    End Property

    Public Property tmpStocktakeDataInsert() As List(Of String())
        Get
            Return _tmpStocktakeDataInsert
        End Get

        Set(ByVal value As List(Of String()))
            _tmpStocktakeDataInsert = value
        End Set
    End Property

    Public Property tmpStocktakeDataUpdate() As List(Of String())
        Get
            Return _tmpStocktakeDataUpdate
        End Get

        Set(ByVal value As List(Of String()))
            _tmpStocktakeDataUpdate = value
        End Set
    End Property

    Public Property lastScannedBarcodeTag() As String
        Get
            Return _lastScannedBarcodeTag
        End Get

        Set(ByVal value As String)
            _lastScannedBarcodeTag = value
        End Set
    End Property

    Public Property lastScannedQty() As String
        Get
            Return _lastScannedQty
        End Get

        Set(ByVal value As String)
            _lastScannedQty = value
        End Set
    End Property

    Public Property countedQty() As Integer
        Get
            Return _countedQty
        End Get

        Set(ByVal value As Integer)
            _countedQty = value
        End Set
    End Property

    Public Property tagCount() As Integer
        Get
            Return _tagCount
        End Get

        Set(ByVal value As Integer)
            _tagCount = value
        End Set
    End Property
#End Region

    Public Sub New(ByVal _stocktakeList As StocktakeList)
        InitializeComponent()

        Me._stocktakeList = _stocktakeList

        dataGridCurrentRow = _stocktakeList.DataGrid1.CurrentCell.RowNumber

        _division = _stocktakeList.DataGrid1( _
            dataGridCurrentRow, _
            StocktakeList.DataGridColumns.Division _
            ).ToString()
        _trinPartNo = _stocktakeList.DataGrid1( _
            dataGridCurrentRow, _
            StocktakeList.DataGridColumns.TrinPartNo _
            ).ToString()

        LabelTrinPartNoValue.Text = _trinPartNo
        LoadDefaultDisplayedValues()
    End Sub

    Private Sub LoadDefaultDisplayedValues()
        Dim strTotalQty As String = _stocktakeList.DataGrid1( _
            dataGridCurrentRow, _
            StocktakeList.DataGridColumns.TotalQty _
            ).ToString()
        Dim strCountedQty As String = _stocktakeList.DataGrid1( _
            dataGridCurrentRow, _
            StocktakeList.DataGridColumns.CountedQty _
            ).ToString()

        Dim currentCulture As CultureInfo = CultureInfo.CurrentCulture

        _totalQty = CInt(Convert.ToDouble(strTotalQty, currentCulture))
        _countedQty = CInt(Convert.ToDouble(strCountedQty, currentCulture))

        _tagCount = CInt( _
            StocktakeDB.GetScannedBarcodeCount( _
                _trinPartNo, _
                _stocktakeList.SelectedStocktakePeriod _
                ) _
            )

        If int_TmpScannedQtyList.Count > 0 Then
            _countedQty += cint_TmpScannedTotalQty
            _tagCount += int_TmpScannedQtyList.Count
        End If

        LabelTotalQtyValue.Text = String.Format("{0:n0}", _totalQty)
        LabelCountedQtyValue.Text = String.Format("{0:n0}", _countedQty)
        LabelTagCountValue.Text = CStr(_tagCount)

        _lastScannedBarcodeTag = String.Empty
        _lastScannedQty = String.Empty
        textBoxScanTag.Text = String.Empty
    End Sub

    Private Sub StocktakeScan_Load( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles MyBase.Load
        scannerIsOn = False

        cint_TmpScannedTotalQty = 0

        Dim sql_Query As String = String.Format("select BarcodeTag, TrinPartNo, ScanQty from {0} where InputFlag = '{1}';", _
                                                mstr_SqliteTable, _
                                                mint_InputFlag _
                                                )

        Dim dt_SqliteData As DataTable = SqliteControllerDB.fncGetDatatable(sql_Query)

        _tmpBarcodeTagData = (From dr_Sqlite In dt_SqliteData.AsEnumerable() Select dr_Sqlite.Field(Of String)(0)).ToList()
        _tmpTrinPartNo = (From dr_Sqlite In dt_SqliteData.AsEnumerable() Select dr_Sqlite.Field(Of String)(1)).ToList()
        _tmpScannedQty = (From dr_Sqlite In dt_SqliteData.AsEnumerable() Select dr_Sqlite.Field(Of String)(2)).ToList()

        Dim str_TrinList As String = String.Join(",", _tmpTrinPartNo.ToArray())

        Dim str_Confirmation As String = String.Format("Data TrinPartNo {0} belum diproses, ingin menyimpan?", _
                                                       str_TrinList)

        If dt_SqliteData.Rows.Count > 0 Then
            bool_TempDataLoad = True

            Dim bool_Confirm As Boolean = DisplayMessage.ConfirmationDialog(str_Confirmation, _
                                                                            "Konfirmasi" _
                                                                            )
            If bool_Confirm Then

                int_TmpScannedQtyList = _tmpScannedQty.ConvertAll(AddressOf Int32.Parse)

                For Each obj_TmpScannedQtyItems In int_TmpScannedQtyList
                    cint_TmpScannedTotalQty += obj_TmpScannedQtyItems
                Next

                For int_Counter As Integer = 0 To _tmpBarcodeTagData.Count - 1

                    Dim singleStockData(4) As String

                    singleStockData(StocktakeValues.TrinPartNo) = dt_SqliteData.Rows(int_Counter)(1).ToString
                    singleStockData(StocktakeValues.BarcodeTag) = dt_SqliteData.Rows(int_Counter)(0).ToString
                    singleStockData(StocktakeValues.ScannedQty) = dt_SqliteData.Rows(int_Counter)(2).ToString
                    singleStockData(StocktakeValues.StocktakePeriod) = ( _
                        _stocktakeList.SelectedStocktakePeriod. _
                            ToString("yyyy-MM-dd 00:00:00") _
                        )
                    singleStockData(StocktakeValues.TakeDateTime) = ( _
                        Now.ToString("yyyy-MM-dd HH:mm:ss") _
                        )

                    _tmpStocktakeDataInsert.Add(singleStockData)

                Next
                SaveScannedData()

                Exit Sub
            Else
                Dim del_Confirm As Boolean = DisplayMessage.ConfirmationDialog("Yakin untuk menghapus data?", _
                                                                            "Konfirmasi" _
                                                                            )
                If del_Confirm Then
                    SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '1'")
                    _tmpBarcodeTagData.Clear()
                    _tmpScannedQty.Clear()
                    _tmpTrinPartNo.Clear()
                    processKeypadIsEnabled = True
                    EnableScanner()
                Else
                    _tmpBarcodeTagData.Clear()
                    _tmpScannedQty.Clear()
                    _tmpTrinPartNo.Clear()
                    Close()
                    Exit Sub
                End If
            End If
        End If

        processKeypadIsEnabled = True
        EnableScanner()
    End Sub

    Private Sub StocktakeScan_Closed( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
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
                SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '1'")
                CloseWindow()
            Case Windows.Forms.Keys.F3
                SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '1'")
                ClearScannedData()
        End Select
    End Sub

    Private Sub UndoChangeToTextBoxScanTag()
        textBoxScanTag.Text = _lastScannedBarcodeTag
        textBoxScanTag.SelectionStart = 0
        textBoxScanTag.SelectionLength = 0
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

    Private Sub textBoxScanTag_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles textBoxScanTag.KeyUp
        ActivateCommonShortcut(e)

        If e.KeyCode <> Keys.SCAN _
            Or e.KeyCode <> Keys.SCAN2 _
            Or e.KeyCode <> Keys.SCAN3 Then
            UndoChangeToTextBoxScanTag()
        End If
    End Sub

    Private Sub textBoxScanTag_KeyDown( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles textBoxScanTag.KeyDown
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
            UndoChangeToTextBoxScanTag()
        End If
    End Sub

    Private Sub ButtonSave_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonSave.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonBack_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonBack.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonReset_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonReset.KeyUp
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
        ) Handles ButtonSave.Click
        SaveScannedData()
    End Sub

    Private Sub ButtonBack_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonBack.Click
        SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '1'")
        CloseWindow()
    End Sub

    Private Sub ButtonReset_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonReset.Click
        SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '1'")
        ClearScannedData()
    End Sub
#End Region

    Private Sub SaveScannedData()
        If _tmpBarcodeTagData.Count > 0 Then
            Dim confirm As Boolean = ResetScannerAndShowConfirmationMessage( _
                "Save Data Yang Telah Di Scan ke Database?", _
                "Konfirmasi" _
                )

            If confirm = True Then
                'Check Connection Wifi
                Try
                    If RF.SYNCHRONIZE(RF.SYNC_CHECK) <> 0 Then
                        ResetScannerAndShowErrorMessage("Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
                            "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.", "Error")
                        Exit Sub
                    End If
                Catch ex As Exception
                    If Err.Number = 5 Then
                        ResetScannerAndShowErrorMessage("Koneksi Wifi di HT tertutup." & vbCrLf & _
                            "Tunggu beberapa detik dan ulangi lagi.", "Error")
                        Dim MyRf As RF
                        MyRf = New RF()
                        MyRf.OpenMode = RF.EN_OPEN_MODE.CONTINUOUSLY
                        MyRf.Open = True
                        Exit Sub
                    End If
                End Try
                ProcessSavingDataToDB()
                'add by lutfi
                DisableScanner()
                Close()
            Else
                If bool_TempDataLoad Then
                    _tmpBarcodeTagData.Clear()
                    _tmpScannedQty.Clear()
                    _tmpTrinPartNo.Clear()
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
    End Sub

    Private Sub CloseWindow()
        If _tmpBarcodeTagData.Count > 0 Then
            Dim confirm As Boolean = ResetScannerAndShowConfirmationMessage( _
                "Data Yang Telah Di Scan Belum Tersimpan di Database. " & _
                    "Lanjut?", _
                "Konfirmasi" _
                )

            If confirm = True Then
                Close()
            End If
        Else
            Close()
        End If
    End Sub

    Private Sub ClearScannedData()
        If _tmpBarcodeTagData.Count = 0 Then
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
        _tmpBarcodeTagData.Clear()
        _tmpScannedQty.Clear()
        _tmpStocktakeDataInsert.Clear()
        _tmpStocktakeDataUpdate.Clear()
        str_tmpSplitQr.Clear()
        int_TmpScannedQtyList.Clear()
        mobj_SqliteData.Clear()
    End Sub

    Private Sub EnableScanner()
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

#Region "Scanned Data Validation"
    Private Function ValidateScannedData( _
        ByVal splitQRCode As String() _
        ) As Boolean
        If Not ScanValidator.isValidFinalQrCode(splitQRCode) Then
            CloseNowLoadingAndEnableControls()
            ResetScannerAndShowErrorMessage( _
                "QR Code Tidak Valid!", _
                "Scan Error" _
                )
            Return False
        End If

        ' QRCode Registered in ProductionAct
        Dim productionAct As ProductionAct = ProductionActDB. _
            GetProdDateLineCode(String.Join(";", splitQRCode), False)

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
                "Barang Sudah Terkirim atau QR Code Tidak Valid!" & _
                "Silahkan Reset dan Ulangi Scan", _
                "Error" _
                )
            Return False
        End If

        Return True
    End Function
#End Region

    Private Sub ShowNowLoadingAndDisableControls()
        EnableFormControls(False)

        nowLoadingWindow = New NowLoading()
        nowLoadingWindow.Show()
    End Sub

    Private Sub CloseNowLoadingAndEnableControls()
        EnableFormControls(True)

        If Not IsNothing(nowLoadingWindow) Then
            nowLoadingWindow.Close()
            nowLoadingWindow = Nothing
        End If
    End Sub

    Private Sub myScanner_OnDone( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles myScanner.OnDone
        textBoxScanTag.Focus()

        Dim scannedQRCode As String = String.Empty
        Try
            If RF.SYNCHRONIZE(RF.SYNC_CHECK) <> 0 Then
                CloseNowLoadingAndEnableControls()
                ResetScannerAndShowErrorMessage("Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
                    "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.", "Error")
                Exit Sub
            End If
        Catch ex As Exception
            If Err.Number = 5 Then
                CloseNowLoadingAndEnableControls()
                ResetScannerAndShowErrorMessage("Koneksi Wifi di HT tertutup." & vbCrLf & _
                    "Tunggu beberapa detik dan ulangi lagi.", "Error")
                Dim MyRf As RF
                MyRf = New RF()
                MyRf.OpenMode = RF.EN_OPEN_MODE.CONTINUOUSLY
                MyRf.Open = True
                Exit Sub
            End If
        End Try
        Try
            scannedQRCode = myScanner.Input(Scanner.ALL_BUFFER).Trim
        Catch ex As Exception
            ResetScannerAndShowErrorMessage(ex.Message, "Scan Error")
        End Try

        If scannedQRCode.Length = 0 Then
            Exit Sub
        End If

        Dim splitQRCode As String() = scannedQRCode.Split(New Char() {";"c})

        BHTController.SoundOK()

        str_tmpSplitQr.Add(splitQRCode)

        If ScanValidator.HasBarcodeTagBeenScanned( _
            myScanner, _
            timerScanner, _
            scannerIsOn, _
            _tmpBarcodeTagData, _
            splitQRCode(FinalQrCodeValues.BarcodeTag) _
            ) Then
            CloseNowLoadingAndEnableControls()
            ResetScannerAndShowErrorMessage( _
                String.Format( _
                    "QR Code [{0}] Telah Di Scan Sebelumnya!", _
                    splitQRCode(FinalQrCodeValues.BarcodeTag) _
                    ), _
                "Error" _
                )
            Exit Sub
        End If

        ' Match TRIN Part No
        Dim trinCodeIsMatched As Boolean = ( _
            ScanValidator.IsProductTrinCodeMatched( _
                myScanner, _
                timerScanner, _
                scannerIsOn, _
                _trinPartNo, _
                splitQRCode(FinalQrCodeValues.TrinPartCode) _
                ) _
            )

        If Not trinCodeIsMatched Then
            CloseNowLoadingAndEnableControls()
            ResetScannerAndShowErrorMessage( _
                "TRIN Part No Tidak Sama!" & _
                "Silahkan Reset dan Ulangi Scan", _
                "Error" _
                )
            Exit Sub
        End If

        scannerOnDuration = 0

        Try
            _tmpBarcodeTagData.Add(splitQRCode(FinalQrCodeValues.BarcodeTag))
            _tmpScannedQty.Add(splitQRCode(FinalQrCodeValues.ActQty))

            Dim singleStockData(4) As String

            For int_Counter As Integer = 0 To _tmpBarcodeTagData.Count - 1
                singleStockData(StocktakeValues.TrinPartNo) = _trinPartNo
                singleStockData(StocktakeValues.BarcodeTag) = _tmpBarcodeTagData(int_Counter)
                singleStockData(StocktakeValues.ScannedQty) = _tmpScannedQty(int_Counter)
                singleStockData(StocktakeValues.StocktakePeriod) = ( _
                    _stocktakeList.SelectedStocktakePeriod. _
                        ToString("yyyy-MM-dd 00:00:00") _
                    )
                singleStockData(StocktakeValues.TakeDateTime) = ( _
                    Now.ToString("yyyy-MM-dd HH:mm:ss") _
                    )
                _tmpStocktakeDataInsert.Add(singleStockData)
            Next
        Catch ex As Exception
            DisplayMessage.ErrorMsg(ex.Message, "Error")
        End Try

        _lastScannedBarcodeTag = splitQRCode(FinalQrCodeValues.BarcodeTag)
        _lastScannedQty = splitQRCode(FinalQrCodeValues.ActQty)
        textBoxScanTag.Text = _lastScannedBarcodeTag

        _countedQty += CInt(splitQRCode(FinalQrCodeValues.ActQty))
        LabelCountedQtyValue.Text = String.Format("{0:n0}", _countedQty)

        _tagCount += 1
        LabelTagCountValue.Text = CStr(_tagCount)

        '********************************** Add Sqlite process *************************************
        '************************************ Adri_[20170309] **************************************

        Try
            subGetSqliteData(_lastScannedBarcodeTag, scannedQRCode, _trinPartNo, _lastScannedQty)

            If SqliteControllerDB.fncInsert(mstr_SqliteTable, mobj_SqliteData) Then
                mobj_SqliteData.Clear()
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
    End Sub

    Private Function ProcessSavingDataToDB() As Boolean
        ShowNowLoadingAndDisableControls()
        For int_Counter As Integer = 0 To str_tmpSplitQr.Count - 1
            If Not ValidateScannedData(str_tmpSplitQr(int_Counter)) Then
                Return False
            End If
        Next

        Dim scannedStocktakes As List(Of Stocktake) = _
            StocktakeDB.GetBarcodesAndScannedQty( _
                _tmpBarcodeTagData, _
                _stocktakeList.SelectedStocktakePeriod, _
                True _
                )
        ' Count > 0, some/all barcodes scanned has been saved
        ' in the database previously
        If scannedStocktakes.Count > 0 Then
            CloseNowLoadingAndEnableControls()

            If DeleteDuplicateBarcodesFromStocktakeScanData(scannedStocktakes) Then
                UpdateItemQtyOnStocktakeScanWindow(scannedStocktakes)
            End If

        End If

        Dim unscannedStocktakes As List(Of Stocktake) = _
            StocktakeDB.GetBarcodesAndScannedQty( _
                _tmpBarcodeTagData, _
                _stocktakeList.SelectedStocktakePeriod, _
                False _
                )
        ' Find how many barcodes registered in the 
        ' stocktake table but have not been scanned.
        ' tmpStocktakeDataAll will then only contain unregistered barcode
        If unscannedStocktakes.Count > 0 Then
            For index As Integer = tmpStocktakeDataInsert.Count - 1 To 0 Step -1

                Dim barcodeTag As String = ( _
                    tmpStocktakeDataInsert(index)(StocktakeValues.BarcodeTag) _
                    )

                Dim stocktake As IEnumerable(Of Stocktake) = _
                    From x In unscannedStocktakes _
                    Where x.BARCODETAG = barcodeTag Select x

                If stocktake.Any Then
                    tmpStocktakeDataUpdate.Add(tmpStocktakeDataInsert(index))
                    tmpStocktakeDataInsert.RemoveAt(index)
                End If
            Next
        End If


        ' Insert the unregistered barcodes to stocktake table
        'modify 9j
        Dim AlreadyDelData As String = String.Empty
        Dim AlreadyDelCount As Integer = 0
        Dim QtyOK As Double = 0
        If _tmpStocktakeDataInsert.Count > 0 Then
            Dim exceptionMsg As String = String.Empty

            If Not StocktakeDB.InsertData( _
                _tmpStocktakeDataInsert, AlreadyDelData, AlreadyDelCount, _
                exceptionMsg, QtyOK _
                ) Then
                CloseNowLoadingAndEnableControls()

                If exceptionMsg.Length > 0 Then
                    ResetScannerAndShowErrorMessage(exceptionMsg, "DB Error")
                End If

                Return False
            End If
        End If

        ' Update the barcodes in the stocktake table
        ' to mark them as "stocktake complete"
        If _tmpStocktakeDataUpdate.Count > 0 Then
            Dim exceptionMsg As String = String.Empty

            If Not StocktakeDB.UpdateData( _
                _tmpStocktakeDataUpdate, AlreadyDelData, AlreadyDelCount, _
                exceptionMsg, QtyOK _
                ) Then
                CloseNowLoadingAndEnableControls()

                If exceptionMsg.Length > 0 Then
                    ResetScannerAndShowErrorMessage(exceptionMsg, "DB Error")
                End If

                ' Rollback inserted data in stocktake because failure to
                ' update data in stocktake table
                StocktakeDB.DeleteData(_tmpStocktakeDataInsert, exceptionMsg)

                If exceptionMsg.Length > 0 Then
                    ResetScannerAndShowErrorMessage(exceptionMsg, "DB Error")
                End If

                Return False
            End If
        End If

        ' Update total item qty and total counted qty
        ' on the data grid in stocktakeList window.
        'add 9j

        If bool_TempDataLoad = True Then
            _countedQty += cint_TmpScannedTotalQty
        End If

        _countedQty = _countedQty - CInt(QtyOK)

        Dim stocktakeStatus As String = CommonLib.GetStockStatus( _
            _totalQty, _
            _countedQty _
            )

        _stocktakeList.DataGrid1( _
            dataGridCurrentRow, _
            StocktakeList.DataGridColumns.TotalQty _
            ) = String.Format("{0:n0}", _totalQty)
        _stocktakeList.DataGrid1( _
            dataGridCurrentRow, _
            StocktakeList.DataGridColumns.CountedQty _
            ) = String.Format("{0:n0}", _countedQty)
        _stocktakeList.DataGrid1( _
            dataGridCurrentRow, _
            StocktakeList.DataGridColumns.Status _
            ) = stocktakeStatus
        _stocktakeList.dataSource.AcceptChanges()

        scannedStocktakes.Clear()
        unscannedStocktakes.Clear()

        ResetScanRelatedData()

        'Modify 9h
        If _totalQty > _countedQty Then
            LoadDefaultDisplayedValues()
        End If

        'LoadDefaultDisplayedValues()
        CloseNowLoadingAndEnableControls()

        'add 9j
        If AlreadyDelCount > 0 Then
            ResetScannerAndShowErrorMessage("Ada " & AlreadyDelCount & _
                                           " Barcodetag yang telah terhapus" & _
                                           vbCrLf & AlreadyDelData, "Peringatan")
        End If

        ResetScannerAndShowInfoMessage( _
            "Data Telah Tersimpan di Database", _
            "Sukses" _
            )

        SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '1'")

        Return True
    End Function

    Private Sub timerScanner_Tick( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
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
        ButtonSave.Enabled = enable
        ButtonBack.Enabled = enable
        ButtonReset.Enabled = enable
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

    Private Function DeleteDuplicateBarcodesFromStocktakeScanData(ByVal scannedStock As List(Of Stocktake)) As Boolean
        Dim duplicatedBarcodes As New List(Of String)
        Dim totalDuplicatedBarcodes As Integer = scannedStock.Count

        For Each stocktake As Stocktake In scannedStock
            Dim str(0) As String

            str(0) = stocktake.BARCODETAG
            duplicatedBarcodes.Add(stocktake.BARCODETAG)

            totalScanQtyOfDuplicatedBarcodes = CInt(totalScanQtyOfDuplicatedBarcodes + stocktake.SCANNEDQTY)
        Next

        For index As Integer = tmpBarcodeTagData.Count - 1 To 0 Step -1
            If duplicatedBarcodes.Contains( _
                _tmpBarcodeTagData(index) _
                ) Then
                _tmpBarcodeTagData.RemoveAt(index)
            End If
        Next

        If _tmpBarcodeTagData.Count = 0 Then
            _tmpStocktakeDataInsert.Clear()
            _tmpStocktakeDataUpdate.Clear()

            _lastScannedBarcodeTag = String.Empty
            textBoxScanTag.Text = String.Empty
        Else
            For index As Integer = ( _
                _tmpStocktakeDataInsert.Count - 1 _
                ) To 0 Step -1
                Dim barcodeTag As String = ( _
                    _tmpStocktakeDataInsert(index) _
                        (StocktakeValues.BarcodeTag) _
                    )
                If duplicatedBarcodes.Contains(barcodeTag) Then
                    _tmpStocktakeDataInsert.RemoveAt(index)
                End If
            Next

            For index As Integer = ( _
                tmpStocktakeDataUpdate.Count - 1 _
                ) To 0 Step -1
                Dim barcodeTag As String = ( _
                    tmpStocktakeDataUpdate(index) _
                        (StocktakeValues.BarcodeTag) _
                    )
                If duplicatedBarcodes.Contains(barcodeTag) Then
                    tmpStocktakeDataUpdate.RemoveAt(index)
                End If
            Next
        End If

        Return True
    End Function

    Private Sub UpdateItemQtyOnStocktakeScanWindow(ByVal scannedStock As List(Of Stocktake))
        Dim totalDuplicatedBarcodes As Integer = scannedStock.Count

        _countedQty = CInt(Convert.ToDouble(LabelCountedQtyValue.Text))
        _countedQty = ( _
            _countedQty - totalScanQtyOfDuplicatedBarcodes _
            )

        totalScanQtyOfDuplicatedBarcodes = 0

        LabelCountedQtyValue.Text = ( _
            String.Format("{0:n0}", _countedQty) _
            )

        _tagCount = ( _
            _tagCount - totalDuplicatedBarcodes _
            )
        LabelTagCountValue.Text = ( _
            String.Format("{0:n0}", _tagCount) _
            )
    End Sub

    Private Sub subGetSqliteData(ByVal str_Barcode As String, _
                                 ByVal str_QrCode As String, _
                                 ByVal str_TrinPartNo As String, _
                                 ByVal str_ScannedQty As String _
                                 )
        Try
            mobj_SqliteData.Add("BarcodeTag", str_Barcode)
            mobj_SqliteData.Add("QrCode", str_QrCode)
            mobj_SqliteData.Add("TrinPartNo", str_TrinPartNo)
            mobj_SqliteData.Add("InputFlag", mint_InputFlag.ToString)
            mobj_SqliteData.Add("ScanQty", str_ScannedQty)
        Catch ex As Exception
            DisplayMessage.ErrorMsg(ex.ToString, "Error")
            Exit Sub
        End Try

    End Sub

End Class
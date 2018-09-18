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


Public Class OfflineStocktake
    Protected Friend nowLoadingWindow As NowLoading

    Private _tagCount As Integer
    Private _countedQty As Integer

    Private _tmpBarcodeTagData As New List(Of String)
    Private _tmpStocktakeDataAll As New List(Of String())
    Private _tmpStocktakeDataUpdate As New List(Of String())
    Private _tmpScannedQty As New List(Of String)
    Private _tmpTrinPartNo As New List(Of String)
    Private int_TmpScannedQtyList As New List(Of Integer)

    Private totalScanQtyOfDuplicatedBarcodes As Integer
    Private _lastScannedBarcodeTag As String

    Private stocktakePeriod As DateTime
    Private scannerOnDuration As Integer
    Private scannerIsOn As Boolean
    Private scannerIsEnabled As Boolean
    Private processKeypadIsEnabled As Boolean

    Private cint_TmpScannedTotalQty As Integer
    Private mstr_SqliteTable As String = "TempScannedData"
    Private mint_InputFlag As Integer = 3
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

    Public Property tmpStocktakeDataAll() As List(Of String())
        Get
            Return _tmpStocktakeDataAll
        End Get

        Set(ByVal value As List(Of String()))
            _tmpStocktakeDataAll = value
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

    Public Sub New(ByVal stocktakePeriod As DateTime)
        InitializeComponent()

        Me.stocktakePeriod = stocktakePeriod
    End Sub

    Private Sub OfflineStocktake_Load( _
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
                        stocktakePeriod.ToString("yyyy-MM-dd HH:mm:ss") _
                        )
                    singleStockData(StocktakeValues.TakeDateTime) = ( _
                        Now.ToString("yyyy-MM-dd HH:mm:ss") _
                        )

                    _tmpStocktakeDataAll.Add(singleStockData)

                Next
                InsertPartialScannedDataToDB()

                Exit Sub
            Else
                Dim del_Confirm As Boolean = DisplayMessage.ConfirmationDialog("Yakin untuk menghapus data?", _
                                                                            "Konfirmasi" _
                                                                            )
                If del_Confirm Then
                    SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '3'")
                    _tmpBarcodeTagData.Clear()
                    _tmpScannedQty.Clear()
                    _tmpTrinPartNo.Clear()
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

    Private Sub OfflineStocktake_Closed( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles MyBase.Closed
        timerScanner.Enabled = False
        DisableScanner()
    End Sub

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
        InsertPartialScannedDataToDB()
    End Sub

    Private Sub ButtonBack_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonBack.Click
        SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '3'")
        CloseWindow()
    End Sub

    Private Sub ButtonReset_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonReset.Click
        SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '3'")
        ClearScannedData()
    End Sub
#End Region

#Region "Key Press"
    Private Sub ActivateCommonShortcut( _
        ByVal e As KeyEventArgs _
        )
        If Not processKeypadIsEnabled Then
            Exit Sub
        End If

        Select Case e.KeyCode
            Case Windows.Forms.Keys.F1
                InsertPartialScannedDataToDB()
            Case Windows.Forms.Keys.F2
                SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '3'")
                CloseWindow()
            Case Windows.Forms.Keys.F3
                SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '3'")
                ClearScannedData()
        End Select
    End Sub

    Private Sub UndoChangeToTextBoxScanTag()
        TextBoxScanTag.Text = _lastScannedBarcodeTag
        TextBoxScanTag.SelectionStart = 0
        TextBoxScanTag.SelectionLength = 0
    End Sub

    Private Sub OfflineStocktake_KeyUp( _
        ByVal sender As Object, _
        ByVal e As System.Windows.Forms.KeyEventArgs _
        ) Handles MyBase.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub TextBoxScanTag_KeyUp( _
        ByVal sender As Object, _
        ByVal e As System.Windows.Forms.KeyEventArgs _
        ) Handles TextBoxScanTag.KeyUp
        ActivateCommonShortcut(e)

        If e.KeyCode <> Keys.SCAN _
            Or e.KeyCode <> Keys.SCAN2 _
            Or e.KeyCode <> Keys.SCAN3 Then
            UndoChangeToTextBoxScanTag()
        End If
    End Sub

    Private Sub TextBoxScanTag_KeyDown( _
        ByVal sender As Object, _
        ByVal e As System.Windows.Forms.KeyEventArgs _
        ) Handles TextBoxScanTag.KeyDown
        If e.KeyCode = Keys.SCAN _
            Or e.KeyCode = Keys.SCAN2 _
            Or e.KeyCode = Keys.SCAN3 Then
            If Not scannerIsEnabled Then
                Exit Sub
            End If

            If Not scannerIsOn Then
                scannerIsOn = True
                timerScanner.Enabled = True
                scannerOnDuration = 0
            Else
                scannerIsOn = False
                timerScanner.Enabled = False
            End If
        Else
            UndoChangeToTextBoxScanTag()
        End If
    End Sub

    Private Sub ButtonSave_KeyUp( _
        ByVal sender As Object, _
        ByVal e As System.Windows.Forms.KeyEventArgs _
        )
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonBack_KeyUp( _
        ByVal sender As Object, _
        ByVal e As System.Windows.Forms.KeyEventArgs _
        ) Handles ButtonBack.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonReset_KeyUp( _
        ByVal sender As Object, _
        ByVal e As System.Windows.Forms.KeyEventArgs _
        )
        ActivateCommonShortcut(e)
    End Sub
#End Region

    Private Sub InsertPartialScannedDataToDB()
        If _tmpBarcodeTagData.Count > 0 Then
            Dim confirm As Boolean = ResetScannerAndShowConfirmationMessage( _
                "Save Data Yang Telah Di Scan ke Database?", _
                "Konfirmasi" _
                )
            If confirm = True Then
                'Check Connection Wifi
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
                ProcessSavingDataToDB()
            Else
                If bool_TempDataLoad Then
                    _tmpBarcodeTagData.Clear()
                    _tmpScannedQty.Clear()
                    _tmpTrinPartNo.Clear()
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
                "Data Yang Telah Di Scan Belum Tersimpan di Database. Lanjut?", _
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

    Private Sub LoadDefaultDisplayedValues()
        _lastScannedBarcodeTag = String.Empty
        TextBoxScanTag.Text = String.Empty

        LabelTrinPartNoValue.Text = "-"
        LabelCountedQtyValue.Text = "0"
        LabelTagCountValue.Text = "0"
    End Sub

    Private Sub ResetScanRelatedData()
        _tmpBarcodeTagData.Clear()
        _tmpScannedQty.Clear()
        _tmpTrinPartNo.Clear()
        _tmpStocktakeDataAll.Clear()
        _tmpStocktakeDataUpdate.Clear()
        _tagCount = 0
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
        TextBoxScanTag.Focus()
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

        Dim scannedQRCode As String = String.Empty

        Try
            scannedQRCode = myScanner.Input(Scanner.ALL_BUFFER).Trim
        Catch ex As Exception
            ResetScannerAndShowErrorMessage(ex.Message, "Scan Error")
        End Try

        If scannedQRCode.Length = 0 Then
            Exit Sub
        End If

        Dim splitQrCode As String() = scannedQRCode.Split(New Char() {";"c})

        If Not ScanValidator.isValidFinalQrCode(splitQrCode) Then
            ResetScannerAndShowErrorMessage( _
                "QR Code Tidak Valid!", _
                "Scan Error" _
                )
            Exit Sub
        End If

        ' QR Code has been scanned
        If ScanValidator.HasBarcodeTagBeenScanned( _
            myScanner, _
            timerScanner, _
            scannerIsOn, _
            _tmpBarcodeTagData, _
            splitQrCode(FinalQrCodeValues.BarcodeTag) _
            ) Then
            ResetScannerAndShowErrorMessage( _
                String.Format( _
                    "QR Code [{0}] Telah Di Scan Sebelumnya!", _
                    splitQrCode(FinalQrCodeValues.BarcodeTag) _
                    ), _
                "Error" _
                )
            Exit Sub
        End If

        BHTController.SoundOK()

        scannerOnDuration = 0

        _tmpBarcodeTagData.Add(splitQrCode(FinalQrCodeValues.BarcodeTag))

        _tmpScannedQty.Add(splitQrCode(FinalQrCodeValues.ActQty))

        _tmpTrinPartNo.Add(splitQrCode(FinalQrCodeValues.TrinPartCode))

        Dim singleStockData(4) As String

        singleStockData(StocktakeValues.TrinPartNo) = ( _
            splitQrCode(FinalQrCodeValues.TrinPartCode) _
            )
        singleStockData(StocktakeValues.BarcodeTag) = ( _
            splitQrCode(FinalQrCodeValues.BarcodeTag) _
            )
        singleStockData(StocktakeValues.ScannedQty) = ( _
            splitQrCode(FinalQrCodeValues.ActQty) _
            )
        singleStockData(StocktakeValues.StocktakePeriod) = ( _
            stocktakePeriod.ToString("yyyy-MM-dd HH:mm:ss") _
            )
        singleStockData(StocktakeValues.TakeDateTime) = ( _
            Now.ToString("yyyy-MM-dd HH:mm:ss") _
            )

        _tmpStocktakeDataAll.Add(singleStockData)

        _lastScannedBarcodeTag = splitQrCode(FinalQrCodeValues.BarcodeTag)
        TextBoxScanTag.Text = _lastScannedBarcodeTag

        LabelTrinPartNoValue.Text = splitQrCode(FinalQrCodeValues.TrinPartCode)
        LabelCountedQtyValue.Text = String.Format( _
            "{0:n0}", _
            splitQrCode(FinalQrCodeValues.ActQty) _
            )

        _tagCount += 1
        LabelTagCountValue.Text = CStr(_tagCount)

        '********************************** Add Sqlite process *************************************
        '************************************ Adri_[20170309] **************************************

        Try
            subGetSqliteData(_lastScannedBarcodeTag, _
                             scannedQRCode, _
                             splitQrCode(FinalQrCodeValues.TrinPartCode), _
                             splitQrCode(FinalQrCodeValues.ActQty) _
                             )

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

        Dim scannedStocktakes As List(Of Stocktake) = _
            StocktakeDB.GetBarcodesAndScannedQty( _
                _tmpBarcodeTagData, _
                stocktakePeriod, _
                True _
                )

        If scannedStocktakes Is Nothing Then

        End If
        ' Count > 0, some/all barcodes scanned has been saved
        ' in the database previously
        If scannedStocktakes.Count > 0 Then
            CloseNowLoadingAndEnableControls()
            DeleteDuplicateBarcodesFromStocktakeScanData(scannedStocktakes)
            UpdateItemQtyOnStocktakeScanWindow(scannedStocktakes)
        End If

        Dim unscannedStocktakes As List(Of Stocktake) = _
            StocktakeDB.GetBarcodesAndScannedQty( _
                _tmpBarcodeTagData, _
                stocktakePeriod, _
                False _
                )

        ' Find how many barcodes registered in the 
        ' stocktake table but have not been scanned.
        ' tmpStocktakeDataAll will then only contain unregistered barcode
        If unscannedStocktakes.Count > 0 Then
            For index As Integer = tmpStocktakeDataAll.Count - 1 To 0 Step -1
                Dim barcodeTag As String = ( _
                    tmpStocktakeDataAll(index)(StocktakeValues.BarcodeTag) _
                    )

                Dim stocktake As IEnumerable(Of Stocktake) = _
                    From x In unscannedStocktakes _
                    Where x.BARCODETAG = barcodeTag Select x

                If stocktake.Any Then
                    tmpStocktakeDataUpdate.Add(tmpStocktakeDataAll(index))
                    tmpStocktakeDataAll.RemoveAt(index)
                End If
            Next
        End If
        ' Insert the unregistered barcodes to stocktake table
        'modify 9j
        Dim AlreadyDelData As String = String.Empty
        Dim AlreadyDelCount As Integer = 0
        If _tmpStocktakeDataAll.Count > 0 Then
            Dim exceptionMsg As String = String.Empty
            If Not StocktakeDB.InsertData( _
                _tmpStocktakeDataAll, AlreadyDelData, AlreadyDelCount, exceptionMsg _
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
                exceptionMsg _
                ) Then
                CloseNowLoadingAndEnableControls()
                If exceptionMsg.Length > 0 Then
                    ResetScannerAndShowErrorMessage(exceptionMsg, "DB Error")
                End If

                ' Rollback inserted data in stocktake because failure to
                ' update data in stocktake table
                StocktakeDB.DeleteData(_tmpStocktakeDataAll, exceptionMsg)
                If exceptionMsg.Length > 0 Then
                    ResetScannerAndShowErrorMessage(exceptionMsg, "DB Error")
                End If

                Return False
            End If
        End If

        ResetScanRelatedData()
        LoadDefaultDisplayedValues()
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

        SqliteControllerDB.fncDelete(mstr_SqliteTable, "InputFlag = '3'")

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

    Private Sub DeleteDuplicateBarcodesFromStocktakeScanData(ByVal scannedStock As List(Of Stocktake))
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

        If tmpBarcodeTagData.Count = 0 Then
            _tmpStocktakeDataAll.Clear()

            _lastScannedBarcodeTag = String.Empty
            textBoxScanTag.Text = String.Empty
            LabelTrinPartNoValue.Text = "-"
            LabelCountedQtyValue.Text = "0"
        Else
            For index As Integer = ( _
                _tmpStocktakeDataAll.Count - 1 _
                ) To 0 Step -1
                Dim barcodeTag As String = ( _
                    _tmpStocktakeDataAll(index) _
                        (StocktakeValues.BarcodeTag) _
                    )
                If duplicatedBarcodes.Contains(barcodeTag) Then
                    _tmpStocktakeDataAll.RemoveAt(index)
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
    End Sub

    Private Sub UpdateItemQtyOnStocktakeScanWindow(ByVal scannedStock As List(Of Stocktake))
        Dim totalDuplicatedBarcodes As Integer = scannedStock.Count

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
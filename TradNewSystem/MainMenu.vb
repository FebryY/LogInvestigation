Option Strict On
Option Explicit On

Imports TradNewSystem.Helpers
Imports TradNewSystem.Model
Imports DNWA.BHTCL

Public Class MainMenu
    Protected Friend nowLoadingWindow As NowLoading

    Private _loginMenu As LoginMenu

    Private hasStarted As Boolean = False
    Private processKeypadPress As Boolean = True

    Public Sub New(ByVal _loginMenu As LoginMenu)
        InitializeComponent()

        Me._loginMenu = _loginMenu
    End Sub

    Private Sub MainMenu_Activated( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles MyBase.Activated
        If Not hasStarted Then
            _loginMenu.nowLoadingWindow.Close()
            _loginMenu.nowLoadingWindow = Nothing

            hasStarted = True
        End If
    End Sub

#Region "Keypad Press"
    Private Sub ActivateCommonShortcut(ByVal e As KeyEventArgs)
        Select Case e.KeyCode
            Case Windows.Forms.Keys.D1
                OpenShipmentWindow()
            Case Windows.Forms.Keys.D2
                OpenIDTagIntegrationWindow()
            Case Windows.Forms.Keys.D3
                OpenIDTagClassificationWindow()
            Case Windows.Forms.Keys.D4
                OpenStocktakeWindow()
            Case Windows.Forms.Keys.D5
                OpenOfflineStocktakeWindow()
            Case Windows.Forms.Keys.D6
                OpenProductModificationWindow()
            Case Windows.Forms.Keys.D9
                LogOffCurrentUser()
        End Select
    End Sub

    Private Sub MainMenu_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles MyBase.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonFullScreen_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonFullScreen.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonShipment_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonShipment.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonIdTagIntegration_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonIdTagIntegration.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonIdTagClassification_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonIdTagClassification.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonStocktake_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonStoctake.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonOfflineStocktake_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonOfflineStocktake.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonProductModification_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonProductModification.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonLogOff_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonLogOff.KeyUp
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

    Private Sub ButtonShipment_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonShipment.Click
        If Not WifiConectionCheck() Then Exit Sub
        EnableControl(False)
        OpenShipmentWindow()
        EnableControl(True)
    End Sub

    Private Sub ButtonIdTagIntegration_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonIdTagIntegration.Click
        If Not WifiConectionCheck() Then Exit Sub
        OpenIDTagIntegrationWindow()
    End Sub

    Private Sub ButtonIdTagClassification_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonIdTagClassification.Click
        If Not WifiConectionCheck() Then Exit Sub
        OpenIDTagClassificationWindow()
    End Sub

    Private Sub ButtonStocktake_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonStoctake.Click
        If Not WifiConectionCheck() Then Exit Sub
        OpenStocktakeWindow()
    End Sub

    Private Sub ButtonOfflineStocktake_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonOfflineStocktake.Click
        OpenOfflineStocktakeWindow()
    End Sub

    Private Sub ButtonProductModification_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonProductModification.Click
        If Not WifiConectionCheck() Then Exit Sub
        OpenProductModificationWindow()
    End Sub

    Private Sub ButtonLogOff_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonLogOff.Click
        LogOffCurrentUser()
    End Sub
#End Region

#Region "Open Window"
    Private Sub OpenShipmentWindow()
        nowLoadingWindow = New NowLoading
        nowLoadingWindow.Show()

        Dim shipmentFilterWindow As New ShipmentFilter(Me)

        shipmentFilterWindow.ShowDialog()
        shipmentFilterWindow = Nothing
    End Sub

    Private Sub OpenIDTagIntegrationWindow()
        Dim configData As ConfigData = ConfigMgr.GetConfigData
        Dim lineCode As String = ProductionActClassificationDB.fncGetLine( _
            configData.lineCode _
            )

        If String.IsNullOrEmpty(lineCode) Then
            DisplayMessage.ErrorMsg( _
                "Line Code " & configData.lineCode & " Tidak Ditemukan", _
                "Error" _
                )
        Else
            Dim TagIntegration As New frm_IdTagIntegration(lineCode)

            TagIntegration.ShowDialog()
            TagIntegration = Nothing
        End If
    End Sub

    Private Sub OpenIDTagClassificationWindow()
        Dim configData As ConfigData = ConfigMgr.GetConfigData
        Dim lineCode As String = ProductionActIntegrationDB.fncGetLine( _
            configData.lineCode _
            )

        If String.IsNullOrEmpty(lineCode) Then
            DisplayMessage.ErrorMsg( _
                "Line Code " & configData.lineCode & " Tidak Ditemukan", _
                "Error" _
                )
        Else
            Dim TagClassification As New frm_IdTagClassification(lineCode)

            TagClassification.ShowDialog()
            TagClassification = Nothing
        End If
    End Sub

    Private Sub OpenStocktakeWindow()
        nowLoadingWindow = New NowLoading
        nowLoadingWindow.Show()

        Dim stocktakeFilterWindow As New StocktakeFilter(Me)

        stocktakeFilterWindow.ShowDialog()
        stocktakeFilterWindow = Nothing
    End Sub

    Private Sub OpenOfflineStocktakeWindow()
        Dim exceptionMsg As String = String.Empty

        Dim stocktakePeriod As DateTime = ( _
            StocktakeDB.GetLatestStocktakePeriod(exceptionMsg) _
            )

        If exceptionMsg.Length > 0 Then
            DisplayMessage.ErrorMsg(exceptionMsg, "DB Error")
            Exit Sub
        End If

        If stocktakePeriod.Year = 1 Then
            DisplayMessage.ErrorMsg( _
                "Tidak Ada Barang Terdaftar Di Sistem Untuk Di Stocktake", _
                "Error" _
                )
        Else
            Dim offlineStocktakeWindow As New OfflineStocktake( _
                stocktakePeriod _
                )

            offlineStocktakeWindow.ShowDialog()
            offlineStocktakeWindow = Nothing
        End If
    End Sub

    Private Sub OpenProductModificationWindow()
        Dim configData As ConfigData = ConfigMgr.GetConfigData
        Dim lineCode As String = ProductionActIntegrationDB.fncGetLine( _
            configData.lineCode _
            )

        If String.IsNullOrEmpty(lineCode) Then
            DisplayMessage.ErrorMsg( _
                "Line Code " & configData.lineCode & " Tidak Ditemukan", _
                "Error" _
                )
        Else
            Dim ProductModification As New frm_ProductModScan(lineCode)

            ProductModification.ShowDialog()
            ProductModification = Nothing
        End If
    End Sub
#End Region

    Private Sub EnableControl(ByVal enable As Boolean)
        ButtonShipment.Enabled = enable
        ButtonIdTagIntegration.Enabled = enable
        ButtonIdTagClassification.Enabled = enable
        ButtonStoctake.Enabled = enable
        ButtonOfflineStocktake.Enabled = enable
        ButtonProductModification.Enabled = enable
        ButtonLogOff.Enabled = enable
        processKeypadPress = enable
    End Sub

    Private Sub LogOffCurrentUser()
        Dim confirm As Boolean = DisplayMessage.ConfirmationDialog( _
            "Apakah Anda Ingin Log Off?", _
            "Konfirmasi" _
            )

        If confirm = True Then
            Close()
        End If
    End Sub

    Function WifiConectionCheck() As Boolean
        Try
            If RF.SYNCHRONIZE(RF.SYNC_CHECK) <> 0 Then
                DisplayMessage.ErrorMsg("Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
                    "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.", "Error")
                Return False
            End If
        Catch ex As Exception
            If Err.Number = 5 Then
                DisplayMessage.ErrorMsg("Koneksi Wifi di HT tertutup." & vbCrLf & _
                    "Tunggu beberapa detik dan ulangi lagi.", "Error")
                Dim MyRf As RF
                MyRf = New RF()
                MyRf.OpenMode = RF.EN_OPEN_MODE.CONTINUOUSLY
                MyRf.Open = True
                Return False
            End If
        End Try
        Return True
    End Function
End Class
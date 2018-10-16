Option Strict On
Option Explicit On

Imports DNWA.BHTCL

Imports TradNewSystem.Helpers
Imports TradNewSystem.Model
Imports TradNewSystem.PocoClass
Imports MySql.Data.MySqlClient
Imports System.Net
Imports log4net

Public Class LoginMenu
    Protected Friend nowLoadingWindow As NowLoading

    Private processKeypadPress As Boolean = True

    Friend WithEvents myScanner As Scanner

    Private Sub LoginMenu_Load( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles MyBase.Load
        ToggleTaskbar.Show(False)
        ' Sample implement log4net
        'log4net.Config.XmlConfigurator.Configure()
        'Dim log As ILog = LogManager.GetLogger("TRLogger")

        'If log.IsInfoEnabled Then
        '    log.Info("Application [ConsoleApp] Start")
        'End If

        'log.Error("TRAD")
        'Dim a As Integer
        'MsgBox(SysInfo.Settings.MachineNumber)
        Dim strEntry As String = ""
        Dim strMultiSzValue As String() = {}
        'If (False = String.IsNullOrEmpty(m_iniFile.Item(NETWORK_SETTING_SECTION_NAME, NETWORK_REG_ENTRY_IPADDRESS))) Then
        'strEntry = "IpAddress"
        'strMultiSzValue = New String() {m_iniFile.Item("NETWORK_SETTING", "IpAddress")}
        'setRegValueMultiSz(strTcpIpSubKey, strEntry, strMultiSzValue)
        'End If
        'MsgBox(Dns.GetHostEntry(Dns.GetHostName()).AddressList(0).ToString())

        LblTanggal.Text = "Tanggal: " & Date.Now.Date.ToString("dd'-'MMMM'-'yyyy")
        LogManager.Shutdown()
    End Sub

    Private Sub LoginMenu_Closed( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles MyBase.Closed
        BHTController.DisposeScanner(myScanner)

        ToggleTaskbar.Show(True)
    End Sub

#Region "Keypad Press"
    Private Sub ActivateCommonShortcut(ByVal e As KeyEventArgs)
        If processKeypadPress Then
            Select Case e.KeyCode
                Case Windows.Forms.Keys.F1
                    ValidateUserLogin()
                Case Windows.Forms.Keys.F2
                    BHTController.TurnOffTerminal()
            End Select
        End If
    End Sub

    Private Sub ButtonCloseWindow_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonCloseWindow.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonFullScreen_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonFullScreen.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub LoginMenu_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles MyBase.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub TextBoxUser_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles TextBoxUser.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub TextBoxPassword_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles TextBoxPassword.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonLogin_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonLogin.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonPowerOff_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonPowerOff.KeyUp
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

    Private Sub ButtonCloseWindow_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonCloseWindow.Click
        Dim confirm As Boolean = DisplayMessage.ConfirmationDialog( _
            "Keluar dari program?", _
            "Konfirmasi" _
            )

        If confirm = True Then
            Close()
        End If
    End Sub

    Private Sub PictureBoxLogo_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles PictureBoxLogo.Click
        If TextBoxUser.Text.ToLower.Trim = "admin" _
            And TextBoxPassword.Text.ToLower.Trim = "admin" Then
            Dim appConfig As New AppConfig

            appConfig.ShowDialog()

            appConfig = Nothing
        End If
    End Sub

    Private Sub ButtonLogin_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonLogin.Click

        ValidateUserLogin()

    End Sub

    Private Sub ButtonPowerOff_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonPowerOff.Click
        BHTController.TurnOffTerminal()
    End Sub
#End Region

#Region "Input Validation"
    Private Function isInputtedUserValid() As Boolean
        Dim validUser As Boolean = True

        If TextBoxUser.TextLength < 4 Or TextBoxUser.TextLength > 20 Then
            LabelInvalidUser.Visible = True

            validUser = False
        Else
            LabelInvalidUser.Visible = False
        End If

        Return validUser
    End Function

    Private Function isInputtedPasswordValid() As Boolean
        Dim validPassword As Boolean = True

        If TextBoxPassword.TextLength < 4 _
            Or TextBoxPassword.TextLength > 12 Then
            LabelInvalidPassword.Visible = True

            validPassword = False
        Else
            LabelInvalidPassword.Visible = False
        End If

        Return validPassword
    End Function
#End Region

#Region "Login User"
    Private Sub HideAllErrorMessages()
        LabelInvalidUser.Visible = False
        LabelInvalidPassword.Visible = False
    End Sub

    Private Sub ValidateUserLogin()

        log4net.Config.XmlConfigurator.Configure()
        Dim log As ILog = LogManager.GetLogger("TRADLogger")

        If isInputtedUserValid() And isInputtedPasswordValid() Then
            Try
                If RF.SYNCHRONIZE(RF.SYNC_CHECK) <> 0 Then

                    log.Info("Start Info WifiConectionCheck Signal Distance Login Menu method ValidateUserLogin")
                    log.Info("Additional Message: Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
                        "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.")
                    log.Info("End Info WifiConectionCheck Signal Distance Login Menu method ValidateUserLogin")

                    DisplayMessage.ErrorMsg("Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
                        "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.", "Error")

                    If Date.Now.Year <= 2017 Then
                        DisplayMessage.ErrorMsg( _
                            "Mohon sesuaikan tanggal pada HT dengan tanggal sekarang", _
                            "Date Error" _
                            )
                    End If

                    Exit Sub
                End If
            Catch ex As Exception

                log.Error("Start Error WifiConectionCheck Login Menu method ValidateUserLogin")

                If Err.Number = 5 Then

                    log.Error("Additional Message: Koneksi Wifi di HT tertutup." & vbCrLf & _
                    "Tunggu beberapa detik dan ulangi lagi.")
                    log.Error("Error Number: " & Err.Number & vbCrLf & "Error Description: " & Err.Description & vbCrLf, ex)
                    log.Error("End Error WifiConectionCheck Login Menu ")

                    DisplayMessage.ErrorMsg("Koneksi Wifi di HT tertutup." & vbCrLf & _
                        "Tunggu beberapa detik dan ulangi lagi.", "Error")

                    If Date.Now.Year <= 2017 Then
                        DisplayMessage.ErrorMsg( _
                            "Mohon sesuaikan tanggal pada HT dengan tanggal sekarang", _
                            "Date Error" _
                            )
                    End If
                    Dim MyRf As RF
                    MyRf = New RF()
                    MyRf.OpenMode = RF.EN_OPEN_MODE.CONTINUOUSLY

                    MyRf.Open = True
                    Exit Sub
                End If

                log.Error("Error Number: " & Err.Number & vbCrLf & "Error Description: " & Err.Description & vbCrLf, ex)
                log.Error("End Error WifiConectionCheck Login Menu method ValidateUserLogin")

            End Try

            LogManager.Shutdown()

            HideAllErrorMessages()

            nowLoadingWindow = New NowLoading
            nowLoadingWindow.Show()

            EnableControl(False)

            Dim isDateCorrect As QueryRetValue = UserMasterDB.ChangeHTDateWithDBDate()

            If isDateCorrect = QueryRetValue.ValueFalse Then
                DisplayMessage.ErrorMsg( _
                "Tanggal di server database dan di HT berbeda, Mohon disesuaikan", _
                "Date Error" _
                )

                nowLoadingWindow.Close()
                nowLoadingWindow = Nothing
                EnableControl(True)
                Exit Sub
            ElseIf isDateCorrect = QueryRetValue.ValueError Then
                nowLoadingWindow.Close()
                nowLoadingWindow = Nothing
                EnableControl(True)
                Exit Sub
            End If

            LblTanggal.Text = "Tanggal: " & Date.Now.Date.ToString("dd'-'MMMM'-'yyyy")

            Dim userIsExist As QueryRetValue = UserMasterDB.IsUserExist( _
                TextBoxUser.Text.Trim, _
                Sha1Generator.Generate(TextBoxPassword.Text.Trim) _
                )

            If userIsExist = QueryRetValue.ValueTrue Then
                TemporaryData.loggedInUserID = TextBoxUser.Text.Trim

                Dim mainMenuWindow As New MainMenu(Me)

                mainMenuWindow.ShowDialog()
                mainMenuWindow = Nothing

                TemporaryData.loggedInUserID = String.Empty
            End If

            If userIsExist = QueryRetValue.ValueFalse _
                Or userIsExist = QueryRetValue.ValueError Then
                nowLoadingWindow.Close()
                nowLoadingWindow = Nothing
            End If

            If userIsExist = QueryRetValue.ValueFalse Then
                DisplayMessage.ErrorMsg( _
                    "Masukkan User/Password Yang Benar dan Cobalah Sekali Lagi!", _
                    "Login Error" _
                    )
            End If

            EnableControl(True)
        End If

        TextBoxPassword.Text = ""
    End Sub
    Private Sub EnableControl(ByVal enable As Boolean)
        ButtonCloseWindow.Enabled = enable
        ButtonLogin.Enabled = enable
        ButtonPowerOff.Enabled = enable
        processKeypadPress = enable
    End Sub
#End Region

#Region "Process Scanner"
    Private Sub TextBoxUser_GotFocus( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles TextBoxUser.GotFocus
        BHTController.InitialiseScanner( _
            myScanner, _
            ScannerCodeType.Code39, _
            ScannerReadMode.Momentary _
            )
    End Sub

    Private Sub TextBoxUser_LostFocus( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles TextBoxUser.LostFocus
        BHTController.DisposeScanner(myScanner)
    End Sub

    Private Sub myScanner_OnDone( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles myScanner.OnDone
        Try
            Dim scannedCode = myScanner.Input(Scanner.ALL_BUFFER)

            If TextBoxUser.Focus Then
                BHTController.SoundOK()

                TextBoxUser.Text = scannedCode

                TextBoxPassword.Focus()
            End If
        Catch ex As Exception
            DisplayMessage.ErrorMsg(ex.Message, "Scan Error")
        End Try
    End Sub
#End Region

End Class

Option Strict On
Option Explicit On

Imports System.Net

Imports TradNewSystem.Helpers
Imports System.Data
Imports MySql.Data.MySqlClient

Public Class AppConfig
    Private configData As ConfigMgr.ConfigData

    Private processKeypadPress As Boolean = True

    Private Const minUserLength As Integer = 4
    Private Const maxUserLength As Integer = 20

    Private Const minDBNameLength As Integer = 4
    Private Const maxDBNameLength As Integer = 100

    Private Const minLineCodeLength As Integer = 2
    Private Const maxLineCodeLength As Integer = 20

    Private Sub DBConfig_Load( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles MyBase.Load
        configData = ConfigMgr.GetConfigData()

        LoadDefaultConfigData()
    End Sub

    Private Sub LoadDefaultConfigData()
        TextBoxIPAddress.Text = configData.dbIPAddress
        NumericUpDownPort.Value = CDec(configData.dbPort)
        TextBoxUser.Text = configData.dbUser
        TextBoxPassword.Text = configData.dbPassword
        TextBoxDBName.Text = configData.dbName
        TextBoxLineCode.Text = configData.lineCode
        txtID.Text = configData.ID
    End Sub

#Region "Keypad Press"
    Private Sub ActivateCommonShortcut(ByVal e As KeyEventArgs)
        If processKeypadPress Then
            Select Case e.KeyCode
                Case Windows.Forms.Keys.F1
                    SaveConfigData()
                Case Windows.Forms.Keys.F2
                    CloseWindow()
                Case Windows.Forms.Keys.F3
                    TestDBConnection()
                Case Windows.Forms.Keys.F4
                    ResetConfigData()
            End Select
        End If
    End Sub

    Private Sub ButtonFullScreen_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonFullScreen.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub DBConfig_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles MyBase.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub tabControl1_KeyUp( _
            ByVal sender As Object, _
            ByVal e As KeyEventArgs _
            ) Handles tabControl1.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub TextBoxIPAddress_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles TextBoxIPAddress.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub NumericUpDownPort_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles NumericUpDownPort.KeyUp
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

    Private Sub TextBoxDBName_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles TextBoxDBName.KeyUp
        ActivateCommonShortcut(e)
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

    Private Sub ButtonTestCon_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonTestCon.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonReset_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonReset.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub TextBoxLineCode_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles TextBoxLineCode.KeyUp
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
        SaveConfigData()
    End Sub

    Private Sub ButtonBack_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonBack.Click
        CloseWindow()
    End Sub

    Private Sub ButtonTestCon_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonTestCon.Click
        TestDBConnection()
    End Sub

    Private Sub ButtonReset_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonReset.Click
        ResetConfigData()
    End Sub
#End Region

#Region "Input Validation"
    Private Function IsInputtedIPAddressValid() As Boolean
        Dim isValidIPAddress As Boolean = True

        If Not CommonLib.IsValidIPv4(TextBoxIPAddress.Text.Trim) Then
            LabelInvalidIPAddress.Visible = True

            isValidIPAddress = False
        Else
            LabelInvalidIPAddress.Visible = False
        End If

        Return isValidIPAddress
    End Function

    Private Function IsInputtedUserValid() As Boolean
        Dim userIsValid As Boolean = True

        If TextBoxUser.Text.Trim.Length < minUserLength OrElse _
            TextBoxUser.Text.Trim.Length > maxUserLength Then
            LabelInvalidUser.Visible = True

            userIsValid = False
        Else
            LabelInvalidUser.Visible = False
        End If

        Return userIsValid
    End Function

    Private Function IsInputtedDatabaseNameValid() As Boolean
        Dim databaseNameIsValid As Boolean = True

        If TextBoxDBName.Text.Trim.Length < minDBNameLength _
            OrElse TextBoxDBName.Text.Trim.Length > maxDBNameLength Then
            LabelInvalidDBName.Visible = True

            databaseNameIsValid = False
        Else
            LabelInvalidDBName.Visible = False
        End If

        Return databaseNameIsValid
    End Function

    Private Function IsInputtedLineCodeValid() As Boolean
        Dim lineCodeIsValid As Boolean = True

        If TextBoxLineCode.Text.Trim.Length < minLineCodeLength _
            OrElse TextBoxLineCode.Text.Trim.Length > maxLineCodeLength Then

            LabelInvalidLineCode.Visible = True

            lineCodeIsValid = False
        Else
            LabelInvalidLineCode.Visible = False
        End If

        Return lineCodeIsValid
    End Function

    Private Function IsValidConfig() As Boolean
        Return IsInputtedIPAddressValid() _
            AndAlso IsInputtedUserValid() _
            AndAlso IsInputtedDatabaseNameValid() _
            AndAlso IsInputtedLineCodeValid()
    End Function
#End Region

    Private Function ChangesAreMade() As Boolean
        Return TextBoxIPAddress.Text.Trim <> configData.dbIPAddress _
            Or TextBoxUser.Text.Trim <> configData.dbUser _
            Or TextBoxPassword.Text.Trim <> configData.dbPassword _
            Or NumericUpDownPort.Value <> CDbl(configData.dbPort) _
            Or TextBoxDBName.Text.Trim <> configData.dbName _
            Or TextBoxLineCode.Text.Trim <> configData.lineCode _
            Or txtID.Text.Trim <> configData.ID
    End Function

    Private Sub SaveConfigData()
        If Not ChangesAreMade() Then
            HideAllErrorMessages()

            DisplayMessage.ErrorMsg( _
                "Tidak Ada Perubahan Untuk Disimpan", _
                "Error" _
                )
        Else
            If IsValidConfig() Then
                HideAllErrorMessages()

                Dim confirm As Boolean = _
                    DisplayMessage.ConfirmationDialog( _
                        "Apakah Anda Ingin Menyimpan Config Baru?", _
                        "Konfirmasi" _
                        )

                If confirm = True Then
                    configData.dbIPAddress = TextBoxIPAddress.Text.Trim
                    configData.dbUser = TextBoxUser.Text.Trim
                    configData.dbPassword = TextBoxPassword.Text.Trim
                    configData.dbPort = NumericUpDownPort.Text.Trim
                    configData.dbName = TextBoxDBName.Text.Trim
                    configData.lineCode = TextBoxLineCode.Text.Trim
                    configData.ID = txtID.Text.Trim

                    ConfigMgr.SaveConfigData(configData)

                    DisplayMessage.OkMsg( _
                        "Config Yang Baru Telah Tersimpan", _
                        "Sukses" _
                        )

                    Close()
                End If
            End If
        End If
    End Sub

    Private Sub TestDBConnection()
        If IsValidConfig() Then
            HideAllErrorMessages()

            Dim nowLoadingWindow As New NowLoading
            nowLoadingWindow.Show()

            EnableControls(False)

            Try
                Using connection As IDbConnection = New MySqlConnection( _
                    String.Format( _
                        CommonLib.GetRawConnectionString(), _
                        TextBoxIPAddress.Text.Trim, _
                        NumericUpDownPort.Value, _
                        TextBoxUser.Text.Trim, _
                        TextBoxPassword.Text.Trim, _
                        TextBoxDBName.Text.Trim) _
                        )

                    connection.Open()

                    nowLoadingWindow.Close()

                    DisplayMessage.OkMsg( _
                        "Koneksi ke Database Berhasil", _
                        "Sukses" _
                        )
                End Using
            Catch ex As Exception
                DisplayMessage.ErrorMsg(ex.Message, "Error")
            End Try

            nowLoadingWindow.Close()
            nowLoadingWindow = Nothing

            EnableControls(True)
        End If
    End Sub

    Private Sub EnableControls(ByVal enable As Boolean)
        ButtonSave.Enabled = enable
        ButtonReset.Enabled = enable
        ButtonTestCon.Enabled = enable
        ButtonReset.Enabled = enable
        processKeypadPress = enable
    End Sub

    Private Sub ResetConfigData()
        If ChangesAreMade() Then

            Dim confirm As Boolean = _
                DisplayMessage.ConfirmationDialog( _
                    "Perubahan Belum Tersimpan. Lanjut?", _
                    "Konfirmasi" _
                    )

            If confirm = True Then
                LoadDefaultConfigData()

                HideAllErrorMessages()
            End If
        End If
    End Sub

    Private Sub HideAllErrorMessages()
        LabelInvalidIPAddress.Visible = False
        LabelInvalidUser.Visible = False
        LabelInvalidDBName.Visible = False
    End Sub

    Private Sub CloseWindow()
        If ChangesAreMade() Then
            Dim confirm As Boolean = DisplayMessage.ConfirmationDialog( _
                "Perubahan Belum Tersimpan. Lanjut?", _
                "Konfirmasi" _
                )

            If confirm = True Then
                Close()
            End If
        Else
            Close()
        End If
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtID.KeyPress
        
    End Sub

    Private Function IsAlphaNum(ByVal strInputText As String) As Boolean
        Return System.Text.RegularExpressions.Regex.IsMatch(strInputText, "^[A-Z1-9]+$")
    End Function

    Private Sub txtID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtID.TextChanged
        If Not IsAlphaNum(txtID.Text) Then
            txtID.Text = ""
        End If
    End Sub
End Class
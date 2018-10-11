<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class LoginMenu
    Inherits System.Windows.Forms.Form

    'Form は、コンポーネント一覧に後処理を実行するために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタでこのプロシージャを変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LoginMenu))
        Me.LabelTitle1 = New System.Windows.Forms.Label
        Me.TextBoxUser = New System.Windows.Forms.TextBox
        Me.TextBoxPassword = New System.Windows.Forms.TextBox
        Me.LabelUser = New System.Windows.Forms.Label
        Me.LabelPassword = New System.Windows.Forms.Label
        Me.LabelTitle2 = New System.Windows.Forms.Label
        Me.LabelInvalidUser = New System.Windows.Forms.Label
        Me.LabelInvalidPassword = New System.Windows.Forms.Label
        Me.PictureBoxLogo = New System.Windows.Forms.PictureBox
        Me.PanelTitle = New System.Windows.Forms.Panel
        Me.ButtonFullScreen = New System.Windows.Forms.Button
        Me.ButtonCloseWindow = New System.Windows.Forms.Button
        Me.LabelWindowTitle = New System.Windows.Forms.Label
        Me.ButtonLogin = New System.Windows.Forms.Button
        Me.ButtonPowerOff = New System.Windows.Forms.Button
        Me.LabelProgramVersion = New System.Windows.Forms.Label
        Me.LblTanggal = New System.Windows.Forms.Label
        Me.PanelTitle.SuspendLayout()
        Me.SuspendLayout()
        '
        'LabelTitle1
        '
        Me.LabelTitle1.Font = New System.Drawing.Font("Tahoma", 14.0!, System.Drawing.FontStyle.Bold)
        Me.LabelTitle1.Location = New System.Drawing.Point(35, 144)
        Me.LabelTitle1.Name = "LabelTitle1"
        Me.LabelTitle1.Size = New System.Drawing.Size(262, 20)
        Me.LabelTitle1.Text = "PT. TRAD INDONESIA"
        '
        'TextBoxUser
        '
        Me.TextBoxUser.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.TextBoxUser.Location = New System.Drawing.Point(121, 217)
        Me.TextBoxUser.Name = "TextBoxUser"
        Me.TextBoxUser.Size = New System.Drawing.Size(176, 26)
        Me.TextBoxUser.TabIndex = 1
        '
        'TextBoxPassword
        '
        Me.TextBoxPassword.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.TextBoxPassword.Location = New System.Drawing.Point(121, 267)
        Me.TextBoxPassword.Name = "TextBoxPassword"
        Me.TextBoxPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBoxPassword.Size = New System.Drawing.Size(176, 26)
        Me.TextBoxPassword.TabIndex = 2
        '
        'LabelUser
        '
        Me.LabelUser.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelUser.Location = New System.Drawing.Point(21, 217)
        Me.LabelUser.Name = "LabelUser"
        Me.LabelUser.Size = New System.Drawing.Size(94, 26)
        Me.LabelUser.Text = "User"
        '
        'LabelPassword
        '
        Me.LabelPassword.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelPassword.Location = New System.Drawing.Point(21, 270)
        Me.LabelPassword.Name = "LabelPassword"
        Me.LabelPassword.Size = New System.Drawing.Size(94, 23)
        Me.LabelPassword.Text = "Password"
        '
        'LabelTitle2
        '
        Me.LabelTitle2.Font = New System.Drawing.Font("Tahoma", 14.0!, System.Drawing.FontStyle.Bold)
        Me.LabelTitle2.Location = New System.Drawing.Point(35, 164)
        Me.LabelTitle2.Name = "LabelTitle2"
        Me.LabelTitle2.Size = New System.Drawing.Size(262, 20)
        Me.LabelTitle2.Text = "NEW SYSTEM"
        '
        'LabelInvalidUser
        '
        Me.LabelInvalidUser.BackColor = System.Drawing.Color.Khaki
        Me.LabelInvalidUser.ForeColor = System.Drawing.Color.Red
        Me.LabelInvalidUser.Location = New System.Drawing.Point(21, 246)
        Me.LabelInvalidUser.Name = "LabelInvalidUser"
        Me.LabelInvalidUser.Size = New System.Drawing.Size(276, 18)
        Me.LabelInvalidUser.Text = "User Harus Antara 4 - 20 Karakter"
        Me.LabelInvalidUser.Visible = False
        '
        'LabelInvalidPassword
        '
        Me.LabelInvalidPassword.BackColor = System.Drawing.Color.Khaki
        Me.LabelInvalidPassword.ForeColor = System.Drawing.Color.Red
        Me.LabelInvalidPassword.Location = New System.Drawing.Point(21, 296)
        Me.LabelInvalidPassword.Name = "LabelInvalidPassword"
        Me.LabelInvalidPassword.Size = New System.Drawing.Size(276, 16)
        Me.LabelInvalidPassword.Text = "Password Harus Antara 4 - 12 Karakter"
        Me.LabelInvalidPassword.Visible = False
        '
        'PictureBoxLogo
        '
        Me.PictureBoxLogo.Image = CType(resources.GetObject("PictureBoxLogo.Image"), System.Drawing.Image)
        Me.PictureBoxLogo.Location = New System.Drawing.Point(21, 61)
        Me.PictureBoxLogo.Name = "PictureBoxLogo"
        Me.PictureBoxLogo.Size = New System.Drawing.Size(276, 80)
        '
        'PanelTitle
        '
        Me.PanelTitle.BackColor = System.Drawing.Color.Navy
        Me.PanelTitle.Controls.Add(Me.ButtonFullScreen)
        Me.PanelTitle.Controls.Add(Me.ButtonCloseWindow)
        Me.PanelTitle.Controls.Add(Me.LabelWindowTitle)
        Me.PanelTitle.Location = New System.Drawing.Point(0, 0)
        Me.PanelTitle.Name = "PanelTitle"
        Me.PanelTitle.Size = New System.Drawing.Size(320, 25)
        '
        'ButtonFullScreen
        '
        Me.ButtonFullScreen.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonFullScreen.Location = New System.Drawing.Point(271, 2)
        Me.ButtonFullScreen.Name = "ButtonFullScreen"
        Me.ButtonFullScreen.Size = New System.Drawing.Size(20, 20)
        Me.ButtonFullScreen.TabIndex = 16
        Me.ButtonFullScreen.TabStop = False
        Me.ButtonFullScreen.Text = "F"
        '
        'ButtonCloseWindow
        '
        Me.ButtonCloseWindow.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonCloseWindow.Location = New System.Drawing.Point(297, 2)
        Me.ButtonCloseWindow.Name = "ButtonCloseWindow"
        Me.ButtonCloseWindow.Size = New System.Drawing.Size(20, 20)
        Me.ButtonCloseWindow.TabIndex = 5
        Me.ButtonCloseWindow.TabStop = False
        Me.ButtonCloseWindow.Text = "X"
        '
        'LabelWindowTitle
        '
        Me.LabelWindowTitle.BackColor = System.Drawing.Color.Navy
        Me.LabelWindowTitle.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.LabelWindowTitle.ForeColor = System.Drawing.Color.White
        Me.LabelWindowTitle.Location = New System.Drawing.Point(11, 4)
        Me.LabelWindowTitle.Name = "LabelWindowTitle"
        Me.LabelWindowTitle.Size = New System.Drawing.Size(204, 21)
        Me.LabelWindowTitle.Text = "Trad New System"
        '
        'ButtonLogin
        '
        Me.ButtonLogin.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonLogin.Location = New System.Drawing.Point(21, 387)
        Me.ButtonLogin.Name = "ButtonLogin"
        Me.ButtonLogin.Size = New System.Drawing.Size(120, 20)
        Me.ButtonLogin.TabIndex = 3
        Me.ButtonLogin.TabStop = False
        Me.ButtonLogin.Text = "F1 Login"
        '
        'ButtonPowerOff
        '
        Me.ButtonPowerOff.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonPowerOff.Location = New System.Drawing.Point(177, 387)
        Me.ButtonPowerOff.Name = "ButtonPowerOff"
        Me.ButtonPowerOff.Size = New System.Drawing.Size(120, 20)
        Me.ButtonPowerOff.TabIndex = 4
        Me.ButtonPowerOff.TabStop = False
        Me.ButtonPowerOff.Text = "F2 Power Off"
        '
        'LabelProgramVersion
        '
        Me.LabelProgramVersion.Location = New System.Drawing.Point(177, 419)
        Me.LabelProgramVersion.Name = "LabelProgramVersion"
        Me.LabelProgramVersion.Size = New System.Drawing.Size(120, 20)
        Me.LabelProgramVersion.Text = "Ver. 0.9m"
        Me.LabelProgramVersion.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'LblTanggal
        '
        Me.LblTanggal.Location = New System.Drawing.Point(135, 28)
        Me.LblTanggal.Name = "LblTanggal"
        Me.LblTanggal.Size = New System.Drawing.Size(180, 20)
        Me.LblTanggal.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'LoginMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.Khaki
        Me.ClientSize = New System.Drawing.Size(320, 480)
        Me.ControlBox = False
        Me.Controls.Add(Me.LblTanggal)
        Me.Controls.Add(Me.LabelProgramVersion)
        Me.Controls.Add(Me.ButtonPowerOff)
        Me.Controls.Add(Me.ButtonLogin)
        Me.Controls.Add(Me.PanelTitle)
        Me.Controls.Add(Me.PictureBoxLogo)
        Me.Controls.Add(Me.LabelInvalidPassword)
        Me.Controls.Add(Me.LabelInvalidUser)
        Me.Controls.Add(Me.LabelTitle2)
        Me.Controls.Add(Me.LabelPassword)
        Me.Controls.Add(Me.LabelUser)
        Me.Controls.Add(Me.TextBoxPassword)
        Me.Controls.Add(Me.TextBoxUser)
        Me.Controls.Add(Me.LabelTitle1)
        Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoginMenu"
        Me.Text = "Login"
        Me.PanelTitle.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LabelTitle1 As System.Windows.Forms.Label
    Friend WithEvents TextBoxUser As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxPassword As System.Windows.Forms.TextBox
    Friend WithEvents LabelUser As System.Windows.Forms.Label
    Friend WithEvents LabelPassword As System.Windows.Forms.Label
    Friend WithEvents LabelTitle2 As System.Windows.Forms.Label
    Friend WithEvents LabelInvalidUser As System.Windows.Forms.Label
    Friend WithEvents LabelInvalidPassword As System.Windows.Forms.Label
    Friend WithEvents PictureBoxLogo As System.Windows.Forms.PictureBox
    Friend WithEvents PanelTitle As System.Windows.Forms.Panel
    Friend WithEvents LabelWindowTitle As System.Windows.Forms.Label
    Friend WithEvents ButtonLogin As System.Windows.Forms.Button
    Friend WithEvents ButtonPowerOff As System.Windows.Forms.Button
    Friend WithEvents ButtonCloseWindow As System.Windows.Forms.Button
    Friend WithEvents ButtonFullScreen As System.Windows.Forms.Button
    Friend WithEvents LabelProgramVersion As System.Windows.Forms.Label
    Friend WithEvents LblTanggal As System.Windows.Forms.Label

End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class AppConfig
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.LabelTitle = New System.Windows.Forms.Label
        Me.PanelTitle = New System.Windows.Forms.Panel
        Me.ButtonFullScreen = New System.Windows.Forms.Button
        Me.LabelWindowTitle = New System.Windows.Forms.Label
        Me.ButtonSave = New System.Windows.Forms.Button
        Me.ButtonBack = New System.Windows.Forms.Button
        Me.ButtonTestCon = New System.Windows.Forms.Button
        Me.ButtonReset = New System.Windows.Forms.Button
        Me.tabControl1 = New System.Windows.Forms.TabControl
        Me.tabDB = New System.Windows.Forms.TabPage
        Me.LabelInvalidDBName = New System.Windows.Forms.Label
        Me.LabelInvalidUser = New System.Windows.Forms.Label
        Me.LabelInvalidIPAddress = New System.Windows.Forms.Label
        Me.LabelDBName = New System.Windows.Forms.Label
        Me.TextBoxDBName = New System.Windows.Forms.TextBox
        Me.NumericUpDownPort = New System.Windows.Forms.NumericUpDown
        Me.LabelIPAddress = New System.Windows.Forms.Label
        Me.TextBoxIPAddress = New System.Windows.Forms.TextBox
        Me.LabelUser = New System.Windows.Forms.Label
        Me.TextBoxUser = New System.Windows.Forms.TextBox
        Me.LabelPort = New System.Windows.Forms.Label
        Me.LabelPassword = New System.Windows.Forms.Label
        Me.TextBoxPassword = New System.Windows.Forms.TextBox
        Me.tabPrinter = New System.Windows.Forms.TabPage
        Me.LabelInvalidLineCode = New System.Windows.Forms.Label
        Me.LabelLineCode = New System.Windows.Forms.Label
        Me.TextBoxLineCode = New System.Windows.Forms.TextBox
        Me.tabID = New System.Windows.Forms.TabPage
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtID = New System.Windows.Forms.TextBox
        Me.PanelTitle.SuspendLayout()
        Me.tabControl1.SuspendLayout()
        Me.tabDB.SuspendLayout()
        Me.tabPrinter.SuspendLayout()
        Me.tabID.SuspendLayout()
        Me.SuspendLayout()
        '
        'LabelTitle
        '
        Me.LabelTitle.Font = New System.Drawing.Font("Tahoma", 16.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.LabelTitle.Location = New System.Drawing.Point(0, 61)
        Me.LabelTitle.Name = "LabelTitle"
        Me.LabelTitle.Size = New System.Drawing.Size(318, 30)
        Me.LabelTitle.Text = "Config"
        Me.LabelTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'PanelTitle
        '
        Me.PanelTitle.BackColor = System.Drawing.Color.Navy
        Me.PanelTitle.Controls.Add(Me.ButtonFullScreen)
        Me.PanelTitle.Controls.Add(Me.LabelWindowTitle)
        Me.PanelTitle.Location = New System.Drawing.Point(0, 0)
        Me.PanelTitle.Name = "PanelTitle"
        Me.PanelTitle.Size = New System.Drawing.Size(320, 25)
        '
        'ButtonFullScreen
        '
        Me.ButtonFullScreen.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonFullScreen.Location = New System.Drawing.Point(297, 2)
        Me.ButtonFullScreen.Name = "ButtonFullScreen"
        Me.ButtonFullScreen.Size = New System.Drawing.Size(20, 20)
        Me.ButtonFullScreen.TabIndex = 16
        Me.ButtonFullScreen.TabStop = False
        Me.ButtonFullScreen.Text = "F"
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
        'ButtonSave
        '
        Me.ButtonSave.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonSave.Location = New System.Drawing.Point(21, 387)
        Me.ButtonSave.Name = "ButtonSave"
        Me.ButtonSave.Size = New System.Drawing.Size(120, 20)
        Me.ButtonSave.TabIndex = 28
        Me.ButtonSave.TabStop = False
        Me.ButtonSave.Text = "F1 Save"
        '
        'ButtonBack
        '
        Me.ButtonBack.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonBack.Location = New System.Drawing.Point(177, 387)
        Me.ButtonBack.Name = "ButtonBack"
        Me.ButtonBack.Size = New System.Drawing.Size(120, 20)
        Me.ButtonBack.TabIndex = 29
        Me.ButtonBack.TabStop = False
        Me.ButtonBack.Text = "F2 Back"
        '
        'ButtonTestCon
        '
        Me.ButtonTestCon.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonTestCon.Location = New System.Drawing.Point(21, 413)
        Me.ButtonTestCon.Name = "ButtonTestCon"
        Me.ButtonTestCon.Size = New System.Drawing.Size(120, 20)
        Me.ButtonTestCon.TabIndex = 30
        Me.ButtonTestCon.TabStop = False
        Me.ButtonTestCon.Text = "F3 Test Con."
        '
        'ButtonReset
        '
        Me.ButtonReset.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonReset.Location = New System.Drawing.Point(177, 413)
        Me.ButtonReset.Name = "ButtonReset"
        Me.ButtonReset.Size = New System.Drawing.Size(120, 20)
        Me.ButtonReset.TabIndex = 31
        Me.ButtonReset.TabStop = False
        Me.ButtonReset.Text = "F4 Reset"
        '
        'tabControl1
        '
        Me.tabControl1.Controls.Add(Me.tabDB)
        Me.tabControl1.Controls.Add(Me.tabPrinter)
        Me.tabControl1.Controls.Add(Me.tabID)
        Me.tabControl1.Location = New System.Drawing.Point(0, 94)
        Me.tabControl1.Name = "tabControl1"
        Me.tabControl1.SelectedIndex = 0
        Me.tabControl1.Size = New System.Drawing.Size(320, 287)
        Me.tabControl1.TabIndex = 1
        Me.tabControl1.TabStop = False
        '
        'tabDB
        '
        Me.tabDB.BackColor = System.Drawing.Color.Gainsboro
        Me.tabDB.Controls.Add(Me.LabelInvalidDBName)
        Me.tabDB.Controls.Add(Me.LabelInvalidUser)
        Me.tabDB.Controls.Add(Me.LabelInvalidIPAddress)
        Me.tabDB.Controls.Add(Me.LabelDBName)
        Me.tabDB.Controls.Add(Me.TextBoxDBName)
        Me.tabDB.Controls.Add(Me.NumericUpDownPort)
        Me.tabDB.Controls.Add(Me.LabelIPAddress)
        Me.tabDB.Controls.Add(Me.TextBoxIPAddress)
        Me.tabDB.Controls.Add(Me.LabelUser)
        Me.tabDB.Controls.Add(Me.TextBoxUser)
        Me.tabDB.Controls.Add(Me.LabelPort)
        Me.tabDB.Controls.Add(Me.LabelPassword)
        Me.tabDB.Controls.Add(Me.TextBoxPassword)
        Me.tabDB.Location = New System.Drawing.Point(4, 25)
        Me.tabDB.Name = "tabDB"
        Me.tabDB.Size = New System.Drawing.Size(312, 258)
        Me.tabDB.Text = "Database"
        '
        'LabelInvalidDBName
        '
        Me.LabelInvalidDBName.BackColor = System.Drawing.Color.Transparent
        Me.LabelInvalidDBName.ForeColor = System.Drawing.Color.Red
        Me.LabelInvalidDBName.Location = New System.Drawing.Point(16, 232)
        Me.LabelInvalidDBName.Name = "LabelInvalidDBName"
        Me.LabelInvalidDBName.Size = New System.Drawing.Size(276, 13)
        Me.LabelInvalidDBName.Text = "DB Name Harus Antara 4 - 20 Karakter"
        Me.LabelInvalidDBName.Visible = False
        '
        'LabelInvalidUser
        '
        Me.LabelInvalidUser.BackColor = System.Drawing.Color.Transparent
        Me.LabelInvalidUser.ForeColor = System.Drawing.Color.Red
        Me.LabelInvalidUser.Location = New System.Drawing.Point(16, 83)
        Me.LabelInvalidUser.Name = "LabelInvalidUser"
        Me.LabelInvalidUser.Size = New System.Drawing.Size(276, 13)
        Me.LabelInvalidUser.Text = "User Harus Antara 4 - 20 Karakter"
        Me.LabelInvalidUser.Visible = False
        '
        'LabelInvalidIPAddress
        '
        Me.LabelInvalidIPAddress.BackColor = System.Drawing.Color.Transparent
        Me.LabelInvalidIPAddress.ForeColor = System.Drawing.Color.Red
        Me.LabelInvalidIPAddress.Location = New System.Drawing.Point(16, 37)
        Me.LabelInvalidIPAddress.Name = "LabelInvalidIPAddress"
        Me.LabelInvalidIPAddress.Size = New System.Drawing.Size(276, 17)
        Me.LabelInvalidIPAddress.Text = "IP Address Tidak Benar"
        Me.LabelInvalidIPAddress.Visible = False
        '
        'LabelDBName
        '
        Me.LabelDBName.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelDBName.Location = New System.Drawing.Point(16, 203)
        Me.LabelDBName.Name = "LabelDBName"
        Me.LabelDBName.Size = New System.Drawing.Size(109, 23)
        Me.LabelDBName.Text = "DB Name"
        '
        'TextBoxDBName
        '
        Me.TextBoxDBName.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.TextBoxDBName.Location = New System.Drawing.Point(131, 203)
        Me.TextBoxDBName.Name = "TextBoxDBName"
        Me.TextBoxDBName.Size = New System.Drawing.Size(161, 26)
        Me.TextBoxDBName.TabIndex = 27
        Me.TextBoxDBName.TabStop = False
        '
        'NumericUpDownPort
        '
        Me.NumericUpDownPort.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.NumericUpDownPort.Location = New System.Drawing.Point(131, 153)
        Me.NumericUpDownPort.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
        Me.NumericUpDownPort.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDownPort.Name = "NumericUpDownPort"
        Me.NumericUpDownPort.Size = New System.Drawing.Size(161, 27)
        Me.NumericUpDownPort.TabIndex = 26
        Me.NumericUpDownPort.TabStop = False
        Me.NumericUpDownPort.Value = New Decimal(New Integer() {3306, 0, 0, 0})
        '
        'LabelIPAddress
        '
        Me.LabelIPAddress.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelIPAddress.Location = New System.Drawing.Point(16, 8)
        Me.LabelIPAddress.Name = "LabelIPAddress"
        Me.LabelIPAddress.Size = New System.Drawing.Size(109, 23)
        Me.LabelIPAddress.Text = "IP Address"
        '
        'TextBoxIPAddress
        '
        Me.TextBoxIPAddress.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.TextBoxIPAddress.Location = New System.Drawing.Point(131, 8)
        Me.TextBoxIPAddress.Name = "TextBoxIPAddress"
        Me.TextBoxIPAddress.Size = New System.Drawing.Size(161, 26)
        Me.TextBoxIPAddress.TabIndex = 20
        Me.TextBoxIPAddress.TabStop = False
        '
        'LabelUser
        '
        Me.LabelUser.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelUser.Location = New System.Drawing.Point(16, 57)
        Me.LabelUser.Name = "LabelUser"
        Me.LabelUser.Size = New System.Drawing.Size(109, 23)
        Me.LabelUser.Text = "User"
        '
        'TextBoxUser
        '
        Me.TextBoxUser.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.TextBoxUser.Location = New System.Drawing.Point(131, 57)
        Me.TextBoxUser.Name = "TextBoxUser"
        Me.TextBoxUser.Size = New System.Drawing.Size(161, 26)
        Me.TextBoxUser.TabIndex = 22
        Me.TextBoxUser.TabStop = False
        '
        'LabelPort
        '
        Me.LabelPort.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelPort.Location = New System.Drawing.Point(16, 153)
        Me.LabelPort.Name = "LabelPort"
        Me.LabelPort.Size = New System.Drawing.Size(109, 27)
        Me.LabelPort.Text = "Port"
        '
        'LabelPassword
        '
        Me.LabelPassword.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelPassword.Location = New System.Drawing.Point(16, 105)
        Me.LabelPassword.Name = "LabelPassword"
        Me.LabelPassword.Size = New System.Drawing.Size(109, 23)
        Me.LabelPassword.Text = "Password"
        '
        'TextBoxPassword
        '
        Me.TextBoxPassword.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.TextBoxPassword.Location = New System.Drawing.Point(131, 105)
        Me.TextBoxPassword.Name = "TextBoxPassword"
        Me.TextBoxPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBoxPassword.Size = New System.Drawing.Size(161, 26)
        Me.TextBoxPassword.TabIndex = 24
        Me.TextBoxPassword.TabStop = False
        '
        'tabPrinter
        '
        Me.tabPrinter.BackColor = System.Drawing.Color.Gainsboro
        Me.tabPrinter.Controls.Add(Me.LabelInvalidLineCode)
        Me.tabPrinter.Controls.Add(Me.LabelLineCode)
        Me.tabPrinter.Controls.Add(Me.TextBoxLineCode)
        Me.tabPrinter.Location = New System.Drawing.Point(4, 25)
        Me.tabPrinter.Name = "tabPrinter"
        Me.tabPrinter.Size = New System.Drawing.Size(312, 258)
        Me.tabPrinter.Text = "Printer"
        '
        'LabelInvalidLineCode
        '
        Me.LabelInvalidLineCode.BackColor = System.Drawing.Color.Transparent
        Me.LabelInvalidLineCode.ForeColor = System.Drawing.Color.Red
        Me.LabelInvalidLineCode.Location = New System.Drawing.Point(16, 37)
        Me.LabelInvalidLineCode.Name = "LabelInvalidLineCode"
        Me.LabelInvalidLineCode.Size = New System.Drawing.Size(276, 17)
        Me.LabelInvalidLineCode.Text = "Line Code Harus Antara 2 - 20 Karakter"
        Me.LabelInvalidLineCode.Visible = False
        '
        'LabelLineCode
        '
        Me.LabelLineCode.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelLineCode.Location = New System.Drawing.Point(16, 8)
        Me.LabelLineCode.Name = "LabelLineCode"
        Me.LabelLineCode.Size = New System.Drawing.Size(109, 23)
        Me.LabelLineCode.Text = "Line Code"
        '
        'TextBoxLineCode
        '
        Me.TextBoxLineCode.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.TextBoxLineCode.Location = New System.Drawing.Point(131, 8)
        Me.TextBoxLineCode.Name = "TextBoxLineCode"
        Me.TextBoxLineCode.Size = New System.Drawing.Size(161, 26)
        Me.TextBoxLineCode.TabIndex = 23
        Me.TextBoxLineCode.TabStop = False
        '
        'tabID
        '
        Me.tabID.Controls.Add(Me.Label1)
        Me.tabID.Controls.Add(Me.Label2)
        Me.tabID.Controls.Add(Me.txtID)
        Me.tabID.Location = New System.Drawing.Point(4, 25)
        Me.tabID.Name = "tabID"
        Me.tabID.Size = New System.Drawing.Size(312, 258)
        Me.tabID.Text = "ID"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(17, 44)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(276, 17)
        Me.Label1.Text = "ID harus 1 digit"
        Me.Label1.Visible = False
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.Label2.Location = New System.Drawing.Point(17, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(109, 23)
        Me.Label2.Text = "ID"
        '
        'txtID
        '
        Me.txtID.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.txtID.Location = New System.Drawing.Point(132, 15)
        Me.txtID.MaxLength = 1
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(161, 26)
        Me.txtID.TabIndex = 26
        Me.txtID.TabStop = False
        '
        'AppConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.Khaki
        Me.ClientSize = New System.Drawing.Size(320, 480)
        Me.ControlBox = False
        Me.Controls.Add(Me.tabControl1)
        Me.Controls.Add(Me.ButtonReset)
        Me.Controls.Add(Me.ButtonTestCon)
        Me.Controls.Add(Me.ButtonBack)
        Me.Controls.Add(Me.ButtonSave)
        Me.Controls.Add(Me.PanelTitle)
        Me.Controls.Add(Me.LabelTitle)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AppConfig"
        Me.Text = ":"
        Me.PanelTitle.ResumeLayout(False)
        Me.tabControl1.ResumeLayout(False)
        Me.tabDB.ResumeLayout(False)
        Me.tabPrinter.ResumeLayout(False)
        Me.tabID.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LabelTitle As System.Windows.Forms.Label
    Friend WithEvents PanelTitle As System.Windows.Forms.Panel
    Friend WithEvents ButtonFullScreen As System.Windows.Forms.Button
    Friend WithEvents LabelWindowTitle As System.Windows.Forms.Label
    Friend WithEvents ButtonSave As System.Windows.Forms.Button
    Friend WithEvents ButtonBack As System.Windows.Forms.Button
    Friend WithEvents ButtonTestCon As System.Windows.Forms.Button
    Friend WithEvents ButtonReset As System.Windows.Forms.Button
    Friend WithEvents tabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabDB As System.Windows.Forms.TabPage
    Friend WithEvents tabPrinter As System.Windows.Forms.TabPage
    Friend WithEvents LabelInvalidDBName As System.Windows.Forms.Label
    Friend WithEvents LabelInvalidUser As System.Windows.Forms.Label
    Friend WithEvents LabelInvalidIPAddress As System.Windows.Forms.Label
    Friend WithEvents LabelDBName As System.Windows.Forms.Label
    Friend WithEvents TextBoxDBName As System.Windows.Forms.TextBox
    Friend WithEvents NumericUpDownPort As System.Windows.Forms.NumericUpDown
    Friend WithEvents LabelIPAddress As System.Windows.Forms.Label
    Friend WithEvents TextBoxIPAddress As System.Windows.Forms.TextBox
    Friend WithEvents LabelUser As System.Windows.Forms.Label
    Friend WithEvents TextBoxUser As System.Windows.Forms.TextBox
    Friend WithEvents LabelPort As System.Windows.Forms.Label
    Friend WithEvents LabelPassword As System.Windows.Forms.Label
    Friend WithEvents TextBoxPassword As System.Windows.Forms.TextBox
    Friend WithEvents LabelInvalidLineCode As System.Windows.Forms.Label
    Friend WithEvents LabelLineCode As System.Windows.Forms.Label
    Friend WithEvents TextBoxLineCode As System.Windows.Forms.TextBox
    Friend WithEvents tabID As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtID As System.Windows.Forms.TextBox
End Class

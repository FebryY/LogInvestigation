<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ShipmentScan
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
        Me.LabelTitle = New System.Windows.Forms.Label
        Me.labelTrinPartCodeTitle = New System.Windows.Forms.Label
        Me.textBoxScannedTag = New System.Windows.Forms.TextBox
        Me.labelScannedTagTitle = New System.Windows.Forms.Label
        Me.labelTrinPartCodeValue = New System.Windows.Forms.Label
        Me.labelPlanQtyTitle = New System.Windows.Forms.Label
        Me.labelPlanQtyValue = New System.Windows.Forms.Label
        Me.labelActualQtyTitle = New System.Windows.Forms.Label
        Me.labelActualQtyValue = New System.Windows.Forms.Label
        Me.labelRemainingQtyTitle = New System.Windows.Forms.Label
        Me.labelRemainingQtyValue = New System.Windows.Forms.Label
        Me.PanelTitle = New System.Windows.Forms.Panel
        Me.ButtonFullScreen = New System.Windows.Forms.Button
        Me.LabelWindowTitle = New System.Windows.Forms.Label
        Me.buttonBack = New System.Windows.Forms.Button
        Me.buttonSave = New System.Windows.Forms.Button
        Me.buttonReset = New System.Windows.Forms.Button
        Me.timerScanner = New System.Windows.Forms.Timer
        Me.PanelTitle.SuspendLayout()
        Me.SuspendLayout()
        '
        'LabelTitle
        '
        Me.LabelTitle.Font = New System.Drawing.Font("Tahoma", 16.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.LabelTitle.Location = New System.Drawing.Point(0, 61)
        Me.LabelTitle.Name = "LabelTitle"
        Me.LabelTitle.Size = New System.Drawing.Size(318, 30)
        Me.LabelTitle.Text = "Shipment"
        Me.LabelTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'labelTrinPartCodeTitle
        '
        Me.labelTrinPartCodeTitle.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.labelTrinPartCodeTitle.Location = New System.Drawing.Point(21, 165)
        Me.labelTrinPartCodeTitle.Name = "labelTrinPartCodeTitle"
        Me.labelTrinPartCodeTitle.Size = New System.Drawing.Size(100, 48)
        Me.labelTrinPartCodeTitle.Text = "TRIN Part Code :"
        '
        'textBoxScannedTag
        '
        Me.textBoxScannedTag.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.textBoxScannedTag.Location = New System.Drawing.Point(121, 126)
        Me.textBoxScannedTag.Name = "textBoxScannedTag"
        Me.textBoxScannedTag.Size = New System.Drawing.Size(176, 26)
        Me.textBoxScannedTag.TabIndex = 1
        '
        'labelScannedTagTitle
        '
        Me.labelScannedTagTitle.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.labelScannedTagTitle.Location = New System.Drawing.Point(21, 126)
        Me.labelScannedTagTitle.Name = "labelScannedTagTitle"
        Me.labelScannedTagTitle.Size = New System.Drawing.Size(94, 26)
        Me.labelScannedTagTitle.Text = "Scan Tag"
        '
        'labelTrinPartCodeValue
        '
        Me.labelTrinPartCodeValue.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.labelTrinPartCodeValue.Location = New System.Drawing.Point(121, 165)
        Me.labelTrinPartCodeValue.Name = "labelTrinPartCodeValue"
        Me.labelTrinPartCodeValue.Size = New System.Drawing.Size(176, 45)
        Me.labelTrinPartCodeValue.Text = "-"
        '
        'labelPlanQtyTitle
        '
        Me.labelPlanQtyTitle.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.labelPlanQtyTitle.Location = New System.Drawing.Point(21, 213)
        Me.labelPlanQtyTitle.Name = "labelPlanQtyTitle"
        Me.labelPlanQtyTitle.Size = New System.Drawing.Size(94, 26)
        Me.labelPlanQtyTitle.Text = "Plan Qty :"
        '
        'labelPlanQtyValue
        '
        Me.labelPlanQtyValue.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.labelPlanQtyValue.Location = New System.Drawing.Point(121, 213)
        Me.labelPlanQtyValue.Name = "labelPlanQtyValue"
        Me.labelPlanQtyValue.Size = New System.Drawing.Size(176, 26)
        Me.labelPlanQtyValue.Text = "0"
        '
        'labelActualQtyTitle
        '
        Me.labelActualQtyTitle.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.labelActualQtyTitle.Location = New System.Drawing.Point(21, 248)
        Me.labelActualQtyTitle.Name = "labelActualQtyTitle"
        Me.labelActualQtyTitle.Size = New System.Drawing.Size(94, 26)
        Me.labelActualQtyTitle.Text = "Actual Qty :"
        '
        'labelActualQtyValue
        '
        Me.labelActualQtyValue.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.labelActualQtyValue.Location = New System.Drawing.Point(121, 248)
        Me.labelActualQtyValue.Name = "labelActualQtyValue"
        Me.labelActualQtyValue.Size = New System.Drawing.Size(176, 26)
        Me.labelActualQtyValue.Text = "0"
        '
        'labelRemainingQtyTitle
        '
        Me.labelRemainingQtyTitle.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.labelRemainingQtyTitle.Location = New System.Drawing.Point(21, 283)
        Me.labelRemainingQtyTitle.Name = "labelRemainingQtyTitle"
        Me.labelRemainingQtyTitle.Size = New System.Drawing.Size(94, 48)
        Me.labelRemainingQtyTitle.Text = "Remaining Qty :"
        '
        'labelRemainingQtyValue
        '
        Me.labelRemainingQtyValue.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.labelRemainingQtyValue.Location = New System.Drawing.Point(121, 283)
        Me.labelRemainingQtyValue.Name = "labelRemainingQtyValue"
        Me.labelRemainingQtyValue.Size = New System.Drawing.Size(176, 48)
        Me.labelRemainingQtyValue.Text = "0"
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
        'buttonBack
        '
        Me.buttonBack.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.buttonBack.Location = New System.Drawing.Point(177, 387)
        Me.buttonBack.Name = "buttonBack"
        Me.buttonBack.Size = New System.Drawing.Size(120, 20)
        Me.buttonBack.TabIndex = 20
        Me.buttonBack.TabStop = False
        Me.buttonBack.Text = "F2 Back"
        '
        'buttonSave
        '
        Me.buttonSave.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.buttonSave.Location = New System.Drawing.Point(21, 387)
        Me.buttonSave.Name = "buttonSave"
        Me.buttonSave.Size = New System.Drawing.Size(120, 20)
        Me.buttonSave.TabIndex = 19
        Me.buttonSave.TabStop = False
        Me.buttonSave.Text = "F1 Save"
        '
        'buttonReset
        '
        Me.buttonReset.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.buttonReset.Location = New System.Drawing.Point(21, 413)
        Me.buttonReset.Name = "buttonReset"
        Me.buttonReset.Size = New System.Drawing.Size(120, 20)
        Me.buttonReset.TabIndex = 21
        Me.buttonReset.TabStop = False
        Me.buttonReset.Text = "F3 Reset"
        '
        'timerScanner
        '
        Me.timerScanner.Interval = 1000
        '
        'ShipmentScan
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.Khaki
        Me.ClientSize = New System.Drawing.Size(320, 480)
        Me.ControlBox = False
        Me.Controls.Add(Me.buttonReset)
        Me.Controls.Add(Me.buttonBack)
        Me.Controls.Add(Me.buttonSave)
        Me.Controls.Add(Me.PanelTitle)
        Me.Controls.Add(Me.labelRemainingQtyValue)
        Me.Controls.Add(Me.labelRemainingQtyTitle)
        Me.Controls.Add(Me.labelActualQtyValue)
        Me.Controls.Add(Me.labelActualQtyTitle)
        Me.Controls.Add(Me.labelPlanQtyValue)
        Me.Controls.Add(Me.labelPlanQtyTitle)
        Me.Controls.Add(Me.labelTrinPartCodeValue)
        Me.Controls.Add(Me.labelScannedTagTitle)
        Me.Controls.Add(Me.labelTrinPartCodeTitle)
        Me.Controls.Add(Me.textBoxScannedTag)
        Me.Controls.Add(Me.LabelTitle)
        Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ShipmentScan"
        Me.Text = "Shipment Scan"
        Me.PanelTitle.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LabelTitle As System.Windows.Forms.Label
    Friend WithEvents labelTrinPartCodeTitle As System.Windows.Forms.Label
    Friend WithEvents textBoxScannedTag As System.Windows.Forms.TextBox
    Friend WithEvents labelScannedTagTitle As System.Windows.Forms.Label
    Friend WithEvents labelTrinPartCodeValue As System.Windows.Forms.Label
    Friend WithEvents labelPlanQtyTitle As System.Windows.Forms.Label
    Friend WithEvents labelPlanQtyValue As System.Windows.Forms.Label
    Friend WithEvents labelActualQtyTitle As System.Windows.Forms.Label
    Friend WithEvents labelActualQtyValue As System.Windows.Forms.Label
    Friend WithEvents labelRemainingQtyTitle As System.Windows.Forms.Label
    Friend WithEvents labelRemainingQtyValue As System.Windows.Forms.Label
    Friend WithEvents PanelTitle As System.Windows.Forms.Panel
    Friend WithEvents ButtonFullScreen As System.Windows.Forms.Button
    Friend WithEvents LabelWindowTitle As System.Windows.Forms.Label
    Friend WithEvents buttonBack As System.Windows.Forms.Button
    Friend WithEvents buttonSave As System.Windows.Forms.Button
    Friend WithEvents buttonReset As System.Windows.Forms.Button
    Friend WithEvents timerScanner As System.Windows.Forms.Timer

End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ShipmentList
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
        Me.DataGrid1 = New System.Windows.Forms.DataGrid
        Me.RadioButtonAll = New System.Windows.Forms.RadioButton
        Me.RadioButtonNewPartial = New System.Windows.Forms.RadioButton
        Me.RadioButtonCompleted = New System.Windows.Forms.RadioButton
        Me.PanelTitle = New System.Windows.Forms.Panel
        Me.ButtonFullScreen = New System.Windows.Forms.Button
        Me.LabelWindowTitle = New System.Windows.Forms.Label
        Me.ButtonBack = New System.Windows.Forms.Button
        Me.ButtonNext = New System.Windows.Forms.Button
        Me.DataGridTableStyle1 = New System.Windows.Forms.DataGridTableStyle
        Me.ButtonSelectGrid = New System.Windows.Forms.Button
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
        'DataGrid1
        '
        Me.DataGrid1.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.DataGrid1.Font = New System.Drawing.Font("Arial", 10.0!, System.Drawing.FontStyle.Regular)
        Me.DataGrid1.Location = New System.Drawing.Point(0, 126)
        Me.DataGrid1.Name = "DataGrid1"
        Me.DataGrid1.RowHeadersVisible = False
        Me.DataGrid1.Size = New System.Drawing.Size(320, 248)
        Me.DataGrid1.TabIndex = 1
        Me.DataGrid1.TabStop = False
        '
        'RadioButtonAll
        '
        Me.RadioButtonAll.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.RadioButtonAll.Location = New System.Drawing.Point(21, 100)
        Me.RadioButtonAll.Name = "RadioButtonAll"
        Me.RadioButtonAll.Size = New System.Drawing.Size(40, 20)
        Me.RadioButtonAll.TabIndex = 5
        Me.RadioButtonAll.TabStop = False
        Me.RadioButtonAll.Text = "All"
        '
        'RadioButtonNewPartial
        '
        Me.RadioButtonNewPartial.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.RadioButtonNewPartial.Location = New System.Drawing.Point(67, 100)
        Me.RadioButtonNewPartial.Name = "RadioButtonNewPartial"
        Me.RadioButtonNewPartial.Size = New System.Drawing.Size(125, 20)
        Me.RadioButtonNewPartial.TabIndex = 6
        Me.RadioButtonNewPartial.TabStop = False
        Me.RadioButtonNewPartial.Text = "New && Partial"
        '
        'RadioButtonCompleted
        '
        Me.RadioButtonCompleted.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.RadioButtonCompleted.Location = New System.Drawing.Point(198, 100)
        Me.RadioButtonCompleted.Name = "RadioButtonCompleted"
        Me.RadioButtonCompleted.Size = New System.Drawing.Size(101, 20)
        Me.RadioButtonCompleted.TabIndex = 8
        Me.RadioButtonCompleted.TabStop = False
        Me.RadioButtonCompleted.Text = "Completed"
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
        'ButtonBack
        '
        Me.ButtonBack.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonBack.Location = New System.Drawing.Point(177, 387)
        Me.ButtonBack.Name = "ButtonBack"
        Me.ButtonBack.Size = New System.Drawing.Size(120, 20)
        Me.ButtonBack.TabIndex = 18
        Me.ButtonBack.TabStop = False
        Me.ButtonBack.Text = "F2 Back"
        '
        'ButtonNext
        '
        Me.ButtonNext.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonNext.Location = New System.Drawing.Point(21, 387)
        Me.ButtonNext.Name = "ButtonNext"
        Me.ButtonNext.Size = New System.Drawing.Size(120, 20)
        Me.ButtonNext.TabIndex = 17
        Me.ButtonNext.TabStop = False
        Me.ButtonNext.Text = "F1 Next"
        '
        'ButtonSelectGrid
        '
        Me.ButtonSelectGrid.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonSelectGrid.Location = New System.Drawing.Point(247, 31)
        Me.ButtonSelectGrid.Name = "ButtonSelectGrid"
        Me.ButtonSelectGrid.Size = New System.Drawing.Size(70, 20)
        Me.ButtonSelectGrid.TabIndex = 19
        Me.ButtonSelectGrid.TabStop = False
        Me.ButtonSelectGrid.Text = "Unselect"
        '
        'ShipmentList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.Khaki
        Me.ClientSize = New System.Drawing.Size(320, 480)
        Me.ControlBox = False
        Me.Controls.Add(Me.ButtonSelectGrid)
        Me.Controls.Add(Me.ButtonBack)
        Me.Controls.Add(Me.ButtonNext)
        Me.Controls.Add(Me.PanelTitle)
        Me.Controls.Add(Me.RadioButtonCompleted)
        Me.Controls.Add(Me.RadioButtonNewPartial)
        Me.Controls.Add(Me.RadioButtonAll)
        Me.Controls.Add(Me.DataGrid1)
        Me.Controls.Add(Me.LabelTitle)
        Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ShipmentList"
        Me.Text = "Shipment List"
        Me.PanelTitle.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LabelTitle As System.Windows.Forms.Label
    Friend WithEvents DataGrid1 As System.Windows.Forms.DataGrid
    Friend WithEvents RadioButtonAll As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonNewPartial As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonCompleted As System.Windows.Forms.RadioButton
    Friend WithEvents PanelTitle As System.Windows.Forms.Panel
    Friend WithEvents ButtonFullScreen As System.Windows.Forms.Button
    Friend WithEvents LabelWindowTitle As System.Windows.Forms.Label
    Friend WithEvents ButtonBack As System.Windows.Forms.Button
    Friend WithEvents ButtonNext As System.Windows.Forms.Button
    Friend WithEvents DataGridTableStyle1 As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents ButtonSelectGrid As System.Windows.Forms.Button

End Class

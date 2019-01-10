<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ShipmentFilter
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
        Me.LabelDateTime = New System.Windows.Forms.Label
        Me.CheckBoxPreviousDate = New System.Windows.Forms.CheckBox
        Me.PanelTitle = New System.Windows.Forms.Panel
        Me.ButtonFullScreen = New System.Windows.Forms.Button
        Me.LabelWindowTitle = New System.Windows.Forms.Label
        Me.ButtonNext = New System.Windows.Forms.Button
        Me.ButtonBack = New System.Windows.Forms.Button
        Me.ComboBoxCustomer = New System.Windows.Forms.ComboBox
        Me.LabelCustomer = New System.Windows.Forms.Label
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker
        Me.CbxCompletedData = New System.Windows.Forms.CheckBox
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
        'LabelDateTime
        '
        Me.LabelDateTime.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelDateTime.Location = New System.Drawing.Point(21, 126)
        Me.LabelDateTime.Name = "LabelDateTime"
        Me.LabelDateTime.Size = New System.Drawing.Size(276, 20)
        Me.LabelDateTime.Text = "Date && Time :"
        '
        'CheckBoxPreviousDate
        '
        Me.CheckBoxPreviousDate.Checked = True
        Me.CheckBoxPreviousDate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxPreviousDate.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.CheckBoxPreviousDate.Location = New System.Drawing.Point(21, 181)
        Me.CheckBoxPreviousDate.Name = "CheckBoxPreviousDate"
        Me.CheckBoxPreviousDate.Size = New System.Drawing.Size(132, 20)
        Me.CheckBoxPreviousDate.TabIndex = 1
        Me.CheckBoxPreviousDate.TabStop = False
        Me.CheckBoxPreviousDate.Text = "Previous Date"
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
        'ButtonNext
        '
        Me.ButtonNext.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonNext.Location = New System.Drawing.Point(21, 387)
        Me.ButtonNext.Name = "ButtonNext"
        Me.ButtonNext.Size = New System.Drawing.Size(120, 20)
        Me.ButtonNext.TabIndex = 15
        Me.ButtonNext.TabStop = False
        Me.ButtonNext.Text = "F1 Next"
        '
        'ButtonBack
        '
        Me.ButtonBack.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonBack.Location = New System.Drawing.Point(177, 387)
        Me.ButtonBack.Name = "ButtonBack"
        Me.ButtonBack.Size = New System.Drawing.Size(120, 20)
        Me.ButtonBack.TabIndex = 16
        Me.ButtonBack.TabStop = False
        Me.ButtonBack.Text = "F2 Back"
        '
        'ComboBoxCustomer
        '
        Me.ComboBoxCustomer.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.ComboBoxCustomer.Location = New System.Drawing.Point(21, 278)
        Me.ComboBoxCustomer.Name = "ComboBoxCustomer"
        Me.ComboBoxCustomer.Size = New System.Drawing.Size(276, 26)
        Me.ComboBoxCustomer.TabIndex = 2
        Me.ComboBoxCustomer.TabStop = False
        '
        'LabelCustomer
        '
        Me.LabelCustomer.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelCustomer.Location = New System.Drawing.Point(21, 255)
        Me.LabelCustomer.Name = "LabelCustomer"
        Me.LabelCustomer.Size = New System.Drawing.Size(276, 20)
        Me.LabelCustomer.Text = "Customer :"
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.DateTimePicker1.Location = New System.Drawing.Point(21, 149)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(276, 27)
        Me.DateTimePicker1.TabIndex = 2
        Me.DateTimePicker1.TabStop = False
        '
        'CbxCompletedData
        '
        Me.CbxCompletedData.Checked = True
        Me.CbxCompletedData.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CbxCompletedData.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.CbxCompletedData.Location = New System.Drawing.Point(21, 207)
        Me.CbxCompletedData.Name = "CbxCompletedData"
        Me.CbxCompletedData.Size = New System.Drawing.Size(155, 20)
        Me.CbxCompletedData.TabIndex = 1
        Me.CbxCompletedData.TabStop = False
        Me.CbxCompletedData.Text = "Include Completed"
        '
        'ShipmentFilter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.Khaki
        Me.ClientSize = New System.Drawing.Size(320, 480)
        Me.ControlBox = False
        Me.Controls.Add(Me.CbxCompletedData)
        Me.Controls.Add(Me.LabelCustomer)
        Me.Controls.Add(Me.ComboBoxCustomer)
        Me.Controls.Add(Me.ButtonBack)
        Me.Controls.Add(Me.ButtonNext)
        Me.Controls.Add(Me.PanelTitle)
        Me.Controls.Add(Me.CheckBoxPreviousDate)
        Me.Controls.Add(Me.LabelDateTime)
        Me.Controls.Add(Me.LabelTitle)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ShipmentFilter"
        Me.Text = "Shipment Filter"
        Me.PanelTitle.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LabelTitle As System.Windows.Forms.Label
    Friend WithEvents LabelDateTime As System.Windows.Forms.Label
    Friend WithEvents CheckBoxPreviousDate As System.Windows.Forms.CheckBox
    Friend WithEvents PanelTitle As System.Windows.Forms.Panel
    Friend WithEvents ButtonFullScreen As System.Windows.Forms.Button
    Friend WithEvents LabelWindowTitle As System.Windows.Forms.Label
    Friend WithEvents ButtonNext As System.Windows.Forms.Button
    Friend WithEvents ButtonBack As System.Windows.Forms.Button
    Friend WithEvents ComboBoxCustomer As System.Windows.Forms.ComboBox
    Friend WithEvents LabelCustomer As System.Windows.Forms.Label
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents CbxCompletedData As System.Windows.Forms.CheckBox

End Class

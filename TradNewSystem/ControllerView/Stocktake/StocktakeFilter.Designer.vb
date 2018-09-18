<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class StocktakeFilter
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.LabelCustomer = New System.Windows.Forms.Label
        Me.ComboBoxDivision = New System.Windows.Forms.ComboBox
        Me.ButtonBack = New System.Windows.Forms.Button
        Me.ButtonNext = New System.Windows.Forms.Button
        Me.PanelTitle = New System.Windows.Forms.Panel
        Me.ButtonFullScreen = New System.Windows.Forms.Button
        Me.LabelWindowTitle = New System.Windows.Forms.Label
        Me.LabelStocktakePeriod = New System.Windows.Forms.Label
        Me.LabelTitle = New System.Windows.Forms.Label
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker
        Me.PanelTitle.SuspendLayout()
        Me.SuspendLayout()
        '
        'LabelCustomer
        '
        Me.LabelCustomer.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelCustomer.Location = New System.Drawing.Point(21, 234)
        Me.LabelCustomer.Name = "LabelCustomer"
        Me.LabelCustomer.Size = New System.Drawing.Size(276, 20)
        Me.LabelCustomer.Text = "Division :"
        '
        'ComboBoxDivision
        '
        Me.ComboBoxDivision.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.ComboBoxDivision.Location = New System.Drawing.Point(21, 257)
        Me.ComboBoxDivision.Name = "ComboBoxDivision"
        Me.ComboBoxDivision.Size = New System.Drawing.Size(276, 26)
        Me.ComboBoxDivision.TabIndex = 23
        Me.ComboBoxDivision.TabStop = False
        '
        'ButtonBack
        '
        Me.ButtonBack.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonBack.Location = New System.Drawing.Point(177, 387)
        Me.ButtonBack.Name = "ButtonBack"
        Me.ButtonBack.Size = New System.Drawing.Size(120, 20)
        Me.ButtonBack.TabIndex = 25
        Me.ButtonBack.TabStop = False
        Me.ButtonBack.Text = "F2 Back"
        '
        'ButtonNext
        '
        Me.ButtonNext.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonNext.Location = New System.Drawing.Point(21, 387)
        Me.ButtonNext.Name = "ButtonNext"
        Me.ButtonNext.Size = New System.Drawing.Size(120, 20)
        Me.ButtonNext.TabIndex = 24
        Me.ButtonNext.TabStop = False
        Me.ButtonNext.Text = "F1 Next"
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
        'LabelStocktakePeriod
        '
        Me.LabelStocktakePeriod.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelStocktakePeriod.Location = New System.Drawing.Point(21, 126)
        Me.LabelStocktakePeriod.Name = "LabelStocktakePeriod"
        Me.LabelStocktakePeriod.Size = New System.Drawing.Size(276, 20)
        Me.LabelStocktakePeriod.Text = "Stocktake Period :"
        '
        'LabelTitle
        '
        Me.LabelTitle.Font = New System.Drawing.Font("Tahoma", 16.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.LabelTitle.Location = New System.Drawing.Point(0, 61)
        Me.LabelTitle.Name = "LabelTitle"
        Me.LabelTitle.Size = New System.Drawing.Size(318, 30)
        Me.LabelTitle.Text = "Stocktake"
        Me.LabelTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.CustomFormat = "MM/yyyy"
        Me.DateTimePicker1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker1.Location = New System.Drawing.Point(21, 149)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(276, 27)
        Me.DateTimePicker1.TabIndex = 22
        Me.DateTimePicker1.TabStop = False
        '
        'StocktakeFilter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.Khaki
        Me.ClientSize = New System.Drawing.Size(320, 480)
        Me.ControlBox = False
        Me.Controls.Add(Me.LabelCustomer)
        Me.Controls.Add(Me.ComboBoxDivision)
        Me.Controls.Add(Me.ButtonBack)
        Me.Controls.Add(Me.ButtonNext)
        Me.Controls.Add(Me.PanelTitle)
        Me.Controls.Add(Me.LabelStocktakePeriod)
        Me.Controls.Add(Me.LabelTitle)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "StocktakeFilter"
        Me.Text = "Stocktake Filter"
        Me.PanelTitle.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LabelCustomer As System.Windows.Forms.Label
    Friend WithEvents ComboBoxDivision As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonBack As System.Windows.Forms.Button
    Friend WithEvents ButtonNext As System.Windows.Forms.Button
    Friend WithEvents PanelTitle As System.Windows.Forms.Panel
    Friend WithEvents ButtonFullScreen As System.Windows.Forms.Button
    Friend WithEvents LabelWindowTitle As System.Windows.Forms.Label
    Friend WithEvents LabelStocktakePeriod As System.Windows.Forms.Label
    Friend WithEvents LabelTitle As System.Windows.Forms.Label
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
End Class

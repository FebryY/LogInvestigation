<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class OfflineStocktake
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
        Me.PanelTitle = New System.Windows.Forms.Panel
        Me.ButtonFullScreen = New System.Windows.Forms.Button
        Me.LabelWindowTitle = New System.Windows.Forms.Label
        Me.LabelTitle = New System.Windows.Forms.Label
        Me.ButtonBack = New System.Windows.Forms.Button
        Me.LabelDivisionValue = New System.Windows.Forms.Label
        Me.LabelDivisionTitle = New System.Windows.Forms.Label
        Me.LabelTrinPartNoValue = New System.Windows.Forms.Label
        Me.LabelScanTag = New System.Windows.Forms.Label
        Me.LabelTrinPartNoTitle = New System.Windows.Forms.Label
        Me.TextBoxScanTag = New System.Windows.Forms.TextBox
        Me.LabelCountedQtyValue = New System.Windows.Forms.Label
        Me.LabelCountedQtyTitle = New System.Windows.Forms.Label
        Me.LabelTotalQtyValue = New System.Windows.Forms.Label
        Me.LabelTotalQtyTitle = New System.Windows.Forms.Label
        Me.LabelTagCountValue = New System.Windows.Forms.Label
        Me.LabelTagCountTitle = New System.Windows.Forms.Label
        Me.ButtonReset = New System.Windows.Forms.Button
        Me.ButtonSave = New System.Windows.Forms.Button
        Me.timerScanner = New System.Windows.Forms.Timer
        Me.PanelTitle.SuspendLayout()
        Me.SuspendLayout()
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
        'LabelTitle
        '
        Me.LabelTitle.Font = New System.Drawing.Font("Tahoma", 16.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.LabelTitle.Location = New System.Drawing.Point(0, 61)
        Me.LabelTitle.Name = "LabelTitle"
        Me.LabelTitle.Size = New System.Drawing.Size(318, 30)
        Me.LabelTitle.Text = "Offline Stocktake"
        Me.LabelTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ButtonBack
        '
        Me.ButtonBack.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonBack.Location = New System.Drawing.Point(177, 387)
        Me.ButtonBack.Name = "ButtonBack"
        Me.ButtonBack.Size = New System.Drawing.Size(120, 20)
        Me.ButtonBack.TabIndex = 2
        Me.ButtonBack.TabStop = False
        Me.ButtonBack.Text = "F2 Back"
        '
        'LabelDivisionValue
        '
        Me.LabelDivisionValue.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelDivisionValue.Location = New System.Drawing.Point(121, 165)
        Me.LabelDivisionValue.Name = "LabelDivisionValue"
        Me.LabelDivisionValue.Size = New System.Drawing.Size(176, 26)
        Me.LabelDivisionValue.Text = "-"
        '
        'LabelDivisionTitle
        '
        Me.LabelDivisionTitle.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelDivisionTitle.Location = New System.Drawing.Point(21, 165)
        Me.LabelDivisionTitle.Name = "LabelDivisionTitle"
        Me.LabelDivisionTitle.Size = New System.Drawing.Size(94, 26)
        Me.LabelDivisionTitle.Text = "Division"
        '
        'LabelTrinPartNoValue
        '
        Me.LabelTrinPartNoValue.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelTrinPartNoValue.Location = New System.Drawing.Point(121, 201)
        Me.LabelTrinPartNoValue.Name = "LabelTrinPartNoValue"
        Me.LabelTrinPartNoValue.Size = New System.Drawing.Size(176, 45)
        Me.LabelTrinPartNoValue.Text = "-"
        '
        'LabelScanTag
        '
        Me.LabelScanTag.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelScanTag.Location = New System.Drawing.Point(21, 126)
        Me.LabelScanTag.Name = "LabelScanTag"
        Me.LabelScanTag.Size = New System.Drawing.Size(94, 26)
        Me.LabelScanTag.Text = "Scan Tag"
        '
        'LabelTrinPartNoTitle
        '
        Me.LabelTrinPartNoTitle.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelTrinPartNoTitle.Location = New System.Drawing.Point(21, 201)
        Me.LabelTrinPartNoTitle.Name = "LabelTrinPartNoTitle"
        Me.LabelTrinPartNoTitle.Size = New System.Drawing.Size(100, 48)
        Me.LabelTrinPartNoTitle.Text = "TRIN Part No"
        '
        'TextBoxScanTag
        '
        Me.TextBoxScanTag.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.TextBoxScanTag.Location = New System.Drawing.Point(121, 126)
        Me.TextBoxScanTag.Name = "TextBoxScanTag"
        Me.TextBoxScanTag.Size = New System.Drawing.Size(176, 26)
        Me.TextBoxScanTag.TabIndex = 1
        '
        'LabelCountedQtyValue
        '
        Me.LabelCountedQtyValue.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelCountedQtyValue.Location = New System.Drawing.Point(121, 249)
        Me.LabelCountedQtyValue.Name = "LabelCountedQtyValue"
        Me.LabelCountedQtyValue.Size = New System.Drawing.Size(176, 48)
        Me.LabelCountedQtyValue.Text = "0"
        '
        'LabelCountedQtyTitle
        '
        Me.LabelCountedQtyTitle.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelCountedQtyTitle.Location = New System.Drawing.Point(21, 249)
        Me.LabelCountedQtyTitle.Name = "LabelCountedQtyTitle"
        Me.LabelCountedQtyTitle.Size = New System.Drawing.Size(94, 48)
        Me.LabelCountedQtyTitle.Text = "Counted Qty"
        '
        'LabelTotalQtyValue
        '
        Me.LabelTotalQtyValue.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelTotalQtyValue.Location = New System.Drawing.Point(121, 297)
        Me.LabelTotalQtyValue.Name = "LabelTotalQtyValue"
        Me.LabelTotalQtyValue.Size = New System.Drawing.Size(176, 26)
        Me.LabelTotalQtyValue.Text = "-"
        '
        'LabelTotalQtyTitle
        '
        Me.LabelTotalQtyTitle.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelTotalQtyTitle.Location = New System.Drawing.Point(21, 297)
        Me.LabelTotalQtyTitle.Name = "LabelTotalQtyTitle"
        Me.LabelTotalQtyTitle.Size = New System.Drawing.Size(94, 26)
        Me.LabelTotalQtyTitle.Text = "Total Qty"
        '
        'LabelTagCountValue
        '
        Me.LabelTagCountValue.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelTagCountValue.Location = New System.Drawing.Point(121, 332)
        Me.LabelTagCountValue.Name = "LabelTagCountValue"
        Me.LabelTagCountValue.Size = New System.Drawing.Size(176, 26)
        Me.LabelTagCountValue.Text = "0"
        '
        'LabelTagCountTitle
        '
        Me.LabelTagCountTitle.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.LabelTagCountTitle.Location = New System.Drawing.Point(21, 332)
        Me.LabelTagCountTitle.Name = "LabelTagCountTitle"
        Me.LabelTagCountTitle.Size = New System.Drawing.Size(94, 26)
        Me.LabelTagCountTitle.Text = "Tag Count"
        '
        'ButtonReset
        '
        Me.ButtonReset.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonReset.Location = New System.Drawing.Point(21, 413)
        Me.ButtonReset.Name = "ButtonReset"
        Me.ButtonReset.Size = New System.Drawing.Size(120, 20)
        Me.ButtonReset.TabIndex = 31
        Me.ButtonReset.TabStop = False
        Me.ButtonReset.Text = "F3 Reset"
        '
        'ButtonSave
        '
        Me.ButtonSave.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonSave.Location = New System.Drawing.Point(21, 387)
        Me.ButtonSave.Name = "ButtonSave"
        Me.ButtonSave.Size = New System.Drawing.Size(120, 20)
        Me.ButtonSave.TabIndex = 30
        Me.ButtonSave.TabStop = False
        Me.ButtonSave.Text = "F1 Save"
        '
        'timerScanner
        '
        Me.timerScanner.Interval = 1000
        '
        'OfflineStocktake
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.Khaki
        Me.ClientSize = New System.Drawing.Size(320, 480)
        Me.ControlBox = False
        Me.Controls.Add(Me.ButtonReset)
        Me.Controls.Add(Me.ButtonSave)
        Me.Controls.Add(Me.LabelTagCountValue)
        Me.Controls.Add(Me.LabelTagCountTitle)
        Me.Controls.Add(Me.LabelTotalQtyValue)
        Me.Controls.Add(Me.LabelTotalQtyTitle)
        Me.Controls.Add(Me.LabelCountedQtyValue)
        Me.Controls.Add(Me.LabelCountedQtyTitle)
        Me.Controls.Add(Me.LabelDivisionValue)
        Me.Controls.Add(Me.LabelDivisionTitle)
        Me.Controls.Add(Me.LabelTrinPartNoValue)
        Me.Controls.Add(Me.LabelScanTag)
        Me.Controls.Add(Me.LabelTrinPartNoTitle)
        Me.Controls.Add(Me.TextBoxScanTag)
        Me.Controls.Add(Me.ButtonBack)
        Me.Controls.Add(Me.PanelTitle)
        Me.Controls.Add(Me.LabelTitle)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "OfflineStocktake"
        Me.Text = "Offline Stocktake"
        Me.PanelTitle.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PanelTitle As System.Windows.Forms.Panel
    Friend WithEvents ButtonFullScreen As System.Windows.Forms.Button
    Friend WithEvents LabelWindowTitle As System.Windows.Forms.Label
    Friend WithEvents LabelTitle As System.Windows.Forms.Label
    Friend WithEvents ButtonBack As System.Windows.Forms.Button
    Friend WithEvents LabelDivisionValue As System.Windows.Forms.Label
    Friend WithEvents LabelDivisionTitle As System.Windows.Forms.Label
    Friend WithEvents LabelTrinPartNoValue As System.Windows.Forms.Label
    Friend WithEvents LabelScanTag As System.Windows.Forms.Label
    Friend WithEvents LabelTrinPartNoTitle As System.Windows.Forms.Label
    Friend WithEvents TextBoxScanTag As System.Windows.Forms.TextBox
    Friend WithEvents LabelCountedQtyValue As System.Windows.Forms.Label
    Friend WithEvents LabelCountedQtyTitle As System.Windows.Forms.Label
    Friend WithEvents LabelTotalQtyValue As System.Windows.Forms.Label
    Friend WithEvents LabelTotalQtyTitle As System.Windows.Forms.Label
    Friend WithEvents LabelTagCountValue As System.Windows.Forms.Label
    Friend WithEvents LabelTagCountTitle As System.Windows.Forms.Label
    Friend WithEvents ButtonReset As System.Windows.Forms.Button
    Friend WithEvents ButtonSave As System.Windows.Forms.Button
    Friend WithEvents timerScanner As System.Windows.Forms.Timer
End Class

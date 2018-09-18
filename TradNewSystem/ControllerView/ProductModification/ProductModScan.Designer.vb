<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class frm_ProductModScan
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
        Me.pnl_Title = New System.Windows.Forms.Panel
        Me.lbl_WindowTitle = New System.Windows.Forms.Label
        Me.btn_FullScreen = New System.Windows.Forms.Button
        Me.lbl_Title = New System.Windows.Forms.Label
        Me.lbl_ScanTag = New System.Windows.Forms.Label
        Me.txt_ScanTag = New System.Windows.Forms.TextBox
        Me.lbl_TRINPartNoTitle = New System.Windows.Forms.Label
        Me.lbl_TRINPartNoValue = New System.Windows.Forms.Label
        Me.lbl_NewTag = New System.Windows.Forms.Label
        Me.btn_Back = New System.Windows.Forms.Button
        Me.btn_Save = New System.Windows.Forms.Button
        Me.cmbNewTRINPartNo = New System.Windows.Forms.ComboBox
        Me.tmr_DbLoad = New System.Windows.Forms.Timer
        Me.lbl_ActualQtyTitle = New System.Windows.Forms.Label
        Me.lbl_ActualQtyValue = New System.Windows.Forms.Label
        Me.lbl_LineNameTitle = New System.Windows.Forms.Label
        Me.lbl_LineNameValue = New System.Windows.Forms.Label
        Me.dgv_ScanResult = New System.Windows.Forms.DataGrid
        Me.SuspendLayout()
        '
        'pnl_Title
        '
        Me.pnl_Title.BackColor = System.Drawing.Color.Navy
        Me.pnl_Title.Location = New System.Drawing.Point(0, 0)
        Me.pnl_Title.Name = "pnl_Title"
        Me.pnl_Title.Size = New System.Drawing.Size(320, 25)
        '
        'lbl_WindowTitle
        '
        Me.lbl_WindowTitle.BackColor = System.Drawing.Color.Navy
        Me.lbl_WindowTitle.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lbl_WindowTitle.ForeColor = System.Drawing.Color.White
        Me.lbl_WindowTitle.Location = New System.Drawing.Point(11, 4)
        Me.lbl_WindowTitle.Name = "lbl_WindowTitle"
        Me.lbl_WindowTitle.Size = New System.Drawing.Size(204, 21)
        Me.lbl_WindowTitle.Text = "Trad New System"
        '
        'btn_FullScreen
        '
        Me.btn_FullScreen.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.btn_FullScreen.Location = New System.Drawing.Point(297, 2)
        Me.btn_FullScreen.Name = "btn_FullScreen"
        Me.btn_FullScreen.Size = New System.Drawing.Size(20, 20)
        Me.btn_FullScreen.TabIndex = 17
        Me.btn_FullScreen.TabStop = False
        Me.btn_FullScreen.Text = "F"
        '
        'lbl_Title
        '
        Me.lbl_Title.Font = New System.Drawing.Font("Tahoma", 16.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.lbl_Title.Location = New System.Drawing.Point(0, 61)
        Me.lbl_Title.Name = "lbl_Title"
        Me.lbl_Title.Size = New System.Drawing.Size(320, 30)
        Me.lbl_Title.Text = "Product Modification"
        Me.lbl_Title.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lbl_ScanTag
        '
        Me.lbl_ScanTag.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_ScanTag.Location = New System.Drawing.Point(21, 106)
        Me.lbl_ScanTag.Name = "lbl_ScanTag"
        Me.lbl_ScanTag.Size = New System.Drawing.Size(276, 20)
        Me.lbl_ScanTag.Text = "Scan Tag :"
        '
        'txt_ScanTag
        '
        Me.txt_ScanTag.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.txt_ScanTag.Location = New System.Drawing.Point(21, 129)
        Me.txt_ScanTag.Name = "txt_ScanTag"
        Me.txt_ScanTag.Size = New System.Drawing.Size(276, 26)
        Me.txt_ScanTag.TabIndex = 1
        '
        'lbl_TRINPartNoTitle
        '
        Me.lbl_TRINPartNoTitle.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_TRINPartNoTitle.Location = New System.Drawing.Point(21, 158)
        Me.lbl_TRINPartNoTitle.Name = "lbl_TRINPartNoTitle"
        Me.lbl_TRINPartNoTitle.Size = New System.Drawing.Size(105, 20)
        Me.lbl_TRINPartNoTitle.Text = "TRIN Part No :"
        '
        'lbl_TRINPartNoValue
        '
        Me.lbl_TRINPartNoValue.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_TRINPartNoValue.Location = New System.Drawing.Point(132, 158)
        Me.lbl_TRINPartNoValue.Name = "lbl_TRINPartNoValue"
        Me.lbl_TRINPartNoValue.Size = New System.Drawing.Size(165, 20)
        Me.lbl_TRINPartNoValue.Text = "0"
        '
        'lbl_NewTag
        '
        Me.lbl_NewTag.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_NewTag.Location = New System.Drawing.Point(21, 318)
        Me.lbl_NewTag.Name = "lbl_NewTag"
        Me.lbl_NewTag.Size = New System.Drawing.Size(276, 20)
        Me.lbl_NewTag.Text = "New Tag :"
        '
        'btn_Back
        '
        Me.btn_Back.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.btn_Back.Location = New System.Drawing.Point(177, 387)
        Me.btn_Back.Name = "btn_Back"
        Me.btn_Back.Size = New System.Drawing.Size(120, 20)
        Me.btn_Back.TabIndex = 30
        Me.btn_Back.TabStop = False
        Me.btn_Back.Text = "F2 Back"
        '
        'btn_Save
        '
        Me.btn_Save.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.btn_Save.Location = New System.Drawing.Point(21, 387)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(120, 20)
        Me.btn_Save.TabIndex = 29
        Me.btn_Save.TabStop = False
        Me.btn_Save.Text = "F1 Save"
        '
        'cmbNewTRINPartNo
        '
        Me.cmbNewTRINPartNo.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.cmbNewTRINPartNo.Location = New System.Drawing.Point(21, 341)
        Me.cmbNewTRINPartNo.Name = "cmbNewTRINPartNo"
        Me.cmbNewTRINPartNo.Size = New System.Drawing.Size(276, 26)
        Me.cmbNewTRINPartNo.TabIndex = 38
        '
        'tmr_DbLoad
        '
        '
        'lbl_ActualQtyTitle
        '
        Me.lbl_ActualQtyTitle.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_ActualQtyTitle.Location = New System.Drawing.Point(21, 178)
        Me.lbl_ActualQtyTitle.Name = "lbl_ActualQtyTitle"
        Me.lbl_ActualQtyTitle.Size = New System.Drawing.Size(88, 20)
        Me.lbl_ActualQtyTitle.Text = "Actual Qty :"
        '
        'lbl_ActualQtyValue
        '
        Me.lbl_ActualQtyValue.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_ActualQtyValue.Location = New System.Drawing.Point(115, 178)
        Me.lbl_ActualQtyValue.Name = "lbl_ActualQtyValue"
        Me.lbl_ActualQtyValue.Size = New System.Drawing.Size(165, 20)
        Me.lbl_ActualQtyValue.Text = "0"
        '
        'lbl_LineNameTitle
        '
        Me.lbl_LineNameTitle.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_LineNameTitle.Location = New System.Drawing.Point(21, 198)
        Me.lbl_LineNameTitle.Name = "lbl_LineNameTitle"
        Me.lbl_LineNameTitle.Size = New System.Drawing.Size(88, 20)
        Me.lbl_LineNameTitle.Text = "Line Name :"
        '
        'lbl_LineNameValue
        '
        Me.lbl_LineNameValue.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_LineNameValue.Location = New System.Drawing.Point(115, 198)
        Me.lbl_LineNameValue.Name = "lbl_LineNameValue"
        Me.lbl_LineNameValue.Size = New System.Drawing.Size(165, 20)
        Me.lbl_LineNameValue.Text = "XXX"
        '
        'dgv_ScanResult
        '
        Me.dgv_ScanResult.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.dgv_ScanResult.Location = New System.Drawing.Point(21, 221)
        Me.dgv_ScanResult.Name = "dgv_ScanResult"
        Me.dgv_ScanResult.RowHeadersVisible = False
        Me.dgv_ScanResult.Size = New System.Drawing.Size(276, 94)
        Me.dgv_ScanResult.TabIndex = 46
        '
        'frm_ProductModScan
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.Khaki
        Me.ClientSize = New System.Drawing.Size(320, 480)
        Me.ControlBox = False
        Me.Controls.Add(Me.dgv_ScanResult)
        Me.Controls.Add(Me.lbl_LineNameValue)
        Me.Controls.Add(Me.lbl_LineNameTitle)
        Me.Controls.Add(Me.lbl_ActualQtyValue)
        Me.Controls.Add(Me.lbl_ActualQtyTitle)
        Me.Controls.Add(Me.cmbNewTRINPartNo)
        Me.Controls.Add(Me.btn_Back)
        Me.Controls.Add(Me.btn_Save)
        Me.Controls.Add(Me.lbl_NewTag)
        Me.Controls.Add(Me.lbl_TRINPartNoValue)
        Me.Controls.Add(Me.lbl_TRINPartNoTitle)
        Me.Controls.Add(Me.txt_ScanTag)
        Me.Controls.Add(Me.lbl_ScanTag)
        Me.Controls.Add(Me.lbl_Title)
        Me.Controls.Add(Me.btn_FullScreen)
        Me.Controls.Add(Me.lbl_WindowTitle)
        Me.Controls.Add(Me.pnl_Title)
        Me.ForeColor = System.Drawing.Color.Black
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frm_ProductModScan"
        Me.Text = "Product Modification"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnl_Title As System.Windows.Forms.Panel
    Friend WithEvents lbl_WindowTitle As System.Windows.Forms.Label
    Friend WithEvents btn_FullScreen As System.Windows.Forms.Button
    Friend WithEvents lbl_Title As System.Windows.Forms.Label
    Friend WithEvents lbl_ScanTag As System.Windows.Forms.Label
    Friend WithEvents txt_ScanTag As System.Windows.Forms.TextBox
    Friend WithEvents lbl_TRINPartNoTitle As System.Windows.Forms.Label
    Friend WithEvents lbl_TRINPartNoValue As System.Windows.Forms.Label
    Friend WithEvents lbl_NewTag As System.Windows.Forms.Label
    Friend WithEvents btn_Back As System.Windows.Forms.Button
    Friend WithEvents btn_Save As System.Windows.Forms.Button
    Friend WithEvents cmbNewTRINPartNo As System.Windows.Forms.ComboBox
    Friend WithEvents tmr_DbLoad As System.Windows.Forms.Timer
    Friend WithEvents lbl_ActualQtyTitle As System.Windows.Forms.Label
    Friend WithEvents lbl_ActualQtyValue As System.Windows.Forms.Label
    Friend WithEvents lbl_LineNameTitle As System.Windows.Forms.Label
    Friend WithEvents lbl_LineNameValue As System.Windows.Forms.Label
    Friend WithEvents dgv_ScanResult As System.Windows.Forms.DataGrid
End Class

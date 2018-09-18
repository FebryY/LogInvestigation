<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class frm_IdTagIntegration
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
        Me.dgv_ScanData = New System.Windows.Forms.DataGrid
        Me.pnl_Title = New System.Windows.Forms.Panel
        Me.lbl_WindowTitle = New System.Windows.Forms.Label
        Me.btn_FullScreen = New System.Windows.Forms.Button
        Me.lbl_Title = New System.Windows.Forms.Label
        Me.lbl_ScanTag = New System.Windows.Forms.Label
        Me.txt_ScanTag = New System.Windows.Forms.TextBox
        Me.lbl_TRINPartNoTitle = New System.Windows.Forms.Label
        Me.lbl_TRINPartNoValue = New System.Windows.Forms.Label
        Me.lbl_ActualQtyTitle = New System.Windows.Forms.Label
        Me.lbl_ActualQtyValue = New System.Windows.Forms.Label
        Me.btn_Merge = New System.Windows.Forms.Button
        Me.btn_Back = New System.Windows.Forms.Button
        Me.lbl_TotalTagTitle = New System.Windows.Forms.Label
        Me.lbl_TotalPackedQtyTitle = New System.Windows.Forms.Label
        Me.lbl_TotalTagValue = New System.Windows.Forms.Label
        Me.lbl_TotalPackedQtyValue = New System.Windows.Forms.Label
        Me.tmr_DbLoad = New System.Windows.Forms.Timer
        Me.lbl_StdQtyValue = New System.Windows.Forms.Label
        Me.lbl_StdQtyTitle = New System.Windows.Forms.Label
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
        Me.btn_FullScreen.TabIndex = 18
        Me.btn_FullScreen.TabStop = False
        Me.btn_FullScreen.Text = "F"
        '
        'lbl_Title
        '
        Me.lbl_Title.Font = New System.Drawing.Font("Tahoma", 16.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.lbl_Title.Location = New System.Drawing.Point(0, 61)
        Me.lbl_Title.Name = "lbl_Title"
        Me.lbl_Title.Size = New System.Drawing.Size(320, 30)
        Me.lbl_Title.Text = "ID Tag Integration"
        Me.lbl_Title.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lbl_ScanTag
        '
        Me.lbl_ScanTag.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_ScanTag.Location = New System.Drawing.Point(21, 100)
        Me.lbl_ScanTag.Name = "lbl_ScanTag"
        Me.lbl_ScanTag.Size = New System.Drawing.Size(276, 20)
        Me.lbl_ScanTag.Text = "Scan Tag :"
        '
        'txt_ScanTag
        '
        Me.txt_ScanTag.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.txt_ScanTag.Location = New System.Drawing.Point(21, 123)
        Me.txt_ScanTag.Name = "txt_ScanTag"
        Me.txt_ScanTag.Size = New System.Drawing.Size(276, 26)
        Me.txt_ScanTag.TabIndex = 1
        '
        'lbl_TRINPartNoTitle
        '
        Me.lbl_TRINPartNoTitle.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_TRINPartNoTitle.Location = New System.Drawing.Point(21, 155)
        Me.lbl_TRINPartNoTitle.Name = "lbl_TRINPartNoTitle"
        Me.lbl_TRINPartNoTitle.Size = New System.Drawing.Size(105, 20)
        Me.lbl_TRINPartNoTitle.Text = "TRIN Part No :"
        '
        'lbl_TRINPartNoValue
        '
        Me.lbl_TRINPartNoValue.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_TRINPartNoValue.Location = New System.Drawing.Point(132, 155)
        Me.lbl_TRINPartNoValue.Name = "lbl_TRINPartNoValue"
        Me.lbl_TRINPartNoValue.Size = New System.Drawing.Size(165, 20)
        Me.lbl_TRINPartNoValue.Text = "0"
        '
        'lbl_ActualQtyTitle
        '
        Me.lbl_ActualQtyTitle.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_ActualQtyTitle.Location = New System.Drawing.Point(21, 175)
        Me.lbl_ActualQtyTitle.Name = "lbl_ActualQtyTitle"
        Me.lbl_ActualQtyTitle.Size = New System.Drawing.Size(105, 16)
        Me.lbl_ActualQtyTitle.Text = "Product Actual Qty :"
        '
        'lbl_ActualQtyValue
        '
        Me.lbl_ActualQtyValue.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_ActualQtyValue.Location = New System.Drawing.Point(132, 175)
        Me.lbl_ActualQtyValue.Name = "lbl_ActualQtyValue"
        Me.lbl_ActualQtyValue.Size = New System.Drawing.Size(45, 16)
        Me.lbl_ActualQtyValue.Text = "0"
        '
        'btn_Merge
        '
        Me.btn_Merge.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.btn_Merge.Location = New System.Drawing.Point(21, 387)
        Me.btn_Merge.Name = "btn_Merge"
        Me.btn_Merge.Size = New System.Drawing.Size(120, 20)
        Me.btn_Merge.TabIndex = 30
        Me.btn_Merge.TabStop = False
        Me.btn_Merge.Text = "F1 Merge Tags"
        '
        'btn_Back
        '
        Me.btn_Back.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.btn_Back.Location = New System.Drawing.Point(177, 387)
        Me.btn_Back.Name = "btn_Back"
        Me.btn_Back.Size = New System.Drawing.Size(120, 20)
        Me.btn_Back.TabIndex = 31
        Me.btn_Back.TabStop = False
        Me.btn_Back.Text = "F2 Back"
        '
        'lbl_TotalTagTitle
        '
        Me.lbl_TotalTagTitle.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_TotalTagTitle.Location = New System.Drawing.Point(21, 350)
        Me.lbl_TotalTagTitle.Name = "lbl_TotalTagTitle"
        Me.lbl_TotalTagTitle.Size = New System.Drawing.Size(73, 16)
        Me.lbl_TotalTagTitle.Text = "Total Tag(s) :"
        '
        'lbl_TotalPackedQtyTitle
        '
        Me.lbl_TotalPackedQtyTitle.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_TotalPackedQtyTitle.Location = New System.Drawing.Point(21, 366)
        Me.lbl_TotalPackedQtyTitle.Name = "lbl_TotalPackedQtyTitle"
        Me.lbl_TotalPackedQtyTitle.Size = New System.Drawing.Size(100, 15)
        Me.lbl_TotalPackedQtyTitle.Text = "Total Packed Qty :"
        '
        'lbl_TotalTagValue
        '
        Me.lbl_TotalTagValue.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_TotalTagValue.Location = New System.Drawing.Point(100, 350)
        Me.lbl_TotalTagValue.Name = "lbl_TotalTagValue"
        Me.lbl_TotalTagValue.Size = New System.Drawing.Size(105, 15)
        Me.lbl_TotalTagValue.Text = "0"
        '
        'lbl_TotalPackedQtyValue
        '
        Me.lbl_TotalPackedQtyValue.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_TotalPackedQtyValue.Location = New System.Drawing.Point(127, 365)
        Me.lbl_TotalPackedQtyValue.Name = "lbl_TotalPackedQtyValue"
        Me.lbl_TotalPackedQtyValue.Size = New System.Drawing.Size(105, 15)
        Me.lbl_TotalPackedQtyValue.Text = "0"
        '
        'dgv_ScanData
        '
        dgv_ScanData.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        dgv_ScanData.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Regular)
        dgv_ScanData.Location = New System.Drawing.Point(21, 194)
        dgv_ScanData.Name = "dgv_ScanData"
        dgv_ScanData.RowHeadersVisible = False
        dgv_ScanData.Size = New System.Drawing.Size(276, 153)
        dgv_ScanData.TabIndex = 40
        dgv_ScanData.TabStop = False
        AddHandler dgv_ScanData.KeyUp, AddressOf Me.dgv_ScanData_KeyUp
        '
        'tmr_DbLoad
        '
        '
        'lbl_StdQtyValue
        '
        Me.lbl_StdQtyValue.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_StdQtyValue.Location = New System.Drawing.Point(268, 175)
        Me.lbl_StdQtyValue.Name = "lbl_StdQtyValue"
        Me.lbl_StdQtyValue.Size = New System.Drawing.Size(45, 16)
        Me.lbl_StdQtyValue.Text = "0"
        '
        'lbl_StdQtyTitle
        '
        Me.lbl_StdQtyTitle.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_StdQtyTitle.Location = New System.Drawing.Point(180, 175)
        Me.lbl_StdQtyTitle.Name = "lbl_StdQtyTitle"
        Me.lbl_StdQtyTitle.Size = New System.Drawing.Size(82, 16)
        Me.lbl_StdQtyTitle.Text = "Standard Qty :"
        '
        'frm_IdTagIntegration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.Khaki
        Me.ClientSize = New System.Drawing.Size(320, 480)
        Me.ControlBox = False
        Me.Controls.Add(Me.lbl_StdQtyValue)
        Me.Controls.Add(Me.lbl_StdQtyTitle)
        Me.Controls.Add(dgv_ScanData)
        Me.Controls.Add(Me.lbl_TotalPackedQtyValue)
        Me.Controls.Add(Me.lbl_TotalTagValue)
        Me.Controls.Add(Me.lbl_TotalPackedQtyTitle)
        Me.Controls.Add(Me.lbl_TotalTagTitle)
        Me.Controls.Add(Me.btn_Back)
        Me.Controls.Add(Me.btn_Merge)
        Me.Controls.Add(Me.lbl_ActualQtyValue)
        Me.Controls.Add(Me.lbl_ActualQtyTitle)
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
        Me.Name = "frm_IdTagIntegration"
        Me.Text = "IdTagIntegration"
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
    Friend WithEvents lbl_ActualQtyTitle As System.Windows.Forms.Label
    Friend WithEvents lbl_ActualQtyValue As System.Windows.Forms.Label
    Friend WithEvents btn_Merge As System.Windows.Forms.Button
    Friend WithEvents btn_Back As System.Windows.Forms.Button
    Friend WithEvents lbl_TotalTagTitle As System.Windows.Forms.Label
    Friend WithEvents lbl_TotalPackedQtyTitle As System.Windows.Forms.Label
    Friend WithEvents lbl_TotalTagValue As System.Windows.Forms.Label
    Friend WithEvents lbl_TotalPackedQtyValue As System.Windows.Forms.Label
    Friend WithEvents dgv_ScanData As System.Windows.Forms.DataGrid
    Friend WithEvents tmr_DbLoad As System.Windows.Forms.Timer
    Friend WithEvents lbl_StdQtyValue As System.Windows.Forms.Label
    Friend WithEvents lbl_StdQtyTitle As System.Windows.Forms.Label
End Class

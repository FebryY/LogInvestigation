<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class frm_IdTagClassification
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
        Me.btn_FullScreen = New System.Windows.Forms.Button
        Me.lbl_WindowTitle = New System.Windows.Forms.Label
        Me.lbl_Title = New System.Windows.Forms.Label
        Me.txt_ScanTag = New System.Windows.Forms.TextBox
        Me.lbl_ScanTag = New System.Windows.Forms.Label
        Me.lbl_TRINPartNoValue = New System.Windows.Forms.Label
        Me.lbl_TRINPartNoTitle = New System.Windows.Forms.Label
        Me.lbl_ActualQtyValue = New System.Windows.Forms.Label
        Me.lbl_ActualQtyTitle = New System.Windows.Forms.Label
        Me.lbl_StandardQtyTitle = New System.Windows.Forms.Label
        Me.lbl_StandardQtyValue = New System.Windows.Forms.Label
        Me.btn_Back = New System.Windows.Forms.Button
        Me.btn_Split = New System.Windows.Forms.Button
        Me.lbl_SplitTag = New System.Windows.Forms.Label
        Me.lbl_SplitQty = New System.Windows.Forms.Label
        Me.lbl_Tag1 = New System.Windows.Forms.Label
        Me.lbl_Tag2 = New System.Windows.Forms.Label
        Me.lbl_Tag3 = New System.Windows.Forms.Label
        Me.lbl_Tag4 = New System.Windows.Forms.Label
        Me.txt_Tag1Value = New System.Windows.Forms.TextBox
        Me.txt_Tag2Value = New System.Windows.Forms.TextBox
        Me.txt_Tag3Value = New System.Windows.Forms.TextBox
        Me.txt_Tag4Value = New System.Windows.Forms.TextBox
        Me.tmr_DbLoad = New System.Windows.Forms.Timer
        Me.lbl_TotalTitle = New System.Windows.Forms.Label
        Me.lbl_TotalValue = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'pnl_Title
        '
        Me.pnl_Title.BackColor = System.Drawing.Color.Navy
        Me.pnl_Title.Location = New System.Drawing.Point(0, 0)
        Me.pnl_Title.Name = "pnl_Title"
        Me.pnl_Title.Size = New System.Drawing.Size(320, 25)
        '
        'btn_FullScreen
        '
        Me.btn_FullScreen.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.btn_FullScreen.Location = New System.Drawing.Point(297, 2)
        Me.btn_FullScreen.Name = "btn_FullScreen"
        Me.btn_FullScreen.Size = New System.Drawing.Size(20, 20)
        Me.btn_FullScreen.TabIndex = 19
        Me.btn_FullScreen.TabStop = False
        Me.btn_FullScreen.Text = "F"
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
        'lbl_Title
        '
        Me.lbl_Title.Font = New System.Drawing.Font("Tahoma", 16.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.lbl_Title.Location = New System.Drawing.Point(0, 61)
        Me.lbl_Title.Name = "lbl_Title"
        Me.lbl_Title.Size = New System.Drawing.Size(320, 30)
        Me.lbl_Title.Text = "ID Tag Classification"
        Me.lbl_Title.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txt_ScanTag
        '
        Me.txt_ScanTag.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.txt_ScanTag.Location = New System.Drawing.Point(21, 123)
        Me.txt_ScanTag.Name = "txt_ScanTag"
        Me.txt_ScanTag.Size = New System.Drawing.Size(276, 26)
        Me.txt_ScanTag.TabIndex = 1
        '
        'lbl_ScanTag
        '
        Me.lbl_ScanTag.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_ScanTag.Location = New System.Drawing.Point(21, 100)
        Me.lbl_ScanTag.Name = "lbl_ScanTag"
        Me.lbl_ScanTag.Size = New System.Drawing.Size(276, 20)
        Me.lbl_ScanTag.Text = "Scan Tag :"
        '
        'lbl_TRINPartNoValue
        '
        Me.lbl_TRINPartNoValue.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_TRINPartNoValue.Location = New System.Drawing.Point(132, 155)
        Me.lbl_TRINPartNoValue.Name = "lbl_TRINPartNoValue"
        Me.lbl_TRINPartNoValue.Size = New System.Drawing.Size(165, 20)
        Me.lbl_TRINPartNoValue.Text = "0"
        '
        'lbl_TRINPartNoTitle
        '
        Me.lbl_TRINPartNoTitle.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_TRINPartNoTitle.Location = New System.Drawing.Point(21, 155)
        Me.lbl_TRINPartNoTitle.Name = "lbl_TRINPartNoTitle"
        Me.lbl_TRINPartNoTitle.Size = New System.Drawing.Size(105, 20)
        Me.lbl_TRINPartNoTitle.Text = "TRIN Part No :"
        '
        'lbl_ActualQtyValue
        '
        Me.lbl_ActualQtyValue.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_ActualQtyValue.Location = New System.Drawing.Point(281, 175)
        Me.lbl_ActualQtyValue.Name = "lbl_ActualQtyValue"
        Me.lbl_ActualQtyValue.Size = New System.Drawing.Size(35, 16)
        Me.lbl_ActualQtyValue.Text = "0"
        '
        'lbl_ActualQtyTitle
        '
        Me.lbl_ActualQtyTitle.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_ActualQtyTitle.Location = New System.Drawing.Point(170, 175)
        Me.lbl_ActualQtyTitle.Name = "lbl_ActualQtyTitle"
        Me.lbl_ActualQtyTitle.Size = New System.Drawing.Size(105, 16)
        Me.lbl_ActualQtyTitle.Text = "Product Actual Qty :"
        '
        'lbl_StandardQtyTitle
        '
        Me.lbl_StandardQtyTitle.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_StandardQtyTitle.Location = New System.Drawing.Point(31, 175)
        Me.lbl_StandardQtyTitle.Name = "lbl_StandardQtyTitle"
        Me.lbl_StandardQtyTitle.Size = New System.Drawing.Size(79, 16)
        Me.lbl_StandardQtyTitle.Text = "Standard Qty :"
        '
        'lbl_StandardQtyValue
        '
        Me.lbl_StandardQtyValue.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lbl_StandardQtyValue.Location = New System.Drawing.Point(116, 175)
        Me.lbl_StandardQtyValue.Name = "lbl_StandardQtyValue"
        Me.lbl_StandardQtyValue.Size = New System.Drawing.Size(35, 16)
        Me.lbl_StandardQtyValue.Text = "0"
        '
        'btn_Back
        '
        Me.btn_Back.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.btn_Back.Location = New System.Drawing.Point(177, 387)
        Me.btn_Back.Name = "btn_Back"
        Me.btn_Back.Size = New System.Drawing.Size(120, 20)
        Me.btn_Back.TabIndex = 40
        Me.btn_Back.TabStop = False
        Me.btn_Back.Text = "F2 Back"
        '
        'btn_Split
        '
        Me.btn_Split.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular)
        Me.btn_Split.Location = New System.Drawing.Point(21, 387)
        Me.btn_Split.Name = "btn_Split"
        Me.btn_Split.Size = New System.Drawing.Size(120, 20)
        Me.btn_Split.TabIndex = 39
        Me.btn_Split.TabStop = False
        Me.btn_Split.Text = "F1 Split Tags"
        '
        'lbl_SplitTag
        '
        Me.lbl_SplitTag.Location = New System.Drawing.Point(50, 202)
        Me.lbl_SplitTag.Name = "lbl_SplitTag"
        Me.lbl_SplitTag.Size = New System.Drawing.Size(58, 18)
        Me.lbl_SplitTag.Text = "Split Tag"
        '
        'lbl_SplitQty
        '
        Me.lbl_SplitQty.Location = New System.Drawing.Point(194, 202)
        Me.lbl_SplitQty.Name = "lbl_SplitQty"
        Me.lbl_SplitQty.Size = New System.Drawing.Size(88, 18)
        Me.lbl_SplitQty.Text = "Split Quantity"
        '
        'lbl_Tag1
        '
        Me.lbl_Tag1.Location = New System.Drawing.Point(50, 235)
        Me.lbl_Tag1.Name = "lbl_Tag1"
        Me.lbl_Tag1.Size = New System.Drawing.Size(58, 18)
        Me.lbl_Tag1.Text = "Tag 1"
        Me.lbl_Tag1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lbl_Tag2
        '
        Me.lbl_Tag2.Location = New System.Drawing.Point(50, 264)
        Me.lbl_Tag2.Name = "lbl_Tag2"
        Me.lbl_Tag2.Size = New System.Drawing.Size(58, 18)
        Me.lbl_Tag2.Text = "Tag 2"
        Me.lbl_Tag2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lbl_Tag3
        '
        Me.lbl_Tag3.Location = New System.Drawing.Point(50, 293)
        Me.lbl_Tag3.Name = "lbl_Tag3"
        Me.lbl_Tag3.Size = New System.Drawing.Size(58, 18)
        Me.lbl_Tag3.Text = "Tag 3"
        Me.lbl_Tag3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lbl_Tag4
        '
        Me.lbl_Tag4.Location = New System.Drawing.Point(50, 321)
        Me.lbl_Tag4.Name = "lbl_Tag4"
        Me.lbl_Tag4.Size = New System.Drawing.Size(58, 18)
        Me.lbl_Tag4.Text = "Tag 4"
        Me.lbl_Tag4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txt_Tag1Value
        '
        Me.txt_Tag1Value.Location = New System.Drawing.Point(177, 232)
        Me.txt_Tag1Value.Name = "txt_Tag1Value"
        Me.txt_Tag1Value.Size = New System.Drawing.Size(120, 23)
        Me.txt_Tag1Value.TabIndex = 57
        '
        'txt_Tag2Value
        '
        Me.txt_Tag2Value.Location = New System.Drawing.Point(177, 261)
        Me.txt_Tag2Value.Name = "txt_Tag2Value"
        Me.txt_Tag2Value.Size = New System.Drawing.Size(120, 23)
        Me.txt_Tag2Value.TabIndex = 58
        '
        'txt_Tag3Value
        '
        Me.txt_Tag3Value.Location = New System.Drawing.Point(177, 290)
        Me.txt_Tag3Value.Name = "txt_Tag3Value"
        Me.txt_Tag3Value.Size = New System.Drawing.Size(120, 23)
        Me.txt_Tag3Value.TabIndex = 59
        '
        'txt_Tag4Value
        '
        Me.txt_Tag4Value.Location = New System.Drawing.Point(177, 319)
        Me.txt_Tag4Value.Name = "txt_Tag4Value"
        Me.txt_Tag4Value.Size = New System.Drawing.Size(120, 23)
        Me.txt_Tag4Value.TabIndex = 60
        '
        'tmr_DbLoad
        '
        '
        'lbl_TotalTitle
        '
        Me.lbl_TotalTitle.Location = New System.Drawing.Point(63, 355)
        Me.lbl_TotalTitle.Name = "lbl_TotalTitle"
        Me.lbl_TotalTitle.Size = New System.Drawing.Size(35, 18)
        Me.lbl_TotalTitle.Text = "Total"
        '
        'lbl_TotalValue
        '
        Me.lbl_TotalValue.Location = New System.Drawing.Point(228, 355)
        Me.lbl_TotalValue.Name = "lbl_TotalValue"
        Me.lbl_TotalValue.Size = New System.Drawing.Size(35, 18)
        Me.lbl_TotalValue.Text = "0"
        '
        'frm_IdTagClassification
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.Khaki
        Me.ClientSize = New System.Drawing.Size(318, 455)
        Me.ControlBox = False
        Me.Controls.Add(Me.lbl_TotalValue)
        Me.Controls.Add(Me.lbl_TotalTitle)
        Me.Controls.Add(Me.txt_Tag4Value)
        Me.Controls.Add(Me.txt_Tag3Value)
        Me.Controls.Add(Me.txt_Tag2Value)
        Me.Controls.Add(Me.txt_Tag1Value)
        Me.Controls.Add(Me.lbl_Tag4)
        Me.Controls.Add(Me.lbl_Tag3)
        Me.Controls.Add(Me.lbl_Tag2)
        Me.Controls.Add(Me.lbl_Tag1)
        Me.Controls.Add(Me.lbl_SplitQty)
        Me.Controls.Add(Me.lbl_SplitTag)
        Me.Controls.Add(Me.btn_Back)
        Me.Controls.Add(Me.btn_Split)
        Me.Controls.Add(Me.lbl_StandardQtyValue)
        Me.Controls.Add(Me.lbl_StandardQtyTitle)
        Me.Controls.Add(Me.lbl_ActualQtyValue)
        Me.Controls.Add(Me.lbl_ActualQtyTitle)
        Me.Controls.Add(Me.lbl_TRINPartNoValue)
        Me.Controls.Add(Me.lbl_TRINPartNoTitle)
        Me.Controls.Add(Me.txt_ScanTag)
        Me.Controls.Add(Me.lbl_ScanTag)
        Me.Controls.Add(Me.lbl_Title)
        Me.Controls.Add(Me.lbl_WindowTitle)
        Me.Controls.Add(Me.btn_FullScreen)
        Me.Controls.Add(Me.pnl_Title)
        Me.ForeColor = System.Drawing.Color.Black
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frm_IdTagClassification"
        Me.Text = "IdTagClassification"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnl_Title As System.Windows.Forms.Panel
    Friend WithEvents btn_FullScreen As System.Windows.Forms.Button
    Friend WithEvents lbl_WindowTitle As System.Windows.Forms.Label
    Friend WithEvents lbl_Title As System.Windows.Forms.Label
    Friend WithEvents txt_ScanTag As System.Windows.Forms.TextBox
    Friend WithEvents lbl_ScanTag As System.Windows.Forms.Label
    Friend WithEvents lbl_TRINPartNoValue As System.Windows.Forms.Label
    Friend WithEvents lbl_TRINPartNoTitle As System.Windows.Forms.Label
    Friend WithEvents lbl_ActualQtyValue As System.Windows.Forms.Label
    Friend WithEvents lbl_ActualQtyTitle As System.Windows.Forms.Label
    Friend WithEvents lbl_StandardQtyTitle As System.Windows.Forms.Label
    Friend WithEvents lbl_StandardQtyValue As System.Windows.Forms.Label
    Friend WithEvents btn_Back As System.Windows.Forms.Button
    Friend WithEvents btn_Split As System.Windows.Forms.Button
    Friend WithEvents lbl_SplitTag As System.Windows.Forms.Label
    Friend WithEvents lbl_SplitQty As System.Windows.Forms.Label
    Friend WithEvents lbl_Tag1 As System.Windows.Forms.Label
    Friend WithEvents lbl_Tag2 As System.Windows.Forms.Label
    Friend WithEvents lbl_Tag3 As System.Windows.Forms.Label
    Friend WithEvents lbl_Tag4 As System.Windows.Forms.Label
    Friend WithEvents txt_Tag1Value As System.Windows.Forms.TextBox
    Friend WithEvents txt_Tag2Value As System.Windows.Forms.TextBox
    Friend WithEvents txt_Tag3Value As System.Windows.Forms.TextBox
    Friend WithEvents txt_Tag4Value As System.Windows.Forms.TextBox
    Friend WithEvents tmr_DbLoad As System.Windows.Forms.Timer
    Friend WithEvents lbl_TotalTitle As System.Windows.Forms.Label
    Friend WithEvents lbl_TotalValue As System.Windows.Forms.Label
End Class

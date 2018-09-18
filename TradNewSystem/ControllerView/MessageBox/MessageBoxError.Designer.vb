<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class MessageBoxError
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MessageBoxError))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.ButtonOK = New System.Windows.Forms.Button
        Me.LabelMessage = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(15, 20)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(40, 50)
        '
        'ButtonOK
        '
        Me.ButtonOK.Location = New System.Drawing.Point(90, 120)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(60, 40)
        Me.ButtonOK.TabIndex = 1
        Me.ButtonOK.Text = "OK"
        '
        'LabelMessage
        '
        Me.LabelMessage.Location = New System.Drawing.Point(61, 20)
        Me.LabelMessage.Name = "LabelMessage"
        Me.LabelMessage.Size = New System.Drawing.Size(162, 85)
        Me.LabelMessage.Text = "Label1"
        '
        'MessageBoxError
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(238, 175)
        Me.ControlBox = False
        Me.Controls.Add(Me.LabelMessage)
        Me.Controls.Add(Me.ButtonOK)
        Me.Controls.Add(Me.PictureBox1)
        Me.Location = New System.Drawing.Point(40, 140)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MessageBoxError"
        Me.Text = "MessageBoxError"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonOK As System.Windows.Forms.Button
    Friend WithEvents LabelMessage As System.Windows.Forms.Label
End Class

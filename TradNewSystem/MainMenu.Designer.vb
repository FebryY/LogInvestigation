<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class MainMenu
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
        Me.PanelTitle = New System.Windows.Forms.Panel
        Me.ButtonFullScreen = New System.Windows.Forms.Button
        Me.LabelWindowTitle = New System.Windows.Forms.Label
        Me.ButtonIdTagIntegration = New System.Windows.Forms.Button
        Me.ButtonShipment = New System.Windows.Forms.Button
        Me.ButtonIdTagClassification = New System.Windows.Forms.Button
        Me.ButtonStoctake = New System.Windows.Forms.Button
        Me.ButtonOfflineStocktake = New System.Windows.Forms.Button
        Me.ButtonProductModification = New System.Windows.Forms.Button
        Me.ButtonLogOff = New System.Windows.Forms.Button
        Me.PanelTitle.SuspendLayout()
        Me.SuspendLayout()
        '
        'LabelTitle
        '
        Me.LabelTitle.Font = New System.Drawing.Font("Tahoma", 16.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.LabelTitle.Location = New System.Drawing.Point(0, 61)
        Me.LabelTitle.Name = "LabelTitle"
        Me.LabelTitle.Size = New System.Drawing.Size(318, 30)
        Me.LabelTitle.Text = "Main Menu"
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
        'ButtonIdTagIntegration
        '
        Me.ButtonIdTagIntegration.Font = New System.Drawing.Font("Tahoma", 14.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonIdTagIntegration.Location = New System.Drawing.Point(21, 162)
        Me.ButtonIdTagIntegration.Name = "ButtonIdTagIntegration"
        Me.ButtonIdTagIntegration.Size = New System.Drawing.Size(276, 30)
        Me.ButtonIdTagIntegration.TabIndex = 2
        Me.ButtonIdTagIntegration.TabStop = False
        Me.ButtonIdTagIntegration.Text = "2. ID Tag Integration"
        '
        'ButtonShipment
        '
        Me.ButtonShipment.Font = New System.Drawing.Font("Tahoma", 14.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonShipment.Location = New System.Drawing.Point(21, 126)
        Me.ButtonShipment.Name = "ButtonShipment"
        Me.ButtonShipment.Size = New System.Drawing.Size(276, 30)
        Me.ButtonShipment.TabIndex = 1
        Me.ButtonShipment.TabStop = False
        Me.ButtonShipment.Text = "1. Shipment"
        '
        'ButtonIdTagClassification
        '
        Me.ButtonIdTagClassification.Font = New System.Drawing.Font("Tahoma", 14.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonIdTagClassification.Location = New System.Drawing.Point(21, 198)
        Me.ButtonIdTagClassification.Name = "ButtonIdTagClassification"
        Me.ButtonIdTagClassification.Size = New System.Drawing.Size(276, 30)
        Me.ButtonIdTagClassification.TabIndex = 3
        Me.ButtonIdTagClassification.TabStop = False
        Me.ButtonIdTagClassification.Text = "3. ID Tag Classification"
        '
        'ButtonStoctake
        '
        Me.ButtonStoctake.Font = New System.Drawing.Font("Tahoma", 14.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonStoctake.Location = New System.Drawing.Point(21, 234)
        Me.ButtonStoctake.Name = "ButtonStoctake"
        Me.ButtonStoctake.Size = New System.Drawing.Size(276, 30)
        Me.ButtonStoctake.TabIndex = 4
        Me.ButtonStoctake.TabStop = False
        Me.ButtonStoctake.Text = "4. Stocktake"
        '
        'ButtonOfflineStocktake
        '
        Me.ButtonOfflineStocktake.Font = New System.Drawing.Font("Tahoma", 14.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonOfflineStocktake.Location = New System.Drawing.Point(21, 270)
        Me.ButtonOfflineStocktake.Name = "ButtonOfflineStocktake"
        Me.ButtonOfflineStocktake.Size = New System.Drawing.Size(276, 30)
        Me.ButtonOfflineStocktake.TabIndex = 5
        Me.ButtonOfflineStocktake.TabStop = False
        Me.ButtonOfflineStocktake.Text = "5. Offline Stocktake"
        '
        'ButtonProductModification
        '
        Me.ButtonProductModification.Font = New System.Drawing.Font("Tahoma", 14.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonProductModification.Location = New System.Drawing.Point(21, 306)
        Me.ButtonProductModification.Name = "ButtonProductModification"
        Me.ButtonProductModification.Size = New System.Drawing.Size(276, 30)
        Me.ButtonProductModification.TabIndex = 6
        Me.ButtonProductModification.TabStop = False
        Me.ButtonProductModification.Text = "6. Product Modification"
        '
        'ButtonLogOff
        '
        Me.ButtonLogOff.Font = New System.Drawing.Font("Tahoma", 14.0!, System.Drawing.FontStyle.Regular)
        Me.ButtonLogOff.Location = New System.Drawing.Point(21, 342)
        Me.ButtonLogOff.Name = "ButtonLogOff"
        Me.ButtonLogOff.Size = New System.Drawing.Size(276, 30)
        Me.ButtonLogOff.TabIndex = 7
        Me.ButtonLogOff.TabStop = False
        Me.ButtonLogOff.Text = "9. Log Off"
        '
        'MainMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.Khaki
        Me.ClientSize = New System.Drawing.Size(320, 480)
        Me.ControlBox = False
        Me.Controls.Add(Me.ButtonLogOff)
        Me.Controls.Add(Me.ButtonProductModification)
        Me.Controls.Add(Me.ButtonOfflineStocktake)
        Me.Controls.Add(Me.ButtonStoctake)
        Me.Controls.Add(Me.ButtonIdTagClassification)
        Me.Controls.Add(Me.ButtonShipment)
        Me.Controls.Add(Me.ButtonIdTagIntegration)
        Me.Controls.Add(Me.PanelTitle)
        Me.Controls.Add(Me.LabelTitle)
        Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MainMenu"
        Me.Text = "Main Menu"
        Me.PanelTitle.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LabelTitle As System.Windows.Forms.Label
    Friend WithEvents PanelTitle As System.Windows.Forms.Panel
    Friend WithEvents ButtonFullScreen As System.Windows.Forms.Button
    Friend WithEvents LabelWindowTitle As System.Windows.Forms.Label
    Friend WithEvents ButtonIdTagIntegration As System.Windows.Forms.Button
    Friend WithEvents ButtonShipment As System.Windows.Forms.Button
    Friend WithEvents ButtonIdTagClassification As System.Windows.Forms.Button
    Friend WithEvents ButtonStoctake As System.Windows.Forms.Button
    Friend WithEvents ButtonOfflineStocktake As System.Windows.Forms.Button
    Friend WithEvents ButtonProductModification As System.Windows.Forms.Button
    Friend WithEvents ButtonLogOff As System.Windows.Forms.Button

End Class

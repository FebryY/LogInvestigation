Option Strict On
Option Explicit On

Public Class MessageBoxError

    Public Sub New(ByVal message As String, ByVal title As String)
        InitializeComponent()

        LabelMessage.Text = message
        Me.Text = title
    End Sub

    Private Sub ButtonOK_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonOK.Click
        Close()
    End Sub

    Private Sub ButtonOK_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonOK.KeyDown
        If e.KeyCode = Keys.Enter Then
            Close()
        End If
    End Sub
End Class
Option Strict On
Option Explicit On

Public Class MessageBoxConfirmation

    Public Sub New(ByVal message As String, ByVal title As String)
        InitializeComponent()

        LabelMessage.Text = message
        Me.Text = title
    End Sub

    Private Sub ButtonYes_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonYes.Click
        TemporaryData.confirmDialogResult = True

        Close()
    End Sub

    Private Sub ButtonYes_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonYes.KeyDown
        TemporaryData.confirmDialogResult = True

        If e.KeyCode = Keys.Enter Then
            Close()
        End If
    End Sub

    Private Sub ButtonNo_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonNo.Click
        TemporaryData.confirmDialogResult = False

        Close()
    End Sub

    Private Sub ButtonNo_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonNo.KeyDown
        TemporaryData.confirmDialogResult = False

        If e.KeyCode = Keys.Enter Then
            Close()
        End If
    End Sub
End Class
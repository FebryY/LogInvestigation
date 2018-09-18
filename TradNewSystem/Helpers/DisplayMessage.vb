Option Strict On
Option Explicit On

Imports DNWA.BHTCL
Imports System.Runtime.InteropServices

Namespace Helpers
    Module DisplayMessage
        Public Sub OkMsg(ByVal msg As String, ByVal title As String)
            BHTController.SoundOK()
            BHTController.ActivateLED(True, DNWA.BHTCL.LED.EN_COLOR.BLUE)

            Dim messageBoxInfo As New MessageBoxInfo(msg, title)

            messageBoxInfo.ShowDialog()
            messageBoxInfo = Nothing

            BHTController.ActivateLED(False)
        End Sub

        Public Sub ErrorMsg( _
            ByVal errorMsg As String, _
            ByVal errorTitle As String _
            )
            BHTController.SoundWarning()
            BHTController.ActivateLED(True, DNWA.BHTCL.LED.EN_COLOR.RED)

            Dim messageBoxError As New MessageBoxError(errorMsg, errorTitle)

            messageBoxError.ShowDialog()
            messageBoxError = Nothing

            BHTController.ActivateLED(False)
        End Sub

        Public Function ConfirmationDialog( _
            ByVal confirmMsg As String, _
            ByVal title As String _
            ) As Boolean
            BHTController.SoundConfirm()
            BHTController.ActivateLED(True, DNWA.BHTCL.LED.EN_COLOR.YELLOW)

            Dim messageBoxConfirmation As New MessageBoxConfirmation(confirmMsg, title)

            messageBoxConfirmation.ShowDialog()
            messageBoxConfirmation = Nothing

            Dim result As Boolean = TemporaryData.confirmDialogResult
            TemporaryData.confirmDialogResult = False

            BHTController.ActivateLED(False)

            Return result
        End Function
    End Module
End Namespace
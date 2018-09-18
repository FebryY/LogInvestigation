Option Strict On
Option Explicit On

Namespace Helpers
    Module ToggleTaskbar
        Declare Function FindWindow Lib "coredll.dll" ( _
            ByVal className As String, _
            ByVal windowName As String _
            ) As Integer
        Declare Function ShowWindow Lib "coredll.dll" ( _
            ByVal hWnd As Integer, _
            ByVal nCmdShow As Integer _
            ) As Integer
        Declare Function EnableWindow Lib "coredll.dll" ( _
            ByVal hwnd As Integer, _
            ByVal bEnable As Boolean _
            ) As Integer

        Public Sub EnableDisable()
            If TemporaryData.taskBarState Then
                Show(False)
                TemporaryData.taskBarState = False
            Else
                Show(True)
                TemporaryData.taskBarState = True
            End If
        End Sub

        Public Sub Show(ByVal shown As Boolean)
            Dim hWnd As Integer = FindWindow( _
                "HHTaskBar".ToCharArray(), _
                Nothing _
                )

            EnableWindow(hWnd, shown)
            ShowWindow(hWnd, -CInt(shown))
        End Sub
    End Module
End Namespace
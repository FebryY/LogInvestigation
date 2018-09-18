Option Strict On
Option Explicit On

Module TemporaryData
    Private _taskBarState As Boolean
    Private _loggedInUserID As String = String.Empty
    Private _confirmDialogResult As Boolean = False

    Public Property taskBarState() As Boolean
        Get
            Return _taskBarState
        End Get

        Set(ByVal value As Boolean)
            _taskBarState = value
        End Set
    End Property

    Public Property loggedInUserID() As String
        Get
            Return _loggedInUserID
        End Get

        Set(ByVal value As String)
            _loggedInUserID = value
        End Set
    End Property

    Public Property confirmDialogResult() As Boolean
        Get
            Return _confirmDialogResult
        End Get

        Set(ByVal value As Boolean)
            _confirmDialogResult = value
        End Set
    End Property
End Module

Option Strict On
Option Explicit On

Namespace PocoClass
    Public Class UserMaster
        Private _userID As String
        Private _userName As String
        Private _fullName As String
        Private _title As String
        Private _comment As String
        Private _userPass As String
        Private _userActive As Boolean
        Private _email As String
        Private _user_logincount As Integer
        Private _last_update As String
        Private _update_time As DateTime
        Private _authority As String
        Private _remarks As String
        Private _activeFlag As String

        Public Property userid() As String
            Get
                Return _userID
            End Get

            Set(ByVal value As String)
                _userID = value
            End Set
        End Property

        Public Property username() As String
            Get
                Return _userName
            End Get

            Set(ByVal value As String)
                _userName = value
            End Set
        End Property

        Public Property fullname() As String
            Get
                Return _fullName
            End Get

            Set(ByVal value As String)
                _fullName = value
            End Set
        End Property

        Public Property title() As String
            Get
                Return _title
            End Get

            Set(ByVal value As String)
                _title = value
            End Set
        End Property

        Public Property comment() As String
            Get
                Return _comment
            End Get

            Set(ByVal value As String)
                _comment = value
            End Set
        End Property

        Public Property userpass() As String
            Get
                Return _userPass
            End Get

            Set(ByVal value As String)
                _userPass = value
            End Set
        End Property

        Public Property useractive() As Boolean
            Get
                Return _userActive
            End Get

            Set(ByVal value As Boolean)
                _userActive = value
            End Set
        End Property

        Public Property email() As String
            Get
                Return _email
            End Get

            Set(ByVal value As String)
                _email = value
            End Set
        End Property

        Public Property user_logincount() As Integer
            Get
                Return _user_logincount
            End Get

            Set(ByVal value As Integer)
                _user_logincount = value
            End Set
        End Property

        Public Property last_update() As String
            Get
                Return _last_update
            End Get

            Set(ByVal value As String)
                _last_update = value
            End Set
        End Property

        Public Property update_time() As DateTime
            Get
                Return _update_time
            End Get

            Set(ByVal value As DateTime)
                _update_time = value
            End Set
        End Property

        Public Property authority() As String
            Get
                Return _authority
            End Get

            Set(ByVal value As String)
                _authority = value
            End Set
        End Property

        Public Property remarks() As String
            Get
                Return _remarks
            End Get

            Set(ByVal value As String)
                _remarks = value
            End Set
        End Property

        Public Property activeflag() As String
            Get
                Return _activeFlag
            End Get

            Set(ByVal value As String)
                _activeFlag = value
            End Set
        End Property
    End Class
End Namespace

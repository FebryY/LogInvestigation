Option Strict On
Option Explicit On

Namespace PocoClass
    Public Class DeptMaster
        Private _divisionCode As String
        Private _actStartDate As DateTime
        Private _actEndDate As DateTime
        Private _divisionName As String

        Public Property DIVISIONCODE() As String
            Get
                Return _divisionCode
            End Get

            Set(ByVal value As String)
                _divisionCode = value
            End Set
        End Property

        Public Property ACTSTARTDATE() As DateTime
            Get
                Return _actStartDate
            End Get

            Set(ByVal value As DateTime)
                _actStartDate = value
            End Set
        End Property

        Public Property ACTENDDATE() As DateTime
            Get
                Return _actEndDate
            End Get

            Set(ByVal value As DateTime)
                _actEndDate = value
            End Set
        End Property

        Public Property DIVISIONNAME() As String
            Get
                Return _divisionName
            End Get

            Set(ByVal value As String)
                _divisionName = value
            End Set
        End Property
    End Class
End Namespace

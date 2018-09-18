Option Strict On
Option Explicit On

Namespace PocoClass
    Public Class CustomerMaster
        Private _customerCode As String
        Private _actStartDate As DateTime
        Private _actEndDate As DateTime
        Private _customerName As String
        Private _shortName As String

        Public Property CUSTOMERCODE() As String
            Get
                Return _customerCode
            End Get

            Set(ByVal value As String)
                _customerCode = value
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

        Public Property CUSTOMERNAME() As String
            Get
                Return _customerName
            End Get

            Set(ByVal value As String)
                _customerName = value
            End Set
        End Property

        Public Property SHORTNAME() As String
            Get
                Return _shortName
            End Get

            Set(ByVal value As String)
                _shortName = value
            End Set
        End Property
    End Class
End Namespace

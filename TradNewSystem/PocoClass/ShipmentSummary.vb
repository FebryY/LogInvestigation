Option Strict On
Option Explicit On

Namespace PocoClass
    Public Class ShipmentSummary
        Private _shipmentDate As DateTime
        Private _model As String
        Private _trinPartNo As String
        Private _shortName As String
        Private _customerCode As String
        Private _planQty As Integer
        Private _sumActQty As Decimal
        Private _plantNo As String
        Private _sid As Integer
        Private _soNumber As String
        Private _divisionCode As String

        Public Property SHIPMENTDATE() As DateTime
            Get
                Return _shipmentDate
            End Get

            Set(ByVal value As DateTime)
                _shipmentDate = value
            End Set
        End Property

        Public Property MODEL() As String
            Get
                Return _model
            End Get

            Set(ByVal value As String)
                _model = value
            End Set
        End Property

        Public Property TRINPARTNO() As String
            Get
                Return _trinPartNo
            End Get

            Set(ByVal value As String)
                _trinPartNo = value
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

        Public Property CUSTOMERCODE() As String
            Get
                Return _customerCode
            End Get

            Set(ByVal value As String)
                _customerCode = value
            End Set
        End Property

        Public Property PLANTNO() As String
            Get
                Return _plantNo
            End Get

            Set(ByVal value As String)
                _plantNo = value
            End Set
        End Property

        Public Property PLANQTY() As Integer
            Get
                Return _planQty
            End Get

            Set(ByVal value As Integer)
                _planQty = value
            End Set
        End Property

        Public Property SUMACTQTY() As Decimal
            Get
                Return _sumActQty
            End Get

            Set(ByVal value As Decimal)
                _sumActQty = value
            End Set
        End Property

        Public Property SID() As Integer
            Get
                Return _sid
            End Get

            Set(ByVal value As Integer)
                _sid = value
            End Set
        End Property

        Public Property SONUMBER() As String
            Get
                Return _soNumber
            End Get

            Set(ByVal value As String)
                _soNumber = value
            End Set
        End Property

        Public Property DIVISIONCODE() As String
            Get
                Return _divisionCode
            End Get

            Set(ByVal value As String)
                _divisionCode = value
            End Set
        End Property
    End Class
End Namespace

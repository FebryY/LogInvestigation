Option Strict On
Option Explicit On

Namespace PocoClass
    Public Class StocktakeSummary
        Private _model As String
        Private _trinPartNo As String
        Private _totalStockQty As Decimal
        Private _totalScannedQty As Decimal
        Private _divisionCode As String

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

        Public Property TOTALSTOCKQTY() As Decimal
            Get
                Return _totalStockQty
            End Get

            Set(ByVal value As Decimal)
                _totalStockQty = value
            End Set
        End Property

        Public Property TOTALSCANNEDQTY() As Decimal
            Get
                Return _totalScannedQty
            End Get

            Set(ByVal value As Decimal)
                _totalScannedQty = value
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

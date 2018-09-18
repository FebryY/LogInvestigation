Option Strict On
Option Explicit On

Namespace PocoClass
    Public Class StockCardSummary
        Private _barcodeTag As String
        Private _totalStockIn As Decimal
        Private _totalStockOut As Decimal

        Public Property BARCODETAG() As String
            Get
                Return _barcodeTag
            End Get

            Set(ByVal value As String)
                _barcodeTag = value
            End Set
        End Property

        Public Property TOTALSTOCKIN() As Decimal
            Get
                Return _totalStockIn
            End Get

            Set(ByVal value As Decimal)
                _totalStockIn = value
            End Set
        End Property

        Public Property TOTALSTOCKOUT() As Decimal
            Get
                Return _totalStockOut
            End Get

            Set(ByVal value As Decimal)
                _totalStockOut = value
            End Set
        End Property
    End Class
End Namespace

Option Strict On
Option Explicit On

Namespace PocoClass
    Public Class Stocktake
        Private _stocktakeID As Integer
        Private _trinPartNo As String
        Private _barcodeTag As String
        Private _currentStock As Integer
        Private _scannedQty As UInteger
        Private _remarks As String
        Private _stocktakePeriod As DateTime
        Private _takeDateTime As DateTime
        Private _addColFlg As Integer
        Private _delColFlg As Integer
        Private _approvedFlg As Integer
        Private _finishTake As Integer

        Public Property STOCKTAKEID() As Integer
            Get
                Return _stocktakeID
            End Get

            Set(ByVal value As Integer)
                _stocktakeID = value
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

        Public Property BARCODETAG() As String
            Get
                Return _barcodeTag
            End Get

            Set(ByVal value As String)
                _barcodeTag = value
            End Set
        End Property

        Public Property CURRENTSTOCK() As Integer
            Get
                Return _currentStock
            End Get

            Set(ByVal value As Integer)
                _currentStock = value
            End Set
        End Property

        Public Property SCANNEDQTY() As UInteger
            Get
                Return _scannedQty
            End Get

            Set(ByVal value As UInteger)
                _scannedQty = value
            End Set
        End Property

        Public Property REMARKS() As String
            Get
                Return _remarks
            End Get

            Set(ByVal value As String)
                _remarks = value
            End Set
        End Property

        Public Property STOCKTAKEPERIOD() As DateTime
            Get
                Return _stocktakePeriod
            End Get

            Set(ByVal value As DateTime)
                _stocktakePeriod = value
            End Set
        End Property

        Public Property TAKEDATETIME() As DateTime
            Get
                Return _takeDateTime
            End Get

            Set(ByVal value As DateTime)
                _takeDateTime = value
            End Set
        End Property

        Public Property ADDCOLFLG() As Integer
            Get
                Return _addColFlg
            End Get

            Set(ByVal value As Integer)
                _addColFlg = value
            End Set
        End Property

        Public Property DELCOLFLG() As Integer
            Get
                Return _delColFlg
            End Get

            Set(ByVal value As Integer)
                _delColFlg = value
            End Set
        End Property

        Public Property APPROVEDFLG() As Integer
            Get
                Return _approvedFlg
            End Get

            Set(ByVal value As Integer)
                _approvedFlg = value
            End Set
        End Property

        Public Property FINISHTAKE() As Integer
            Get
                Return _finishTake
            End Get

            Set(ByVal value As Integer)
                _finishTake = value
            End Set
        End Property
    End Class
End Namespace

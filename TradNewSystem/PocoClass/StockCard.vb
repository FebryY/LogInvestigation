Option Strict On
Option Explicit On

Namespace PocoClass
    Public Class StockCard
        Private _stockCardId As Int64
        Private _trinPartNo As String
        Private _actId As Integer
        Private _stock_in As Integer
        Private _stock_out As Integer
        Private _sum_stock_in As Decimal
        Private _sum_stock_out As Decimal
        Private _remark As String
        Private _type_id As Integer
        Private _barcodeTag As String
        Private _lineCode As String
        Private _date_time As DateTime
        Private _userId As String
        Private _delFlag As Integer

        Public Property STOCK_CARD_ID() As Int64
            Get
                Return _stockCardId
            End Get

            Set(ByVal value As Int64)
                _stockCardId = value
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

        Public Property ACTID() As Integer
            Get
                Return _actId
            End Get

            Set(ByVal value As Integer)
                _actId = value
            End Set
        End Property

        Public Property STOCK_IN() As Integer
            Get
                Return _stock_in
            End Get

            Set(ByVal value As Integer)
                _stock_in = value
            End Set
        End Property

        Public Property STOCK_OUT() As Integer
            Get
                Return _stock_out
            End Get

            Set(ByVal value As Integer)
                _stock_out = value
            End Set
        End Property

        Public Property SUM_STOCK_IN() As Decimal
            Get
                Return _sum_stock_in
            End Get

            Set(ByVal value As Decimal)
                _sum_stock_in = value
            End Set
        End Property

        Public Property SUM_STOCK_OUT() As Decimal
            Get
                Return _sum_stock_out
            End Get

            Set(ByVal value As Decimal)
                _sum_stock_out = value
            End Set
        End Property

        Public Property REMARK() As String
            Get
                Return _remark
            End Get

            Set(ByVal value As String)
                _remark = value
            End Set
        End Property

        Public Property TYPE_ID() As Integer
            Get
                Return _type_id
            End Get

            Set(ByVal value As Integer)
                _type_id = value
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

        Public Property LINECODE() As String
            Get
                Return _lineCode
            End Get

            Set(ByVal value As String)
                _lineCode = value
            End Set
        End Property

        Public Property date_time() As DateTime
            Get
                Return _date_time
            End Get

            Set(ByVal value As DateTime)
                _date_time = value
            End Set
        End Property

        Public Property userid() As String
            Get
                Return _userId
            End Get

            Set(ByVal value As String)
                _userId = value
            End Set
        End Property

        Public Property DELFLAG() As Integer
            Get
                Return _delFlag
            End Get

            Set(ByVal value As Integer)
                _delFlag = value
            End Set
        End Property
    End Class
End Namespace

Option Strict On
Option Explicit On

Namespace PocoClass
    Public Class ProductionActIntegration
        Private _actID As Int32
        Private _finalID As Int32
        Private _barcodeTag As String
        Private _barcodeLine As String
        Private _counting As String
        Private _prodDate As DateTime
        Private _trinPartNo As String
        Private _lineCode As String
        Private _okNg As Integer
        Private _actQty As Integer
        Private _printFlg As Integer
        Private _accPacSendFlg As Integer
        Private _userid As String
        Private _qrCode As String
        Private _imgfile As String
        Private _remark As String
        Private _remarks As String
        Private _delFlag As Integer

        Private _delFlag1 As Integer
        Private _delFlag2 As Integer

        Private _stock_In As Integer
        Private _date_Time As DateTime
        Private _businessday As Date
        Private _packageStandard As Int16

        Private _splitFlg As Int16

        Public Property ACTID() As Int32
            Get
                Return _actID
            End Get

            Set(ByVal value As Integer)
                _actID = value
            End Set
        End Property

        Public Property FINALID() As Int32
            Get
                Return _finalID
            End Get

            Set(ByVal value As Integer)
                _finalID = value
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

        Public Property BARCODELINE() As String
            Get
                Return _barcodeLine
            End Get

            Set(ByVal value As String)
                _barcodeLine = value
            End Set
        End Property

        Public Property COUNTING() As String
            Get
                Return _counting
            End Get

            Set(ByVal value As String)
                _counting = value
            End Set
        End Property

        Public Property PRODDATE() As DateTime
            Get
                Return _prodDate
            End Get

            Set(ByVal value As DateTime)
                _prodDate = value
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

        Public Property LINECODE() As String
            Get
                Return _lineCode
            End Get

            Set(ByVal value As String)
                _lineCode = value
            End Set
        End Property

        Public Property OKNG() As Integer
            Get
                Return _okNg
            End Get

            Set(ByVal value As Integer)
                _okNg = value
            End Set
        End Property

        Public Property ACTQTY() As Integer
            Get
                Return _actQty
            End Get

            Set(ByVal value As Integer)
                _actQty = value
            End Set
        End Property

        Public Property PRINTFLG() As Integer
            Get
                Return _printFlg
            End Get

            Set(ByVal value As Integer)
                _printFlg = value
            End Set
        End Property

        Public Property ACCPACSENDFLG() As Integer
            Get
                Return _accPacSendFlg
            End Get

            Set(ByVal value As Integer)
                _accPacSendFlg = value
            End Set
        End Property

        Public Property USERID() As String
            Get
                Return _userid
            End Get

            Set(ByVal value As String)
                _userid = value
            End Set
        End Property

        Public Property QRCODE() As String
            Get
                Return _qrCode
            End Get

            Set(ByVal value As String)
                _qrCode = value
            End Set
        End Property

        Public Property IMGFILE() As String
            Get
                Return _imgfile
            End Get

            Set(ByVal value As String)
                _imgfile = value
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

        Public Property REMARKS() As String
            Get
                Return _remarks
            End Get

            Set(ByVal value As String)
                _remarks = value
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

        Public Property DELFLAG1() As Integer
            Get
                Return _delFlag1
            End Get

            Set(ByVal value As Integer)
                _delFlag1 = value
            End Set
        End Property

        Public Property DELFLAG2() As Integer
            Get
                Return _delFlag2
            End Get

            Set(ByVal value As Integer)
                _delFlag2 = value
            End Set
        End Property

        Public Property STOCK_IN() As Integer
            Get
                Return _stock_In
            End Get

            Set(ByVal value As Integer)
                _stock_In = value
            End Set
        End Property

        Public Property DATE_TIME() As DateTime
            Get
                Return _date_Time
            End Get

            Set(ByVal value As DateTime)
                _date_Time = value
            End Set
        End Property

        Public Property BUSINESSDAY() As Date
            Get
                Return _businessday
            End Get

            Set(ByVal value As Date)
                _businessday = value
            End Set
        End Property

        Public Property PACKAGESTANDARD() As Int16
            Get
                Return _packageStandard
            End Get

            Set(ByVal value As Int16)
                _packageStandard = value
            End Set
        End Property

        Public Property SPLITFLG() As Int16
            Get
                Return _splitFlg
            End Get

            Set(ByVal value As Int16)
                _splitFlg = value
            End Set
        End Property

    End Class
End Namespace
Option Strict On
Option Explicit On

Namespace PocoClass
    Public Class ProductionPlanMod
        Private _finalId As Integer
        Private _lineCode As String
        Private _prodDate As DateTime
        Private _endTime As DateTime
        Private _trinPartNo As String
        Private _period As String
        Private _planQty As Int32
        Private _importDate As DateTime
        Private _delFlag As Int16
        Private _trinModFlag As Int16

        Private _delFlag1 As Int16
        Private _delFlag2 As Int16

        Private _actID As Int32
        Private _barcodeTag As String
        Private _barcodeLine As String
        Private _counting As String
        Private _okNg As Integer
        Private _actQty As Integer
        Private _printFlg As Integer
        Private _accPacSendFlg As Int16
        Private _userid As String
        Private _qrCode As String
        Private _imgfile As String
        Private _remarks As String
        Private _modBarcode As String
        Private _oldProdDate As DateTime

        Private _stock_In As Integer
        Private _date_Time As DateTime
        Private _businessDay As Date

        Public Property FINALID() As Integer
            Get
                Return _finalId
            End Get

            Set(ByVal value As Integer)
                _finalId = value
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

        Public Property PRODDATE() As DateTime
            Get
                Return _prodDate
            End Get

            Set(ByVal value As DateTime)
                _prodDate = value
            End Set
        End Property

        Public Property ENDTIME() As DateTime
            Get
                Return _endTime
            End Get

            Set(ByVal value As DateTime)
                _endTime = value
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

        Public Property PERIOD() As String
            Get
                Return _period
            End Get

            Set(ByVal value As String)
                _period = value
            End Set
        End Property

        Public Property PLANQTY() As Int32
            Get
                Return _planQty
            End Get

            Set(ByVal value As Int32)
                _planQty = value
            End Set
        End Property

        Public Property IMPORTDATE() As DateTime
            Get
                Return _importDate
            End Get

            Set(ByVal value As DateTime)
                _importDate = value
            End Set
        End Property

        Public Property DELFLAG() As Int16
            Get
                Return _delFlag
            End Get

            Set(ByVal value As Int16)
                _delFlag = value
            End Set
        End Property

        Public Property TRINMODFLAG() As Int16
            Get
                Return _trinModFlag
            End Get

            Set(ByVal value As Int16)
                _trinModFlag = value
            End Set
        End Property

        Public Property ACTID() As Int32
            Get
                Return _actID
            End Get

            Set(ByVal value As Int32)
                _actID = value
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

        Public Property ACCPACSENDFLG() As Int16
            Get
                Return _accPacSendFlg
            End Get

            Set(ByVal value As Int16)
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

        Public Property REMARKS() As String
            Get
                Return _remarks
            End Get

            Set(ByVal value As String)
                _remarks = value
            End Set
        End Property

        Public Property MODBARCODE() As String
            Get
                Return _modBarcode
            End Get

            Set(ByVal value As String)
                _modBarcode = value
            End Set
        End Property

        Public Property OLDPRODDATE() As DateTime
            Get
                Return _oldProdDate
            End Get

            Set(ByVal value As DateTime)
                _oldProdDate = value
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
                Return _businessDay
            End Get

            Set(ByVal value As Date)
                _businessDay = value
            End Set
        End Property

        Public Property DELFLAG1() As Int16
            Get
                Return _delFlag1
            End Get

            Set(ByVal value As Int16)
                _delFlag1 = value
            End Set
        End Property

        Public Property DELFLAG2() As Int16
            Get
                Return _delFlag2
            End Get

            Set(ByVal value As Int16)
                _delFlag2 = value
            End Set
        End Property

    End Class
End Namespace

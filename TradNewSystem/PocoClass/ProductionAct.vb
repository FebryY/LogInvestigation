Option Strict On
Option Explicit On

Namespace PocoClass
    Public Class ProductionAct
        Private _actID As Integer
        Private _finalID As Integer
        Private _barcodeTag As String
        Private _prodDate As DateTime
        Private _trinPartNo As String
        Private _lineCode As String
        Private _okNg As Integer
        Private _actQty As Integer
        Private _printFlg As Integer
        Private _printDate As DateTime
        Private _accPacSendFlg As Integer
        Private _accPacSendDate As DateTime
        Private _labelQrCode As String
        Private _labelCustomerName As String
        Private _labelPartNo As String
        Private _labelTrinPartNo As String
        Private _labelPartName As String
        Private _labelModel As String
        Private _labelActQty As Integer
        Private _labelStandard As Integer
        Private _userid As String
        Private _qrCode As String
        Private _imgfile As String
        Private _remarks As String
        Private _delFlag As Integer

        Public Property ACTID() As Integer
            Get
                Return _actID
            End Get

            Set(ByVal value As Integer)
                _actID = value
            End Set
        End Property

        Public Property FINALID() As Integer
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

        Public Property PRINTDATE() As DateTime
            Get
                Return _printDate
            End Get

            Set(ByVal value As DateTime)
                _printDate = value
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

        Public Property ACCPACSENDDATE() As DateTime
            Get
                Return _accPacSendDate
            End Get

            Set(ByVal value As DateTime)
                _accPacSendDate = value
            End Set
        End Property

        Public Property LABELQRCODE() As String
            Get
                Return _labelQrCode
            End Get

            Set(ByVal value As String)
                _labelQrCode = value
            End Set
        End Property

        Public Property LABELCUSTOMERNAME() As String
            Get
                Return _labelCustomerName
            End Get

            Set(ByVal value As String)
                _labelCustomerName = value
            End Set
        End Property

        Public Property LABELPARTNO() As String
            Get
                Return _labelPartNo
            End Get

            Set(ByVal value As String)
                _labelPartNo = value
            End Set
        End Property

        Public Property LABELTRINPARTNO() As String
            Get
                Return _labelTrinPartNo
            End Get

            Set(ByVal value As String)
                _labelTrinPartNo = value
            End Set
        End Property

        Public Property LABELPARTNAME() As String
            Get
                Return _labelPartName
            End Get

            Set(ByVal value As String)
                _labelPartName = value
            End Set
        End Property

        Public Property LABELMODEL() As String
            Get
                Return _labelModel
            End Get

            Set(ByVal value As String)
                _labelModel = value
            End Set
        End Property

        Public Property LABELACTQTY() As Integer
            Get
                Return _labelActQty
            End Get

            Set(ByVal value As Integer)
                _labelActQty = value
            End Set
        End Property

        Public Property LABELSTANDARD() As Integer
            Get
                Return _labelStandard
            End Get

            Set(ByVal value As Integer)
                _labelStandard = value
            End Set
        End Property

        Public Property userid() As String
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

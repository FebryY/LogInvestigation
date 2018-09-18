Option Strict On
Option Explicit On

Namespace PocoClass
    Public Class ShipmentAct
        Private _actId As Integer
        Private _sid As Integer
        Private _soNumber As String
        Private _customerCode As String
        Private _plantNo As String
        Private _shipmentDate As DateTime
        Private _userID As String
        Private _barcodeTag As String
        Private _prodDate As String
        Private _trinPartNo As String
        Private _okNG As Integer
        Private _actQty As Integer
        Private _printFlg As Integer
        Private _printDate As DateTime
        Private _accPacSendFlg As Integer
        Private _accPacSendDate As DateTime
        Private _labelQrCode As String
        Private _labelCustomerName As String
        Private _labelPartNo As String
        Private _labeltrinPartNo As String
        Private _labelPartName As String
        Private _labelModel As String
        Private _labelActQty As Integer
        Private _labelStandard As Integer
        Private _delFlag As Integer

        Public Property ACTID() As Integer
            Get
                Return _actId
            End Get

            Set(ByVal value As Integer)
                _actId = value
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

        Public Property SHIPMENTDATE() As DateTime
            Get
                Return _shipmentDate
            End Get

            Set(ByVal value As DateTime)
                _shipmentDate = value
            End Set
        End Property

        Public Property USERID() As String
            Get
                Return _userID
            End Get

            Set(ByVal value As String)
                _userID = value
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

        Public Property PRODDATE() As String
            Get
                Return _prodDate
            End Get

            Set(ByVal value As String)
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

        Public Property OKNG() As Integer
            Get
                Return _okNG
            End Get

            Set(ByVal value As Integer)
                _okNG = value
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
                Return _labeltrinPartNo
            End Get

            Set(ByVal value As String)
                _labeltrinPartNo = value
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
Option Strict On
Option Explicit On

Namespace PocoClass
    Public Class SqliteController
        Private _barcodeTag As String
        Private _qrCode As String
        Private _trinPartNo As String
        Private _inputFlag As String
        Private _scanQty As String
        Private _actQty As String
        Private _sid As String
        Private _soNumber As String
        Private _custCode As String
        Private _plantNo As String

        Public Property BARCODETAG() As String
            Get
                Return _barcodeTag
            End Get

            Set(ByVal value As String)
                _barcodeTag = value
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

        Public Property TRINPARTNO() As String
            Get
                Return _trinPartNo
            End Get

            Set(ByVal value As String)
                _trinPartNo = value
            End Set
        End Property

        Public Property INPUTFLAG() As String
            Get
                Return _inputFlag
            End Get
            Set(ByVal value As String)
                _inputFlag = value
            End Set
        End Property

        Public Property SCANQTY() As String
            Get
                Return _scanQty
            End Get
            Set(ByVal value As String)
                _scanQty = value
            End Set
        End Property

        Public Property ACTQTY() As String
            Get
                Return _actQty
            End Get
            Set(ByVal value As String)
                _actQty = value
            End Set
        End Property

        Public Property SID() As String
            Get
                Return _sid
            End Get
            Set(ByVal value As String)
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

        Public Property CUSTCODE() As String
            Get
                Return _custCode
            End Get
            Set(ByVal value As String)
                _custCode = value
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

    End Class
End Namespace
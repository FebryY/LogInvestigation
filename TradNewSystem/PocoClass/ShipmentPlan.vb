Option Strict On
Option Explicit On

Namespace PocoClass
    Public Class ShipmentPlan
        Private _sID As Integer
        Private _soNumber As String
        Private _trinPartNo As String
        Private _customerCode As String
        Private _plantNo As String
        Private _shipmentDate As DateTime
        Private _planQty As Integer
        Private _notAssy As Integer
        Private _delFlag As Integer
        Private _stockTakeFlag As Int16

        Public Property SID() As Integer
            Get
                Return _sID
            End Get

            Set(ByVal value As Integer)
                _sID = value
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

        Public Property TRINPARTNO() As String
            Get
                Return _trinPartNo
            End Get

            Set(ByVal value As String)
                _trinPartNo = value
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

        Public Property PLANQTY() As Integer
            Get
                Return _planQty
            End Get

            Set(ByVal value As Integer)
                _planQty = value
            End Set
        End Property

        Public Property NOTASSY() As Integer
            Get
                Return _notAssy
            End Get

            Set(ByVal value As Integer)
                _notAssy = value
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

        Public Property STOCKTAKEFLAG() As Int16
            Get
                Return _stockTakeFlag
            End Get

            Set(ByVal value As Int16)
                _stockTakeFlag = value
            End Set
        End Property
    End Class
End Namespace

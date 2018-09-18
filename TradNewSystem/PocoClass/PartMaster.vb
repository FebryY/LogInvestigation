Option Strict On
Option Explicit On

Namespace PocoClass
    Public Class PartMaster
        Private _trinPartNo As String
        Private _actStartDate As DateTime
        Private _actEndDate As DateTime
        Private _partNo As String
        Private _partName As String
        Private _customerCode As String
        Private _divisionCode As String
        Private _model As String
        Private _minStock As Integer
        Private _maxStock As Integer
        Private _packageStandard1 As Integer
        Private _packageStandard2 As Integer
        Private _unit As String
        Private _taktTime As Integer
        Private _remarks As String

        Public Property TRINPARTNO() As String
            Get
                Return _trinPartNo
            End Get

            Set(ByVal value As String)
                _trinPartNo = value
            End Set
        End Property

        Public Property ACTSTARTDATE() As DateTime
            Get
                Return _actStartDate
            End Get

            Set(ByVal value As DateTime)
                _actStartDate = value
            End Set
        End Property

        Public Property ACTENDDATE() As DateTime
            Get
                Return _actEndDate
            End Get

            Set(ByVal value As DateTime)
                _actEndDate = value
            End Set
        End Property

        Public Property PARTNO() As String
            Get
                Return _partNo
            End Get

            Set(ByVal value As String)
                _partNo = value
            End Set
        End Property

        Public Property PARTNAME() As String
            Get
                Return _partName
            End Get

            Set(ByVal value As String)
                _partName = value
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

        Public Property DIVISIONCODE() As String
            Get
                Return _divisionCode
            End Get

            Set(ByVal value As String)
                _divisionCode = value
            End Set
        End Property

        Public Property MODEL() As String
            Get
                Return _model
            End Get

            Set(ByVal value As String)
                _model = value
            End Set
        End Property

        Public Property MINSTOCK() As Integer
            Get
                Return _minStock
            End Get

            Set(ByVal value As Integer)
                _minStock = value
            End Set
        End Property

        Public Property MAXSTOCK() As Integer
            Get
                Return _maxStock
            End Get

            Set(ByVal value As Integer)
                _maxStock = value
            End Set
        End Property

        Public Property PACKAGESTANDARD1() As Integer
            Get
                Return _packageStandard1
            End Get

            Set(ByVal value As Integer)
                _packageStandard1 = value
            End Set
        End Property

        Public Property PACKAGESTANDARD2() As Integer
            Get
                Return _packageStandard2
            End Get

            Set(ByVal value As Integer)
                _packageStandard2 = value
            End Set
        End Property

        Public Property UNIT() As String
            Get
                Return _unit
            End Get

            Set(ByVal value As String)
                _unit = value
            End Set
        End Property

        Public Property TAKTTIME() As Integer
            Get
                Return _taktTime
            End Get

            Set(ByVal value As Integer)
                _taktTime = value
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
    End Class
End Namespace
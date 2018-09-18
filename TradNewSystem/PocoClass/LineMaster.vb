Namespace PocoClass
    Public Class LineMaster
        Private _lineCode As String
        Private _actStartDate As DateTime
        Private _actEndDate As DateTime
        Private _lineName As String
        Private _barcodeLine As String
        Private _ipAddress As String
        Private _printerName As String

        Public Property LINECODE() As String
            Get
                Return _lineCode
            End Get

            Set(ByVal value As String)
                _lineCode = value
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

        Public Property LINENAME() As String
            Get
                Return _lineName
            End Get

            Set(ByVal value As String)
                _lineName = value
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

        Public Property IPADDRESS() As String
            Get
                Return _ipAddress
            End Get

            Set(ByVal value As String)
                _ipAddress = value
            End Set
        End Property

        Public Property PRINTERNAME() As String
            Get
                Return _printerName
            End Get

            Set(ByVal value As String)
                _printerName = value
            End Set
        End Property

    End Class
End Namespace

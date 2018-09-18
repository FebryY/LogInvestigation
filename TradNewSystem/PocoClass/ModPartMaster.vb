Namespace PocoClass
    Public Class ModPartMaster
        Private _trinPartNo As String
        Private _actStartDate As DateTime
        Private _actEndDate As DateTime
        Private _newPartNo As String

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

        Public Property NEWPARTNO() As String
            Get
                Return _newPartNo
            End Get

            Set(ByVal value As String)
                _newPartNo = value
            End Set
        End Property

    End Class
End Namespace

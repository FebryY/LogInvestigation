Imports System.Data
Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass

Namespace Model
    Module PartMasterDB
        Public Function fncCheckTRIMPartNoMaster(ByVal str_TRIN As String) As QueryRetValue
            Dim partMaster As PartMaster = Nothing

            Dim ret_TRIN As QueryRetValue = QueryRetValue.ValueFalse

            Try
                Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                    connection.Open()

                    Dim sqlString As String = "SELECT TRINPARTNO FROM PARTMASTER WHERE TRINPARTNO=@TRINPARTNO"

                    partMaster = connection.Query(Of PartMaster)(sqlString, New With {Key .TRINPARTNO = str_TRIN}).FirstOrDefault
                End Using
            Catch ex As Exception
                ret_TRIN = QueryRetValue.ValueError
                DisplayMessage.ErrorMsg(ex.Message, "DB Error")
            End Try

            If Not partMaster Is Nothing Then
                ret_TRIN = QueryRetValue.ValueTrue
            Else
                ret_TRIN = QueryRetValue.ValueFalse
            End If

            Return ret_TRIN
        End Function
    End Module
End Namespace

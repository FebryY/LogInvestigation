Imports System.Data
Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass

Namespace Model
    Module LineMasterDB

        Public Function fncGetLineCodes(ByVal str_Code As String) As String
            Dim lineMaster As List(Of LineMaster) = Nothing

            Dim str_LineCode As String = String.Empty

            Try
                Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                    connection.Open()

                    Dim sqlString As String = "SELECT LINECODE FROM LINEMASTER WHERE BARCODELINE = @BARCODELINE"

                    lineMaster = CType(connection.Query(Of LineMaster)(sqlString, New With {Key .BARCODELINE = str_Code}), List(Of LineMaster))
                End Using
            Catch ex As Exception
                DisplayMessage.ErrorMsg(ex.Message, "DB Error")
            End Try

            If Not lineMaster Is Nothing Then
                For Each lineMasterItem As LineMaster In lineMaster
                    str_LineCode = lineMasterItem.LINECODE
                Next
            End If

            Return str_LineCode
        End Function

    End Module
End Namespace

Imports System.Data
Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass

Namespace Model
    Module ModPartMasterDB

        Public Function fncGetTRINPartNo(ByVal str_OldTRIN As String) As String
            Dim modPartMaster As List(Of ModPartMaster) = Nothing

            Dim str_OldTRINPartNo As String = String.Empty

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
            Try
                connection.Open()

                Dim sqlString As String = "SELECT TRINPARTNO FROM MODPARTMASTER WHERE TRINPARTNO = @TRINPARTNO"

                modPartMaster = CType(connection.Query(Of ModPartMaster)(sqlString, New With {Key .TRINPARTNO = str_OldTRIN}), List(Of ModPartMaster))
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not modPartMaster Is Nothing Then
                For Each productionActItem As ModPartMaster In modPartMaster
                    str_OldTRINPartNo = productionActItem.TRINPARTNO
                Next
            End If

            Return str_OldTRINPartNo
        End Function

        Public Function fncGetNewTRIMPartNo(ByVal str_NewTRIN As String) As List(Of ModPartMaster)
            Dim newTRINNo As List(Of ModPartMaster) = Nothing

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
            Try
                connection.Open()

                Dim sqlString As String = "SELECT TRINPARTNO,NEWPARTNO FROM MODPARTMASTER WHERE TRINPARTNO = @TRINPARTNO"

                newTRINNo = CType(connection.Query(Of ModPartMaster)(sqlString, New With {Key .TRINPARTNO = str_NewTRIN}), List(Of ModPartMaster))
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return newTRINNo
        End Function

    End Module
End Namespace

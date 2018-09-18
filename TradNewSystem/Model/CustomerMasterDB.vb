Option Strict On
Option Explicit On

Imports System.Data

Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass


Namespace Model
    Module CustomerMasterDB
        Public Function GetAllCustomers() As List(Of CustomerMaster)
            Dim customers As List(Of CustomerMaster) = Nothing

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    connection.Open()

                    Dim sqlString As String = ( _
                        "SELECT * FROM CUSTOMERMASTER ORDER BY SHORTNAME" _
                        )

                    customers = CType( _
                        connection.Query(Of CustomerMaster)(sqlString),  _
                        List(Of CustomerMaster) _
                        )
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return customers
        End Function
    End Module
End Namespace
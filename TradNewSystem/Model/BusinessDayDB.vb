Option Strict On
Option Explicit On

Imports System.Data

Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass


Namespace Model
    Module BusinessDayDB
        Public Function GetBusinessHour() As TimeSpan
            Dim businessHour As TimeSpan

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    connection.Open()

                    Dim sqlString As String = ( _
                        "SELECT BUSINESSHOUR FROM BUSINESSDAY" _
                        )

                    businessHour = connection.Query(Of TimeSpan) _
                        (sqlString).DefaultIfEmpty(New TimeSpan(0, 0, 0)). _
                        FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return businessHour
        End Function
    End Module
End Namespace
Option Strict On
Option Explicit On

Imports System.Data

Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass
Imports log4net


Namespace Model
    Module BusinessDayDB
        Public Function GetBusinessHour() As TimeSpan
            Dim businessHour As TimeSpan
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    log.Info("GetBusinessHour, Open connection")

                    connection.Open()

                    log.Info("GetBusinessHour, Open connection success")

                    Dim sqlString As String = ( _
                        "SELECT BUSINESSHOUR FROM BUSINESSDAY" _
                        )

                    log.Info("GetBusinessHour SQL string: " & sqlString)

                    businessHour = connection.Query(Of TimeSpan) _
                        (sqlString).DefaultIfEmpty(New TimeSpan(0, 0, 0)). _
                        FirstOrDefault

                    log.Info("GetBusinessHour can get result")

                Catch ex As Exception
                    log.Error("GetBusinessHour DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            LogManager.Shutdown()
            Return businessHour
        End Function
    End Module
End Namespace
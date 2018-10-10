Option Strict On
Option Explicit On

Imports System.Data

Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass

Imports log4net


Namespace Model
    Module UserMasterDB
        Public Function IsUserExist( _
            ByVal userID As String, _
            ByVal hashedPassword As String _
            ) As QueryRetValue

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim retValue As QueryRetValue = QueryRetValue.ValueFalse
            Dim user As UserMaster = Nothing

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    log.Info("IsUserExist, Open Connection")

                    connection.Open()

                    log.Info("IsUserExist, Open Connection success")

                    Dim sqlString As String = _
                        "SELECT * " & _
                        "FROM USER_MASTER " & _
                        "WHERE USERID = @USERID " & _
                            "AND USERPASS = @USERPASS " & _
                            "AND USERACTIVE = 1"

                    log.Info("IsUserExist SQL string: " & sqlString)

                    Dim parameter As Object = New With { _
                        Key .userid = userID, .userpass = hashedPassword _
                        }
                    user = connection.Query(Of UserMaster) _
                        (sqlString, parameter).FirstOrDefault

                   
                Catch ex As Exception
                    retValue = QueryRetValue.ValueError
                    log.Error("IsUserExist DB Error ", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try

            End Using

            If Not user Is Nothing Then
                retValue = QueryRetValue.ValueTrue
            End If

            log.Info("IsUserExist can get result " & retValue)

            LogManager.Shutdown()

            Return retValue
        End Function

        Public Function IsDBDateSamewithHT() As Boolean

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim result As Boolean = False

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    log.Info("IsDBDateSamewithHT, Open Connection")

                    connection.Open()

                    log.Info("IsDBDateSamewithHT, Open Connection success")

                    Dim sqlString As String = "SELECT NOW()"

                    log.Info("IsDBDateSamewithHT SQL string: " & sqlString)

                    Dim dbDate = CDate(connection.Query(Of Date) _
                            (sqlString).FirstOrDefault)

                    If Date.Now.Date = dbDate.Date Then
                        result = True
                    End If

                Catch ex As Exception

                    log.Error("IsDBDateSamewithHT DB Error ", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try

            End Using


            log.Info("IsDBDateSamewithHT can get result " & result)

            LogManager.Shutdown()

            Return result
        End Function
    End Module
End Namespace
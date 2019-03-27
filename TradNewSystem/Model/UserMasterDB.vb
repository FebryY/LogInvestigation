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

            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim retValue As QueryRetValue = QueryRetValue.ValueFalse
            Dim user As UserMaster = Nothing

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    'log.Info("IsUserExist, Open Connection")

                    connection.Open()

                    'log.Info("IsUserExist, Open Connection success")

                    Dim sqlString As String = _
                        "SELECT * " & _
                        "FROM USER_MASTER " & _
                        "WHERE USERID = @USERID " & _
                            "AND USERPASS = @USERPASS " & _
                            "AND USERACTIVE = 1"

                    'log.Info("IsUserExist SQL string: " & sqlString)

                    Dim parameter As Object = New With { _
                        Key .userid = userID, .userpass = hashedPassword _
                        }
                    user = connection.Query(Of UserMaster) _
                        (sqlString, parameter).FirstOrDefault

                   
                Catch ex As Exception
                    retValue = QueryRetValue.ValueError
                    'log.Error("IsUserExist DB Error ", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try

            End Using

            If Not user Is Nothing Then
                retValue = QueryRetValue.ValueTrue
            End If

            'log.Info("IsUserExist can get result " & retValue)

            'LogManager.Shutdown()

            Return retValue
        End Function

        Public Function ChangeHTDateWithDBDate() As QueryRetValue

            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim retValue As QueryRetValue = QueryRetValue.ValueFalse
            Dim dbDate As Date?

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    'log.Info("IsDBDateSamewithHT, Open Connection")

                    connection.Open()

                    'log.Info("IsDBDateSamewithHT, Open Connection success")

                    Dim sqlString As String = "SELECT NOW()"

                    'log.Info("IsDBDateSamewithHT SQL string: " & sqlString)

                    dbDate = CDate(connection.Query(Of Date) _
                            (sqlString).FirstOrDefault)

                Catch ex As Exception
                    retValue = QueryRetValue.ValueError
                    'log.Error("IsDBDateSamewithHT DB Error ", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try

            End Using

            'log.Info("IsDBDateSamewithHT can get result " & retValue)

            'LogManager.Shutdown()

            If Not dbDate Is Nothing Then
                If Date.Now.Date = dbDate.Value.Date Then
                    retValue = QueryRetValue.ValueTrue
                Else
                    retValue = QueryRetValue.ValueTrue
                    Dim d As DateTime
                    d = CDate("" & dbDate.Value.Hour & ":" & dbDate.Value.Minute & ":" & dbDate.Value.Second & "")

                    Try
                        Microsoft.VisualBasic.TimeOfDay = d
                        Microsoft.VisualBasic.DateString = dbDate.Value.Month & "/" & dbDate.Value.Day & "/" & dbDate.Value.Year
                    Catch ex As Exception
                        retValue = QueryRetValue.ValueError
                        DisplayMessage.ErrorMsg(ex.Message, "Change Date Error")
                    End Try
                End If
            End If

            Return retValue
        End Function
    End Module
End Namespace
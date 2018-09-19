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

                    log.Info("IsUserExist result " & user.ToString())

                Catch ex As Exception
                    retValue = QueryRetValue.ValueError
                    log.Error("IsUserExist DB Error ", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try

            End Using

            If Not user Is Nothing Then
                retValue = QueryRetValue.ValueTrue
            End If

            Return retValue
        End Function
    End Module
End Namespace
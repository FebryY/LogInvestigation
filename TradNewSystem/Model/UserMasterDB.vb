Option Strict On
Option Explicit On

Imports System.Data

Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass


Namespace Model
    Module UserMasterDB
        Public Function IsUserExist( _
            ByVal userID As String, _
            ByVal hashedPassword As String _
            ) As QueryRetValue
            Dim retValue As QueryRetValue = QueryRetValue.ValueFalse
            Dim user As UserMaster = Nothing

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    connection.Open()

                    Dim sqlString As String = _
                        "SELECT * " & _
                        "FROM USER_MASTER " & _
                        "WHERE USERID = @USERID " & _
                            "AND USERPASS = @USERPASS " & _
                            "AND USERACTIVE = 1"

                    Dim parameter As Object = New With { _
                        Key .userid = userID, .userpass = hashedPassword _
                        }
                    user = connection.Query(Of UserMaster) _
                        (sqlString, parameter).FirstOrDefault
                Catch ex As Exception
                    retValue = QueryRetValue.ValueError

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
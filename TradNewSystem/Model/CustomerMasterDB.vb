Option Strict On
Option Explicit On

Imports System.Data

Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass
Imports log4net


Namespace Model
    Module CustomerMasterDB
        Public Function GetAllCustomers() As List(Of CustomerMaster)
            Dim customers As List(Of CustomerMaster) = Nothing
            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    'log.Info("GetAllCustomers, Open connection")

                    connection.Open()

                    'log.Info("GetAllCustomers, Open connection success")

                    Dim sqlString As String = ( _
"SELECT CUSTOMERCODE,SHORTNAME FROM CUSTOMERMASTER ORDER BY SHORTNAME" _
                        )

                    'log.Info("GetAllCustomers SQL string: " & sqlString)

                    customers = CType( _
                        connection.Query(Of CustomerMaster)(sqlString),  _
                        List(Of CustomerMaster) _
                        )

                    'log.Info("GetAllCustomers result " & customers.Count())

                    connection.Close() 'Febry 5 December 2018
                Catch ex As Exception
                    'log.Error("GetAllCustomers DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                    connection.Close()
                End Try
            End Using

            'LogManager.Shutdown()
            Return customers
        End Function
    End Module
End Namespace
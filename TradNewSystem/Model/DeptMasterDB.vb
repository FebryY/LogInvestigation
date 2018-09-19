Option Strict On
Option Explicit On

Imports System.Data

Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass
Imports log4net


Namespace Model
    Module DeptMasterDB
        Public Function GetAllDivisions() As List(Of DeptMaster)
            Dim deptMasters As List(Of DeptMaster) = Nothing
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Try
                Using connection As IDbConnection = New MySqlConnection( _
                    CommonLib.GenerateConnectionString _
                    )
                    log.Info("GetAllDivisions, Open connection")

                    connection.Open()

                    log.Info("GetAllDivisions, Open connection success")

                    Dim sqlString As String = ( _
                        "SELECT * FROM DEPTMASTER ORDER BY DIVISIONCODE" _
                        )

                    log.Info("GetAllDivisions SQL string: " & sqlString)

                    deptMasters = CType( _
                        connection.Query(Of DeptMaster)(sqlString),  _
                        List(Of DeptMaster) _
                        )

                    log.Info("GetAllDivisions result " & deptMasters.Count())

                End Using
            Catch ex As Exception
                log.Error("GetAllDivisions DB Error", ex)
                DisplayMessage.ErrorMsg(ex.Message, "DB Error")
            End Try

            LogManager.Shutdown()
            Return deptMasters
        End Function
    End Module
End Namespace

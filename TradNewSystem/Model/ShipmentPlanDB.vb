Option Strict On
Option Explicit On

Imports System.Data

Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass

Imports log4net


Namespace Model
    Module ShipmentPlanDB
        Public Function GetShipmentPlans() As List(Of ShipmentPlan)
            Dim shipments As List(Of ShipmentPlan) = Nothing

            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    'log.Info("GetShipmentPlans, Open Connection")

                    connection.Open()

                    'log.Info("GetShipmentPlans, Open Connection success")

                    Dim sqlString As String = ( _
                        "SELECT * FROM SHIPMENTPLAN WHERE DELFLAG=0" _
                        )

                    'log.Info("GetShipmentPlans SQL string: " & sqlString)

                    shipments = CType( _
                        connection.Query(Of ShipmentPlan)(sqlString),  _
                        List(Of ShipmentPlan) _
                        )

                    'log.Info("GetShipmentPlans result " & shipments.Count())

                Catch ex As Exception

                    'log.Error("GetShipmentPlans DB Error ", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            'LogManager.Shutdown()

            Return shipments
        End Function

        Public Function fncCheckStockTakeFlag(ByVal str_SID As String) As Int32
            Dim shipPlan As ShipmentPlan = Nothing
            Dim int_Flag As Int32 = 0

            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    'log.Info("fncCheckStockTakeFlag, Open Connection")

                    connection.Open()

                    'log.Info("fncCheckStockTakeFlag, Open Connection success")

                    Dim sqlString As String = ( _
                        "SELECT STOCKTAKEFLAG " & _
                        "FROM SHIPMENTPLAN " & _
                        "WHERE SID = @SID " & _
                            "AND DELFLAG=0 " & _
                            "AND STOCKTAKEFLAG=1" _
                        )

                    'log.Info("fncCheckStockTakeFlag SQL string: " & sqlString)

                    shipPlan = connection.Query(Of ShipmentPlan)(sqlString, New With {Key .SID = str_SID}).FirstOrDefault

                Catch ex As Exception

                    'log.Error("fncCheckStockTakeFlag DB Error ", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not shipPlan Is Nothing Then
                int_Flag = shipPlan.STOCKTAKEFLAG
            End If

            'log.Info("fncCheckStockTakeFlag result " & int_Flag)

            'LogManager.Shutdown()

            Return int_Flag
        End Function
    End Module
End Namespace
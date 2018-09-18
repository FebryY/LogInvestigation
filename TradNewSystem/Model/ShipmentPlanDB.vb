Option Strict On
Option Explicit On

Imports System.Data

Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass


Namespace Model
    Module ShipmentPlanDB
        Public Function GetShipmentPlans() As List(Of ShipmentPlan)
            Dim shipments As List(Of ShipmentPlan) = Nothing

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    connection.Open()

                    Dim sqlString As String = ( _
                        "SELECT * FROM SHIPMENTPLAN WHERE DELFLAG=0" _
                        )

                    shipments = CType( _
                        connection.Query(Of ShipmentPlan)(sqlString),  _
                        List(Of ShipmentPlan) _
                        )
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return shipments
        End Function

        Public Function fncCheckStockTakeFlag(ByVal str_SID As String) As Int32
            Dim shipPlan As ShipmentPlan = Nothing
            Dim int_Flag As Int32 = 0

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    connection.Open()

                    Dim sqlString As String = ( _
                        "SELECT STOCKTAKEFLAG " & _
                        "FROM SHIPMENTPLAN " & _
                        "WHERE SID = @SID " & _
                            "AND DELFLAG=0 " & _
                            "AND STOCKTAKEFLAG=1" _
                        )

                    shipPlan = connection.Query(Of ShipmentPlan)(sqlString, New With {Key .SID = str_SID}).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not shipPlan Is Nothing Then
                int_Flag = shipPlan.STOCKTAKEFLAG
            End If

            Return int_Flag
        End Function
    End Module
End Namespace
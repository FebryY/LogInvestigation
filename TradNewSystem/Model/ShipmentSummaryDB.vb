Option Strict On
Option Explicit On

Imports System.Data
Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass

Imports log4net

Namespace Model
    Module ShipmentSummaryDB
        Public Function GetShipmentSummaries( _
            ByVal selectedShipmentDate As Date, _
            ByVal businessHour As TimeSpan, _
            ByVal includePrevDateShipment As Boolean, _
            ByVal includeCompleteShipment As Boolean, _
            ByVal customerCode As String _
            ) As List(Of ShipmentSummary)
            Dim shipmentSummaries As List(Of ShipmentSummary) = Nothing

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")


            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    log.Info("GetShipmentSummaries, Open Connection")

                    connection.Open()

                    log.Info("GetShipmentSummaries, Open Connection success")

                    Dim sqlString As String = _
                        "SELECT spl.SHIPMENTDATE, pt.MODEL, " & _
                            "spl.TRINPARTNO, cm.SHORTNAME, " & _
                            "cm.CUSTOMERCODE, spl.PLANTNO, " & _
                            "spl.PLANQTY, spl.SID, spl.SONUMBER, " & _
                            "IFNULL(SUMACTQTY, 0) AS SUMACTQTY, " & _
                            "pt.DIVISIONCODE, " & _
                            "CASE " & _
                                "WHEN IFNULL(SUMACTQTY,0) = 0 THEN 'New'" & _
                                "WHEN SUMACTQTY < spl.PLANQTY THEN 'Partial' " & _
                                "WHEN SUMACTQTY = spl.PLANQTY THEN 'Completed' " & _
                      "ELSE " & _
                                "'Over' " & _
                            "END as STATUS " & _
                        "FROM SHIPMENTPLAN spl " & _
                        "LEFT JOIN (" & _
                            "SELECT SID, SUM(ACTQTY) AS SUMACTQTY " & _
                            "FROM SHIPMENTACT " & _
                            "WHERE DELFLAG = 0 " & _
                            "GROUP BY SID " & _
                            ") sct ON spl.SID = sct.SID " & _
                        "INNER JOIN PARTMASTER pt " & _
                            "ON spl.TRINPARTNO = pt.TRINPARTNO " & _
                        "INNER JOIN CUSTOMERMASTER cm " & _
                            "ON spl.CUSTOMERCODE = cm.CUSTOMERCODE " & _
                        "WHERE {0} " & _
                          "AND {1} " & _
                          "AND spl.DELFLAG = 0 " & _
                        "ORDER BY SHIPMENTDATE ASC, SHORTNAME ASC, " & _
                            "spl.TRINPARTNO ASC, spl.PLANQTY ASC "

                    Dim subString0 As String = String.Empty
                    Dim tomorrowDate As String = (selectedShipmentDate.AddDays(1).ToString("yyyy-MM-dd"))

                    If includePrevDateShipment = True And includeCompleteShipment = True Then
                        subString0 = String.Format("spl.SHIPMENTDATE < '{0}'", tomorrowDate & " " & String.Format("{0:HH:mm:ss}", businessHour))

                    ElseIf includePrevDateShipment = True And includeCompleteShipment = False Then
                        subString0 = String.Format("spl.SHIPMENTDATE < '{0}'", tomorrowDate & " " & String.Format("{0:HH:mm:ss}", businessHour), "AND IFNULL(SUMACTQTY,0) = spl.PLANQTY")

                    ElseIf includePrevDateShipment = False And includeCompleteShipment = True Then
                        Dim todayDate As String = (selectedShipmentDate.ToString("yyyy-MM-dd"))
                        Dim subSql As String = ("spl.SHIPMENTDATE >= '{0}' AND " & "spl.SHIPMENTDATE <= '{1}'")
                        subString0 = String.Format(subSql, todayDate & " " & String.Format("{0:HH:mm:ss}", businessHour), tomorrowDate & " " & String.Format("{0:HH:mm:ss}", businessHour))

                    ElseIf includePrevDateShipment = False And includeCompleteShipment = False Then
                        Dim todayDate As String = (selectedShipmentDate.ToString("yyyy-MM-dd"))
                        Dim subSql As String = ("spl.SHIPMENTDATE >= '{0}' AND " & "spl.SHIPMENTDATE <= '{1}'" & "AND IFNULL(SUMACTQTY,0) <> spl.PLANQTY")
                        subString0 = String.Format(subSql, todayDate & " " & String.Format("{0:HH:mm:ss}", businessHour), tomorrowDate & " " & String.Format("{0:HH:mm:ss}", businessHour))
                    End If

                    If customerCode = "ALL" Then
                        sqlString = String.Format(sqlString, subString0, "spl.CUSTOMERCODE like '%'")
                        log.Info("GetShipmentSummaries SQL string: " & sqlString)

                        shipmentSummaries = CType(connection.Query(Of ShipmentSummary)(sqlString), List(Of ShipmentSummary))
                        log.Info("GetShipmentSummaries result " & shipmentSummaries.Count())

                    Else
                        Dim parameter As Object = (New With {Key .CUSTOMERCODE = customerCode})
                        sqlString = String.Format(sqlString, subString0, "spl.CUSTOMERCODE = " & "'" & customerCode & "'")
                        log.Info("GetShipmentSummaries SQL string: " & sqlString)

                        shipmentSummaries = CType(connection.Query(Of ShipmentSummary)(sqlString), List(Of ShipmentSummary))
                        log.Info("GetShipmentSummaries result " & shipmentSummaries.Count())

                    End If
                Catch ex As Exception

                    log.Error("GetShipmentSummaries DB Error ", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            LogManager.Shutdown()

            Return shipmentSummaries
        End Function
    End Module
End Namespace
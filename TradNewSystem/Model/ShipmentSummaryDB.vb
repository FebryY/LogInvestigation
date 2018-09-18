Option Strict On
Option Explicit On

Imports System.Data
Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass

Namespace Model
    Module ShipmentSummaryDB
        Public Function GetShipmentSummaries( _
            ByVal selectedShipmentDate As Date, _
            ByVal businessHour As TimeSpan, _
            ByVal includePrevDateShipment As Boolean, _
            ByVal customerCode As String _
            ) As List(Of ShipmentSummary)
            Dim shipmentSummaries As List(Of ShipmentSummary) = Nothing

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    connection.Open()

                    ' INFO - SUM Function Works On Dapper Lite
                    '        MySQL SUM INT/DECIMAL return DECIMAL
                    '        MySQL SUM DOUBLE/FLOAT return DOUBLE
                    Dim sqlString As String = _
                        "SELECT spl.SHIPMENTDATE, pt.MODEL, " & _
                            "spl.TRINPARTNO, cm.SHORTNAME, " & _
                            "cm.CUSTOMERCODE, spl.PLANTNO, " & _
                            "spl.PLANQTY, spl.SID, spl.SONUMBER, " & _
                            "IFNULL(SUMACTQTY, 0) AS SUMACTQTY, " & _
                             "pt.DIVISIONCODE " & _
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
                    Dim tomorrowDate As String = ( _
                        selectedShipmentDate.AddDays(1). _
                        ToString("yyyy-MM-dd") _
                        )

                    If includePrevDateShipment Then
                        subString0 = String.Format( _
                            "spl.SHIPMENTDATE < '{0}'", _
                            tomorrowDate & " " & _
                            String.Format("{0:HH:mm:ss}", businessHour) _
                            )
                    Else
                        Dim todayDate As String = ( _
                            selectedShipmentDate.ToString("yyyy-MM-dd") _
                            )
                        Dim subSql As String = ( _
                            "spl.SHIPMENTDATE >= '{0}' AND " & _
                            "spl.SHIPMENTDATE <= '{1}'" _
                            )
                        subString0 = String.Format( _
                            subSql, _
                            todayDate & " " & _
                            String.Format("{0:HH:mm:ss}", businessHour), _
                            tomorrowDate & " " & _
                            String.Format("{0:HH:mm:ss}", businessHour) _
                            )
                    End If

                    If customerCode = "ALL" Then
                        sqlString = String.Format( _
                            sqlString, subString0, _
                            "spl.CUSTOMERCODE like '%'" _
                            )
                        shipmentSummaries = CType( _
                            connection.Query(Of ShipmentSummary)(sqlString),  _
                            List(Of ShipmentSummary) _
                            )
                    Else
                        sqlString = String.Format( _
                            sqlString, subString0, _
                            "spl.CUSTOMERCODE = @CUSTOMERCODE" _
                            )

                        Dim parameter As Object = ( _
                            New With {Key .CUSTOMERCODE = customerCode} _
                            )
                        shipmentSummaries = CType( _
                            connection.Query(Of ShipmentSummary) _
                                (sqlString, parameter),  _
                            List(Of ShipmentSummary) _
                            )
                    End If
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return shipmentSummaries
        End Function
    End Module
End Namespace
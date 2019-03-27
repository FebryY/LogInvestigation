Option Strict On
Option Explicit On

Imports System.Data
Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass

Imports log4net

Namespace Model
    Module StocktakeSummaryDB
        Public Function GetStocktakeSummaries( _
            ByVal stocktakePeriod As Date, _
            ByVal divisionCode As String _
            ) As List(Of StocktakeSummary)

            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim stocktakeSummaries As List(Of StocktakeSummary) = Nothing

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    'log.Info("GetStocktakeSummaries, Open Connection")

                    connection.Open()

                    'log.Info("GetStocktakeSummaries, Open Connection success")

                    Dim sqlString As String = _
                        "SELECT MODEL, st.TRINPARTNO, " & _
                            "SUM(CURRENTSTOCK) AS TOTALSTOCKQTY, " & _
                            "SUM(SCANNEDQTY) AS TOTALSCANNEDQTY, " & _
                            "DIVISIONCODE " & _
                        "FROM STOCKTAKE st, PARTMASTER p " & _
                        "WHERE st.TRINPARTNO = p.TRINPARTNO " & _
                            "AND YEAR(st.STOCKTAKEPERIOD) = @YEAR " & _
                            "AND MONTH(st.STOCKTAKEPERIOD) = @MONTH " & _
                            "AND DAY(st.STOCKTAKEPERIOD) = @DAY " & _
                            "AND st.FINISHTAKE = 0 " & _
                            "AND st.TRINPARTNO IN (" & _
                                "SELECT TRINPARTNO " & _
                                "FROM PARTMASTER " & _
                                "WHERE {0}" & _
                            ") " & _
                        "GROUP BY st.TRINPARTNO, DIVISIONCODE"

                    If divisionCode = "ALL" Then
                        sqlString = String.Format( _
                            sqlString, _
                            "DIVISIONCODE like '%'" _
                            )

                        'log.Info("GetStocktakeSummaries SQL string: " & sqlString)

                        Dim parameters As Object = New With { _
                            Key .YEAR = stocktakePeriod.Year.ToString(), _
                            .MONTH = stocktakePeriod.Month.ToString(), _
                            .DAY = stocktakePeriod.Day.ToString() _
                            }
                        stocktakeSummaries = CType( _
                            connection.Query(Of StocktakeSummary) _
                                (sqlString, parameters),  _
                            List(Of StocktakeSummary) _
                            )

                        'log.Info("GetStocktakeSummaries can get result ")

                    Else
                        sqlString = String.Format( _
                            sqlString, _
                            "DIVISIONCODE = @DIVISIONCODE" _
                            )

                        'log.Info("GetStocktakeSummaries SQL string: " & sqlString)

                        Dim parameters As Object = New With { _
                            Key .YEAR = stocktakePeriod.Year.ToString(), _
                            .MONTH = stocktakePeriod.Month.ToString(), _
                            .DAY = stocktakePeriod.Day.ToString(), _
                            .DIVISIONCODE = divisionCode _
                            }
                        stocktakeSummaries = CType( _
                            connection.Query(Of StocktakeSummary) _
                                (sqlString, parameters),  _
                            List(Of StocktakeSummary) _
                            )

                        'log.Info("GetStocktakeSummaries can get result ")

                    End If
                Catch ex As Exception
                    'log.Error("GetStocktakeSummaries DB Error ", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            'LogManager.Shutdown()

            Return stocktakeSummaries
        End Function
    End Module
End Namespace
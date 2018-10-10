Option Strict On
Option Explicit On

Imports System.Data

Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass

Imports log4net


Namespace Model
    Module StockCardSummaryDB
        Public Function GetStockBalanceQty( _
            ByVal barcodeTag As String, _
            ByRef exceptionMsg As String _
            ) As Integer

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim stockCardSummary As StockCardSummary = Nothing

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    log.Info("GetStockBalanceQty, Open Connection")

                    connection.Open()

                    log.Info("GetStockBalanceQty, Open Connection success")


                    Dim sqlString As String = _
                        "SELECT BARCODETAG, " & _
                            "SUM(STOCK_IN) AS TOTALSTOCKIN, " & _
                            "SUM(STOCK_OUT) AS TOTALSTOCKOUT " & _
                        "FROM STOCK_CARD " & _
                        "WHERE BARCODETAG = @BARCODETAG " & _
                            "AND DELFLAG = 0 " & _
                        "GROUP BY BARCODETAG"

                    log.Info("GetStockBalanceQty SQL string: " & sqlString)

                    Dim parameter As Object = New With { _
                        Key .BARCODETAG = barcodeTag _
                        }

                    stockCardSummary = connection.Query(Of StockCardSummary) _
                            (sqlString, parameter).FirstOrDefault

                    log.Info("GetStockBalanceQty can get result ")

                Catch ex As Exception

                    log.Error("GetStockBalanceQty DB Error ", ex)

                    exceptionMsg = ex.Message
                    Return -99999
                End Try
            End Using

            If stockCardSummary Is Nothing Then
                Return 0
            Else
                Return CInt( _
                    stockCardSummary.TOTALSTOCKIN _
                    - stockCardSummary.TOTALSTOCKOUT _
                    )
            End If

            LogManager.Shutdown()

        End Function

        Public Function CheckDeletedData( _
            ByVal barcodeTag As String, _
            ByRef exceptionMsg As String _
            ) As Integer
            'Dim stockCardSummary As StockCardSummary = Nothing

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim CountOfData As Integer = 0

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    log.Info("CheckDeletedData, Open Connection")

                    connection.Open()

                    log.Info("CheckDeletedData, Open Connection success")

                    Dim sqlString As String = _
                    "Select CAST(Count(*) AS UNSIGNED INTEGER) TotalQty  from( " & _
                        "(Select * from Productionact Where DELFLAG=1 AND Barcodetag= '" & barcodeTag & "')a " & _
                            "inner Join " & _
                        "(SELECT  " & _
                        "TRINPARTNO, SUM(STOCK_IN - STOCK_OUT) AS CURRENTSTOCK, BARCODETAG as Barcodetag1 " & _
                        "FROM(STOCK_CARD)" & _
                        "WHERE(DELFLAG = 0 AND Barcodetag= '" & barcodeTag & "')" & _
                        "GROUP BY BARCODETAG " & _
                        "HAVING CURRENTSTOCK =0)b " & _
                "on a.Barcodetag=b.Barcodetag1 " & _
                ")"

                    log.Info("CheckDeletedData SQL string: " & sqlString)

                    'Dim parameter As Object = New With { _
                    '    Key .BARCODETAG = barcodeTag _
                    '    }

                    CountOfData = CInt(connection.Query(Of ULong) _
                            (sqlString).FirstOrDefault)

                    log.Info("CheckDeletedData result " & CountOfData.ToString())

                Catch ex As Exception

                    log.Error("CheckDeletedData DB Error ", ex)

                    exceptionMsg = ex.Message
                    Return -1
                End Try
            End Using


            LogManager.Shutdown()


            'If CountOfData = 0 Then
            Return CountOfData
            'Else
            'Return True
            'End If
        End Function
    End Module
End Namespace



'*************************** FUTURE RELEASE *****************************************

'Option Strict On
'Option Explicit On

'Imports System.Data

'Imports DapperLite.SqlMapper
'Imports MySql.Data.MySqlClient

'Imports TradNewSystem.Helpers
'Imports TradNewSystem.PocoClass


'Namespace Model
'    Module StockCardSummaryDB
'        Public Function GetStockBalanceQty( _
'            ByVal barcodeTag As List(Of String), _
'            ByRef exceptionMsg As String _
'            ) As Integer
'            Dim stockCardSummary As StockCardSummary = Nothing

'            Using connection As IDbConnection = New MySqlConnection( _
'                CommonLib.GenerateConnectionString _
'                )
'                Try
'                    connection.Open()

'                    Dim sqlString As String = _
'                        "SELECT BARCODETAG, " & _
'                            "SUM(STOCK_IN) AS TOTALSTOCKIN, " & _
'                            "SUM(STOCK_OUT) AS TOTALSTOCKOUT " & _
'                        "FROM STOCK_CARD " & _
'                        "WHERE FIND_IN_SET(BARCODETAG, @BARCODETAG) " & _
'                            "AND DELFLAG = 0 " & _
'                        "GROUP BY BARCODETAG"

'                    Dim parameter As Object = New With { _
'                        Key .BARCODETAG = ( _
'                            String.Join(",", barcodeTag.ToArray()) _
'                            ) _
'                        }

'                    stockCardSummary = connection.Query(Of StockCardSummary) _
'                            (sqlString, parameter).FirstOrDefault
'                Catch ex As Exception
'                    exceptionMsg = ex.Message
'                    Return -99999
'                End Try
'            End Using

'            If stockCardSummary Is Nothing Then
'                Return 0
'            Else
'                Return CInt( _
'                    stockCardSummary.TOTALSTOCKIN _
'                    - stockCardSummary.TOTALSTOCKOUT _
'                    )
'            End If
'        End Function
'    End Module
'End Namespace
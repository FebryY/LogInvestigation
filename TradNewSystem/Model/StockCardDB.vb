Option Strict On
Option Explicit On

Imports System.Data

Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass

Imports log4net


Namespace Model
    Public Structure TotalStock
        Dim StockIn As Integer
        Dim StockOut As Integer
    End Structure

    Public Enum StockCardValues
        TrinPartCode
        ActID
        ActQty
        TypeID
        BarcodeTag
        LineCode
        ScanDateTime
        UserID
    End Enum

    Module StockCardDB
        Public Function InsertData( _
            ByVal stockCardCollection As List(Of String()), _
            ByRef exceptionMsg As String _
            ) As Boolean

            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Dim transaction As IDbTransaction = Nothing

                Try
                    'log.Info("InsertData, Open Connection")

                    connection.Open()

                    'log.Info("InsertData, Open Connection success")

                    transaction = connection.BeginTransaction()

                    Dim sqlString As String = ( _
                        "INSERT STOCK_CARD(" & _
                            "TRINPARTNO, ACTID, STOCK_OUT, " & _
                            "TYPE_ID, BARCODETAG, LINECODE, " & _
                            "DATE_TIME, USERID, DELFLAG" & _
                        ") " & _
                        "VALUES(" & _
                            "@TRINPARTNO, @ACTID, @STOCK_OUT, " & _
                            "@TYPE_ID, @BARCODETAG, @LINECODE, " & _
                            "@DATE_TIME, @USERID, 0" & _
                        ")" _
                        )

                    'log.Info("InsertData SQL string: " & sqlString)

                    For Each stockCardData As String() In stockCardCollection
                        Dim parameters As Object = New With { _
                            Key .TRINPARTNO = stockCardData( _
                                StockCardValues.TrinPartCode _
                                ), _
                            .ACTID = stockCardData( _
                                StockCardValues.ActID _
                                ), _
                            .STOCK_OUT = stockCardData( _
                                StockCardValues.ActQty _
                                ), _
                            .TYPE_ID = stockCardData( _
                                StockCardValues.TypeID _
                                ), _
                            .BARCODETAG = stockCardData( _
                                StockCardValues.BarcodeTag _
                                ), _
                            .LINECODE = stockCardData( _
                                StockCardValues.LineCode _
                                ), _
                            .DATE_TIME = stockCardData( _
                            StockCardValues.ScanDateTime _
                                ), _
                            .USERID = stockCardData( _
                                StockCardValues.UserID _
                                ) _
                            }
                        connection.Execute(sqlString, parameters, transaction)
                    Next

                    'log.Info("Operation End")

                    transaction.Commit()
                Catch ex As Exception
                    'log.Error("InsertData DB Error ", ex)
                    If Not transaction Is Nothing Then
                        transaction.Rollback()
                    End If

                    exceptionMsg = ex.Message
                    Return False
                End Try
            End Using

            LogManager.Shutdown()

            Return True
        End Function

        Public Function InsertDataForShipment( _
            ByVal connection As IDbConnection, _
            ByVal transaction As IDbTransaction, _
            ByVal stockCardCollection As List(Of String()), _
            ByRef exceptionMsg As String _
            ) As Boolean

            'Using connection As IDbConnection = New MySqlConnection( _
            '    CommonLib.GenerateConnectionString _
            '    )
            '    Dim transaction As IDbTransaction = Nothing
            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Try
                '        connection.Open()
                'log.Info("InsertDataForShipment, Open Connection")

                transaction = connection.BeginTransaction()

                'log.Info("InsertDataForShipment, Open Connection success")


                Dim sqlString As String = ( _
                    "INSERT STOCK_CARD(" & _
                        "TRINPARTNO, ACTID, STOCK_OUT, " & _
                        "TYPE_ID, BARCODETAG, LINECODE, " & _
                        "DATE_TIME, USERID, DELFLAG" & _
                    ") " & _
                    "VALUES(" & _
                        "@TRINPARTNO, @ACTID, @STOCK_OUT, " & _
                        "@TYPE_ID, @BARCODETAG, @LINECODE, " & _
                        "@DATE_TIME, @USERID, 0" & _
                    ");" & _
                    "Update shipmentact set DELFLAG=0, ACCPACSENDFLG=0 where ACTID=@ACTID" _
                    )

                'log.Info("InsertDataForShipment SQL string: " & sqlString)

                For Each stockCardData As String() In stockCardCollection
                    Dim parameters As Object = New With { _
                        Key .TRINPARTNO = stockCardData( _
                            StockCardValues.TrinPartCode _
                            ), _
                        .ACTID = stockCardData( _
                            StockCardValues.ActID _
                            ), _
                        .STOCK_OUT = stockCardData( _
                            StockCardValues.ActQty _
                            ), _
                        .TYPE_ID = stockCardData( _
                            StockCardValues.TypeID _
                            ), _
                        .BARCODETAG = stockCardData( _
                            StockCardValues.BarcodeTag _
                            ), _
                        .LINECODE = stockCardData( _
                            StockCardValues.LineCode _
                            ), _
                        .DATE_TIME = stockCardData( _
                        StockCardValues.ScanDateTime _
                            ), _
                        .USERID = stockCardData( _
                            StockCardValues.UserID _
                            ) _
                        }
                    connection.Execute(sqlString, parameters, transaction)
                Next

                'log.Info("Operation End")

                transaction.Commit()
            Catch ex As Exception

                'log.Error("InsertDataForShipment DB Error ", ex)

                If Not transaction Is Nothing Then
                    transaction.Rollback()
                End If

                exceptionMsg = ex.Message
                Return False
            End Try
            'End Using

            'LogManager.Shutdown()

            Return True
        End Function

        Public Function UpdateData( _
            ByVal stockCardCollection As List(Of String()), _
            ByRef exceptionMsg As String _
            ) As Boolean

            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Dim transaction As IDbTransaction = Nothing

                Try
                    'log.Info("UpdateData, Open Connection")

                    connection.Open()

                    transaction = connection.BeginTransaction()

                    'log.Info("UpdateData, Open Connection success")


                    Dim sqlString As String = ( _
                        "UPDATE STOCK_CARD " & _
                        "SET STOCK_OUT=@STOCK_OUT, LINECODE=@LINECODE, " & _
                            "DATE_TIME=@DATE_TIME, USERID=@USERID, " & _
                            "DELFLAG=0 " & _
                        "WHERE ACTID=@ACTID " & _
                            "AND BARCODETAG=@BARCODETAG " & _
                            "AND DELFLAG=1" _
                        )

                    'log.Info("UpdateData SQL string: " & sqlString)

                    For Each stockCardData As String() In stockCardCollection
                        Dim parameters As Object = New With { _
                            Key .STOCK_OUT = stockCardData( _
                                StockCardValues.ActQty _
                                ), _
                            .LINECODE = stockCardData( _
                                StockCardValues.LineCode _
                                ), _
                            .DATE_TIME = stockCardData( _
                                StockCardValues.ScanDateTime _
                                ), _
                            .USERID = stockCardData( _
                                StockCardValues.UserID _
                                ), _
                            .ACTID = stockCardData( _
                                StockCardValues.ActID _
                                ), _
                            .BARCODETAG = stockCardData( _
                                StockCardValues.BarcodeTag _
                                ) _
                            }

                        connection.Execute(sqlString, parameters, transaction)
                    Next
                    'log.Info("Operation End")
                    transaction.Commit()
                Catch ex As Exception

                    'log.Error("UpdateData DB Error ", ex)

                    If Not transaction Is Nothing Then
                        transaction.Rollback()
                    End If

                    exceptionMsg = ex.Message
                    Return False
                End Try
            End Using

            'LogManager.Shutdown()

            Return True
        End Function

        Public Function UpdateDataForShipment( _
          ByVal connection As IDbConnection, _
          ByVal transaction As IDbTransaction, _
          ByVal stockCardCollection As List(Of String()), _
          ByRef exceptionMsg As String _
          ) As Boolean
            'Using connection As IDbConnection = New MySqlConnection( _
            '    CommonLib.GenerateConnectionString _
            '    )
            '    Dim transaction As IDbTransaction = Nothing

            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Try
                'log.Info("UpdateDataForShipment, Open Connection")

                connection.Open()

                transaction = connection.BeginTransaction()

                'log.Info("UpdateDataForShipment, Open Connection success")

                Dim sqlString As String = ( _
                    "UPDATE STOCK_CARD " & _
                    "SET STOCK_OUT=@STOCK_OUT, LINECODE=@LINECODE, " & _
                        "DATE_TIME=@DATE_TIME, USERID=@USERID, " & _
                        "DELFLAG=0 " & _
                    "WHERE ACTID=@ACTID " & _
                        "AND BARCODETAG=@BARCODETAG " & _
                        "AND DELFLAG=1" _
                    )

                'log.Info("UpdateDataForShipment SQL string: " & sqlString)

                For Each stockCardData As String() In stockCardCollection
                    Dim parameters As Object = New With { _
                        Key .STOCK_OUT = stockCardData( _
                            StockCardValues.ActQty _
                            ), _
                        .LINECODE = stockCardData( _
                            StockCardValues.LineCode _
                            ), _
                        .DATE_TIME = stockCardData( _
                            StockCardValues.ScanDateTime _
                            ), _
                        .USERID = stockCardData( _
                            StockCardValues.UserID _
                            ), _
                        .ACTID = stockCardData( _
                            StockCardValues.ActID _
                            ), _
                        .BARCODETAG = stockCardData( _
                            StockCardValues.BarcodeTag _
                            ) _
                        }

                    connection.Execute(sqlString, parameters, transaction)
                Next
                'log.Info("Operation End")
                transaction.Commit()
            Catch ex As Exception

                'log.Error("UpdateDataForShipment DB Error ", ex)

                If Not transaction Is Nothing Then
                    transaction.Rollback()
                End If

                exceptionMsg = ex.Message
                Return False
            End Try
            'End Using

            'LogManager.Shutdown()

            Return True
        End Function

        Public Function GetTotalStockInOut( _
            ByVal trinPartNo As String _
            ) As TotalStock

            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim stockCard As StockCard = Nothing

            Dim totalStock As TotalStock
            totalStock.StockIn = 0
            totalStock.StockOut = 0

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    'log.Info("GetTotalStockInOut, Open Connection")

                    connection.Open()

                    'log.Info("GetTotalStockInOut, Open Connection success")

                    Dim sqlString As String = ( _
                        "SELECT SUM(STOCK_IN) AS SUM_STOCK_IN, " & _
                            "SUM(STOCK_OUT) AS SUM_STOCK_OUT " & _
                        "FROM STOCK_CARD " & _
                        "WHERE TRINPARTNO = @TRINPARTNO " & _
                            "AND DELFLAG = 0" _
                        )

                    'log.Info("GetTotalStockInOut SQL string: " & sqlString)

                    Dim parameter As Object = New With { _
                        Key .TRINPARTNO = trinPartNo _
                        }
                    stockCard = connection.Query(Of StockCard) _
                        (sqlString, parameter).FirstOrDefault()



                Catch ex As Exception

                    'log.Error("GetTotalStockInOut DB Error ", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not stockCard Is Nothing Then
                totalStock.StockIn = CInt(stockCard.SUM_STOCK_IN)
                totalStock.StockOut = CInt(stockCard.SUM_STOCK_OUT)
            End If

            'log.Info("GetTotalStockIn result " & totalStock.StockIn)
            'log.Info("GetTotalStockOut result " & totalStock.StockOut)
            'LogManager.Shutdown()

            Return totalStock
        End Function

        Public Function DeleteData( _
            ByVal actIds As List(Of Integer), _
            ByRef exceptionMsg As String _
            ) As Boolean

            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim strActIds As String() = ( _
                actIds.[Select](Function(x) x.ToString()).ToArray() _
                )

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Dim transaction As IDbTransaction = Nothing

                Try
                    'log.Info("DeleteData, Open Connection")

                    connection.Open()
                    transaction = connection.BeginTransaction

                    'log.Info("DeleteData, Open Connection success")
                    
                    Dim sqlString As String = ( _
                        "DELETE FROM STOCK_CARD " & _
                        "WHERE FIND_IN_SET(ACTID, @ACTIDS) " & _
                            "AND TYPE_ID=2" _
                        )

                    'log.Info("DeleteData SQL string: " & sqlString)

                    Dim param As Object = New With { _
                        Key .ACTIDS = String.Join(",", strActIds) _
                        }

                    connection.Execute( _
                        sqlString, param, transaction _
                        )

                    'log.Info("Operation End")
                    transaction.Commit()
                Catch ex As Exception

                    'log.Error("DeleteData DB Error ", ex)

                    If Not transaction Is Nothing Then
                        transaction.Rollback()
                    End If

                    exceptionMsg = ex.Message
                    Return False
                End Try
            End Using

            'LogManager.Shutdown()

            Return True
        End Function

        Public Function GetRemainStockByTag( _
            ByVal BarcodeTag As String _
            ) As Integer
            'Dim stockCard As StockCard = Nothing

            'Dim totalStock As TotalStock
            'totalStock.StockIn = 0
            'totalStock.StockOut = 0
            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim RemainQty As Integer = 0

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    'log.Info("GetRemainStockByTag, Open Connection")

                    connection.Open()

                    'log.Info("GetRemainStockByTag, Open Connection success")

                    Dim sqlString As String = ( _
                        "SELECT CAST(SUM(STOCK_IN) AS UNSIGNED INTEGER)- " & _
                            "CAST(SUM(STOCK_OUT) AS UNSIGNED INTEGER) AS RemainStock " & _
                        "FROM STOCK_CARD " & _
                        "WHERE BARCODETAG = '" & BarcodeTag & "' AND DELFLAG = 0" & _
                            "" _
                        )

                    'log.Info("GetRemainStockByTag SQL string: " & sqlString)

                    'Dim parameter As Object = New With { _
                    '    Key .BARCODETAG = BarcodeTag _
                    '    }
                    'stockCard = connection.Query(Of StockCard) _
                    '    (sqlString, parameter).FirstOrDefault()

                    'stockCard = connection.Query(Of StockCard) _
                    '    (sqlString).FirstOrDefault()
                    RemainQty = CInt( _
                       connection.Query(Of ULong)(sqlString).DefaultIfEmpty(0).FirstOrDefault)

                    'log.Info("GetRemainStockByTag can get result ")

                Catch ex As Exception

                    'log.Error("GetRemainStockByTag DB Error ", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            'If Not stockCard Is Nothing Then
            '    totalStock.StockIn = CInt(stockCard.SUM_STOCK_IN)
            '    totalStock.StockOut = CInt(stockCard.SUM_STOCK_OUT)
            'End If

            'LogManager.Shutdown()

            Return RemainQty
        End Function
    End Module
End Namespace
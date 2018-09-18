Option Strict On
Option Explicit On

Imports System.Data

Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass


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

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Dim transaction As IDbTransaction = Nothing

                Try
                    connection.Open()
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

                    transaction.Commit()
                Catch ex As Exception
                    If Not transaction Is Nothing Then
                        transaction.Rollback()
                    End If

                    exceptionMsg = ex.Message
                    Return False
                End Try
            End Using

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

            Try
                '        connection.Open()
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
                    ");" & _
                    "Update shipmentact set DELFLAG=0, ACCPACSENDFLG=0 where ACTID=@ACTID" _
                    )

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

                transaction.Commit()
            Catch ex As Exception
                If Not transaction Is Nothing Then
                    transaction.Rollback()
                End If

                exceptionMsg = ex.Message
                Return False
            End Try
            'End Using

            Return True
        End Function

        Public Function UpdateData( _
            ByVal stockCardCollection As List(Of String()), _
            ByRef exceptionMsg As String _
            ) As Boolean
            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Dim transaction As IDbTransaction = Nothing

                Try
                    connection.Open()
                    transaction = connection.BeginTransaction()

                    Dim sqlString As String = ( _
                        "UPDATE STOCK_CARD " & _
                        "SET STOCK_OUT=@STOCK_OUT, LINECODE=@LINECODE, " & _
                            "DATE_TIME=@DATE_TIME, USERID=@USERID, " & _
                            "DELFLAG=0 " & _
                        "WHERE ACTID=@ACTID " & _
                            "AND BARCODETAG=@BARCODETAG " & _
                            "AND DELFLAG=1" _
                        )

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

                    transaction.Commit()
                Catch ex As Exception
                    If Not transaction Is Nothing Then
                        transaction.Rollback()
                    End If

                    exceptionMsg = ex.Message
                    Return False
                End Try
            End Using

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

            Try
                connection.Open()
                transaction = connection.BeginTransaction()

                Dim sqlString As String = ( _
                    "UPDATE STOCK_CARD " & _
                    "SET STOCK_OUT=@STOCK_OUT, LINECODE=@LINECODE, " & _
                        "DATE_TIME=@DATE_TIME, USERID=@USERID, " & _
                        "DELFLAG=0 " & _
                    "WHERE ACTID=@ACTID " & _
                        "AND BARCODETAG=@BARCODETAG " & _
                        "AND DELFLAG=1" _
                    )

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

                transaction.Commit()
            Catch ex As Exception
                If Not transaction Is Nothing Then
                    transaction.Rollback()
                End If

                exceptionMsg = ex.Message
                Return False
            End Try
            'End Using

            Return True
        End Function

        Public Function GetTotalStockInOut( _
            ByVal trinPartNo As String _
            ) As TotalStock
            Dim stockCard As StockCard = Nothing

            Dim totalStock As TotalStock
            totalStock.StockIn = 0
            totalStock.StockOut = 0

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    connection.Open()

                    Dim sqlString As String = ( _
                        "SELECT SUM(STOCK_IN) AS SUM_STOCK_IN, " & _
                            "SUM(STOCK_OUT) AS SUM_STOCK_OUT " & _
                        "FROM STOCK_CARD " & _
                        "WHERE TRINPARTNO = @TRINPARTNO " & _
                            "AND DELFLAG = 0" _
                        )

                    Dim parameter As Object = New With { _
                        Key .TRINPARTNO = trinPartNo _
                        }
                    stockCard = connection.Query(Of StockCard) _
                        (sqlString, parameter).FirstOrDefault()
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not stockCard Is Nothing Then
                totalStock.StockIn = CInt(stockCard.SUM_STOCK_IN)
                totalStock.StockOut = CInt(stockCard.SUM_STOCK_OUT)
            End If

            Return totalStock
        End Function

        Public Function DeleteData( _
            ByVal actIds As List(Of Integer), _
            ByRef exceptionMsg As String _
            ) As Boolean
            Dim strActIds As String() = ( _
                actIds.[Select](Function(x) x.ToString()).ToArray() _
                )

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Dim transaction As IDbTransaction = Nothing

                Try
                    connection.Open()
                    transaction = connection.BeginTransaction

                    Dim sqlString As String = ( _
                        "DELETE FROM STOCK_CARD " & _
                        "WHERE FIND_IN_SET(ACTID, @ACTIDS) " & _
                            "AND TYPE_ID=2" _
                        )
                    Dim param As Object = New With { _
                        Key .ACTIDS = String.Join(",", strActIds) _
                        }

                    connection.Execute( _
                        sqlString, param, transaction _
                        )
                    transaction.Commit()
                Catch ex As Exception
                    If Not transaction Is Nothing Then
                        transaction.Rollback()
                    End If

                    exceptionMsg = ex.Message
                    Return False
                End Try
            End Using

            Return True
        End Function

        Public Function GetRemainStockByTag( _
            ByVal BarcodeTag As String _
            ) As Integer
            'Dim stockCard As StockCard = Nothing

            'Dim totalStock As TotalStock
            'totalStock.StockIn = 0
            'totalStock.StockOut = 0
            Dim RemainQty As Integer = 0

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    connection.Open()

                    Dim sqlString As String = ( _
                        "SELECT CAST(SUM(STOCK_IN) AS UNSIGNED INTEGER)- " & _
                            "CAST(SUM(STOCK_OUT) AS UNSIGNED INTEGER) AS RemainStock " & _
                        "FROM STOCK_CARD " & _
                        "WHERE BARCODETAG = '" & BarcodeTag & "' AND DELFLAG = 0" & _
                            "" _
                        )

                    'Dim parameter As Object = New With { _
                    '    Key .BARCODETAG = BarcodeTag _
                    '    }
                    'stockCard = connection.Query(Of StockCard) _
                    '    (sqlString, parameter).FirstOrDefault()

                    'stockCard = connection.Query(Of StockCard) _
                    '    (sqlString).FirstOrDefault()
                    RemainQty = CInt( _
                       connection.Query(Of ULong)(sqlString).DefaultIfEmpty(0).FirstOrDefault)
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            'If Not stockCard Is Nothing Then
            '    totalStock.StockIn = CInt(stockCard.SUM_STOCK_IN)
            '    totalStock.StockOut = CInt(stockCard.SUM_STOCK_OUT)
            'End If

            Return RemainQty
        End Function
    End Module
End Namespace
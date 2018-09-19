Option Strict On
Option Explicit On

Imports System.Data

Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass

Imports log4net

Namespace Model
    Public Enum StocktakeValues
        TrinPartNo
        BarcodeTag
        ScannedQty
        StocktakePeriod
        TakeDateTime
    End Enum

    Module StocktakeDB
        Public Function GetLatestStocktakePeriod( _
            ByRef exceptionMsg As String _
            ) As DateTime

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim stocktakePeriod As DateTime = Nothing

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    log.Info("GetLatestStocktakePeriod, Open Connection")

                    connection.Open()

                    log.Info("GetLatestStocktakePeriod, Open Connection success")


                    Dim sqlString As String = ( _
                        "SELECT DISTINCT MAX(STOCKTAKEPERIOD) " & _
                        "FROM STOCKTAKE " & _
                        "WHERE FINISHTAKE = 0 " & _
                            "AND DELFLAG = 0" _
                        )

                    log.Info("GetLatestStocktakePeriod SQL string: " & sqlString)

                    stocktakePeriod = connection.Query(Of DateTime) _
                        (sqlString).DefaultIfEmpty(Nothing).FirstOrDefault

                    log.Info("GetLatestStocktakePeriod result " & stocktakePeriod.ToString())

                Catch ex As Exception
                    log.Error("GetLatestStocktakePeriod DB Error ", ex)

                    exceptionMsg = ex.Message
                End Try
            End Using

            Return stocktakePeriod
        End Function

        Public Function GetScannedBarcodeCount( _
            ByVal trinPartNo As String, _
            ByVal stocktakePeriod As Date _
            ) As Int64

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim barcodeCount As Int64 = 0

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    log.Info("GetScannedBarcodeCount, Open Connection")

                    connection.Open()

                    log.Info("GetScannedBarcodeCount, Open Connection success")

                    Dim sqlString As String = ( _
                        "SELECT COUNT(*) " & _
                        "FROM STOCKTAKE " & _
                        "WHERE TRINPARTNO = @TRINPARTNO " & _
                            "AND SCANNEDQTY > 0 " & _
                            "AND YEAR(STOCKTAKEPERIOD) = @YEAR " & _
                            "AND MONTH(STOCKTAKEPERIOD) = @MONTH " & _
                            "AND FINISHTAKE = 0 " & _
                            "AND DELFLAG = 0" _
                        )

                    log.Info("GetScannedBarcodeCount SQL string: " & sqlString)

                    Dim parameters As Object = New With { _
                        Key .TRINPARTNO = trinPartNo, _
                        .YEAR = stocktakePeriod.Year.ToString(), _
                        .MONTH = stocktakePeriod.Month.ToString() _
                        }

                    barcodeCount = connection.Query(Of Int64) _
                        (sqlString, parameters).DefaultIfEmpty(0). _
                            FirstOrDefault

                    log.Info("GetScannedBarcodeCount result " & barcodeCount.ToString())

                Catch ex As Exception
                    log.Error("GetScannedBarcodeCount DB Error ", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return barcodeCount
        End Function

        Public Function GetBarcodesAndScannedQty( _
            ByVal barcodeTags As List(Of String), _
            ByVal stocktakePeriod As Date, _
            ByVal scannedBarcode As Boolean _
            ) As List(Of Stocktake)

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim stocktakes As List(Of Stocktake) = Nothing

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    log.Info("GetBarcodesAndScannedQty, Open Connection")

                    connection.Open()

                    log.Info("GetBarcodesAndScannedQty, Open Connection success")

                    Dim sqlString As String = ( _
                        "SELECT BARCODETAG, SCANNEDQTY " & _
                        "FROM STOCKTAKE " & _
                        "WHERE FIND_IN_SET(BARCODETAG, @BARCODETAGS) " & _
                            "AND YEAR(STOCKTAKEPERIOD) = @YEAR " & _
                            "AND MONTH(STOCKTAKEPERIOD) = @MONTH " & _
                            "AND FINISHTAKE = 0 " _
                        )

                    If scannedBarcode Then
                        sqlString &= "AND SCANNEDQTY > 0 "
                        sqlString &= "AND DELFLAG = 0"

                        log.Info("GetBarcodesAndScannedQty SQL string: " & sqlString)

                    Else
                        sqlString &= "AND SCANNEDQTY = 0"

                        log.Info("GetBarcodesAndScannedQty SQL string: " & sqlString)

                    End If

                    Dim mergedBarcodes As String = String.Join( _
                        ",", _
                        barcodeTags.ToArray() _
                        )

                    Dim parameters As Object = New With { _
                        Key .BARCODETAGS = mergedBarcodes, _
                        .YEAR = stocktakePeriod.Year.ToString(), _
                        .MONTH = stocktakePeriod.Month.ToString() _
                        }
                    stocktakes = CType( _
                        connection.Query(Of Stocktake) _
                            (sqlString, parameters),  _
                        List(Of Stocktake) _
                        )

                    log.Info("GetBarcodesAndScannedQty result " & stocktakes.ToString())

                Catch ex As Exception
                    log.Error("GetBarcodesAndScannedQty DB Error ", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return stocktakes
        End Function

        Public Function IsRegistered( _
            ByVal barcodeTag As String, _
            ByVal stocktakePeriod As Date _
            ) As QueryRetValue

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim stocktake As Stocktake = Nothing
            Dim retValue As QueryRetValue = QueryRetValue.ValueNil

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    log.Info("IsRegistered, Open Connection")

                    connection.Open()

                    log.Info("IsRegistered, Open Connection success")

                    Dim sqlString As String = ( _
                        "SELECT STOCKTAKEID " & _
                        "FROM STOCKTAKE " & _
                        "WHERE BARCODETAG = @BARCODETAG " & _
                            "AND YEAR(STOCKTAKEPERIOD) = @YEAR " & _
                            "AND MONTH(STOCKTAKEPERIOD) = @MONTH " & _
                            "AND FINISHTAKE = 0" _
                        )


                    log.Info("IsRegistered SQL string: " & sqlString)

                    Dim parameters As Object = New With { _
                        Key .BARCODETAG = barcodeTag, _
                        .YEAR = stocktakePeriod.Year.ToString(), _
                        .MONTH = stocktakePeriod.Month.ToString() _
                        }

                    stocktake = connection.Query(Of Stocktake) _
                        (sqlString, parameters).FirstOrDefault

                    log.Info("IsRegistered result " & stocktake.ToString())

                Catch ex As Exception

                    log.Error("IsRegistered DB Error ", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                    retValue = QueryRetValue.ValueError
                End Try
            End Using

            If retValue = QueryRetValue.ValueNil Then
                If Not stocktake Is Nothing Then
                    retValue = QueryRetValue.ValueTrue
                Else
                    retValue = QueryRetValue.ValueFalse
                End If
            End If

            Return retValue
        End Function

        Public Function HasBeenScannedAndSaved( _
            ByVal barcodeTag As String, _
            ByVal stocktakePeriod As Date _
            ) As Boolean

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim stocktake As Stocktake = Nothing

            Dim hasBeenSaved As Boolean = False

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    log.Info("HasBeenScannedAndSaved, Open Connection")

                    connection.Open()

                    log.Info("HasBeenScannedAndSaved, Open Connection success")

                    Dim sqlString As String = ( _
                        "SELECT CURRENTSTOCK, SCANNEDQTY " & _
                        "FROM STOCKTAKE " & _
                        "WHERE BARCODETAG = @BARCODETAG " & _
                            "AND YEAR(STOCKTAKEPERIOD) = @YEAR " & _
                            "AND MONTH(STOCKTAKEPERIOD) = @MONTH " & _
                            "AND FINISHTAKE = 0 " & _
                            "AND DELFLAG = 0" _
                        )

                    log.Info("HasBeenScannedAndSaved SQL string: " & sqlString)

                    Dim parameters As Object = New With { _
                        Key .BARCODETAG = barcodeTag, _
                        .YEAR = stocktakePeriod.Year.ToString(), _
                        .MONTH = stocktakePeriod.Month.ToString() _
                        }

                    stocktake = connection.Query(Of Stocktake) _
                        (sqlString, parameters).FirstOrDefault

                    log.Info("HasBeenScannedAndSaved result " & stocktake.ToString())

                Catch ex As Exception

                    log.Error("HasBeenScannedAndSaved DB Error ", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not stocktake Is Nothing Then
                If stocktake.CURRENTSTOCK = stocktake.SCANNEDQTY _
                    Or (stocktake.CURRENTSTOCK = 0 _
                            And stocktake.SCANNEDQTY > 0) Then
                    hasBeenSaved = True
                End If
            End If

            Return hasBeenSaved
        End Function
        'Remarks 9j
        'Public Function InsertData( _
        '    ByVal stocktakeCollection As List(Of String()), _
        '    ByRef exceptionMsg As String _
        '    ) As Boolean
        '    Using connection As IDbConnection = New MySqlConnection( _
        '        CommonLib.GenerateConnectionString _
        '        )
        '        Dim transaction As IDbTransaction = Nothing

        '        Try
        '            connection.Open()
        '            'transaction = connection.BeginTransaction(IsolationLevel.Serializable)

        '            ' Skip inserting data if duplicate found
        '            Dim sqlString As String = _
        '                "INSERT INTO STOCKTAKE(" & _
        '                    "TRINPARTNO, BARCODETAG, CURRENTSTOCK, " & _
        '                    "SCANNEDQTY, REMARKS, STOCKTAKEPERIOD, " & _
        '                    "TAKEDATETIME, ADDCOLFLG, DELCOLFLG, " & _
        '                    "APPROVEDFLG, FINISHTAKE, DELFLAG, " & _
        '                    "ACCPACSENDFLG" & _
        '                ") " & _
        '                "SELECT @TRINPARTNO, @BARCODETAG, 0, " & _
        '                    "@SCANNEDQTY, '', @STOCKTAKEPERIOD, " & _
        '                    "@TAKEDATETIME, 1, 0, " & _
        '                    "0, 0, 0, " & _
        '                    "0 " & _
        '                "FROM STOCKTAKE " & _
        '                "WHERE NOT EXISTS( " & _
        '                    "SELECT BARCODETAG " & _
        '                    "FROM STOCKTAKE " & _
        '                    "WHERE BARCODETAG = @BARCODETAG " & _
        '                        "AND YEAR(STOCKTAKEPERIOD) = @YEAR " & _
        '                        "AND MONTH(STOCKTAKEPERIOD) = @MONTH " & _
        '                        "AND FINISHTAKE = 0" & _
        '                ") " & _
        '                "LIMIT 1"

        '            For Each stocktakeData As String() In stocktakeCollection
        '                Dim stocktakeDateTime As String = ( _
        '                    stocktakeData(StocktakeValues.StocktakePeriod) _
        '                    )
        '                Dim stocktakePeriod As DateTime = ( _
        '                    DateTime.ParseExact( _
        '                        stocktakeDateTime, _
        '                        "yyyy-MM-dd HH:mm:ss", _
        '                        Nothing _
        '                        ) _
        '                    )
        '                Dim parameters As Object = New With { _
        '                    Key .TRINPARTNO = stocktakeData( _
        '                        StocktakeValues.TrinPartNo _
        '                        ), _
        '                    .BARCODETAG = stocktakeData( _
        '                        StocktakeValues.BarcodeTag _
        '                        ), _
        '                    .SCANNEDQTY = stocktakeData( _
        '                        StocktakeValues.ScannedQty _
        '                        ), _
        '                    .STOCKTAKEPERIOD = stocktakeData( _
        '                        StocktakeValues.StocktakePeriod _
        '                        ), _
        '                    .TAKEDATETIME = stocktakeData( _
        '                        StocktakeValues.TakeDateTime _
        '                        ), _
        '                    .YEAR = stocktakePeriod.Year.ToString(), _
        '                    .MONTH = stocktakePeriod.Month.ToString() _
        '                    }



        '                connection.Execute( _
        '                    sqlString, _
        '                    parameters, _
        '                    transaction _
        '                    )
        '            Next

        '            'transaction.Commit()
        '        Catch ex As MySqlException
        '            If Not transaction Is Nothing Then
        '                'transaction.Rollback()
        '            End If

        '            If ex.Number = 0 Then
        '                DisplayMessage.ErrorMsg(ex.Message.ToString, "Error")
        '            End If

        '            exceptionMsg = ex.Message
        '            Return False
        '        End Try
        '    End Using

        '    Return True
        'End Function

        'Add 9j
        Public Function InsertData( _
           ByVal stocktakeCollection As List(Of String()), _
           ByRef AlreadyDeleteBarcodetag As String, _
           ByRef alreadyDelcount As Integer, _
           ByRef exceptionMsg As String, _
           Optional ByRef alreadyDelQty As Double = 0 _
           ) As Boolean

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            'Dim alreadyDelcount As Integer = 0
            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Dim transaction As IDbTransaction = Nothing

                Try
                    log.Info("InsertData Stock Take, Open Connection")

                    connection.Open()

                    log.Info("InsertData Stock Take, Open Connection success")
                    'transaction = connection.BeginTransaction(IsolationLevel.Serializable)

                    ' Skip inserting data if duplicate found
                    Dim sqlString As String = _
                        "INSERT INTO STOCKTAKE(" & _
                            "TRINPARTNO, BARCODETAG, CURRENTSTOCK, " & _
                            "SCANNEDQTY, REMARKS, STOCKTAKEPERIOD, " & _
                            "TAKEDATETIME, ADDCOLFLG, DELCOLFLG, " & _
                            "APPROVEDFLG, FINISHTAKE, DELFLAG, " & _
                            "ACCPACSENDFLG" & _
                        ") " & _
                        "SELECT @TRINPARTNO, @BARCODETAG, 0, " & _
                            "@SCANNEDQTY, '', @STOCKTAKEPERIOD, " & _
                            "@TAKEDATETIME, 1, 0, " & _
                            "0, 0, 0, " & _
                            "0 " & _
                        "FROM STOCKTAKE " & _
                        "WHERE NOT EXISTS( " & _
                            "SELECT BARCODETAG " & _
                            "FROM STOCKTAKE " & _
                            "WHERE BARCODETAG = @BARCODETAG " & _
                                "AND YEAR(STOCKTAKEPERIOD) = @YEAR " & _
                                "AND MONTH(STOCKTAKEPERIOD) = @MONTH " & _
                                "AND FINISHTAKE = 0" & _
                        ") " & _
                        "LIMIT 1"

                    log.Info("InsertData Stock Take SQL string: " & sqlString)


                    For Each stocktakeData As String() In stocktakeCollection
                        Dim stocktakeDateTime As String = ( _
                            stocktakeData(StocktakeValues.StocktakePeriod) _
                            )
                        Dim stocktakePeriod As DateTime = ( _
                            DateTime.ParseExact( _
                                stocktakeDateTime, _
                                "yyyy-MM-dd HH:mm:ss", _
                                Nothing _
                                ) _
                            )
                        Dim parameters As Object = New With { _
                            Key .TRINPARTNO = stocktakeData( _
                                StocktakeValues.TrinPartNo _
                                ), _
                            .BARCODETAG = stocktakeData( _
                                StocktakeValues.BarcodeTag _
                                ), _
                            .SCANNEDQTY = stocktakeData( _
                                StocktakeValues.ScannedQty _
                                ), _
                            .STOCKTAKEPERIOD = stocktakeData( _
                                StocktakeValues.StocktakePeriod _
                                ), _
                            .TAKEDATETIME = stocktakeData( _
                                StocktakeValues.TakeDateTime _
                                ), _
                            .YEAR = stocktakePeriod.Year.ToString(), _
                            .MONTH = stocktakePeriod.Month.ToString() _
                            }


                        ''Modify 9k-3
                        'Dim QrValid As QueryRetValue = ProductionActDB.fncCheckDeletedBarcodetag(stocktakeData( _
                        '        StocktakeValues.BarcodeTag _
                        '        ))

                        'Dim exceptionMsg2 As String = String.Empty
                        Dim balanceQty As Integer = StockCardSummaryDB.CheckDeletedData( _
                             stocktakeData(StocktakeValues.BarcodeTag), _
                            exceptionMsg _
                            )

                        If balanceQty > 0 Then
                            'If QrValid = QueryRetValue.ValueTrue Then

                            If Not AlreadyDeleteBarcodetag.Contains(stocktakeData(StocktakeValues.BarcodeTag)) Then
                                alreadyDelcount += 1
                                If alreadyDelcount <= 2 Then
                                    AlreadyDeleteBarcodetag &= stocktakeData(StocktakeValues.BarcodeTag) & vbCrLf
                                End If
                                alreadyDelQty += CDbl(stocktakeData( _
                             StocktakeValues.ScannedQty _
                             ))
                            End If

                        Else
                            connection.Execute( _
                                sqlString, _
                                parameters, _
                                transaction _
                                )
                        End If
                    Next

                    If alreadyDelcount > 2 Then
                        AlreadyDeleteBarcodetag &= "ect"
                    End If
                    'transaction.Commit()
                Catch ex As MySqlException

                    log.Error("InsertData Stock Take DB Error ", ex)

                    If Not transaction Is Nothing Then
                        'transaction.Rollback()
                    End If

                    If ex.Number = 0 Then
                        DisplayMessage.ErrorMsg(ex.Message.ToString, "Error")
                    End If

                    exceptionMsg = ex.Message
                    Return False
                End Try
            End Using

            Return True
        End Function

        Public Function UpdateData( _
           ByVal stocktakeCollection As List(Of String()), _
           ByRef AlreadyDeleteBarcodetag As String, _
           ByRef alreadyDelcount As Integer, _
           ByRef exceptionMsg As String, _
           Optional ByRef alreadyDelQty As Double = 0 _
            ) As Boolean

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim alreadyBarcodetagAll As String = ""

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Dim transaction As IDbTransaction = Nothing



                Try
                    log.Info("UpdateData Stock Take, Open Connection")

                    connection.Open()
                    transaction = connection.BeginTransaction()

                    log.Info("UpdateData Stock Take, Open Connection success")
                    

                    Dim sqlString As String = ( _
                        "UPDATE STOCKTAKE " & _
                        "SET SCANNEDQTY = @SCANNEDQTY, " & _
                            "TAKEDATETIME = @TAKEDATETIME, " & _
                            "REMARKS = '', DELCOLFLG = 0, " & _
                            "APPROVEDFLG = 0, ACCPACSENDFLG = 0, " & _
                            "DELFLAG = 0 " & _
                        "WHERE BARCODETAG = @BARCODETAG " & _
                            "AND YEAR(STOCKTAKEPERIOD) = @YEAR " & _
                            "AND MONTH(STOCKTAKEPERIOD) = @MONTH " & _
                            "AND FINISHTAKE = 0" _
                        )

                    log.Info("UpdateData Stock Take SQL string: " & sqlString)


                    'Dim x As String = ""
                    For Each stocktakeData As String() In stocktakeCollection
                        Dim stocktakeDateTime As String = ( _
                            stocktakeData(StocktakeValues.StocktakePeriod) _
                            )
                        Dim stocktakePeriod As DateTime = ( _
                            DateTime.ParseExact( _
                                stocktakeDateTime, _
                                "yyyy-MM-dd HH:mm:ss", _
                                Nothing _
                                ) _
                            )
                        Dim parameters As Object = New With { _
                            Key .SCANNEDQTY = stocktakeData( _
                                StocktakeValues.ScannedQty _
                                ), _
                            .TAKEDATETIME = stocktakeData( _
                                StocktakeValues.TakeDateTime _
                                ), _
                            .BARCODETAG = stocktakeData( _
                                StocktakeValues.BarcodeTag _
                                ), _
                            .YEAR = stocktakePeriod.Year.ToString(), _
                            .MONTH = stocktakePeriod.Month.ToString() _
                            }

                        'x &= "UPDATE STOCKTAKE " & _
                        '"SET SCANNEDQTY =" & stocktakeData( _
                        '        StocktakeValues.ScannedQty _
                        '        ) & "," & _
                        '    "TAKEDATETIME = '" & stocktakeDateTime & "', " & _
                        '    "REMARKS = '', DELCOLFLG = 0, " & _
                        '    "APPROVEDFLG = 0, ACCPACSENDFLG = 0, " & _
                        '    "DELFLAG = 0 " & _
                        '"WHERE BARCODETAG = '" & stocktakeData( _
                        '        StocktakeValues.BarcodeTag _
                        '        ) & "' " & _
                        '    "AND YEAR(STOCKTAKEPERIOD) = 2017 " & _
                        '    "AND MONTH(STOCKTAKEPERIOD) = 4 " & _
                        '    "AND FINISHTAKE = 0;"

                        'Dim QrValid As QueryRetValue = ProductionActDB.fncCheckDeletedBarcodetag(stocktakeData( _
                        '        StocktakeValues.BarcodeTag _
                        '        ))

                        Dim balanceQty As Integer = StockCardSummaryDB.CheckDeletedData( _
                         stocktakeData(StocktakeValues.BarcodeTag), _
                        exceptionMsg _
                        )

                        If balanceQty >= 0 Then
                            If balanceQty > 0 Then
                                If Not alreadyBarcodetagAll.Contains(stocktakeData(StocktakeValues.BarcodeTag)) Then
                                    alreadyBarcodetagAll &= stocktakeData(StocktakeValues.BarcodeTag) & vbCrLf
                                    alreadyDelcount += 1
                                    If alreadyDelcount <= 2 Then
                                        AlreadyDeleteBarcodetag &= stocktakeData(StocktakeValues.BarcodeTag) & vbCrLf
                                    End If
                                    alreadyDelQty += CDbl(stocktakeData( _
                                    StocktakeValues.ScannedQty _
                                    ))
                                End If
                            Else
                                connection.Execute( _
                                    sqlString, _
                                    parameters, _
                                    transaction _
                                    )
                            End If
                        End If
                    Next

                    If alreadyDelcount > 2 And Not AlreadyDeleteBarcodetag.Contains("ect") Then
                        AlreadyDeleteBarcodetag &= "ect"
                    End If


                    transaction.Commit()
                Catch ex As MySqlException

                    log.Error("UpdateData Stock Take DB Error ", ex)

                    If Not transaction Is Nothing Then
                        'transaction.Rollback()
                    End If

                    If ex.Number = 0 Then
                        DisplayMessage.ErrorMsg(ex.Message.ToString, "Error")
                    End If

                    exceptionMsg = ex.Message
                    Return False
                End Try
            End Using

            Return True
        End Function

        Public Function DeleteData( _
                ByVal stocktakeCollection As List(Of String()), _
                ByRef exceptionMsg As String _
                ) As Boolean

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Dim transaction As IDbTransaction = Nothing

                Try
                    log.Info("DeleteData Stock Take, Open Connection")

                    connection.Open()
                    transaction = connection.BeginTransaction

                    log.Info("DeleteData Stock Take, Open Connection success")

                    Dim sqlString As String = _
                        "DELETE FROM STOCKTAKE " & _
                        "WHERE TRINPARTNO=@TRINPARTNO " & _
                            "AND BARCODETAG=@BARCODETAG " & _
                            "AND SCANNEDQTY=@SCANNEDQTY " & _
                            "AND STOCKTAKEPERIOD=@STOCKTAKEPERIOD " & _
                            "AND TAKEDATETIME=@TAKEDATETIME " & _
                            "AND ADDCOLFLG = 1"

                    log.Info("DeleteData Stock Take SQL string: " & sqlString)

                    For Each stocktakeData As String() In stocktakeCollection
                        Dim stocktakeDateTime As String = ( _
                            stocktakeData(StocktakeValues.StocktakePeriod) _
                            )
                        Dim stocktakePeriod As DateTime = ( _
                            DateTime.ParseExact( _
                                stocktakeDateTime, _
                                "yyyy-MM-dd HH:mm:ss", _
                                Nothing _
                                ) _
                            )
                        Dim parameters As Object = New With { _
                            Key .TRINPARTNO = stocktakeData( _
                                StocktakeValues.TrinPartNo _
                                ), _
                            .BARCODETAG = stocktakeData( _
                                StocktakeValues.BarcodeTag _
                                ), _
                            .SCANNEDQTY = stocktakeData( _
                                StocktakeValues.ScannedQty _
                                ), _
                            .STOCKTAKEPERIOD = stocktakeData( _
                                StocktakeValues.StocktakePeriod _
                                ), _
                            .TAKEDATETIME = stocktakeData( _
                                StocktakeValues.TakeDateTime _
                                ) _
                            }

                        connection.Execute( _
                            sqlString, _
                            parameters, _
                            transaction _
                            )
                    Next

                    transaction.Commit()
                Catch ex As MySqlException

                    log.Error("DeleteData Stock Take DB Error ", ex)

                    If Not transaction Is Nothing Then
                        transaction.Rollback()
                    End If

                    If ex.Number = 0 Then
                        DisplayMessage.ErrorMsg(ex.Message.ToString, "Error")
                    End If

                    exceptionMsg = ex.Message
                    Return False
                End Try
            End Using

            Return True
        End Function
    End Module
End Namespace
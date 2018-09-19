Option Strict On
Option Explicit On

Imports System.Data
Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass
Imports log4net


Namespace Model
    Module ProductionActClassificationDB
        Public Function fncGetBusinessDay() As Date
            Dim productionActClassification As List(Of ProductionActClassification) = Nothing
            Dim str_Date As Date

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    log.Info("fncGetBusinessDay, Open connection")

                    connection.Open()

                    log.Info("fncGetBusinessDay, Open connection success")

                    Dim sqlString As String = "SELECT BUSINESSDAY FROM BUSINESSDAY"

                    log.Info("fncGetBusinessDay SQL string: " & sqlString)

                    productionActClassification = CType(connection.Query(Of ProductionActClassification)(sqlString), List(Of ProductionActClassification))

                    log.Info("fncGetBusinessDay result " & productionActClassification.Count())

                Catch ex As Exception
                    log.Error("fncGetBusinessDay DB Error", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using
            LogManager.Shutdown()

            If Not productionActClassification Is Nothing Then
                For Each prodActClassItems As ProductionActClassification In productionActClassification
                    str_Date = prodActClassItems.BUSINESSDAY
                Next
            End If

            Return str_Date
        End Function

        Public Function fncGetBarcodeValLine(ByVal str_BarcodeVal As String) As List(Of ProductionActClassification)
            Dim productionActClassification As List(Of ProductionActClassification) = Nothing

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    log.Info("fncGetBarcodeValLine, Open connection")

                    connection.Open()

                    log.Info("fncGetBarcodeValLine, Open connection success")

                    Dim sqlString As String = "SELECT pa.BARCODETAG, pa.ACTQTY, lm.BARCODELINE " & _
                                              "FROM PRODUCTIONACT AS pa INNER JOIN LINEMASTER AS lm " & _
                                              "ON pa.LINECODE = lm.LINECODE WHERE pa.BARCODETAG = @BARCODETAG AND pa.DELFLAG=0 AND pa.OKNG=1"

                    log.Info("fncGetBarcodeValLine SQL string: " & sqlString)

                    productionActClassification = CType(connection.Query(Of ProductionActClassification)(sqlString, New With {Key .BARCODETAG = str_BarcodeVal}), List(Of ProductionActClassification))

                    log.Info("fncGetBarcodeValLine result " & productionActClassification.Count())

                Catch ex As Exception
                    log.Error("fncGetBarcodeValLine DB Error", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using
            LogManager.Shutdown()

            Return productionActClassification
        End Function

        Public Function fncGetLastBarcodeCountValue(ByVal str_DateVal As String, ByVal str_BarcodeLine As String, ByVal str_ID As String) As String
            Dim productionActClassification As ProductionActClassification = Nothing

            Dim str_BarcodeCount As String = String.Empty
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    log.Info("fncGetLastBarcodeCountValue, Open connection")

                    connection.Open()

                    log.Info("fncGetLastBarcodeCountValue, Open connection success")

                    Dim sqlString As String = "SELECT mid(BARCODETAG, 11) AS COUNTING " & _
                                              "FROM PRODUCTIONACT WHERE " & _
                                              "BARCODETAG LIKE @DATEPROD AND MID(BARCODETAG,1,1) = @BARCODELINE AND DELFLAG = '0' AND OKNG='1' AND MID(BARCODETAG,10,1)='" & str_ID & "'" & _
                                              "ORDER BY COUNTING DESC LIMIT 1"

                    log.Info("fncGetLastBarcodeCountValue SQL string: " & sqlString)

                    productionActClassification = connection.Query(Of ProductionActClassification)(sqlString, New With {Key .DATEPROD = str_DateVal, .BARCODELINE = str_BarcodeLine, .ID = str_ID}).FirstOrDefault

                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not productionActClassification Is Nothing Then
                str_BarcodeCount = productionActClassification.COUNTING
            End If

            log.Info("fncGetLastBarcodeCountValue result " & str_BarcodeCount)
            LogManager.Shutdown()

            Return str_BarcodeCount
        End Function

        Public Function fncInsertTagClassification(ByVal str_TagClassification As String()) As Integer
            Dim int_ActID As Integer
            Dim str_InsertSqlBuilder As New System.Text.StringBuilder
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            str_InsertSqlBuilder.AppendLine("INSERT IGNORE PRODUCTIONACT ")
            str_InsertSqlBuilder.AppendLine(" (")
            str_InsertSqlBuilder.AppendLine(" ACTID, FINALID, BARCODETAG, PRODDATE, TRINPARTNO, LINECODE, OKNG, ACTQTY, PRINTFLG, ACCPACSENDFLG, USERID, QRCODE, IMGFILE, REMARKS, DELFLAG, SPLITFLG")
            str_InsertSqlBuilder.AppendLine(" ) ")
            str_InsertSqlBuilder.AppendLine(" VALUES ")
            str_InsertSqlBuilder.AppendLine(" (")
            str_InsertSqlBuilder.AppendLine(" 0, @FINALID, @BARCODETAG, @PRODDATE, @TRINPARTNO, @LINECODE, 1, @ACTQTY, 0, 1, @USERID, @QRCODE, @IMGFILE, @REMARKS, 0, 1")
            str_InsertSqlBuilder.AppendLine(" );")
            str_InsertSqlBuilder.AppendLine("SELECT CAST(LAST_INSERT_ID() AS UNSIGNED INTEGER)")

            Dim str_InsertSql = str_InsertSqlBuilder.ToString

            Using obj_DBConnection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    log.Info("fncInsertTagClassification, Open connection")

                    obj_DBConnection.Open()

                    log.Info("fncInsertTagClassification, Open connection success")

                    log.Info("fncInsertTagClassification SQL string: " & str_InsertSql)

                    int_ActID = CInt(obj_DBConnection.Query(Of ULong)(str_InsertSql, New With {Key .FINALID = str_TagClassification(0), .BARCODETAG = str_TagClassification(1), _
                                                                                               .PRODDATE = str_TagClassification(2), .TRINPARTNO = str_TagClassification(3), _
                                                                                               .LINECODE = str_TagClassification(4), .ACTQTY = str_TagClassification(5), _
                                                                                               .USERID = str_TagClassification(6), .QRCODE = str_TagClassification(7), _
                                                                                               .IMGFILE = str_TagClassification(8), .REMARKS = str_TagClassification(9) _
                                                                                          }).SingleOrDefault)
                    log.Info("fncInsertTagClassification result " & int_ActID)

                Catch ex As Exception
                    log.Error("fncInsertTagClassification DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using
            LogManager.Shutdown()

            Return int_ActID
        End Function

        Public Function fncInsertTagClassificationUseTransaction(ByVal str_TagClassification As String(), ByVal obj_DBConnection As IDbConnection) As Integer
            Dim int_ActID As Integer
            Dim str_InsertSqlBuilder As New System.Text.StringBuilder
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            str_InsertSqlBuilder.AppendLine("INSERT IGNORE PRODUCTIONACT ")
            str_InsertSqlBuilder.AppendLine(" (")
            str_InsertSqlBuilder.AppendLine(" ACTID, FINALID, BARCODETAG, PRODDATE, TRINPARTNO, LINECODE, OKNG, ACTQTY, PRINTFLG, ACCPACSENDFLG, USERID, QRCODE, IMGFILE, REMARKS, DELFLAG,DELSENDFLG, SPLITFLG")
            str_InsertSqlBuilder.AppendLine(" ) ")
            str_InsertSqlBuilder.AppendLine(" VALUES ")
            str_InsertSqlBuilder.AppendLine(" (")
            str_InsertSqlBuilder.AppendLine(" 0, @FINALID, @BARCODETAG, @PRODDATE, @TRINPARTNO, @LINECODE, 1, @ACTQTY, 0, 1, @USERID, @QRCODE, @IMGFILE, @REMARKS, 1,1, 1")
            str_InsertSqlBuilder.AppendLine(" );")
            str_InsertSqlBuilder.AppendLine("SELECT CAST(LAST_INSERT_ID() AS UNSIGNED INTEGER)")

            Dim str_InsertSql = str_InsertSqlBuilder.ToString

            'Using obj_DBConnection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
            Try
                '    obj_DBConnection.Open()
                log.Info("fncInsertTagClassificationUseTransaction, Open connection from fncProcessSplit")
                log.Info("fncInsertTagClassificationUseTransaction SQL string: " & str_InsertSql)

                int_ActID = CInt(obj_DBConnection.Query(Of ULong)(str_InsertSql, New With {Key .FINALID = str_TagClassification(0), .BARCODETAG = str_TagClassification(1), _
                                                                                           .PRODDATE = str_TagClassification(2), .TRINPARTNO = str_TagClassification(3), _
                                                                                           .LINECODE = str_TagClassification(4), .ACTQTY = str_TagClassification(5), _
                                                                                           .USERID = str_TagClassification(6), .QRCODE = str_TagClassification(7), _
                                                                                           .IMGFILE = str_TagClassification(8), .REMARKS = str_TagClassification(9) _
                                                                                      }).SingleOrDefault)
            Catch ex As Exception
                log.Error("fncInsertTagClassificationUseTransaction DB Error", ex)
                DisplayMessage.ErrorMsg(ex.Message, "DB Error")
            End Try
            'End Using

            log.Info("fncInsertTagClassificationUseTransaction result " & int_ActID)
            LogManager.Shutdown()
            Return int_ActID
        End Function

        Public Function fncUpdateOldTag(ByVal str_Barcode As String, _
                                        ByVal int_SplitFlg As Int16 _
                                        ) As Boolean
            Dim bool_Upd As Boolean = False
            Dim str_UpdateSqlBuilder As New System.Text.StringBuilder
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            str_UpdateSqlBuilder.AppendLine("UPDATE PRODUCTIONACT ")
            str_UpdateSqlBuilder.AppendLine(" SET")
            str_UpdateSqlBuilder.AppendLine(" DELFLAG = 1, ")
            str_UpdateSqlBuilder.AppendLine(" DELSENDFLG = 1 ")
            If int_SplitFlg = 1 Then
                str_UpdateSqlBuilder.AppendLine(" , SPLITFLG = 1 ")
            Else
                str_UpdateSqlBuilder.AppendLine(" , SPLITFLG = 2 ")
            End If
            str_UpdateSqlBuilder.AppendLine(" WHERE")
            str_UpdateSqlBuilder.AppendLine(" BARCODETAG = @BARCODETAG AND DELFLAG = 0")

            Dim str_UpdateSql As String = str_UpdateSqlBuilder.ToString
            Dim int_UpdRowCheck As Integer

            Using obj_DBConnection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim obj_DBTransaction As IDbTransaction = Nothing
                Try
                    log.Info("fncUpdateOldTag, Open connection")

                    obj_DBConnection.Open()

                    log.Info("fncUpdateOldTag, Open connection success")
                    log.Info("fncUpdateOldTag SQL string: " & str_UpdateSql)

                    obj_DBTransaction = obj_DBConnection.BeginTransaction
                    int_UpdRowCheck = obj_DBConnection.Execute(str_UpdateSql, New With {Key .BARCODETAG = str_Barcode}, obj_DBTransaction)

                    If int_UpdRowCheck > 0 Then
                        bool_Upd = True
                        obj_DBTransaction.Commit()
                    End If
                    log.Info("fncUpdateOldTag result " & bool_Upd)
                Catch ex As Exception
                    If Not obj_DBTransaction Is Nothing Then
                        obj_DBTransaction.Rollback()
                        log.Error("fncUpdateOldTag DB Error", ex)
                        DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                    End If
                End Try
            End Using
            LogManager.Shutdown()
            Return bool_Upd
        End Function

        Public Function fncUpdateOldTagUseTransaction(ByVal str_Barcode As String, _
                                        ByVal int_SplitFlg As Int16, _
                                        ByVal obj_DBConnection As IDbConnection, _
                                        ByVal obj_DBTransaction As IDbTransaction _
                                        ) As Boolean
            Dim bool_Upd As Boolean = False
            Dim str_UpdateSqlBuilder As New System.Text.StringBuilder
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            str_UpdateSqlBuilder.AppendLine("UPDATE PRODUCTIONACT ")
            str_UpdateSqlBuilder.AppendLine(" SET")
            str_UpdateSqlBuilder.AppendLine(" DELFLAG = 1, ")
            str_UpdateSqlBuilder.AppendLine(" DELSENDFLG = 1 ")
            If int_SplitFlg = 1 Then
                str_UpdateSqlBuilder.AppendLine(" , SPLITFLG = 1 ")
            Else
                str_UpdateSqlBuilder.AppendLine(" , SPLITFLG = 2 ")
            End If
            str_UpdateSqlBuilder.AppendLine(" WHERE")
            str_UpdateSqlBuilder.AppendLine(" BARCODETAG = @BARCODETAG AND DELFLAG = 0")

            Dim str_UpdateSql As String = str_UpdateSqlBuilder.ToString
            Dim int_UpdRowCheck As Integer

            'Using obj_DBConnection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
            'Dim obj_DBTransaction As IDbTransaction = Nothing
            Try
                log.Info("fncUpdateOldTagUseTransaction, Open connection from fncProcessSplit")
                log.Info("fncUpdateOldTagUseTransaction SQL string: " & str_UpdateSql)
                'obj_DBConnection.Open()
                'obj_DBTransaction = obj_DBConnection.BeginTransaction
                int_UpdRowCheck = obj_DBConnection.Execute(str_UpdateSql, New With {Key .BARCODETAG = str_Barcode}, obj_DBTransaction)

                If int_UpdRowCheck > 0 Then
                    bool_Upd = True
                    '   obj_DBTransaction.Commit()
                End If
                log.Info("fncUpdateOldTagUseTransaction result " & bool_Upd)
            Catch ex As Exception
                'If Not obj_DBTransaction Is Nothing Then
                'obj_DBTransaction.Rollback()
                log.Error("fncUpdateOldTagUseTransaction DB Error", ex)
                DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                'End If
            End Try
            'End Using
            LogManager.Shutdown()
            Return bool_Upd
        End Function

        Public Function fncUpdateReturnOldTag(ByVal str_Barcode As String, _
                                              ByVal int_SplitFlg As Int16 _
                                              ) As Boolean
            Dim bool_Upd As Boolean = False
            Dim str_UpdateSqlBuilder As New System.Text.StringBuilder
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            str_UpdateSqlBuilder.AppendLine("UPDATE PRODUCTIONACT ")
            str_UpdateSqlBuilder.AppendLine(" SET")
            str_UpdateSqlBuilder.AppendLine(" DELFLAG = 0, ")
            str_UpdateSqlBuilder.AppendLine(" SPLITFLG = " & int_SplitFlg & " ")
            str_UpdateSqlBuilder.AppendLine(" WHERE")
            str_UpdateSqlBuilder.AppendLine(" BARCODETAG = @BARCODETAG AND DELFLAG = 1")

            Dim str_UpdateSql As String = str_UpdateSqlBuilder.ToString
            Dim int_UpdRowCheck As Integer

            Using obj_DBConnection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim obj_DBTransaction As IDbTransaction = Nothing
                Try
                    log.Info("fncUpdateReturnOldTag, Open connection")

                    obj_DBConnection.Open()

                    log.Info("fncUpdateReturnOldTag, Open connection success")
                    log.Info("fncUpdateReturnOldTag SQL string: " & str_UpdateSql)

                    obj_DBTransaction = obj_DBConnection.BeginTransaction
                    int_UpdRowCheck = obj_DBConnection.Execute(str_UpdateSql, New With {Key .BARCODETAG = str_Barcode}, obj_DBTransaction)

                    If int_UpdRowCheck > 0 Then
                        bool_Upd = True
                        obj_DBTransaction.Commit()
                    End If
                    log.Info("fncUpdateReturnOldTag result " & bool_Upd)
                Catch ex As Exception
                    If Not obj_DBTransaction Is Nothing Then
                        obj_DBTransaction.Rollback()
                        log.Error("fncUpdateReturnOldTag DB Error", ex)
                        DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                    End If
                End Try
            End Using
            LogManager.Shutdown()
            Return bool_Upd
        End Function

        Public Function fncInsertStockCardTagClassification(ByVal str_TagClassification As String()) As Boolean
            Dim bool_Ins As Boolean = False
            Dim str_InsertSqlBuilder As New System.Text.StringBuilder
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            str_InsertSqlBuilder.AppendLine("INSERT IGNORE STOCK_CARD ")
            str_InsertSqlBuilder.AppendLine(" (")
            str_InsertSqlBuilder.AppendLine(" TRINPARTNO, ACTID, STOCK_IN, STOCK_OUT, REMARK, TYPE_ID, BARCODETAG, LINECODE, DATE_TIME, USERID, DELFLAG, STOCKTAKEFLG")
            str_InsertSqlBuilder.AppendLine(" ) ")
            str_InsertSqlBuilder.AppendLine(" VALUES ")
            str_InsertSqlBuilder.AppendLine(" (")
            str_InsertSqlBuilder.AppendLine(" @TRINPARTNO, @ACTID, @STOCK_IN, 0, @REMARK, 5, @BARCODETAG, @LINECODE, @DATE_TIME, @USERID, 0,0")
            str_InsertSqlBuilder.AppendLine(" )")

            Dim str_InsertSql = str_InsertSqlBuilder.ToString
            Dim int_InsertRowCheck As Integer

            Using obj_DBConnection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim obj_DBTransaction As IDbTransaction = Nothing
                Try
                    log.Info("fncInsertStockCardTagClassification, Open connection")

                    obj_DBConnection.Open()

                    log.Info("fncInsertStockCardTagClassification, Open connection success")
                    log.Info("fncInsertStockCardTagClassification SQL string: " & str_InsertSql)
                    obj_DBTransaction = obj_DBConnection.BeginTransaction
                    int_InsertRowCheck = obj_DBConnection.Execute(str_InsertSql, New With {Key .TRINPARTNO = str_TagClassification(0), .ACTID = str_TagClassification(1), _
                                                                                               .STOCK_IN = str_TagClassification(2), .REMARK = str_TagClassification(3), _
                                                                                               .BARCODETAG = str_TagClassification(4), .LINECODE = str_TagClassification(5), _
                                                                                               .DATE_TIME = str_TagClassification(6), .USERID = str_TagClassification(7) _
                                                                                          }, obj_DBTransaction)
                    If int_InsertRowCheck > 0 Then
                        bool_Ins = True

                        obj_DBTransaction.Commit()
                    End If
                    log.Info("fncInsertStockCardTagClassification result " & bool_Ins)
                Catch ex As Exception
                    If Not obj_DBTransaction Is Nothing Then
                        obj_DBTransaction.Rollback()
                        log.Error("fncInsertStockCardTagClassification DB Error", ex)
                        DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                    End If
                End Try
            End Using
            LogManager.Shutdown()
            Return bool_Ins
        End Function



        Public Function fncInsertStockCardTagClassificationUseTransaction(ByVal str_TagClassification As String(), ByVal obj_DBConnection As IDbConnection, ByVal obj_DBTransaction As IDbTransaction) As Boolean
            Dim bool_Ins As Boolean = False
            Dim str_InsertSqlBuilder As New System.Text.StringBuilder
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            str_InsertSqlBuilder.AppendLine("INSERT IGNORE STOCK_CARD ")
            str_InsertSqlBuilder.AppendLine(" (")
            str_InsertSqlBuilder.AppendLine(" TRINPARTNO, ACTID, STOCK_IN, STOCK_OUT, REMARK, TYPE_ID, BARCODETAG, LINECODE, DATE_TIME, USERID, DELFLAG, STOCKTAKEFLG")
            str_InsertSqlBuilder.AppendLine(" ) ")
            str_InsertSqlBuilder.AppendLine(" VALUES ")
            str_InsertSqlBuilder.AppendLine(" (")
            str_InsertSqlBuilder.AppendLine(" @TRINPARTNO, @ACTID, @STOCK_IN, 0, @REMARK, 5, @BARCODETAG, @LINECODE, @DATE_TIME, @USERID, 1,0")
            str_InsertSqlBuilder.AppendLine(" )")

            Dim str_InsertSql = str_InsertSqlBuilder.ToString
            Dim int_InsertRowCheck As Integer

            'Using obj_DBConnection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
            'Dim obj_DBTransaction As IDbTransaction = Nothing
            Try
                log.Info("fncInsertStockCardTagClassificationUseTransaction, Open connection from fncProcessSplit")
                'obj_DBConnection.Open()
                log.Info("fncInsertStockCardTagClassificationUseTransaction SQL string: " & str_InsertSql)
                'obj_DBTransaction = obj_DBConnection.BeginTransaction
                int_InsertRowCheck = obj_DBConnection.Execute(str_InsertSql, New With {Key .TRINPARTNO = str_TagClassification(0), .ACTID = str_TagClassification(1), _
                                                                                           .STOCK_IN = str_TagClassification(2), .REMARK = str_TagClassification(3), _
                                                                                           .BARCODETAG = str_TagClassification(4), .LINECODE = str_TagClassification(5), _
                                                                                           .DATE_TIME = str_TagClassification(6), .USERID = str_TagClassification(7) _
                                                                                      }, obj_DBTransaction)
                If int_InsertRowCheck > 0 Then
                    bool_Ins = True

                    'obj_DBTransaction.Commit()
                End If
                log.Info("fncGetTRfncInsertStockCardTagClassificationUseTransactionINPartNo result " & bool_Ins)
            Catch ex As Exception
                'If Not obj_DBTransaction Is Nothing Then
                'obj_DBTransaction.Rollback()
                log.Error("fncInsertStockCardTagClassificationUseTransaction DB Error", ex)
                DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                'End If
            End Try
            'End Using
            LogManager.Shutdown()
            Return bool_Ins
        End Function


        Public Function fncUpdateStockCardTagClassification(ByVal str_Barcode As String) As Boolean
            Dim bool_Upd As Boolean = False
            Dim str_UpdateSqlBuilder As New System.Text.StringBuilder
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            str_UpdateSqlBuilder.AppendLine("UPDATE STOCK_CARD ")
            str_UpdateSqlBuilder.AppendLine(" SET")
            str_UpdateSqlBuilder.AppendLine(" DELFLAG = 1")
            str_UpdateSqlBuilder.AppendLine(" WHERE")
            str_UpdateSqlBuilder.AppendLine(" BARCODETAG = @BARCODETAG AND DELFLAG = 0")

            Dim str_UpdateSql As String = str_UpdateSqlBuilder.ToString
            Dim int_UpdRowCheck As Integer

            Using obj_DBConnection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim obj_DBTransaction As IDbTransaction = Nothing
                Try
                    log.Info("fncUpdateStockCardTagClassification, Open connection")

                    obj_DBConnection.Open()

                    log.Info("fncUpdateStockCardTagClassification, Open connection success")

                    log.Info("fncUpdateStockCardTagClassification SQL string: " & str_UpdateSql)

                    obj_DBTransaction = obj_DBConnection.BeginTransaction
                    int_UpdRowCheck = obj_DBConnection.Execute(str_UpdateSql, New With {Key .BARCODETAG = str_Barcode}, obj_DBTransaction)

                    If int_UpdRowCheck > 0 Then
                        bool_Upd = True
                        obj_DBTransaction.Commit()
                    End If
                    log.Info("fncUpdateStockCardTagClassification result " & bool_Upd)
                Catch ex As Exception
                    If Not obj_DBTransaction Is Nothing Then
                        obj_DBTransaction.Rollback()
                        log.Error("fncUpdateStockCardTagClassification DB Error", ex)
                        DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                    End If
                End Try
            End Using
            LogManager.Shutdown()
            Return bool_Upd
        End Function

        Public Function fncUpdateStockCardTagClassificationUseTransaction(ByVal str_Barcode As String, ByVal obj_DBConnection As IDbConnection, ByVal obj_DBTransaction As IDbTransaction) As Boolean
            Dim bool_Upd As Boolean = False
            Dim str_UpdateSqlBuilder As New System.Text.StringBuilder
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            str_UpdateSqlBuilder.AppendLine("UPDATE STOCK_CARD ")
            str_UpdateSqlBuilder.AppendLine(" SET")
            str_UpdateSqlBuilder.AppendLine(" DELFLAG = 1")
            str_UpdateSqlBuilder.AppendLine(" WHERE")
            str_UpdateSqlBuilder.AppendLine(" BARCODETAG = @BARCODETAG AND DELFLAG = 0")

            Dim str_UpdateSql As String = str_UpdateSqlBuilder.ToString
            Dim int_UpdRowCheck As Integer

            'Using obj_DBConnection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
            'Dim obj_DBTransaction As IDbTransaction = Nothing
            Try
                log.Info("fncUpdateStockCardTagClassificationUseTransaction, Open connection from fncProcessSplit")
                'obj_DBConnection.Open()
                log.Info("fncUpdateStockCardTagClassificationUseTransaction SQL string: " & str_UpdateSql)
                'obj_DBTransaction = obj_DBConnection.BeginTransaction
                int_UpdRowCheck = obj_DBConnection.Execute(str_UpdateSql, New With {Key .BARCODETAG = str_Barcode}, obj_DBTransaction)

                If int_UpdRowCheck > 0 Then
                    bool_Upd = True
                    '   obj_DBTransaction.Commit()
                End If
                log.Info("fncUpdateStockCardTagClassificationUseTransaction result " & bool_Upd)
            Catch ex As Exception
                'If Not obj_DBTransaction Is Nothing Then
                'obj_DBTransaction.Rollback()
                DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                'End If
            End Try
            'End Using
            LogManager.Shutdown()
            Return bool_Upd
        End Function


        Public Function fncGetFinalID(ByVal str_QRCode As String) As Int32
            Dim prodActClassification As ProductionActClassification = Nothing
            Dim int_FinalID As Int32 = 0
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    log.Info("fncGetFinalID, Open connection")

                    connection.Open()

                    log.Info("fncGetFinalID, Open connection success")

                    Dim sqlString As String = "SELECT FINALID FROM PRODUCTIONACT WHERE QRCODE = @QRCODE AND DELFLAG=0"

                    log.Info("fncGetFinalID SQL string: " & sqlString)

                    prodActClassification = connection.Query(Of ProductionActClassification)(sqlString, New With {Key .QRCODE = str_QRCode}).FirstOrDefault


                Catch ex As Exception
                    log.Error("fncGetFinalID DB Error", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActClassification Is Nothing Then
                int_FinalID = prodActClassification.FINALID
            End If

            log.Info("fncGetFinalID result " & int_FinalID)
            LogManager.Shutdown()
            Return int_FinalID
        End Function

        Public Function fncGetUserID(ByVal str_QRCode As String) As String
            Dim prodActClassification As ProductionActClassification = Nothing
            Dim str_UserID As String = String.Empty
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    log.Info("fncGetUserID, Open connection")

                    connection.Open()

                    log.Info("fncGetUserID, Open connection success")

                    Dim sqlString As String = "SELECT USERID FROM PRODUCTIONACT WHERE QRCODE = @QRCODE AND DELFLAG=0"

                    log.Info("fncGetUserID SQL string: " & sqlString)

                    prodActClassification = connection.Query(Of ProductionActClassification)(sqlString, New With {Key .QRCODE = str_QRCode}).FirstOrDefault
                Catch ex As Exception
                    log.Error("fncGetUserID DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActClassification Is Nothing Then
                str_UserID = prodActClassification.USERID
            End If

            log.Info("fncGetUserID result " & str_UserID)
            LogManager.Shutdown()

            Return str_UserID
        End Function

        Public Function DeleteData(ByVal str_ActId As Int32) As Boolean
            Dim success As Boolean = False
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim obj_DBTransaction As IDbTransaction = Nothing
                Try
                    log.Info("DeleteData, Open connection")

                    connection.Open()

                    log.Info("DeleteData, Open connection success")

                    Dim sqlString As String = "DELETE FROM PRODUCTIONACT WHERE FIND_IN_SET(ACTID, @ACTIDS)"

                    log.Info("DeleteData SQL string: " & sqlString)

                    obj_DBTransaction = connection.BeginTransaction
                    Dim rowsAffected As Integer = connection.Execute( _
                     sqlString, New With {Key .ACTIDS = str_ActId}, obj_DBTransaction)

                    If rowsAffected > 0 Then
                        success = True

                        obj_DBTransaction.Commit()
                    End If

                    log.Info("DeleteData result " & success)
                Catch ex As Exception
                    If Not obj_DBTransaction Is Nothing Then
                        obj_DBTransaction.Rollback()
                        log.Error("DeleteData DB Error", ex)
                        DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                    End If
                End Try
            End Using
            LogManager.Shutdown()
            Return success
        End Function

        'Add DeleteDataStockCard 9i
        Public Function DeleteDataStockCard(ByVal str_ActId As Int32) As Boolean
            Dim success As Boolean = False
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim obj_DBTransaction As IDbTransaction = Nothing
                Try
                    log.Info("DeleteDataStockCard, Open connection")

                    connection.Open()

                    log.Info("DeleteDataStockCard, Open connection success")

                    Dim sqlString As String = "DELETE FROM STOCK_CARD WHERE FIND_IN_SET(ACTID, @ACTIDS) AND " & _
                                             "TYPE_ID = 5"

                    log.Info("DeleteDataStockCard SQL string: " & sqlString)

                    obj_DBTransaction = connection.BeginTransaction
                    Dim rowsAffected As Integer = connection.Execute( _
                     sqlString, New With {Key .ACTIDS = str_ActId}, obj_DBTransaction)

                    If rowsAffected > 0 Then
                        success = True

                        obj_DBTransaction.Commit()
                    End If
                    log.Info("DeleteDataStockCard result " & success)
                Catch ex As Exception
                    If Not obj_DBTransaction Is Nothing Then
                        obj_DBTransaction.Rollback()
                        log.Error("DeleteDataStockCard DB Error", ex)
                        DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                    End If
                End Try
            End Using
            LogManager.Shutdown()

            Return success
        End Function


        Public Function fncGetStdQty(ByVal str_TrinPartNo As String) As Int16
            Dim int_Qty2 As Int16 = 0
            Dim prodActClassification As ProductionActClassification = Nothing
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    log.Info("fncGetStdQty, Open connection")

                    connection.Open()

                    log.Info("fncGetStdQty, Open connection success")

                    Dim str_SqlStringBuild As New System.Text.StringBuilder

                    str_SqlStringBuild.AppendLine("SELECT ")
                    str_SqlStringBuild.AppendLine(" case PACKAGESTANDARD2 when null then")
                    str_SqlStringBuild.AppendLine(" 	PACKAGESTANDARD1")
                    str_SqlStringBuild.AppendLine(" else")
                    str_SqlStringBuild.AppendLine(" 	PACKAGESTANDARD2")
                    str_SqlStringBuild.AppendLine(" end as PACKAGESTANDARD")
                    str_SqlStringBuild.AppendLine(" FROM PARTMASTER WHERE TRINPARTNO = @TRINPARTNO")

                    Dim sqlString As String = str_SqlStringBuild.ToString
                    log.Info("fncGetStdQty SQL string: " & sqlString)
                    prodActClassification = connection.Query(Of ProductionActClassification)(sqlString, New With {Key .TRINPARTNO = str_TrinPartNo}).SingleOrDefault
                Catch ex As Exception
                    log.Error("fncGetStdQty DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActClassification Is Nothing Then
                int_Qty2 = prodActClassification.PACKAGESTANDARD
            End If
            log.Info("fncGetStdQty result " & int_Qty2)
            LogManager.Shutdown()

            Return int_Qty2
        End Function

        Public Function fncGetLine(ByVal str_LineCode As String) As String
            Dim prodActClassification As ProductionActClassification = Nothing
            Dim str_Line As String = String.Empty
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    log.Info("fncGetLine, Open connection")

                    connection.Open()

                    log.Info("fncGetLine, Open connection success")


                    Dim sqlString As String = "SELECT BARCODELINE FROM LINEMASTER WHERE LINECODE = @LINECODE"

                    log.Info("fncGetLine SQL string: " & sqlString)

                    prodActClassification = connection.Query(Of ProductionActClassification)(sqlString, New With {Key .LINECODE = str_LineCode}).FirstOrDefault
                Catch ex As Exception
                    log.Error("fncGetLine DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActClassification Is Nothing Then
                str_Line = prodActClassification.BARCODELINE
            End If
            log.Info("fncGetLine result " & str_Line)
            LogManager.Shutdown()
            Return str_Line
        End Function

        Public Function fncGetUser(ByVal str_BarcodeVal As String) As String
            Dim prodActClassification As ProductionActClassification = Nothing
            Dim str_Line As String = String.Empty
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    log.Info("fncGetUser, Open connection")

                    connection.Open()

                    log.Info("fncGetUser, Open connection success")

                    Dim sqlString As String = "SELECT USERID FROM STOCK_CARD WHERE BARCODETAG = @BARCODETAG " & _
                                              "AND TYPE_ID IN (1,4,5)"

                    log.Info("fncGetUser SQL string: " & sqlString)

                    prodActClassification = connection.Query(Of ProductionActClassification)(sqlString, New With {Key .BARCODETAG = str_BarcodeVal}).FirstOrDefault
                Catch ex As Exception
                    log.Error("fncGetUser DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActClassification Is Nothing Then
                str_Line = prodActClassification.USERID
            End If
            log.Info("fncGetUser result " & str_Line)
            LogManager.Shutdown()
            Return str_Line
        End Function

        Public Function fncGetDateTime(ByVal str_BarcodeVal As String) As String
            Dim prodActClassification As ProductionActClassification = Nothing
            Dim str_Line As String = String.Empty
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    log.Info("fncGetDateTime, Open connection")

                    connection.Open()

                    log.Info("fncGetDateTime, Open connection success")

                    Dim sqlString As String = "SELECT DATE_TIME FROM STOCK_CARD WHERE BARCODETAG = @BARCODETAG " & _
                                              "AND TYPE_ID IN (1,4,5)"

                    log.Info("fncGetDateTime SQL string: " & sqlString)

                    prodActClassification = connection.Query(Of ProductionActClassification)(sqlString, New With {Key .BARCODETAG = str_BarcodeVal}).FirstOrDefault
                Catch ex As Exception
                    log.Error("fncGetDateTime DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActClassification Is Nothing Then
                str_Line = prodActClassification.DATE_TIME.ToString("yyyy-MM-dd HH:mm:ss")
            End If

            log.Info("fncGetDateTime result " & str_Line)
            LogManager.Shutdown()


            Return str_Line
        End Function

        Public Function fncGetSplitFlag(ByVal str_Barcodetag As String) As Int16
            Dim prodActIntegration As ProductionActIntegration = Nothing
            Dim int_SplitFlag As Int16 = 0
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    log.Info("fncGetSplitFlag, Open connection")

                    connection.Open()

                    log.Info("fncGetSplitFlag, Open connection success")

                    Dim sqlString As String = "SELECT SPLITFLG FROM PRODUCTIONACT WHERE BARCODETAG = @BARCODE AND DELFLAG=0"
                    log.Info("fncGetSplitFlag SQL string: " & sqlString)
                    prodActIntegration = connection.Query(Of ProductionActIntegration)(sqlString, New With {Key .BARCODE = str_Barcodetag}).FirstOrDefault
                Catch ex As Exception
                    log.Error("fncGetSplitFlag DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActIntegration Is Nothing Then
                int_SplitFlag = prodActIntegration.SPLITFLG
            End If
            log.Info("fncGetSplitFlag result " & int_SplitFlag)
            LogManager.Shutdown()

            Return int_SplitFlag
        End Function

    End Module
End Namespace

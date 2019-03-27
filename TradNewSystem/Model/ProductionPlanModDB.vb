Option Strict On
Option Explicit On

Imports System.Data
Imports System.Text
Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass
Imports log4net

Namespace Model
    Module ProductionPlanModDB

        Public Function fncGetFinalID() As Integer
            Dim prodPlanMod As ProductionPlanMod = Nothing
            Dim int_LastFinalID As Integer = 0
            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    'log.Info("fncGetFinalID, Open connection")
                    connection.Open()
                    'log.Info("fncGetFinalID, Open connection success")

                    Dim sqlString As String = "SELECT MAX(FINALID) AS ACTID FROM PRODUCTIONPLAN WHERE DELFLAG=0"
                    'log.Info("fncGetFinalID SQL string: " & sqlString)

                    prodPlanMod = connection.Query(Of ProductionPlanMod)(sqlString).SingleOrDefault
                    'log.Info("fncGetFinalID not get result ")

                Catch ex As Exception
                    'log.Error("fncGetFinalID DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodPlanMod Is Nothing Then
                int_LastFinalID = prodPlanMod.FINALID
            End If
            'log.Info("fncGetFinalID result " & int_LastFinalID)
            'LogManager.Shutdown()

            Return int_LastFinalID
        End Function

        Public Function fncInsertProdPlanMod(ByVal str_ProdPlanData As String()) As Integer
            Dim int_FinalId As Integer
            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim str_InsertSqlBuilder As New System.Text.StringBuilder

            str_InsertSqlBuilder.AppendLine("INSERT PRODUCTIONPLAN")
            str_InsertSqlBuilder.AppendLine(" (")
            str_InsertSqlBuilder.AppendLine(" FINALID, LINECODE, PRODDATE, ENDTIME, TRINPARTNO, PERIOD, PLANQTY, IMPORTDATE, DELFLAG, TRINMODFLAG, MODBARCODE, OLDPRODDATE")
            str_InsertSqlBuilder.AppendLine(" ) ")
            str_InsertSqlBuilder.AppendLine(" VALUES ")
            str_InsertSqlBuilder.AppendLine(" (")
            str_InsertSqlBuilder.AppendLine(" 0, @LINECODE, @PRODDATE, @ENDTIME, @TRINPARTNO, @PERIOD, @PLANQTY, @IMPORTDATE, 0, 1, @MODBARCODE, @OLDPRODDATE")
            str_InsertSqlBuilder.AppendLine(" );")
            str_InsertSqlBuilder.AppendLine("SELECT CAST(LAST_INSERT_ID() AS UNSIGNED INTEGER)")

            Dim str_InsertSql = str_InsertSqlBuilder.ToString
            'log.Info("fncInsertProdPlanMod SQL string: " & str_InsertSql)

            Using obj_DBConnection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    'log.Info("fncInsertProdPlanMod, Open connection")
                    obj_DBConnection.Open()
                    'log.Info("fncInsertProdPlanMod, Open connection success")

                    int_FinalId = CInt(obj_DBConnection.Query(Of ULong)(str_InsertSql, New With {Key .LINECODE = str_ProdPlanData(0), .PRODDATE = str_ProdPlanData(1), _
                                                                                               .ENDTIME = str_ProdPlanData(2), .TRINPARTNO = str_ProdPlanData(3), _
                                                                                               .PERIOD = str_ProdPlanData(4), .PLANQTY = str_ProdPlanData(5), _
                                                                                               .IMPORTDATE = str_ProdPlanData(6), .MODBARCODE = str_ProdPlanData(7), _
                                                                                               .OLDPRODDATE = str_ProdPlanData(8) _
                                                                                          }).DefaultIfEmpty(0).FirstOrDefault)
                    'log.Info("fncInsertProdPlanMod result " & int_FinalId.ToString())

                Catch ex As Exception
                    'log.Error("fncInsertProdPlanMod DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using
            'LogManager.Shutdown()

            Return int_FinalId
        End Function

        Public Function fncUpdateProdActMod(ByVal str_QrCode As String, ByVal int_DelFlagUpd As Int16, ByVal int_DelFlagReturn As Int16) As Boolean
            Dim bool_Upd As Boolean = False
            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim str_UpdSqlBuilder As New StringBuilder

            str_UpdSqlBuilder.AppendLine("UPDATE PRODUCTIONACT")
            str_UpdSqlBuilder.AppendLine(" SET")
            str_UpdSqlBuilder.AppendLine(" DELFLAG = @DELFLAG1 ")
            ' remark by ver 9.h
            'str_UpdSqlBuilder.AppendLine(" DELSENDFLG = 1")
            str_UpdSqlBuilder.AppendLine(" WHERE")
            str_UpdSqlBuilder.AppendLine(" QRCODE = @QRCODE AND DELFLAG = @DELFLAG2")

            Dim str_UpdSql As String = str_UpdSqlBuilder.ToString
            'log.Info("fncUpdateProdActMod SQL string: " & str_UpdSql)

            Dim int_UpdRowCheck As Integer

            Using obj_DBConnection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim obj_DBTransaction As IDbTransaction = Nothing
                Try
                    'log.Info("fncUpdateProdActMod, Open connection")
                    obj_DBConnection.Open()
                    'log.Info("fncUpdateProdActMod, Open connection success")

                    obj_DBTransaction = obj_DBConnection.BeginTransaction
                    int_UpdRowCheck = obj_DBConnection.Execute(str_UpdSql, New With {Key .QRCODE = str_QrCode, .DELFLAG1 = int_DelFlagUpd, _
                                                                                         .DELFLAG2 = int_DelFlagReturn}, _
                                                                                         obj_DBTransaction)
                    'log.Info("fncUpdateProdActMod result " & int_UpdRowCheck.ToString())

                    If int_UpdRowCheck > 0 Then
                        bool_Upd = True
                        obj_DBTransaction.Commit()
                    End If
                Catch ex As Exception
                    If Not obj_DBTransaction Is Nothing Then
                        obj_DBTransaction.Rollback()
                        'log.Error("fncUpdateProdActMod DB Error", ex)
                        DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                    End If
                End Try
            End Using
            'LogManager.Shutdown()

            Return bool_Upd
        End Function

        Public Function fncGetEndDate(ByVal str_TRINNo As String) As ProductionPlanMod
            Dim productionPlanMod As ProductionPlanMod = Nothing
            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim str_EndDate As String = String.Empty
            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    'log.Info("fncGetEndDate, Open connection")
                    connection.Open()
                    'log.Info("fncGetEndDate, Open connection success")

                    Dim sqlString As String = "SELECT ENDTIME FROM PRODUCTIONPLAN WHERE " & _
                                              "TRINPARTNO = @TRINPARTNO " & _
                                              "ORDER BY ENDTIME DESC LIMIT 1"
                    'log.Info("fncGetEndDate SQL string: " & sqlString)

                    productionPlanMod = connection.Query(Of ProductionPlanMod)(sqlString, New With {Key .TRINPARTNO = str_TRINNo}).FirstOrDefault
                    'log.Info("fncGetEndDate not get result ")

                Catch ex As Exception
                    'log.Error("fncGetEndDate DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using
            'LogManager.Shutdown()

            Return productionPlanMod
        End Function

        Public Function fncUpdateStockCardTagModification(ByVal str_Barcode As String, ByVal int_DelFlagUpd As Int16, ByVal int_DelFlagReturn As Int16) As Boolean
            Dim bool_Upd As Boolean = False
            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim str_UpdateSqlBuilder As New System.Text.StringBuilder

            str_UpdateSqlBuilder.AppendLine("UPDATE STOCK_CARD ")
            str_UpdateSqlBuilder.AppendLine(" SET")
            str_UpdateSqlBuilder.AppendLine(" DELFLAG = @DELFLAG1")
            str_UpdateSqlBuilder.AppendLine(" WHERE")
            str_UpdateSqlBuilder.AppendLine(" BARCODETAG = @BARCODETAG AND DELFLAG = @DELFLAG2")

            Dim str_UpdateSql As String = str_UpdateSqlBuilder.ToString
            'log.Info("fncUpdateStockCardTagModification SQL string: " & str_UpdateSql)

            Dim int_UpdRowCheck As Integer

            Using obj_DBConnection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim obj_DBTransaction As IDbTransaction = Nothing
                Try
                    'log.Info("fncUpdateStockCardTagModification, Open connection")
                    obj_DBConnection.Open()
                    'log.Info("fncUpdateStockCardTagModification, Open connection success")

                    obj_DBTransaction = obj_DBConnection.BeginTransaction
                    int_UpdRowCheck = obj_DBConnection.Execute(str_UpdateSql, New With {Key .BARCODETAG = str_Barcode, .DELFLAG1 = int_DelFlagUpd, _
                                                                                         .DELFLAG2 = int_DelFlagReturn}, _
                                                                                         obj_DBTransaction)
                    'log.Info("fncUpdateStockCardTagModification result " & int_UpdRowCheck.ToString())

                    If int_UpdRowCheck > 0 Then
                        bool_Upd = True
                        obj_DBTransaction.Commit()
                    End If
                Catch ex As Exception
                    If Not obj_DBTransaction Is Nothing Then
                        obj_DBTransaction.Rollback()
                        'log.Error("fncUpdateStockCardTagModification DB Error", ex)
                        DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                    End If
                End Try
            End Using
            'LogManager.Shutdown()

            Return bool_Upd
        End Function

        Public Function fncGetBarcodeValLine(ByVal str_QRVal As String) As List(Of ProductionPlanMod)
            Dim productionPlanMod As List(Of ProductionPlanMod) = Nothing
            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    'log.Info("fncGetBarcodeValLine, Open connection")
                    connection.Open()
                    'log.Info("fncGetBarcodeValLine, Open connection success")

                    Dim sqlString As String = "SELECT pa.BARCODETAG, pa.ACTQTY, lm.BARCODELINE " & _
                                              "FROM PRODUCTIONACT AS pa INNER JOIN LINEMASTER AS lm " & _
                                              "ON pa.LINECODE = lm.LINECODE WHERE pa.QRCODE = @QRCODE AND pa.DELFLAG=0 AND pa.OKNG=1 " & _
                                              "LIMIT 1"
                    'log.Info("fncGetBarcodeValLine SQL string: " & sqlString)

                    productionPlanMod = CType(connection.Query(Of ProductionPlanMod)(sqlString, New With {Key .QRCODE = str_QRVal}), List(Of ProductionPlanMod))
                    'log.Info("fncGetBarcodeValLine result " & productionPlanMod.Count())

                Catch ex As Exception
                    'log.Error("fncGetBarcodeValLine DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using
            'LogManager.Shutdown()

            Return productionPlanMod
        End Function

        Public Function DeleteData(ByVal str_FinalId As Int32, ByVal str_TrinPartNo As String) As Boolean
            Dim success As Boolean = False
            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim obj_DBTransaction As IDbTransaction = Nothing
                Try
                    'log.Info("DeleteData, Open connection")
                    connection.Open()
                    'log.Info("DeleteData, Open connection success")

                    Dim sqlString As String = "DELETE FROM PRODUCTIONPLAN WHERE FIND_IN_SET(FINALID, @FINALIDS) AND " & _
                                              "TRINPARTNO = @TRINPARTNO"
                    'log.Info("DeleteData SQL string: " & sqlString)

                    obj_DBTransaction = connection.BeginTransaction
                    Dim rowsAffected As Integer = connection.Execute( _
                     sqlString, New With {Key .FINALIDS = str_FinalId, .TRINPARTNO = str_TrinPartNo}, obj_DBTransaction)
                    'log.Info("DeleteData result " & rowsAffected.ToString())

                    If rowsAffected > 0 Then
                        success = True

                        obj_DBTransaction.Commit()
                    End If
                Catch ex As Exception
                    If Not obj_DBTransaction Is Nothing Then
                        obj_DBTransaction.Rollback()
                        'log.Error("DeleteData DB Error", ex)
                        DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                    End If
                End Try
            End Using
            'LogManager.Shutdown()

            Return success
        End Function

        Public Function fncGetUser(ByVal str_BarcodeVal As String) As String
            Dim prodPlanMod As ProductionPlanMod = Nothing
            Dim str_Line As String = String.Empty
            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    'log.Info("fncGetUser, Open connection")
                    connection.Open()
                    'log.Info("fncGetUser, Open connection success")

                    Dim sqlString As String = "SELECT USERID FROM STOCK_CARD WHERE BARCODETAG = @BARCODETAG " & _
                                              "AND TYPE_ID IN (1,4,5)"
                    'log.Info("fncGetUser SQL string: " & sqlString)

                    prodPlanMod = connection.Query(Of ProductionPlanMod)(sqlString, New With {Key .BARCODETAG = str_BarcodeVal}).FirstOrDefault
                    'log.Info("fncGetUser not get result ")

                Catch ex As Exception
                    'log.Error("fncGetUser DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using
            'LogManager.Shutdown()

            If Not prodPlanMod Is Nothing Then
                str_Line = prodPlanMod.USERID
            End If
            'log.Info("fncGetUser result " & str_Line)
            'LogManager.Shutdown()

            Return str_Line
        End Function

        Public Function fncGetDateTime(ByVal str_BarcodeVal As String) As String
            Dim prodPlanMod As ProductionPlanMod = Nothing
            Dim str_Line As String = String.Empty
            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    'log.Info("fncGetDateTime, Open connection")
                    connection.Open()
                    'log.Info("fncGetDateTime, Open connection success")

                    Dim sqlString As String = "SELECT DATE_TIME FROM STOCK_CARD WHERE BARCODETAG = @BARCODETAG " & _
                                              "AND TYPE_ID IN (1,4,5)"
                    'log.Info("fncGetDateTime SQL string: " & sqlString)

                    prodPlanMod = connection.Query(Of ProductionPlanMod)(sqlString, New With {Key .BARCODETAG = str_BarcodeVal}).FirstOrDefault
                    'log.Info("fncGetDateTime not get result ")

                Catch ex As Exception
                    'log.Error("fncGetDateTime DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using
            'LogManager.Shutdown()

            If Not prodPlanMod Is Nothing Then
                str_Line = prodPlanMod.DATE_TIME.ToString("yyyy-MM-dd hh:MM:ss")
            End If
            'log.Info("fncGetDateTime result " & str_Line)
            'LogManager.Shutdown()

            Return str_Line
        End Function


    End Module
End Namespace

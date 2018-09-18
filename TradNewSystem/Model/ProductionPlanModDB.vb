Option Strict On
Option Explicit On

Imports System.Data
Imports System.Text
Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass

Namespace Model
    Module ProductionPlanModDB

        Public Function fncGetFinalID() As Integer
            Dim prodPlanMod As ProductionPlanMod = Nothing
            Dim int_LastFinalID As Integer = 0

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
            Try
                connection.Open()

                Dim sqlString As String = "SELECT MAX(FINALID) AS ACTID FROM PRODUCTIONPLAN WHERE DELFLAG=0"

                prodPlanMod = connection.Query(Of ProductionPlanMod)(sqlString).SingleOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodPlanMod Is Nothing Then
                int_LastFinalID = prodPlanMod.FINALID
            End If

            Return int_LastFinalID
        End Function

        Public Function fncInsertProdPlanMod(ByVal str_ProdPlanData As String()) As Integer
            Dim int_FinalId As Integer

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

            Using obj_DBConnection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    obj_DBConnection.Open()

                    int_FinalId = CInt(obj_DBConnection.Query(Of ULong)(str_InsertSql, New With {Key .LINECODE = str_ProdPlanData(0), .PRODDATE = str_ProdPlanData(1), _
                                                                                               .ENDTIME = str_ProdPlanData(2), .TRINPARTNO = str_ProdPlanData(3), _
                                                                                               .PERIOD = str_ProdPlanData(4), .PLANQTY = str_ProdPlanData(5), _
                                                                                               .IMPORTDATE = str_ProdPlanData(6), .MODBARCODE = str_ProdPlanData(7), _
                                                                                               .OLDPRODDATE = str_ProdPlanData(8) _
                                                                                          }).DefaultIfEmpty(0).FirstOrDefault)
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return int_FinalId
        End Function

        Public Function fncUpdateProdActMod(ByVal str_QrCode As String, ByVal int_DelFlagUpd As Int16, ByVal int_DelFlagReturn As Int16) As Boolean
            Dim bool_Upd As Boolean = False

            Dim str_UpdSqlBuilder As New StringBuilder

            str_UpdSqlBuilder.AppendLine("UPDATE PRODUCTIONACT")
            str_UpdSqlBuilder.AppendLine(" SET")
            str_UpdSqlBuilder.AppendLine(" DELFLAG = @DELFLAG1 ")
            ' remark by ver 9.h
            'str_UpdSqlBuilder.AppendLine(" DELSENDFLG = 1")
            str_UpdSqlBuilder.AppendLine(" WHERE")
            str_UpdSqlBuilder.AppendLine(" QRCODE = @QRCODE AND DELFLAG = @DELFLAG2")

            Dim str_UpdSql As String = str_UpdSqlBuilder.ToString
            Dim int_UpdRowCheck As Integer

            Using obj_DBConnection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim obj_DBTransaction As IDbTransaction = Nothing
                Try
                    obj_DBConnection.Open()

                    obj_DBTransaction = obj_DBConnection.BeginTransaction
                    int_UpdRowCheck = obj_DBConnection.Execute(str_UpdSql, New With {Key .QRCODE = str_QrCode, .DELFLAG1 = int_DelFlagUpd, _
                                                                                         .DELFLAG2 = int_DelFlagReturn}, _
                                                                                         obj_DBTransaction)

                    If int_UpdRowCheck > 0 Then
                        bool_Upd = True
                        obj_DBTransaction.Commit()
                    End If
                Catch ex As Exception
                    If Not obj_DBTransaction Is Nothing Then
                        obj_DBTransaction.Rollback()
                        DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                    End If
                End Try
            End Using

            Return bool_Upd
        End Function

        Public Function fncGetEndDate(ByVal str_TRINNo As String) As ProductionPlanMod
            Dim productionPlanMod As ProductionPlanMod = Nothing

            Dim str_EndDate As String = String.Empty
            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT ENDTIME FROM PRODUCTIONPLAN WHERE " & _
                                              "TRINPARTNO = @TRINPARTNO " & _
                                              "ORDER BY ENDTIME DESC LIMIT 1"

                    productionPlanMod = connection.Query(Of ProductionPlanMod)(sqlString, New With {Key .TRINPARTNO = str_TRINNo}).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return productionPlanMod
        End Function

        Public Function fncUpdateStockCardTagModification(ByVal str_Barcode As String, ByVal int_DelFlagUpd As Int16, ByVal int_DelFlagReturn As Int16) As Boolean
            Dim bool_Upd As Boolean = False
            Dim str_UpdateSqlBuilder As New System.Text.StringBuilder

            str_UpdateSqlBuilder.AppendLine("UPDATE STOCK_CARD ")
            str_UpdateSqlBuilder.AppendLine(" SET")
            str_UpdateSqlBuilder.AppendLine(" DELFLAG = @DELFLAG1")
            str_UpdateSqlBuilder.AppendLine(" WHERE")
            str_UpdateSqlBuilder.AppendLine(" BARCODETAG = @BARCODETAG AND DELFLAG = @DELFLAG2")

            Dim str_UpdateSql As String = str_UpdateSqlBuilder.ToString
            Dim int_UpdRowCheck As Integer

            Using obj_DBConnection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim obj_DBTransaction As IDbTransaction = Nothing
                Try
                    obj_DBConnection.Open()

                    obj_DBTransaction = obj_DBConnection.BeginTransaction
                    int_UpdRowCheck = obj_DBConnection.Execute(str_UpdateSql, New With {Key .BARCODETAG = str_Barcode, .DELFLAG1 = int_DelFlagUpd, _
                                                                                         .DELFLAG2 = int_DelFlagReturn}, _
                                                                                         obj_DBTransaction)

                    If int_UpdRowCheck > 0 Then
                        bool_Upd = True
                        obj_DBTransaction.Commit()
                    End If
                Catch ex As Exception
                    If Not obj_DBTransaction Is Nothing Then
                        obj_DBTransaction.Rollback()
                        DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                    End If
                End Try
            End Using

            Return bool_Upd
        End Function

        Public Function fncGetBarcodeValLine(ByVal str_QRVal As String) As List(Of ProductionPlanMod)
            Dim productionPlanMod As List(Of ProductionPlanMod) = Nothing

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT pa.BARCODETAG, pa.ACTQTY, lm.BARCODELINE " & _
                                              "FROM PRODUCTIONACT AS pa INNER JOIN LINEMASTER AS lm " & _
                                              "ON pa.LINECODE = lm.LINECODE WHERE pa.QRCODE = @QRCODE AND pa.DELFLAG=0 AND pa.OKNG=1 " & _
                                              "LIMIT 1"

                    productionPlanMod = CType(connection.Query(Of ProductionPlanMod)(sqlString, New With {Key .QRCODE = str_QRVal}), List(Of ProductionPlanMod))
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return productionPlanMod
        End Function

        Public Function DeleteData(ByVal str_FinalId As Int32, ByVal str_TrinPartNo As String) As Boolean
            Dim success As Boolean = False

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim obj_DBTransaction As IDbTransaction = Nothing
                Try
                    connection.Open()

                    Dim sqlString As String = "DELETE FROM PRODUCTIONPLAN WHERE FIND_IN_SET(FINALID, @FINALIDS) AND " & _
                                              "TRINPARTNO = @TRINPARTNO"

                    obj_DBTransaction = connection.BeginTransaction
                    Dim rowsAffected As Integer = connection.Execute( _
                     sqlString, New With {Key .FINALIDS = str_FinalId, .TRINPARTNO = str_TrinPartNo}, obj_DBTransaction)

                    If rowsAffected > 0 Then
                        success = True

                        obj_DBTransaction.Commit()
                    End If
                Catch ex As Exception
                    If Not obj_DBTransaction Is Nothing Then
                        obj_DBTransaction.Rollback()
                        DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                    End If
                End Try
            End Using

            Return success
        End Function

        Public Function fncGetUser(ByVal str_BarcodeVal As String) As String
            Dim prodPlanMod As ProductionPlanMod = Nothing
            Dim str_Line As String = String.Empty

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT USERID FROM STOCK_CARD WHERE BARCODETAG = @BARCODETAG " & _
                                              "AND TYPE_ID IN (1,4,5)"

                    prodPlanMod = connection.Query(Of ProductionPlanMod)(sqlString, New With {Key .BARCODETAG = str_BarcodeVal}).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodPlanMod Is Nothing Then
                str_Line = prodPlanMod.USERID
            End If

            Return str_Line
        End Function

        Public Function fncGetDateTime(ByVal str_BarcodeVal As String) As String
            Dim prodPlanMod As ProductionPlanMod = Nothing
            Dim str_Line As String = String.Empty

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT DATE_TIME FROM STOCK_CARD WHERE BARCODETAG = @BARCODETAG " & _
                                              "AND TYPE_ID IN (1,4,5)"
                    prodPlanMod = connection.Query(Of ProductionPlanMod)(sqlString, New With {Key .BARCODETAG = str_BarcodeVal}).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodPlanMod Is Nothing Then
                str_Line = prodPlanMod.DATE_TIME.ToString("yyyy-MM-dd hh:MM:ss")
            End If

            Return str_Line
        End Function


    End Module
End Namespace

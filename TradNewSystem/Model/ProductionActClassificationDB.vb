Option Strict On
Option Explicit On

Imports System.Data
Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass

Namespace Model
    Module ProductionActClassificationDB
        Public Function fncGetBusinessDay() As Date
            Dim productionActClassification As List(Of ProductionActClassification) = Nothing
            Dim str_Date As Date

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT BUSINESSDAY FROM BUSINESSDAY"

                    productionActClassification = CType(connection.Query(Of ProductionActClassification)(sqlString), List(Of ProductionActClassification))

                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not productionActClassification Is Nothing Then
                For Each prodActClassItems As ProductionActClassification In productionActClassification
                    str_Date = prodActClassItems.BUSINESSDAY
                Next
            End If

            Return str_Date
        End Function

        Public Function fncGetBarcodeValLine(ByVal str_BarcodeVal As String) As List(Of ProductionActClassification)
            Dim productionActClassification As List(Of ProductionActClassification) = Nothing

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT pa.BARCODETAG, pa.ACTQTY, lm.BARCODELINE " & _
                                              "FROM PRODUCTIONACT AS pa INNER JOIN LINEMASTER AS lm " & _
                                              "ON pa.LINECODE = lm.LINECODE WHERE pa.BARCODETAG = @BARCODETAG AND pa.DELFLAG=0 AND pa.OKNG=1"

                    productionActClassification = CType(connection.Query(Of ProductionActClassification)(sqlString, New With {Key .BARCODETAG = str_BarcodeVal}), List(Of ProductionActClassification))
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return productionActClassification
        End Function

        Public Function fncGetLastBarcodeCountValue(ByVal str_DateVal As String, ByVal str_BarcodeLine As String, ByVal str_ID As String) As String
            Dim productionActClassification As ProductionActClassification = Nothing

            Dim str_BarcodeCount As String = String.Empty

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT mid(BARCODETAG, 11) AS COUNTING " & _
                                              "FROM PRODUCTIONACT WHERE " & _
                                              "BARCODETAG LIKE @DATEPROD AND MID(BARCODETAG,1,1) = @BARCODELINE AND DELFLAG = '0' AND OKNG='1' AND MID(BARCODETAG,10,1)='" & str_ID & "'" & _
                                              "ORDER BY COUNTING DESC LIMIT 1"

                    productionActClassification = connection.Query(Of ProductionActClassification)(sqlString, New With {Key .DATEPROD = str_DateVal, .BARCODELINE = str_BarcodeLine, .ID = str_id}).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not productionActClassification Is Nothing Then
                str_BarcodeCount = productionActClassification.COUNTING
            End If

            Return str_BarcodeCount
        End Function

        Public Function fncInsertTagClassification(ByVal str_TagClassification As String()) As Integer
            Dim int_ActID As Integer
            Dim str_InsertSqlBuilder As New System.Text.StringBuilder

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
                    obj_DBConnection.Open()

                    int_ActID = CInt(obj_DBConnection.Query(Of ULong)(str_InsertSql, New With {Key .FINALID = str_TagClassification(0), .BARCODETAG = str_TagClassification(1), _
                                                                                               .PRODDATE = str_TagClassification(2), .TRINPARTNO = str_TagClassification(3), _
                                                                                               .LINECODE = str_TagClassification(4), .ACTQTY = str_TagClassification(5), _
                                                                                               .USERID = str_TagClassification(6), .QRCODE = str_TagClassification(7), _
                                                                                               .IMGFILE = str_TagClassification(8), .REMARKS = str_TagClassification(9) _
                                                                                          }).SingleOrDefault)
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return int_ActID
        End Function

        Public Function fncInsertTagClassificationUseTransaction(ByVal str_TagClassification As String(), ByVal obj_DBConnection As IDbConnection) As Integer
            Dim int_ActID As Integer
            Dim str_InsertSqlBuilder As New System.Text.StringBuilder

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

                int_ActID = CInt(obj_DBConnection.Query(Of ULong)(str_InsertSql, New With {Key .FINALID = str_TagClassification(0), .BARCODETAG = str_TagClassification(1), _
                                                                                           .PRODDATE = str_TagClassification(2), .TRINPARTNO = str_TagClassification(3), _
                                                                                           .LINECODE = str_TagClassification(4), .ACTQTY = str_TagClassification(5), _
                                                                                           .USERID = str_TagClassification(6), .QRCODE = str_TagClassification(7), _
                                                                                           .IMGFILE = str_TagClassification(8), .REMARKS = str_TagClassification(9) _
                                                                                      }).SingleOrDefault)
            Catch ex As Exception
                DisplayMessage.ErrorMsg(ex.Message, "DB Error")
            End Try
            'End Using

            Return int_ActID
        End Function

        Public Function fncUpdateOldTag(ByVal str_Barcode As String, _
                                        ByVal int_SplitFlg As Int16 _
                                        ) As Boolean
            Dim bool_Upd As Boolean = False
            Dim str_UpdateSqlBuilder As New System.Text.StringBuilder

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
                    obj_DBConnection.Open()
                    obj_DBTransaction = obj_DBConnection.BeginTransaction
                    int_UpdRowCheck = obj_DBConnection.Execute(str_UpdateSql, New With {Key .BARCODETAG = str_Barcode}, obj_DBTransaction)

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

        Public Function fncUpdateOldTagUseTransaction(ByVal str_Barcode As String, _
                                        ByVal int_SplitFlg As Int16, _
                                        ByVal obj_DBConnection As IDbConnection, _
                                        ByVal obj_DBTransaction As IDbTransaction _
                                        ) As Boolean
            Dim bool_Upd As Boolean = False
            Dim str_UpdateSqlBuilder As New System.Text.StringBuilder

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
                'obj_DBConnection.Open()
                'obj_DBTransaction = obj_DBConnection.BeginTransaction
                int_UpdRowCheck = obj_DBConnection.Execute(str_UpdateSql, New With {Key .BARCODETAG = str_Barcode}, obj_DBTransaction)

                If int_UpdRowCheck > 0 Then
                    bool_Upd = True
                    '   obj_DBTransaction.Commit()
                End If
            Catch ex As Exception
                'If Not obj_DBTransaction Is Nothing Then
                'obj_DBTransaction.Rollback()
                DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                'End If
            End Try
            'End Using

            Return bool_Upd
        End Function

        Public Function fncUpdateReturnOldTag(ByVal str_Barcode As String, _
                                              ByVal int_SplitFlg As Int16 _
                                              ) As Boolean
            Dim bool_Upd As Boolean = False
            Dim str_UpdateSqlBuilder As New System.Text.StringBuilder

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
                    obj_DBConnection.Open()

                    obj_DBTransaction = obj_DBConnection.BeginTransaction
                    int_UpdRowCheck = obj_DBConnection.Execute(str_UpdateSql, New With {Key .BARCODETAG = str_Barcode}, obj_DBTransaction)

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

        Public Function fncInsertStockCardTagClassification(ByVal str_TagClassification As String()) As Boolean
            Dim bool_Ins As Boolean = False
            Dim str_InsertSqlBuilder As New System.Text.StringBuilder

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
                    obj_DBConnection.Open()

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
                Catch ex As Exception
                    If Not obj_DBTransaction Is Nothing Then
                        obj_DBTransaction.Rollback()
                        DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                    End If
                End Try
            End Using

            Return bool_Ins
        End Function



        Public Function fncInsertStockCardTagClassificationUseTransaction(ByVal str_TagClassification As String(), ByVal obj_DBConnection As IDbConnection, ByVal obj_DBTransaction As IDbTransaction) As Boolean
            Dim bool_Ins As Boolean = False
            Dim str_InsertSqlBuilder As New System.Text.StringBuilder

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
                'obj_DBConnection.Open()

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
            Catch ex As Exception
                'If Not obj_DBTransaction Is Nothing Then
                'obj_DBTransaction.Rollback()
                DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                'End If
            End Try
            'End Using

            Return bool_Ins
        End Function


        Public Function fncUpdateStockCardTagClassification(ByVal str_Barcode As String) As Boolean
            Dim bool_Upd As Boolean = False
            Dim str_UpdateSqlBuilder As New System.Text.StringBuilder

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
                    obj_DBConnection.Open()

                    obj_DBTransaction = obj_DBConnection.BeginTransaction
                    int_UpdRowCheck = obj_DBConnection.Execute(str_UpdateSql, New With {Key .BARCODETAG = str_Barcode}, obj_DBTransaction)

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

        Public Function fncUpdateStockCardTagClassificationUseTransaction(ByVal str_Barcode As String, ByVal obj_DBConnection As IDbConnection, ByVal obj_DBTransaction As IDbTransaction) As Boolean
            Dim bool_Upd As Boolean = False
            Dim str_UpdateSqlBuilder As New System.Text.StringBuilder

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
                'obj_DBConnection.Open()

                'obj_DBTransaction = obj_DBConnection.BeginTransaction
                int_UpdRowCheck = obj_DBConnection.Execute(str_UpdateSql, New With {Key .BARCODETAG = str_Barcode}, obj_DBTransaction)

                If int_UpdRowCheck > 0 Then
                    bool_Upd = True
                    '   obj_DBTransaction.Commit()
                End If
            Catch ex As Exception
                'If Not obj_DBTransaction Is Nothing Then
                'obj_DBTransaction.Rollback()
                DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                'End If
            End Try
            'End Using

            Return bool_Upd
        End Function


        Public Function fncGetFinalID(ByVal str_QRCode As String) As Int32
            Dim prodActClassification As ProductionActClassification = Nothing
            Dim int_FinalID As Int32 = 0

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT FINALID FROM PRODUCTIONACT WHERE QRCODE = @QRCODE AND DELFLAG=0"

                    prodActClassification = connection.Query(Of ProductionActClassification)(sqlString, New With {Key .QRCODE = str_QRCode}).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActClassification Is Nothing Then
                int_FinalID = prodActClassification.FINALID
            End If

            Return int_FinalID
        End Function

        Public Function fncGetUserID(ByVal str_QRCode As String) As String
            Dim prodActClassification As ProductionActClassification = Nothing
            Dim str_UserID As String = String.Empty

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT USERID FROM PRODUCTIONACT WHERE QRCODE = @QRCODE AND DELFLAG=0"

                    prodActClassification = connection.Query(Of ProductionActClassification)(sqlString, New With {Key .QRCODE = str_QRCode}).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActClassification Is Nothing Then
                str_UserID = prodActClassification.USERID
            End If

            Return str_UserID
        End Function

        Public Function DeleteData(ByVal str_ActId As Int32) As Boolean
            Dim success As Boolean = False

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim obj_DBTransaction As IDbTransaction = Nothing
                Try
                    connection.Open()

                    Dim sqlString As String = "DELETE FROM PRODUCTIONACT WHERE FIND_IN_SET(ACTID, @ACTIDS)"

                    obj_DBTransaction = connection.BeginTransaction
                    Dim rowsAffected As Integer = connection.Execute( _
                     sqlString, New With {Key .ACTIDS = str_ActId}, obj_DBTransaction)

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

        'Add DeleteDataStockCard 9i
        Public Function DeleteDataStockCard(ByVal str_ActId As Int32) As Boolean
            Dim success As Boolean = False

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim obj_DBTransaction As IDbTransaction = Nothing
                Try
                    connection.Open()

                    Dim sqlString As String = "DELETE FROM STOCK_CARD WHERE FIND_IN_SET(ACTID, @ACTIDS) AND " & _
                                             "TYPE_ID = 5"

                    obj_DBTransaction = connection.BeginTransaction
                    Dim rowsAffected As Integer = connection.Execute( _
                     sqlString, New With {Key .ACTIDS = str_ActId}, obj_DBTransaction)

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


        Public Function fncGetStdQty(ByVal str_TrinPartNo As String) As Int16
            Dim int_Qty2 As Int16 = 0
            Dim prodActClassification As ProductionActClassification = Nothing

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim str_SqlStringBuild As New System.Text.StringBuilder

                    str_SqlStringBuild.AppendLine("SELECT ")
                    str_SqlStringBuild.AppendLine(" case PACKAGESTANDARD2 when null then")
                    str_SqlStringBuild.AppendLine(" 	PACKAGESTANDARD1")
                    str_SqlStringBuild.AppendLine(" else")
                    str_SqlStringBuild.AppendLine(" 	PACKAGESTANDARD2")
                    str_SqlStringBuild.AppendLine(" end as PACKAGESTANDARD")
                    str_SqlStringBuild.AppendLine(" FROM PARTMASTER WHERE TRINPARTNO = @TRINPARTNO")

                    Dim sqlString As String = str_SqlStringBuild.ToString

                    prodActClassification = connection.Query(Of ProductionActClassification)(sqlString, New With {Key .TRINPARTNO = str_TrinPartNo}).SingleOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActClassification Is Nothing Then
                int_Qty2 = prodActClassification.PACKAGESTANDARD
            End If

            Return int_Qty2
        End Function

        Public Function fncGetLine(ByVal str_LineCode As String) As String
            Dim prodActClassification As ProductionActClassification = Nothing
            Dim str_Line As String = String.Empty

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT BARCODELINE FROM LINEMASTER WHERE LINECODE = @LINECODE"

                    prodActClassification = connection.Query(Of ProductionActClassification)(sqlString, New With {Key .LINECODE = str_LineCode}).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActClassification Is Nothing Then
                str_Line = prodActClassification.BARCODELINE
            End If

            Return str_Line
        End Function

        Public Function fncGetUser(ByVal str_BarcodeVal As String) As String
            Dim prodActClassification As ProductionActClassification = Nothing
            Dim str_Line As String = String.Empty

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT USERID FROM STOCK_CARD WHERE BARCODETAG = @BARCODETAG " & _
                                              "AND TYPE_ID IN (1,4,5)"

                    prodActClassification = connection.Query(Of ProductionActClassification)(sqlString, New With {Key .BARCODETAG = str_BarcodeVal}).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActClassification Is Nothing Then
                str_Line = prodActClassification.USERID
            End If

            Return str_Line
        End Function

        Public Function fncGetDateTime(ByVal str_BarcodeVal As String) As String
            Dim prodActClassification As ProductionActClassification = Nothing
            Dim str_Line As String = String.Empty

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT DATE_TIME FROM STOCK_CARD WHERE BARCODETAG = @BARCODETAG " & _
                                              "AND TYPE_ID IN (1,4,5)"
                    prodActClassification = connection.Query(Of ProductionActClassification)(sqlString, New With {Key .BARCODETAG = str_BarcodeVal}).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActClassification Is Nothing Then
                str_Line = prodActClassification.DATE_TIME.ToString("yyyy-MM-dd HH:mm:ss")
            End If

            Return str_Line
        End Function

        Public Function fncGetSplitFlag(ByVal str_Barcodetag As String) As Int16
            Dim prodActIntegration As ProductionActIntegration = Nothing
            Dim int_SplitFlag As Int16 = 0

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT SPLITFLG FROM PRODUCTIONACT WHERE BARCODETAG = @BARCODE AND DELFLAG=0"

                    prodActIntegration = connection.Query(Of ProductionActIntegration)(sqlString, New With {Key .BARCODE = str_Barcodetag}).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActIntegration Is Nothing Then
                int_SplitFlag = prodActIntegration.SPLITFLG
            End If

            Return int_SplitFlag
        End Function

    End Module
End Namespace

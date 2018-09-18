Option Strict On
Option Explicit On

Imports System.Data
Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass

Namespace Model
    Module ProductionActIntegrationDB
        Public Function fncGetBusinessDay() As Date
            Dim productionActIntegration As List(Of ProductionActIntegration) = Nothing
            Dim str_Date As Date

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT BUSINESSDAY FROM BUSINESSDAY"

                    productionActIntegration = CType(connection.Query(Of ProductionActIntegration)(sqlString), List(Of ProductionActIntegration))

                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not productionActIntegration Is Nothing Then
                For Each prodActItems As ProductionActIntegration In productionActIntegration
                    str_Date = prodActItems.BUSINESSDAY
                Next
            End If

            Return str_Date
        End Function

        Public Function fncGetBarcodeValLine(ByVal str_QRVal As String) As List(Of ProductionActIntegration)
            Dim productionActIntegration As List(Of ProductionActIntegration) = Nothing

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT pa.BARCODETAG, pa.ACTQTY, lm.BARCODELINE " & _
                                              "FROM PRODUCTIONACT AS pa INNER JOIN LINEMASTER AS lm " & _
                                              "ON pa.LINECODE = lm.LINECODE WHERE pa.QRCODE = @QRCODE AND pa.DELFLAG=0 AND pa.OKNG=1 " & _
                                              "LIMIT 1"

                    productionActIntegration = CType(connection.Query(Of ProductionActIntegration)(sqlString, New With {Key .QRCODE = str_QRVal}), List(Of ProductionActIntegration))
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return productionActIntegration
        End Function

        Public Function fncGetLastBarcodeCountValue(ByVal str_DateVal As String, ByVal str_BarcodeLine As String, ByVal str_ID As String) As String
            Dim productionActIntegration As ProductionActIntegration = Nothing

            Dim str_BarcodeCount As String = String.Empty

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    ' modify 9H
                    Dim sqlString As String = "SELECT mid(BARCODETAG, 11) AS COUNTING " & _
                                              "FROM PRODUCTIONACT WHERE " & _
                                              "BARCODETAG LIKE @DATEPROD AND MID(BARCODETAG,1,1) = @BARCODELINE AND DELFLAG = '0' AND OKNG='1' AND MID(BARCODETAG,10,1)='" & str_ID & "'" & _
                                              "ORDER BY COUNTING DESC LIMIT 1"

                    productionActIntegration = connection.Query(Of ProductionActIntegration)(sqlString, New With {Key .DATEPROD = str_DateVal, .BARCODELINE = str_BarcodeLine}).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not productionActIntegration Is Nothing Then
                str_BarcodeCount = productionActIntegration.COUNTING
            End If

            Return str_BarcodeCount
        End Function

        Public Function fncInsertTagIntegration(ByVal str_TagIntegration As String()) As Integer
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

                    int_ActID = CInt(obj_DBConnection.Query(Of ULong)(str_InsertSql, New With {Key .FINALID = str_TagIntegration(0), .BARCODETAG = str_TagIntegration(1), _
                                                                                               .PRODDATE = str_TagIntegration(2), .TRINPARTNO = str_TagIntegration(3), _
                                                                                               .LINECODE = str_TagIntegration(4), .ACTQTY = str_TagIntegration(5), _
                                                                                               .USERID = str_TagIntegration(6), .QRCODE = str_TagIntegration(7), _
                                                                                               .IMGFILE = str_TagIntegration(8), .REMARKS = str_TagIntegration(9) _
                                                                                          }).DefaultIfEmpty(0).FirstOrDefault)
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return int_ActID
        End Function

        Public Function fncInsertTagIntegrationUseTransaction(ByVal str_TagIntegration As String(), ByVal obj_DBConnection As IDbConnection) As Integer
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
                '        obj_DBConnection.Open()

                int_ActID = CInt(obj_DBConnection.Query(Of ULong)(str_InsertSql, New With {Key .FINALID = str_TagIntegration(0), .BARCODETAG = str_TagIntegration(1), _
                                                                                           .PRODDATE = str_TagIntegration(2), .TRINPARTNO = str_TagIntegration(3), _
                                                                                           .LINECODE = str_TagIntegration(4), .ACTQTY = str_TagIntegration(5), _
                                                                                           .USERID = str_TagIntegration(6), .QRCODE = str_TagIntegration(7), _
                                                                                           .IMGFILE = str_TagIntegration(8), .REMARKS = str_TagIntegration(9) _
                                                                                      }).DefaultIfEmpty(0).FirstOrDefault)
            Catch ex As Exception
                DisplayMessage.ErrorMsg(ex.Message, "DB Error")
            End Try
            'End Using

            Return int_ActID
        End Function

        Public Function fncUpdateOldTag(ByVal str_Barcode As String, _
                                        ByVal str_DelFlagVal As Integer, _
                                        ByVal str_DelFlagOld As Integer, _
                                        ByVal int_SplitFlg As Int16) As Boolean
            Dim bool_Upd As Boolean = False
            Dim str_UpdateSqlBuilder As New System.Text.StringBuilder

            str_UpdateSqlBuilder.AppendLine("UPDATE PRODUCTIONACT ")
            str_UpdateSqlBuilder.AppendLine(" SET")
            str_UpdateSqlBuilder.AppendLine(" DELFLAG = @DELFLAG1, ")
            str_UpdateSqlBuilder.AppendLine(" DELSENDFLG = 1 ")
            If int_SplitFlg = 1 And str_DelFlagVal = 1 Then
                str_UpdateSqlBuilder.AppendLine(" , SPLITFLG = 1 ")
            ElseIf str_DelFlagVal = 0 Then
                str_UpdateSqlBuilder.AppendLine(" , SPLITFLG = " & int_SplitFlg & " ")
            ElseIf int_SplitFlg = 0 And str_DelFlagVal = 1 Then
                str_UpdateSqlBuilder.AppendLine(" , SPLITFLG = 2 ")
            End If
            str_UpdateSqlBuilder.AppendLine(" WHERE")
            str_UpdateSqlBuilder.AppendLine(" BARCODETAG = @BARCODETAG AND DELFLAG = @DELFLAG2")

            Dim str_UpdateSql As String = str_UpdateSqlBuilder.ToString
            Dim int_UpdRowCheck As Integer

            Using obj_DBConnection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim obj_DBTransaction As IDbTransaction = Nothing
                Try
                    obj_DBConnection.Open()
                    obj_DBTransaction = obj_DBConnection.BeginTransaction
                    int_UpdRowCheck = obj_DBConnection.Execute(str_UpdateSql, New With {Key .BARCODETAG = str_Barcode, _
                                                                                            .DELFLAG1 = str_DelFlagVal, _
                                                                                            .DELFLAG2 = str_DelFlagOld _
                                                                                        }, obj_DBTransaction)

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

        Public Function fncUpdateOldTagUseTrasaction(ByVal str_Barcode As String, _
                                        ByVal str_DelFlagVal As Integer, _
                                        ByVal str_DelFlagOld As Integer, _
                                        ByVal int_SplitFlg As Int16, _
                                        ByVal obj_DBConnection As IDbConnection, _
                                        ByVal obj_DBTransaction As IDbTransaction) As Boolean

            Dim bool_Upd As Boolean = False
            Dim str_UpdateSqlBuilder As New System.Text.StringBuilder

            str_UpdateSqlBuilder.AppendLine("UPDATE PRODUCTIONACT ")
            str_UpdateSqlBuilder.AppendLine(" SET")
            str_UpdateSqlBuilder.AppendLine(" DELFLAG = @DELFLAG1, ")
            str_UpdateSqlBuilder.AppendLine(" DELSENDFLG = 1 ")
            If int_SplitFlg = 1 And str_DelFlagVal = 1 Then
                str_UpdateSqlBuilder.AppendLine(" , SPLITFLG = 1 ")
            ElseIf str_DelFlagVal = 0 Then
                str_UpdateSqlBuilder.AppendLine(" , SPLITFLG = " & int_SplitFlg & " ")
            ElseIf int_SplitFlg = 0 And str_DelFlagVal = 1 Then
                str_UpdateSqlBuilder.AppendLine(" , SPLITFLG = 2 ")
            End If
            str_UpdateSqlBuilder.AppendLine(" WHERE")
            str_UpdateSqlBuilder.AppendLine(" BARCODETAG = @BARCODETAG AND DELFLAG = @DELFLAG2")

            Dim str_UpdateSql As String = str_UpdateSqlBuilder.ToString
            Dim int_UpdRowCheck As Integer

            Try
                int_UpdRowCheck = obj_DBConnection.Execute(str_UpdateSql, New With {Key .BARCODETAG = str_Barcode, _
                                                                                        .DELFLAG1 = str_DelFlagVal, _
                                                                                        .DELFLAG2 = str_DelFlagOld _
                                                                                    }, obj_DBTransaction)

                If int_UpdRowCheck > 0 Then
                    bool_Upd = True
                End If
            Catch ex As Exception
                DisplayMessage.ErrorMsg(ex.Message, "DB Error")
            End Try


            Return bool_Upd
        End Function


        Public Function fncUpdateReturnOldTag(ByVal str_Barcode As String) As Boolean
            Dim bool_Upd As Boolean = False
            Dim str_UpdateSqlBuilder As New System.Text.StringBuilder

            str_UpdateSqlBuilder.AppendLine("UPDATE PRODUCTIONACT ")
            str_UpdateSqlBuilder.AppendLine(" SET")
            str_UpdateSqlBuilder.AppendLine(" DELFLAG = 0")
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

        Public Function fncInsertStockCardTagIntegration(ByVal str_TagIntegration As String()) As Boolean
            Dim bool_Ins As Boolean = False
            Dim str_InsertSqlBuilder As New System.Text.StringBuilder

            str_InsertSqlBuilder.AppendLine("INSERT IGNORE STOCK_CARD ")
            str_InsertSqlBuilder.AppendLine(" (")
            str_InsertSqlBuilder.AppendLine(" TRINPARTNO, ACTID, STOCK_IN, STOCK_OUT, REMARK, TYPE_ID, BARCODETAG, LINECODE, DATE_TIME, USERID, DELFLAG, STOCKTAKEFLG")
            str_InsertSqlBuilder.AppendLine(" ) ")
            str_InsertSqlBuilder.AppendLine(" VALUES ")
            str_InsertSqlBuilder.AppendLine(" (")
            str_InsertSqlBuilder.AppendLine(" @TRINPARTNO, @ACTID, @STOCK_IN, 0, @REMARK, 4, @BARCODETAG, @LINECODE, @DATE_TIME, @USERID, 0,0")
            str_InsertSqlBuilder.AppendLine(" )")

            Dim str_InsertSql = str_InsertSqlBuilder.ToString
            Dim int_InsertRowCheck As Integer

            Using obj_DBConnection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim obj_DBTransaction As IDbTransaction = Nothing
                Try
                    obj_DBConnection.Open()

                    obj_DBTransaction = obj_DBConnection.BeginTransaction
                    int_InsertRowCheck = obj_DBConnection.Execute(str_InsertSql, New With {Key .TRINPARTNO = str_TagIntegration(0), .ACTID = str_TagIntegration(1), _
                                                                                               .STOCK_IN = str_TagIntegration(2), .REMARK = str_TagIntegration(3), _
                                                                                               .BARCODETAG = str_TagIntegration(4), .LINECODE = str_TagIntegration(5), _
                                                                                               .DATE_TIME = str_TagIntegration(6), .USERID = str_TagIntegration(7) _
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

        Public Function fncInsertStockCardTagIntegrationUsingTransaction(ByVal str_TagIntegration As String(), ByVal obj_DBConnection As IDbConnection, ByVal obj_DBTransaction As IDbTransaction) As Boolean
            Dim bool_Ins As Boolean = False
            Dim str_InsertSqlBuilder As New System.Text.StringBuilder

            str_InsertSqlBuilder.AppendLine("INSERT IGNORE STOCK_CARD ")
            str_InsertSqlBuilder.AppendLine(" (")
            str_InsertSqlBuilder.AppendLine(" TRINPARTNO, ACTID, STOCK_IN, STOCK_OUT, REMARK, TYPE_ID, BARCODETAG, LINECODE, DATE_TIME, USERID, DELFLAG, STOCKTAKEFLG")
            str_InsertSqlBuilder.AppendLine(" ) ")
            str_InsertSqlBuilder.AppendLine(" VALUES ")
            str_InsertSqlBuilder.AppendLine(" (")
            str_InsertSqlBuilder.AppendLine(" @TRINPARTNO, @ACTID, @STOCK_IN, 0, @REMARK, 4, @BARCODETAG, @LINECODE, @DATE_TIME, @USERID, 1,0")
            str_InsertSqlBuilder.AppendLine(" )")

            Dim str_InsertSql = str_InsertSqlBuilder.ToString
            Dim int_InsertRowCheck As Integer

            'Using obj_DBConnection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
            '    Dim obj_DBTransaction As IDbTransaction = Nothing
            Try
                '        obj_DBConnection.Open()

                '        obj_DBTransaction = obj_DBConnection.BeginTransaction
                int_InsertRowCheck = obj_DBConnection.Execute(str_InsertSql, New With {Key .TRINPARTNO = str_TagIntegration(0), .ACTID = str_TagIntegration(1), _
                                                                                           .STOCK_IN = str_TagIntegration(2), .REMARK = str_TagIntegration(3), _
                                                                                           .BARCODETAG = str_TagIntegration(4), .LINECODE = str_TagIntegration(5), _
                                                                                           .DATE_TIME = str_TagIntegration(6), .USERID = str_TagIntegration(7) _
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

        Public Function fncUpdateStockCardTagIntegration(ByVal str_Barcode As String, ByVal str_DelFlagVal As Integer, ByVal str_DelFlagOld As Integer) As Boolean
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
                    int_UpdRowCheck = obj_DBConnection.Execute(str_UpdateSql, New With {Key .BARCODETAG = str_Barcode, .DELFLAG1 = str_DelFlagVal, .DELFLAG2 = str_DelFlagOld}, obj_DBTransaction)

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

        Public Function fncUpdateStockCardTagIntegrationUseTransaction(ByVal str_Barcode As String, _
                                                                       ByVal str_DelFlagVal As Integer, _
                                                                       ByVal str_DelFlagOld As Integer, _
                                                                       ByVal obj_DBConnection As IDbConnection, _
                                                                        ByVal obj_DBTransaction As IDbTransaction) As Boolean
            Dim bool_Upd As Boolean = False
            Dim str_UpdateSqlBuilder As New System.Text.StringBuilder

            str_UpdateSqlBuilder.AppendLine("UPDATE STOCK_CARD ")
            str_UpdateSqlBuilder.AppendLine(" SET")
            str_UpdateSqlBuilder.AppendLine(" DELFLAG = @DELFLAG1")
            str_UpdateSqlBuilder.AppendLine(" WHERE")
            str_UpdateSqlBuilder.AppendLine(" BARCODETAG = @BARCODETAG AND DELFLAG = @DELFLAG2")

            Dim str_UpdateSql As String = str_UpdateSqlBuilder.ToString
            Dim int_UpdRowCheck As Integer

         
            Try
         

                int_UpdRowCheck = obj_DBConnection.Execute(str_UpdateSql, New With {Key .BARCODETAG = str_Barcode, .DELFLAG1 = str_DelFlagVal, .DELFLAG2 = str_DelFlagOld}, obj_DBTransaction)

                If int_UpdRowCheck > 0 Then
                    bool_Upd = True

                End If
            Catch ex As Exception
         
                DisplayMessage.ErrorMsg(ex.Message, "DB Error")

            End Try


            Return bool_Upd
        End Function


        Public Function fncUpdateStockCardTagIntegrationNewtag(ByVal ACTID As Integer, _
                                                               ByVal TypeID As Integer, _
                                                               ByVal obj_DBConnection As IDbConnection, _
                                                               ByVal obj_DBTransaction As IDbTransaction) As Boolean
            Dim bool_Upd As Boolean = False
            Dim str_UpdateSqlBuilder As New System.Text.StringBuilder

            str_UpdateSqlBuilder.AppendLine("UPDATE STOCK_CARD ")
            str_UpdateSqlBuilder.AppendLine(" SET")
            str_UpdateSqlBuilder.AppendLine(" DELFLAG = 0")
            str_UpdateSqlBuilder.AppendLine(" WHERE")
            str_UpdateSqlBuilder.AppendLine(" ACTID = @ACTID AND DELFLAG =1 AND TYPE_ID=@TYPE_ID")

            Dim str_UpdateSql As String = str_UpdateSqlBuilder.ToString
            Dim int_UpdRowCheck As Integer


            Try


                int_UpdRowCheck = obj_DBConnection.Execute(str_UpdateSql, New With {Key .ACTID = ACTID, .TYPE_ID = TypeID}, obj_DBTransaction)

                If int_UpdRowCheck > 0 Then
                    bool_Upd = True

                End If
            Catch ex As Exception

                DisplayMessage.ErrorMsg(ex.Message, "DB Error")

            End Try


            Return bool_Upd
        End Function

        Public Function fncProdactTagIntegrationNewtag(ByVal ACTID As Integer, _
                                                       ByVal obj_DBConnection As IDbConnection, _
                                                       ByVal obj_DBTransaction As IDbTransaction) As Boolean
            Dim bool_Upd As Boolean = False
            Dim str_UpdateSqlBuilder As New System.Text.StringBuilder

            str_UpdateSqlBuilder.AppendLine("UPDATE PRODUCTIONACT ")
            str_UpdateSqlBuilder.AppendLine(" SET")
            str_UpdateSqlBuilder.AppendLine(" DELFLAG = 0, DELSENDFLG=0")
            str_UpdateSqlBuilder.AppendLine(" WHERE")
            str_UpdateSqlBuilder.AppendLine(" ACTID = @ACTID AND DELFLAG =1")

            Dim str_UpdateSql As String = str_UpdateSqlBuilder.ToString
            Dim int_UpdRowCheck As Integer


            Try


                int_UpdRowCheck = obj_DBConnection.Execute(str_UpdateSql, New With {Key .ACTID = ACTID}, obj_DBTransaction)

                If int_UpdRowCheck > 0 Then
                    bool_Upd = True

                End If
            Catch ex As Exception

                DisplayMessage.ErrorMsg(ex.Message, "DB Error")

            End Try


            Return bool_Upd
        End Function

        Public Function fncGetActID() As Int32
            Dim prodActIntegration As ProductionActIntegration = Nothing
            Dim int_LastActID As Int32 = 0

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT MAX(ACTID) AS ACTID FROM PRODUCTIONACT WHERE DELFLAG=0"

                    prodActIntegration = connection.Query(Of ProductionActIntegration)(sqlString).SingleOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActIntegration Is Nothing Then
                int_LastActID = prodActIntegration.ACTID
            End If

            Return int_LastActID
        End Function

        Public Function fncGetFinalID(ByVal str_QRCode As String) As Int32
            Dim prodActIntegration As ProductionActIntegration = Nothing
            Dim int_FinalID As Int32 = 0

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT FINALID FROM PRODUCTIONACT WHERE QRCODE = @QRCODE AND DELFLAG=0"

                    prodActIntegration = connection.Query(Of ProductionActIntegration)(sqlString, New With {Key .QRCODE = str_QRCode}).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActIntegration Is Nothing Then
                int_FinalID = prodActIntegration.FINALID
            End If

            Return int_FinalID
        End Function

        Public Function fncGetUserID(ByVal str_QRCode As String) As String
            Dim prodActIntegration As ProductionActIntegration = Nothing
            Dim str_UserID As String = String.Empty

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT USERID FROM PRODUCTIONACT WHERE QRCODE = @QRCODE AND DELFLAG=0"

                    prodActIntegration = connection.Query(Of ProductionActIntegration)(sqlString, New With {Key .QRCODE = str_QRCode}).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActIntegration Is Nothing Then
                str_UserID = prodActIntegration.USERID
            End If

            Return str_UserID
        End Function

        Public Function DeleteData(ByVal str_ActId As Int32, ByVal str_Barcode As String) As Boolean
            Dim success As Boolean = False

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim obj_DBTransaction As IDbTransaction = Nothing
                Try
                    connection.Open()

                    Dim sqlString As String = "DELETE FROM PRODUCTIONACT WHERE FIND_IN_SET(ACTID, @ACTIDS) AND " & _
                                              "BARCODETAG = @BARCODETAG"

                    obj_DBTransaction = connection.BeginTransaction
                    Dim rowsAffected As Integer = connection.Execute( _
                     sqlString, New With {Key .ACTIDS = str_ActId, .BARCODETAG = str_Barcode}, obj_DBTransaction)

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

        Public Function DeleteDataStockCard(ByVal str_ActId As Int32) As Boolean
            Dim success As Boolean = False

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Dim obj_DBTransaction As IDbTransaction = Nothing
                Try
                    connection.Open()


                    'Modify 9i
                    'Dim sqlString As String = "DELETE FROM STOCK_CARD WHERE FIND_IN_SET(ACTID, @ACTIDS) AND " & _
                    '                          "TYPE_ID = 1"

                    Dim sqlString As String = "DELETE FROM STOCK_CARD WHERE FIND_IN_SET(ACTID, @ACTIDS) AND " & _
                                             "TYPE_ID = 4"

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
            Dim prodActIntegration As ProductionActIntegration = Nothing

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim str_SqlStringBuild As New System.Text.StringBuilder

                    str_SqlStringBuild.AppendLine("SELECT ")
                    str_SqlStringBuild.AppendLine(" case when PACKAGESTANDARD2 is null or")
                    str_SqlStringBuild.AppendLine(" 	PACKAGESTANDARD2 = 0 then")
                    str_SqlStringBuild.AppendLine(" 	PACKAGESTANDARD1")
                    str_SqlStringBuild.AppendLine(" else")
                    str_SqlStringBuild.AppendLine(" 	PACKAGESTANDARD2")
                    str_SqlStringBuild.AppendLine(" end as PACKAGESTANDARD")
                    str_SqlStringBuild.AppendLine(" FROM PARTMASTER WHERE TRINPARTNO = @TRINPARTNO")

                    Dim sqlString As String = str_SqlStringBuild.ToString

                    prodActIntegration = connection.Query(Of ProductionActIntegration)(sqlString, New With {Key .TRINPARTNO = str_TrinPartNo}).SingleOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActIntegration Is Nothing Then
                int_Qty2 = prodActIntegration.PACKAGESTANDARD
            End If

            Return int_Qty2
        End Function

        Public Function fncGetLine(ByVal str_LineCode As String) As String
            Dim prodActIntegration As ProductionActIntegration = Nothing
            Dim str_Line As String = String.Empty

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT BARCODELINE FROM LINEMASTER WHERE LINECODE = @LINECODE"

                    prodActIntegration = connection.Query(Of ProductionActIntegration)(sqlString, New With {Key .LINECODE = str_LineCode}).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActIntegration Is Nothing Then
                str_Line = prodActIntegration.BARCODELINE
            End If

            Return str_Line
        End Function

        Public Function fncGetUser(ByVal str_BarcodeVal As String) As String
            Dim prodActIntegration As ProductionActIntegration = Nothing
            Dim str_Line As String = String.Empty

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT USERID FROM STOCK_CARD WHERE BARCODETAG = @BARCODETAG " & _
                                              "AND TYPE_ID IN (1,4,5)"

                    prodActIntegration = connection.Query(Of ProductionActIntegration)(sqlString, New With {Key .BARCODETAG = str_BarcodeVal}).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActIntegration Is Nothing Then
                str_Line = prodActIntegration.USERID
            End If

            Return str_Line
        End Function

        Public Function fncGetDateTime(ByVal str_BarcodeVal As String) As String
            Dim prodActIntegration As ProductionActIntegration = Nothing
            Dim str_Line As String = String.Empty

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT DATE_TIME FROM STOCK_CARD WHERE BARCODETAG = @BARCODETAG " & _
                                              "AND TYPE_ID IN (1,4,5)"
                    prodActIntegration = connection.Query(Of ProductionActIntegration)(sqlString, New With {Key .BARCODETAG = str_BarcodeVal}).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not prodActIntegration Is Nothing Then
                str_Line = prodActIntegration.DATE_TIME.ToString("yyyy-MM-dd HH:mm:ss")
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

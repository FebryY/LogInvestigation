Option Strict On
Option Explicit On

Imports System.Data

Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass
Imports System.Net


Namespace Model
    Public Enum ShipmentActValues
        SID
        SONumber
        CustomerCode
        PlantNo
        ShipmentDate
        UserID
        BarcodeTag
        ItemProductionDate
        TrinPartCode
        OKNG
        ActQty
        ActIdOfDeletedData
        LineCode
        LabelPartName
    End Enum

    Module ShipmentActDB
        Public Function GetActIdOfDeletedData( _
            ByVal barcodeTag As String _
            ) As Integer
            Dim shipAct As ShipmentAct = Nothing
            Dim actId As Integer = -1

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    connection.Open()

                    Dim sqlString As String = ( _
                        "SELECT ACTID " & _
                        "FROM SHIPMENTACT " & _
                        "WHERE BARCODETAG=@BARCODETAG " & _
                            "AND DELFLAG=1" _
                        )

                    Dim parameter As Object = New With { _
                        Key .BARCODETAG = barcodeTag _
                        }
                    shipAct = connection.Query(Of ShipmentAct) _
                        (sqlString, parameter).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not shipAct Is Nothing Then
                actId = shipAct.ACTID
            End If

            Return actId
        End Function

        Public Function IsBarcodeTagExist( _
            ByVal barcodeTag As String _
            ) As Boolean
            Dim shipAct As ShipmentAct = Nothing

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    connection.Open()

                    Dim sqlString As String = ( _
                        "SELECT BARCODETAG " & _
                        "FROM SHIPMENTACT " & _
                        "WHERE BARCODETAG=@BARCODETAG " & _
                            "AND DELFLAG=0" _
                        )

                    Dim parameter As Object = New With { _
                        Key .BARCODETAG = barcodeTag _
                        }
                    shipAct = connection.Query(Of ShipmentAct) _
                        (sqlString, parameter).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return Not shipAct Is Nothing
        End Function

        Public Function GetBarcodeTagsAndActQty( _
            ByVal barcodeTags As List(Of String) _
            ) As List(Of ShipmentAct)
            Dim shipActs As List(Of ShipmentAct) = Nothing

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    connection.Open()

                    Dim sqlString As String = ( _
                        "SELECT BARCODETAG, ACTQTY " & _
                        "FROM SHIPMENTACT " & _
                        "WHERE FIND_IN_SET(BARCODETAG, @BARCODETAGS) " & _
                            "AND DELFLAG=0" _
                        )

                    Dim param As Object = New With { _
                        Key .BARCODETAGS = ( _
                            String.Join(",", barcodeTags.ToArray()) _
                            ) _
                        }
                    shipActs = CType( _
                        connection.Query(Of ShipmentAct)(sqlString, param),  _
                        List(Of ShipmentAct) _
                        )
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return shipActs
        End Function

        'Add by Lutfi
        Public Function GetSummarySIDAndActQty( _
            ByVal SID As String _
            ) As Integer
            Dim QtyActy As Integer = 0

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )


                Try
                    connection.Open()
                    'add by lutfi 9e ; update Query 9f
                    Dim sqlString As String = ( _
                                         "select CAST(Sum(ACTQTY) AS UNSIGNED INTEGER) TotalQty from shipmentact where SID='" & SID & "'  AND DELFLAG=0 group by SID")
                    'Dim sqlString As String = ( _
                    '                     "select CAST(Sum(ACTQTY) AS UNSIGNED INTEGER) TotalQty from shipmentact where SONumber='" & SID & "'  AND DELFLAG=0 group by SID")

                    QtyActy = CInt( _
                        connection.Query(Of ULong)(sqlString).DefaultIfEmpty(0).FirstOrDefault)


                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return QtyActy
        End Function

        Public Function InsertData( _
            ByVal connection As IDbConnection, _
            ByVal transaction As IDbTransaction, _
            ByVal shipActDataCollection As List(Of String()), _
            ByRef exceptionMsg As String _
            ) As List(Of Integer)
            Dim newActIds As New List(Of Integer)
            Dim x As Integer = 0
            Dim strUser As String = ""
            'Using connection As IDbConnection = New MySqlConnection( _
            '    CommonLib.GenerateConnectionString _
            '    )
            Try
                transaction = connection.BeginTransaction()

                Dim sqlString As String = ( _
                  "INSERT INTO SHIPMENTACT(" & _
                      "SID, SONUMBER, CUSTOMERCODE, PLANTNO, " & _
                      "SHIPMENTDATE, USERID, BARCODETAG, PRODDATE, " & _
                      "TRINPARTNO, OKNG, ACTQTY, DELFLAG, ACCPACSENDFLG," & _
                      "LABELPARTNAME, LABELTRINPARTNO" & _
                  ") Values (" & _
                  " @SID, @SONUMBER, @CUSTOMERCODE, @PLANTNO, " & _
                      "@SHIPMENTDATE, @USERID, @BARCODETAG, " & _
                      "@PRODDATE, @TRINPARTNO, @OKNG, @ACTQTY, 1,1,@LABELPARTNAME, @LABELTRINPARTNO);" _
                 )


                'Dim sqlString As String = ( _
                '       "INSERT INTO SHIPMENTACT(" & _
                '           "SID, SONUMBER, CUSTOMERCODE, PLANTNO, " & _
                '           "SHIPMENTDATE, USERID, BARCODETAG, PRODDATE, " & _
                '           "TRINPARTNO, OKNG, ACTQTY, DELFLAG" & _
                '       ") Values (" & _
                '       " @SID, @SONUMBER, @CUSTOMERCODE, @PLANTNO, " & _
                '           "@SHIPMENTDATE, @USERID, @BARCODETAG, " & _
                '           "@PRODDATE, @TRINPARTNO, @OKNG, @ACTQTY, 0 );" & _
                '       "SELECT CAST(LAST_INSERT_ID() AS UNSIGNED INTEGER)" _
                '       )


                For Each shipActData As String() In shipActDataCollection
                    Dim params As Object = New With { _
                        Key .SID = shipActData(ShipmentActValues.SID), _
                        .SONUMBER = shipActData( _
                            ShipmentActValues.SONumber _
                            ), _
                        .CUSTOMERCODE = shipActData( _
                            ShipmentActValues.CustomerCode _
                            ), _
                        .PLANTNO = shipActData( _
                            ShipmentActValues.PlantNo _
                            ), _
                        .SHIPMENTDATE = shipActData( _
                            ShipmentActValues.ShipmentDate _
                            ), _
                        .USERID = shipActData( _
                            ShipmentActValues.UserID _
                            ), _
                        .BARCODETAG = shipActData( _
                            ShipmentActValues.BarcodeTag _
                            ), _
                        .PRODDATE = shipActData( _
                            ShipmentActValues.ItemProductionDate _
                            ), _
                        .TRINPARTNO = shipActData( _
                            ShipmentActValues.TrinPartCode _
                            ), _
                        .OKNG = shipActData( _
                            ShipmentActValues.OKNG _
                            ), _
                        .ACTQTY = shipActData( _
                            ShipmentActValues.ActQty _
                            ), _
                        .LABELTRINPARTNO = Format(Now(), "yyyyMMddHHmmss"), _
                        .LABELPARTNAME = shipActData( _
                            ShipmentActValues.LabelPartName _
                            ) _
                        }

                    strUser = shipActData(ShipmentActValues.UserID)
                    'Dim lastInsertedId As Integer = CInt( _
                    '                                    connection.Query(Of ULong)(sqlString). _
                    '                                    Single _
                    '                                    )

                    connection.Execute(sqlString, params, transaction)

                    x += 1
                Next



            Catch ex As Exception
                transaction.Rollback()
                exceptionMsg = ex.Message
                Return newActIds
            End Try

            transaction.Commit()


            Dim sqlStringGet As String = ("select * from(select ACTID from shipmentact where UserID='" & strUser & "' and LABELPARTNAME='" & Dns.GetHostEntry(Dns.GetHostName()).AddressList(0).ToString() & "' order by ACTID desc limit " & x & ") as tbl Order by ACTID;")

            Dim myConnection As MySqlConnection = Nothing
            Dim dr As MySqlDataReader

            Try
                myConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                'you need to provide password for sql server
                myConnection.Open()
                Dim myCommand As MySqlCommand = New MySqlCommand(sqlStringGet, myConnection)

                dr = myCommand.ExecuteReader

                While dr.Read()
                    newActIds.Add(CInt(dr(0)))
                End While
            Catch
            End Try
            dr.Close()
            myConnection.Close()

            Return newActIds

        End Function

        Public Function UpdateData( _
            ByVal connection As IDbConnection, _
            ByVal transaction As IDbTransaction, _
            ByVal shipActDataCollection As List(Of String()), _
            ByVal delFlagValue As Integer, _
            ByRef exceptionMsg As String _
            ) As Boolean
            'Using connection As IDbConnection = New MySqlConnection( _
            '    CommonLib.GenerateConnectionString _
            '    )
            '    Dim transaction As IDbTransaction = Nothing

            Try
                connection.Open()
                transaction = connection.BeginTransaction

                Dim sqlString As String = ( _
                    "UPDATE SHIPMENTACT " & _
                    "SET SID = @SID, SONUMBER = @SONUMBER, " & _
                        "CUSTOMERCODE=@CUSTOMERCODE, " & _
                        "PLANTNO=@PLANTNO, " & _
                        "SHIPMENTDATE=@SHIPMENTDATE, " & _
                        "USERID=@USERID, " & _
                        "PRODDATE=@PRODDATE, " & _
                        "DELFLAG=@DELFLAG " & _
                    "WHERE ACTID=@ACTID" _
                    )

                For Each shipActData As String() In shipActDataCollection
                    Dim params As Object = New With { _
                        Key .SID = shipActData( _
                            ShipmentActValues.SID _
                            ), _
                        .SONUMBER = shipActData( _
                            ShipmentActValues.SONumber _
                            ), _
                        .CUSTOMERCODE = shipActData( _
                            ShipmentActValues.CustomerCode _
                            ), _
                        .PLANTNO = shipActData( _
                            ShipmentActValues.PlantNo _
                            ), _
                        .SHIPMENTDATE = shipActData( _
                            ShipmentActValues.ShipmentDate _
                            ), _
                        .USERID = shipActData( _
                            ShipmentActValues.UserID _
                            ), _
                        .PRODDATE = shipActData( _
                            ShipmentActValues.ItemProductionDate _
                            ), _
                        .ACTID = shipActData( _
                            ShipmentActValues.ActIdOfDeletedData _
                            ), _
                        .DELFLAG = delFlagValue _
                        }
                    connection.Execute(sqlString, params, transaction)

                Next

                transaction.Commit()
            Catch ex As Exception
                If Not transaction Is Nothing Then
                    transaction.Rollback()
                End If

                exceptionMsg = ex.Message
                Return False
            End Try


            Return True
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
                        "DELETE FROM SHIPMENTACT " & _
                        "WHERE FIND_IN_SET(ACTID, @ACTIDS)" _
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

        Public Function fncCheckSID(ByVal str_SID As String) As Boolean
            Dim shipAct As ShipmentAct = Nothing
            Dim bool_Res As Boolean = False

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    connection.Open()

                    Dim sqlString As String = ( _
                        "SELECT SID " & _
                        "FROM SHIPMENTPLAN " & _
                        "WHERE SID = @SID " & _
                            "AND DELFLAG=0" _
                        )

                    shipAct = connection.Query(Of ShipmentAct)(sqlString, New With {Key .SID = str_SID}).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not shipAct Is Nothing Then
                bool_Res = True
            End If

            Return bool_Res
        End Function

        Public Function fncInsertStockTakeTemp( _
            ByVal shipActDataCollection As List(Of String()), _
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
                        "INSERT STOCKTAKE_TMP(" & _
                            "TRINPARTNO, BARCODETAG, QTY" & _
                        ") " & _
                        "VALUES(" & _
                            "@TRINPARTNO, @BARCODETAG, @ACTQTY" & _
                        ")" _
                        )

                    For Each shipActData As String() In shipActDataCollection

                        Dim parameters As Object = New With { _
                            Key .TRINPARTNO = shipActData( _
                                ShipmentActValues.TrinPartCode _
                                ), _
                            .ACTQTY = shipActData( _
                                ShipmentActValues.ActQty _
                                ), _
                            .BARCODETAG = shipActData( _
                                ShipmentActValues.BarcodeTag _
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

    End Module
End Namespace



'*********************  FUTURE RELEASE **************************************

'Option Strict On
'Option Explicit On

'Imports System.Data

'Imports DapperLite.SqlMapper
'Imports MySql.Data.MySqlClient

'Imports TradNewSystem.Helpers
'Imports TradNewSystem.PocoClass


'Namespace Model
'    Public Enum ShipmentActValues
'        SID
'        SONumber
'        CustomerCode
'        PlantNo
'        ShipmentDate
'        UserID
'        BarcodeTag
'        ItemProductionDate
'        TrinPartCode
'        OKNG
'        ActQty
'        ActIdOfDeletedData
'        LineCode
'    End Enum

'    Module ShipmentActDB
'        Public Function GetActIdOfDeletedData( _
'            ByVal barcodeTags As List(Of String) _
'            ) As List(Of ShipmentAct)
'            Dim shipAct As List(Of ShipmentAct) = Nothing

'            Using connection As IDbConnection = New MySqlConnection( _
'                CommonLib.GenerateConnectionString _
'                )
'                Try
'                    connection.Open()

'                    Dim sqlString As String = ( _
'                        "SELECT ACTID " & _
'                        "FROM SHIPMENTACT " & _
'                        "WHERE FIND_IN_SET(BARCODETAG ,@BARCODETAGS) " & _
'                            "AND DELFLAG=1" _
'                        )

'                    Dim parameter As Object = New With { _
'                        Key .BARCODETAGS = String.Join(",", barcodeTags.ToArray()) _
'                        }
'                    shipAct = CType(connection.Query(Of ShipmentAct) _
'                        (sqlString, parameter), List(Of ShipmentAct))
'                Catch ex As Exception
'                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
'                End Try
'            End Using

'            Return shipAct
'        End Function

'        Public Function IsBarcodeTagExist( _
'            ByVal barcodeTag As String _
'            ) As Boolean
'            Dim shipAct As ShipmentAct = Nothing

'            Using connection As IDbConnection = New MySqlConnection( _
'                CommonLib.GenerateConnectionString _
'                )
'                Try
'                    connection.Open()

'                    Dim sqlString As String = ( _
'                        "SELECT BARCODETAG " & _
'                        "FROM SHIPMENTACT " & _
'                        "WHERE BARCODETAG=@BARCODETAG " & _
'                            "AND DELFLAG=0" _
'                        )

'                    Dim parameter As Object = New With { _
'                        Key .BARCODETAG = barcodeTag _
'                        }
'                    shipAct = connection.Query(Of ShipmentAct) _
'                        (sqlString, parameter).FirstOrDefault
'                Catch ex As Exception
'                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
'                End Try
'            End Using

'            Return Not shipAct Is Nothing
'        End Function

'        Public Function GetBarcodeTagsAndActQty( _
'            ByVal barcodeTags As List(Of String) _
'            ) As List(Of ShipmentAct)
'            Dim shipActs As List(Of ShipmentAct) = Nothing

'            Using connection As IDbConnection = New MySqlConnection( _
'                CommonLib.GenerateConnectionString _
'                )
'                Try
'                    connection.Open()

'                    Dim sqlString As String = ( _
'                        "SELECT BARCODETAG, ACTQTY " & _
'                        "FROM SHIPMENTACT " & _
'                        "WHERE FIND_IN_SET(BARCODETAG, @BARCODETAGS) " & _
'                            "AND DELFLAG=0" _
'                        )

'                    Dim param As Object = New With { _
'                        Key .BARCODETAGS = ( _
'                            String.Join(",", barcodeTags.ToArray()) _
'                            ) _
'                        }
'                    shipActs = CType( _
'                        connection.Query(Of ShipmentAct)(sqlString, param),  _
'                        List(Of ShipmentAct) _
'                        )
'                Catch ex As Exception
'                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
'                End Try
'            End Using

'            Return shipActs
'        End Function

'        Public Function InsertData( _
'            ByVal shipActDataCollection As List(Of String()), _
'            ByRef exceptionMsg As String _
'            ) As List(Of Integer)
'            Dim newActIds As New List(Of Integer)

'            Using connection As IDbConnection = New MySqlConnection( _
'                CommonLib.GenerateConnectionString _
'                )
'                Try
'                    connection.Open()

'                    ' Skip inserting data if duplicate found
'                    Dim sqlString As String = ( _
'                        "INSERT INTO SHIPMENTACT(" & _
'                            "SID, SONUMBER, CUSTOMERCODE, PLANTNO, " & _
'                            "SHIPMENTDATE, USERID, BARCODETAG, PRODDATE, " & _
'                            "TRINPARTNO, OKNG, ACTQTY, DELFLAG" & _
'                        ") " & _
'                        "SELECT @SID, @SONUMBER, @CUSTOMERCODE, @PLANTNO, " & _
'                            "@SHIPMENTDATE, @USERID, @BARCODETAG, " & _
'                            "@PRODDATE, @TRINPARTNO, @OKNG, @ACTQTY, 0 " & _
'                        "FROM SHIPMENTACT " & _
'                        "WHERE NOT EXISTS( " & _
'                            "SELECT BARCODETAG " & _
'                            "FROM SHIPMENTACT " & _
'                            "WHERE BARCODETAG = @BARCODETAG" & _
'                        ") " & _
'                        "LIMIT 1; " & _
'                        "SELECT CAST(LAST_INSERT_ID() AS UNSIGNED INTEGER)" _
'                        )

'                    For Each shipActData As String() In shipActDataCollection
'                        Dim params As Object = New With { _
'                            Key .SID = shipActData(ShipmentActValues.SID), _
'                            .SONUMBER = shipActData( _
'                                ShipmentActValues.SONumber _
'                                ), _
'                            .CUSTOMERCODE = shipActData( _
'                                ShipmentActValues.CustomerCode _
'                                ), _
'                            .PLANTNO = shipActData( _
'                                ShipmentActValues.PlantNo _
'                                ), _
'                            .SHIPMENTDATE = shipActData( _
'                                ShipmentActValues.ShipmentDate _
'                                ), _
'                            .USERID = shipActData( _
'                                ShipmentActValues.UserID _
'                                ), _
'                            .BARCODETAG = shipActData( _
'                                ShipmentActValues.BarcodeTag _
'                                ), _
'                            .PRODDATE = shipActData( _
'                                ShipmentActValues.ItemProductionDate _
'                                ), _
'                            .TRINPARTNO = shipActData( _
'                                ShipmentActValues.TrinPartCode _
'                                ), _
'                            .OKNG = shipActData( _
'                                ShipmentActValues.OKNG _
'                                ), _
'                            .ACTQTY = shipActData( _
'                                ShipmentActValues.ActQty _
'                                ) _
'                            }

'                        Dim lastInsertedId As Integer = CInt( _
'                            connection.Query(Of ULong)(sqlString, params). _
'                            DefaultIfEmpty(0).FirstOrDefault _
'                            )

'                        newActIds.Add(lastInsertedId)
'                    Next
'                Catch ex As Exception
'                    exceptionMsg = ex.Message
'                End Try
'            End Using

'            Return newActIds
'        End Function

'        Public Function UpdateData( _
'            ByVal shipActDataCollection As List(Of String()), _
'            ByVal delFlagValue As Integer, _
'            ByVal actIds As List(Of Integer), _
'            ByRef exceptionMsg As String _
'            ) As Boolean
'            Using connection As IDbConnection = New MySqlConnection( _
'                CommonLib.GenerateConnectionString _
'                )
'                Dim transaction As IDbTransaction = Nothing

'                Try
'                    connection.Open()
'                    transaction = connection.BeginTransaction

'                    Dim lst_String As List(Of String) = actIds.ConvertAll(Of String)(Function(int_Convert As Integer) int_Convert.ToString())

'                    Dim sqlString As String = ( _
'                    "UPDATE SHIPMENTACT " & _
'                    "SET SID = @SID, SONUMBER = @SONUMBER, " & _
'                        "CUSTOMERCODE=@CUSTOMERCODE, " & _
'                        "PLANTNO=@PLANTNO, " & _
'                        "SHIPMENTDATE=@SHIPMENTDATE, " & _
'                        "USERID=@USERID, " & _
'                        "PRODDATE=@PRODDATE, " & _
'                        "DELFLAG=@DELFLAG " & _
'                    "WHERE FIND_IN_SET(ACTID, @ACTIDS)" _
'                    )

'                    For Each shipActData As String() In shipActDataCollection
'                        Dim params As Object = New With { _
'                            Key .SID = shipActData( _
'                                ShipmentActValues.SID _
'                                ), _
'                            .SONUMBER = shipActData( _
'                                ShipmentActValues.SONumber _
'                                ), _
'                            .CUSTOMERCODE = shipActData( _
'                                ShipmentActValues.CustomerCode _
'                                ), _
'                            .PLANTNO = shipActData( _
'                                ShipmentActValues.PlantNo _
'                                ), _
'                            .SHIPMENTDATE = shipActData( _
'                                ShipmentActValues.ShipmentDate _
'                                ), _
'                            .USERID = shipActData( _
'                                ShipmentActValues.UserID _
'                                ), _
'                            .PRODDATE = shipActData( _
'                                ShipmentActValues.ItemProductionDate _
'                                ), _
'                            .ACTIDS = String.Join(",", lst_String.ToArray()), _
'                            .DELFLAG = delFlagValue _
'                            }
'                        connection.Execute(sqlString, params, transaction)
'                    Next

'                    transaction.Commit()
'                Catch ex As Exception
'                    If Not transaction Is Nothing Then
'                        transaction.Rollback()
'                    End If

'                    exceptionMsg = ex.Message
'                    Return False
'                End Try
'            End Using

'            Return True
'        End Function

'        Public Function DeleteData( _
'            ByVal actIds As List(Of Integer), _
'            ByRef exceptionMsg As String _
'            ) As Boolean
'            Dim strActIds As String() = ( _
'                actIds.[Select](Function(x) x.ToString()).ToArray() _
'                )

'            Using connection As IDbConnection = New MySqlConnection( _
'                CommonLib.GenerateConnectionString _
'                )
'                Dim transaction As IDbTransaction = Nothing

'                Try
'                    connection.Open()
'                    transaction = connection.BeginTransaction

'                    Dim sqlString As String = ( _
'                        "DELETE FROM SHIPMENTACT " & _
'                        "WHERE FIND_IN_SET(ACTID, @ACTIDS)" _
'                        )
'                    Dim param As Object = New With { _
'                        Key .ACTIDS = String.Join(",", strActIds) _
'                        }

'                    connection.Execute( _
'                        sqlString, param, transaction _
'                        )
'                    transaction.Commit()
'                Catch ex As Exception
'                    If Not transaction Is Nothing Then
'                        transaction.Rollback()
'                    End If

'                    exceptionMsg = ex.Message
'                    Return False
'                End Try
'            End Using

'            Return True
'        End Function

'        Public Function fncCheckSID(ByVal str_SID As String) As Boolean
'            Dim shipAct As ShipmentAct = Nothing
'            Dim bool_Res As Boolean = False

'            Using connection As IDbConnection = New MySqlConnection( _
'                CommonLib.GenerateConnectionString _
'                )
'                Try
'                    connection.Open()

'                    Dim sqlString As String = ( _
'                        "SELECT SID " & _
'                        "FROM SHIPMENTPLAN " & _
'                        "WHERE SID = @SID " & _
'                            "AND DELFLAG=0" _
'                        )

'                    shipAct = connection.Query(Of ShipmentAct)(sqlString, New With {Key .SID = str_SID}).FirstOrDefault
'                Catch ex As Exception
'                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
'                End Try
'            End Using

'            If Not shipAct Is Nothing Then
'                bool_Res = True
'            End If

'            Return bool_Res
'        End Function

'        Public Function fncInsertStockTakeTemp( _
'            ByVal shipActDataCollection As List(Of String()), _
'            ByRef exceptionMsg As String _
'            ) As Boolean

'            Using connection As IDbConnection = New MySqlConnection( _
'                CommonLib.GenerateConnectionString _
'                )
'                Dim transaction As IDbTransaction = Nothing

'                Try
'                    connection.Open()
'                    transaction = connection.BeginTransaction()

'                    Dim sqlString As String = ( _
'                        "INSERT STOCKTAKE_TMP(" & _
'                            "TRINPARTNO, BARCODETAG, QTY" & _
'                        ") " & _
'                        "VALUES(" & _
'                            "@TRINPARTNO, @BARCODETAG, @ACTQTY" & _
'                        ")" _
'                        )

'                    For Each shipActData As String() In shipActDataCollection
'                        Dim parameters As Object = New With { _
'                            Key .TRINPARTNO = shipActData( _
'                                ShipmentActValues.TrinPartCode _
'                                ), _
'                            .ACTQTY = shipActData( _
'                                ShipmentActValues.ActQty _
'                                ), _
'                            .BARCODETAG = shipActData( _
'                                ShipmentActValues.BarcodeTag _
'                                ) _
'                            }
'                        connection.Execute(sqlString, parameters, transaction)
'                    Next

'                    transaction.Commit()
'                Catch ex As Exception
'                    If Not transaction Is Nothing Then
'                        transaction.Rollback()
'                    End If

'                    exceptionMsg = ex.Message
'                    Return False
'                End Try
'            End Using

'            Return True
'        End Function

'    End Module
'End Namespace
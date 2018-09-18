Option Strict On
Option Explicit On

Imports System.Data
Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass
Imports DNWA.BHTCL

Namespace Model
    Module ProductionActDB
        Public Function GetActIdOfDeletedData( _
            ByVal productQrCode As String _
            ) As Integer
            Dim productionAct As ProductionAct = Nothing

            Dim actId As Integer = -1

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    connection.Open()

                    Dim sqlString As String = ( _
                        "SELECT ACTID " & _
                        "FROM PRODUCTIONACT " & _
                        "WHERE QRCODE=@QRCODE " & _
                            "AND DELFLAG=1" _
                        )

                    Dim parameter As Object = New With { _
                        Key .QRCODE = productQrCode _
                        }

                    productionAct = connection.Query(Of ProductionAct) _
                        (sqlString, parameter).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not productionAct Is Nothing Then
                actId = productionAct.ACTID
            End If

            Return actId
        End Function

        Public Function GetProdDateLineCodeUserID( _
            ByVal productQrCode As String, _
            ByVal checkDelFlg As Boolean _
            ) As ProductionAct
            Dim productionAct As ProductionAct = Nothing

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    connection.Open()

                    Dim sqlString As String = ( _
                        "SELECT PRODDATE, LINECODE , userid " & _
                        "FROM PRODUCTIONACT " & _
                        "WHERE QRCODE=@QRCODE" _
                        )

                    If checkDelFlg Then
                        sqlString &= " AND DELFLAG=0"
                    End If

                    Dim parameter As Object = New With { _
                        Key .QRCODE = productQrCode _
                        }

                    productionAct = connection.Query(Of ProductionAct) _
                        (sqlString, parameter).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return productionAct
        End Function

        Public Function GetProdDateLineCode( _
           ByVal productQrCode As String, _
           ByVal checkDelFlg As Boolean _
           ) As ProductionAct
            Dim productionAct As ProductionAct = Nothing

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    connection.Open()

                    Dim sqlString As String = ( _
                        "SELECT PRODDATE, LINECODE , userid " & _
                        "FROM PRODUCTIONACT " & _
                        "WHERE QRCODE=@QRCODE" _
                        )

                    If checkDelFlg Then
                        sqlString &= " AND DELFLAG=0"
                    End If

                    Dim parameter As Object = New With { _
                        Key .QRCODE = productQrCode _
                        }

                    productionAct = connection.Query(Of ProductionAct) _
                        (sqlString, parameter).FirstOrDefault
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return productionAct
        End Function

        Public Function fncCheckQR(ByVal productQrCode As String) As QueryRetValue
            Dim productionAct As ProductionAct = Nothing
            Dim ret_productQR As QueryRetValue = QueryRetValue.ValueFalse

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT QRCODE FROM PRODUCTIONACT WHERE QRCODE=@QRCODE AND DELFLAG=0"

                    productionAct = connection.Query(Of ProductionAct)(sqlString, New With {Key .QRCODE = productQrCode}).FirstOrDefault
                Catch ex As Exception
                    ret_productQR = QueryRetValue.ValueError
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not productionAct Is Nothing Then
                ret_productQR = QueryRetValue.ValueTrue
            Else
                ret_productQR = QueryRetValue.ValueFalse
            End If

            Return ret_productQR
        End Function

        Public Function fncCheckBarcodetag(ByVal productQrCode As String) As QueryRetValue
            Dim productionAct As ProductionAct = Nothing
            Dim ret_productQR As QueryRetValue = QueryRetValue.ValueFalse

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT QRCODE FROM PRODUCTIONACT WHERE BARCODETAG=@QRCODE AND DELFLAG=0"

                    productionAct = connection.Query(Of ProductionAct)(sqlString, New With {Key .QRCODE = productQrCode}).FirstOrDefault
                Catch ex As Exception
                    ret_productQR = QueryRetValue.ValueError
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not productionAct Is Nothing Then
                ret_productQR = QueryRetValue.ValueTrue
            Else
                ret_productQR = QueryRetValue.ValueFalse
            End If

            Return ret_productQR
        End Function

        Public Function fncCheckDeletedBarcodetag(ByVal productQrCode As String) As QueryRetValue
            Dim productionAct As ProductionAct = Nothing
            Dim ret_productQR As QueryRetValue = QueryRetValue.ValueFalse

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT QRCODE FROM PRODUCTIONACT WHERE BARCODETAG=@QRCODE AND DELFLAG=1"

                    'Dim MyRf As RF
                    'MyRf = New RF()
                    'MyRf.OpenMode = RF.EN_OPEN_MODE.CONTINUOUSLY
                    'MyRf.Open = False

                    productionAct = connection.Query(Of ProductionAct)(sqlString, New With {Key .QRCODE = productQrCode}).FirstOrDefault
                Catch ex As Exception
                    ret_productQR = QueryRetValue.ValueError
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not productionAct Is Nothing Then
                ret_productQR = QueryRetValue.ValueTrue
            Else
                ret_productQR = QueryRetValue.ValueFalse
            End If

            Return ret_productQR
        End Function

        Public Function fncGetTRINPartNo(ByVal productQrCode As String) As String
            Dim productionAct As List(Of ProductionAct) = Nothing

            Dim TRINPartNo As String = String.Empty

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    connection.Open()

                    Dim sqlString As String = "SELECT TRINPARTNO FROM PRODUCTIONACT WHERE QRCODE=@QRCODE AND DELFLAG=0"

                    productionAct = CType(connection.Query(Of ProductionAct)(sqlString, New With {Key .QRCODE = productQrCode}), List(Of ProductionAct))
                Catch ex As Exception
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not productionAct Is Nothing Then
                For Each productionActItem As ProductionAct In productionAct
                    TRINPartNo = productionActItem.TRINPARTNO
                Next
            End If

            Return TRINPartNo
        End Function
    End Module
End Namespace
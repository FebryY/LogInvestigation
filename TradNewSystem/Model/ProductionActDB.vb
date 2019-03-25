Option Strict On
Option Explicit On

Imports System.Data
Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass
Imports DNWA.BHTCL
Imports log4net

Namespace Model
    Module ProductionActDB
        Public Function GetActIdOfDeletedData( _
            ByVal productQrCode As String _
            ) As Integer
            Dim productionAct As ProductionAct = Nothing

            Dim actId As Integer = -1
            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    'log.Info("ProductionActDB, Open connection")
                    connection.Open()
                    'log.Info("ProductionActDB, Open connection success")

                    Dim sqlString As String = ( _
                        "SELECT ACTID " & _
                        "FROM PRODUCTIONACT " & _
                        "WHERE QRCODE=@QRCODE " & _
                            "AND DELFLAG=1" _
                        )
                    'log.Info("ProductionActDB SQL string: " & sqlString)

                    Dim parameter As Object = New With { _
                        Key .QRCODE = productQrCode _
                        }

                    productionAct = connection.Query(Of ProductionAct) _
                        (sqlString, parameter).FirstOrDefault
                    'log.Info("ProductionActDB can get result ")

                Catch ex As Exception
                    'log.Error("ProductionActDB DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not productionAct Is Nothing Then
                actId = productionAct.ACTID
            End If
            'log.Info("ProductionActDB result" & actId)
            LogManager.Shutdown()

            Return actId
        End Function

        Public Function GetProdDateLineCodeUserID( _
            ByVal productQrCode As String) As ProductionAct
            Dim productionAct As ProductionAct = Nothing
            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    'log.Info("GetProdDateLineCodeUserID, Open connection")
                    connection.Open()
                    'log.Info("GetProdDateLineCodeUserID, Open connection success")

                    Dim sqlString As String = ( _
                        "SELECT PRODDATE, LINECODE , userid " & _
                        "FROM PRODUCTIONACT " & _
                        "WHERE QRCODE=@QRCODE AND DELFLAG=0" _
                        )
                    'log.Info("GetProdDateLineCodeUserID SQL string: " & sqlString)

                    'If checkDelFlg Then
                    '    sqlString &= " AND DELFLAG=0"
                    'End If

                    Dim parameter As Object = New With { _
                        Key .QRCODE = productQrCode _
                        }

                    productionAct = connection.Query(Of ProductionAct) _
                        (sqlString, parameter).FirstOrDefault
                    'log.Info("GetProdDateLineCodeUserID can get result")

                Catch ex As Exception
                    'log.Error("GetProdDateLineCodeUserID DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using
            'LogManager.Shutdown()

            Return productionAct
        End Function

        Public Function GetProdDateLineCode( _
           ByVal productQrCode As String, _
           ByVal checkDelFlg As Boolean _
           ) As ProductionAct
            Dim productionAct As ProductionAct = Nothing
            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection( _
                CommonLib.GenerateConnectionString _
                )
                Try
                    'log.Info("GetProdDateLineCode, Open connection")
                    connection.Open()
                    'log.Info("GetProdDateLineCode, Open connection success")

                    Dim sqlString As String = ( _
                        "SELECT PRODDATE, LINECODE , userid " & _
                        "FROM PRODUCTIONACT " & _
                        "WHERE QRCODE=@QRCODE" _
                        )
                    'log.Info("GetProdDateLineCode SQL string: " & sqlString)

                    If checkDelFlg Then
                        sqlString &= " AND DELFLAG=0"
                    End If

                    Dim parameter As Object = New With { _
                        Key .QRCODE = productQrCode _
                        }

                    productionAct = connection.Query(Of ProductionAct) _
                        (sqlString, parameter).FirstOrDefault
                    'log.Info("GetProdDateLineCode can get result ")

                Catch ex As Exception
                    'log.Error("GetProdDateLineCode DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using
            'LogManager.Shutdown()

            Return productionAct
        End Function

        Public Function fncCheckQR(ByVal productQrCode As String) As QueryRetValue
            Dim productionAct As ProductionAct = Nothing
            Dim ret_productQR As QueryRetValue = QueryRetValue.ValueFalse
            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    'log.Info("fncCheckQR, Open connection")
                    connection.Open()
                    'log.Info("fncCheckQR, Open connection success")

                    Dim sqlString As String = "SELECT QRCODE FROM PRODUCTIONACT WHERE QRCODE=@QRCODE AND DELFLAG=0"
                    'log.Info("fncCheckQR SQL string: " & sqlString)

                    productionAct = connection.Query(Of ProductionAct)(sqlString, New With {Key .QRCODE = productQrCode}).FirstOrDefault
                    'log.Info("fncCheckQR can get result ")

                Catch ex As Exception
                    ret_productQR = QueryRetValue.ValueError
                    'log.Error("fncCheckQR DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not productionAct Is Nothing Then
                ret_productQR = QueryRetValue.ValueTrue
            Else
                ret_productQR = QueryRetValue.ValueFalse
            End If
            'log.Info("fncCheckQR result " & ret_productQR)
            'LogManager.Shutdown()

            Return ret_productQR
        End Function

        Public Function fncCheckBarcodetag(ByVal productQrCode As String) As QueryRetValue
            Dim productionAct As ProductionAct = Nothing
            Dim ret_productQR As QueryRetValue = QueryRetValue.ValueFalse
            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    'log.Info("fncCheckBarcodetag, Open connection")
                    connection.Open()
                    'log.Info("fncCheckBarcodetag, Open connection success")

                    Dim sqlString As String = "SELECT QRCODE FROM PRODUCTIONACT WHERE BARCODETAG=@QRCODE AND DELFLAG=0"
                    'log.Info("fncCheckBarcodetag SQL string: " & sqlString)

                    productionAct = connection.Query(Of ProductionAct)(sqlString, New With {Key .QRCODE = productQrCode}).FirstOrDefault
                    'log.Info("fncCheckBarcodetag can get result ")
                Catch ex As Exception
                    ret_productQR = QueryRetValue.ValueError
                    'log.Error("fncCheckBarcodetag DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not productionAct Is Nothing Then
                ret_productQR = QueryRetValue.ValueTrue
            Else
                ret_productQR = QueryRetValue.ValueFalse
            End If
            'log.Info("fncCheckBarcodetag result " & ret_productQR)
            'LogManager.Shutdown()

            Return ret_productQR
        End Function

        Public Function fncCheckDeletedBarcodetag(ByVal productQrCode As String) As QueryRetValue
            Dim productionAct As ProductionAct = Nothing
            Dim ret_productQR As QueryRetValue = QueryRetValue.ValueFalse
            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    'log.Info("fncCheckDeletedBarcodetag, Open connection")
                    connection.Open()
                    'log.Info("fncCheckDeletedBarcodetag, Open connection success")

                    Dim sqlString As String = "SELECT QRCODE FROM PRODUCTIONACT WHERE BARCODETAG=@QRCODE AND DELFLAG=1"
                    'log.Info("fncCheckDeletedBarcodetag SQL string: " & sqlString)

                    'Dim MyRf As RF
                    'MyRf = New RF()
                    'MyRf.OpenMode = RF.EN_OPEN_MODE.CONTINUOUSLY
                    'MyRf.Open = False

                    productionAct = connection.Query(Of ProductionAct)(sqlString, New With {Key .QRCODE = productQrCode}).FirstOrDefault
                    'log.Info("fncCheckDeletedBarcodetag can get result")

                Catch ex As Exception
                    ret_productQR = QueryRetValue.ValueError
                    'log.Error("fncCheckDeletedBarcodetag DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            If Not productionAct Is Nothing Then
                ret_productQR = QueryRetValue.ValueTrue
            Else
                ret_productQR = QueryRetValue.ValueFalse
            End If
            'log.Info("fncCheckBarcodetag result " & ret_productQR)
            'LogManager.Shutdown()

            Return ret_productQR
        End Function

        Public Function fncGetTRINPartNo(ByVal productQrCode As String) As String
            Dim productionAct As List(Of ProductionAct) = Nothing

            Dim TRINPartNo As String = String.Empty
            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    'log.Info("fncGetTRINPartNo, Open connection")
                    connection.Open()
                    'log.Info("fncGetTRINPartNo, Open connection success")

                    Dim sqlString As String = "SELECT TRINPARTNO FROM PRODUCTIONACT WHERE QRCODE=@QRCODE AND DELFLAG=0"
                    'log.Info("fncGetTRINPartNo SQL string: " & sqlString)

                    productionAct = CType(connection.Query(Of ProductionAct)(sqlString, New With {Key .QRCODE = productQrCode}), List(Of ProductionAct))
                    'log.Info("fncGetTRINPartNo result " & productionAct.Count())

                Catch ex As Exception
                    'log.Error("fncGetTRINPartNo DB Error", ex)
                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using
            'LogManager.Shutdown()

            If Not productionAct Is Nothing Then
                For Each productionActItem As ProductionAct In productionAct
                    TRINPartNo = productionActItem.TRINPARTNO
                Next
            End If

            Return TRINPartNo
        End Function
    End Module
End Namespace
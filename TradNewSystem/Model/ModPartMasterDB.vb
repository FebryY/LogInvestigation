Imports System.Data
Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass
Imports log4net


Namespace Model
    Module ModPartMasterDB

        Public Function fncGetTRINPartNo(ByVal str_OldTRIN As String) As String
            Dim modPartMaster As List(Of ModPartMaster) = Nothing

            Dim str_OldTRINPartNo As String = String.Empty
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    log.Info("fncGetTRINPartNo, Open connection")

                    connection.Open()

                    log.Info("fncGetTRINPartNo, Open connection success")

                    Dim sqlString As String = "SELECT TRINPARTNO FROM MODPARTMASTER WHERE TRINPARTNO = @TRINPARTNO"

                    log.Info("fncGetTRINPartNo SQL string: " & sqlString)

                    modPartMaster = CType(connection.Query(Of ModPartMaster)(sqlString, New With {Key .TRINPARTNO = str_OldTRIN}), List(Of ModPartMaster))

                    log.Info("fncGetTRINPartNo result " & modPartMaster.Count())

                Catch ex As Exception
                    log.Error("fncGetTRINPartNo DB Error", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using
            LogManager.Shutdown()

            If Not modPartMaster Is Nothing Then
                For Each productionActItem As ModPartMaster In modPartMaster
                    str_OldTRINPartNo = productionActItem.TRINPARTNO
                Next
            End If

            Return str_OldTRINPartNo
        End Function

        Public Function fncGetNewTRIMPartNo(ByVal str_NewTRIN As String) As List(Of ModPartMaster)
            Dim newTRINNo As List(Of ModPartMaster) = Nothing
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")
            Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                Try
                    log.Info("fncGetNewTRIMPartNo, Open connection")

                    connection.Open()

                    log.Info("fncGetNewTRIMPartNo, Open connection success")

                    Dim sqlString As String = "SELECT TRINPARTNO,NEWPARTNO FROM MODPARTMASTER WHERE TRINPARTNO = @TRINPARTNO"

                    log.Info("fncGetNewTRIMPartNo SQL string: " & sqlString)

                    newTRINNo = CType(connection.Query(Of ModPartMaster)(sqlString, New With {Key .TRINPARTNO = str_NewTRIN}), List(Of ModPartMaster))
                    log.Info("fncGetNewTRIMPartNo result " & newTRINNo.Count())

                Catch ex As Exception
                    log.Error("fncGetTRINPartNo DB Error", ex)

                    DisplayMessage.ErrorMsg(ex.Message, "DB Error")
                End Try
            End Using

            Return newTRINNo
        End Function

    End Module
End Namespace

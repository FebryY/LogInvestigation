Imports System.Data
Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass
Imports log4net


Namespace Model
    Module PartMasterDB
        Public Function fncCheckTRIMPartNoMaster(ByVal str_TRIN As String) As QueryRetValue
            Dim partMaster As PartMaster = Nothing

            Dim ret_TRIN As QueryRetValue = QueryRetValue.ValueFalse

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Try
                Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)
                    log.Info("fncCheckTRIMPartNoMaster, Open connection")

                    connection.Open()

                    log.Info("fncCheckTRIMPartNoMaster, Open connection success")

                    Dim sqlString As String = "SELECT TRINPARTNO FROM PARTMASTER WHERE TRINPARTNO=@TRINPARTNO"

                    log.Info("fncCheckTRIMPartNoMaster SQL string: " & sqlString)

                    partMaster = connection.Query(Of PartMaster)(sqlString, New With {Key .TRINPARTNO = str_TRIN}).FirstOrDefault

                End Using
            Catch ex As Exception
                ret_TRIN = QueryRetValue.ValueError
                log.Error("fncCheckTRIMPartNoMaster DB Error", ex)

                DisplayMessage.ErrorMsg(ex.Message, "DB Error")
            End Try


            If Not partMaster Is Nothing Then
                ret_TRIN = QueryRetValue.ValueTrue
            Else
                ret_TRIN = QueryRetValue.ValueFalse
            End If

            log.Info("fncCheckTRIMPartNoMaster result " & ret_TRIN)
            LogManager.Shutdown()

            Return ret_TRIN
        End Function
    End Module
End Namespace

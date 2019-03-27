Imports System.Data
Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass
Imports log4net

Namespace Model
    Module LineMasterDB

        Public Function fncGetLineCodes(ByVal str_Code As String) As String
            Dim lineMaster As List(Of LineMaster) = Nothing

            Dim str_LineCode As String = String.Empty

            'log4net.Config.XmlConfigurator.Configure()
            'Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Try
                Using connection As IDbConnection = New MySqlConnection(CommonLib.GenerateConnectionString)

                    'log.Info("fncGetLineCodes, Open connection")

                    connection.Open()

                    'log.Info("fncGetLineCodes, Open connection success")

                    Dim sqlString As String = "SELECT LINECODE FROM LINEMASTER WHERE BARCODELINE = @BARCODELINE"

                    'log.Info("fncGetLineCodes SQL string: " & sqlString)

                    lineMaster = CType(connection.Query(Of LineMaster)(sqlString, New With {Key .BARCODELINE = str_Code}), List(Of LineMaster))

                    'log.Info("fncGetLineCodes result " & lineMaster.Count())

                End Using
            Catch ex As Exception
                'log.Error("fncGetLineCodes DB Error", ex)
                DisplayMessage.ErrorMsg(ex.Message, "DB Error")
            End Try

            'LogManager.Shutdown()

            If Not lineMaster Is Nothing Then
                For Each lineMasterItem As LineMaster In lineMaster
                    str_LineCode = lineMasterItem.LINECODE
                Next
            End If

            Return str_LineCode
        End Function

    End Module
End Namespace

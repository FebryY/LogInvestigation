Option Strict On
Option Explicit On

Imports System
Imports System.Globalization
Imports System.Data

Imports System.Data.SQLite

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass

Imports log4net

Namespace Model
    Module SqliteControllerDB
        Private str_File As String = "ScannedBarcodeTag.db"

        Private str_DbConn As String = String.Format("Data Source = {0}", str_File)

#Region "Function"
        Public Function fncGetDatatable(ByVal sql_Query As String) As DataTable
            Dim dt_TempData As DataTable = New DataTable

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Try
                Dim sqlLite_Conn As New SQLiteConnection(str_DbConn)

                log.Info("fncGetDatatable, Open Connection")

                sqlLite_Conn.Open()

                log.Info("fncGetDatatable, Open Connection success")


                Dim sqlLite_Comm As New SQLiteCommand(sqlLite_Conn)

                sqlLite_Comm.CommandText = sql_Query

                log.Info("fncGetDatatable SQL string: " & sql_Query)

                Dim sqlLite_DataReader As SQLiteDataReader = sqlLite_Comm.ExecuteReader

                dt_TempData.Load(sqlLite_DataReader)

                log.Info("fncGetDatatable result " & sqlLite_DataReader.ToString())

                sqlLite_DataReader.Close()

                sqlLite_Conn.Close()

            Catch ex As Exception

                log.Error("fncGetDatatable DB Error ", ex)

                DisplayMessage.ErrorMsg(ex.ToString, "Error")
            End Try

            Return dt_TempData
        End Function

        Public Function fncExecuteNonQuery(ByVal sql_Query As String) As Integer

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim sqlLite_Conn As New SQLiteConnection(str_DbConn)

            log.Info("fncExecuteNonQuery, Open Connection")

            sqlLite_Conn.Open()

            log.Info("fncExecuteNonQuery, Open Connection success")

            Dim sqlLite_Comm As New SQLiteCommand(sqlLite_Conn)

            sqlLite_Comm.CommandText = sql_Query

            Dim int_RowsAffected As Integer = sqlLite_Comm.ExecuteNonQuery

            log.Info("fncExecuteNonQuery result " & int_RowsAffected.ToString())

            sqlLite_Conn.Close()

            Return int_RowsAffected
        End Function

        Public Function fncExecuteScalar(ByVal sql_Query As String) As String

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim str_Res As String = String.Empty
            Dim sqlLite_Conn As New SQLiteConnection(str_DbConn)

            log.Info("fncExecuteScalar, Open Connection")

            sqlLite_Conn.Open()

            log.Info("fncExecuteScalar, Open Connection success")

            Dim sqlLite_Comm As New SQLiteCommand(sqlLite_Conn)

            sqlLite_Comm.CommandText = sql_Query

            log.Info("fncExecuteScalar SQL string: " & sql_Query)

            Dim obj_Value As Object = sqlLite_Comm.ExecuteScalar

            log.Info("fncExecuteScalar result " & obj_Value.ToString())

            sqlLite_Conn.Close()

            If obj_Value IsNot Nothing Then
                str_Res = obj_Value.ToString
            End If

            Return str_Res
        End Function

        Public Function fncUpdate(ByVal str_TableName As String, _
                                  ByVal int_Flag As Integer, _
                                  ByVal obj_DictData As Dictionary(Of String, String), _
                                  ByVal str_Where As String _
                                  ) As Boolean

            Dim bool_RetVal As Boolean = True
            Dim str_Val As String = String.Empty

            If obj_DictData.Count >= 1 Then
                For Each obj_Val As KeyValuePair(Of String, String) In obj_DictData
                    str_Val += String.Format(" {0} = '{1}',", obj_Val.Key.ToString(), obj_Val.Value.ToString())
                Next
                str_Val = str_Val.Substring(0, str_Val.Length - 1)
            End If

            Try
                SqliteControllerDB.fncExecuteNonQuery(String.Format("update {0} set {1} where {2} and InputFlag = {3};", _
                                                                    str_TableName, _
                                                                    str_Val, _
                                                                    str_Where, _
                                                                    int_Flag) _
                                                    )
            Catch
                bool_RetVal = False
            End Try

            Return bool_RetVal
        End Function

        Public Function fncInsert(ByVal str_TableName As String, _
                                  ByVal obj_DictData As Dictionary(Of String, String) _
                                  ) As Boolean
            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim bool_RetVal As Boolean = True
            Dim str_TblColumn As String = String.Empty
            Dim str_TblVal As String = String.Empty

            For Each obj_Val As KeyValuePair(Of String, String) In obj_DictData
                str_TblColumn += String.Format(" {0},", obj_Val.Key.ToString())
                str_TblVal += String.Format(" '{0}',", obj_Val.Value)
            Next

            str_TblColumn = str_TblColumn.Substring(0, str_TblColumn.Length - 1)
            str_TblVal = str_TblVal.Substring(0, str_TblVal.Length - 1)

            Try
                SqliteControllerDB.fncExecuteNonQuery(String.Format("insert into {0}({1}) values({2});", _
                                                                  str_TableName, _
                                                                  str_TblColumn, _
                                                                  str_TblVal _
                                                                  ) _
                                                    )
            Catch ex As Exception

                log.Error("fncInsert sqlite Controller DB Error ", ex)

                DisplayMessage.ErrorMsg(String.Concat("Temp Data failed to save, ", ex.ToString), "Error")
                bool_RetVal = False
            End Try

            Return bool_RetVal
        End Function

        Public Function fncDelete(ByVal str_TableName As String, _
                                  ByVal str_Where As String _
                                  ) As Boolean

            log4net.Config.XmlConfigurator.Configure()
            Dim log As ILog = LogManager.GetLogger("TRADLogger")

            Dim bool_RetVal As Boolean = True

            Try
                SqliteControllerDB.fncExecuteNonQuery(String.Format("delete from {0} where {1};", _
                                                                  str_TableName, _
                                                                  str_Where) _
                                                    )
            Catch ex As Exception

                log.Error("fncDelete sqlite Controller DB Error ", ex)

                DisplayMessage.ErrorMsg(String.Concat("Failed to erase Temp Data, ", ex.ToString), "Error")
                bool_RetVal = False
            End Try

        End Function

        Public Function fncClearData(ByVal str_TableName As String) As Boolean
            Try
                SqliteControllerDB.fncExecuteNonQuery(String.Format("delete from {0};", _
                                                                  str_TableName) _
                                                    )
                Return True
            Catch
                Return False
            End Try
        End Function

#End Region

    End Module
End Namespace
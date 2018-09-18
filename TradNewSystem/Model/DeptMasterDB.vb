Option Strict On
Option Explicit On

Imports System.Data

Imports DapperLite.SqlMapper
Imports MySql.Data.MySqlClient

Imports TradNewSystem.Helpers
Imports TradNewSystem.PocoClass


Namespace Model
    Module DeptMasterDB
        Public Function GetAllDivisions() As List(Of DeptMaster)
            Dim deptMasters As List(Of DeptMaster) = Nothing

            Try
                Using connection As IDbConnection = New MySqlConnection( _
                    CommonLib.GenerateConnectionString _
                    )
                    connection.Open()

                    Dim sqlString As String = ( _
                        "SELECT * FROM DEPTMASTER ORDER BY DIVISIONCODE" _
                        )

                    deptMasters = CType( _
                        connection.Query(Of DeptMaster)(sqlString),  _
                        List(Of DeptMaster) _
                        )
                End Using
            Catch ex As Exception
                DisplayMessage.ErrorMsg(ex.Message, "DB Error")
            End Try

            Return deptMasters
        End Function
    End Module
End Namespace

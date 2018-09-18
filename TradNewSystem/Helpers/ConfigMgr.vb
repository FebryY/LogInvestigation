Option Strict On
Option Explicit On

Imports System.IO
Imports System.Reflection
Imports System.Xml.Serialization

Namespace Helpers
    Module ConfigMgr
        Public BIN_PATH As String = Path.GetDirectoryName( _
            Assembly.GetExecutingAssembly().GetName().CodeBase _
            )

        ' Default Config Data
        Private Const DEF_IP_ADDRESS As String = "10.112.0.10" '"169.254.0.1"
        Private Const DEF_PORT As String = "3306"
        Private Const DEF_USER As String = "root"
        Private Const DEF_PASSWORD As String = "trad2016" '"password"
        Private Const DEF_DB_NAME As String = "traddb" '"tradnew"
        Private Const DEF_LINE_CODE As String = "UNDEFINED"
        Private Const DEF_ID As String = "1"

        Private CONFIG_PATH As String = String.Format( _
            "{0}\config.xml", _
            BIN_PATH _
            )

        Public Function GetConfigData() As ConfigData
            Dim confData As ConfigData = Nothing

            If Not File.Exists(CONFIG_PATH) Then
                Using fStream As FileStream = New FileStream( _
                    CONFIG_PATH, _
                    FileMode.Create _
                    )
                    Dim xSerializer As XmlSerializer = New XmlSerializer( _
                        GetType(ConfigData) _
                        )

                    confData = New ConfigData

                    xSerializer.Serialize(fStream, confData)
                End Using
            Else
                Using fStream As FileStream = New FileStream( _
                    CONFIG_PATH, _
                    FileMode.Open _
                    )
                    Dim xSerializer As XmlSerializer = New XmlSerializer( _
                        GetType(ConfigData) _
                        )

                    confData = CType( _
                        xSerializer.Deserialize(fStream),  _
                        ConfigData _
                        )
                End Using
            End If

            Return confData
        End Function

        Public Function SaveConfigData(ByVal confData As ConfigData) As Boolean
            If Not File.Exists(CONFIG_PATH) Then
                DisplayMessage.ErrorMsg( _
                    "Config File Tidak Dapat Ditemukan", _
                    "Save Error" _
                    )

                Return False
            End If

            Using fStream As FileStream = New FileStream( _
                CONFIG_PATH, _
                FileMode.Truncate _
                )
                Dim xSerializer As New XmlSerializer(GetType(ConfigData))

                xSerializer.Serialize(fStream, confData)

                Return True
            End Using
        End Function

        Public Class ConfigData
            Public dbIPAddress As String
            Public dbUser As String
            Public dbPassword As String
            Public dbPort As String
            Public dbName As String
            Public lineCode As String
            Public ID As String

            Public Sub New()
                dbIPAddress = DEF_IP_ADDRESS
                dbUser = DEF_USER
                dbPassword = DEF_PASSWORD
                dbPort = DEF_PORT
                dbName = DEF_DB_NAME
                lineCode = DEF_LINE_CODE
                ID = DEF_ID
            End Sub
        End Class
    End Module
End Namespace
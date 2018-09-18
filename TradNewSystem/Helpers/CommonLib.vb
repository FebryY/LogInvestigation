Option Strict On
Option Explicit On

Imports System.Text.RegularExpressions
Imports System.Net.NetworkCredential

Namespace Helpers
    Module CommonLib
        Public Function GenerateConnectionString() As String
            Dim configData As ConfigMgr.ConfigData = ( _
                ConfigMgr.GetConfigData() _
                )

            Return String.Format( _
                GetRawConnectionString(), _
                configData.dbIPAddress, _
                configData.dbPort, _
                configData.dbUser, _
                configData.dbPassword, _
                configData.dbName _
                )
        End Function

        Public Function GetRawConnectionString() As String
            Return ( _
                "server={0}; " & _
                "port={1}; " & _
                "user id={2}; " & _
                "password={3}; " & _
                "database={4}; " & _
                "Convert Zero Datetime=True; " & _
                "ConnectionTimeout=30; " & _
                "DefaultCommandTimeout=45;" _
                )
        End Function

        Public Function IsValidIPv4(ByVal ipAddress As String) As Boolean
            Dim isIPAddress As Boolean = False

            Dim ipPattern As String = ( _
                "\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.)" & _
                "{3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b" _
                )

            Dim regex As New Regex(ipPattern)
            isIPAddress = regex.IsMatch(ipAddress)

            Return isIPAddress
        End Function

        Public Function GetStockStatus( _
            ByVal planQty As Integer, _
            ByVal actualQty As Integer _
            ) As String
            Dim stockStatus As String = String.Empty

            If actualQty = 0 Then
                stockStatus = "New"
            ElseIf actualQty < planQty Then
                stockStatus = "Partial"
            ElseIf actualQty = planQty Then
                stockStatus = "Completed"
            Else
                stockStatus = "Over"
            End If

            Return stockStatus
        End Function

        Public Function GetDataGridHeaderFont(ByVal dataFont As Font) As Font
            Dim FontName As String = dataFont.Name
            Dim FontSize As Single = dataFont.Size

            Return New Font(FontName, FontSize, FontStyle.Bold)
        End Function


        Private Function PingCheck(ByVal IPadd As String) As Boolean
            '    Try

            '        If My.mputer.Network.Ping(IPadd, 100) Then
            '            Return True
            '        Else
            '            Return False
            '        End If

            '    Catch ex As Exception
            '        Return False
            '    End Try
            'Try
            '    ipEntry = Net.Dns.GetHostEntry(DestinationHost.Text)
            '    destinationIP = ipEntry.AddressList(0)
            'Catch ex As Exception
            'End Try

            'If reply.Status = System.Net.NetworkInformation.IPStatus.Success Then

            '    Return True

            'Else

            '    Return False

            'End If
        End Function
    End Module
End Namespace
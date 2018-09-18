Option Strict On
Option Explicit On

Imports DNWA.BHTCL

Namespace Helpers
    Module BHTController
        Public Enum BeeperFrequency
            C = 261
            D = 2349
            E = 2637
            F = 2793
            G = 3135
            A = 3520
            B = 3951
        End Enum

        Public Enum ScannerCodeType
            Code39
            QrCode
        End Enum

        Public Enum ScannerReadMode
            Momentary
            AutoOff
            Alternate
            Continous
            TriggerRelease
        End Enum

        Private MyBeep As Beep = New Beep
        Private MyLED As LED = New LED
        Private activeLEDColor As LED.EN_COLOR

#Region "Scanner"
        Public Sub InitialiseScanner( _
            ByRef myScanner As Scanner, _
            ByVal codeType As ScannerCodeType, _
            ByVal mode As ScannerReadMode _
            )
            'If myScanner Is Nothing Then
            '    myScanner = New Scanner

            '    Select Case codeType
            '        Case ScannerCodeType.Code39
            '            myScanner.RdType = "M:1-30"
            '        Case ScannerCodeType.QrCode
            '            myScanner.RdType = "Q"
            '    End Select

            '    Select Case mode
            '        Case ScannerReadMode.Momentary
            '            myScanner.RdMode = "M"
            '        Case ScannerReadMode.AutoOff
            '            myScanner.RdMode = "F"
            '        Case ScannerReadMode.Alternate
            '            myScanner.RdMode = "A"
            '        Case ScannerReadMode.Continous
            '            myScanner.RdMode = "C"
            '        Case ScannerReadMode.TriggerRelease
            '            myScanner.RdMode = "R"
            '    End Select

            '    ScannerPortMgmt(myScanner, True)
            'End If
        End Sub

        Public Sub ScannerReadCode39(ByRef myScanner As Scanner)
            SetScannerReadType(myScanner, "M:1-30")
        End Sub

        Public Sub ScannerReadCodeQR(ByRef myScanner As Scanner)
            SetScannerReadType(myScanner, "Q")
        End Sub

        Public Sub DisposeScanner(ByRef myScanner As Scanner)
            If Not myScanner Is Nothing Then
                CloseScannerPort(myScanner)

                myScanner.Dispose()

                myScanner = Nothing
            End If
        End Sub

        Private Sub OpenScannerPort(ByRef myScanner As Scanner)
            ScannerPortMgmt(myScanner, True)
        End Sub

        Private Sub CloseScannerPort(ByRef myScanner As Scanner)
            ScannerPortMgmt(myScanner, False)
        End Sub

        Private Sub ScannerPortMgmt( _
            ByRef myScanner As Scanner, _
            ByVal portIsOpen As Boolean _
            )
            If myScanner Is Nothing Then Exit Sub

            If myScanner.PortOpen <> portIsOpen Then
                Try
                    myScanner.PortOpen = portIsOpen
                Catch ex As Exception
                    'DisplayMessage.ErrorMsg(ex.Message, "Scanner Port Error")
                    'myScanner.PortOpen = False
                    'ScannerPortMgmt2(myScanner, True)
                End Try
            End If
        End Sub

        Private Sub ScannerPortMgmt2( _
           ByRef myScanner As Scanner, _
           ByVal portIsOpen As Boolean _
           )
            If myScanner Is Nothing Then Exit Sub

            If myScanner.PortOpen <> portIsOpen Then
                Try
                    myScanner.PortOpen = portIsOpen
                Catch ex As Exception
                    'DisplayMessage.ErrorMsg(ex.Message, "Scanner Port Error")
                    myScanner.PortOpen = False
                End Try
            End If
        End Sub

        Private Sub SetScannerReadType( _
            ByRef myScanner As Scanner, _
            ByVal readType As String _
            )
            ScannerPortMgmt(myScanner, False)

            myScanner.RdType = readType

            ScannerPortMgmt(myScanner, True)
        End Sub
#End Region

#Region "LED"
        Public Sub ActivateLED( _
            ByVal activate As Boolean, _
            Optional ByVal color As DNWA.BHTCL.LED.EN_COLOR = Nothing _
            )
            If activate Then
                MyLED(LED.EN_DEVICE.BAR, color) = LED.EN_CTRL.ON
                activeLEDColor = color
            Else
                MyLED(LED.EN_DEVICE.BAR, activeLEDColor) = LED.EN_CTRL.OFF
            End If
        End Sub
#End Region

#Region "Beeper"
        Public Sub SoundWarning()
            ActivateBeeper(BeeperFrequency.F)
        End Sub

        Public Sub SoundOK()
            ActivateBeeper(BeeperFrequency.F, 1)
        End Sub

        Public Sub SoundConfirm()
            ActivateBeeper(BeeperFrequency.A)
        End Sub

        Private Sub ActivateBeeper( _
            ByVal frequency As Integer, _
            Optional ByVal onTime As Integer = 5 _
            )
            MyBeep.Frequency = frequency
            MyBeep.OnTime = onTime
            MyBeep.Item(Beep.Settings.EN_DEVICE.BUZZER) = Beep.EN_CTRL.ON
        End Sub
#End Region

#Region "Power Manager"
        Public Sub TurnOffTerminal()
            Dim confirm As Boolean = DisplayMessage.ConfirmationDialog( _
                "Apakah Anda Ingin Mematikan Terminal?", _
                "Konfirmasi" _
                )

            If confirm = True Then
                PwrMng.ShutDown(PwrMng.EN_SHUTDOWN_MODE.SUSPEND)
            End If
        End Sub
#End Region
    End Module
End Namespace


Option Strict On
Option Explicit On

Imports DNWA.BHTCL

Imports TradNewSystem.Model
Imports TradNewSystem.PocoClass

Namespace Helpers
    Module ScanValidator
        Public Function isValidFinalQrCode( _
            ByVal shipmentSplitCode As String() _
            ) As Boolean
            Return shipmentSplitCode.Length = 4
        End Function

        Public Function IsProductTrinCodeMatched( _
            ByRef myScanner As Scanner, _
            ByRef TimerScanner As Timer, _
            ByRef scannerIsOn As Boolean, _
            ByVal _trinPartNo As String, _
            ByVal scannedTrinCode As String _
            ) As Boolean
            Dim trinCodeMatched As Boolean = True

            If scannedTrinCode <> _trinPartNo Then
                TimerScanner.Enabled = False
                scannerIsOn = False

                BHTController.DisposeScanner(myScanner)
                BHTController.InitialiseScanner( _
                    myScanner, _
                    ScannerCodeType.QrCode, _
                    ScannerReadMode.Alternate _
                    )

                trinCodeMatched = False
            End If

            Return trinCodeMatched
        End Function

        Public Function IsItemQRCodeRegistered( _
            ByRef myScanner As Scanner, _
            ByRef TimerScanner As Timer, _
            ByRef scannerIsOn As Boolean, _
            ByVal productionAct As ProductionAct _
            ) As Boolean
            If productionAct Is Nothing Then
                TimerScanner.Enabled = False
                scannerIsOn = False

                BHTController.DisposeScanner(myScanner)
                BHTController.InitialiseScanner( _
                    myScanner, _
                    ScannerCodeType.QrCode, _
                    ScannerReadMode.Alternate _
                    )

                Return False
            End If

            Return True
        End Function

        Public Function HasBarcodeTagBeenScanned( _
            ByRef myScanner As Scanner, _
            ByRef TimerScanner As Timer, _
            ByRef scannerIsOn As Boolean, _
            ByVal tmpBarcodeTagData As List(Of String), _
            ByVal scannedBarcodeTag As String _
            ) As Boolean
            Dim barcodeHasBeenScanned As Boolean = False

            If tmpBarcodeTagData.Contains(scannedBarcodeTag) Then
                TimerScanner.Enabled = False
                scannerIsOn = False

                BHTController.DisposeScanner(myScanner)
                BHTController.InitialiseScanner( _
                    myScanner, _
                    ScannerCodeType.QrCode, _
                    ScannerReadMode.Alternate _
                    )

                barcodeHasBeenScanned = True
            End If

            Return barcodeHasBeenScanned
        End Function

        Public Function HasItemBeenShipped( _
            ByRef myScanner As Scanner, _
            ByRef TimerScanner As Timer, _
            ByRef scannerIsOn As Boolean, _
            ByVal scannedBarcodeTag As String _
            ) As Boolean
            Dim hasBeenShipped As Boolean = ShipmentActDB.IsBarcodeTagExist( _
                scannedBarcodeTag _
                )

            If hasBeenShipped Then
                'add by lutfi 9f
                Dim RemainStock As Integer = StockCardDB.GetRemainStockByTag( _
                scannedBarcodeTag _
                )
                If RemainStock <= 0 Then
                    TimerScanner.Enabled = False
                    scannerIsOn = False

                    BHTController.DisposeScanner(myScanner)
                    BHTController.InitialiseScanner( _
                        myScanner, _
                        ScannerCodeType.QrCode, _
                        ScannerReadMode.Alternate _
                        )
                Else
                    hasBeenShipped = False
                End If
            End If
                Return hasBeenShipped
        End Function

        
    End Module
End Namespace

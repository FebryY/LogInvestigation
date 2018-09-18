Imports System.Globalization

Imports TradNewSystem.Helpers
Imports TradNewSystem.Model
Imports TradNewSystem.PocoClass


Public Class ShipmentDuplicateBarcodes
    Private _shipmentScan As ShipmentScan
    Private duplicatedBarcodes As New List(Of String)
    Private totalActQtyOfDuplicatedBarcodes As Integer

    Public Sub New( _
        ByVal _shipmentScan As ShipmentScan, _
        ByVal duplicateShipActs As List(Of ShipmentAct) _
        )
        InitializeComponent()

        Me._shipmentScan = _shipmentScan

        For Each shipAct As ShipmentAct In duplicateShipActs
            Dim str(0) As String

            str(0) = shipAct.BARCODETAG
            duplicatedBarcodes.Add(shipAct.BARCODETAG)

            Dim itm As New ListViewItem(str)
            ListView1.Items.Add(itm)

            totalActQtyOfDuplicatedBarcodes += shipAct.ACTQTY
        Next
    End Sub

#Region "Key Press"
    Private Sub ActivateCommonShortcut( _
        ByVal e As KeyEventArgs _
        )
        Select Case e.KeyCode
            Case Windows.Forms.Keys.F1
                ClearDuplicateBarcodes()
            Case Windows.Forms.Keys.F2
                Close()
        End Select
    End Sub

    Private Sub ButtonFullScreen_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonFullScreen.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ShipmentDuplicateBarcodes_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles MyBase.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ListView1_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ListView1.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonClear_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonClear.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonBack_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonBack.KeyUp
        ActivateCommonShortcut(e)
    End Sub
#End Region

#Region "Button Click"
    Private Sub ButtonFullScreen_Click( _
        ByVal sender As System.Object, _
        ByVal e As EventArgs _
        ) Handles ButtonFullScreen.Click
        ToggleTaskbar.EnableDisable()
    End Sub

    Private Sub ButtonClear_Click( _
        ByVal sender As System.Object, _
        ByVal e As EventArgs _
        ) Handles ButtonClear.Click
        ClearDuplicateBarcodes()
    End Sub

    Private Sub ButtonBack_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonBack.Click
        Close()
    End Sub
#End Region

    Private Sub ClearDuplicateBarcodes()
        Dim confirmed As Boolean = DisplayMessage.ConfirmationDialog( _
            "Hapus Duplikat Barcode Tag Dari Hasil Scan?", _
            "Konfirmasi" _
            )

        If confirmed Then
            DeleteDuplicateBarcodesFromShipmentScanData()
            UpdateItemQtyOnShipmentScanWindow()

            ListView1.Clear()
            DisplayMessage.OkMsg( _
            "Duplikat Barcode Tag Telah Dihapus!", _
            "Sukses" _
            )
            Close()
        End If
    End Sub

    Private Sub DeleteDuplicateBarcodesFromShipmentScanData()
        For index As Integer = ( _
            _shipmentScan.scannedBarcodes.Count - 1 _
            ) To 0 Step -1
            If duplicatedBarcodes.Contains( _
                _shipmentScan.scannedBarcodes(index) _
                ) Then
                _shipmentScan.scannedBarcodes.RemoveAt(index)
            End If
        Next

        If _shipmentScan.scannedBarcodes.Count = 0 Then
            _shipmentScan.tmpShipActDataInsert.Clear()
            _shipmentScan.tmpShipActDataUpdate.Clear()
            _shipmentScan.tmpStockCardDataInsert.Clear()
            _shipmentScan.tmpStockCardDataUpdate.Clear()

            _shipmentScan.lastScannedBarcode = String.Empty
            _shipmentScan.textBoxScannedTag.Text = String.Empty
        Else
            For index As Integer = ( _
                _shipmentScan.tmpShipActDataInsert.Count - 1 _
                ) To 0 Step -1
                Dim shipmentBarcode As String = ( _
                    _shipmentScan.tmpShipActDataInsert(index) _
                    (ShipmentActValues.BarcodeTag) _
                    )

                If duplicatedBarcodes.Contains(shipmentBarcode) Then
                    _shipmentScan.tmpShipActDataInsert.RemoveAt(index)
                End If
            Next

            For index As Integer = ( _
                _shipmentScan.tmpShipActDataUpdate.Count - 1 _
                ) To 0 Step -1
                Dim shipmentBarcode As String = ( _
                    _shipmentScan.tmpShipActDataUpdate(index) _
                    (ShipmentActValues.BarcodeTag) _
                    )
                If duplicatedBarcodes.Contains(shipmentBarcode) Then
                    _shipmentScan.tmpShipActDataUpdate.RemoveAt(index)
                End If
            Next
        End If
    End Sub

    Private Sub UpdateItemQtyOnShipmentScanWindow()
        Dim currentCulture As CultureInfo = CultureInfo.CurrentCulture

        Dim _actualQty As Integer = CInt( _
            Convert.ToDouble( _
                _shipmentScan.labelActualQtyValue.Text, _
                currentCulture _
                ) _
            )
        Dim _remainingQty As Integer = CInt( _
            Convert.ToDouble( _
                _shipmentScan.labelActualQtyValue.Text, _
                currentCulture _
                ) _
            )

        _shipmentScan.totalScannedQty = ( _
            _shipmentScan.totalScannedQty - totalActQtyOfDuplicatedBarcodes _
            )
        _shipmentScan.actualQty = _actualQty - totalActQtyOfDuplicatedBarcodes
        _shipmentScan.labelActualQtyValue.Text = String.Format( _
            "{0:n0}", _
            _shipmentScan.actualQty _
            )
        _shipmentScan.labelRemainingQtyValue.Text = String.Format( _
            "{0:n0}", _
            _remainingQty + totalActQtyOfDuplicatedBarcodes _
            )
    End Sub
End Class
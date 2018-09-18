Imports TradNewSystem.Helpers
Imports TradNewSystem.Model
Imports TradNewSystem.PocoClass

Public Class StocktakeDuplicateBarcodes
    Private _stocktakeScan As StocktakeScan
    Private duplicatedBarcodes As New List(Of String)
    Private totalScanQtyOfDuplicatedBarcodes As Integer
    Private totalDuplicatedBarcodes As Integer

    Public Sub New( _
        ByVal _stocktakeScan As StocktakeScan, _
        ByVal duplicatedStocktakes As List(Of Stocktake) _
        )
        InitializeComponent()

        Me._stocktakeScan = _stocktakeScan
        totalDuplicatedBarcodes = duplicatedStocktakes.Count

        For Each stocktake As Stocktake In duplicatedStocktakes
            Dim str(0) As String

            str(0) = stocktake.BARCODETAG
            duplicatedBarcodes.Add(stocktake.BARCODETAG)

            Dim itm As New ListViewItem(str)
            ListView1.Items.Add(itm)

            totalScanQtyOfDuplicatedBarcodes += stocktake.SCANNEDQTY
        Next
    End Sub

#Region "Key Press"
    Private Sub ActivateCommonShortcut(ByVal e As KeyEventArgs)
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

    Private Sub StocktakeDuplicateBarcodes_KeyUp( _
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
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonFullScreen.Click
        ToggleTaskbar.EnableDisable()
    End Sub

    Private Sub ButtonClear_Click( _
        ByVal sender As Object, _
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
            DeleteDuplicateBarcodesFromStocktakeScanData()
            UpdateItemQtyOnStocktakeScanWindow()

            ListView1.Clear()
            DisplayMessage.OkMsg( _
                "Duplikat Barcode Tag Telah Dihapus!", _
                "Sukses" _
                )
            Close()
        End If
    End Sub

    Private Sub DeleteDuplicateBarcodesFromStocktakeScanData()
        For index As Integer = _stocktakeScan.tmpBarcodeTagData.Count - 1 To 0 Step -1
            If duplicatedBarcodes.Contains( _
                _stocktakeScan.tmpBarcodeTagData(index) _
                ) Then
                _stocktakeScan.tmpBarcodeTagData.RemoveAt(index)
            End If
        Next

        If _stocktakeScan.tmpBarcodeTagData.Count = 0 Then
            _stocktakeScan.tmpStocktakeDataInsert.Clear()
            _stocktakeScan.tmpStocktakeDataUpdate.Clear()

            _stocktakeScan.lastScannedBarcodeTag = String.Empty
            _stocktakeScan.textBoxScanTag.Text = String.Empty
        Else
            For index As Integer = ( _
                _stocktakeScan.tmpStocktakeDataInsert.Count - 1 _
                ) To 0 Step -1
                Dim barcodeTag As String = ( _
                    _stocktakeScan.tmpStocktakeDataInsert(index) _
                        (StocktakeValues.BarcodeTag) _
                    )
                If duplicatedBarcodes.Contains(barcodeTag) Then
                    _stocktakeScan.tmpStocktakeDataInsert.RemoveAt(index)
                End If
            Next

            For index As Integer = ( _
                _stocktakeScan.tmpStocktakeDataUpdate.Count - 1 _
                ) To 0 Step -1
                Dim barcodeTag As String = ( _
                    _stocktakeScan.tmpStocktakeDataUpdate(index) _
                        (StocktakeValues.BarcodeTag) _
                    )
                If duplicatedBarcodes.Contains(barcodeTag) Then
                    _stocktakeScan.tmpStocktakeDataUpdate.RemoveAt(index)
                End If
            Next
        End If
    End Sub

    Private Sub UpdateItemQtyOnStocktakeScanWindow()
        _stocktakeScan.countedQty = ( _
            _stocktakeScan.countedQty - totalScanQtyOfDuplicatedBarcodes _
            )
        _stocktakeScan.LabelCountedQtyValue.Text = ( _
            String.Format("{0:n0}", _stocktakeScan.countedQty) _
            )

        _stocktakeScan.tagCount = ( _
            _stocktakeScan.tagCount - totalDuplicatedBarcodes _
            )
        _stocktakeScan.LabelTagCountValue.Text = ( _
            String.Format("{0:n0}", _stocktakeScan.tagCount) _
            )
    End Sub
End Class
Imports TradNewSystem.Helpers
Imports TradNewSystem.Model
Imports TradNewSystem.PocoClass

Public Class OfflineStocktakeDuplicateBarcodes
    Private _offlineStocktake As OfflineStocktake
    Private duplicatedBarcodes As New List(Of String)
    Private totalScanQtyOfDuplicatedBarcodes As Integer
    Private totalDuplicatedBarcodes As Integer

    Public Sub New( _
        ByVal _offlineStocktake As OfflineStocktake, _
        ByVal duplicatedStocktakes As List(Of Stocktake) _
        )
        InitializeComponent()

        Me._offlineStocktake = _offlineStocktake
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

    Private Sub OfflineStocktakeDuplicateBarcodes_KeyUp( _
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
        For index As Integer = _offlineStocktake. _
            tmpBarcodeTagData.Count - 1 To 0 Step -1
            If duplicatedBarcodes.Contains( _
                _offlineStocktake.tmpBarcodeTagData(index)) Then
                _offlineStocktake.tmpBarcodeTagData.RemoveAt(index)
            End If
        Next

        If _offlineStocktake.tmpBarcodeTagData.Count = 0 Then
            _offlineStocktake.tmpStocktakeDataAll.Clear()

            _offlineStocktake.lastScannedBarcodeTag = String.Empty
            _offlineStocktake.TextBoxScanTag.Text = String.Empty
            _offlineStocktake.LabelTrinPartNoValue.Text = "-"
            _offlineStocktake.LabelCountedQtyValue.Text = "0"
        Else
            For index As Integer = _offlineStocktake. _
                tmpStocktakeDataAll.Count - 1 To 0 Step -1
                Dim barcodeTag As String = _offlineStocktake. _
                    tmpStocktakeDataAll(index)(StocktakeValues.BarcodeTag)

                If duplicatedBarcodes.Contains(barcodeTag) Then
                    _offlineStocktake.tmpStocktakeDataAll.RemoveAt(index)
                End If
            Next
        End If
    End Sub

    Private Sub UpdateItemQtyOnStocktakeScanWindow()
        _offlineStocktake.tagCount = ( _
            _offlineStocktake.tagCount - totalDuplicatedBarcodes _
            )
        _offlineStocktake.LabelTagCountValue.Text = ( _
            String.Format("{0:n0}", _offlineStocktake.tagCount) _
            )
    End Sub
End Class
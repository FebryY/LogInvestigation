Option Strict On
Option Explicit On

Imports System.Data

Imports TradNewSystem.Helpers
Imports TradNewSystem.Model
Imports TradNewSystem.PocoClass

Imports DataGridCustomColumns
Imports DNWA.BHTCL

Public Class StocktakeList
    Friend Enum DataGridColumns
        Model
        TrinPartNo
        TotalQty
        CountedQty
        Status
        Division
    End Enum

    Protected Friend dataSource As DataSet

    Private _stocktakeFilter As StocktakeFilter

    Private activeFilter As String
    Private bindingSource As New BindingSource

    Private _selectedStocktakePeriod As Date
    Private _selectedDivision As String

    Private gridIsSelected As Boolean = True
    Private hasLoaded As Boolean = False

    Public Property SelectedStocktakePeriod() As Date
        Get
            Return _selectedStocktakePeriod
        End Get

        Set(ByVal value As Date)
            _selectedStocktakePeriod = value
        End Set
    End Property

    Public Property SelectedDivision() As String
        Get
            Return _selectedDivision
        End Get

        Set(ByVal value As String)
            _selectedDivision = value
        End Set
    End Property

    Public Sub New(ByVal _stocktakeFilter As StocktakeFilter)
        InitializeComponent()

        Me._stocktakeFilter = _stocktakeFilter
        Me.DataGrid1.TableStyles.Add(Me.DataGridTableStyle1)
    End Sub

    Private Sub StocktakeList_Activated( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles MyBase.Activated
        If Not hasLoaded Then
            _stocktakeFilter.nowLoadingWindow.Close()
            _stocktakeFilter.nowLoadingWindow = Nothing

            hasLoaded = True
        End If
    End Sub

    Private Sub StocktakeList_Load( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles MyBase.Load
        Try
            If RF.SYNCHRONIZE(RF.SYNC_CHECK) <> 0 Then
                DisplayMessage.ErrorMsg("Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
                    "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.", "Error")
                Exit Sub
            End If
        Catch ex As Exception
            If Err.Number = 5 Then
                DisplayMessage.ErrorMsg("Koneksi Wifi di HT tertutup." & vbCrLf & _
                    "Tunggu beberapa detik dan ulangi lagi.", "Error")
                Dim MyRf As RF
                MyRf = New RF()
                MyRf.OpenMode = RF.EN_OPEN_MODE.CONTINUOUSLY
                MyRf.Open = True
                Exit Sub
            End If
        End Try
        LoadGridWithStocktakeData()
    End Sub

    Private Sub DataGrid1_CurrentCellChanged( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles DataGrid1.CurrentCellChanged
        DataGrid1.Select(DataGrid1.CurrentRowIndex)
    End Sub

#Region "Load Stocktake Data"
    Private Sub LoadGridWithStocktakeData()
        dataSource = New DataSet

        Dim dataTable As New DataTable("Stocktake")

        dataTable.Columns.Add("Model", Type.GetType("System.String"))
        dataTable.Columns.Add("TRIN Part No", Type.GetType("System.String"))
        dataTable.Columns.Add("Total Qty", Type.GetType("System.String"))
        dataTable.Columns.Add("Counted Qty", Type.GetType("System.String"))
        dataTable.Columns.Add("Status", Type.GetType("System.String"))
        dataTable.Columns.Add("Division", Type.GetType("System.String"))

        ' Add Row Data
        Dim stocktakes As List(Of StocktakeSummary) = ( _
            StocktakeSummaryDB.GetStocktakeSummaries( _
                _selectedStocktakePeriod, _
                _selectedDivision _
                ) _
            )

        If Not stocktakes Is Nothing Then
            Dim dataRow As DataRow = Nothing

            For Each stocktake As StocktakeSummary In stocktakes
                Dim stocktakeStatus As String = CommonLib.GetStockStatus( _
                    CInt(stocktake.TOTALSTOCKQTY), _
                    CInt(stocktake.TOTALSCANNEDQTY) _
                    )

                ' Inserting Data
                dataRow = dataTable.NewRow

                dataRow(DataGridColumns.Model) = stocktake.MODEL
                dataRow(DataGridColumns.TrinPartNo) = stocktake.TRINPARTNO
                dataRow(DataGridColumns.TotalQty) = String.Format( _
                    "{0:n0}", stocktake.TOTALSTOCKQTY _
                    )
                dataRow(DataGridColumns.CountedQty) = String.Format( _
                    "{0:n0}", stocktake.TOTALSCANNEDQTY _
                    )
                dataRow(DataGridColumns.Status) = stocktakeStatus
                dataRow(DataGridColumns.Division) = stocktake.DIVISIONCODE

                dataTable.Rows.Add(dataRow)
            Next
        End If

        dataSource.Tables.Add(dataTable)

        SetupTableStyles()

        RadioButtonAll.Focus()
        DataGrid1.Focus()
    End Sub

    Private Sub SetupTableStyles()
        Dim stocktakeData As DataTable = dataSource.Tables(0)

        ' StatusColumnIndex -> column index for stocktake status, 
        ' needed for colouring the table
        Dim dataGridCustomColumns(5) As DataGridCustomTextBoxColumn

        For index As Integer = 0 To dataGridCustomColumns.Count - 1
            dataGridCustomColumns(index) = New DataGridCustomTextBoxColumn

            With dataGridCustomColumns(index)
                .Owner = DataGrid1
                .HeaderText = stocktakeData.Columns(index).ColumnName
                .MappingName = stocktakeData.Columns(index).ColumnName
                .StatusColumnIndex = DataGridColumns.Status

                If index = DataGridColumns.Model Or _
                    index = DataGridColumns.TrinPartNo Then
                    .Width = 108
                End If

                If index = DataGridColumns.TotalQty Or _
                index = DataGridColumns.CountedQty Then
                    .Width = 57
                End If

                If index = DataGridColumns.Status Then
                    .Width = 70
                End If

                If index = DataGridColumns.Division Then
                    .Width = 90
                End If

                .ReadOnly = True
            End With

            DataGridTableStyle1.GridColumnStyles.Add( _
                dataGridCustomColumns(index) _
                )
        Next

        DataGridTableStyle1.MappingName = stocktakeData.TableName

        bindingSource.DataSource = stocktakeData

        DataGrid1.DataSource = bindingSource
    End Sub
#End Region

#Region "Key Press"
    Private Sub ActivateCommonShortcut(ByVal e As KeyEventArgs)
        Select Case e.KeyCode
            Case Windows.Forms.Keys.F1
                ProcessSelectedRow()
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

    Private Sub ButtonSelectGrid_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonSelectGrid.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub StocktakeList_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles MyBase.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub DataGrid1_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles DataGrid1.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub RadioButtonAll_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles RadioButtonAll.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub RadioButtonNewPartial_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles RadioButtonNewPartial.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub RadioButtonCompleted_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles RadioButtonCompleted.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub LinkLabelNext_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        )
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonNext_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonNext.KeyUp
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

    Private Sub ButtonNext_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonNext.Click
        ProcessSelectedRow()
    End Sub

    Private Sub ButtonBack_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonBack.Click
        Close()
    End Sub

    Private Sub ButtonSelectGrid_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonSelectGrid.Click
        If gridIsSelected Then
            SetGridAsInactive()
        Else
            SetGridAsActive()
        End If
    End Sub
#End Region

    Public Sub ProcessSelectedRow()
        If bindingSource.Count = 0 Then
            DisplayMessage.ErrorMsg( _
                "Tidak ada produk untuk di stocktake", _
                "Error" _
                )
            Exit Sub
        End If

        If Not gridIsSelected Then
            DisplayMessage.ErrorMsg( _
                "Pilihlah item dari daftar terdahulu", _
                "Error" _
                )
            Exit Sub
        End If

        Dim currentRowIndex As Integer = DataGrid1.CurrentCell.RowNumber

        Dim stocktakeScanWindow As New StocktakeScan(Me)
        stocktakeScanWindow.ShowDialog()
        stocktakeScanWindow = Nothing
        If bindingSource.Count > 0 Then
            DataGrid1.Select(0)
        End If
        'DataGrid1.Select(currentRowIndex)
    End Sub

#Region "Radio Button OnChange"
    Private Sub UpdateBindingFilter()
        If Not bindingSource Is Nothing Then
            bindingSource.Filter = activeFilter
        End If

        Try
            If bindingSource.Count > 0 Then
                DataGrid1.Select(0)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub RadioButtonAll_CheckedChanged( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles RadioButtonAll.CheckedChanged
        activeFilter = ""

        UpdateBindingFilter()
    End Sub

    Private Sub RadioButtonNewPartial_CheckedChanged( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles RadioButtonNewPartial.CheckedChanged
        activeFilter = "`Status` = 'New' OR `Status` = 'Partial'"

        UpdateBindingFilter()
    End Sub

    Private Sub RadioButtonCompleted_CheckedChanged( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles RadioButtonCompleted.CheckedChanged
        activeFilter = "`Status` = 'Completed' OR `Status` = 'Over'"

        UpdateBindingFilter()
    End Sub
#End Region

    Private Sub DataGrid1_GotFocus( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles DataGrid1.GotFocus
        If Not gridIsSelected Then
            SetGridAsActive()
        End If
    End Sub

    Private Sub SetGridAsActive()
        DataGrid1.SelectionBackColor = SystemColors.ActiveCaption
        DataGrid1.SelectionForeColor = SystemColors.ActiveCaptionText

        gridIsSelected = True

        ButtonSelectGrid.Text = "Unselect"
    End Sub

    Private Sub SetGridAsInactive()
        If bindingSource.Count = 0 Then Exit Sub

        gridIsSelected = False

        ButtonSelectGrid.Text = "Select"

        Dim stocktakeStatus As String = DataGrid1( _
            DataGrid1.CurrentCell.RowNumber, DataGridColumns.Status _
            ).ToString()

        If stocktakeStatus = "New" Then
            DataGrid1.SelectionBackColor = Color.FromArgb(220, 232, 243)
        End If

        If stocktakeStatus = "Partial" Then
            DataGrid1.SelectionBackColor = Color.FromArgb(255, 230, 230)
        End If

        If stocktakeStatus = "Completed" Then
            DataGrid1.SelectionBackColor = Color.FromArgb(230, 255, 230)
        End If

        DataGrid1.SelectionForeColor = SystemColors.ControlText
    End Sub
End Class
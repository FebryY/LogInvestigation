Option Strict On
Option Explicit On

Imports System.Data

Imports DataGridCustomColumns

Imports TradNewSystem.Helpers
Imports TradNewSystem.Model
Imports TradNewSystem.PocoClass


Public Class ShipmentList
    Friend Enum DataGridColumns
        ShipmentDate
        Model
        TrinPartNo
        PlanQty
        ActQty
        CustomerShortName
        SONumber
        Status
        PlantNo
        SID
        DivisionCode
        CustomerCode
        LastIndex = CustomerCode
    End Enum

    Protected Friend dataSource As DataSet

    Private headerFont As Font
    Private dataGridColumnSizes() As Integer
    Private activeFilter As String
    Private bindingSource As New BindingSource

    Private businessHour As TimeSpan

    Private _shipmentFilter As ShipmentFilter
    Private _selectedShipmentDate As Date
    Private _selectedCustomerCode As String
    Private _includePrevDateShipment As Boolean

    Private gridIsSelected As Boolean = True
    Private hasLoaded As Boolean = False

    Public WriteOnly Property SelectedShipmentDate() As Date
        Set(ByVal value As Date)
            _selectedShipmentDate = value
        End Set
    End Property

    Public WriteOnly Property SelectedCustomerCode() As String
        Set(ByVal value As String)
            _selectedCustomerCode = value
        End Set
    End Property

    Public WriteOnly Property IncludePrevDateShipment() As Boolean
        Set(ByVal value As Boolean)
            _includePrevDateShipment = value
        End Set
    End Property

    Public Sub New(ByVal _shipmentFilter As ShipmentFilter)
        InitializeComponent()

        Me._shipmentFilter = _shipmentFilter

        Me.DataGrid1.TableStyles.Add(Me.DataGridTableStyle1)
    End Sub

    Private Sub ShipmentList_Activated( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles MyBase.Activated
        If Not hasLoaded Then
            _shipmentFilter.nowLoadingWindow.Close()
            _shipmentFilter.nowLoadingWindow = Nothing

            hasLoaded = True
        End If
    End Sub

    Private Sub ShipmentList_Load( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles MyBase.Load
        businessHour = BusinessDayDB.GetBusinessHour()

        headerFont = GetDataGridHeaderFont(DataGrid1.Font)
        LoadGridWithShipmentData()
    End Sub

    Private Sub DataGrid1_CurrentCellChanged( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles DataGrid1.CurrentCellChanged
        DataGrid1.Select(DataGrid1.CurrentRowIndex)
    End Sub

#Region "Load Shipment Data"
    Private Sub LoadGridWithShipmentData(Optional ByVal FirstLoad As Boolean = True)
        dataSource = New DataSet

        Dim dataTable As New DataTable("Shipment")

        dataTable.Columns.Add("Date & Time", Type.GetType("System.String"))
        dataTable.Columns.Add("Model", Type.GetType("System.String"))
        dataTable.Columns.Add("TRIN Part No", Type.GetType("System.String"))
        dataTable.Columns.Add("Plan", Type.GetType("System.String"))
        dataTable.Columns.Add("Act", Type.GetType("System.String"))
        dataTable.Columns.Add("Customer", Type.GetType("System.String"))
        dataTable.Columns.Add("SO No", Type.GetType("System.String"))
        dataTable.Columns.Add("Status", Type.GetType("System.String"))
        dataTable.Columns.Add("Plant No", Type.GetType("System.String"))
        dataTable.Columns.Add("SID", Type.GetType("System.Int32"))
        dataTable.Columns.Add("Division Code", Type.GetType("System.String"))
        dataTable.Columns.Add("Customer Code", Type.GetType("System.String"))

        dataGridColumnSizes = New Integer(2) {}

        SetDataGridColumnSizes("Customer", 0)
        SetDataGridColumnSizes("SO No", 1)
        SetDataGridColumnSizes("Status", 2)

        ' Add Row Data
        Dim shipments As List(Of ShipmentSummary) = ShipmentSummaryDB. _
            GetShipmentSummaries( _
                _selectedShipmentDate, _
                businessHour, _
                _includePrevDateShipment, _
                _selectedCustomerCode _
                )

        If Not shipments Is Nothing Then
            Dim dataRow As DataRow = Nothing

            For Each shipment As ShipmentSummary In shipments
                If shipment.SHIPMENTDATE.Date < _selectedShipmentDate _
                    And shipment.SUMACTQTY >= shipment.PLANQTY Then
                    Continue For
                End If

                Dim shipmentStatus As String = CommonLib. _
                    GetStockStatus(shipment.PLANQTY, CInt(shipment.SUMACTQTY))

                ' Inserting Data
                dataRow = dataTable.NewRow

                dataRow(DataGridColumns.ShipmentDate) = ( _
                    shipment.SHIPMENTDATE.ToString("MM/dd/yy HH:mm") _
                    )
                dataRow(DataGridColumns.Model) = shipment.MODEL
                dataRow(DataGridColumns.TrinPartNo) = shipment.TRINPARTNO
                dataRow(DataGridColumns.PlanQty) = ( _
                    String.Format("{0:n0}", shipment.PLANQTY) _
                    )
                dataRow(DataGridColumns.ActQty) = ( _
                    String.Format("{0:n0}", shipment.SUMACTQTY) _
                    )
                dataRow(DataGridColumns.CustomerShortName) = shipment.SHORTNAME
                dataRow(DataGridColumns.SONumber) = shipment.SONUMBER
                dataRow(DataGridColumns.Status) = shipmentStatus
                dataRow(DataGridColumns.PlantNo) = shipment.PLANTNO
                dataRow(DataGridColumns.SID) = shipment.SID
                dataRow(DataGridColumns.DivisionCode) = shipment.DIVISIONCODE
                dataRow(DataGridColumns.CustomerCode) = shipment.CUSTOMERCODE

                SetDataGridColumnSizes(_selectedCustomerCode, 0)
                SetDataGridColumnSizes(shipment.SONUMBER, 1)
                SetDataGridColumnSizes(shipmentStatus, 2)

                dataTable.Rows.Add(dataRow)
            Next
        End If

        dataSource.Tables.Add(dataTable)

        'Modify at lutfi

        SetupTableStyles()
        If FirstLoad Then
            RadioButtonAll.Focus()
        End If

        'SetupTableStyles()
        'RadioButtonAll.Focus()

        DataGrid1.Focus()
    End Sub

    Private Sub SetupTableStyles()
        Dim shipmentData As DataTable = dataSource.Tables(0)

        ' StatusColumnIndex -> column index for shipment status, 
        ' needed for colouring the table
        Dim arraySize As Integer = DataGridColumns.LastIndex
        Dim dataGridCustomColumns(arraySize) As DataGridCustomTextBoxColumn

        For index As Integer = 0 To dataGridCustomColumns.Count - 1
            dataGridCustomColumns(index) = New DataGridCustomTextBoxColumn

            With dataGridCustomColumns(index)
                .Owner = DataGrid1
                .HeaderText = shipmentData.Columns(index).ColumnName
                .MappingName = shipmentData.Columns(index).ColumnName
                .StatusColumnIndex = DataGridColumns.Status

                If index = DataGridColumns.ShipmentDate Then
                    .Width = 94
                End If

                If index = DataGridColumns.Model Or _
                    index = DataGridColumns.TrinPartNo Then
                    .Width = 108
                End If

                If index = DataGridColumns.PlanQty Or _
                index = DataGridColumns.ActQty Then
                    .Width = 45
                End If

                If index = DataGridColumns.CustomerShortName Then
                    .Width = dataGridColumnSizes(0)
                End If

                If index = DataGridColumns.SONumber Then
                    .Width = dataGridColumnSizes(1)
                End If

                If index = DataGridColumns.Status Then
                    .Width = dataGridColumnSizes(2)
                End If

                If index > DataGridColumns.Status Then
                    .Width = -1
                End If

                .ReadOnly = True
            End With

            DataGridTableStyle1.GridColumnStyles.Add( _
                dataGridCustomColumns(index) _
                )
        Next

        DataGridTableStyle1.MappingName = shipmentData.TableName
        bindingSource.DataSource = shipmentData
        DataGrid1.DataSource = bindingSource
    End Sub

#End Region

#Region "Resize Grid Columns"
    Private Sub SetDataGridColumnSizes( _
        ByVal text As String, _
        ByVal index As Integer _
        )
        If Not dataGridColumnSizes Is Nothing Or _
            dataGridColumnSizes.Length - 1 >= index Then

            Dim textWidthSize As Integer = MeasureTextWidth(text, headerFont)

            If dataGridColumnSizes(index) < textWidthSize + 10 Then
                dataGridColumnSizes(index) = textWidthSize + 10
            End If
        End If
    End Sub

    Public Function MeasureTextWidth( _
        ByVal text As String, _
        ByVal font As Font _
        ) As Integer
        Dim instance As Graphics = DataGrid1.CreateGraphics

        Return CInt(instance.MeasureString(text, font).Width)
    End Function
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

    Private Sub ShipmentList_KeyUp( _
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

    Private Sub ProcessSelectedRow()
        If bindingSource.Count = 0 Then
            DisplayMessage.ErrorMsg("Tidak ada produk untuk dikirim", "Error")
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

        Try
            Dim shipmentStatus As String = ( _
                DataGrid1(currentRowIndex, DataGridColumns.Status).ToString() _
                )
            If shipmentStatus = "Completed" Then
                DisplayMessage.ErrorMsg( _
                    "Shipment untuk produk yang terpilih telah selesai", _
                    "Error" _
                    )
            Else
                Dim trinPartNo As String = DataGrid1( _
                    DataGrid1.CurrentCell.RowNumber, _
                    DataGridColumns.TrinPartNo _
                    ).ToString()

                Dim totalStock As TotalStock = StockCardDB. _
                    GetTotalStockInOut(trinPartNo)

                Dim remainingStock As Integer = ( _
                    totalStock.StockIn - totalStock.StockOut _
                    )

                If remainingStock <= 0 Then
                    DisplayMessage.ErrorMsg( _
                        "Jumlah stock yang tersisa = " & remainingStock, _
                        "Error" _
                        )
                    Exit Sub
                End If

                Dim shipmentScanWindow As New ShipmentScan(Me)
                shipmentScanWindow.ShowDialog()
                shipmentScanWindow = Nothing

                'Add by lutfi
                DataGridTableStyle1 = New DataGridTableStyle
                LoadGridWithShipmentData(False)
                If RadioButtonNewPartial.Checked Then
                    activeFilter = "`Status` = 'New' OR `Status` = 'Partial'"
                ElseIf RadioButtonCompleted.Checked Then
                    activeFilter = "`Status` = 'Completed'"
                End If
                UpdateBindingFilter()
                '===
                'If DataGrid1.CurrentRowIndex <> -1 Then
                '    DataGrid1.Select(currentRowIndex)
                'End If
            End If
        Catch ex As Exception
            DisplayMessage.ErrorMsg(ex.Message, "Error")
        End Try
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
        activeFilter = "`Status` = 'Completed'"

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

        Dim shipmentStatus As String = DataGrid1( _
            DataGrid1.CurrentCell.RowNumber, _
            DataGridColumns.Status _
            ).ToString()

        If shipmentStatus = "New" Then
            DataGrid1.SelectionBackColor = Color.FromArgb(220, 232, 243)
        End If

        If shipmentStatus = "Partial" Then
            DataGrid1.SelectionBackColor = Color.FromArgb(255, 230, 230)
        End If

        If shipmentStatus = "Completed" Then
            DataGrid1.SelectionBackColor = Color.FromArgb(230, 255, 230)
        End If

        DataGrid1.SelectionForeColor = SystemColors.ControlText
    End Sub
End Class
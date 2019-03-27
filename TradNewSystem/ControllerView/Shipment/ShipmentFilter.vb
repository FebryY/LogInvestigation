Option Strict On
Option Explicit On

Imports System.Globalization

Imports TradNewSystem.Helpers
Imports TradNewSystem.Model
Imports TradNewSystem.PocoClass
Imports DNWA.BHTCL
Imports log4net

Public Class ShipmentFilter
    Protected Friend nowLoadingWindow As NowLoading

    Private _mainMenuWindow As MainMenu

    Private hasLoaded As Boolean = False
    Private processKeypadPress As Boolean = True

    Private currentSetDate As DateTime

    Public Sub New(ByVal _mainMenuWindow As MainMenu)
        InitializeComponent()

        Me._mainMenuWindow = _mainMenuWindow
    End Sub

    Private Sub ShipmentFilter_Activated( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles MyBase.Activated
        If Not hasLoaded Then
            _mainMenuWindow.nowLoadingWindow.Close()
            _mainMenuWindow.nowLoadingWindow = Nothing

            hasLoaded = True
        End If
    End Sub

    Private Sub ShipmentFilter_Load( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles MyBase.Load
        DateTimePicker1.Value = Today
        'If Not WifiConectionCheck() Then Exit Sub (disable by Febry 5 December 2018)
        LoadCustomerDataToComboBox()
    End Sub

    Private Sub LoadCustomerDataToComboBox()
        Dim customers As List(Of CustomerMaster) = ( _
            CustomerMasterDB.GetAllCustomers() _
            )

        Dim comboSource As New Dictionary(Of String, String)()

        comboSource.Add(String.Empty, "Select Customer ...")
        comboSource.Add("ALL", "All Customers")

        If Not customers Is Nothing Then
            For Each customer As CustomerMaster In customers
                comboSource.Add(customer.CUSTOMERCODE, customer.SHORTNAME)
            Next
        End If

        ComboBoxCustomer.DataSource = New BindingSource(comboSource, Nothing)
        ComboBoxCustomer.DisplayMember = "Value"
        ComboBoxCustomer.ValueMember = "Key"
    End Sub

#Region "Keypad Press"
    Private Sub ActivateCommonShortcut( _
        ByVal e As System.Windows.Forms.KeyEventArgs _
        )
        If Not processKeypadPress Then Exit Sub

        Select Case e.KeyCode
            Case Windows.Forms.Keys.F1
                QueryPendingShipment()
            Case Windows.Forms.Keys.F2
                Close()
        End Select
    End Sub

    Private Sub Shipment_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles MyBase.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonFullScreen_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ButtonFullScreen.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub DateTimePicker1_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles DateTimePicker1.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub CheckBoxPreviousDate_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles CheckBoxPreviousDate.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ComboBoxCustomer_KeyUp( _
        ByVal sender As Object, _
        ByVal e As KeyEventArgs _
        ) Handles ComboBoxCustomer.KeyUp
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
        QueryPendingShipment()
    End Sub

    Private Sub ButtonBack_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
        ) Handles ButtonBack.Click
        Close()
    End Sub
#End Region

    Private Sub QueryPendingShipment()
        'If Not WifiConectionCheck() Then Exit Sub (disable by Febry 5 December 2018)
        Dim selectedCustomerCode As String = DirectCast(ComboBoxCustomer.SelectedItem, KeyValuePair(Of String, String)).Key
        Dim checkedIncludePrevDate As Boolean = CheckBoxPreviousDate.Checked
        Dim checkedIncludeCompleteShipment As Boolean = CbxCompletedData.Checked


        If selectedCustomerCode = String.Empty Then
            DisplayMessage.ErrorMsg("Pilih Customer Name dan Cobalah Sekali Lagi!", "Error")
            Exit Sub
        End If

        If checkedIncludePrevDate = True And checkedIncludeCompleteShipment = True Then
            Dim MsgPrevCompleted As Boolean = DisplayMessage.ConfirmationDialog("Anda Yakin Memilih Previous Data dan Completed Status, load data akan sangat lambat?", "Konfirmasi")
            If MsgPrevCompleted = True Then
                nowLoadingWindow = New NowLoading
                nowLoadingWindow.Show()

                LoadShipmentListWindow(selectedCustomerCode)
            Else
                Exit Sub
            End If
        End If

        If checkedIncludePrevDate = True And checkedIncludeCompleteShipment = False Then
            Dim MsgIncludePrev As Boolean = DisplayMessage.ConfirmationDialog("Anda Yakin Memilih Previous Data,load data akan sangat lambat?", "Konfirmasi")
            If MsgIncludePrev = True Then
                nowLoadingWindow = New NowLoading
                nowLoadingWindow.Show()
                LoadShipmentListWindow(selectedCustomerCode)
            Else
                Exit Sub
            End If
        End If

        If checkedIncludePrevDate = False And checkedIncludeCompleteShipment = True Then
            Dim MsgCompleteShipment As Boolean = DisplayMessage.ConfirmationDialog("Anda Yakin Memilih Completed Status Shipment,load data akan sangat lambat?", "Konfirmasi")
            If MsgCompleteShipment = True Then
                nowLoadingWindow = New NowLoading
                nowLoadingWindow.Show()
                LoadShipmentListWindow(selectedCustomerCode)
            Else
                Exit Sub
            End If
        End If

        If checkedIncludePrevDate = False And checkedIncludeCompleteShipment = False Then
            nowLoadingWindow = New NowLoading
            nowLoadingWindow.Show()
            LoadShipmentListWindow(selectedCustomerCode)
        Else
            Exit Sub
        End If
        Exit Sub
    End Sub

    Private Sub LoadShipmentListWindow(ByVal selectedCustomerCode As String)
        EnableControl(False)

        Dim shipmentListWindow As New ShipmentList(Me)

        shipmentListWindow.SelectedShipmentDate = DateTimePicker1.Value
        shipmentListWindow.IncludePrevDateShipment = ( _
            CheckBoxPreviousDate.Checked _
            )
        shipmentListWindow.IncludeCompleteShipment = ( _
            CbxCompletedData.Checked _
            )
        shipmentListWindow.SelectedCustomerCode = selectedCustomerCode
        shipmentListWindow.ShowDialog()
        shipmentListWindow = Nothing

        EnableControl(True)
    End Sub

    Private Sub EnableControl(ByVal enable As Boolean)
        ButtonNext.Enabled = enable
        ButtonBack.Enabled = enable
        processKeypadPress = enable
    End Sub

    Private Function WifiConectionCheck() As Boolean
        'modify 0.9l
        'log4net.Config.XmlConfigurator.Configure()
        'Dim log As ILog = LogManager.GetLogger("TRADLogger")
        Try
            If RF.SYNCHRONIZE(RF.SYNC_CHECK) <> 0 Then
                'log.Info("Start Info WifiConectionCheck Signal Distance Shipment Filter")
                'log.Info("Additional Message: Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
                '    "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.")
                'log.Info("End Info WifiConectionCheck Signal Distance Shipment Filter")

                DisplayMessage.ErrorMsg("Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
                    "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.", "Error")
                Return False
                Exit Function
            End If
        Catch ex As Exception
            'log.Error("Start Error WifiConectionCheck Shipment Filter")
            If Err.Number = 5 Then
                'log.Error("Additional Message: Koneksi Wifi di HT tertutup." & vbCrLf & _
                '    "Tunggu beberapa detik dan ulangi lagi.")
                'log.Error("Error Number: " & Err.Number & vbCrLf & "Error Description: " & Err.Description & vbCrLf, ex)
                'log.Error("End Error WifiConectionCheck Shipment Filter")

                DisplayMessage.ErrorMsg("Koneksi Wifi di HT tertutup." & vbCrLf & _
                    "Tunggu beberapa detik dan ulangi lagi.", "Error")
                Dim MyRf As RF
                MyRf = New RF()
                MyRf.OpenMode = RF.EN_OPEN_MODE.CONTINUOUSLY
                MyRf.Open = True
                Return False
                Exit Function
            End If
            'log.Error("Error Number: " & Err.Number & vbCrLf & "Error Description: " & Err.Description & vbCrLf, ex)
            'log.Error("End Error WifiConectionCheck Shipment Filter")
        End Try
        'LogManager.Shutdown()
        Return True
    End Function
End Class
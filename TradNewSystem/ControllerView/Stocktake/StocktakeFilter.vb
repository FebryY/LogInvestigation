Option Strict On
Option Explicit On

Imports TradNewSystem.Helpers
Imports TradNewSystem.Model
Imports TradNewSystem.PocoClass
Imports DNWA.BHTCL

Public Class StocktakeFilter
    Protected Friend nowLoadingWindow As NowLoading

    Private _mainMenuWindow As MainMenu

    Private processKeypadPress As Boolean = True
    Private hasLoaded As Boolean = False

    Public Sub New(ByVal _mainMenuWindow As MainMenu)
        InitializeComponent()

        Me._mainMenuWindow = _mainMenuWindow
    End Sub

    Private Sub StocktakeFilter_Activated( _
        ByVal sender As Object, ByVal e As EventArgs _
        ) Handles MyBase.Activated
        If Not hasLoaded Then
            _mainMenuWindow.nowLoadingWindow.Close()
            _mainMenuWindow.nowLoadingWindow = Nothing

            hasLoaded = True
        End If
    End Sub

    Private Sub StocktakeFilter_Load( _
        ByVal sender As Object, ByVal e As EventArgs _
        ) Handles MyBase.Load
        DateTimePicker1.Format = DateTimePickerFormat.Custom
        DateTimePicker1.CustomFormat = "dd/MM/yyyy"

        DateTimePicker1.Value = Today
        If RF.SYNCHRONIZE(RF.SYNC_CHECK) <> 0 Then
            DisplayMessage.ErrorMsg( _
            "Posisi anda tidak terjangkau sinyal Wi-fi." & vbCrLf & _
            "Tolong Pindah ke tempat yg terjangkau sinyal Wi-fi dan coba lagi.", _
            "Error" _
            )
            Exit Sub
        End If
        LoadDivisionDataToComboBox()
    End Sub

    Private Sub LoadDivisionDataToComboBox()
        Dim deptMasters As List(Of DeptMaster) = DeptMasterDB.GetAllDivisions

        Dim comboSource As New Dictionary(Of String, String)()

        comboSource.Add(String.Empty, "Select Division ...")
        comboSource.Add("ALL", "All Divisions")

        If Not deptMasters Is Nothing Then
            For Each deptMaster As DeptMaster In deptMasters
                comboSource.Add( _
                    deptMaster.DIVISIONCODE, deptMaster.DIVISIONNAME _
                    )
            Next
        End If

        ComboBoxDivision.DataSource = New BindingSource(comboSource, Nothing)
        ComboBoxDivision.DisplayMember = "Value"
        ComboBoxDivision.ValueMember = "Key"
    End Sub

#Region "Key Press"
    Private Sub ActivateCommonShortcut(ByVal e As KeyEventArgs)
        If processKeypadPress Then
            Select Case e.KeyCode
                Case Windows.Forms.Keys.F1
                    QueryStocktake()
                Case Windows.Forms.Keys.F2
                    Close()
            End Select
        End If
    End Sub

    Private Sub StocktakeFilter_KeyUp( _
        ByVal sender As Object, ByVal e As KeyEventArgs _
        ) Handles MyBase.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonNext_KeyUp( _
        ByVal sender As Object, ByVal e As KeyEventArgs _
        ) Handles ButtonNext.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonBack_KeyUp( _
        ByVal sender As Object, ByVal e As KeyEventArgs _
        ) Handles ButtonBack.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ButtonFullScreen_KeyUp( _
        ByVal sender As Object, ByVal e As KeyEventArgs _
        ) Handles ButtonFullScreen.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub DateTimePicker1_KeyUp( _
        ByVal sender As Object, ByVal e As KeyEventArgs _
        ) Handles DateTimePicker1.KeyUp
        ActivateCommonShortcut(e)
    End Sub

    Private Sub ComboBoxDivision_KeyUp( _
        ByVal sender As Object, ByVal e As KeyEventArgs _
        ) Handles ComboBoxDivision.KeyUp
        ActivateCommonShortcut(e)
    End Sub
#End Region

#Region "Button Click"
    Private Sub ButtonFullScreen_Click( _
        ByVal sender As Object, ByVal e As EventArgs _
        ) Handles ButtonFullScreen.Click
        ToggleTaskbar.EnableDisable()
    End Sub

    Private Sub ButtonNext_Click( _
        ByVal sender As Object, ByVal e As EventArgs _
        ) Handles ButtonNext.Click
        QueryStocktake()
    End Sub

    Private Sub ButtonBack_Click( _
        ByVal sender As Object, ByVal e As EventArgs _
        ) Handles ButtonBack.Click
        Close()
    End Sub
#End Region

    Private Sub QueryStocktake()
        Dim selectedDivision As String = DirectCast( _
            ComboBoxDivision.SelectedItem, KeyValuePair(Of String, String) _
            ).Key

        If selectedDivision = String.Empty Then
            DisplayMessage.ErrorMsg( _
                "Pilih Division dan Cobalah Sekali Lagi!", "Error" _
                )
        Else
            nowLoadingWindow = New NowLoading
            nowLoadingWindow.Show()

            LoadStocktakeListWindow(selectedDivision)
        End If
    End Sub

    Private Sub LoadStocktakeListWindow(ByVal selectedDivision As String)
        EnableControl(False)

        Dim stocktakeListWindow As New StocktakeList(Me)

        stocktakeListWindow.SelectedStocktakePeriod = DateTimePicker1.Value
        stocktakeListWindow.SelectedDivision = selectedDivision
        stocktakeListWindow.ShowDialog()
        stocktakeListWindow = Nothing

        EnableControl(True)
    End Sub

    Private Sub EnableControl(ByVal enable As Boolean)
        ButtonNext.Enabled = enable
        ButtonBack.Enabled = enable
        processKeypadPress = enable
    End Sub
End Class
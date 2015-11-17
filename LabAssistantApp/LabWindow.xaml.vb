Imports System.Windows.Media.Animation

Public Class LabWindow
    Inherits LabAssistantWindow

    Public Sub Initialize()
        SetElements(tableViewbox)
        handleMenuClick(tableMenuBtn, Nothing)
        timer1.Interval = TimeSpan.FromSeconds(0.7)
        timer2.Interval = TimeSpan.FromSeconds(0.7)
        timer3.Interval = TimeSpan.FromSeconds(0.7)
        timer4.Interval = TimeSpan.FromSeconds(0.7)
        versionBlock.Text = My.Application.Info.Version.ToString(3)
        Dim b As New Binding("AutoStartup")
        b.Source = My.Settings
        b.Mode = BindingMode.TwoWay
        startSelection.SetBinding(LabToggleButton.EnabledProperty, b)
        Dim b2 As New Binding("AutoStartupFile")
        b2.Source = My.Settings
        b2.Mode = BindingMode.TwoWay
        selectStartupLabel.SetBinding(Label.ContentProperty, b2)
        Dim b3 As New Binding("AskForConfirmationOnDelete")
        b3.Source = My.Settings
        b3.Mode = BindingMode.TwoWay

    End Sub

    Private Sub SetElements(inElement As DependencyObject)
        For Each o As Object In LogicalTreeHelper.GetChildren(inElement)

            If o.GetType().IsEquivalentTo(GetType(TableButton)) Then
                Dim tb As TableButton = o
                If tb.Tag IsNot Nothing Then
                    Dim use As Integer
                    If Integer.TryParse(tb.Tag, use) Then
                        tb.Element = Matter.Element.FromAtomicNumber(use)
                    Else
                        tb.PaintGroup([Enum].Parse(GetType(Matter.Element.Groups), tb.Tag))
                    End If

                End If

            Else
                If o.GetType().IsSubclassOf(GetType(DependencyObject)) Then
                    SetElements(o)
                End If
            End If
        Next
    End Sub

    Public Sub handleLaboratoryLoaded(sender As Laboratory, e As EventArgs)
        handleLabLoaded(sender, e)
        handleLabChanged(sender, e)
    End Sub

    Public Sub handleLaboratoryUnloaded(sender As Object, e As EventArgs)
        saveBtn.IsEnabled = False
        inventoryBtn.IsEnabled = False
    End Sub


    Private Sub handleLabLoaded(sender As Laboratory, e As EventArgs)
        saveBtn.IsEnabled = False
        inventoryBtn.IsEnabled = True
        AddHandler sender.LabChanged, labChangedDelegate
    End Sub

    Private labChangedDelegate As EventHandler(Of EventArgs) = AddressOf handleLabChanged
    Private Sub handleLabChanged(sender As Laboratory, e As EventArgs)
        Matter.Reaction.UpdateRecreatable()
        saveBtn.IsEnabled = True
        inventoryGrid.GetBindingExpression(DataGrid.ItemsSourceProperty).UpdateTarget()
        inventoryGrid.Items.Refresh()
    End Sub

#Region "Menu Selection"

    Private hidden As Boolean = False
    Private fullWidth As Double
    Private animating As Boolean = False
    Private WithEvents da As New DoubleAnimation(0, GetDuration(0.5))
    Private Sub handleMenuClick(sender As Object, e As RoutedEventArgs)
        Select Case sender.Tag
            Case Is = "start"
                DeselectMenuItems(menuStackPanel, sender)
                tabDisplay.SelectedItem = startupSettingsPage
            Case Is = "table"
                DeselectMenuItems(menuStackPanel, sender)
                tabDisplay.SelectedItem = tableTabPage
            Case Is = "inorganic"
                DeselectMenuItems(menuStackPanel, sender)
                tabDisplay.SelectedItem = inorganicsPage
            Case Is = "organic"
                DeselectMenuItems(menuStackPanel, sender)
                tabDisplay.SelectedItem = organicsPage
            Case Is = "hide"
                If Not animating Then
                    If hidden Then
                        da.From = leftPanel.ActualWidth
                        da.To = fullWidth
                        leftPanel.BeginAnimation(Grid.WidthProperty, da)
                        animating = True
                        AnimateAllLabels(leftPanel, False)
                    Else
                        fullWidth = leftPanel.ActualWidth
                        da.From = leftPanel.ActualWidth
                        da.To = Me.FindResource("itemHeight") + 3.5
                        leftPanel.BeginAnimation(Grid.WidthProperty, da)
                        animating = True
                        AnimateAllLabels(leftPanel, True)
                    End If
                    hidden = Not hidden
                End If
            Case Is = "about"
                DeselectMenuItems(menuStackPanel, sender)
                tabDisplay.SelectedItem = aboutPage
            Case Is = "load"
                Dim o As New Microsoft.Win32.OpenFileDialog()
                o.Filter = OpenLabFilter
                o.Multiselect = False
                o.Title = "Load a Laboratory"
                o.CheckFileExists = True
                o.CheckPathExists = True

                If o.ShowDialog(Me) Then
                    Dim l As Laboratory = Laboratory.LoadFrom(o.FileName)
                    If Not IsNothing(l) Then
                        Matter.Info.LoadLab(l)
                    End If
                End If
            Case Is = "save"
                Dim s As New Microsoft.Win32.SaveFileDialog()
                s.Filter = Constants.OpenLabFilter
                s.AddExtension = True
                s.CheckPathExists = True
                s.DefaultExt = "*.clab"
                If s.ShowDialog(Me) Then
                    Try
                        RemoveHandler Matter.Info.LoadedLab.LabChanged, labChangedDelegate
                        If Laboratory.SaveTo(s.FileName, Matter.Info.LoadedLab) Then
                            MsgBox("Laboratory saved!")
                            saveBtn.IsEnabled = False
                        Else
                            Throw New Exception()
                        End If
                    Catch ex As Exception
                        MsgBox("An error occured!")
                    Finally
                        AddHandler Matter.Info.LoadedLab.LabChanged, labChangedDelegate
                    End Try
                Else
                    MsgBox("Not saved!")
                End If
            Case Is = "new"
                If saveBtn.IsEnabled Then
                    Dim dr As MsgBoxResult = MsgBox("You have modified the current laboratory." & vbNewLine & "Do you want to save first?", MsgBoxStyle.YesNoCancel)
                    Select Case dr
                        Case MsgBoxResult.Yes
                            saveBtn.PerformClick()
                        Case MsgBoxResult.No
                            createNewLab()
                    End Select
                Else
                    createNewLab()
                End If
            Case Is = "reaction"
                DeselectMenuItems(menuStackPanel, sender)
                tabDisplay.SelectedItem = reactionsPage
            Case Is = "inventory"
                DeselectMenuItems(menuStackPanel, sender)
                tabDisplay.SelectedItem = inventoryPage
        End Select
    End Sub

    Private Sub createNewLab()
        Dim l As New Laboratory()
        Matter.Info.LoadLab(l)
    End Sub

    Private Sub aniCompleted(sender As Object, e As EventArgs) Handles da.Completed
        If hidden Then
            panelHider.SetValue(ImageButton.ImageBrushProperty, Me.FindResource("rightArrow"))
        Else
            leftPanel.InvalidateProperty(Grid.ActualWidthProperty)
            panelHider.SetValue(ImageButton.ImageBrushProperty, Me.FindResource("leftArrow"))
        End If
        animating = False
    End Sub

    Private Sub AnimateAllLabels(inElement As DependencyObject, ByVal hide As Boolean)
        For Each o As Object In LogicalTreeHelper.GetChildren(inElement)

            If o.GetType().IsEquivalentTo(GetType(Label)) Then
                Dim lb As Label = o

                Dim da As DoubleAnimation
                If hide Then
                    da = New DoubleAnimation(Me.FindResource("labelHeight"), 0, GetDuration(0.5))
                Else
                    da = New DoubleAnimation(Me.FindResource("labelHeight"), GetDuration(0.5))
                End If
                lb.BeginAnimation(Label.HeightProperty, da)
            Else
                If o.GetType().IsSubclassOf(GetType(DependencyObject)) AndAlso Not o.GetType().IsEquivalentTo(GetType(ImageButton)) Then
                    AnimateAllLabels(o, hide)
                End If
            End If
        Next
    End Sub

    Private Sub DeselectMenuItems(inElement As DependencyObject, ByVal exception As ImageButton)
        RecursiveDeselect(inElement, exception.Tag)
        exception.IsSelected = True
    End Sub

    Private Sub RecursiveDeselect(inElement As DependencyObject, ByVal exceptTag As String)
        For Each o As Object In LogicalTreeHelper.GetChildren(inElement)

            If o.GetType().IsEquivalentTo(GetType(ImageButton)) Then
                Dim tb As ImageButton = o
                If Not tb.Tag.Equals(exceptTag) Then tb.IsSelected = False
                If o.GetType().IsSubclassOf(GetType(DependencyObject)) Then
                    RecursiveDeselect(o, exceptTag)
                End If
            End If
        Next
    End Sub

#End Region

#Region "Searching"

    Private WithEvents timer1 As New Threading.DispatcherTimer
    Private Sub handleTick(sender As Object, e As EventArgs) Handles timer1.Tick
        searchBox.Text = searchBox.Template.FindName("tbCore", searchBox).Text
        timer1.Stop()
    End Sub

    Private lastSearch As String = String.Empty
    Private lastSearch2 As String = String.Empty
    Private lastSearch3 As String = String.Empty
    Private lastSearch4 As String = String.Empty
    Private Sub handleSearchChanged(sender As Object, e As TextChangedEventArgs)
        Dim tbCore As TextBox = sender
        Select Case tbCore.TemplatedParent.GetValue(TextBox.NameProperty)
            Case "searchBox"
                If Not lastSearch.Equals(sender.Text) Then
                    timer1.Stop()
                    timer1.Start()
                    lastSearch = sender.Text
                End If
            Case "organicSearchBox"
                If Not lastSearch2.Equals(sender.Text) Then
                    timer2.Stop()
                    timer2.Start()
                    lastSearch2 = sender.Text
                End If
            Case "inventorySearchBox"
                If Not lastSearch3.Equals(sender.Text) Then
                    timer3.Stop()
                    timer3.Start()
                    lastSearch3 = sender.Text
                End If
            Case "reactionsSearchBox"
                If Not lastSearch4.Equals(sender.Text) Then
                    timer4.Stop()
                    timer4.Start()
                    lastSearch4 = sender.Text
                End If
        End Select
    End Sub

    Private WithEvents timer2 As New Threading.DispatcherTimer
    Private Sub handleTick2(sender As Object, e As EventArgs) Handles timer2.Tick
        organicSearchBox.Text = organicSearchBox.Template.FindName("tbCore", organicSearchBox).Text
        timer2.Stop()
    End Sub

    Private WithEvents timer3 As New Threading.DispatcherTimer
    Private Sub handleTick3(sender As Object, e As EventArgs) Handles timer3.Tick
        inventorySearchBox.Text = inventorySearchBox.Template.FindName("tbCore", inventorySearchBox).Text
        timer3.Stop()
    End Sub

    Private WithEvents timer4 As New Threading.DispatcherTimer
    Private Sub handleTick4(sender As Object, e As EventArgs) Handles timer4.Tick
        reactionsSearchBox.Text = reactionsSearchBox.Template.FindName("tbCore", reactionsSearchBox).Text
        If reactionsSearchBox.Text.Length > 0 Then
            Dim rt As ReactionSearchConverter.SearchType = SearchType.All
            Select Case reactionTypeComboBox.SelectedIndex
                Case 1
                    rt = SearchType.Decomposition
                Case 2
                    rt = SearchType.Electrolysis
                Case 3
                    rt = SearchType.Synthesis
                Case 4
                    rt = SearchType.Other
            End Select
            Dim l As List(Of Matter.Reaction) = FindReactions(reactionsSearchBox.Text, rt)
            If l.Count > 0 Then
                If l.Count Mod 100 = 1 Then
                    reactionsStatusText.Content = String.Format("Found {0} reaction.", l.Count)
                Else
                    reactionsStatusText.Content = String.Format("Found {0} reactions.", l.Count)
                End If
            Else
                reactionsStatusText.Content = "No reactions found."
            End If
            reactionList.LoadReactions(l)
        Else
            reactionsStatusText.Content = "Search for a reaction."
            reactionList.LoadReactions(New List(Of Matter.Reaction))
        End If
        timer4.Stop()
    End Sub

#End Region

    Private Sub handleElementClick(sender As Object, e As RoutedEventArgs)
        Dim tb As TableButton = sender
        If Not IsNothing(tb.Element) Then
            Application.CreateElementInfoForm(tb.Element)
        End If
    End Sub

    Private Sub handleClosing(sender As Object, e As ComponentModel.CancelEventArgs)
        My.Settings.Save()
    End Sub

    Private Sub changeAutoLoadPath(sender As Object, e As MouseButtonEventArgs) Handles selectStartupLabel.MouseLeftButtonDown
        Dim o As New Microsoft.Win32.OpenFileDialog()
        o.Filter = OpenLabFilter
        o.Multiselect = False
        o.Title = "Select autoload laboratory"
        o.CheckFileExists = True
        o.CheckPathExists = True

        If o.ShowDialog(Me) Then
            My.Settings.AutoStartupFile = o.FileName
        End If
    End Sub

End Class

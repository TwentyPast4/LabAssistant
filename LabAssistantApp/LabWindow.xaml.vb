Imports System.Windows.Media.Animation

Public Class LabWindow
    Inherits LabAssistantWindow

    Public Sub Initialize()
        SetElements(tableViewbox)
        handleMenuClick(tableMenuBtn, Nothing)
        timer1 = New Threading.DispatcherTimer()
        timer1.Interval = TimeSpan.FromSeconds(0.7)
        AddHandler timer1.Tick, AddressOf handleTick
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
        AddHandler sender.LabChanged, AddressOf Me.handleLabChange
    End Sub

    Private Sub handleLabChange(sender As Laboratory, e As EventArgs)

    End Sub

#Region "Menu Selection"

    Private hidden As Boolean = False
    Private fullWidth As Double
    Private animating As Boolean = False
    Private WithEvents da As New DoubleAnimation(0, GetDuration(0.5))
    Private Sub handleMenuClick(sender As Object, e As RoutedEventArgs)
        Select Case sender.Tag
            Case Is = "start"
                For Each el In Matter.Element.ElementList
                    el.SetLabState(CType(Math.Floor(Rnd() * 4), Matter.StateInLab))
                Next
            Case Is = "table"
                DeselectMenuItems(menuStackPanel, sender.Tag)
                sender.IsSelected = True
                tabDisplay.SelectedIndex = 0
            Case Is = "inorganic"
                DeselectMenuItems(menuStackPanel, sender.Tag)
                sender.IsSelected = True
                tabDisplay.SelectedIndex = 1
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
        End Select
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

    Private Sub DeselectMenuItems(inElement As DependencyObject, ByVal exceptionTag As String)
        For Each o As Object In LogicalTreeHelper.GetChildren(inElement)

            If o.GetType().IsEquivalentTo(GetType(ImageButton)) Then
                Dim tb As ImageButton = o
                If Not tb.Tag.Equals(exceptionTag) Then tb.IsSelected = False
                If o.GetType().IsSubclassOf(GetType(DependencyObject)) Then
                    DeselectMenuItems(o, exceptionTag)
                End If
            End If
        Next
    End Sub



#End Region

#Region "Searching"
    Private timer1 As Threading.DispatcherTimer
    Private Sub handleTick(sender As Object, e As EventArgs)
        searchBox.Text = searchBox.Template.FindName("tbCore", searchBox).Text
    End Sub

    Private lastSearch As String = String.Empty
    Private Sub handleSearchChanged(sender As Object, e As TextChangedEventArgs)
        If Not lastSearch.Equals(sender.Text) Then
            timer1.Stop()
            timer1.Start()
            lastSearch = sender.Text
        End If
    End Sub
#End Region

    Private Sub handleElementClick(sender As Object, e As RoutedEventArgs)
        Dim tb As TableButton = sender
        Dim infoDialog As New ElementInfoWindow(tb.Element)
        infoDialog.Show()
    End Sub

End Class

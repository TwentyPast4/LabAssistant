Imports System.Windows.Media.Animation
Imports System.Runtime.InteropServices
Imports System.Windows.Interop

Class MainWindow

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        SetElements(tableViewbox)
        handleMenuClick(tableMenuBtn, Nothing)
        inorganicGrid.ItemsSource = Matter.Compound.GetAllOfType(False)

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

    Private Sub topBorderMouseDown(sender As Object, e As MouseButtonEventArgs) Handles topBorder.MouseLeftButtonDown
        Me.DragMove()
    End Sub

    Private Sub handleToolbarEnter(sender As Button, e As MouseEventArgs)
        Dim da As New DoubleAnimation(1, GetDuration(0.2))
        sender.BeginAnimation(Button.OpacityProperty, da)
    End Sub

    Public Shared Function GetDuration(ByVal seconds As Double) As Duration
        Return New Duration(TimeSpan.FromSeconds(seconds))
    End Function

    Private Sub handleToolbarLeave(sender As Button, e As MouseEventArgs)
        Dim da As New DoubleAnimation(0.4, GetDuration(0.7))
        sender.BeginAnimation(Button.OpacityProperty, da)
    End Sub

    Private Sub handleToolbarClick(sender As Button, e As EventArgs)
        Select Case sender.Name
            Case Is = "exitBtn"
                Me.Close()
            Case Is = "maxBtn"
                If Me.WindowState = Windows.WindowState.Maximized Then
                    DeMaximize()
                Else
                    Maximize()
                End If
            Case Is = "minBtn"
                Me.WindowState = Windows.WindowState.Minimized
        End Select
    End Sub

    Private Sub handleToolbarDown(sender As Object, e As MouseButtonEventArgs)
        Dim g As Grid = Template.FindName("buttonBase", sender)
        g.Background = New SolidColorBrush(Color.FromArgb(100, 0, 0, 0))
    End Sub

    Private Sub handleToolbarUp(sender As Button, e As MouseButtonEventArgs)
        Dim g As Grid = Template.FindName("buttonBase", sender)
        g.Background = Brushes.Transparent
    End Sub

    Private Sub handleBorderMouseDown(sender As Border, e As MouseButtonEventArgs)
        If e.LeftButton Then
            Select Case sender.Name
                Case Is = midLeftBorder.Name
                    ResizeWindow(SysCommandSize.SC_SIZE_HTLEFT)
                Case Is = lowLeftBorder.Name
                    ResizeWindow(SysCommandSize.SC_SIZE_HTBOTTOMLEFT)
                Case Is = midBottomBorder.Name
                    ResizeWindow(SysCommandSize.SC_SIZE_HTBOTTOM)
                Case Is = lowRightBorder.Name
                    ResizeWindow(SysCommandSize.SC_SIZE_HTBOTTOMRIGHT)
                Case Is = midRightBorder.Name
                    ResizeWindow(SysCommandSize.SC_SIZE_HTRIGHT)
            End Select
        End If
    End Sub

    Public Sub Maximize()
        Dim t As New Thickness(0)
        contentBorder.BorderThickness = t
        lowLeftBorder.BorderThickness = t
        midLeftBorder.BorderThickness = t
        midBottomBorder.BorderThickness = t
        lowRightBorder.BorderThickness = t
        midRightBorder.BorderThickness = t
        Me.WindowState = Windows.WindowState.Maximized
    End Sub

    Public Sub DeMaximize()
        Dim bt As Integer = 3
        contentBorder.BorderThickness = New Thickness(bt, 0, bt, bt)
        lowLeftBorder.BorderThickness = New Thickness(bt, 0, 0, bt)
        midLeftBorder.BorderThickness = New Thickness(bt, 0, 0, 0)
        midBottomBorder.BorderThickness = New Thickness(0, 0, 0, bt)
        lowRightBorder.BorderThickness = New Thickness(0, 0, bt, bt)
        midRightBorder.BorderThickness = New Thickness(0, 0, bt, 0)
        Me.WindowState = Windows.WindowState.Normal
    End Sub

#Region "Resizing"

    <DllImport("user32.dll")> _
    Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    End Function

    Private Const WM_SYSCOMMAND As Integer = &H112
    Private Const SC_SIZE As UInteger = &HF000
    Public Enum SysCommandSize As UInteger
        SC_SIZE_HTLEFT = 1
        SC_SIZE_HTRIGHT = 2
        SC_SIZE_HTTOP = 3
        SC_SIZE_HTTOPLEFT = 4
        SC_SIZE_HTTOPRIGHT = 5
        SC_SIZE_HTBOTTOM = 6
        SC_SIZE_HTBOTTOMLEFT = 7
        SC_SIZE_HTBOTTOMRIGHT = 8
    End Enum

    Private Sub ResizeWindow(ByVal direction As SysCommandSize)
        SendMessage(_hwndSource.Handle, WM_SYSCOMMAND, SC_SIZE + direction, IntPtr.Zero)
    End Sub

    Private _hwndSource As HwndSource

    Private Sub srcHandle(sender As Object, e As EventArgs) Handles Me.SourceInitialized
        _hwndSource = CType(PresentationSource.FromVisual(Me), HwndSource)
    End Sub

#End Region

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

End Class

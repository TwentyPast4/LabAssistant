Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Windows.Interop
Imports System.Windows.Media.Animation

Public Class LabAssistantWindow
    Inherits Window
    Implements INotifyPropertyChanged

    Public Shared ReadOnly CanResizeProperty As DependencyProperty =
        DependencyProperty.Register("CanResize", GetType(Boolean), GetType(LabAssistantWindow), New UIPropertyMetadata(True))
    Public Shared ReadOnly MaximizeShownProperty As DependencyProperty =
        DependencyProperty.Register("MaximizeShown", GetType(Boolean), GetType(LabAssistantWindow), New UIPropertyMetadata(True))
    Public Shared ReadOnly MinimizeShownProperty As DependencyProperty =
        DependencyProperty.Register("MinimizeShown", GetType(Boolean), GetType(LabAssistantWindow), New UIPropertyMetadata(True))

    Private contentBorder As Border
    Private midLeftBorder As Border
    Private lowLeftBorder As Border
    Private midBottomBorder As Border
    Private lowRightBorder As Border
    Private midRightBorder As Border

    Private exitBtn As Button
    Private maxBtn As Button
    Private minBtn As Button

    Private topBorder As Grid

    Public Property CanResize As Boolean
        Get
            Return canResize_
        End Get
        Set(value As Boolean)
            canResize_ = value
        End Set
    End Property
    Private canResize_ As Boolean = True

    Public Property MinimizeShown As Boolean
        Get
            Return minShown_
        End Get
        Set(value As Boolean)
            minShown_ = value
        End Set
    End Property
    Private minShown_ As Boolean = True

    Public Property MaximizeShown As Boolean
        Get
            Return maxShown_
        End Get
        Set(value As Boolean)
            maxShown_ = value
        End Set
    End Property
    Private maxShown_ As Boolean = True

    Public Sub New()
        DefaultStyleKey = GetType(Window)
    End Sub

    Public Overrides Sub OnApplyTemplate()
        MyBase.OnApplyTemplate()
        SetStuff()
    End Sub

    Private Sub SetStuff()
        contentBorder = MyBase.GetTemplateChild("contentBorder")
        midLeftBorder = GetTemplateChild("midLeftBorder")
        lowLeftBorder = GetTemplateChild("lowLeftBorder")
        midBottomBorder = GetTemplateChild("midBottomBorder")
        lowRightBorder = GetTemplateChild("lowRightBorder")
        midRightBorder = GetTemplateChild("midRightBorder")

        AddHandler midLeftBorder.MouseLeftButtonDown, AddressOf handleBorderMouseDown
        AddHandler lowLeftBorder.MouseLeftButtonDown, AddressOf handleBorderMouseDown
        AddHandler midBottomBorder.MouseLeftButtonDown, AddressOf handleBorderMouseDown
        AddHandler lowRightBorder.MouseLeftButtonDown, AddressOf handleBorderMouseDown
        AddHandler midRightBorder.MouseLeftButtonDown, AddressOf handleBorderMouseDown

        exitBtn = GetTemplateChild("exitBtn")
        maxBtn = GetTemplateChild("maxBtn")
        minBtn = GetTemplateChild("minBtn")

        AddHandlers(exitBtn)
        AddHandlers(maxBtn)
        AddHandlers(minBtn)

        topBorder = GetTemplateChild("topBorder")

        AddHandler topBorder.MouseLeftButtonDown, AddressOf topBorderMouseDown
        AddHandler topBorder.MouseLeftButtonUp, AddressOf topBorderMouseUp

        Dim btvc As New BooleanToVisibilityConverter()
        Dim b As New Binding()
        b.Path = New PropertyPath(LabAssistantWindow.CanResizeProperty)
        b.Source = Me
        b.Converter = btvc
        bindTo(midLeftBorder, Border.VisibilityProperty, b)
        bindTo(lowLeftBorder, Border.VisibilityProperty, b)
        bindTo(midBottomBorder, Border.VisibilityProperty, b)
        bindTo(lowRightBorder, Border.VisibilityProperty, b)
        bindTo(midRightBorder, Border.VisibilityProperty, b)

        Dim b2 As New Binding()
        b2.Path = New PropertyPath(LabAssistantWindow.MaximizeShownProperty)
        b2.Source = Me
        b2.Converter = btvc
        bindTo(maxBtn, Button.VisibilityProperty, b2)

        Dim b3 As New Binding()
        b3.Path = New PropertyPath(LabAssistantWindow.MinimizeShownProperty)
        b3.Source = Me
        b3.Converter = btvc
        bindTo(minBtn, Button.VisibilityProperty, b3)
    End Sub

    Private Shared Sub bindTo(ByVal o As FrameworkElement, dp As DependencyProperty, b As Binding)
        o.SetBinding(dp, b)
    End Sub

    Private Sub AddHandlers(ByVal b As Button)
        AddHandler b.Click, AddressOf handleToolbarClick
        AddHandler b.MouseEnter, AddressOf handleToolbarEnter
        AddHandler b.MouseLeave, AddressOf handleToolbarLeave
        AddHandler b.MouseLeftButtonUp, AddressOf handleToolbarUp
        AddHandler b.MouseLeftButtonDown, AddressOf handleToolbarDown
    End Sub

    Private Sub topBorderMouseDown(sender As Object, e As MouseButtonEventArgs)
        Me.DragMove()
    End Sub

    Private Sub topBorderMouseUp(sender As Object, e As MouseButtonEventArgs)
        If e.ClickCount = 2 Then
            If Me.WindowState = WindowState.Maximized Then
                Me.WindowState = WindowState.Normal
            Else
                Me.WindowState = WindowState.Maximized
            End If
        End If
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
                Case Is = "midLeftBorder"
                    ResizeWindow(SysCommandSize.SC_SIZE_HTLEFT)
                Case Is = "lowLeftBorder"
                    ResizeWindow(SysCommandSize.SC_SIZE_HTBOTTOMLEFT)
                Case Is = "midBottomBorder"
                    ResizeWindow(SysCommandSize.SC_SIZE_HTBOTTOM)
                Case Is = "lowRightBorder"
                    ResizeWindow(SysCommandSize.SC_SIZE_HTBOTTOMRIGHT)
                Case Is = "midRightBorder"
                    ResizeWindow(SysCommandSize.SC_SIZE_HTRIGHT)
            End Select
        End If
    End Sub

    Public Sub Maximize()
        Dim t As New Thickness(0)
        contentBorder.BorderThickness = t
        midLeftBorder.BorderThickness = t
        lowLeftBorder.BorderThickness = t
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

    <DllImport("user32.dll")>
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
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private Sub srcHandle(sender As Object, e As EventArgs) Handles Me.SourceInitialized
        _hwndSource = CType(PresentationSource.FromVisual(Me), HwndSource)
    End Sub

#End Region

    Protected Overloads Sub OnPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Protected Function SetField(Of T)(ByRef field As T, value As T, propertyName As String) As Boolean
        If EqualityComparer(Of T).[Default].Equals(field, value) Then
            Return False
        End If
        field = value
        OnPropertyChanged(propertyName)
        Return True
    End Function

End Class

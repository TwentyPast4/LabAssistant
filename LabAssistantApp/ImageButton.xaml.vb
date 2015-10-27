Imports System.ComponentModel
Imports System.Windows.Media.Animation


Public Class ImageButton
    Inherits ClickableControl

    Public Shared ReadOnly TextPaddingProperty As DependencyProperty =
        DependencyProperty.Register("TextPadding", GetType(Thickness), GetType(ImageButton), New UIPropertyMetadata(New Thickness(0)))
    Public Shared ReadOnly TextProperty As DependencyProperty =
        DependencyProperty.Register("Text", GetType(String), GetType(ImageButton), New UIPropertyMetadata("ImageButton"))
    Public Shared ReadOnly ImageBrushProperty As DependencyProperty =
        DependencyProperty.Register("ImageBrush", GetType(Brush), GetType(ImageButton), New UIPropertyMetadata(Nothing))
    Public Shared ReadOnly ImageTransformProperty As DependencyProperty =
        DependencyProperty.Register("ImageTransform", GetType(Transform), GetType(ImageButton), New UIPropertyMetadata(Nothing))
    Public Shared ReadOnly HorizontalTextAlignmentProperty As DependencyProperty =
        DependencyProperty.Register("HorizontalTextAlignment", GetType(HorizontalAlignment), GetType(ImageButton), New UIPropertyMetadata(HorizontalAlignment.Left))
    Public Shared ReadOnly VerticalTextAlignmentProperty As DependencyProperty =
        DependencyProperty.Register("VerticalTextAlignment", GetType(VerticalAlignment), GetType(ImageButton), New UIPropertyMetadata(VerticalAlignment.Center))
    Public Shared ReadOnly HoverColorProperty As DependencyProperty =
        DependencyProperty.Register("HoverColor", GetType(Color), GetType(ImageButton), New UIPropertyMetadata(Colors.White))
    Public Shared ReadOnly MouseDownColorProperty As DependencyProperty =
        DependencyProperty.Register("MouseDownColor", GetType(Color), GetType(ImageButton), New UIPropertyMetadata(Colors.Black))
    Public Shared ReadOnly HoverOpacityProperty As DependencyProperty =
        DependencyProperty.Register("HoverOpacity", GetType(Double), GetType(ImageButton), New UIPropertyMetadata(0.3))
    Public Shared ReadOnly IsSelectedProperty As DependencyProperty =
        DependencyProperty.Register("IsSelected", GetType(Boolean), GetType(ImageButton), New UIPropertyMetadata(False))
    Public Shared ReadOnly SelectedBrushProperty As DependencyProperty =
        DependencyProperty.Register("SelectedBrush", GetType(Brush), GetType(ImageButton), New UIPropertyMetadata(Nothing))

    <Browsable(False)>
    Public Overloads Property Content
    <Browsable(False)>
    Public Overloads Property HorizontalContentAlignment
    <Browsable(False)>
    Public Overloads Property VerticalContentAlignment

    <Description("The padding of the button's text."), Category("Layout")>
    Public Property TextPadding As Thickness
        Get
            Return txtPadding

        End Get
        Set(value As Thickness)
            txtPadding = value
        End Set
    End Property
    Private txtPadding As Thickness

    <Description("The horizontal alignment of the text"), Category("Layout")>
    Public Property HorizontalTextAlignment As HorizontalAlignment
        Get
            Return txtHLayout
        End Get
        Set(value As HorizontalAlignment)
            txtHLayout = value
        End Set
    End Property
    Private txtHLayout As HorizontalAlignment

    <Description("The vertical alignment of the text"), Category("Layout")>
    Public Property VerticalTextAlignment As VerticalAlignment
        Get
            Return txtVLayout
        End Get
        Set(value As VerticalAlignment)
            txtVLayout = value
        End Set
    End Property
    Private txtVLayout As VerticalAlignment

    <Description("The text displayed by the button."), Category("Common")>
    Public Property Text As String
        Get
            Return txt
        End Get
        Set(value As String)
            txt = value
        End Set
    End Property
    Private txt As String

    <Description("Gets or sets the selection state of this button."), Category("Common")>
    Public Property IsSelected As Boolean
        Get
            Return sel
        End Get
        Set(value As Boolean)
            sel = value
            Dim b As New Binding()
            b.Source = Me
            If sel Then
                b.Path = New PropertyPath(ImageButton.SelectedBrushProperty)
            Else
                b.Path = New PropertyPath(ImageButton.BackgroundProperty)
            End If
            controlBorder.SetBinding(Border.BackgroundProperty, b)
        End Set
    End Property
    Private sel As Boolean

    <Description("The brush to use when the button is selected."), Category("Brush")>
    Public Property SelectedBrush As Brush
        Get
            Return selBr
        End Get
        Set(value As Brush)
            selBr = value
        End Set
    End Property
    Private selBr As Brush

    <Description("The image displayed by the button."), Category("Brush")>
    Public Property ImageBrush As Brush
        Get
            Return img
        End Get
        Set(value As Brush)
            img = value
        End Set
    End Property
    Private img As Brush

    <Description("The color to use when the mouse hovers over this control."), Category("Appearance")>
    Public Property HoverColor As Color
        Get
            Return hovBr
        End Get
        Set(value As Color)
            hovBr = value
        End Set
    End Property
    Private hovBr As Color

    <Description("The color to use when the mouse is pressed."), Category("Appearance")>
    Public Property MouseDownColor As Color
        Get
            Return mouseCl
        End Get
        Set(value As Color)
            mouseCl = value
        End Set
    End Property
    Private mouseCl As Color

    <Description("Gets or sets the rendering transform information of the image."), Category("Transform")>
    Public Property ImageTransform As Transform
        Get
            Return tran
        End Get
        Set(value As Transform)
            tran = value
        End Set
    End Property
    Private tran As Transform

    <Description("Gets or sets the opacity of the glow overlay."), Category("Appearance")>
    Public Property HoverOpacity As Double
        Get
            Return hovO
        End Get
        Set(value As Double)
            hovO = value
        End Set
    End Property
    Private hovO As Double

    Private Sub meLeftDown(sender As Object, e As MouseButtonEventArgs)
        If Me.IsEnabled Then
            Dim ca As New ColorAnimation(Me.GetValue(ImageButton.MouseDownColorProperty), New Duration(TimeSpan.FromSeconds(0)))
            stackPanelBrush.BeginAnimation(SolidColorBrush.ColorProperty, ca)
        End If
    End Sub

    Private Sub meLeftUp(sender As Object, e As MouseButtonEventArgs)
        unPressed()
    End Sub

    Private Sub meMouseOver(sender As Object, e As MouseEventArgs)
        If Me.IsEnabled Then
            Dim toValue As Double = Me.GetValue(ImageButton.HoverOpacityProperty)
            Dim da As New DoubleAnimation(toValue, New Duration(TimeSpan.FromSeconds(((toValue - stackPanelBrush.Opacity) / toValue) * 0.2)))
            stackPanelBrush.BeginAnimation(SolidColorBrush.OpacityProperty, da)
        End If
    End Sub

    Private Sub meMouseLeave(sender As Object, e As MouseEventArgs)
        unPressed()
        Dim da As New DoubleAnimation(0, New Duration(TimeSpan.FromSeconds((stackPanelBrush.Opacity / Me.GetValue(ImageButton.HoverOpacityProperty)) * 0.3)))
        stackPanelBrush.BeginAnimation(SolidColorBrush.OpacityProperty, da)
    End Sub

    Private Sub unPressed()
        Dim ca As New ColorAnimation(Me.GetValue(ImageButton.HoverColorProperty), New Duration(TimeSpan.FromSeconds(0.2)))
        stackPanelBrush.BeginAnimation(SolidColorBrush.ColorProperty, ca)
    End Sub

    Private Sub meEnabledChanged(sender As Object, e As DependencyPropertyChangedEventArgs)
        If CBool(e.NewValue) Then
            Dim da As New DoubleAnimation(0, New Duration(TimeSpan.FromSeconds(1)))
            Dim da2 As New DoubleAnimation(1, New Duration(TimeSpan.FromSeconds(1)))
            controlBlur.BeginAnimation(Effects.BlurEffect.RadiusProperty, da)
            Me.BeginAnimation(ImageButton.OpacityProperty, da2)
        Else
            Dim da As New DoubleAnimation(3, New Duration(TimeSpan.FromSeconds(1)))
            Dim da2 As New DoubleAnimation(0.4, New Duration(TimeSpan.FromSeconds(1)))
            controlBlur.BeginAnimation(Effects.BlurEffect.RadiusProperty, da)
            Me.BeginAnimation(ImageButton.OpacityProperty, da2)
        End If
    End Sub

End Class

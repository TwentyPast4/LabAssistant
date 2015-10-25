Imports System.Windows.Media.Animation

''' <summary>
''' Interaction logic for TableButton.xaml
''' </summary>
Public Partial Class TableButton
	Inherits UserControl
	
    Public Shared ReadOnly BackgroundHoverProperty As DependencyProperty = _
        DependencyProperty.Register("BackgroundHover", GetType(Color), GetType(TableButton), New UIPropertyMetadata(Colors.White))
    Public Shared ReadOnly BackgroundHoverOpacityProperty As DependencyProperty = _
        DependencyProperty.Register("BackgroundHoverOpacity", GetType(Double), GetType(TableButton), New UIPropertyMetadata(0.5))
    Public Shared ReadOnly ElementProperty As DependencyProperty = _
        DependencyProperty.Register("Element", GetType(Matter.Element), GetType(TableButton), New UIPropertyMetadata(Nothing))

	Public Property BackgroundHover As Color
		Get
			Return hovBr
		End Get
		Set(value as Color)
			hovBr = value
		End Set
	End Property
    Private hovBr As Color

    Public Property BackgroundHoverOpacity As Double
        Get
            Return hovO
        End Get
        Set(value As Double)
            hovO = value
        End Set
    End Property
    Private hovO As Double

    Public Property Element As Matter.Element
        Get
            Return el
        End Get
        Set(value As Matter.Element)
            el = value
            handleElementChange()
        End Set
    End Property
    Private el As Matter.Element

	Private Sub handleMouseOver(ByVal sender as object, e as MouseEventArgs)
		If baseGrid.Background isnot nothing then
            Dim da As New DoubleAnimation(Me.GetValue(TableButton.BackgroundHoverOpacityProperty), New Duration(TimeSpan.FromSeconds(0.3)))
			baseGrid.Background.BeginAnimation(Brush.OpacityProperty, da)
		end if
		
	End Sub

	Private Sub handleMouseLeave(ByVal sender as object, e as MouseEventArgs)
		If baseGrid.Background isnot nothing then
			Dim da as New DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.5)))
			baseGrid.Background.BeginAnimation(Brush.OpacityProperty, da)
		end if
		
	End Sub

    Private Sub handleElementChange()
        symbolBox.Content = el.Symbol
        symbolBox.Foreground = Me.FindResource(el.State.ToString())
        weightBox.Text = Matter.Info.ToStringDecimal(el.AtomicMass)
        numberBox.Text = el.AtomicNumber.ToString
        Dim s As String = String.Empty
        For Each v As Integer In el.OxidationStates
            If s.Length > 0 Then s += vbNewLine
            If v > 0 Then s += "+"
            s += v.ToString()
        Next
        oxyBox.Text = s
        configBox.Text = el.GetElectronConfigurationByShellString()
        PaintGroup(el.Group)
        paintLabState()
        AddHandler el.LabStateChanged, AddressOf handleLabStateChanged
    End Sub

    Private Sub handleLabStateChanged(sender As Object, e As EventArgs)
        paintLabState()
    End Sub

    Private Sub paintLabState()
        If el.LabState = Matter.StateInLab.Unavailable Then
            iconCanvas.Background = Nothing
            symbolBox.BorderBrush = Nothing
        ElseIf el.LabState = Matter.StateInLab.Synthesizable Then
            symbolBox.BorderBrush = Brushes.White
        Else
            iconCanvas.Background = Me.FindResource(el.LabState.ToString())
            symbolBox.BorderBrush = Nothing
        End If
    End Sub

    Public Sub PaintGroup(g As Matter.Element.Groups)
        Dim srcBr As SolidColorBrush = Me.FindResource(g.ToString)
        Dim c1 As Color = Color.Multiply(srcBr.Color, 0.5)
        c1.A = Byte.MaxValue
        Me.Background = New SolidColorBrush(c1)
        Me.BorderBrush = srcBr
    End Sub


End Class
Imports System.ComponentModel

Public Class ReactionList
    Implements INotifyPropertyChanged

    Public Shared ReadOnly RowBackgroundProperty As DependencyProperty =
        DependencyProperty.Register("RowBackground", GetType(Brush), GetType(ReactionList), New UIPropertyMetadata(Nothing))
    Public Shared ReadOnly AlternatingRowBackgroundProperty As DependencyProperty =
        DependencyProperty.Register("AlternatingRowBackground", GetType(Brush), GetType(ReactionList), New UIPropertyMetadata(Nothing))
    Public Shared ReadOnly AlternateIndexProperty As DependencyProperty =
        DependencyProperty.Register("AlternateIndex", GetType(Integer), GetType(ReactionList), New UIPropertyMetadata(1))
    Public Shared ReadOnly RowSeperatorProperty As DependencyProperty =
        DependencyProperty.Register("RowSeperator", GetType(Brush), GetType(ReactionList), New UIPropertyMetadata(Nothing))
    Public Shared ReadOnly CanSelectProperty As DependencyProperty =
        DependencyProperty.Register("CanSelect", GetType(Boolean), GetType(ReactionList), New UIPropertyMetadata(False))
    Public Shared ReadOnly HoverBrushProperty As DependencyProperty =
        DependencyProperty.Register("HoverBrush", GetType(Brush), GetType(ReactionList), New UIPropertyMetadata(New SolidColorBrush(Color.FromArgb(32, 255, 255, 255))))
    Public Shared ReadOnly SelectedBrushProperty As DependencyProperty =
        DependencyProperty.Register("SelectedBrush", GetType(Brush), GetType(ReactionList), New UIPropertyMetadata(Brushes.DeepSkyBlue))

    <Description("The brush to paint the row backgrounds with."), Category("Brush")>
    Public Property RowBackground As Brush
        Get
            Return rowB
        End Get
        Set(value As Brush)
            If Not rowB.Equals(value) Then
                rowB = value
                NotifyPropertyChanged("RowBackground")
            End If
        End Set
    End Property
    Private rowB As Brush

    <Description("The brush to paint the alternating row backgrounds with."), Category("Brush")>
    Public Property AlternatingRowBackground As Brush
        Get
            Return altRowB
        End Get
        Set(value As Brush)
            If Not altRowB.Equals(value) Then
                altRowB = value
                NotifyPropertyChanged("AlternatingRowBackground")
            End If
        End Set
    End Property
    Private altRowB As Brush

    <Description("The brush to paint the row backgrounds with when the mouse is over the control."), Category("Brush")>
    Public Property HoverBrush As Brush
        Get
            Return hovB
        End Get
        Set(value As Brush)
            If Not hovB.Equals(value) Then
                hovB = value
                NotifyPropertyChanged("HoverBrush")
            End If
        End Set
    End Property
    Private hovB As Brush

    <Description("The brush to paint the row backgrounds with when the row is selected."), Category("Brush")>
    Public Property SelectedBrush As Brush
        Get
            Return selB
        End Get
        Set(value As Brush)
            If Not selB.Equals(value) Then
                selB = value
                NotifyPropertyChanged("SelectedBrush")
            End If
        End Set
    End Property
    Private selB As Brush

    <Description("The brush to paint the row seperator with."), Category("Brush")>
    Public Property RowSeperator As Brush
        Get
            Return rowSepB
        End Get
        Set(value As Brush)
            If Not rowSepB.Equals(value) Then
                rowSepB = value
                NotifyPropertyChanged("RowSeperator")
            End If
        End Set
    End Property
    Private rowSepB As Brush

    <Description("The index(and interval-1) at which rows alternate."), Category("Common")>
    Public Property AlternateIndex As Integer
        Get
            Return altIn
        End Get
        Set(value As Integer)
            If value <> altIn Then
                altIn = value
                NotifyPropertyChanged("AlternateIndex")
            End If
        End Set
    End Property
    Private altIn As Integer

    <Description("Boolean value representing if items can be selected"), Category("Common")>
    Public Property CanSelect As Boolean
        Get
            Return sel_
        End Get
        Set(value As Boolean)
            If value Xor sel_ Then
                sel_ = value
                NotifyPropertyChanged("CanSelect")
            End If
        End Set
    End Property
    Private sel_ As Boolean

    Public Event RowClicked As RoutedEventHandler

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Public Sub LoadReactions(ByVal reactions As IEnumerable(Of Matter.Reaction), ByVal Optional clear As Boolean = True)
        If clear Then basePanel.Children.Clear()
        Dim counter As Integer = 0
        Dim sepBinding As New Binding("RowSeperator")
        Dim backBinding As New Binding("RowBackground")
        Dim altBackBinding As New Binding("AlternatingRowBackground")
        Dim frontBinding As New Binding("Foreground")
        sepBinding.Source = Me
        backBinding.Source = Me
        altBackBinding.Source = Me
        frontBinding.Source = Me
        Dim rrstyle As New Style(GetType(ReactionRow))
        rrstyle.Setters.Add(New Setter(ReactionRow.ForegroundProperty, frontBinding))
        rrstyle.Setters.Add(New Setter(ReactionRow.BackgroundProperty, backBinding))
        rrstyle.Setters.Add(New Setter(ReactionRow.HorizontalAlignmentProperty, HorizontalAlignment.Left))
        rrstyle.Setters.Add(New EventSetter(ReactionRow.ClickEvent, New RoutedEventHandler(AddressOf rowClickHandler)))
        rrstyle.Setters.Add(New EventSetter(ReactionRow.MouseEnterEvent, New MouseEventHandler(AddressOf rowEnterHover)))
        rrstyle.Setters.Add(New EventSetter(ReactionRow.MouseLeaveEvent, New MouseEventHandler(AddressOf rowExitHover)))
        rrstyle.Seal()

        For Each r In reactions
            Dim rr As New ReactionRow()
            rr.Style = rrstyle
            rr.Reaction = r
            If Me.GetValue(ReactionList.AlternateIndexProperty) > 0 AndAlso ((counter + 1) Mod (Me.GetValue(ReactionList.AlternateIndexProperty) + 1) = 0) Then
                rr.SetBinding(ReactionRow.BackgroundProperty, altBackBinding)
            End If
            If Not IsNothing(Me.GetValue(ReactionList.RowSeperatorProperty)) Then
                Dim sep As New Rectangle()
                sep.SetBinding(Rectangle.FillProperty, sepBinding)
                sep.HorizontalAlignment = HorizontalAlignment.Stretch
                sep.Height = 1
                basePanel.Children.Add(sep)
            End If
            basePanel.Children.Add(rr)
            counter += 1
        Next
        Dim rect As New Rectangle()
        rect.Height = 8
        rect.HorizontalAlignment = HorizontalAlignment.Stretch
        rect.VerticalAlignment = VerticalAlignment.Top
        basePanel.Children.Add(rect)
    End Sub

    Private selRow As ReactionRow
    Private Sub rowClickHandler(sender As Object, e As RoutedEventArgs)
        If Me.GetValue(ReactionList.CanSelectProperty) Then
            If Not IsNothing(selRow) Then setBackground(selRow, Brushes.Transparent)
            selRow = sender
            setBackground(sender, Me.GetValue(ReactionList.SelectedBrushProperty))
        End If
        RaiseEvent RowClicked(sender, e)
    End Sub

    Private Sub rowEnterHover(sender As Object, e As MouseEventArgs)
        If Not sender.Equals(selRow) Then setBackground(sender, Me.GetValue(ReactionList.HoverBrushProperty))
    End Sub

    Private Sub rowExitHover(sender As Object, e As MouseEventArgs)
        If Not sender.Equals(selRow) Then setBackground(sender, Brushes.Transparent)
    End Sub

    Private Sub setBackground(target As ReactionRow, b As Brush)
        CType(target.Content, Grid).Background = b
    End Sub

End Class

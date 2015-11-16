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

        For Each r In reactions
            Dim rr As New ReactionRow()
            rr.Reaction = r
            If Me.GetValue(ReactionList.AlternateIndexProperty) > 0 AndAlso ((counter + 1) Mod (Me.GetValue(ReactionList.AlternateIndexProperty) + 1) = 0) Then
                rr.SetBinding(ReactionRow.BackgroundProperty, altBackBinding)
            Else
                rr.SetBinding(ReactionRow.BackgroundProperty, backBinding)
            End If
            rr.SetBinding(ReactionRow.ForegroundProperty, frontBinding)
            rr.HorizontalAlignment = HorizontalAlignment.Stretch
            If Not IsNothing(Me.GetValue(ReactionList.RowSeperatorProperty)) Then
                rr.BorderThickness = New Thickness(0, 0, 0, 1)
                rr.SetBinding(ReactionRow.BorderBrushProperty, sepBinding)
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

End Class

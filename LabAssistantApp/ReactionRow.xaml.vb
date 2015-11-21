Imports System.ComponentModel
Imports LabAssistantApp.Matter

Public Class ReactionRow
    Inherits ClickableControl
    Implements INotifyPropertyChanged

    Public Shared ReadOnly ReactionProperty As DependencyProperty =
        DependencyProperty.Register("Reaction", GetType(Reaction), GetType(ReactionRow), New UIPropertyMetadata(Nothing))

    Public Property Reaction As Reaction
        Get
            Return reac
        End Get
        Set(value As Reaction)
            If IsNothing(reac) Then
                If Not IsNothing(value) Then
                    reac = value
                    SetReaction(reac)
                    OnPropertyChanged("Reaction")
                End If
            Else
                If IsNothing(value) Then
                    reac = value
                    SetReaction(reac)
                    OnPropertyChanged("Reaction")
                Else
                    If Not value.Equals(reac) Then
                        reac = value
                        SetReaction(reac)
                        OnPropertyChanged("Reaction")
                    End If
                End If
            End If
        End Set
    End Property
    Private reac As Reaction

    Private displayConverter As New FormulaDisplayConverter()

    Private Sub SetReaction(ByVal r As Reaction)
        If r.IsReversible Then
            arrowPath.Data = Me.FindResource("reversibleReactionArrowAbsolute")
        Else
            arrowPath.Data = Me.FindResource("reactionArrowAbsolute")
        End If

        Dim reacList As New List(Of UIElement)
        For i As Integer = 0 To r.Reactants.Count - 1
            Dim cf As New CompoundFormula(r.Reactants.ElementAt(i))
            If i > 0 Then
                reacList.Add(getPlusGrid())
            End If
            Dim state As StateInLab = Chemical.GetLabStateFromFormula(cf.ToString())
            Dim coef As Integer = r.ReactantCoeficients.ElementAt(i)
            If Not (state = StateInLab.Unavailable) And coef > 1 Then
                Dim tb As New TextBlock()
                tb.Inlines.Add(coef)
                tb.VerticalAlignment = VerticalAlignment.Center
                tb.Foreground = Me.Foreground
                reacList.Add(tb)
                coef = -1
            End If
            reacList.Add(createLabel(coef, state, cf))
        Next

        Dim proList As New List(Of UIElement)
        For i As Integer = 0 To r.Products.Count - 1
            Dim cf As New CompoundFormula(r.Products.ElementAt(i))
            If i > 0 Then
                proList.Add(getPlusGrid())
            End If
            Dim state As StateInLab = Chemical.GetLabStateFromFormula(cf.ToString())
            Dim coef As Integer = r.ProductCoeficients.ElementAt(i)
            If Not (state = StateInLab.Unavailable) And coef > 1 Then
                Dim tb As New TextBlock()
                tb.Inlines.Add(coef)
                tb.VerticalAlignment = VerticalAlignment.Center
                tb.Foreground = Me.Foreground
                proList.Add(tb)
                coef = -1
            End If
            proList.Add(createLabel(coef, state, cf))
        Next

        reactantsPanel.Children.Clear()
        For Each obj As UIElement In reacList
            reactantsPanel.Children.Add(obj)
        Next
        productsPanel.Children.Clear()
        For Each obj As UIElement In proList
            productsPanel.Children.Add(obj)
        Next

        If r.Status = Reaction.ReactionStatus.Recreatable Then
            recreatablePath.Visibility = Visibility.Visible
        Else
            recreatablePath.Visibility = Visibility.Hidden
        End If
    End Sub

    Private Function getPlusGrid() As Grid
        Dim g As New Grid()
        g.RowDefinitions.Add(getRowDef)
        g.RowDefinitions.Add(getRowDef)
        g.RowDefinitions.Add(getRowDef)
        Dim plusPath As New Path()
        Grid.SetRow(plusPath, 1)
        plusPath.Stretch = Stretch.Uniform
        Dim dcc As New DarkColorConverter()
        plusPath.StrokeThickness = 2
        plusPath.Stroke = dcc.Convert(Me.Foreground, Nothing, 7, Nothing)
        plusPath.Data = Me.FindResource("reactionPlusAbsolute")
        g.Children.Add(plusPath)
        Return g
    End Function

    Private Function createLabel(ByVal coef As Integer, ByVal state As StateInLab, ByVal formula As CompoundFormula) As Label
        Dim l As New Label()
        If state = StateInLab.Unavailable Then
            l.Foreground = Me.Foreground
        Else
            l.Foreground = Me.FindResource(state.ToString())
        End If
        l.VerticalContentAlignment = VerticalAlignment.Center
        l.Content = displayConverter.Convert(formula, Nothing, coef, Nothing)
        If coef < 0 Then l.Padding = New Thickness(0, 5, 5, 5)
        Return l
    End Function

    Private Shared Function getRowDef() As RowDefinition
        Dim r As New RowDefinition()
        r.Height = New GridLength(1, GridUnitType.Star)
        Return r
    End Function

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

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

Imports LabAssistantApp.Matter

Public Class CompoundInfoWindow

    Public Shared ReadOnly DisplayedCompoundProperty As DependencyProperty =
        DependencyProperty.Register("DisplayedCompound", GetType(Element), GetType(TableButton), New UIPropertyMetadata(Nothing))

    Public Property DisplayedCompound As Compound
        Get
            Return co
        End Get
        Set(value As Compound)
            co = value
        End Set
    End Property
    Private co As Compound

    Public Sub New(ByRef c As Compound)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        co = c

        Me.Title = co.Name
        Dim tb As New TextBlock()
        tb.Text = co.Appearance
        tb.TextWrapping = TextWrapping.Wrap
        appearanceLabel.Content = tb
        Dim fc As New FormulaDisplayConverter
        formulaLabel.Content = fc.Convert(co.Formula, Nothing, Nothing, Nothing)
        If co.Density < Math.Pow(10, -2) Then
            densityLabel.Content = String.Format("{0} mg/mL", ToStringDecimal(co.Density * Math.Pow(10, 3)))
        Else
            densityLabel.Content = String.Format("{0} g/mL", ToStringDecimal(co.Density))
        End If
        If IsNothing(co.MeltingPoint) Then
            meltingLabel.Content = "Unknown"
        Else
            meltingLabel.Content = String.Concat(ToStringDecimal(co.MeltingPoint), " K")
        End If
        If IsNothing(co.BoilingPoint) Then
            boilingLabel.Content = "Unknown"
        Else
            boilingLabel.Content = String.Concat(ToStringDecimal(co.BoilingPoint), " K")
        End If
        solubilityLabel.Content = co.Solubility

        statusLabel.Content = getStatusText()
        molarMassLabel.Content = String.Format("{0} g/mol", co.Formula.GetMolarMass())
        reactionList.LoadReactions(Reaction.GetAllOf(co.FormulaString))
    End Sub

    Private Function getStatusText() As String
        Dim labstate As StateInLab = co.LabState
        Select Case labstate
            Case StateInLab.Available
                Return "Available (unlimited)"
            Case StateInLab.In_Stock
                Dim mass As Double = Info.LoadedLab.GetInfoOf(co.FormulaString).Mass
                If mass >= 1000 Then
                    Return String.Format("In stock ({0} kg)", Info.ToStringDecimal(mass, 3))
                Else
                    Return String.Format("In stock ({0} g)", Info.ToStringDecimal(mass * 1000, 3))
                End If
            Case StateInLab.Synthesizable
                Return "Synthesizable"
            Case Else
                Return "Unavailable"
        End Select
    End Function

    Private Sub handleRowClick(sender As Object, e As RoutedEventArgs) Handles reactionList.RowClicked
        CType(My.Application.MainWindow, LabWindow).SelectedReaction = CType(sender, ReactionRow).Reaction
    End Sub

End Class

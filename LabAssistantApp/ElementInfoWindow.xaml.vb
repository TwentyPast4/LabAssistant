Imports LabAssistantApp.Matter

Public Class ElementInfoWindow

    Public Shared ReadOnly DisplayedElementProperty As DependencyProperty =
        DependencyProperty.Register("DisplayedElement", GetType(Element), GetType(TableButton), New UIPropertyMetadata(Nothing))

    Public Property DisplayedElement As Element
        Get
            Return el
        End Get
        Set(value As Element)
            el = value
        End Set
    End Property
    Private el As Element

    Public Sub New(ByRef e As Element)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        el = e

        infoElement.Element = el
        Me.Title = el.Name
        Dim tb As New TextBlock()
        tb.Text = el.Appearance
        tb.TextWrapping = TextWrapping.Wrap
        appearanceLabel.Content = tb
        If el.Density < Math.Pow(10, -2) Then
            densityLabel.Content = String.Format("{0} mg/mL", ToStringDecimal(el.Density * Math.Pow(10, 3)))
        Else
            densityLabel.Content = String.Format("{0} g/mL", ToStringDecimal(el.Density))
        End If
        meltingLabel.Content = String.Concat(ToStringDecimal(el.MeltingPoint), " K")
        If IsNothing(el.BoilingPoint) Then
            boilingLabel.Content = "Unknown"
        Else
            boilingLabel.Content = String.Concat(ToStringDecimal(el.BoilingPoint), " K")
        End If
        categoryLabel.Content = el.GetGroupName()
        groupLabel.Content = el.TableGroupNumber
        periodLabel.Content = el.Period
    End Sub

End Class

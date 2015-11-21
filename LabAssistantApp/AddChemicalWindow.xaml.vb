Public Class AddChemicalWindow

    Public ReadOnly Property Amount As Double
        Get
            Dim am As Double
            If isUnlimited.Enabled Then
                Return -1
            Else
                am = Matter.Info.ToDouble(amountBox.Text)
            End If
            Return Convert(am, unitBox.SelectedIndex, Matter.UnitOfMass.Kilogram, loadedChem.Formula)
        End Get
    End Property

    Public ReadOnly Property Comment As String
        Get
            Return commentBox.Text
        End Get
    End Property

    Private Sub checkNumber(sender As Object, e As TextCompositionEventArgs) Handles amountBox.PreviewTextInput
        Dim b As Boolean = True
        Dim i As Integer = 0
        While b And i < e.Text.Length
            b = Char.IsNumber(e.Text.Chars(i)) Or e.Text.Chars(i).Equals(Constants.Period)
        End While
        e.Handled = b
    End Sub


    Private loadedChem As Matter.Chemical
    Public Sub New(ByVal c As Matter.Chemical)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        loadedChem = c
        chemicalName.Content = c.Name.ToUpper()
    End Sub

    Private Sub okClicked(sender As Object, e As RoutedEventArgs) Handles okBtn.Click
        If isUnlimited.Enabled Then
            Me.DialogResult = True
        ElseIf amountBox.Text.Length > 0 Then
            If isNumber(amountBox.Text) Then
                If Matter.Info.ToDouble(amountBox.Text) > 0 Then
                    Me.DialogResult = True
                Else
                    MsgBox("Amount cannot be 0.", MsgBoxStyle.Exclamation, "Lab Assistant")
                End If
            Else
                MsgBox("Amount is not a number.", MsgBoxStyle.Exclamation, "Lab Assistant")
            End If
        Else
            MsgBox("Amount cannot be empty.", MsgBoxStyle.Exclamation, "Lab Assistant")
        End If
    End Sub

    Private Sub cancelClicked(sender As Object, e As RoutedEventArgs) Handles cancelBtn.Click
        Me.DialogResult = False
    End Sub

End Class

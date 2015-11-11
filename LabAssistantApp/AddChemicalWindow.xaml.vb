Public Class AddChemicalWindow

    Private Sub checkNumber(sender As Object, e As TextCompositionEventArgs) Handles amountBox.PreviewTextInput
        Dim b As Boolean = True
        Dim i As Integer = 0
        While b And i < e.Text.Length
            b = Char.IsNumber(e.Text.Chars(i)) Or e.Text.Chars(i).Equals(Constants.Period)
        End While
        e.Handled = b
    End Sub

End Class

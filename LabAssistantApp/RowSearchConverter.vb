Imports System.Globalization

Public Class RowSearchConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Dim chemName As String = CType(value, Matter.Chemical).Name
        chemName.ToLower.Replace(Space(1), String.Empty)
        Dim searchPhrase As String = parameter
        searchPhrase = searchPhrase.ToLower.Replace(Space(1), String.Empty)
        If searchPhrase.Length = 0 OrElse (chemName.Contains(searchPhrase) Or searchPhrase.Contains(chemName)) Then
            Return Visibility.Visible
        Else
            Return Visibility.Collapsed
        End If
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class

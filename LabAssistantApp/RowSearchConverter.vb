Imports System.Globalization

Public Class RowSearchConverter
    Implements IMultiValueConverter

    Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
        If IsNothing(values(0)) Then Return True
        Dim chemName As String = values(0).Name
        chemName = chemName.ToLower.Replace(Space(1), String.Empty)
        Dim searchFor As String = values(1)
        searchFor = searchFor.ToLower.Replace(Space(1), String.Empty)
        If searchFor.Length = 0 OrElse (chemName.Contains(searchFor) Or searchFor.Contains(chemName)) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function ConvertBack(value As Object, targetTypes() As Type, parameter As Object, culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function

End Class

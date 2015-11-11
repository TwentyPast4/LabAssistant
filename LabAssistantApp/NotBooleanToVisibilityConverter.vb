Imports System.Globalization

Public Class NotBooleanToVisibilityConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If CBool(value) Then Return Visibility.Collapsed Else Return Visibility.Visible
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Nothing
    End Function
End Class

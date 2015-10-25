Imports System.Globalization

Public Class DarkColorConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert

        Dim c As Color = CType(value, SolidColorBrush).Color
        Dim p As Double = CInt(parameter) / 10
        c.R *= p
        c.G *= p
        c.B *= p
        Return New SolidColorBrush(c)
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Nothing
    End Function
End Class

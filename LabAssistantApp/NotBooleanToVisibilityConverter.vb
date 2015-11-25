Imports System.Globalization

Public Class NotBooleanToVisibilityConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If CBool(value) Then
            If IsNothing(parameter) Then
                Return Visibility.Collapsed
            Else
                Return False
            End If
        Else
            If IsNothing(parameter) Then
                Return Visibility.Visible
            Else
                Return True
            End If
        End If
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Nothing
    End Function
End Class

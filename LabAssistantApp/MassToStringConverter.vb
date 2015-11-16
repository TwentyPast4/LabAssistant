Imports System.Globalization

Public Class MassToStringConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Dim val As Double = value
        If val < 0 Then
            Return "Unlimited"
        ElseIf val < 1 Then
            Return String.Format("{0} g", Matter.Info.ToStringDecimal(val, 3))
        Else
            Return String.Format("{0} kg", Matter.Info.ToStringDecimal(val, 3))
        End If
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Nothing
    End Function
End Class

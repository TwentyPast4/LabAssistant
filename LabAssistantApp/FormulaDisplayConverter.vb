Imports System.Globalization

Public Class FormulaDisplayConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Dim f As Matter.CompoundFormula = value
        Dim tb As New TextBlock()
        For Each rs In f.Raw
            Dim s As New Span()
            If rs.Type = Matter.CompoundFormula.RawStructure.Types.Number Then
                s.BaselineAlignment = BaselineAlignment.Subscript
                s.FontSize *= 0.8
            End If
            s.Inlines.Add(rs.Value)
            tb.Inlines.Add(s)
        Next
        Return tb
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class

Imports System.Globalization

Public Class FormulaDisplayConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Dim f As Matter.CompoundFormula
        If value.GetType.Equals(GetType(String)) Then
            f = New Matter.CompoundFormula(value)
        Else
            f = value
        End If
        Dim tb As New TextBlock()
        If Not IsNothing(parameter) Then
            Dim coef As Integer = parameter
            If coef > 1 Then
                tb.Inlines.Add(coef.ToString())
            End If
        End If
        For Each rs In f.Raw
            Dim s As New Span()
            If rs.Type = Matter.CompoundFormula.RawStructure.Types.Number Then
                s.BaselineAlignment = BaselineAlignment.Subscript
                If IsNothing(targetType) Then
                    s.FontSize *= 1.1
                Else
                    s.FontSize *= 0.9
                End If
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

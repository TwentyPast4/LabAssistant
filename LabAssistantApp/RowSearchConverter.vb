Imports System.Globalization

Public Class RowSearchConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Dim s As String = value
        s = s.ToLower().Replace(Space(1), String.Empty)
        If s.Length > 0 Then
            If parameter.ToString.Equals("inorganic") Then
                Return Matter.Compound.GetAllOfType(False).Where(Function(ByVal c As Matter.Compound) c.Name.ToLower.Replace(Space(1), String.Empty).Contains(s))
            ElseIf parameter.ToString.Equals("organic") Then
                Return Matter.Compound.GetAllOfType(True).Where(Function(ByVal c As Matter.Compound) c.Name.ToLower.Replace(Space(1), String.Empty).Contains(s))
            Else
                Return Matter.Compound.CompoundList.Where(Function(ByVal c As Matter.Compound) c.Name.ToLower.Replace(Space(1), String.Empty).Contains(s))
            End If
        Else
            If parameter.ToString.Equals("inorganic") Then
                Return Matter.Compound.GetAllOfType(False)
            ElseIf parameter.ToString.Equals("organic") Then
                Return Matter.Compound.GetAllOfType(True)
            Else
                Return Matter.Compound.CompoundList
            End If
        End If
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function

End Class

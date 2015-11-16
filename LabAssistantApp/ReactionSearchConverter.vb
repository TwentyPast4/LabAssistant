Imports System.Globalization

Public Class ReactionSearchConverter
    Implements IValueConverter

    Private Enum ReactionString
        Complete
        Complete_Opposite
        Complete_Reversible
        Incomplete
        Invalid
    End Enum

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Dim s As String = value
        Select Case isCorrectFormat(s)
            Case ReactionString.Complete_Opposite
                Dim right As String = s.Substring(0, s.IndexOf("<"))
                Dim left As String = s.Substring(s.IndexOf("<")).Remove(0, 1)
            Case ReactionString.Complete_Reversible
                Dim left As String = s.Substring(0, s.IndexOf("<>"))
                Dim right As String = s.Substring(s.IndexOf("<>")).Remove(0, 2)

            Case ReactionString.Complete
                Dim left As String = s.Substring(0, s.IndexOf(">"))
                Dim right As String = s.Substring(s.IndexOf(">")).Remove(0, 1)


            Case ReactionString.Incomplete

            Case Else
                Return Nothing
        End Select
    End Function

    Private Function SolveFor(ByVal leftSide As String, ByVal [Type] As ReactionString, ByVal Optional rightSide As String = "") As List(Of Matter.Reaction)
        Dim partsL() As String = leftSide.Split("+".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
        Dim partsR() As String = rightSide.Split("+".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
        If Type = ReactionString.Incomplete Then
            For Each r In Matter.Reaction.ReactionList
                Matter.Reaction.ge
            Next
        End If
    End Function

    Private Function isCorrectFormat(ByVal s As String) As ReactionString
        If s.Contains("-") Then Return ReactionString.Invalid
        Dim countRight As Integer = sequenceCount(s, ">")
        If countRight > 1 Then Return ReactionString.Invalid
        Dim countLeft As Integer = sequenceCount(s, "<")
        If countLeft > 1 Then
            Return ReactionString.Invalid
        ElseIf countLeft = 1 And countRight = 0 Then
            Return ReactionString.Complete_Opposite
        ElseIf countLeft = 1 And countRight = 1 Then
            If s.IndexOf("<") + 1 <> s.IndexOf(">") Then
                Return ReactionString.Invalid
            Else
                Return ReactionString.Complete_Reversible
            End If
        End If
        If countRight = 1 Then Return ReactionString.Complete Else Return ReactionString.Incomplete
    End Function

    Private Function sequenceCount(s As String, s2 As String) As Integer
        Dim n As Integer = 0
        For i As Integer = 0 To s.Length - s2.Length
            Dim f As Integer = s.IndexOf(s2, i)
            If f < 0 Then
                Exit For
            Else
                n += 1
                i = f + s2.Length
            End If
        Next
        Return n
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function

End Class

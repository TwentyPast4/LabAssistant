Imports LabAssistantApp.Matter

Public Module ReactionSearchConverter

    Public Enum SearchType
        All
        Decomposition
        Synthesis
        Electrolysis
        Other
    End Enum

    Private Enum ReactionString
        Complete
        Complete_Opposite
        Complete_Reversible
        Incomplete
        Invalid
    End Enum

    Public Function FindReactions(value As Object, Optional reactionType As SearchType = SearchType.All) As List(Of Reaction)
        Dim s As String = value
        If s.Equals("-") Then
            FindReactions = Reaction.ReactionList
        Else
            Select Case isCorrectFormat(s)
                Case ReactionString.Complete_Opposite
                    Dim right As String = s.Substring(0, s.IndexOf("<"))
                    Dim left As String = s.Substring(s.IndexOf("<")).Remove(0, 1)
                    FindReactions = SolveFor(left, ReactionString.Complete_Opposite, right)
                Case ReactionString.Complete_Reversible
                    Dim left As String = s.Substring(0, s.IndexOf("<>"))
                    Dim right As String = s.Substring(s.IndexOf("<>")).Remove(0, 2)
                    FindReactions = SolveFor(left, ReactionString.Complete_Reversible, right)
                Case ReactionString.Complete
                    Dim left As String = s.Substring(0, s.IndexOf(">"))
                    Dim right As String = s.Substring(s.IndexOf(">")).Remove(0, 1)
                    FindReactions = SolveFor(left, ReactionString.Complete, right)
                Case ReactionString.Incomplete
                    FindReactions = SolveFor(s, ReactionString.Incomplete)
                Case Else
                    Return New List(Of Reaction)
            End Select
        End If
        Select Case reactionType
            Case SearchType.Decomposition
                FindReactions = FindReactions.Where(Function(r As Reaction) r.Type = Reaction.ReactionType.Decomposition).ToList
            Case SearchType.Other
                FindReactions = FindReactions.Where(Function(r As Reaction) r.Type = Reaction.ReactionType.Other).ToList
            Case SearchType.Synthesis
                FindReactions = FindReactions.Where(Function(r As Reaction) r.Type = Reaction.ReactionType.Synthesis).ToList
            Case SearchType.Electrolysis
                FindReactions = FindReactions.Where(Function(r As Reaction) r.IsElectrolytic).ToList
        End Select
    End Function

    Private Function SolveFor(ByVal leftSide As String, ByVal [Type] As ReactionString, ByVal Optional rightSide As String = "") As List(Of Reaction)
        Dim partsL As List(Of String) = leftSide.Split("+".ToCharArray, StringSplitOptions.RemoveEmptyEntries).ToList
        Dim partsR As List(Of String) = rightSide.Split("+".ToCharArray, StringSplitOptions.RemoveEmptyEntries).ToList
        For i As Integer = 0 To partsL.Count - 1
            partsL(i) = partsL(i).Trim
            If partsL(i).Length = 0 Then partsL.RemoveAt(i)
        Next
        For i As Integer = 0 To partsR.Count - 1
            partsR(i) = partsR(i).Trim
            If partsR(i).Length = 0 Then partsR.RemoveAt(i)
        Next

        SolveFor = New List(Of Reaction)
        Dim cFormulasL(partsL.Count - 1) As CompoundFormula
        For i As Integer = 0 To partsL.Count - 1
            cFormulasL(i) = New CompoundFormula(partsL(i))
        Next
        Dim cFormulasR(partsR.Count - 1) As CompoundFormula
        For i As Integer = 0 To partsR.Count - 1
            cFormulasR(i) = New CompoundFormula(partsR(i))
        Next
        If Type = ReactionString.Incomplete Then
            Return Reaction.ReactionList.Where(Function(r As Reaction) (r.Reactants.Intersect(partsL).Count = partsL.Count) Or (r.Products.Intersect(partsL).Count = partsL.Count)).ToList
        ElseIf Type = ReactionString.Complete Or Type = ReactionString.Complete_Opposite Then
            Return Reaction.ReactionList.Where(Function(r As Reaction) (r.Reactants.Intersect(partsL).Count = partsL.Count) And (r.Products.Intersect(partsR).Count = partsR.Count) And Not r.IsReversible).ToList
        ElseIf Type = ReactionString.Complete_Reversible
            Return Reaction.GetAllReversible().Where(Function(r As Reaction) ((r.Reactants.Intersect(partsL).Count = partsL.Count) And (r.Products.Intersect(partsR).Count = partsR.Count)) _
                                                   Or ((r.Reactants.Intersect(partsR).Count = partsR.Count) And (r.Products.Intersect(partsL).Count = partsL.Count))).ToList
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

End Module

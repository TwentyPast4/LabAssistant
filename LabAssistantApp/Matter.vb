Imports System.Globalization

Namespace Matter

    Public Enum StateOfMatter
        Solid
        Liquid
        Gas
    End Enum

    Public Enum StateInLab
        Unavailable
        Available
        Synthesizable
        In_Stock
    End Enum

    Public Enum UnitOfMass
        Kilogram
        Gram
        Pound
        Ounce
    End Enum

    Public Enum UnitOfTemperature
        Kelvin
        Celsius
        Fahrenheit
    End Enum

    Public Structure ChemistryConstants
        Public Const ZeroKelvin As Double = -273.15
        Public Const AvogadrosNumber As Decimal = 6.02214129 * 10 ^ 23
        Public Const FaradaysConstant As Decimal = 9.64853399 * 10 ^ 4
        Public Const IdealGasConstant As Decimal = 8.3144621
        Public Const RoomTemperature As Decimal = 293.15
    End Structure

    Public Class LabStateChangedEventArgs
        Inherits EventArgs

        Public ReadOnly Property StateInLab As StateInLab
            Get
                Return state_
            End Get
        End Property
        Private state_ As StateInLab

        Public ReadOnly Property PreviousStateInLab As StateInLab
            Get
                Return prestate_
            End Get
        End Property
        Private prestate_ As StateInLab

        Sub New(ByVal NewValue As StateInLab, ByVal OldValue As StateInLab)
            state_ = NewValue
            prestate_ = OldValue
        End Sub

    End Class

    Public MustInherit Class Chemical

        Public ReadOnly Property Name As String
            Get
                Return name_
            End Get
        End Property
        Protected name_ As String

        Public ReadOnly Property LabState As StateInLab
            Get
                Return labstate_
            End Get
        End Property
        Protected labstate_ As StateInLab = StateInLab.Unavailable

        Public ReadOnly Property State As StateOfMatter
            Get
                Return state_
            End Get
        End Property
        Protected state_ As StateOfMatter

        Public ReadOnly Property MeltingPoint As Single?
            Get
                If melt_ < 0 Then Return Nothing Else Return melt_
            End Get
        End Property
        Protected melt_ As Single

        Public ReadOnly Property BoilingPoint As Single?
            Get
                If boil_ < 0 Then Return Nothing Else Return boil_
            End Get
        End Property
        Protected boil_ As Single

        Public ReadOnly Property Density As Decimal
            Get
                If dens_ < 0 Then Return Nothing Else Return dens_
            End Get
        End Property
        Protected dens_ As Decimal

        Public ReadOnly Property Formula As CompoundFormula
            Get
                Return formula_
            End Get
        End Property
        Protected formula_ As CompoundFormula

        Public ReadOnly Property FormulaString As String
            Get
                Return formula_.ToString()
            End Get
        End Property

        Public ReadOnly Property Appearance As String
            Get
                Return appearance_
            End Get
        End Property
        Protected appearance_ As String

        Public Event LabStateChanged As EventHandler(Of LabStateChangedEventArgs)

        Private Sub handleLabStateChanged(ByVal sender As Object, e As LabStateChangedEventArgs) Handles Me.LabStateChanged
            If Not IsNothing(Info.LoadedLab) Then
                If e.PreviousStateInLab = StateInLab.Available Or e.PreviousStateInLab = StateInLab.In_Stock Then
                    If e.StateInLab = StateInLab.Synthesizable Or e.StateInLab = StateInLab.Unavailable Then
                        Reaction.UpdateRecreatable()
                    End If
                Else
                    If e.StateInLab = StateInLab.Available Or e.StateInLab = StateInLab.In_Stock Then
                        Reaction.UpdateRecreatable()
                    End If
                End If
            End If
        End Sub

        Public Sub SetLabState(ByVal newState As StateInLab)
            If Not (newState = labstate_) Then
                Dim oldv As StateInLab = labstate_
                labstate_ = newState
                RaiseEvent LabStateChanged(Me, New LabStateChangedEventArgs(newState, oldv))
            End If
        End Sub

        Public Sub TryUpdateLabState()
            If Me.labstate_ = StateInLab.Available Or Me.labstate_ = StateInLab.In_Stock Then Exit Sub
            If (Not IsNothing(Info.LoadedLab)) AndAlso Info.LoadedLab.IsAvailable(formula_.ToString) Then
                If Info.LoadedLab.GetInfoOf(formula_.ToString).IsUnlimited Then
                    SetLabState(StateInLab.Available)
                Else
                    SetLabState(StateInLab.In_Stock)
                End If
            Else
                If Reaction.ReactionList.FindAll(
                    Function(x As Reaction) (x.Status = Reaction.ReactionStatus.Recreatable) AndAlso
                        (x.Products.FindAll(Function(y As String) y.Equals(formula_.ToString)).Count > 0)).Count > 0 Then
                    SetLabState(StateInLab.Synthesizable)
                Else
                    SetLabState(StateInLab.Unavailable)
                End If
            End If
        End Sub

        Public Function GetStateAt(ByVal temperature As Double) As StateOfMatter
            If melt_ >= temperature Then
                Return StateOfMatter.Solid
            ElseIf boil_ <= temperature Then
                Return StateOfMatter.Gas
            Else
                Return StateOfMatter.Liquid
            End If
        End Function

        Public Shared Function GetLabStateFromFormula(ByVal chemicalFormula As String)
            Dim elementFind As Element = Element.ElementList.Find(Function(ByVal x As Element) x.FormulaString.Equals(chemicalFormula))
            If IsNothing(elementFind) Then
                Dim compoundFind As Compound = Compound.CompoundList.Find(Function(ByVal x As Compound) x.FormulaString.Equals(chemicalFormula))
                If IsNothing(compoundFind) Then
                    Return Nothing
                Else
                    Return compoundFind.LabState
                End If
            Else
                Return elementFind.LabState
            End If
        End Function

    End Class

    Public Class Element
        Inherits Chemical

        Public Shared ElementList As New List(Of Element)

        Public Enum Groups
            Actinides
            AlkaliMetals
            AlkalineEarthMetals
            TransitionMetals
            Metalloids
            NonMetals
            Post_transitionMetals
            Halogens
            NobleGases
            Lanthanides
            Unknown
        End Enum

        Public Function GetGroupName() As String
            Return CallByName(New GroupStrings, gro.ToString, CallType.Get, Nothing)
        End Function

        Public Shared Function GetGroupName(ByVal group As Groups) As String
            Return CallByName(New GroupStrings, group.ToString, CallType.Get, Nothing)
        End Function

        Public ReadOnly Property AtomicMass As Double
            Get
                Return mass
            End Get
        End Property
        Private mass As Double

        Public ReadOnly Property MolarMass As Double
            Get
                Return formula_.GetMolarMass()
            End Get
        End Property

        Public ReadOnly Property Symbol As String
            Get
                Return sym
            End Get
        End Property
        Private sym As String

        Public ReadOnly Property OxidationStates As Integer()
            Get
                Return oxy
            End Get
        End Property
        Private oxy() As Integer

        Public ReadOnly Property Period As Integer
            Get
                Return per
            End Get
        End Property
        Private per As Integer

        Public ReadOnly Property Group As Groups
            Get
                Return gro
            End Get
        End Property
        Private gro As Groups

        Public ReadOnly Property TableGroupNumber As Integer
            Get
                Return groupNum
            End Get
        End Property
        Private groupNum As Integer

        Public ReadOnly Property AtomicNumber As Integer
            Get
                Return numb
            End Get
        End Property
        Private numb As Integer

        Public ReadOnly Property ElectronConfiguration As ElectronConfig()
            Get
                Return elconfig
            End Get
        End Property
        Private elconfig As ElectronConfig()

        Private Sub New(ByVal AtomicNumber As Integer, ByVal name As String, ByVal appearance As String, ByVal oxidationStates() As Integer, ByVal symbol As String, ByVal atomicMass As Double, ByVal meltingP As Single, ByVal boilingP As Single, ByVal density As Double)
            numb = AtomicNumber
            name_ = name
            appearance_ = appearance.Replace(Constants.NewLine, vbNewLine)
            sym = symbol
            mass = atomicMass
            melt_ = meltingP
            boil_ = boilingP
            dens_ = density
            oxy = oxidationStates
            state_ = GetStateAt(ChemistryConstants.RoomTemperature)
            per = GetPeriod(numb)
            gro = GetGroup()
            groupNum = getGroupNumber()
            formula_ = New CompoundFormula(MakeFormula(numb, sym, gro))
            elconfig = GetElConfig(AtomicNumber)
        End Sub

        Public Shared Sub Initialize()
            ElementList.Clear()
            Using sr As New IO.StringReader(My.Resources.ElementsResx)
                While sr.Peek > -1
                    If sr.ReadLine().Equals(String.Concat(Constants.TextFileStartElement, Constants.Column)) Then
                        Dim n As Integer = 1
                        While sr.Peek > -1
                            Dim name As String = sr.ReadLine()

                            ' 0=symbol 1=appearance 2=atomicWeight 3=density 4=oxidationStates 5=meltingPoint 6=boilingPoint
                            Dim raw(6) As String
                            For i As Integer = 0 To raw.Count - 1
                                raw(i) = sr.ReadLine().Remove(0, 1)
                            Next
                            Dim atomW As Double = ToDouble(raw(2))
                            Dim dens As Double
                            Dim ind As Integer = raw(3).IndexOf("e")
                            If ind > 0 Then
                                dens = Integer.Parse(raw(3).Remove(ind)) * 10 ^ (Integer.Parse(raw(3).Substring(ind + 1)))
                            Else
                                dens = ToDouble(raw(3))
                            End If
                            Dim oxi() As Integer = GetOxy(raw(4))
                            Dim melt As Double
                            If raw(5).Equals(Constants.NoInformation) Then melt = -1 Else melt = ToDouble(raw(5))
                            Dim boil As Double
                            If raw(6).Equals(Constants.NoInformation) Then boil = -1 Else boil = ToDouble(raw(6))
                            Dim e As New Element(n, name, raw(1), oxi, raw(0), atomW, melt, boil, dens)
                            ElementList.Add(e)
                            n += 1
                        End While
                    End If
                End While
            End Using

        End Sub

        Public Shared Function FromAtomicNumber(ByVal AtomicNumber As Integer) As Element
            Return ElementList.Find(Function(x As Element) x.AtomicNumber = AtomicNumber)
        End Function

        Public Shared Function FromName(ByVal name As String) As Element
            Return ElementList.Find(Function(x As Element) x.Name = name)
        End Function

        Public Shared Function GetElementFromSymbol(ByVal symbol As String) As Element
            Return ElementList.Find(Function(x As Element) x.Symbol.Equals(symbol))
        End Function

        Public Shared Function GetElementFromName(ByVal name As String) As Element
            Return ElementList.First(Function(ByVal comp As Element) comp.Name = name)
        End Function

        Private Shared Function MakeFormula(ByVal number As Integer, ByVal symbol As String, ByVal group As Groups) As String
            Select Case number
                Case Is = 1
                    Return String.Concat(symbol, 2)
                Case Is = 7
                    Return String.Concat(symbol, 2)
                Case Is = 8
                    Return String.Concat(symbol, 2)
                Case Is = 15
                    Return String.Concat(symbol, 4)
                Case Is = 33
                    Return String.Concat(symbol, 4)
                Case Else
                    If group = Groups.Halogens Then
                        Return String.Concat(symbol, 2)
                    Else
                        Return symbol
                    End If
            End Select
        End Function

        Private Shared Function GetOxy(ByVal raw As String) As Integer()
            Dim oxystr As String = raw
            Dim oxlst As New List(Of Integer)
            If oxystr.EndsWith("0") Then
                If oxystr = "0" Then
                    oxlst.Add(0)
                    Return oxlst.ToArray
                End If
                For Each num In oxystr.Replace("0", String.Empty).ToCharArray
                    oxlst.Add(-CInt(num.ToString))
                Next
                Return oxlst.ToArray
            End If
            If Not oxystr.Contains("0") Then
                For Each num In oxystr.ToCharArray
                    oxlst.Add(CInt(num.ToString))
                Next
                Return oxlst.ToArray
            End If
            For Each num In oxystr.Split("0")(0)
                oxlst.Add(-CInt(num.ToString))
            Next
            For Each num In oxystr.Split("0")(1)
                oxlst.Add(CInt(num.ToString))
            Next
            Return oxlst.ToArray
        End Function

        Private Shared Function GetPeriod(ByVal numb As Integer) As Integer
            If numb >= 3 And numb <= 10 Then Return 2
            If numb >= 11 And numb <= 18 Then Return 3
            If numb >= 19 And numb <= 36 Then Return 4
            If numb >= 37 And numb <= 54 Then Return 5
            If numb >= 55 And numb <= 86 Then Return 6
            If numb = 1 Or numb = 2 Then Return 1
            Return 7
        End Function

        Private Function getGroupNumber() As Integer

            If gro = Groups.AlkaliMetals Or numb = 1 Then Return 1
            If gro = Groups.AlkalineEarthMetals Then Return 2
            If numb > 102 Then Return numb - 100
            If gro = Groups.Actinides Then Return 0
            If numb > 70 Then Return numb - 68
            If gro = Groups.Lanthanides Then Return 0
            If numb > 38 Then Return numb - 36
            If numb > 20 Then Return numb - 18
            If numb > 12 Then Return numb
            If numb > 4 Then Return numb + 8
            Return numb
        End Function

        Private Function GetGroup() As Groups
            If numb = 1 Or (numb >= 6 And numb <= 8) Or numb = 15 Or numb = 16 Or numb = 34 Then Return Groups.NonMetals
            If numb = 2 Or numb = 10 Or numb = 18 Or numb = 36 Or numb = 54 Or numb = 86 Then Return Groups.NobleGases
            If numb = 3 Or numb = 11 Or numb = 19 Or numb = 37 Or numb = 55 Or numb = 87 Then Return Groups.AlkaliMetals
            If numb = 4 Or numb = 12 Or numb = 20 Or numb = 38 Or numb = 56 Or numb = 88 Then Return Groups.AlkalineEarthMetals
            If numb = 5 Or numb = 14 Or numb = 32 Or numb = 33 Or numb = 51 Or numb = 52 Or numb = 84 Then Return Groups.Metalloids
            If numb = 9 Or numb = 17 Or numb = 35 Or numb = 53 Or numb = 85 Then Return Groups.Halogens
            If numb = 13 Or numb = 31 Or numb = 49 Or numb = 50 Or (numb >= 81 And numb <= 83) Or numb = 114 Then Return Groups.Post_transitionMetals
            If numb >= 57 And numb <= 71 Then Return Groups.Lanthanides
            If numb >= 89 And numb <= 103 Then Return Groups.Actinides
            If (numb >= 109 And numb <= 111) Or (numb = 113) Or (numb >= 115 And numb <= 118) Then Return Groups.Unknown
            Return Groups.TransitionMetals
        End Function

        Public Shared ReadOnly Property Orbitals As Orbital()
            Get
                Return orbitals_
            End Get
        End Property
        Private Shared orbitals_ As Orbital() = getOrbitals()

        Private Shared Function getOrbitals() As Orbital()
            Dim d As New List(Of Orbital)
            For x As Single = 0 To 6 Step 0.5
                Dim curX As Integer = Math.Floor(x)
                Dim curY As Integer = Math.Ceiling(x)
                For delta As Integer = 0 To curX
                    Dim suffix As String = ""
                    Select Case curX - delta
                        Case 0
                            suffix = "s"
                        Case 1
                            suffix = "p"
                        Case 2
                            suffix = "d"
                        Case 3
                            suffix = "f"
                        Case 4
                            suffix = "g"
                        Case 5
                            suffix = "h"
                        Case 6
                            suffix = "i"
                    End Select
                    d.Add(New Orbital(suffix, curY + delta + 1, 2 + 4 * (curX - delta)))
                Next
            Next
            Return d.ToArray()
        End Function

        Public Class Orbital
            Public ReadOnly Property Name(ByVal withShellNumber As Boolean) As String
                Get
                    If withShellNumber Then
                        Return String.Concat(shell_, name_)
                    Else
                        Return name_
                    End If
                End Get
            End Property
            Private name_ As Char

            Public ReadOnly Property Shell As Integer
                Get
                    Return shell_
                End Get
            End Property
            Private shell_ As Integer

            Public ReadOnly Property Capacity As Integer
                Get
                    Return cap_
                End Get
            End Property
            Private cap_ As Integer

            Friend Sub New(ByVal oName As String, ByVal oShell As Integer, ByVal oCapacity As Integer)
                name_ = oName
                shell_ = oShell
                cap_ = oCapacity
            End Sub

            Public Overloads Function Equals(obj As Orbital) As Boolean
                Return (name_ = obj.name_ And shell_ = obj.shell_ And cap_ = obj.cap_)
            End Function
        End Class

        Public Class ElectronConfig
            Public ReadOnly Property Orbital As Orbital
                Get
                    Return orb_
                End Get
            End Property
            Private orb_ As Orbital
            Public ReadOnly Property NumberOfElectrons As Integer
                Get
                    Return ele_
                End Get
            End Property
            Private ele_ As Integer
            Friend Sub New(ByRef orb As Orbital, ByVal ele As Integer)
                orb_ = orb
                ele_ = ele
            End Sub
            Friend Sub New(ByVal orbitalName As String, ByVal ele As Integer)
                orb_ = Orbitals.First(Function(ByVal com As Orbital) com.Name(True).Equals(orbitalName))
                ele_ = ele
            End Sub

            Public Shared Function AddOrbital(ByRef e As ElectronConfig(), ByVal add As ElectronConfig) As ElectronConfig()
                Dim ec As ElectronConfig = e.ToList().Find(Function(ByVal com As ElectronConfig) com.Orbital.Equals(add.Orbital))
                If IsNothing(ec) Then
                    ReDim Preserve e(e.Count)
                    e(e.Count - 1) = add
                Else
                    ec.ele_ += add.ele_
                End If
                Return e
            End Function
        End Class

        Private Shared Function GetElConfig(ByVal electrons As Integer) As ElectronConfig()
            'Elements with anomalies
            Select Case electrons
                Case 24
                    Return ElectronConfig.AddOrbital(GetElConfig(19), New ElectronConfig("3d", 5))
                Case 29
                    Return ElectronConfig.AddOrbital(GetElConfig(19), New ElectronConfig("3d", 10))
                Case 41
                    Return ElectronConfig.AddOrbital(GetElConfig(37), New ElectronConfig("4d", 4))
                Case 42
                    Return ElectronConfig.AddOrbital(GetElConfig(37), New ElectronConfig("4d", 5))
                Case 44
                    Return ElectronConfig.AddOrbital(GetElConfig(37), New ElectronConfig("4d", 7))
                Case 45
                    Return ElectronConfig.AddOrbital(GetElConfig(37), New ElectronConfig("4d", 8))
                Case 46
                    Return ElectronConfig.AddOrbital(GetElConfig(36), New ElectronConfig("4d", 10))
                Case 47
                    Return ElectronConfig.AddOrbital(GetElConfig(37), New ElectronConfig("4d", 10))
                Case 57
                    Return ElectronConfig.AddOrbital(GetElConfig(56), New ElectronConfig("5d", 1))
                Case 58
                    Return ElectronConfig.AddOrbital(ElectronConfig.AddOrbital(GetElConfig(56), New ElectronConfig("4f", 1)), New ElectronConfig("5d", 1))
                Case 64
                    Return ElectronConfig.AddOrbital(GetElConfig(63), New ElectronConfig("5d", 1))
                Case 78
                    Return ElectronConfig.AddOrbital(ElectronConfig.AddOrbital(GetElConfig(55), New ElectronConfig("4f", 14)), New ElectronConfig("5d", 9))
                Case 79
                    Return ElectronConfig.AddOrbital(ElectronConfig.AddOrbital(GetElConfig(55), New ElectronConfig("4f", 14)), New ElectronConfig("5d", 10))
                Case 89
                    Return ElectronConfig.AddOrbital(GetElConfig(88), New ElectronConfig("6d", 1))
                Case 90
                    Return ElectronConfig.AddOrbital(GetElConfig(88), New ElectronConfig("6d", 2))
                Case 91
                    Return ElectronConfig.AddOrbital(ElectronConfig.AddOrbital(GetElConfig(88), New ElectronConfig("5f", 2)), New ElectronConfig("6d", 1))
                Case 92
                    Return ElectronConfig.AddOrbital(ElectronConfig.AddOrbital(GetElConfig(88), New ElectronConfig("5f", 3)), New ElectronConfig("6d", 1))
                Case 93
                    Return ElectronConfig.AddOrbital(ElectronConfig.AddOrbital(GetElConfig(88), New ElectronConfig("5f", 4)), New ElectronConfig("6d", 1))
                Case 96
                    Return ElectronConfig.AddOrbital(GetElConfig(95), New ElectronConfig("6d", 1))
                Case Else
                    'Elements without anomalies
                    Dim ell As New List(Of ElectronConfig)
                    Dim orbital As Integer = 0
                    While electrons > 0
                        Dim o As Orbital = Orbitals(orbital)
                        Dim take As Integer
                        If electrons < o.Capacity Then take = electrons Else take = o.Capacity
                        ell.Add(New ElectronConfig(o, take))
                        electrons -= take
                        orbital += 1
                    End While
                    Return ell.ToArray()
            End Select
        End Function

        Public Function GetElectronConfigurationByShell() As Integer()
            Dim ell As New List(Of Integer)
            Dim electrons As Integer = numb
            For Each ec In elconfig
                If ell.Count < ec.Orbital.Shell Then ell.Add(0)
                ell(ec.Orbital.Shell - 1) += ec.NumberOfElectrons
            Next
            Return ell.ToArray
        End Function

        Public Function GetElectronConfigurationByShellString() As String
            Dim config() As Integer = GetElectronConfigurationByShell()
            Dim str As String = String.Empty
            For n As Integer = 0 To config.Count - 1
                If config(n) = 0 Then Exit For
                If n = 0 Then str = config(n) Else str = str & "-" & config(n)
            Next
            Return str
        End Function

        Private Structure GroupStrings
            Public Const Actinides As String = "Actinides"
            Public Const AlkaliMetals As String = "Alkali metals"
            Public Const AlkalineEarthMetals As String = "Alkaline earth metals"
            Public Const TransitionMetals As String = "Transition metals"
            Public Const Metalloids As String = "Metalloids"
            Public Const NonMetals As String = "Non-metals"
            Public Const Post_transitionMetals As String = "Post-transition metals"
            Public Const Halogens As String = "Halogens"
            Public Const NobleGases As String = "Noble gases"
            Public Const Lanthanides As String = "Lanthanides"
            Public Const Unknown As String = "Unknown"
        End Structure

        Public Shared Sub UpdateLabStates()
            For Each e In ElementList
                e.TryUpdateLabState()
            Next
        End Sub

    End Class

    Public Class Compound
        Inherits Chemical

        Public Shared CompoundList As New List(Of Compound)

        Public ReadOnly Property IsOrganic As Boolean
            Get
                Return formula_.IsOrganic(name_)
            End Get
        End Property

        Public ReadOnly Property MolarMass As Double
            Get
                Return GetMolarMass()
            End Get
        End Property

        Public ReadOnly Property Solubility As String
            Get
                Return compoundSolubility
            End Get
        End Property
        Private compoundSolubility As String

        Private Sub New(ByVal name As String, ByVal formula As CompoundFormula, ByVal appearance As String, ByVal density As Single,
                        ByVal melt As Single, ByVal boil As Single, ByVal solubility As String, ByVal state As StateOfMatter)
            name_ = name
            formula_ = formula
            appearance_ = appearance.Replace(Constants.NewLine, vbNewLine)
            boil_ = boil
            melt_ = melt
            dens_ = density
            compoundSolubility = solubility.Replace(Constants.NewLine, vbNewLine).Replace(Constants.Degree, Constants.DegreeReal)
            state_ = state
        End Sub

        Public Shared Sub Initialize()
            CompoundList.Clear()
            Using sr As New IO.StringReader(My.Resources.InorganicCompoundsResx)
                While sr.Peek > -1
                    If sr.ReadLine().Equals(String.Concat(Constants.TextFileStart, Constants.Column)) Then
                        While sr.Peek > -1
                            Dim name As String = sr.ReadLine()

                            ' 0=formula 1=appearance 2=density 3=meltingPoint 4=boilingPoint 5=solubility
                            Dim raw(5) As String
                            For i As Integer = 0 To raw.Count - 1
                                raw(i) = sr.ReadLine().Remove(0, 1)
                            Next

                            Dim dens As Double
                            Dim melt As Double
                            Dim boil As Double
                            If raw(1).Equals(Constants.NoInformation) Then raw(1) = String.Empty
                            If raw(2).Equals(Constants.NoInformation) Then
                                raw(2) = Nothing
                                dens = -1
                            Else
                                dens = ToDouble(raw(2))
                            End If
                            If raw(3).Equals(Constants.NoInformation) Then
                                raw(3) = Nothing
                                melt = -1
                            Else
                                melt = ToDouble(raw(3))
                            End If
                            If raw(4).Equals(Constants.NoInformation) Then
                                raw(4) = Nothing
                                boil = -1
                            Else
                                boil = ToDouble(raw(4))
                            End If
                            If raw(5).Equals(Constants.NoInformation) Then raw(5) = String.Empty Else raw(5) = raw(5).Replace(Constants.Degree, "°")
                            Dim state As StateOfMatter
                            If IsNothing(raw(3)) Then
                                state = StateOfMatter.Solid
                            Else
                                If melt < ChemistryConstants.RoomTemperature Then
                                    If IsNothing(raw(4)) Then
                                        state = StateOfMatter.Liquid
                                    Else
                                        If boil < ChemistryConstants.RoomTemperature Then
                                            state = StateOfMatter.Gas
                                        Else
                                            state = StateOfMatter.Liquid
                                        End If
                                    End If
                                Else
                                    state = StateOfMatter.Solid
                                End If

                            End If
                            Dim c As New Compound(name, New CompoundFormula(raw(0)), raw(1), dens, melt, boil, raw(5), state)
                            CompoundList.Add(c)
                        End While
                    End If
                End While
            End Using
            CompoundList.Sort(New Comparison(Of Compound)(AddressOf CompoundSorter))
        End Sub

        Private Shared Function CompoundSorter(a As Compound, b As Compound) As Integer
            Return a.Name.CompareTo(b.Name)
        End Function

        Public Shared Function FromName(ByVal name As String) As Compound
            Return CompoundList.Find(Function(x As Compound) x.Name = name)
        End Function

        Private Function GetMolarMass() As Double
            Return formula_.GetMolarMass()
        End Function

        Public Shared Function GetAllOfType(ByVal organic As Boolean) As List(Of Compound)
            Return CompoundList.FindAll(Function(x As Compound) x.IsOrganic = organic)
        End Function

        Public Shared Sub UpdateLabStates()
            For Each c In CompoundList
                c.TryUpdateLabState()
            Next
        End Sub

    End Class

    Public Class Reaction

        Public Shared ReactionList As New List(Of Reaction)

        Public Enum ReactionStatus
            Unreacreatable
            Recreatable
        End Enum

        Public Enum ReactionSides
            Reactants
            Products
            All
        End Enum

        Public Enum ReactionType
            Synthesis
            Decomposition
            Other
        End Enum

        Public ReadOnly Property Reactants As List(Of String)
            Get
                Return ReactionReactants
            End Get
        End Property
        Private ReactionReactants As List(Of String)

        Public ReadOnly Property Products As List(Of String)
            Get
                Return ReactionProducts
            End Get
        End Property
        Private ReactionProducts As List(Of String)

        Public ReadOnly Property ReactantCoeficients As List(Of Integer)
            Get
                Return ReactionReactantCoef
            End Get
        End Property
        Private ReactionReactantCoef As List(Of Integer)

        Public ReadOnly Property ProductCoeficients As List(Of Integer)
            Get
                Return ReactionProductCoef
            End Get
        End Property
        Private ReactionProductCoef As List(Of Integer)

        Public ReadOnly Property Comment As String
            Get
                Return ReactionComment
            End Get
        End Property
        Private ReactionComment As String

        Public ReadOnly Property IsCommented As Boolean
            Get
                Return Not (ReactionComment = String.Empty)
            End Get
        End Property

        Public ReadOnly Property HasTemperatureSpan As Boolean
            Get
                Dim b As Boolean = ((TempSpan.GetValue(0) <= 0) And (TempSpan.GetValue(1) <= 0))
                Return Not b
            End Get
        End Property

        Public ReadOnly Property TemperatureSpan As Double()
            Get
                Dim a(TempSpan.Length - 1) As Double
                TempSpan.CopyTo(a, 0)
                Return a
            End Get
        End Property
        Private TempSpan(1) As Double

        Public ReadOnly Property IsReversible As Boolean
            Get
                Return rev
            End Get
        End Property
        Private rev As Boolean = False

        Public ReadOnly Property IsElectrolytic As Boolean
            Get
                If IsNothing(Comment) Then
                    Return False
                Else
                    Return Comment.ToLower.Contains(Constants.CommentElectrolysis)
                End If
            End Get
        End Property

        Public ReadOnly Property Status As ReactionStatus
            Get
                Return state
            End Get
        End Property
        Private state As ReactionStatus

        Public ReadOnly Property [Type] As ReactionType
            Get
                Return typ
            End Get
        End Property
        Private typ As ReactionType

        Private react As String

        Private Sub New(ByVal reactionString As String, Optional ByVal reactionComment As String = Nothing)
            ReactionReactants = New List(Of String)
            ReactionProducts = New List(Of String)
            ReactionReactantCoef = New List(Of Integer)
            ReactionProductCoef = New List(Of Integer)
            Dim splitarray() As Char = Constants.ReactionInteractionChar.ToString.ToCharArray
            If IsNothing(reactionComment) Then
                Me.ReactionComment = String.Empty
            Else
                Me.ReactionComment = reactionComment.Replace(Constants.Degree, Constants.DegreeReal)
            End If
            Dim str As String = reactionString
            react = str
            If str.Contains(Constants.ReactionReversible) Then
                str = str.Replace(Constants.ReactionReversible, Constants.ReactionDirection)
                rev = True
            End If
            Dim left As String = str.Remove(str.IndexOf(Constants.ReactionDirection))
            Dim right As String = str.Remove(0, str.IndexOf(Constants.ReactionDirection) + 1)
            If left.Contains(Constants.ReactionInteractionChar) Then
                For Each item In left.Split(splitarray, StringSplitOptions.RemoveEmptyEntries)
                    item = item.Trim()
                    If item.Length = 0 Then Continue For
                    If Char.IsNumber(item.ToCharArray().First) Then
                        Dim valuu As Integer = StrVal(item)
                        ReactionReactants.Add(item.Remove(0, valuu.ToString.Length))
                        ReactionReactantCoef.Add(valuu)
                    Else
                        ReactionReactants.Add(item)
                        ReactionReactantCoef.Add(1)
                    End If
                Next
            Else
                left = left.Replace(Space(1), String.Empty)
                If Char.IsNumber(left.ToCharArray().First) Then
                    Dim valuu As Integer = StrVal(left)
                    ReactionReactants.Add(left.Remove(0, valuu.ToString.Length))
                    ReactionReactantCoef.Add(valuu)
                Else
                    ReactionReactants.Add(left)
                    ReactionReactantCoef.Add(1)
                End If
            End If

            If right.Contains(Constants.ReactionInteractionChar) Then
                For Each item In right.Split(splitarray, StringSplitOptions.RemoveEmptyEntries)
                    item = item.Trim()
                    If item.Length = 0 Then Continue For
                    If Char.IsNumber(item.ToCharArray().First) Then
                        Dim valuu As Integer = StrVal(item)
                        ReactionProducts.Add(item.Remove(0, valuu.ToString.Length))
                        ReactionProductCoef.Add(valuu)
                    Else
                        ReactionProducts.Add(item)
                        ReactionProductCoef.Add(1)
                    End If
                Next
            Else
                right = right.Replace(Space(1), String.Empty)
                If Char.IsNumber(right.ToCharArray().First) Then
                    Dim valuu As Integer = StrVal(right)
                    ReactionProducts.Add(right.Remove(0, valuu.ToString.Length))
                    ReactionProductCoef.Add(valuu)
                Else
                    ReactionProducts.Add(right)
                    ReactionProductCoef.Add(1)
                End If
            End If

            If ReactionProductCoef.Count = 1 Then
                typ = ReactionType.Synthesis
            Else
                If ReactionReactantCoef.Count = 1 Then
                    typ = ReactionType.Decomposition
                Else
                    typ = ReactionType.Other
                End If
            End If
            TempSpan = GetTempSpan()
        End Sub

        Public Shared Sub Initialize()
            ReactionList.Clear()
            Using sr As New IO.StringReader(My.Resources.ReactionsResx)
                Dim lastLn As String = String.Empty
                While sr.Peek > -1
                    Dim thisLn As String = sr.ReadLine()
                    If lastLn.Length > 0 Then
                        If thisLn.StartsWith(vbTab) Then
                            ReactionList.Add(New Reaction(lastLn, thisLn.Substring(1)))
                        Else
                            If Not lastLn.StartsWith(vbTab) Then ReactionList.Add(New Reaction(lastLn))
                        End If
                    End If
                    lastLn = thisLn
                End While
            End Using
        End Sub

        Public Overrides Function ToString() As String
            Return react
        End Function

        Public Function Contains(ByVal compoundf As String, Optional ByVal Wheretolook As ReactionSides = ReactionSides.All) As Boolean
            Select Case Wheretolook
                Case Is = ReactionSides.Reactants
                    Return ReactionReactants.Contains(compoundf)
                Case Is = ReactionSides.Products
                    Return ReactionProducts.Contains(compoundf)
                Case Else
                    Return GetAllCompoundStrings.Contains(compoundf)
            End Select
        End Function

        Public Function GetAllCompoundStrings() As List(Of String)
            Dim rt As New List(Of String)
            For Each i In ReactionReactants
                rt.Add(i.ToString)
            Next
            For Each i In ReactionProducts
                rt.Add(i.ToString)
            Next
            Return rt
        End Function

        Public Shared Function GetAllOf(ByVal SubstanceFormula As String, Optional ByVal whereToLook As ReactionSides = ReactionSides.All) As List(Of Reaction)
            Return ReactionList.FindAll(Function(x As Reaction) x.Contains(SubstanceFormula, whereToLook))
        End Function

        Public Shared Function GetAllStatus(ByVal targetStatus As ReactionStatus) As List(Of Reaction)
            Return ReactionList.FindAll(Function(x As Reaction) x.Status = targetStatus)
        End Function

        Public Shared Function GetAllTypes(ByVal targetType As ReactionType)
            Return ReactionList.FindAll(Function(x As Reaction) x.Type = targetType)
        End Function

        Public Shared Function GetAllReversible() As List(Of Reaction)
            Return ReactionList.FindAll(Function(x As Reaction) x.IsReversible)
        End Function

        Public Shared Function GetAllElectrolytic() As List(Of Reaction)
            Return ReactionList.FindAll(Function(x As Reaction) x.Comment.Contains(Constants.CommentElectrolysis))
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            If obj.GetType.Equals(GetType(Reaction)) Then
                Dim r1 As Reaction = obj
                If r1.IsReversible And Me.IsReversible Then
                    Dim difReac As Integer = r1.Reactants.Count - Me.Reactants.Count
                    Dim difReacPro As Integer = r1.Reactants.Count - Me.Products.Count
                    If difReac <> 0 And difReacPro <> 0 Then

                    End If
                Else
                    Return False
                End If
                If Not r1.Reactants.Count = Me.Reactants.Count Then Return False
                If Not r1.Products.Count = Me.Products.Count Then Return False
                For Each re1 In r1.Reactants
                    If Not Me.Reactants.Contains(re1) Then Return False
                Next
                For Each pr1 In r1.Products
                    If Not Me.Products.Contains(pr1) Then Return False
                Next
                Return True
            Else
                Return MyBase.Equals(obj)
            End If
        End Function

        Private Function GetTempSpan() As Double()
            Dim r(1) As Double
            If IsCommented Then
                Dim c As String = ReactionComment
                Dim n As Double = StrVal(c, Nothing)
                If Not IsNothing(n) Then n = Math.Round(n, 2)
                If IsNothing(n) Then
                    r.SetValue(0, 0)
                    r.SetValue(0, 1)
                Else
                    n = Constants.FromKelvinToCelsius(Math.Round(n, 2), True)
                    If c.StartsWith(Constants.CommentTempStartMore) Then
                        r.SetValue(n, 0)
                        r.SetValue(0, 1)
                    Else
                        If c.StartsWith(Constants.CommentTempStart) Then
                            r.SetValue(n, 0)
                            r.SetValue(n, 1)
                        Else
                            If c.StartsWith(Constants.CommentTempStartBetween) Then
                                Dim n2 As Double = Constants.FromKelvinToCelsius(StrVal(c.Remove(0, c.IndexOf(Constants.CommentTempSeperator) + 1)), True)
                                r.SetValue(n, 0)
                                r.SetValue(n2, 1)
                            Else
                                If c.StartsWith(Constants.CommentTempStartLess) Then
                                    r.SetValue(0, 0)
                                    r.SetValue(n, 1)
                                Else
                                    If c.Contains(Constants.CommentElectrolysis) And ReactionReactants.Count = 1 Then
                                        Dim substance As Compound = Compound.FromName(CompoundFormula.NameOf(ReactionReactants.First))
                                        r.SetValue(substance.MeltingPoint, 0)
                                        r.SetValue(substance.BoilingPoint, 1)
                                    Else
                                        r.SetValue(0, 0)
                                        r.SetValue(0, 1)
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
            Return r
        End Function

        Public Shared Sub UpdateRecreatable()
            For Each r In ReactionList
                Dim b As Boolean = Not IsNothing(Info.LoadedLab)
                Dim i As Integer = 0
                While b And i < r.Reactants.Count()
                    b = Info.LoadedLab.IsAvailable(r.Reactants.ElementAt(i))
                    i += 1
                End While
                If b Then
                    r.state = ReactionStatus.Recreatable
                Else
                    r.state = ReactionStatus.Unreacreatable
                End If
            Next
            Element.UpdateLabStates()
            Compound.UpdateLabStates()
        End Sub

        Public Shared Function GetReactionsOf(ByVal e As Element)
            Return ReactionList.FindAll(Function(x As Reaction) x.Contains(e.Formula.ToString(), ReactionSides.All))
        End Function

        Public Shared Function GetReactionsOf(ByVal c As Compound)
            Return ReactionList.FindAll(Function(x As Reaction) x.Contains(c.Formula.ToString(), ReactionSides.All))
        End Function

    End Class

    Public Class CompoundFormula

        Public ReadOnly Property Elements As List(Of String)
            Get
                Return els
            End Get
        End Property
        Public ReadOnly Property ElementNumbers As List(Of Byte)
            Get
                Return eln
            End Get
        End Property
        Private eln As List(Of Byte)
        Private els As List(Of String)
        Private formula_ As String

        Public ReadOnly Property IsElement As Boolean
            Get
                Return (els.Count = 1)
            End Get
        End Property

        Public Class RawStructure

            Public Enum Types
                Element
                Number
                OpeningBracket
                ClosingBracket
            End Enum

            Public ReadOnly Property Type As Types
                Get
                    Return val2
                End Get
            End Property
            Private val2 As Types
            Public ReadOnly Property Value As String
                Get
                    Return val
                End Get
            End Property
            Private val As String

            Public Sub New(ByVal RawString As String)
                val = RawString
                Select Case val
                    Case Is = "("
                        val2 = Types.OpeningBracket
                    Case Is = ")"
                        val2 = Types.ClosingBracket
                    Case Else
                        If IsNumeric(val) Then
                            val2 = Types.Number
                        Else
                            val2 = Types.Element
                        End If
                End Select
            End Sub

        End Class

        Public ReadOnly Property Raw As List(Of RawStructure)
            Get
                Return GetRaw()
            End Get
        End Property

        Sub New(ByVal formula As String)
            Me.formula_ = formula
            Dim str As String = formula
            'Loop 1
            While New String(str).Contains("(")
                Dim starti As Integer = str.LastIndexOf("(")
                Dim endi As Integer = 0
                If str.Contains(")") Then
                    endi = str.Remove(0, starti + 1).IndexOf(")") + starti + 1
                    If str.Length - 1 = endi Then
                        str = str.Remove(endi, 1).Remove(starti, 1)
                    Else
                        If Char.IsNumber(str.ToCharArray()(endi + 1)) Then
                            Dim valuu As String = StrVal(str.Remove(0, endi + 1))
                            str = str.Remove(starti) & Factorize(str.Remove(endi).Remove(0, starti + 1), valuu) &
                                str.Remove(0, endi + valuu.ToString.Length + 1)
                        Else
                            str = str.Remove(endi, 1).Remove(starti, 1)
                        End If
                    End If
                Else
                    str = str.Remove(starti, 1)
                End If
            End While
            'List of elements
            Me.els = New List(Of String)
            Me.eln = New List(Of Byte)
            'Loop 2
            While New String(str).Length > 0
                If str.Length = 1 Then
                    els.Add(str)
                    eln.Add(1)
                    str = String.Empty
                Else
                    Dim c() As Char = str.ToCharArray
                    If Char.IsNumber(c(1)) Then
                        Dim valu As Integer = StrVal(str)
                        If valu = 0 Then valu = 1
                        els.Add(c(0))
                        eln.Add(valu)
                        str = str.Remove(0, 1 + valu.ToString.Length)
                        Continue While
                    End If
                    If Char.IsUpper(c(1)) Then
                        els.Add(str.Remove(1))
                        eln.Add(1)
                        str = str.Remove(0, 1)
                        Continue While
                    End If
                    If Char.IsLower(c(1)) Then
                        If str.Length < 3 Then
                            Dim valu As Integer = StrVal(str)
                            If valu = 0 Then valu = 1
                            els.Add(str.Replace(valu, String.Empty))
                            eln.Add(valu)
                            str = String.Empty
                        Else
                            If Char.IsNumber(c(2)) Then
                                Dim valu As Integer = StrVal(str)
                                If valu = 0 Then valu = 1
                                els.Add(str.Remove(2))
                                eln.Add(valu)
                                str = str.Remove(0, 2 + valu.ToString.Length)
                            Else
                                els.Add(str.Remove(2))
                                eln.Add(1)
                                str = str.Remove(0, 2)
                            End If
                        End If
                    End If
                End If
            End While
        End Sub

        Sub New(ByVal elements As List(Of String), ByVal numbers As List(Of Byte))
            Me.els = elements
            Me.eln = numbers
            Me.formula_ = String.Empty
            For i As Integer = 0 To elements.Count - 1
                formula_ = formula_ & elements(i) & numbers(i)
            Next
        End Sub

        Public Function GetMolarMass() As Double
            Dim d As Double = 0
            For i As Integer = 0 To els.Count - 1
                d += Element.GetElementFromSymbol(els(i)).AtomicMass * eln(i)
            Next
            Return d
        End Function

        Private Function Factorize(ByVal s As String, ByVal factor As Integer) As String
            Dim rets As String = String.Empty
            Dim ca() As Char = s.ToCharArray
            For i As Integer = 0 To ca.Count - 1
                If Char.IsNumber(ca(i)) Then Continue For
                If Char.IsUpper(ca(i)) Then
                    If i + 1 = ca.Count Then
                        rets = rets & ca(i) & factor
                        Continue For
                    End If
                    If Char.IsNumber(ca(i + 1)) Then
                        rets = rets & ca(i) & (StrVal(s.Remove(0, i + 1)) * factor)
                        Continue For
                    End If
                    If Char.IsUpper(ca(i + 1)) Then
                        rets = rets & ca(i) & factor
                        Continue For
                    End If
                End If
                If Char.IsLower(ca(i)) Then
                    If i + 1 = ca.Count Then
                        rets = rets & s.Remove(0, i - 1) & factor
                        Continue For
                    End If
                    If Char.IsNumber(ca(i + 1)) Then
                        rets = rets & s.Remove(i + 1).Remove(0, i - 1) & (StrVal(s.Remove(0, i + 1)) * factor)
                        Continue For
                    End If
                End If
            Next
            Return rets
        End Function

        Private Function GetRaw() As List(Of RawStructure)
            Dim rs As New List(Of RawStructure)
            Dim str1 As String = formula_
            Dim ca() As Char = str1.ToCharArray
            While New String(str1).Length > 0
                ca = str1.ToCharArray
                If Char.IsNumber(ca(0)) Then
                    Dim vl As Integer = StrVal(str1)
                    str1 = str1.Remove(0, vl.ToString.Length)
                    rs.Add(New RawStructure(vl))
                    Continue While
                End If
                If ca(0) = "(" Or ca(0) = ")" Then
                    str1 = str1.Remove(0, 1)
                    rs.Add(New RawStructure(ca(0)))
                    Continue While
                End If
                If Char.IsUpper(ca(0)) Then
                    If ca.Count > 1 Then
                        If Char.IsLower(ca(1)) Then
                            str1 = str1.Remove(0, 2)
                            rs.Add(New RawStructure(ca(0) & ca(1)))
                        Else
                            str1 = str1.Remove(0, 1)
                            rs.Add(New RawStructure(ca(0)))
                        End If
                    Else
                        str1 = String.Empty
                        rs.Add(New RawStructure(ca(0)))
                    End If
                End If
            End While
            Return rs
        End Function

        Public Overrides Function ToString() As String
            Return formula_
        End Function

        Public Shared Function [NameOf](ByVal CompoundFormula As String) As String
            Dim cf As New CompoundFormula(CompoundFormula)
            If cf.IsElement Then
                Return Element.GetElementFromSymbol(cf.els.First()).Name
            Else
                Return Compound.CompoundList.Find(Function(x As Compound) x.Formula.Equals(cf)).Name
            End If
        End Function

        Public Function IsOrganic(ByVal name As String) As Boolean
            If els.Count > 1 Then
                Dim b As Boolean = False
                For Each e In els
                    Dim el As Element = Element.GetElementFromSymbol(e)
                    If el.AtomicNumber = 6 Then
                        b = True
                    End If
                    If el.Group = Element.Groups.AlkaliMetals Or el.Group = Element.Groups.AlkalineEarthMetals _
                        Or el.Group = Element.Groups.Lanthanides Or el.Group = Element.Groups.Metalloids Or
                        el.Group = Element.Groups.TransitionMetals Or el.Group = Element.Groups.Post_transitionMetals Then
                        b = False
                        Exit For
                    End If
                Next
                If b Then
                    Dim n As String = name.ToLower
                    If els.Count = 2 Then
                        If els(0).Equals("O") Or els(1).Equals("O") Then Return False
                    End If
                    If n.Contains(Constants.CarbonateString.ToLower) Then Return False
                    If n.Contains(Constants.CyanideString.ToLower) Then Return False
                    If n.Contains(Constants.OxideString.ToLower) Then Return False
                    If n.Contains(Constants.CarbideString.ToLower) Then Return False
                    If n.Contains(Constants.CyanateString.ToLower) Then Return False
                    If n.Contains(Constants.ThyocyanateString.ToLower) Then Return False
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End Function

        Public Overloads Function Equals(ByVal cf As CompoundFormula) As Boolean
            Return Me.ToString().Equals(cf.ToString())
        End Function

    End Class

    Public Module Info

        Public ReadOnly Property LoadedLab As Laboratory
            Get
                Return ldLab
            End Get
        End Property
        Private ldLab As Laboratory

        Public Event LaboratoryLoaded As EventHandler(Of EventArgs)
        Public Event LaboratoryUnloaded As EventHandler(Of EventArgs)

        Private nfi As New NumberFormatInfo

        Public Sub InitializeEnvironment()
            nfi.NumberDecimalSeparator = Constants.Period

            Element.Initialize()
            Compound.Initialize()
            Reaction.Initialize()
        End Sub

        Public Sub LoadLab(ByVal lab As Laboratory)
            ldLab = lab
            RaiseEvent LaboratoryLoaded(lab, EventArgs.Empty)
            Reaction.UpdateRecreatable()
        End Sub

        Public Sub UnloadLab()
            ldLab = Nothing
            RaiseEvent LaboratoryUnloaded(Nothing, EventArgs.Empty)
            Reaction.UpdateRecreatable()
        End Sub

        Public Function ToDouble(ByVal s As String) As Double
            If s.StartsWith(Constants.Period) AndAlso s.Length > 1 Then s = String.Concat("0", s)
            Return Double.Parse(s, NumberStyles.AllowDecimalPoint, nfi)
            Return Double.Parse(s, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo)
        End Function

        Public Function ToStringDecimal(ByVal d As Decimal, ByVal Optional decimals As Integer = -1) As String
            If decimals >= 0 Then d = Decimal.Round(d, decimals)
            Return d.ToString(nfi)
        End Function

    End Module

End Namespace

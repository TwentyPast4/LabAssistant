Module Constants

#Region "Graphics"
    Public Const DefaultBorderWidth As Integer = 4
    Public Const DefaultElementTileSize As Integer = 332
    Public Const MarginTop1 As Integer = 17
    Public Const MMassMarginLeft As Integer = 18
    Public Const MMassSize As Integer = 42
    Public Const CenterSize As Integer = 270
    Public Const atomicnumberMarginTop As Integer = 227
    Public Const atomicnumberMarginLeft As Integer = 5
    Public Const atomicnumberSize As Integer = 60
    Public Const electronconfigMarginTop As Integer = 275
    Public Const electronconfigMarginLeft As Integer = 10
    Public Const electronconfigSize As Integer = 40
    Public Const oxyMarginRight As Integer = 7
    Public Const oxySize As Integer = 35
    Public Const oxyMarginLeft As Integer = 270
    Public Const centerDrop As Integer = 28
    Public Const centerMarginLeft As Integer = 15
    Public Const OpeningBracketDeflection As Integer = -20
    Public Const ClosingBracketDeflection As Integer = -25
    Public Const OpeningBracket As String = "("
    Public Const Comma As String = ","
    Public Const Period As String = "."
    Public Const Half As Decimal = 0.5
    Public Const OxySizeFactor As Decimal = 0.75

    Public Const TopMargin As Integer = 60
    Public Const SidesMargin As Integer = 20
    Public Const NeckHeight As Integer = 180
    Public Const h1 As Integer = 432
    Public Const h2 As Integer = 112
    Public Const r1 As Integer = 270
    Public Const r2 As Integer = 70
    Public VolumeLow As Integer = Math.PI * (r1 ^ 2 * h1 - r2 ^ 2 * h2) / 3
    Public VolumeHigh As Integer = Math.PI * r2 ^ 2 * NeckHeight
    Public BorderKoeficient As Decimal = Math.Round(VolumeLow / (VolumeHigh + VolumeLow), 4, MidpointRounding.ToEven)
    Public BubbleRadius As Integer() = {10, 17, 15, 7}
    Public BubbleOffset As Point() = {New Point(30, 50), New Point(-30, 20), New Point(15, -20), New Point(-25, -50)}
    Public Const MinimumBubbleHeight As Integer = 100
    Public FillRegion As Point() = {New Point(20, 566), New Point(8, 542), New Point(210, 240), New Point(210, 60), New Point(365, 60), _
                                    New Point(365, 240), New Point(572, 542), New Point(560, 566)}

    Public Const SymbolMargin As Decimal = 0.125

    Public AniBorderCl As Color = Colors.LightSeaGreen
    Public AniFillCl As Color = Colors.LightSeaGreen

#End Region

#Region "Periodic Table"
    Public Const InfoAtomicWeight As String = "Atomic Weight"
    Public Const InfoAtomicNumber As String = "Atomic Number"
    Public Const InfoOxyStates As String = "Common Oxidation States"
    Public Const InfoElConfig As String = "Electron Configuration"
    Public Const InfoSymbol As String = "Symbol"
    Public Const AppearanceGroupName As String = "Element Appearance"
    Public Const TableKeyGroupName As String = "Table Key"
    Public Const ForeColorFormat As String = "{0}ForeColor"
    Public Const PeriodicTableFormat As String = "TableElement{0}"
    Public Const PeriodicTableInfoFormat As String = "TableInfo{0}"
    Public Const PeriodicTableInfoStart As String = "TableInfo"
    Public Const KeyName As String = "LegendPicture"
    Public Const LanthanidesNumberString As String = "57 - 71"
    Public Const LanthanidesTitleSize As Integer = 95
    Public Const LanthanidesRowCount As Integer = 15
    Public Const ElementsRowCount As Integer = 18
    Public Const MarginMinimum As Integer = 3
    Public Const FrameWidth As Integer = 9
    Public Const TransitionMetals As Integer = 11
    Public Const JumpToHelium As Integer = 17
    Public Const ElementsColumnCount As Integer = 6
    Public Const InfoLineWidth As Integer = 5
    Public Const Displacement As Integer = 100
    Public Const InfoFontSize As Integer = 55
    Public Const FrameKeyWidth As Integer = 7
    Public AtomicWeightPoint As Point = New Point(20, 37)
    Public AtomicNumberPoint As Point = New Point(10, 258)
    Public SymbolPoint As Point = New Point(300, 158)
    Public OxidationStatesPoint As Point = New Point(328, 40)
    Public ElectronConfigPoint As Point = New Point(180, 295)
    Public SolidForeColor As Color = Color.FromRgb(32, 32, 32)
    Public LiquidForeColor As Color = Colors.Teal
    Public GasForeColor As Color = Colors.DimGray
    Public AlkaliMetalsFrame As Color = Color.FromRgb(165, 99, 209)
    Public AlkaliMetalsBack As Color = Color.FromRgb(228, 186, 255)
    Public AlkalineEarthMetalsFrame As Color = Color.FromRgb(208, 100, 212)
    Public AlkalineEarthMetalsBack As Color = Color.FromRgb(254, 193, 255)
    Public MetalloidsFrame As Color = Color.FromRgb(39, 219, 117)
    Public MetalloidsBack As Color = Color.FromRgb(164, 255, 204)
    Public TransitionMetalsFrame As Color = Color.FromRgb(77, 198, 242)
    Public TransitionMetalsBack As Color = Color.FromRgb(166, 232, 255)
    Public LanthanidesFrame As Color = Color.FromRgb(100, 238, 210)
    Public LanthanidesBack As Color = Color.FromRgb(205, 255, 246)
    Public NonMetalsFrame As Color = Color.FromRgb(233, 222, 70)
    Public NonMetalsBack As Color = Color.FromRgb(255, 251, 178)
    Public Post_transitionMetalsFrame As Color = Color.FromRgb(230, 73, 220)
    Public Post_transitionMetalsBack As Color = Color.FromRgb(255, 174, 251)
    Public HalogensFrame As Color = Color.FromRgb(242, 174, 63)
    Public HalogensBack As Color = Color.FromRgb(255, 204, 122)
    Public NobleGasesFrame As Color = Color.FromRgb(235, 94, 94)
    Public NobleGasesBack As Color = Color.FromRgb(255, 150, 150)
    Public TableKeyColor As Color = Color.FromRgb(240, 240, 240)
    Public TableKeyBack As Color = Colors.Transparent
    Public TableKeyTextColor As Color = Color.FromRgb(32, 32, 32)
    Public TableKeyLineColor As Color = Color.FromRgb(32, 32, 32)
    Public TableKeyStatesBackColor As Color = Color.FromRgb(240, 240, 240)
    Public TableKeyElement As Integer = 1
    Public TableKeySolid As Integer = 26
    Public TableKeyLiquid As Integer = 35
    Public TableKeyGas As Integer = 1
#End Region

#Region "Reactions"
    Public CalculateByGlowColor As Color = Colors.White
    Public CalculateBySelectedColor As Color = Color.FromRgb(90, 90, 100)
    Public CalculateByStrongWarning As Color = Color.FromRgb(90, 70, 70)
    Public CalculateByMildWarning As Color = Color.FromRgb(90, 90, 70)
    Public Const CalculateByGlowAmount As Decimal = 0.35
    Public Const CalculateByGlowSpeed As Integer = 6
    Public Const CalculateByDimSpeed As Integer = 1
    Public Const CalculateByItemHeight As Integer = 30
    Public Const CoreNameFormat As String = "ButtonOf{0}"
    Public Const ResultNameFormat As String = "ResultOf{0}"
    Public CalculateByForeColor As Color = Colors.DeepSkyBlue
    Public CalculateByBorderColor As Color = Colors.DeepSkyBlue
    Public CalculateByProductsForeColor As Color = Colors.LimeGreen
    Public CalculateByProductsBorderColor As Color = Colors.LimeGreen
    Public CalculateByResultBackColor As Color = Color.FromRgb(105, 105, 105)
    Public Const ConversionCelsiusFormat As String = "{0} °C"
    Public Const ConversionFahrenheitFormat As String = "{0} °F"
    Public Const CalculateByFormulaFormats As String = "{0} ({1})"
    Public Const CalculateByMoleFormat As String = "{0} mole"
    Public Const CalculateByMoleMoreFormat As String = "{0} moles"
    Public Const CalculateByGramFormat As String = "{0} g"
    Public Const CalculateSymbolsSolid As Char = "s"
    Public Const CalculateSymbolsLiquid As Char = "l"
    Public Const CalculateSymbolsGas As Char = "g"
    Public Const CalculateSymbolsUnknown As Char = "?"
    Public Const CommentTempStart As String = "At "
    Public Const CommentTempStartMore As String = "Above "
    Public Const CommentTempStartLess As String = "Below "
    Public Const CommentTempStartBetween As String = "Between "
    Public Const CommentPercent As Char = "~"
    Public Const CommentTempLiquid As String = "molten "
    Public Const CommentMoreOf As String = "excess "
    Public Const CommentElectrolysis As String = "electrolysis"
    Public Const CommentElevated As String = "At elevated temperatures"
    Public Const CommentHeating As String = "heating"
    Public Const CommentTempSeperator As Char = "-"
    Public Const ReactionInteractionChar As Char = "+"
    Public Const SolutionConcentration As Char = "%"
    Public Const ReactionDirection As Char = ">"
    Public Const ReactionDirectionReverse As Char = "<"
    Public Const ReactionReversible As String = "<>"
    Public Const WaterMolecule As String = "H2O"
    Public Const WarningSolution As String = "This compound needs to be in a solution to react with water"
#End Region

#Region "Printing"
    Public LabPrintSize As Size = New Size(2481, 3507)
    Public Const PrintTitleSize As Integer = 55
    Public Const SubtitleSize As Integer = 40
    Public Const SubtitleMargin As Integer = 10
    Public PrintTitleColor As Color = Colors.Black
    Public SubtitleColor As Color = Colors.Gray
    Public Const TableStartMargin As Integer = 50
    Public Const TableBorderWidth As Integer = 4
    Public Const TableMeshWidth As Integer = 2
    Public TableBorderColor As Color = Color.FromRgb(32, 32, 32)
    Public TableMeshColor As Color = Colors.LightGray
    Public Const ColumnHeaderSize As Integer = 30
    Public ColumnHeaderColor As Color = Colors.DimGray
    Public Line1BackColor As Color = Color.FromRgb(250, 250, 250)
    Public Line2BackColor As Color = Color.FromRgb(245, 245, 245)
    Public Const EntrySize As Integer = 25
    Public EntryColor As Color = Color.FromRgb(64, 64, 64)
    Public Const NameWidth As Decimal = 0.25
    Public Const FormulaWidth As Decimal = 0.1
    Public Const AppearanceWidth As Decimal = 0.2
    Public Const DensityWidth As Decimal = 0.1
    Public Const StateWidth As Decimal = 0.07
    Public Const AmountWidth As Decimal = 0.08
    Public Const CommentsWidth As Decimal = 0.2

    Public Const PrintingTitle As String = "Inventory"
    Public Const PrintingSubtitle As String = "Lab Assistant"
    Public Const PrintingName As String = "Name"
    Public Const PrintingFormula As String = "Formula"
    Public Const PrintingAppearance As String = "Appearance"
    Public Const PrintingDensity As String = "Density"
    Public Const PrintingState As String = "State"
    Public Const PrintingAmount As String = "Amount"
    Public Const PrintingComments As String = "Comments"
#End Region

    Public Const MaxElements As Integer = 13
    Public Const MainFormFrameMaximizeButtonMargin As Integer = 4
    Public Const MainFormFrameExitButtonMargin As Integer = 3
    Public Const Lim1 As Integer = 4
    Public Const Lim2 As Integer = 6
    Public Const Lim3 As Integer = 7
    Public Const Lim4 As Integer = 8
    Public Const Lim5 As Integer = 9
    Public Const MinSize As Integer = 30
    Public Const Full130 As Integer = 130
    Public Const Full110 As Integer = 110
    Public Const Full43 As Integer = 43
    Public Const Full38 As Integer = 38
    Public Const Full35 As Integer = 35
    Public Const Neg15 As Integer = -15
    Public Const Neg10 As Integer = -10
    Public Const NumFontFactor As Decimal = 2 / 9
    Public Const SymbolBorder As Integer = 4
    Public Const SymbolGlow As Decimal = 0.4
    Public Const SymbolGlowSpeed As Integer = 8
    Public Const SymbolStep As Integer = 5
    Public Const SymbolDimSpeed As Integer = 1
    Public SymbolColor As Color = Colors.Gainsboro
    Public BracketColor As Color = Color.FromRgb(170, 170, 170)
    Public ReactionSubstanceOwnedColor As Color = Color.FromRgb(0, 150, 0)
    Public ReactionSubstanceCoreColor As Color = Colors.MediumTurquoise
    Public ReactionCoreProductBackColor As Color = Color.FromRgb(75, 80, 90)
    Public ReactionCommentColor As Color = Color.FromRgb(120, 120, 120)
    Public Const NumSizeFactor As Single = 1.25
    Public Const NumWidthFactor As Single = 0.75
    Public Const BracketSizeFactor As Single = 1.75
    Public Const BracketWidthFactor As Decimal = 5 / 13
    Public Const Margin3 As Integer = 3
    Public Const Margin6 As Integer = 6
    Public Const ToFahrenheitFactor As Single = 1.8
    Public Const ToFahrenheitAdd As Integer = 32
    Public Const GramKilograms As Integer = 10 ^ 3
    Public Const PoundKilograms As Decimal = 0.45359237
    Public Const OuncePounds As Integer = 16
    Public Const Unlimited As Integer = 10000
    Public Const TempFormat As String = "##0.00"
    Public Const KelvinFormat As String = "#0.## 'K'"
    Public Const CelsiusFormat As String = "#0.## '°C'"
    Public Const FahrenheitFormat As String = "#0.## '°F'"
    Public Const KilogramFormat As String = "#0.## 'kg'"
    Public Const GramFormat As String = "#0.## 'g'"
    Public Const PoundFormat As String = "#0.## 'lb'"
    Public Const OunceFormat As String = "#0.## 'oz'"
    Public Const DensityFormat As String = "#0.00#### 'g/cm³'"

    Public Const NoInformation As String = "[]"
    Public Const Degree As String = "[Deg]"
    Public Const DegreeReal As Char = "°"
    Public Const NewLine As String = "[n]"
    Public Const Splitter As Char = ";"

    Public Const Name As String = "[Name]"
    Public Const Formula As String = "[Formula]"
    Public Const Appearance As String = "[Appearance]"
    Public Const Density As String = "[Density]"
    Public Const MeltingPoint As String = "[Melting Point]"
    Public Const BoilingPoint As String = "[Boiling Point]"
    Public Const MolarMass As String = "[Molar Mass]"
    Public Const StateAtRT As String = "[State]"
    Public Const StateinLab As String = "[Status]"
    Public Const Amount As String = "[Amount]"
    Public Const Comments As String = "[Comment]"
    Public Const FormulaTab As String = "Formula:"
    Public Const CallByNamePanel As String = "SelectPanel"
    Public Const CallByNameType As String = "Select"
    Public Const PanelSuffix As String = "Panel"
    Public Const PanelButtonSuffix As String = "_Btn"
    Public Const DataButtonSuffix As String = "Btn"
    Public Const LibrarySuffix As String = "Lib"
    Public Const Library As String = "Library"
    Public Const Laboratory As String = "Laboratory"
    Public Const Reactions As String = "Reactions"
    Public Const OpenLabFilter As String = "Laboratory files (*.lab)|*.lab"
    Public Const LoadingText As String = "Loading library{0}"

#Region "Language"
    Public Const OpenLabTitle As String = "Select laboratory file"
    Public Const FileExistsPrompt As String = "File already exist. Would you like to overwrite it?"
    Public Const SubstanceExistsPrompt As String = "The substance you are trying to add({0}) is already in your laboratory." & _
        vbCrLf & "Would you like to:" & vbCrLf & "- Replace the old information with new information" & vbCrLf & _
        "- Add the new mass to the old mass" & vbCrLf & "- Skip"
    Public Const SubstanceExistsTitle As String = "Substance duplicate found"
    Public Const SubstanceExistsReplaceBtnText As String = "Replace"
    Public Const SubstanceExistsAddBtnText As String = "Add"
    Public Const SubstanceExistsCancelBtnText As String = "Cancel"
    Public Const LabResetSure As String = "Are you sure you want to revert all changes you've made?"
    Public Const AreYouSure As String = "Are you sure?"
    Public Const DefaultAvailableRemove As String = "This substance is available by default, do you still wish to remove it?"
    Public Const UnlimitedBtnText_U As String = "Unlimited"
    Public Const UnlimitedBtnText_L As String = "Limited"
    Public Const OverwriteFileTitle As String = "File already exists"
#End Region

    Public Const DumpFolder As String = "C:\Users\nabrd_000\Documents\Visual Studio 2013\Projects\Lab Assistant\Release\"
    Public Const BuildFormat As String = "{0} v.{1}.{2}b{3}"
    Public Const BuildFormatSuffix As String = "({0}).exe"
    Public Const GearMargin As Integer = 10
    Public Const EncryptionSize As Integer = 32
    Public Const LabFormat As String = "*.lab"
    Public Const EncryptionKey As String = "LabAssistantFile"
    Public Const TimerSpeed As Integer = 5

    Public Const TextFileStart As String = "Compounds"
    Public Const TextFileStartElement As String = "Elements"
    Public Const GlowGroupName As String = "Glow"

    Public Const DefaultTextNode As String = ".Text"
    Public Const Column As Char = ":"
    Public Const FormulaIndexSize As Integer = 8
    Public Const FormulaIndexOffset As Integer = -4
    Public Const TitleSize As Integer = 12
    Public TitleColor As Color = Colors.DarkTurquoise
    Public LabTitleColor As Color = Colors.Lime
    Public AddLabFound As Color = Colors.DodgerBlue
    Public AddLabFoundOne As Color = Colors.MediumSpringGreen
    Public AddLabFoundZero As Color = Colors.Crimson
    Public AddLabColumn As Color = Color.FromRgb(90, 90, 90)
    Public AddLabColumnBorder As Color = Color.FromRgb(130, 130, 130)
    Public Const DefaultAvailability As String = "Available by default"
    Public Const ButtonPanelSuffix As String = "ButtonPanel"
    Public Const CommentOnSyn As String = "Synthesizable"
    Public Const AllBtnFormat As String = "All ({0}/{1})"
    Public Const InorganicBtnFormat As String = "Inorganic ({0}/{1})"
    Public Const OrganicBtnFormat As String = "Organic ({0}/{1})"
    Public Const ElementsBtnFormat As String = "Elements ({0}/{1})"
    Public Const LabAllBtnFormat As String = "All ({0})"
    Public Const LabInorganicBtnFormat As String = "Inorganic ({0})"
    Public Const LabOrganicBtnFormat As String = "Organic ({0})"
    Public Const LabElementsBtnFormat As String = "Elements ({0})"
    Public Const LabSynthasizableBtnFormat As String = "Synthasizable ({0})"
    Public Const CompoundsFormat As String = "Compounds ({0})"
    Public Const ReaAllBtnFormat As String = "All ({0})"
    Public Const ReaRecreatableBtnFormat As String = "Recreatable({0})"
    Public Const ReaSynthesisBtnFormat As String = "Synthesis ({0})"
    Public Const ReaDecompositionBtnFormat As String = "Decomposition ({0})"
    Public Const ReaReversibleBtnFormat As String = "Reversible ({0})"
    Public Const ReaInProgressBtnFormat As String = "InProgress ({0})"
    Public Const ReaOtherBtnFormat As String = "Other ({0})"
    Public Const SelectTypeFormat As String = "Select{0}Type"

    Public Const CarbonateString As String = "Carbonate"
    Public Const CarbideString As String = "Carbide"
    Public Const OxideString As String = "Oxide"
    Public Const CyanideString As String = "Cyanide"
    Public Const CyanateString As String = "Cyanate"
    Public Const ThyocyanateString As String = "Thyocyanate"

    Public Const EmailFormat As String = "mailto:{0}?subject=Suggestion&body=Name:"
    Public Const Email As String = "labassistant.request@gmail.com"

    Public Const ReactionPlus As String = "  + "

    Private Const UnmodFormat As String = "{0}:{1}:{2}"
    Public Const ConversionFormat As String = "{0}FromKilogram"
    Public Const MoleRatioFormat As String = "{0} > {1}"

    Public ReadOnly Property OwnershipFormat As String
        Get
            Return UnmodFormat.Replace(Column, Splitter)
        End Get
    End Property

    Public Function FromKelvinToFahrenheit(ByVal temperature As Double) As Double
        Return (temperature + Matter.ChemistryConstants.ZeroKelvin) * ToFahrenheitFactor + ToFahrenheitAdd
    End Function

    Public Function FromKelvinToCelsius(ByVal temperature As Double, Optional ByVal viceversa As Boolean = False) As Double
        If viceversa Then
            Return temperature - Matter.ChemistryConstants.ZeroKelvin
        Else
            Return temperature + Matter.ChemistryConstants.ZeroKelvin
        End If
    End Function

    Public Class Conversion

        Public Shared Function GramFromKilogram(ByVal mass As Double, Optional ByVal viceversa As Boolean = False) As Double
            If viceversa Then
                Return mass / GramKilograms
            Else
                Return mass * GramKilograms
            End If
        End Function

        Public Shared Function PoundFromKilogram(ByVal mass As Double, Optional ByVal viceversa As Boolean = False) As Double
            If viceversa Then
                Return mass / PoundKilograms
            Else
                Return mass * PoundKilograms
            End If
        End Function

        Public Shared Function OunceFromKilogram(ByVal mass As Double, Optional ByVal viceversa As Boolean = False) As Double
            If viceversa Then
                Return mass / (PoundKilograms * OuncePounds)
            Else
                Return mass * PoundKilograms * OuncePounds
            End If
        End Function

    End Class

    Public Function StrVal(ByVal str As String, Optional ByVal ReturnValueIfNoNumbers As Integer = 0) As Integer
        Dim c() As Char = str.ToCharArray
        Dim nstr As String = String.Empty
        Dim n As Integer = ReturnValueIfNoNumbers
        Dim neg As Boolean = False
        For i As Integer = 0 To c.Count - 1
            If Char.IsNumber(c(i)) Then
                If i > 0 Then
                    neg = (c(i - 1) = CommentTempSeperator)
                End If
                For y As Integer = i To c.Count - 1
                    If Char.IsNumber(c(y)) Then
                        nstr = nstr & c(y)
                    Else
                        Exit For
                    End If
                Next
                Exit For
            End If
        Next
        If Not String.IsNullOrEmpty(nstr) Then
            n = CInt(nstr)
            If neg Then n *= -1
        End If
        Return n
    End Function

    Public Function FormatSubstanceAt(ByVal formula As String, ByVal temp As Double) As String
        Dim cf As New Matter.CompoundFormula(formula)
        If cf.Elements.Count = 1 Then
            Return FormatSubstanceAt(cf.Elements.First, temp)
        Else
            Dim n As String = Matter.CompoundFormula.NameOf(formula)
            If String.IsNullOrEmpty(n) Then
                Return String.Format(CalculateByFormulaFormats, formula, CalculateSymbolsUnknown)
            Else
                Return FormatSubstanceAt(Matter.Compound.FromName(n), temp)
            End If
        End If
    End Function

    Public Function FormatSubstanceAt(ByVal s As Matter.Compound, ByVal temp As Double) As String
        Return FormatSubstanceAt(s.MeltingPoint, s.BoilingPoint, s.Formula.ToString, temp)
    End Function

    Public Function FormatSubstanceAt(ByVal e As Matter.Element, ByVal temp As Double) As String
        Return FormatSubstanceAt(e.MeltingPoint, e.BoilingPoint, e.Formula.ToString, temp)
    End Function

    Private Function FormatSubstanceAt(ByVal melt As Double, ByVal boil As Double, ByVal formula As String, ByVal temp As Double) As String
        If IsNothing(melt) Then
            Return String.Format(CalculateByFormulaFormats, formula, CalculateSymbolsSolid)
        Else
            If melt <= temp Then
                If IsNothing(boil) Then
                    Return String.Format(CalculateByFormulaFormats, formula, CalculateSymbolsLiquid)
                Else
                    If boil <= temp Then
                        Return String.Format(CalculateByFormulaFormats, formula, CalculateSymbolsGas)
                    Else
                        Return String.Format(CalculateByFormulaFormats, formula, CalculateSymbolsLiquid)
                    End If
                End If
            Else
                Return String.Format(CalculateByFormulaFormats, formula, CalculateSymbolsSolid)
            End If
        End If
    End Function

End Module

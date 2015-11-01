Module Constants

#Region "Graphics"

    Public Const Comma As String = ","
    Public Const Period As String = "."

#End Region

#Region "Reactions"
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

    Public Const ToFahrenheitFactor As Single = 1.8
    Public Const ToFahrenheitAdd As Integer = 32
    Public Const GramKilograms As Integer = 10 ^ 3
    Public Const PoundKilograms As Decimal = 0.45359237
    Public Const OuncePounds As Integer = 16

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
    Public Const Amount As String = "[Amount]"
    Public Const Comments As String = "[Comment]"
    Public Const OpenLabFilter As String = "Laboratory files (*.clab)|*.clab"

    Public Const TextFileStart As String = "Compounds"
    Public Const TextFileStartElement As String = "Elements"

    Public Const Column As Char = ":"

    Public Const CarbonateString As String = "Carbonate"
    Public Const CarbideString As String = "Carbide"
    Public Const OxideString As String = "Oxide"
    Public Const CyanideString As String = "Cyanide"
    Public Const CyanateString As String = "Cyanate"
    Public Const ThyocyanateString As String = "Thyocyanate"

    Public Const EmailFormat As String = "mailto:{0}?subject=Suggestion&body=Name:"
    Public Const Email As String = "labassistant.request@gmail.com"

    Private Const UnmodFormat As String = "{0}:{1}:{2}"
    Public Const ConversionFormat As String = "{0}FromKilogram"

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

End Module

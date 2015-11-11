Imports LabAssistantApp.Matter
Imports System.Runtime.Serialization.Formatters.Binary

<Serializable>
Public Class Laboratory

    <Serializable>
    Public Class OwnershipInfo

        Public Enum ChemicalType
            Element
            Compound
        End Enum

        Public ReadOnly Property [Type] As ChemicalType
            Get
                Return typ
            End Get
        End Property
        Private typ As ChemicalType

        Public ReadOnly Property Name As String
            Get
                Return nm
            End Get
        End Property
        Private nm As String

        Public ReadOnly Property Formula As String
            Get
                Return formula_
            End Get
        End Property
        Private formula_ As String

        Public Property Mass As Double
            Get
                Return mass_
            End Get
            Set(value As Double)
                mass_ = value
                RaiseEvent MassChanged(Me, EventArgs.Empty)
            End Set
        End Property
        Private mass_ As Double

        Public ReadOnly Property IsUnlimited As Boolean
            Get
                Return mass_ < 0
            End Get
        End Property

        Public ReadOnly Property Comment As String
            Get
                Return cmt
            End Get
        End Property
        Private cmt As String

        Public Event MassChanged As EventHandler(Of EventArgs)

        Public Sub New(ByVal name As String, ByVal formula As String, ByVal mass As Double, ByVal comment As String)
            nm = name
            formula_ = formula
            mass_ = mass
            cmt = comment
        End Sub

    End Class

    Public ReadOnly Property Inventory As List(Of OwnershipInfo)
        Get
            Return inv
        End Get
    End Property
    Private inv As New List(Of OwnershipInfo)

    Public Overridable Property Name As String
        Get
            Return lab_name
        End Get
        Set(value As String)
            lab_name = value
            raiseChanged()
        End Set
    End Property
    Private lab_name As String

    Public ReadOnly Property HasChanged As Boolean
        Get
            Return isChange
        End Get
    End Property
    Private isChange As Boolean = False

    Public Event LabChanged As EventHandler(Of EventArgs)
    Private Sub raiseChanged()
        isChange = True
        RaiseEvent LabChanged(Me, EventArgs.Empty)
    End Sub

    Public Sub New()
        lab_name = "Unnamed laboratory"
    End Sub

    Public Sub New(ByVal labName As String)
        If IsNothing(labName) OrElse labName.Length = 0 Then
            lab_name = "Unnamed laboratory"
        Else
            lab_name = labName
        End If
    End Sub

    Public Sub AddChemical(ByVal c As Chemical, ByVal mass As Double, Optional ByVal isUnlimeted As Boolean = False, Optional ByVal Comment As String = "")
        If inv.FindAll(Function(x As OwnershipInfo) x.Name = c.Name).Count = 0 Then
            If isUnlimeted Then mass = -1
            inv.Add(New OwnershipInfo(c.Name, c.FormulaString, mass, Comment))
            If isUnlimeted Then c.SetLabState(StateInLab.Available) Else c.SetLabState(StateInLab.In_Stock)
            raiseChanged()
        End If
    End Sub

    Public Sub RemoveChemical(ByVal c As Chemical)
        If inv.RemoveAll(Function(x As OwnershipInfo) x.Name = c.Name) > 0 Then
            raiseChanged()
        End If
    End Sub

    Public Function IsAvailable(ByVal formula As String) As Boolean
        Return inv.FindAll(Function(x As OwnershipInfo) x.Formula.Equals(formula)).Count > 0
    End Function

    Public Function GetInfoOf(ByVal formula As String) As OwnershipInfo
        Return inv.Find(Function(x As OwnershipInfo) x.Formula = formula)
    End Function

    Public Shared Function SaveTo(ByVal path As String, ByVal lab As Laboratory) As Boolean
        Try
            Dim bf As New BinaryFormatter()
            Using ms As New IO.MemoryStream()
                bf.Serialize(ms, lab)
                IO.File.WriteAllBytes(path, ms.ToArray())
            End Using
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function LoadFrom(ByVal path As String) As Laboratory
        Try
            Dim bf As New BinaryFormatter()
            Using fs As IO.FileStream = IO.File.OpenRead(path)
                LoadFrom = bf.Deserialize(fs)
            End Using
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Class

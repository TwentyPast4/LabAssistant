﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("LabAssistantApp.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Structure:
        '''	[Name]
        '''		[Appearance]
        '''		[Oxidation states]
        '''Elements:
        '''Hydrogen
        '''	Colourless gas
        '''	101
        '''Helium
        '''	Colourless gas, exhibiting a red-orange glow when placed in a high-voltage electric field
        '''	0
        '''Lithium
        '''	Silvery-white (shown floating in oil)
        '''	1
        '''Beryllium
        '''	White-grey metallic
        '''	2
        '''Boron
        '''	Black-brown
        '''	3
        '''Carbon
        '''	Diamond: clear[n]Graphite: black
        '''	432101234
        '''Nitrogen
        '''	Colourless gas, liquid or solid
        '''	3035
        '''Oxygen
        '''	Colorless gas, pale blue liquid
        '''	20
        '''Fluorine
        '''	Gas: very pale yellow[n]Li [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property ElementsResx() As String
            Get
                Return ResourceManager.GetString("ElementsResx", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Structure:
        '''	[IUPAC name]
        '''		[Formula]
        '''		[Appearance]
        '''		[Density]
        '''		[Melting]
        '''		[Boiling]
        '''		[Solubility]
        '''Compounds:
        '''Aluminium antimonide
        '''	AlSb
        '''	Black crystals
        '''	4,36
        '''	1330
        '''	2740
        '''	Insoluble
        '''Aluminium arsenide
        '''	AlAs
        '''	Orange crystals
        '''	3,72
        '''	2013
        '''	[]
        '''	Reacts
        '''Aluminium nitride
        '''	AlN
        '''	White to pale-yellow solid
        '''	3,26
        '''	2470
        '''	[]
        '''	Reacts
        '''Aluminium oxide
        '''	Al2O3
        '''	White solid
        '''	4
        '''	2345
        '''	3250
        '''	Insoluble
        '''Aluminium phosphide
        '''	AlP
        '''	Yellow or gray crystals
        '''	2,85
        '''	2800
        '''	[]
        '''	Reacts
        '''Alumi [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property InorganicCompoundsResx() As String
            Get
                Return ResourceManager.GetString("InorganicCompoundsResx", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 2NaCl &gt; 2Na + Cl2
        '''	Electrolysis
        '''2NaCl + 2H2O &gt; Cl2 + H2 + 2NaOH
        '''	Electrolysis
        '''2KCl + 2H2O &gt; Cl2 + H2 + 2KOH
        '''	Electrolysis
        '''4HCl + MnO2 &gt; MnCl2 + 2H2O + Cl2
        '''S + O2 &gt; SO2
        '''S + 3F2 &gt; SF6
        '''5S + 4KOH &gt; K2S3 + K2S2O3 + H2O
        '''4Ca5(PO4)3F + 18SiO2 + 30C &gt; 3P4 + 30CO + 18CaSiO3 + 2CaF2
        '''2Ca3(PO4)2 + 6SiO2 + 10C &gt; 6CaSiO3 + 10CO + P4
        '''3Ca(PO3)2 + 10C &gt; Ca3(PO4)2 + 10CO + P4
        '''P4 + 6F2 &gt; 4PF3
        '''P4 + 6I2 &gt; 4PI3
        '''Si + O2 &gt; SiO2
        '''Si + 2F2 &gt; SiF4
        '''Si + 2Cl2 &gt; SiCl4
        '''Si + 2Br2 &gt; SiBr4
        '''Si + 2I2 &gt; SiI4
        '''4Al + 3O2 &gt; 2Al2O [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property ReactionsResx() As String
            Get
                Return ResourceManager.GetString("ReactionsResx", resourceCulture)
            End Get
        End Property
    End Module
End Namespace

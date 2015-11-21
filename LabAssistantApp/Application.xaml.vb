Imports System.Threading
Imports System.Windows.Threading
Imports LabAssistantApp.Matter

Class Application

    Public Shared bc As New Concurrent.ConcurrentQueue(Of String)
    Public Icon As ImageSource
    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.
    Public Sub startSub(sender As Object, e As StartupEventArgs) Handles Me.Startup
        Dim t As New Thread(New ParameterizedThreadStart(AddressOf loadingStart))
        t.SetApartmentState(ApartmentState.STA)
        t.IsBackground = True
        t.Start(Me)

        bc.Enqueue("Loading chemicals")
        'Work
        Matter.Info.InitializeEnvironment()

        If My.Settings.AutoStartup Then
            Dim startFile As String = My.Settings.AutoStartupFile
            If IO.File.Exists(startFile) Then
                bc.Enqueue("Loading laboratory")
                Dim l As Laboratory = Laboratory.LoadFrom(startFile)
                If IsNothing(l) Then
                    MsgBox(String.Format("Error loading data from {0}", startFile), MsgBoxStyle.Exclamation, "Lab Assistant")
                Else
                    Matter.Info.LoadLab(l)
                End If
            End If
        End If

        bc.Enqueue("Creating interface")
        Dim mw As New LabWindow
        Icon = getIconSource()
        mw.Icon = Me.Icon
        Me.MainWindow = mw
        AddHandler Matter.Info.LaboratoryLoaded, AddressOf mw.handleLaboratoryLoaded

        mw.Initialize()
        mw.Show()

        Dim cm As ContextMenu = Me.FindResource("labMenu")
        For Each i As MenuItem In cm.Items
            AddHandler i.Click, AddressOf handleMenuClick
        Next
        AddHandler cm.Opened, AddressOf handleMenuOpened

        bc.Enqueue("close")
    End Sub

    Private Sub handleRowDoubleClick(sender As Object, e As RoutedEventArgs)
        CreateCompoundInfoForm(CType(sender, DataGridRow).Item)
    End Sub

    Private Shared lastChemical As Chemical
    Private Shared Sub handleMenuOpened(ByVal sender As ContextMenu, e As RoutedEventArgs)
        Dim clickedSubstance As Chemical = Nothing
        Dim senderType As Type = sender.PlacementTarget.GetType()
        If senderType.Equals(GetType(DataGridRow)) Then
            clickedSubstance = CType(sender.PlacementTarget, DataGridRow).Item
        ElseIf senderType.Equals(GetType(TableButton)) Then
            clickedSubstance = CType(sender.PlacementTarget, TableButton).Element
        End If
        lastChemical = clickedSubstance
        Dim b As Boolean = Not IsNothing(Matter.Info.LoadedLab) AndAlso Matter.Info.LoadedLab.IsAvailable(clickedSubstance.FormulaString)
        For Each itm As MenuItem In sender.Items
            If itm.Name.Equals("addToLab") Then
                itm.IsEnabled = Not b
            ElseIf itm.Name.Equals("removeFromLab") Then
                itm.IsEnabled = b
            End If
        Next
    End Sub

    Private Shared Function getIconSource() As ImageSource
        Dim img As System.Drawing.Icon = My.Resources.LabAssistantIcon
        Dim bitmap As System.Drawing.Bitmap = img.ToBitmap()
        Return Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())
    End Function

#Region "Loading thread"

    Private Shared Sub loadingStart(ByVal app As Application)
        Dim lw As New LoadWindow(app)
        lw.Show()
        Dispatcher.Run()
    End Sub

#End Region

    Private Shared Sub handleMenuClick(ByVal sender As Object, e As EventArgs)
        Select Case sender.Name
            Case "addToLab"
                If IsNothing(Matter.Info.LoadedLab) Then
                    displayLabWarning()
                Else
                    Dim aw As New AddChemicalWindow(lastChemical)
                    aw.Owner = My.Application.MainWindow
                    If aw.ShowDialog() Then
                        AddToLaboratory(lastChemical, aw.Amount, aw.Comment)
                    End If
                End If
            Case "removeFromLab"
                If Not My.Settings.AskForConfirmationOnDelete Then
                    Dim msgRes As MessageBoxResult = MsgBox("Are you sure you want to remove " + lastChemical.Name + " from your laboratory?", MsgBoxStyle.YesNo, "Confirmation")
                    If msgRes = MessageBoxResult.Yes Then
                        RemoveFromLaboratory(lastChemical)
                    End If
                Else
                    RemoveFromLaboratory(lastChemical)
                End If
            Case "info"
                If lastChemical.Formula.IsElement Then
                    CreateElementInfoForm(Element.GetElementFromName(lastChemical.Name))
                Else
                    CreateCompoundInfoForm(Compound.FromName(lastChemical.Name))
                End If
        End Select
    End Sub

    Public Shared Sub CreateElementInfoForm(ByVal e As Element)
        Dim infoDialog As New ElementInfoWindow(e)
        infoDialog.Icon = My.Application.Icon
        infoDialog.Owner = My.Application.MainWindow
        infoDialog.Show()
    End Sub

    Public Shared Sub CreateCompoundInfoForm(ByVal c As Compound)
        Dim infoDialog As New CompoundInfoWindow(c)
        infoDialog.Icon = My.Application.Icon
        infoDialog.Owner = My.Application.MainWindow
        infoDialog.Show()
    End Sub

    Public Shared Sub AddToLaboratory(ByVal c As Chemical, amount As Double, ByVal Optional comment As String = "")
        If IsNothing(Matter.Info.LoadedLab) Then
            displayLabWarning()
        Else
            Matter.Info.LoadedLab.AddChemical(c, amount, amount < 0, comment)
        End If
    End Sub

    Public Shared Sub RemoveFromLaboratory(ByVal c As Chemical)
        If IsNothing(Matter.Info.LoadedLab) Then
            displayLabWarning()
        Else
            Matter.Info.LoadedLab.RemoveChemical(c)
        End If
    End Sub

    Private Shared Sub displayLabWarning()
        MsgBox("You do not have an open laboratory." & vbNewLine & "Create or load a laboratory to begin adding chemicals.", MsgBoxStyle.Information, "Lab Assistant")
    End Sub



End Class

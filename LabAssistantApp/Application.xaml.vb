Imports System.Threading
Imports System.Windows.Threading

Class Application

    Public Shared bc As New Concurrent.ConcurrentQueue(Of String)

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.
    Public Sub test(sender As Object, e As StartupEventArgs) Handles Me.Startup
        Dim t As New Thread(New ParameterizedThreadStart(AddressOf loadingStart))
        t.SetApartmentState(ApartmentState.STA)
        t.IsBackground = True
        t.Start(Me)

        bc.Enqueue("Loading chemicals")
        'Work
        Matter.Info.InitializeEnvironment()

        bc.Enqueue("Checking for laboratory")
        If My.Settings.AutoStartup Then
            Dim startFile As String = My.Settings.AutoStartupFile
            If IO.File.Exists(startFile) Then
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
        Me.MainWindow = mw
        AddHandler Matter.Info.LabratoryLoaded, AddressOf mw.handleLaboratoryLoaded
        mw.Initialize()
        mw.Show()

        Dim cm As ContextMenu = Me.FindResource("labMenu")
        AddHandler cm.ContextMenuOpening, AddressOf handleMenuOpened
        Dim asstolabbtn As MenuItem = cm.Template.FindName("addToLab", cm)


        bc.Enqueue("close")
    End Sub

    Private Sub handleMenuOpened(ByVal sender As ContextMenu, e As EventArgs)
        Dim clickedSubstance As Matter.Chemical = CType(sender.PlacementTarget, DataGridRow).Item
        Dim addtolabbtn As MenuItem = sender.Template.FindName("addToLab", sender)
        If Not IsNothing(Matter.Info.LoadedLab) AndAlso Matter.Info.LoadedLab.IsAvailable(clickedSubstance.FormulaString) Then
            addtolabbtn.IsEnabled = False
        Else
            addtolabbtn.IsEnabled = True
        End If
    End Sub

#Region "Loading thread"

    Private Shared Sub loadingStart(ByVal app As Application)
        Dim lw As New LoadWindow(app)
        lw.Show()
        Dispatcher.Run()
    End Sub

#End Region

    Private Shared Sub handleMenuClick(ByVal sender As Object, e As EventArgs)
        Dim s As MenuItem = sender
        Dim clickedSubstance As Matter.Chemical = CType(CType(s.Parent, ContextMenu).PlacementTarget, DataGridRow).Item
        Select Case s.Name
            Case "addToLab"
                Console.Write("Add to laboratory: ")
            Case "removeFromLab"
                Console.Write("Remove from laboratory: ")
            Case "info"
                Console.Write("Properties of: ")
        End Select
        Console.WriteLine(clickedSubstance.Name)
    End Sub


End Class

Imports System.Collections.Concurrent
Imports System.Threading
Imports System.Windows.Threading

Class Application

    Public bc As New BlockingCollection(Of String)

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.
    Public Sub test(sender As Object, e As StartupEventArgs) Handles Me.Startup
        Dim t As New Thread(New ParameterizedThreadStart(AddressOf loadingStart))
        t.SetApartmentState(ApartmentState.STA)
        t.IsBackground = True
        t.Start(Me)
        bc.Add("Loading chemicals")
    End Sub

    Private Sub setMainWindow(ByVal w As Window)
        Me.MainWindow = w
    End Sub

#Region "Loading thread"

    Private Shared Sub loadingStart(ByVal app As Application)
        Dim lw As New LoadWindow(app)
        AddHandler lw.ContentRendered, AddressOf loadingRendered
        lw.Show()
        Dispatcher.Run()
    End Sub

    Private Shared Sub loadingRendered(ByVal sender As LoadWindow, e As EventArgs)
        sender.ApplicationReference.Dispatcher.Invoke(New Action(Of LoadWindow)(AddressOf sender.ApplicationReference.doWork), sender)
    End Sub

    Private Sub doWork(ByVal sender As LoadWindow)
        Matter.Info.InitializeEnvironment()

        If My.Settings.AutoStartup Then
            'writeProgress(sender, "Searching for laboratory")
            Dim startFile As String = My.Settings.AutoStartupFile
            If IO.File.Exists(startFile) Then
                'writeProgress(sender, "Loading laboratory")
                Dim l As Laboratory = Laboratory.LoadFrom(startFile)
                If IsNothing(l) Then
                    MsgBox(String.Format("Error loading data from {0}", startFile), MsgBoxStyle.Exclamation, "Lab Assistant")
                Else
                    Matter.Info.LoadLab(l)
                End If
            End If
        End If

        'writeProgress(sender, "Loading main interface")
        Dim mw As New LabWindow
        Me.MainWindow = mw
        AddHandler Matter.Info.LabratoryLoaded, AddressOf mw.handleLaboratoryLoaded
        'writeProgress(sender, "Loading graphics")
        mw.Initialize()
        mw.Show()
        bc.Add("close")
    End Sub

#End Region


End Class

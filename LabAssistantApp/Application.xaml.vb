Imports System.Threading
Imports System.Windows.Threading

Class Application

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.
    Public Sub test(sender As Object, e As StartupEventArgs) Handles Me.Startup
        Dim t As New Thread(New ParameterizedThreadStart(AddressOf loadingStart))
        t.SetApartmentState(ApartmentState.STA)
        t.IsBackground = True
        t.Start(Me)
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

        'app.Dispatcher.Invoke(New Action(Of Window)(AddressOf app.setMainWindow), lw)
    End Sub

    Public Shared Sub loadingRendered(ByVal sender As LoadWindow, e As EventArgs)
        sender.writeInfo("Loading chemicals")
        'writeProgress(sender, "Loading chemicals")
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
        sender.ApplicationReference.Dispatcher.Invoke(New Action(Of Window)(AddressOf sender.ApplicationReference.setMainWindow), mw)
        AddHandler Matter.Info.LabratoryLoaded, AddressOf mw.handleLaboratoryLoaded
        'writeProgress(sender, "Loading graphics")
        mw.Initialize()

        mw.Show()
        sender.Dispatcher.Invoke(New Action(AddressOf sender.Close))
    End Sub

    Private Sub writeProgress(ByVal window As LoadWindow, ByVal info As String)
        window.Dispatcher.Invoke(New Action(Of String)(AddressOf window.writeInfo), info)
    End Sub

#End Region


End Class

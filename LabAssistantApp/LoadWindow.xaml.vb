Imports System.Windows.Media.Animation
Imports System.ComponentModel

Public Class LoadWindow

    Private WithEvents da As New DoubleAnimation()
    Private WithEvents ca As New ColorAnimation(Colors.Green, New Duration(TimeSpan.FromSeconds(2)))
    Private WithEvents bw As New BackgroundWorker()
    Private isAnimRunning As Boolean = False
    Private nextGoal As Double = -1
    Private progress As Double = 0

    Private lastAnimation As Double = 0
    Private lastColor As Integer = 0

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        da.Duration = New Duration(TimeSpan.FromSeconds(1))

        statusText.Text = "Initializing"
        bw.WorkerReportsProgress = True
        fillRectangle.Fill = New SolidColorBrush(Colors.Blue)
        animateColor()
        With My.Application.Info.Version
            versionText.Text = String.Format("v {0}.{1}.{2}", .Major, .Minor, .Build)
        End With
    End Sub

    Private Sub HandleContentRender(sender As Object, e As EventArgs)
        bw.RunWorkerAsync()
    End Sub

    Private Sub handlebwWork(sender As Object, e As DoWorkEventArgs) Handles bw.DoWork
        Dim bw1 As BackgroundWorker = sender
        bw1.ReportProgress(0, "Loading chemicals")
        Matter.Info.InitializeEnvironment()

        bw1.ReportProgress(0, "Checking for laboratory")
        Dim startFile As String = My.Settings.AutoStartupFile
        If IO.File.Exists(startFile) Then
            bw1.ReportProgress(0, "Loading laboratory")
            Dim l As Laboratory = Laboratory.LoadFrom(startFile)
            If IsNothing(l) Then
                MsgBox(String.Format("Error loading data from {0}", startFile), MsgBoxStyle.Exclamation, "Lab Assistant")
            Else
                Matter.Info.LoadLab(l)
            End If
        End If
    End Sub

    Private Sub handleProgress(sender As Object, e As ProgressChangedEventArgs) Handles bw.ProgressChanged
        statusText.Text = e.UserState
    End Sub

    Private Sub animateColor()
        fillRectangle.Fill.BeginAnimation(SolidColorBrush.ColorProperty, ca)
    End Sub

    Private Sub HandleColorAnimCompleted(sender As Object, e As EventArgs) Handles ca.Completed
        Select Case lastColor Mod 3
            Case Is = 0
                ca.To = Colors.Red
            Case Is = 1
                ca.To = Colors.Blue
            Case Is = 2
                ca.To = Colors.Green
        End Select
        animateColor()
        lastColor += 1
    End Sub

    Private Sub loadFinished(sender As Object, e As RunWorkerCompletedEventArgs) Handles bw.RunWorkerCompleted
        Dim mw As New LabWindow
        mw.Initialize()
        AddHandler Matter.Info.LabratoryLoaded, AddressOf mw.handleLaboratoryLoaded
        mw.Show()
        Me.Close()
    End Sub

End Class

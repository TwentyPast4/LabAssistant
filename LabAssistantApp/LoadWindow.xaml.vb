Imports System.Windows.Media.Animation
Imports System.Runtime.Serialization.Formatters.Binary

Public Class LoadWindow

    Private WithEvents da As New DoubleAnimation()
    Private WithEvents ca As New ColorAnimation(Colors.Green, New Duration(TimeSpan.FromSeconds(3)))
    Private isAnimRunning As Boolean = False
    Private nextGoal As Double = -1
    Private progress As Double = 0

    Private lastAnimation As Double = 0
    Private lastColor As Integer = 0

    Private mw As MainWindow

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        da.Duration = New Duration(TimeSpan.FromSeconds(1))

        statusText.Text = "Initializing"
        fillRectangle.Fill = New SolidColorBrush(Colors.Blue)
        animateColor()
        With My.Application.Info.Version
            versionText.Text = String.Format("v {0}.{1}.{2}", .Major, .Minor, .Build)
        End With
    End Sub

    Private Sub HandleContentRender(sender As Object, e As EventArgs)


        statusText.Text = "Loading chemicals"
        Matter.Info.InitializeEnvironment()
        mw = New MainWindow
        AddHandler Matter.Info.LabratoryLoaded, AddressOf mw.handleLaboratoryLoaded

        statusText.Text = "Checking for laboratory"
        Dim startFile As String = My.Settings.AutoStartupFile
        If IO.File.Exists(startFile) Then
            statusText.Text = "Loading laboratory"
            Dim l As Laboratory = Laboratory.LoadFrom(startFile)
            If IsNothing(l) Then
                MsgBox(String.Format("Error loading data from {0}", startFile), MsgBoxStyle.Exclamation, "Lab Assistant")
            Else
                Matter.Info.LoadLab(l)
            End If
        End If

        loadFinished()
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

    Private Sub loadFinished()
        mw.Show()
        Me.Close()
    End Sub

End Class

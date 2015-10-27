Imports System.ComponentModel

Public Class ClickableControl
    Inherits UserControl

    Public Shared ReadOnly ClickEvent As RoutedEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, GetType(RoutedEventHandler), GetType(ClickableControl))

    <Browsable(True)>
    Public Custom Event Click As RoutedEventHandler
        AddHandler(ByVal value As RoutedEventHandler)
            Me.AddHandler(ClickEvent, value)
        End AddHandler

        RemoveHandler(ByVal value As RoutedEventHandler)
            Me.RemoveHandler(ClickEvent, value)
        End RemoveHandler

        RaiseEvent(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event

    Private Sub RaiseTapEvent()
        Dim newEventArgs As New RoutedEventArgs(ClickableControl.ClickEvent)
        MyBase.RaiseEvent(newEventArgs)
    End Sub

    Private Sub RaiseClickEvent()
        Dim e As New RoutedEventArgs(ClickableControl.ClickEvent)
        MyBase.RaiseEvent(e)
    End Sub


    Private clicked_ As Boolean = False
    Private Sub meLeftDownc(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseLeftButtonDown
        If Me.IsEnabled Then
            clicked_ = True
        End If
    End Sub

    Private Sub meLeftUpc(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseLeftButtonUp
        If clicked_ Then
            RaiseClickEvent()
        End If
        clicked_ = False
    End Sub

    Private Sub meLeavec(sender As Object, e As MouseEventArgs) Handles Me.MouseLeave
        clicked_ = False
    End Sub

End Class

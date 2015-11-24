Public Class NumberTextBox

    Public Event NumberChanged As RoutedEventHandler

    Public Property Number As Decimal
        Get
            Return num
        End Get
        Set(value As Decimal)
            If value <> num Then
                Me.Text = Matter.Info.ToStringDecimal(value)
            End If
        End Set
    End Property
    Private num As Decimal = 0

    Private Sub handleTextChanged(sender As Object, e As TextChangedEventArgs) Handles Me.TextChanged
        If isNumber(Me.Text) Then
            Dim d As Decimal = Matter.Info.ToDecimal(Me.Text)
            If d <> num Then
                num = d
                If Me.IsInitialized Then RaiseEvent NumberChanged(Me, New RoutedEventArgs())
            End If
        End If
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

End Class

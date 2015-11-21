Public Class ReactantRow

    Private handleUnit As Boolean = True
    Public Property SelectedUnit As Matter.UnitOfMass
        Get
            Return unitComboBox.SelectedIndex
        End Get
        Set(value As Matter.UnitOfMass)
            If CInt(value) <> unitComboBox.SelectedIndex Then
                handleUnit = False
                unitComboBox.SelectedIndex = CInt(value)
                handleUnit = True
            End If
        End Set
    End Property

    Public Property Amount As Decimal
        Get
            Return amount_
        End Get
        Set(value As Decimal)
            If amount_ <> value Then
                amount_ = value
                handleUnit = False
                amountBox.Text = Matter.Info.ToStringDecimal(value, 4)
                handleUnit = True
            End If
        End Set
    End Property
    Private amount_ As Decimal = 0

    Public Property Formula As String
        Get
            Return formula_
        End Get
        Set(value As String)
            If Not value.Equals(formula_) Then
                formula_ = value
                formulaLabel.Content = disC.Convert(value, Nothing, Nothing, Nothing)
            End If
        End Set
    End Property
    Private formula_ As String

    Private disC As New FormulaDisplayConverter()
    Public Sub New(ByVal formula__ As String, ByVal Optional amount__ As Decimal = 0)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Formula = formula__
        amountBox.Text = Matter.Info.ToStringDecimal(amount__, 4)
    End Sub

    Public Event AmountChanged As RoutedEventHandler

    Private Sub textChangedHandler(sender As Object, e As TextChangedEventArgs) Handles amountBox.TextChanged
        If isNumber(amountBox.Text) Then
            Dim val As Decimal = amount_
            amount_ = Matter.Info.ToDecimal(amountBox.Text)
            If handleUnit Then
                RaiseEvent AmountChanged(Me, New RoutedEventArgs())
            End If
        End If
    End Sub

    Private Sub selectionChangedHandler(sender As Object, e As SelectionChangedEventArgs) Handles unitComboBox.SelectionChanged
        If Me.IsInitialized Then
            handleUnit = False
            Me.Amount = Convert(amount_, unitComboBox.Items.IndexOf(e.RemovedItems(0)), unitComboBox.SelectedIndex, New Matter.CompoundFormula(formula_))
            handleUnit = True
        End If
    End Sub

End Class

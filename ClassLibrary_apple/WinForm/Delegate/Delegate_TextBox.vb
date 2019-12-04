'20170414
Public Class Delegate_TextBox
    Private t_textbox As System.Windows.Forms.TextBox
    Sub New(ByVal textbox As System.Windows.Forms.TextBox)
        Me.t_textbox = textbox
    End Sub


    Delegate Sub SetTextCallback(ByVal textbox As System.Windows.Forms.TextBox, ByVal text As String)
    Sub setText(ByVal textbox As System.Windows.Forms.TextBox, ByVal text As String)
        If t_textbox.InvokeRequired Then
            t_textbox.FindForm.Invoke(New SetTextCallback(AddressOf setText), New Object() {t_textbox, text})
        Else
            t_textbox.Text = text
        End If
    End Sub

    Property text As String
        Get
            Return t_textbox.Text
        End Get
        Set(value As String)
            setText(t_textbox, value)
        End Set
    End Property
    Delegate Sub SetTextArrayCallback(ByVal textbox As System.Windows.Forms.TextBox, ByVal text As String())
    Sub setTextArray(ByVal textbox As System.Windows.Forms.TextBox, ByVal text As String())
        If t_textbox.InvokeRequired Then
            t_textbox.FindForm.Invoke(New SetTextArrayCallback(AddressOf setTextArray), New Object() {t_textbox, text})
        Else
            t_textbox.Lines = text
        End If
    End Sub
    Property Lines As String()
        Get
            Return t_textbox.Lines
        End Get
        Set(value As String())
            Me.setTextArray(t_textbox, value)
        End Set
    End Property
End Class

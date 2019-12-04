'20170110
Module M_Form_Delegate
#Region " TextBox"
    Public Delegate Sub Delegate_TextBox_Line_Callback(ByVal TextBox As System.Windows.Forms.TextBox, ByVal lines As String())

    Public Sub setTextBox_Line(ByVal TextBox As System.Windows.Forms.TextBox, ByVal lines As String())
        If TextBox.FindForm.InvokeRequired() Then
            Dim d_reportPing As New Delegate_TextBox_Line_Callback(AddressOf setTextBox_Line)
            TextBox.FindForm.BeginInvoke(d_reportPing, TextBox, lines)
            'Me.Invoke(d_reportPing, IsSuccess)
        Else
            TextBox.Lines = lines
            '    Button.Enabled = IsEnabled
            TextBox.SelectionStart = TextBox.Lines.Length
            TextBox.ScrollToCaret()
        End If
    End Sub
    Public Delegate Sub Delegate_TextBox_Text_Callback(ByVal TextBox As System.Windows.Forms.TextBox, ByVal text As String)

    Public Sub setTextBox_Text(ByVal TextBox As System.Windows.Forms.TextBox, ByVal text As String)
        If TextBox.FindForm.InvokeRequired() Then
            Dim d_reportPing As New Delegate_TextBox_Text_Callback(AddressOf setTextBox_Text)
            TextBox.FindForm.BeginInvoke(d_reportPing, TextBox, text)
            'Me.Invoke(d_reportPing, IsSuccess)
        Else
            TextBox.Text = text
            '    Button.Enabled = IsEnabled
        End If
    End Sub
#End Region


#Region "Button"
    Delegate Sub SetButton_Enabled_Callback(ByVal Button As System.Windows.Forms.Button, ByVal value As Boolean)
    Public Sub SetButton_Enabled(ByVal Button As System.Windows.Forms.Button, ByVal value As Boolean)

        ' InvokeRequired required compares the thread ID of the
        ' calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If Button.FindForm.InvokeRequired Then
            Dim d As New SetButton_Enabled_Callback(AddressOf SetButton_Enabled)
            Button.FindForm.BeginInvoke(d, New Object() {Button, value})
        Else
            Button.Enabled = value
        End If
    End Sub
#End Region
#Region "Label"
    Public Delegate Sub setLabel_Text_Callback(ByVal Label As System.Windows.Forms.Label, ByVal text As String)

    Public Sub setLabel_text(ByVal Label As System.Windows.Forms.Label, ByVal text As String)
        If Label.FindForm.InvokeRequired() Then
            Dim d_reportPing As New setLabel_Text_Callback(AddressOf setLabel_text)
            Label.FindForm.BeginInvoke(d_reportPing, Label, text)
            'Me.Invoke(d_reportPing, IsSuccess)
        Else
            Label.Text = text
            '    Button.Enabled = IsEnabled
        End If
    End Sub
#End Region



End Module

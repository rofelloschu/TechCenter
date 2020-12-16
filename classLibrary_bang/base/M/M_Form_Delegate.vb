'20190508
'20200408
'20200521
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

#Region "Control"

    Delegate Sub SetCtrlTextDelegate(ByVal Control As System.Windows.Forms.Control, ByVal ctrlText As String)

    Public Sub SetCtrlText(ByVal Control As System.Windows.Forms.Control, ByVal ctrlText As String)
        If Control.InvokeRequired Then
            Control.FindForm.Invoke(New SetCtrlTextDelegate(AddressOf SetCtrlText), New Object() {Control, ctrlText})
        Else
            Control.Text = ctrlText
        End If
    End Sub
#End Region
#Region "ListBox"
    Delegate Sub AddListBoxDelegate(ByVal Control As System.Windows.Forms.ListBox, ByVal addText As String)

    Public Sub AddListBox(ByVal Control As System.Windows.Forms.ListBox, ByVal addText As String)
        If Control.InvokeRequired Then
            Control.FindForm.Invoke(New AddListBoxDelegate(AddressOf AddListBox), New Object() {Control, addText})
        Else
            Control.Items.Add(addText)
        End If
    End Sub
    Delegate Sub AddRangeListBoxDelegate(ByVal Control As System.Windows.Forms.ListBox, ByVal addText() As String)

    Public Sub AddRangeListBox(ByVal Control As System.Windows.Forms.ListBox, ByVal addText() As String)
        If Control.InvokeRequired Then
            Control.FindForm.Invoke(New AddListBoxDelegate(AddressOf AddListBox), New Object() {Control, addText})
        Else
            Control.Items.AddRange(addText)
        End If
    End Sub
    Delegate Sub RemoveListBoxDelegate(ByVal Control As System.Windows.Forms.ListBox, ByVal index As Integer)

    Public Sub RemoveListBox(ByVal Control As System.Windows.Forms.ListBox, ByVal index As Integer)
        If Control.InvokeRequired Then
            Control.FindForm.Invoke(New RemoveListBoxDelegate(AddressOf AddListBox), New Object() {Control, index})
        Else
            Control.Items.RemoveAt(index)
        End If
    End Sub
    Delegate Sub ClearListBoxDelegate(ByVal Control As System.Windows.Forms.ListBox)

    Public Sub ClearListBox(ByVal Control As System.Windows.Forms.ListBox)
        If Control.InvokeRequired Then
            Control.FindForm.Invoke(New ClearListBoxDelegate(AddressOf ClearListBox), New Object() {Control})
        Else
            Control.Items.Clear()
        End If
    End Sub
#End Region
#Region "ComboBox"
    Public Delegate Sub Delegate_ComboBox_Line_Callback(ByVal ComboBox As System.Windows.Forms.ComboBox, ByVal lines As String())
    Public Sub setComboBox_Line(ByVal ComboBox As System.Windows.Forms.ComboBox, ByVal lines As String())
        If ComboBox.FindForm.InvokeRequired() Then
            Dim d_reportPing As New Delegate_ComboBox_Line_Callback(AddressOf setComboBox_Line)
            ComboBox.FindForm.BeginInvoke(d_reportPing, ComboBox, lines)
            'Me.Invoke(d_reportPing, IsSuccess)
        Else
            If lines Is Nothing Then
                Exit Sub
            End If
            ComboBox.Items.AddRange(lines)
            '    Button.Enabled = IsEnabled
            'ComboBox.SelectionStart = ComboBox.Items.Count
            ComboBox.SelectionStart = 0
            'ComboBox.ScrollToCaret()
        End If
    End Sub

    Public Delegate Sub Delegate_ComboBox_Text_Callback(ByVal ComboBox As System.Windows.Forms.ComboBox, ByVal Text As String)
    Public Sub setComboBox_Text(ByVal ComboBox As System.Windows.Forms.ComboBox, ByVal Text As String)
        If ComboBox.FindForm.InvokeRequired() Then
            Dim d_reportPing As New Delegate_ComboBox_Text_Callback(AddressOf setComboBox_Text)
            ComboBox.FindForm.BeginInvoke(d_reportPing, ComboBox, Text)
            'Me.Invoke(d_reportPing, IsSuccess)
        Else
            If Text Is Nothing Then
                Exit Sub
            End If
            ComboBox.Text = Text

        End If
    End Sub

    Public Delegate Sub Delegate_ComboBox_clear_Callback(ByVal ComboBox As System.Windows.Forms.ComboBox)
    Public Sub setComboBox_clear(ByVal ComboBox As System.Windows.Forms.ComboBox)
        If ComboBox.FindForm.InvokeRequired() Then
            Dim d_reportPing As New Delegate_ComboBox_Line_Callback(AddressOf setComboBox_Line)
            ComboBox.FindForm.BeginInvoke(d_reportPing, ComboBox)
            'Me.Invoke(d_reportPing, IsSuccess)
        Else

            ComboBox.Items.Clear()


        End If
    End Sub

#End Region

#Region "NumericUpDown"
    Delegate Sub SetNumericUpDown_value_Delegate(ByVal Control As System.Windows.Forms.NumericUpDown, ByVal ctrlText As String)

    Private Sub SetNumericUpDown_value(ByVal Control As System.Windows.Forms.NumericUpDown, ByVal value As Integer)
        If Control.InvokeRequired Then
            Dim d_Delegate As New SetNumericUpDown_value_Delegate(AddressOf SetNumericUpDown_value)
            Control.FindForm.BeginInvoke(d_Delegate, Control, value)

        Else
            Control.Value = value
        End If
    End Sub
#End Region

End Module

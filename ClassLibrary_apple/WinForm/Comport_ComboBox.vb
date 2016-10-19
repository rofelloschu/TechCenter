'配合comport使用
Public Class Comport_ComboBox
    Private m_ComboBox As System.Windows.Forms.ComboBox

    Sub New(t_ComboBox As System.Windows.Forms.ComboBox)
        m_ComboBox = t_ComboBox
        Dim port_string As String() = System.IO.Ports.SerialPort.GetPortNames()

        For index As Integer = 0 To port_string.Length - 1
            SetComboBoxItemAdd(m_ComboBox, port_string(index))
        Next
    End Sub
    Delegate Sub SetComboBoxItemAddCallback(ByVal ComboBox As System.Windows.Forms.ComboBox, ByVal text As String)
    Private Sub SetComboBoxItemAdd(ByVal ComboBox As System.Windows.Forms.ComboBox, ByVal text As String)
        If ComboBox.InvokeRequired Then

            ComboBox.Invoke(New SetComboBoxItemAddCallback(AddressOf SetComboBoxItemAdd), New Object() {ComboBox, text})
        Else
            ComboBox.Items.Add(text)
        End If

    End Sub
End Class

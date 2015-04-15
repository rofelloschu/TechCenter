'20140529
Public Class ComboBoxSelectToTabControl
    Private WithEvents m_ComboBox As System.Windows.Forms.ComboBox
    Private WithEvents m_TabControl As System.Windows.Forms.TabControl
    Sub New(ByVal ComboBox As System.Windows.Forms.ComboBox, ByVal TabControl As System.Windows.Forms.TabControl)
        Me.m_ComboBox = ComboBox
        Me.m_TabControl = TabControl

        For index As Integer = 0 To m_TabControl.TabCount - 1
            m_ComboBox.Items.Add(m_TabControl.TabPages(index).Text)
        Next
        m_ComboBox.SelectedIndex = 0
    End Sub
    Sub ComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles m_ComboBox.SelectedIndexChanged
        m_TabControl.SelectedIndex = DirectCast(sender, System.Windows.Forms.ComboBox).SelectedIndex
        DirectCast(sender, System.Windows.Forms.ComboBox).Focus()
    End Sub
    Sub TabControl_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles m_TabControl.SelectedIndexChanged
        m_ComboBox.SelectedIndex = DirectCast(sender, System.Windows.Forms.TabControl).SelectedIndex
        DirectCast(sender, System.Windows.Forms.TabControl).Focus()
    End Sub
End Class


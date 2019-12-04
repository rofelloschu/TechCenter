Public Class Previous_Next_FlowLayoutPanel
    Private m_FlowLayoutPanel As System.Windows.Forms.FlowLayoutPanel
    Private UserControlList As List(Of Object)
    Private list_index As Integer
    Sub New(t_FlowLayoutPanel As System.Windows.Forms.FlowLayoutPanel)

        m_FlowLayoutPanel = t_FlowLayoutPanel
        UserControlList = New List(Of Object)

        list_index = 0
        'm_FlowLayoutPanel.Controls.Add(UserControlList(list_index))
    End Sub
    Sub addControl(t As Object)

        UserControlList.Add(t)


    End Sub
    Function isHaveNext() As Boolean
        Dim index As Integer = list_index + 1
        If index + 1 > UserControlList.Count Then
            Return False
        End If
        Return True
    End Function
    Function nextControl() As Boolean
        list_index = list_index + 1
        If list_index + 1 > UserControlList.Count Then
            list_index = list_index - 1
            Return False
        End If

        m_FlowLayoutPanel.Controls.Clear()
        m_FlowLayoutPanel.Controls.Add(UserControlList(list_index))
        Return True
    End Function
    Function isHavePrevious() As Boolean
        Dim index As Integer = list_index - 1
        If index + 1 <= 0 Then

            Return False
        End If
        Return True
    End Function
    Function previousControl() As Boolean
        list_index = list_index - 1
        If list_index + 1 <= 0 Then
            list_index = list_index + 1
            Return False
        End If
        m_FlowLayoutPanel.Controls.Clear()
        m_FlowLayoutPanel.Controls.Add(UserControlList(list_index))
        Return True
    End Function
    Function readControl(index) As Boolean
        If index + 1 <= UserControlList.Count And index + 1 > 0 Then
            m_FlowLayoutPanel.Controls.Add(UserControlList(index))

            Return True
        Else
            Return False
        End If
    End Function
End Class

Public Class Work_Ping2_auto
    Inherits Work_Ping2
    Event Reply_status(ip As String, value As Boolean)
    Private t_auto_threePing As Threading.Thread
    Private t_auto_threePing_enable As Boolean
    Public m_ip As String
    Public m_value As Boolean
    Sub New(t_ip As String)
        MyBase.New()
        t_auto_threePing_enable = False
        m_ip = t_ip
        m_value = False
    End Sub
    Sub run_auto_threePing()
        If t_auto_threePing_enable Then
            Exit Sub
        End If
        t_auto_threePing_enable = True
        t_auto_threePing = New Threading.Thread(AddressOf AddressOf_auto_threePing)
        t_auto_threePing.Start()
    End Sub
    Private Sub AddressOf_auto_threePing()
        Dim e_value As Boolean = False
        '1
        e_value = MyBase.Ping(m_ip)
        '2
        If MyBase.Ping(m_ip) Then
            e_value = True
        Else
            '
        End If
        '3
        If MyBase.Ping(m_ip) Then
            e_value = True
        Else
            '
        End If
        Me.m_value = e_value
        RaiseEvent Reply_status(m_ip, e_value)
        t_auto_threePing_enable = False
    End Sub
End Class

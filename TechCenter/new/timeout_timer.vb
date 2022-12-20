'20221130
Public Class timeout_timer
    Protected dead_time As DateTime
    Protected m_timer As Threading.Timer
    Protected m_period_time As Integer
    Protected m_istimeout As Boolean
    Sub New(period_time As Integer)
        m_period_time = period_time
        '最低10秒
        If m_period_time < 10000 Then
            m_period_time = 10000
        End If
        Me.reset()
        m_timer = New Threading.Timer(AddressOf m_timer_sub, Nothing, Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        Me.m_timer.Change(m_period_time, Threading.Timeout.Infinite)

    End Sub

    Protected Sub close()
        Me.m_timer.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
        Me.m_timer.Dispose()
    End Sub
    Protected Overridable Sub m_timer_sub(ByVal stateInfo As Object)
        Me.m_timer.Change(m_period_time, Threading.Timeout.Infinite)
        If Now > dead_time Then
            RaiseEvent timeout()
            m_istimeout = True
        End If
    End Sub
    Overridable Sub reset()
        dead_time = Now.AddMilliseconds(m_period_time)
        m_istimeout = False
    End Sub
    Public ReadOnly Property istimeout() As Boolean
        Get
            Return m_istimeout
        End Get
    End Property

    Event timeout()

End Class

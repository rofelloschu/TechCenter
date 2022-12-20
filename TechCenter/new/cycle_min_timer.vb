'20221130
Public Class cycle_min_timer
    Inherits timeout_timer
    Protected period_min As Integer
    Sub New(min_value As Integer)

        MyBase.New(60000 * min_value)
        period_min = min_value
        setNexttme_due()
    End Sub
    Protected Overrides Sub m_timer_sub(ByVal stateInfo As Object)
        MyBase.m_timer.Change(m_period_time, Threading.Timeout.Infinite)
        If Now > dead_time Then
            RaiseEvent cycle()
            setNexttme()
        End If
    End Sub

    Sub setNexttme()
        dead_time = Now
        dead_time = dead_time.AddMilliseconds(-dead_time.Millisecond)
        dead_time = dead_time.AddSeconds(-dead_time.Second)
        dead_time = dead_time.AddMinutes(period_min)
        Console.WriteLine("n_time " + dead_time.ToString)
    End Sub
    Sub setNexttme_due()
        Me.setNexttme()
        Dim dueTime As Integer = CInt(Me.dead_time.Subtract(Now).TotalMilliseconds)
        MyBase.m_timer.Change(dueTime, Threading.Timeout.Infinite)

    End Sub
    Event cycle()

End Class

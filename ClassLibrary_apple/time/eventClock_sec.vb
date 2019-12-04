'20170929
Namespace time
    Public Class eventClock_sec
        Public Timer As Threading.Timer
        Event timeout()
        Event trigger()
        Private m_period_sec As Integer
        'Private m_dueTime_sec As Integer
        'new 啟動計時

        '測試參數
        Private test_text As Boolean = False
        Sub New(t_period_sec As Integer)
            Me.m_period_sec = t_period_sec
            'Me.m_dueTime_sec = m_period_sec
            Me.Timer = New Threading.Timer(AddressOf AddressOf_Timer, Nothing, Me.m_period_sec * 1000, Me.m_period_sec * 1000)
            If test_text Then
                Console.WriteLine(Now.ToString("u") + " Timer")
            End If

        End Sub
        Private Sub AddressOf_Timer(ByVal state As Object)
            RaiseEvent timeout()
            If test_text Then
                Console.WriteLine(Now.ToString("u") + " timeout")
            End If

        End Sub
        Sub resetTime()
            'Timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
            Timer.Change(Me.m_period_sec * 1000, Me.m_period_sec * 1000)
            RaiseEvent trigger()
            If test_text Then
                Console.WriteLine(Now.ToString("u") + " trigger")
            End If

        End Sub

        Sub close()
            Timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
            Timer.Dispose()
        End Sub
    End Class
End Namespace


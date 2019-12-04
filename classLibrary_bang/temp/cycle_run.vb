Imports System.Threading
'20181217
'ex       Dim a As New cycle_run(AddressOf time)
'整點min
Public Class cycle_run
    Private cycleTime As DateTime
    Private cycleDataTimer As Threading.Timer
    'Private CycleDataPeriod As String
    Public Delegate Sub run_sub()
    Public run As run_sub
    Private mSecPeriod As Integer = 60000
    Private isclose As Boolean
    Sub New(r As run_sub)
        run = r
        isclose = False
        cycleDataTimer = New Threading.Timer(AddressOf CycleDataTimerHandler, Nothing, Timeout.Infinite, Timeout.Infinite)
        mSecPeriod = 60000

        cycleTime = Now
        setNexttimer(mSecPeriod)
    End Sub
    Sub New(r As run_sub, t_SecPeriod As Integer)
        run = r
        cycleDataTimer = New Threading.Timer(AddressOf CycleDataTimerHandler, Nothing, Timeout.Infinite, Timeout.Infinite)
        mSecPeriod = t_SecPeriod
        If mSecPeriod < 6000 Then
            mSecPeriod = 6000
        End If
        setNexttimer(mSecPeriod)
    End Sub
    Sub close()
        isclose = True
        Me.cycleDataTimer.Change(Timeout.Infinite, Timeout.Infinite)
        Me.cycleDataTimer.Dispose()
        GC.Collect()
    End Sub
    Private Sub CycleDataTimerHandler(ByVal stateInfo As Object)

        'Select Case Me.CycleDataPeriod
        '    Case Else
        '        run()

        'End Select
        run()
        setNexttimer(mSecPeriod)
    End Sub
    Public Sub setNexttimer(mSecPeriod As Integer)
        Dim timeNow As DateTime = DateTime.Now
        '設定下次時間
        Me.cycleTime = Me.cycleTime.AddMilliseconds(mSecPeriod)
        '時間相差太大的設定下次時間
        While DateTime.Compare(timeNow, Me.cycleTime) > 0
            Me.cycleTime = Me.cycleTime.AddMilliseconds(mSecPeriod)
        End While
        If True Then
            Me.cycleTime = Me.cycleTime.AddSeconds(-Me.cycleTime.Second)
            Me.cycleTime = Me.cycleTime.AddMilliseconds(-Me.cycleTime.Millisecond)
        End If

        '時差,設定timer
        Dim dueTime As Integer = CInt(Me.cycleTime.Subtract(timeNow).TotalMilliseconds)
        If Not isclose Then
            Me.cycleDataTimer.Change(dueTime, Timeout.Infinite)
        End If

    End Sub

End Class
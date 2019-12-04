Imports System.Threading
Public Class timing_timer
    Private nextLogTime As DateTime
    Private CyclePoint As Integer = 5
    Private logTimer As Threading.Timer
    Sub New()

        Me.logTimer = New Threading.Timer(AddressOf LogTimerHandler, Nothing, Timeout.Infinite, Timeout.Infinite)

        ' Set nextLogTime
        Dim timeNow As DateTime = DateTime.Now
        Me.nextLogTime = New DateTime(timeNow.Year, timeNow.Month, timeNow.Day, timeNow.Hour, timeNow.Minute, CyclePoint)
        If DateTime.Compare(timeNow, Me.nextLogTime) > 0 Then
            Me.nextLogTime = Me.nextLogTime.AddMinutes(1)
        End If
        Console.WriteLine("nextLogTime " + nextLogTime.ToString)
        Dim dueTime As Integer = CInt(Me.nextLogTime.Subtract(timeNow).TotalMilliseconds)
        Me.logTimer.Change(dueTime, Timeout.Infinite)
        Console.WriteLine("dueTime " + dueTime.ToString)
    End Sub
    Private Sub LogTimerHandler(ByVal stateInfo As Object)
        '20161222 推測其他執行緒在還在產生資料 先停1秒等待產生
        Console.WriteLine(Now.ToString)
        System.Threading.Thread.Sleep(5000)
         
         

        

        ' Set nextLogTime
        'timeNow會隨時間 越來越早
        Dim timeNow As DateTime = DateTime.Now
        Me.nextLogTime = Me.nextLogTime.AddMinutes(1)
        While DateTime.Compare(timeNow, Me.nextLogTime) > 0
            Me.nextLogTime = Me.nextLogTime.AddMinutes(1)
        End While
        Console.WriteLine("nextLogTime " + nextLogTime.ToString)
        Dim dueTime As Integer = CInt(Me.nextLogTime.Subtract(timeNow).TotalMilliseconds)
        Me.logTimer.Change(dueTime, Timeout.Infinite)
        Console.WriteLine("dueTime " + dueTime.ToString)
        'Dim timeNow As DateTime = DateTime.Now
        'Me.nextLogTime = New DateTime(timeNow.Year, timeNow.Month, timeNow.Day, timeNow.Hour, timeNow.Minute, value)
        'If DateTime.Compare(timeNow, Me.nextLogTime) > 0 Then
        '    Me.nextLogTime = Me.nextLogTime.AddMinutes(1)
        'End If
        'Dim dueTime As Integer = CInt(Me.nextLogTime.Subtract(timeNow).TotalMilliseconds)
        'Me.logTimer.Change(dueTime, Timeout.Infinite)
    End Sub
End Class

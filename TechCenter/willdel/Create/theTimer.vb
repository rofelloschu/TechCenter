Public Class theTimer
    Private Stopwatch As Stopwatch
    Sub timeStart()
        Stopwatch = New Stopwatch
        Stopwatch.Start()
    End Sub
    Function timeStop() As Long
        Stopwatch.Stop()
        Return Stopwatch.ElapsedMilliseconds
    End Function

    Shared Sub test()
        Dim Stopwatch As New Stopwatch
        Stopwatch.Reset()
        Stopwatch.Start()
        Dim dteStart As DateTime = Now

        Dim t1 As New Threading.Thread(AddressOf auto_count)
        Dim t2 As New Threading.Thread(AddressOf auto_count)
        t1.Start()
        t2.Start()
        t1.Join()
        t2.Join()
        'Threading.Thread.Sleep(1000)
        Stopwatch.Stop()

        Dim TS As TimeSpan = Now.Subtract(dteStart)
        Console.WriteLine("執行時間1-1: " & Stopwatch.ElapsedMilliseconds & " 毫秒")
        Console.WriteLine("執行時間1-2: " & TS.TotalMilliseconds & " 毫秒")



        Stopwatch.Reset()
        Stopwatch.Start()
        dteStart = Now
        Dim t3 As New Threading.Thread(AddressOf auto_count)
        t3.Start()
        t3.Join()
        Stopwatch.Stop()
        'Threading.Thread.Sleep(1000)
        Dim TS2 As TimeSpan = Now.Subtract(dteStart)
        Console.WriteLine("執行時間2-1: " & Stopwatch.ElapsedMilliseconds & " 毫秒")
        Console.WriteLine("執行時間2-2: " & TS2.TotalMilliseconds & " 毫秒")

    End Sub
    Shared Sub auto_count()
        Dim count As Integer = 0
        Dim max As Integer = 50000000
        For index As Integer = 0 To max - 1
            count = index Mod 7
        Next
        For index As Integer = 0 To max - 1
            count = index Mod 7
        Next
    End Sub
End Class

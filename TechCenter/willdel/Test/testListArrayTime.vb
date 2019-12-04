'測試List和Array
Class testListArrayTime
    Private time1 As DateTime
    Private time2 As DateTime
    Private Stopwatch1 As New Stopwatch
    Private Stopwatch2 As New Stopwatch

    Sub New()

    End Sub
    Sub testListArray()
        Dim anow As DateTime = Now
        Dim aList As New List(Of DateTime)
        timeStart("aList建立", Stopwatch1)
        Dim bArray(10000) As DateTime

        For i As Integer = 0 To 10000
            aList.Add(anow)
            anow.AddSeconds(1)
        Next
        timeEnd(Stopwatch1)
        timeStart("bArray建立", Stopwatch2)
        For i As Integer = 0 To 10000
            bArray(i) = anow
            anow.AddSeconds(1)
        Next
        timeEnd(Stopwatch2)

        Console.ReadKey()
        Dim index As Integer = 0
        Dim indexW As Integer = 0
        Dim a As DateTime = Now.AddMinutes(1)
        While a > Now
            timeStart("aList加入", Stopwatch1)
            aList.Add(anow)
            aList.RemoveAt(0)
            timeEnd(Stopwatch1)
            timeStart("bArray加入", Stopwatch2)
            bArray(index) = anow
            If index = 10000 Then
                index = 0
            Else
                index = index + 1
            End If
            timeEnd(Stopwatch2)

            anow.AddSeconds(1)
            indexW = indexW + 1
        End While
        Console.WriteLine("count:" + indexW.ToString)
        Console.ReadKey()
    End Sub
    Sub timeStart(ByVal text As String, ByVal Stopwatch As Stopwatch)
        Console.WriteLine(text)
        Stopwatch.Start()
    End Sub
    Sub timeEnd(ByVal Stopwatch As Stopwatch)
        Stopwatch.Stop()
        Dim ts As Long = Stopwatch.ElapsedMilliseconds
        Console.WriteLine(ts.ToString + " Milliseconds")

    End Sub
End Class
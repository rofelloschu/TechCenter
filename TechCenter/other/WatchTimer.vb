'20130627
'�{���B�@�ɶ��p��
'Namespace tools
<ObsoleteAttribute("DLL�L�ɡA���sunskyLibrary", False)> _
Class watchTimer
    Private Stopwatch As Stopwatch
    Sub New()
        Stopwatch = New Stopwatch
    End Sub
    Sub timeStart()

        Stopwatch.Reset()
        Stopwatch.Start()
    End Sub
    Sub timeStart(ByVal text As String)
        M_WriteLineMaster.WriteLine(text)
        Me.timeStart()
    End Sub
    Function timeStop() As Long
        Stopwatch.Stop()
        M_WriteLineMaster.WriteLine(Stopwatch.ElapsedMilliseconds.ToString + " Milliseconds")
        Return Stopwatch.ElapsedMilliseconds
    End Function
    'Function timeStopString() As String
    '    Stopwatch.Stop()
    '    'Console.WriteLine(Stopwatch.ElapsedMilliseconds.ToString + " Milliseconds")
    '    Return Stopwatch.ElapsedMilliseconds
    'End Function
End Class
'End Namespace


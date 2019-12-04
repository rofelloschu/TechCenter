'20180510
Imports System.Threading
Module M_log
    Public debug_log As Boolean = True
    Private AutoResetEvent As AutoResetEvent = New AutoResetEvent(True)
    Sub writeline(path As String, contents As String)
        AutoResetEvent.WaitOne()
        System.IO.File.AppendAllText(path, Now.ToString("u") + " " + contents + vbCrLf)
        AutoResetEvent.Set()
    End Sub
    Sub writeline(path As String, contents() As String)
        AutoResetEvent.WaitOne()
        System.IO.File.AppendAllText(path, Now.ToString("u") + vbCrLf)
        For index As Integer = 0 To contents.Length - 1
            System.IO.File.AppendAllText(path, contents(index) + vbCrLf)
        Next
        AutoResetEvent.Set()
    End Sub

    Sub writeline(path As String, contents As String, encoding As System.Text.Encoding)
        AutoResetEvent.WaitOne()
        System.IO.File.AppendAllText(path, Now.ToString("u") + " " + contents + vbCrLf, encoding)
        AutoResetEvent.Set()
    End Sub
    Sub writeline(path As String, contents() As String, encoding As System.Text.Encoding)
        AutoResetEvent.WaitOne()
        System.IO.File.AppendAllText(path, Now.ToString("u") + vbCrLf, encoding)
        For index As Integer = 0 To contents.Length - 1
            System.IO.File.AppendAllText(path, contents(index) + vbCrLf, encoding)
        Next
        AutoResetEvent.Set()
    End Sub
    Public Sub writebyte(path As String, ByVal data() As Byte)
        AutoResetEvent.WaitOne()
        Dim text As String = ""
        For i As Integer = 0 To data.Length - 1
            text = text + data(i).ToString("X2")
        Next
        System.IO.File.AppendAllText(path, Now.ToString("u") + " " + text + vbCrLf)
        AutoResetEvent.Set()
    End Sub
End Module

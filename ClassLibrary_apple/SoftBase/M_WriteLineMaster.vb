'20120617
Imports System.IO
Imports System.Threading 

'<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Public Module M_WriteLineMaster
    Private TotalBytes As Long
    Private t_WriteLine As Thread
    Private t_Write As Thread
    Private AutoResetEvent As AutoResetEvent = New AutoResetEvent(True)
    Private AutoResetEvent2 As AutoResetEvent = New AutoResetEvent(True)

    Sub New()
        TotalBytes = 0

    End Sub
    Sub WriteLine(ByVal text_o As String)
        AutoResetEvent2.WaitOne()
        t_WriteLine = New Thread(AddressOf M_WriteLineMaster.WriteLine)
        t_WriteLine.Start(text_o)
        AutoResetEvent2.Set()
    End Sub
    Sub Write(ByVal text_o As String)
        AutoResetEvent2.WaitOne()
        t_Write = New Thread(AddressOf M_WriteLineMaster.Write)
        t_Write.Start(text_o)
        AutoResetEvent2.Set()
    End Sub
    Private Sub WriteLine(ByVal text_o As Object)
        AutoResetEvent.WaitOne()
        Try
            Dim text As String = text_o
            checkByte()
            TotalBytes = TotalBytes + text.Length
            Console.WriteLine(text)
        Catch ex As Exception
            M_catchException.printExceptionInFile("WriteLineMaster.WriteLine", ex)
        End Try
        AutoResetEvent.Set()
    End Sub
    Private Sub Write(ByVal text_o As Object)
        AutoResetEvent.WaitOne()
        Try
            Dim text As String = text_o
            checkByte()
            TotalBytes = TotalBytes + text.Length
            Console.Write(text)
        Catch ex As Exception
            M_catchException.printExceptionInFile("WriteLineMaster.Write", ex)
        End Try
        AutoResetEvent.Set()
    End Sub
    Sub checkByte()
        If TotalBytes > 1000000000 Then
            ' Close previous output stream and redirect output to standard output.
            Console.Out.Close()
            Dim sw = New StreamWriter(Console.OpenStandardOutput())
            sw.AutoFlush = True
            Console.SetOut(sw)
        End If
    End Sub
End Module

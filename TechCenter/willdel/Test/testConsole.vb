Imports System.Threading
Imports System.IO
Class testConsole
    Private textbig As String
    Private texto As String
    Private t As Thread
    Private tt As watchTimer
    Private time As Long
    Private tcount As Integer
    Sub New(ByVal text As String, ByVal count As Integer)
        textbig = text
        texto = text
        tcount = count
        tt = New watchTimer
        For index As Integer = 0 To tcount
            textbig = textbig + textbig
        Next
    End Sub
    Sub start()
        t = New Thread(AddressOf show)
        t.Start()
    End Sub
    Sub show()


        tt.timeStart()
        For index As Integer = 0 To 9
            Console.WriteLine(textbig)
        Next
        time = tt.timeStop()
        showtime()
    End Sub
    Sub showtime()
        Console.WriteLine("")
        Console.WriteLine("testConsole: " + texto + "   " + time.ToString + " Milliseconds")
        File.AppendAllText("testConsole.txt", Now.ToString + "testConsole: " + texto + "   " + time.ToString + " Milliseconds" + Environment.NewLine)
    End Sub
End Class
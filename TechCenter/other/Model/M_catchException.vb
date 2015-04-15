'20140612
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Public Module M_catchException
    Private ExceptionLog As APFile
    Private fileDate As DateTime
    Private FilePath As String
    Private output_mutex As System.Threading.Mutex = New System.Threading.Mutex(False)
    Public isTest As Boolean = False
    Sub New()
        fileDate = Now
        FilePath = "ExceptionLog" + fileDate.Year.ToString + "-" + fileDate.Month.ToString("D2") + "-" + fileDate.Day.ToString("D2") + ".txt"
        ExceptionLog = New APFile(FilePath)
    End Sub

    Public Sub printExceptionInFile(ByVal Exception As Exception)


        checktime()
        'System.IO.File.AppendAllText(FilePath, getNow() + " " + Exception.ToString + Environment.NewLine)
        ExceptionLog.write(getNow() + " " + Exception.Message.ToString)


    End Sub
    Public Sub printExceptionInFile(ByVal exName As String, ByVal Exception As Exception)

        checktime()

        ExceptionLog.write(getNow() + " " + exName + " " + Exception.Message.ToString)
        ' System.IO.File.AppendAllText(FilePath, getNow() + " " + exName + " " + Exception.ToString + Environment.NewLine)

    End Sub
    Private Function getNow() As String

        Return Now.Year.ToString + "-" + Now.Month.ToString("D2") + "-" + Now.Day.ToString("D2") + "-" + Now.Hour.ToString("D2") + ":" + Now.Minute.ToString("D2") + ":" + Now.Second.ToString("D2")
    End Function
    Private Sub WritteNow()
        output_mutex.WaitOne()
        Try
            checktime()
            ExceptionLog.write(getNow())
            'System.IO.File.AppendAllText(FilePath, getNow() + Environment.NewLine)
        Catch ex As Exception

        Finally
            output_mutex.ReleaseMutex()
        End Try

    End Sub
    Public Sub exWritte(ByVal text As String)
        If Not isTest Then
            Exit Sub
        End If
        output_mutex.WaitOne()
        Try
            checktime()
            ExceptionLog.write(text)
            ' System.IO.File.AppendAllText(FilePath, text + Environment.NewLine)
        Catch ex As Exception
        Finally
            output_mutex.ReleaseMutex()
        End Try

    End Sub
    Private Sub checktime()
        If Not fileDate.Day.Equals(Now.Day) Then
            fileDate = Now
            FilePath = "ExceptionLog" + fileDate.Year.ToString + "-" + fileDate.Month.ToString("D2") + "-" + fileDate.Day.ToString("D2") + ".txt"
            ExceptionLog = New APFile(FilePath)
        End If
    End Sub
    Public Function getFilePath() As String
        FilePath = "ExceptionLog" + fileDate.Year.ToString + "-" + fileDate.Month.ToString("D2") + "-" + fileDate.Day.ToString("D2") + ".txt"
        Return FilePath
    End Function
End Module


'20150616
'Namespace IO
Public Class ExceptionWrite
    '  Private FilePath As String
    Private AutoResetEvent As System.Threading.AutoResetEvent = New System.Threading.AutoResetEvent(True)
    Private name As String
    Private fileDate As DateTime
    Private DirPath As String
    Private FullFilePath As String
    Sub New(ByVal t_path As String, ByVal t_name As String)
        Me.DirPath = t_path
        If Not System.IO.Directory.Exists(Me.DirPath) Then
            Try
                System.IO.Directory.CreateDirectory(Me.DirPath)
                '重覆確認
                If Not System.IO.Directory.Exists(Me.DirPath) Then
                    Throw New Exception("資料夾建立失敗")
                End If
            Catch ex As Exception
                Throw ex
            End Try

        End If
        Me.name = t_name
        FullFilePath = getFilePath()
    End Sub
    Sub writeLine(ByVal text As String)
        AutoResetEvent.WaitOne()
        checktime()
        System.IO.File.AppendAllText(FullFilePath, getNow() + " [" + Me.name + "] " + text + Environment.NewLine)
        AutoResetEvent.Set()
    End Sub
    Private Function getNow() As String

        Return Now.Year.ToString + "-" + Now.Month.ToString("D2") + "-" + Now.Day.ToString("D2") + "-" + Now.Hour.ToString("D2") + ":" + Now.Minute.ToString("D2") + ":" + Now.Second.ToString("D2")
    End Function
    Private Sub checktime()
        If Not fileDate.Day.Equals(Now.Day) Then
            fileDate = Now
            ' FilePath = "ExceptionLog" + fileDate.Year.ToString + "-" + fileDate.Month.ToString("D2") + "-" + fileDate.Day.ToString("D2") + ".txt"
            Me.FullFilePath = getFilePath()
        End If
    End Sub
    Private Function getFilePath() As String
        'Dim t_path As String
        '' FilePath = "ExceptionLog" + fileDate.Year.ToString + "-" + fileDate.Month.ToString("D2") + "-" + fileDate.Day.ToString("D2") + ".txt"
        't_path = Me.DirPath + Me.name + fileDate.Year.ToString + "-" + fileDate.Month.ToString("D2") + "-" + fileDate.Day.ToString("D2") + ".txt"
        Return Me.DirPath + Me.name + "-" + fileDate.Year.ToString + "-" + fileDate.Month.ToString("D2") + "-" + fileDate.Day.ToString("D2") + ".txt"
    End Function
End Class
'End Namespace

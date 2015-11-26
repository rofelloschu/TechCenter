Imports System.Threading
Namespace Test
    Public Module M_test
   

        Private t_onlyCmd As Thread
        Private cmdString As String
        Private ResultCmdString As String
        Public Sub thread_Cmd(ByVal Command As String)
            If t_onlyCmd Is Nothing OrElse Not t_onlyCmd.IsAlive Then
                Console.WriteLine(Now.ToString)
                t_onlyCmd = New Thread(AddressOf AddressOf_Cmd)
                t_onlyCmd.Start(Command)
            End If

        End Sub
        Public Sub AddressOf_Cmd(ByVal Command As String)

            ResultCmdString = Cmd(Command)
            Console.WriteLine(ResultCmdString)
        End Sub
        Public Function Cmd(ByVal Command As String) As String
            Dim process As New System.Diagnostics.Process()
            process.StartInfo.FileName = "cmd.exe"
            process.StartInfo.UseShellExecute = False
            process.StartInfo.RedirectStandardInput = True
            process.StartInfo.RedirectStandardOutput = True
            process.StartInfo.RedirectStandardError = True
            process.StartInfo.CreateNoWindow = True
            process.Start()
            process.StandardInput.WriteLine(Command)
            process.StandardInput.WriteLine("exit")
            Dim Result As String = process.StandardOutput.ReadToEnd()
            process.Close()
            Return Result
        End Function
        Sub test001()
            MsgBox(Environment.Version.ToString())
        End Sub
        Sub test002()
            'If FolderBrowserDialog1.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            '    '   Dim DirectorySPath As String() = System.IO.Directory.GetDirectories(FolderBrowserDialog1.SelectedPath)
            '    Dim DirectorySPath As New System.IO.DirectoryInfo(FolderBrowserDialog1.SelectedPath)
            '    '(FolderBrowserDialog1.SelectedPath)
            '    If DirectorySPath.GetDirectories.Length = 0 Then
            '        Exit Sub
            '    End If
            '    Dim DinfoR2 As System.IO.DirectoryInfo() = DirectorySPath.GetDirectories
            '    Dim tFileS As System.IO.FileInfo()
            '    For index As Integer = 0 To DinfoR2.Length - 1
            '        If DinfoR2(index).GetFiles.Length = 0 Then
            '            Continue For
            '        End If
            '        tFileS = DinfoR2(index).GetFiles
            '        For index2 As Integer = 0 To tFileS.Length - 1
            '            If tFileS(index2).Name = "Static.cfg" Then
            '                work(DinfoR2(index).Name, DinfoR2(index).FullName, tFileS(index2).Name)
            '            End If
            '        Next
            '    Next
            'End If
        End Sub
        Sub work(ByVal number As String, ByVal path As String, ByVal filename As String)
            'Dim file As New tools.file.APFile(path, filename)
            'Try
            '    file.writeKey("Address,", number)
            '    file.writeKey("GPRSSrvPort,", (CInt(number) - 10000).ToString)
            'Catch ex As Exception

            'End Try

        End Sub
       
        
        Sub OpenDir(dirPath As String)
            Try
                System.Diagnostics.Process.Start("explorer.exe", dirPath)
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try

        End Sub
        Function IsIP(ByVal IP As String) As Boolean
            Return System.Text.RegularExpressions.Regex.IsMatch(IP, "\b((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$\b")
        End Function
    End Module

End Namespace


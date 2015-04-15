Imports System.Threading
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Module M_test
    Sub a()
        MsgBox(Convert.ToString(Convert.ToInt32("1111", 2))) '//2進制轉10進制
        MsgBox(Convert.ToString(Convert.ToInt32("11", 8))) '//8進制轉10進制
        MsgBox(Convert.ToString(Convert.ToInt32("0XFF", 16))) '//16進制轉10進制
    End Sub

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
    Sub allRandom()
        Dim Counter As Random = New Random(Guid.NewGuid().GetHashCode())
        Console.WriteLine(Counter.Next(0, 10))

    End Sub
    Function chrToHexString(text As String) As String
        Dim bytesString As String() = text.Split(" ")
        Dim stringbyteslist As New List(Of Byte)
        For index As Integer = 0 To bytesString.Length - 1
            Try
                stringbyteslist.Add(Convert.ToInt32("0X" + bytesString(index), 16))
            Catch ex As Exception
                Console.WriteLine("err " + bytesString(index))
                stringbyteslist.Add(0)
            End Try

        Next
        Return System.Text.Encoding.ASCII.GetString(stringbyteslist.ToArray)
    End Function
    Function HexStringToChr(text As String)
        Dim stringbytes As Byte() = System.Text.Encoding.ASCII.GetBytes(text)
        Dim bytesString As String = ""
        For index As Integer = 0 To stringbytes.Length - 1
            If index = 0 Then
                bytesString = stringbytes(index).ToString("x2")
            Else
                bytesString = bytesString + " " + stringbytes(index).ToString("x2")
            End If

        Next
        Return bytesString
    End Function
End Module

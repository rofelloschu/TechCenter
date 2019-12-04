'20190816
Public Class appDiary
    Private FileName As String
    Private appName As String
    Private myProcess As System.Diagnostics.Process
    Private timer As System.Threading.Timer
    Sub New()
        'Console.WriteLine(My.Application.Info.AssemblyName)
        'Console.WriteLine(My.Application.Info.DirectoryPath)
        'Console.WriteLine(My.Application.Info.WorkingSet.ToString)
        'Console.WriteLine(My.Application.Info.ProductName.ToString)
        'Console.WriteLine(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName)
        '  Environment.TickCount.ToString()
        Me.myProcess = System.Diagnostics.Process.GetCurrentProcess()
        Me.appName = Me.myProcess.MainModule.ModuleName

        FileName = "appDiary.log"
        startwite()
        timer = New System.Threading.Timer(AddressOf AddressOf_timer, Nothing, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
        'timer.Change(0, 3600000)
        timer.Change(0, 60000) 'test
        'timer.Change(60000, 60000)
    End Sub

    Sub AddressOf_timer(ByVal state As Object)
        Me.DiaryWrite(Now.ToString("u") + " - " + Me.appName)
        'Dim myProcess As System.Diagnostics.Process

        ' myProcess = System.Diagnostics.Process.()
        ''If System.Diagnostics.Process.GetProcessesByName(Me.appName).Length > 0 Then
        ''    myProcess = System.Diagnostics.Process.GetProcessesByName(Me.appName)(0)
        'https://csharpkh.blogspot.com/2018/07/CSharp-Inheritance-Polymorphism-Override-New-Memory.html
        ''End If
        'myProcess = System.Diagnostics.Process.GetCurrentProcess()
        'Me.DiaryWrite("PrivateMemorySize64 " + myProcess.PrivateMemorySize64.ToString)
        'Me.DiaryWrite("PrivateMemorySize64 " + String.Format("{0:N0}KB", myProcess.PrivateMemorySize64 / 1024))
        'Me.DiaryWrite("PeakWorkingSet64 " + myProcess.PeakWorkingSet64.ToString)
        'Me.DiaryWrite("PeakWorkingSet64 " + String.Format("{0:N0}KB", myProcess.PeakWorkingSet64 / 1024))
        ''Me.DiaryWrite("PagedMemorySize64 " + myProcess.PagedMemorySize64.ToString)
        ''Me.DiaryWrite("PagedSystemMemorySize64 " + myProcess.PagedSystemMemorySize64.ToString)
        ''Me.DiaryWrite("PeakVirtualMemorySize64 " + myProcess.PeakVirtualMemorySize64.ToString)
        ''Me.DiaryWrite("PeakPagedMemorySize64 " + myProcess.PeakPagedMemorySize64.ToString)
        'Me.DiaryWrite("WorkingSet " + My.Application.Info.WorkingSet.ToString)
        'Me.DiaryWrite("WorkingSet " + String.Format("{0:N0}KB", My.Application.Info.WorkingSet / 1024))
        'Dim TotalMemory As Long = GC.GetTotalMemory(False)
        '' Me.DiaryWrite("GetTotalMemory " + TotalMemory.ToString)
        'Me.DiaryWrite("GetTotalMemory " + String.Format("{0:N0}KB", TotalMemory / 1024))
        'Me.DiaryWrite("GetTotalMemory " + String.Format("{0:N0}MB", (TotalMemory / 1024) / 1024))
        'Me.DiaryWrite(myProcess..ToString)
        DiaryWriteMemory()
    End Sub
    Private Sub startwite()
        Me.DiaryWrite(Now.ToString("u") + " start")
        'DiaryWriteMemory()
    End Sub
    Private Sub DiaryWriteMemory()
        Dim TotalMemory As Long = GC.GetTotalMemory(False)

        Me.DiaryWrite("GetTotalMemory " + String.Format("{0:N0}KB", TotalMemory / 1024))
        Me.DiaryWrite("GetTotalMemory " + String.Format("{0:N0}MB", (TotalMemory / 1024) / 1024))
        'https://www.itread01.com/article/1479437721.html
        'https://dotblogs.com.tw/abbee/2014/11/24/147403
        Dim pf1 As New PerformanceCounter("Process", "Working Set - Private", Me.myProcess.ProcessName)
        'Dim pf2 As New PerformanceCounter("Process", "Working Set", Me.myProcess.ProcessName)
        'Dim ps As System.Diagnostics.Process = System.Diagnostics.Process.GetCurrentProcess()
        'Me.DiaryWrite("GetTotalMemory " + String.Format("{0}:{1} {2:N}KB", ps.ProcessName, "工作集(程序類)", ps.WorkingSet64 / 1024))

        'Try
        '    Me.DiaryWrite("GetTotalMemory " + String.Format("{0}:{1} {2:N}KB", ps.ProcessName, "工作集 ", pf2.NextValue() / 1024))

        'Catch ex As Exception

        'End Try
        Try
            'Me.DiaryWrite("GetTotalMemory " + String.Format("{0}:{1} {2:N}KB", Me.myProcess.ProcessName, "私人工作集 ", pf1.NextValue() / 1024))
            Me.DiaryWrite("GetTotalMemory " + String.Format("{0}:{1} {2:N}KB", Me.myProcess.ProcessName, "Working Set - Private", pf1.NextValue() / 1024))
        Catch ex As Exception

        End Try
    End Sub
    Private Sub DiaryWrite(ByVal data As String)
        Dim Filepath As String = System.IO.Directory.GetCurrentDirectory & "\" + FileName
        ' If System.IO.File.Exists(Filepath) Then
        Try
            Using sw As System.IO.StreamWriter = New System.IO.StreamWriter(Filepath, True, System.Text.Encoding.Default)
                sw.WriteLine(data.ToString())
                sw.Close()
            End Using
        Catch ex As Exception

        End Try
        'Else

        'End If

    End Sub


End Class

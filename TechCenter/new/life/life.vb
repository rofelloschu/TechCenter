Imports System.Threading
Public Class life
    Private life_timer As Timer
    Private life_value As Long
    Sub New()
        Me.onlyone()
        Me.life_timer = New Threading.Timer(AddressOf life_end, Nothing, Timeout.Infinite, Timeout.Infinite)
        life_value = 30000
        Me.life_timer.Change(life_value, Timeout.Infinite)
    End Sub
    Sub reset_Life(value As Long)
        Me.life_timer.Change(value, Timeout.Infinite)
    End Sub
    Private Sub onlyone()
        'Console.WriteLine(System.Diagnostics.Process.GetCurrentProcess().ProcessName)
        Dim allProcess() As System.Diagnostics.Process = System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName)

        Dim path As String = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName
        'Dim path As String = System.AppDomain.CurrentDomain.BaseDirectory + System.AppDomain.CurrentDomain.FriendlyName
        'ErrlogFile.Write("onluone path " + path)
        For index As Integer = 0 To allProcess.Length - 1
            If allProcess(index).MainModule.FileName() = path Then
                If allProcess(index).StartTime.Equals(System.Diagnostics.Process.GetCurrentProcess().StartTime) Then
                    'ErrlogFile.Write("onluone Process same " + allProcess(index).StartTime.ToString("u"))

                Else
                    'ErrlogFile.Write("kill Process " + allProcess(index).StartTime.ToString("u"))
                    allProcess(index).Kill()
                    'ErrlogFile.Write("kill Process")
                End If
            End If
        Next
        'If System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1 Then
        '    Dim myProces As System.Diagnostics.Process = System.Diagnostics.Process.GetCurrentProcess
        '    myProces.Kill()
        'End If
    End Sub
    Overridable Sub life_end_aften()

    End Sub
    Sub life_end(stateInfo As Object)
        Try
            life_end_aften()

        Catch ex As Exception

        Finally
            System.Diagnostics.Process.GetCurrentProcess().Kill()
            'Dim myProces As System.Diagnostics.Process = System.Diagnostics.Process.GetCurrentProcess
            'myProces.Kill()
        End Try
    End Sub
End Class

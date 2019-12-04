'20150526
Imports System.Threading
Public Class threadList
    Private Threadlist As List(Of Thread)
    Private plist As List(Of Object)
    Private Thread_run As Thread
    Private m_deleytime As Integer
    Private IsRun As Boolean
    Protected AutoResetEvent As New System.Threading.AutoResetEvent(True)
    Sub New(t_deleytime As Integer)
        Threadlist = New List(Of Thread)
        plist = New List(Of Object)
        m_deleytime = t_deleytime
        IsRun = True
        Thread_run = New Thread(AddressOf AddressOf_Run)
        Thread_run.Start()
    End Sub
    Sub close()
        IsRun = False
    End Sub
    Sub addThread(t_thread As System.Threading.Thread, t_o As Object)
        AutoResetEvent.WaitOne()
        Threadlist.Add(t_thread)
        plist.Add(t_o)
        AutoResetEvent.Set()
    End Sub
    Protected Sub AddressOf_Run()
        While IsRun
            System.Threading.Thread.Sleep(m_deleytime)
            AutoResetEvent.WaitOne()
            If Threadlist.Count > 0 Then
                Threadlist(0).Start(plist(0))
                Threadlist.RemoveAt(0)
                plist.RemoveAt(0)
            End If
            AutoResetEvent.Set()
        End While
    End Sub
End Class

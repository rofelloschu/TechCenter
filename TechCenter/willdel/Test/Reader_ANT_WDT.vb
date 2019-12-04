Imports System.Threading
Public Class Reader_ANT_WDT
    Private status As Boolean
    'false 正常
    'true 異常
    Private t_thread As Thread
    Private time_min As Integer
    Private nextTime As DateTime
    Sub New(t_time_min As Integer)
        time_min = t_time_min
        nextTime = Now
        nextTime = nextTime.AddMinutes(time_min)
        status = False
        t_thread = New Thread(AddressOf AddressOf_Thread)
        t_thread.Start()
    End Sub
    Sub close()
        Try
            t_thread.Abort()
        Catch ex As Exception

        End Try
    End Sub
    Sub Set_getData()
        'status = False
        setStatus(False)
        nextTime = Now.AddMinutes(time_min)
    End Sub
    Sub AddressOf_Thread()
        While True
            Thread.Sleep(1000)

            If Now > nextTime Then
                'status = True
                setStatus(True)
                nextTime = Now.AddMinutes(time_min)
            End If
        End While


    End Sub
    Sub setStatus(value As Boolean)
        If Me.status <> value Then
            Me.status = value
            RaiseEvent event_Status(value)
        End If
    End Sub
    Event event_Status(value As Boolean)
End Class

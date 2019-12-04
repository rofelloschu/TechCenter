'20180517
Public Class sleep
    Private sleep_time As Integer
    Private t_sleep As Threading.Thread
    Sub New(t_sleep_time)
        sleep_time = t_sleep_time
    End Sub
    Sub sleep()

    
        'If t_sleep IsNot Nothing Then
        '    t_sleep.Abort()
        'End If
        t_sleep = New Threading.Thread(AddressOf AddressOf_sleep)
        t_sleep.Start()
    End Sub
    Sub AddressOf_sleep()
        System.Threading.Thread.Sleep(sleep_time)
    End Sub
    Sub wakeup()
        If t_sleep Is Nothing Then
            Exit Sub
        End If
        Try
            t_sleep.Abort()
        Catch ex As Exception

        End Try
    End Sub

    Function isSleep() As Boolean
        If t_sleep Is Nothing Then
            Return False
        End If
        Return t_sleep.IsAlive
    End Function
End Class

'將時間呼叫抽出 與功能分開  類似排程
Imports System.Threading
Public Class cycle_min
    Public sleep_ms As Integer
    Event Mcall()
    Public t_cycle_sleep As Thread
    Public c_min As Integer
    Public c_min_enable As Boolean
    Sub New()
        c_min = 0
        c_min_enable = False
        sleep_ms = 1000

    End Sub
    Sub start()
        Try
            t_cycle_sleep.Abort()
        Catch ex As Exception

        End Try
        t_cycle_sleep = New Thread(AddressOf run)
        t_cycle_sleep.Start()
    End Sub
    Sub close()
        Try
            t_cycle_sleep.Abort()
        Catch ex As Exception

        End Try
    End Sub
    Sub run()
        While True
            System.Threading.Thread.Sleep(sleep_ms)
            If c_min <> 0 Then
                If Now.Minute Mod c_min = 0 Then
                    If c_min_enable Then
                        RaiseEvent Mcall()
                        c_min_enable = False
                    End If

                Else
                    c_min_enable = True
                End If
            End If
        End While
    End Sub
End Class

'將時間呼叫抽出 與功能分開  類似排程
Imports System.Threading
Public Class cycle_min
    Implements if_call

    Public sleep_ms As Integer
    Event Mcall() Implements if_call.Mcall
    Public t_cycle_sleep As Thread
    Private t_run_enable As Boolean

    Public c_min As Integer
    Private c_min_enable As Boolean
 
    Sub New()
        c_min = 0
        c_min_enable = False
        sleep_ms = 1000

    End Sub
    Sub start() Implements if_call.strat
        Try
            t_cycle_sleep.Abort()
        Catch ex As Exception

        End Try
        t_run_enable = True
        t_cycle_sleep = New Thread(AddressOf run)
        t_cycle_sleep.Start()
    End Sub
    Sub close() Implements if_call.close
        Try
            't_cycle_sleep.Abort()
            t_run_enable = False
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub run()
        While t_run_enable
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
#Region "test"
    Private isTest As Boolean = False
    Private testcell As Boolean = False
    Public Sub test_start1() Implements if_call.test_start
        If isTest Then
            RaiseEvent_test_text(Now.ToString("u") + " test  running")
            Exit Sub
        End If

        isTest = True
        RaiseEvent_test_text(Now.ToString("u") + " test start")
        AddHandler Me.Mcall, AddressOf test_cell
        Me.start()


    End Sub

    Protected Sub test_cell()

        Try
            RaiseEvent_test_text(Now.ToString("u") + " test cell")
            'testcell = False
            Me.close()
            RaiseEvent_test_text(Now.ToString("u") + " test end")
            isTest = False
        Catch ex As Exception
            RaiseEvent_test_text(Now.ToString("u") + " test test_cell err")
            isTest = False
        End Try

    End Sub
    Protected Sub RaiseEvent_test_text(text As String)
        If isTest Then
            RaiseEvent test_text(text)
        End If
    End Sub
    Public Event test_text(text As String) Implements if_call.test_text
#End Region

End Class

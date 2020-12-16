Imports System.Threading
'每天
Public Class morningcall
    Implements if_call


    Public Event Mcall() Implements if_call.Mcall
    Public sleep_ms As Integer

    Public t_run As Thread
    Private t_run_enable As Boolean

    Public call_time As DateTime

    Private call_enable As Boolean
    Sub New()
        call_enable = False
        sleep_ms = 1000
        t_run_enable = True
    End Sub

    Sub start() Implements if_call.strat
        Try
            If t_run IsNot Nothing Then
                t_run.Abort()
            End If

        Catch ex As Exception

        End Try
        t_run_enable = True
        t_run = New Thread(AddressOf run)
        t_run.Start()
    End Sub
    Sub close() Implements if_call.close
        Try
            t_run_enable = False
            't_run.Abort()
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub
    Protected Sub run()


        Try
            While t_run_enable
                System.Threading.Thread.Sleep(sleep_ms)
                If Now > call_time Then
                    RaiseEvent Mcall()
                    call_time = call_time.AddDays(1)
                End If
            End While
        Catch ex As Exception

        End Try

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

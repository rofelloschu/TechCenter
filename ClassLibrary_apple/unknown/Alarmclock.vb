'20151126
Imports System.Threading
Public Class Alarmclock
    Private thread_alarm As Thread
    Private thread_alarm_e As Boolean
    Private p_startTime As DateTime
    Private p_isAlarmStart As Boolean
    Private p_cycleSec As Long
    Private p_isStart As Boolean
#Region "newAndClose"
    Sub New()
        thread_alarm_e = True
        p_isAlarmStart = False
        p_isStart = False
        '預設
        p_startTime = Now
        p_cycleSec = 1
    End Sub
    Sub start()
        If p_isStart Then
            Exit Sub
        End If
        p_isStart = True
        thread_alarm = New Thread(AddressOf AddressOf_alarm)
        thread_alarm.Start()
    End Sub
    Sub close()
        thread_alarm_e = False
        Try
            If thread_alarm IsNot Nothing Then
                thread_alarm.Abort()
            End If

        Catch ex As Exception

        End Try
        GC.Collect()
    End Sub
#End Region

#Region "set"
    Public Sub setCycleTime(startTime As DateTime, cycleSec As Long)
        If p_isStart Then
            Exit Sub
        End If
        If cycleSec = 0 Then
            Exit Sub
        End If
        Me.p_startTime = startTime
        Me.p_cycleSec = cycleSec
    End Sub
#End Region
#Region "get"
    Public Function getstartTime() As DateTime
        Return Me.p_startTime
    End Function
    Public Function getcycleSec() As Long
        Return Me.p_cycleSec
    End Function
    Public Function isStart() As Boolean
        Return Me.p_isStart
    End Function
#End Region
#Region "core"
    Event event_alarm()
    Sub AddressOf_alarm()
        Dim nextTime As DateTime = Now
        While thread_alarm_e
            Thread.Sleep(1000)
            If Not p_isAlarmStart Then
                If Now > Me.p_startTime Then
                    Me.p_isAlarmStart = True
                    nextTime = getNextTime(Me.p_startTime)
                End If
                Continue While
            End If
            If Now > nextTime Then
                nextTime = getNextTime(nextTime)
                RaiseEvent event_alarm()
            End If
        End While
    End Sub
    Private Function getNextTime(nowTime As DateTime) As DateTime
        Return nowTime.AddSeconds(Me.p_cycleSec)
    End Function
#End Region


End Class

Imports System.Threading
Imports Microsoft.VisualBasic.Devices
Class MyTimer
    '程式內定時間
    '需Imports Microsoft.VisualBasic.Devices
    'Imports System.Threading

    'Private year As Integer
    'Private month As Integer
    'Private day As Integer
    'Private hour As Integer
    'Private min As Integer
    'Private seconds As Integer

    Private nowTime As Date
    Private systemClock As New Clock
    Private listTime As New Dictionary(Of Integer, Integer)
    Private sTime As Integer
    Private nowTick As Integer
    Private lastTick As Integer
    Public autocolck As New Thread(AddressOf runTime)
    Private lTime As Integer

    Private isAlive As Boolean
    Sub New(ByVal i As Integer)
        sTime = 0
        setNowTime()
        nowTick = systemClock.TickCount + i

        lTime = 1000
        isAlive = True
    End Sub
    Sub runTime()
        While (isAlive)
            Thread.Sleep(1000)

            oneTime()
        End While
    End Sub
    Sub oneTime()
        lastTick = nowTick
        nowTick = systemClock.TickCount
        sTime = (nowTick - lastTick) + sTime
        Dim sTime_p As Integer = sTime \ lTime
        'If sTime < lTime Then
        '    lTime = lTime - 1
        '    ' Exit Sub
        'End If

        'If (sTime >= lTime) Then
        '    lTime = lTime + 1
        '    ' Exit Sub
        'End If
        If (sTime_p > 0) Then
            nowTime = nowTime.AddSeconds(sTime \ lTime)
            sTime = sTime Mod lTime
            lTime = lTime + sTime
            Exit Sub
        Else
            lTime = lTime - ((lTime - sTime) \ 2)
        End If
    End Sub
    Sub oneTime_2()
        lastTick = nowTick
        nowTick = systemClock.TickCount
        sTime = (nowTick - lastTick) + sTime
        If (sTime \ lTime > 0) Then
            nowTime = nowTime.AddSeconds(sTime \ lTime)
            sTime = sTime Mod lTime
        End If

    End Sub
    Sub setLlTime(ByVal i As Integer)
        lTime = i
    End Sub
    Function getLlTime() As Integer
        Return lTime
    End Function
    Sub setNowTime()
        nowTime = Now()
    End Sub
    Function getMyTime() As Date
        '?
        Return nowTime
    End Function

    Sub starTime(ByVal i As Integer)
        listTime.Add(i, systemClock.TickCount)

    End Sub
    Function endTime(ByVal i As Integer) As Integer
        'fix
        Dim j As Integer = 0
        If (listTime.ContainsKey(i)) Then
            j = systemClock.TickCount - listTime.Item(i)
        Else
        End If
        If (listTime.Remove(j)) Then
        Else
            j = 0
        End If
        Return j
    End Function
    Public Overrides Function ToString() As String
        Dim name As String
        name = MyBase.ToString.Substring(MyBase.ToString.Length - 4)
        Return name
    End Function

    Sub close()
        isAlive = False
    End Sub
End Class
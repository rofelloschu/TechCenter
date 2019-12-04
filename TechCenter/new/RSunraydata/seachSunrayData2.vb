Imports classLibrary_bang
Imports System.Threading
Public Class seachSunrayData2
    'Private m_willSeach_date As mutexList_V02(Of DateTime)
    'Private m_Seach_data As mutexList_V02(Of sunray_format_FFF9)



    'Private save_historySunrayData As save_historySunrayData
    Private guessSunrayData As guessSunrayData
    Private now_deviation As Integer

    Private savelog As Boolean = False
    Sub New()
        now_deviation = 0
        'm_willSeach_date = New mutexList_V02(Of DateTime)
        'm_Seach_data = New mutexList_V02(Of sunray_format_FFF9)
    End Sub


    Event readFFF9(ByVal data_fff9 As sunray_format_FFF9)
    Function ReceivedFFF9(ByVal data_fff9 As sunray_format_FFF9) As Boolean
        'file.write(Now.ToString("u") + "@" + data_fff9.to_string)

        '1
        Dim nowData As Boolean = False
        If Not nowData And Me.isDataNow(data_fff9.time, Now) Then
            'save_historySunrayData.addFFF9(data_fff9)
            'm_Seach_data.Add(data_fff9)
            'RaiseEvent readFFF9(data_fff9)
            nowData = True
        End If
        If Not nowData And Me.isDataNow(data_fff9.time.AddMinutes(-1), Now) Then
            'save_historySunrayData.addFFF9(data_fff9)
            'm_Seach_data.Add(data_fff9)
            'RaiseEvent readFFF9(data_fff9)
            nowData = True
        End If
        If Not nowData And Me.isDataNow(data_fff9.time.AddMinutes(1), Now) Then
            'save_historySunrayData.addFFF9(data_fff9)
            'm_Seach_data.Add(data_fff9)
            'RaiseEvent readFFF9(data_fff9)
            nowData = True
        End If
        'If nowData Then
        '    M_dubug.debugfile.Write("現在資料 " + data_fff9.time.ToString("u"))
        'End If
        '2
        If Not nowData And guessSunrayData IsNot Nothing Then

            If guessSunrayData.setReturnData(data_fff9) = guessresult.find Then
                'save_historySunrayData.addFFF9(data_fff9)
                'm_Seach_data.Add(data_fff9)
                RaiseEvent readFFF9(data_fff9)
            End If
            If savelog Then
                'M_dubug.debugfile.Write("收到資料 " + data_fff9.time.ToString("u"))
            End If

        End If
        Return Not nowData
    End Function
    Private Function isDataNow(ByVal data_time As DateTime, ByVal now_time As DateTime) As Boolean
        ''Dim isNowData As Boolean = True
        If (data_time.Minute = now_time.Minute) Then
            'isNowData = False
        Else

            Return False

        End If

        If (data_time.Hour = now_time.Hour) Then
            'isNowData = False
        Else
            Return False
        End If
        If (data_time.Day = now_time.Day) Then
            'isNowData = False
        Else
            Return False

        End If
        If (data_time.Month = now_time.Month) Then
            'isNowData = False
        Else
            Return False

        End If
        If (data_time.Year = now_time.Year) Then
            'isNowData = 
        Else
            Return False

        End If
        Return True

    End Function

    Event WriteFFF3Data(ByVal data() As Byte)
    Sub WriteFFF3(ByVal TargetDataTime As DateTime)

        'Dim TargetDataTime As DateTime = Now
        'Dim guessSunrayData As guessSunrayData = New guessSunrayData(now_deviation)
        guessSunrayData = New guessSunrayData(TargetDataTime, now_deviation, 10)
        'AutoResetEvent2.WaitOne()
        'guessSunrayData_list.Add(guessSunrayData)
        'AutoResetEvent2.Set()

        'Dim temp_count As Integer = now_deviation
        While guessSunrayData.getResult = guessresult.notfind
            Thread.Sleep(1000)
            If Now.Second = 59 Then
                '避免時間查錯
                Thread.Sleep(1000)
            End If
            Dim data As Byte() = guessSunrayData.getNewFFF3()
            RaiseEvent WriteFFF3Data(data)
            'Me.com.Write(data, 0, data.Length)
            'r2_file.write(Now.ToString + " send")
            'r2_file.write(data)

        End While
        If guessSunrayData.getResult = guessresult.close Then
            If savelog Then
                'M_dubug.debugfile.Write("close " + TargetDataTime.ToString("u"))
            End If

        End If
        If guessSunrayData.getResult = guessresult.nothings Then
            If savelog Then
                'M_dubug.debugfile.Write("找不到 " + TargetDataTime.ToString("u") + " count " + guessSunrayData.getdeviation.ToString)
            End If
        End If
        If guessSunrayData.getResult = guessresult.find Then
            If savelog Then
                'M_dubug.debugfile.Write("找到 " + TargetDataTime.ToString("u") + "  count " + guessSunrayData.getdeviation.ToString)
            End If
            'now_deviation = guessSunrayData.getdeviation
        End If
    End Sub
    Sub close()
        Try

        Catch ex As Exception

        End Try

    End Sub

    'Function getData() As sunray_format_FFF9
    '    If m_Seach_data.Count > 0 Then
    '        Return m_Seach_data.getFirstValue
    '    Else
    '        Return Nothing
    '    End If
    'End Function
End Class

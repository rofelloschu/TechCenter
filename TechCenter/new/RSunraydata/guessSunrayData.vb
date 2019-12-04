'20180423
Public Class guessSunrayData
    Private result As guessresult
     

    '要搜尋的時間
    Private m_seachtime As DateTime
    '誤差
    Private t_deviation_count As Integer
    '計算搜尋次數
    Private m_seach_maxcount As Integer '搜尋上限
    Private t_seach_totalcount As Integer


    Sub New(data_time As DateTime, t_deviation As Integer, t_seach_Maxcount As Integer)
        Me.m_seachtime = data_time
        t_deviation_count = t_deviation
        m_seach_maxcount = t_seach_Maxcount
        t_seach_totalcount = 0
        result = guessresult.notfind

    End Sub
  
    Function setReturnData(data As sunray_format_FFF9) As guessresult
        '過濾車道
        If data.LaneID <> 0 Then
            Return result
        End If
        If data.time.Year = 2000 OrElse data.time.Year > Now.AddYears(10).Year Then
            '搜尋失敗 重新設定
            t_deviation_count = 0
            result = guessresult.notfind
            Return result
        End If
        t_seach_totalcount = t_seach_totalcount + 1
        If data.time < m_seachtime Or data.time > m_seachtime Then
            If t_seach_totalcount >= m_seach_maxcount Then
                '判斷找不到
                result = guessresult.nothings
                Return result
            Else
                '還沒找到
                result = guessresult.notfind
                'Me.time = data.time
                '更新m_deviation_count 
                t_deviation_count = guessCount(data)
                Return result
            End If

        End If
        If data.time = m_seachtime Then
            '正確
            result = guessresult.find
            Return result
        End If

        Return result
    End Function
    Function getNewFFF3() As Byte()
        '從0開始
        Dim FFF3 As New sunray_format_FFF3
        Dim nowtime As DateTime = Now
        nowtime = nowtime.AddSeconds(-nowtime.Second)
        Dim TimeSpan As TimeSpan = nowtime - Me.m_seachtime
        Dim tempIndex_diff As Integer
        tempIndex_diff = TimeSpan.TotalMinutes
        tempIndex_diff = tempIndex_diff + t_deviation_count

        FFF3.setIndex(tempIndex_diff)
        Return FFF3.getdata
    End Function
    Function getResult() As guessresult
        Return result
    End Function

    Function getdeviation() As Integer
        Return t_deviation_count
    End Function
    Sub close()
        Me.result = guessresult.close
    End Sub
    Private Function guessCount(data As sunray_format_FFF9) As Integer
        Dim r_count As Integer = 0
        If Me.m_seachtime = data.time Then
            r_count = 0
        End If
        If Me.m_seachtime > data.time Then
            Dim TimeSpan As TimeSpan = Me.m_seachtime - data.time
            r_count = t_deviation_count - TimeSpan.TotalMinutes
        End If
        If Me.m_seachtime < data.time Then
            Dim TimeSpan As TimeSpan = data.time - Me.m_seachtime
            r_count = t_deviation_count + TimeSpan.TotalMinutes
        End If
        Return r_count
    End Function
End Class
Public Enum guessresult
    find
    notfind
    nothings
    'nowtime
    close
End Enum
Public Class historySunrayData_min
    Public laneData() As String
    Public time As DateTime
    Sub New()

    End Sub
    Function LaneNumber() As Integer
        If laneData Is Nothing Then
            Return 0
        Else
            Return laneData.Length
        End If
    End Function
    Sub setlaneData(t_datas As String())
 

        '加入車道資料
        'Dim t_list As New List(Of String)
        'If Me.LaneNumber > 0 Then
        '    t_list.AddRange(t_datas)
        'End If
        ''t_list.Add(t_data)
        laneData = t_datas
    End Sub
    Sub addlaneData(t_data As String)
        '檢查車道重複
        If laneData IsNot Nothing Then
            For index As Integer = 0 To laneData.Length - 1
                If get_String_LaneNumberString(t_data) = get_String_LaneNumberString(laneData(index)) Then
                    Exit Sub
                End If
            Next
        End If

        '加入車道資料
        Dim t_list As New List(Of String)
        If Me.LaneNumber > 0 Then
            t_list.AddRange(laneData)
        End If
        t_list.Add(t_data)
        laneData = t_list.ToArray
    End Sub
    Private Function get_String_LaneNumberString(data As String) As String
        Return data.Substring(14, 2)
    End Function
End Class

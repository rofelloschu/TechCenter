'vd資料存檔用
Public Class save_historySunrayData
    Public data_day_dict As Dictionary(Of DateTime, historySunrayData_day)

    Sub New()
        data_day_dict = New Dictionary(Of DateTime, historySunrayData_day)

    End Sub

    Sub addFFF9(t_data As sunray_format_FFF9)

        'Dim min_data As New historySunrayData_min
        'min_data.time = t_data.time
        'min_data.addlaneData(t_data.byteToString)


        Dim day_time As New DateTime(t_data.time.Year, t_data.time.Month, t_data.time.Day)
        If data_day_dict.ContainsKey(day_time) Then

            'data_dict(day_time) = min_data
            data_day_dict(day_time).addMinData(t_data.time, t_data.byteToString)
            data_day_dict(day_time).saveFile()
        Else
            Dim day_data As New historySunrayData_day(Me.getFileInfo(day_time))
            Me.addData(day_data.time, day_data)
            data_day_dict(day_data.time).addMinData(t_data.time, t_data.byteToString)
            data_day_dict(day_data.time).saveFile()
        End If

    End Sub
    Sub loadAllFile()
        'sensor_2018-03-29.txt
        Dim FileNames() As String = System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory)
        Dim sunrayFile_list As New List(Of System.IO.FileInfo)
        For index As Integer = 0 To FileNames.Length - 1
            Dim f_info As New System.IO.FileInfo(FileNames(index))
            If f_info.Name.Length <> 21 Then
                Continue For
            End If
            'MsgBox(f_info.Name)
            If Not f_info.Name.Substring(0, 6).Equals("sensor") Then
                Continue For
            End If
            'MsgBox(f_info.Name)
            sunrayFile_list.Add(f_info)
        Next

        For index2 As Integer = 0 To sunrayFile_list.Count - 1
            Dim t_day As New historySunrayData_day(sunrayFile_list(index2))

            t_day.loadFile()
            Me.addData(t_day.time, t_day)
        Next

    End Sub

    Private Function getFileName(Time As DateTime) As String
        Return "sensor_" + Time.ToString("yyyy-MM-dd") + ".txt"
    End Function

    Private Function getFileInfo(Time As DateTime) As System.IO.FileInfo
        Return New System.IO.FileInfo("sensor_" + Time.ToString("yyyy-MM-dd") + ".txt")
    End Function
    Private Sub addData(key As DateTime, value As historySunrayData_day)

        If data_day_dict.ContainsKey(key) Then
            data_day_dict(key) = value
        Else
            data_day_dict.Add(key, value)
        End If
    End Sub
End Class

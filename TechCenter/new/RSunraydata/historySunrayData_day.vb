Imports System.IO
Imports classLibrary_bang
Public Class historySunrayData_day
    Public data_min_dict As Dictionary(Of DateTime, historySunrayData_min)
    'Public data() As historySunrayData_min
    Public m_fileinfo As FileInfo
    Public time As DateTime
    Sub New(t_fileinfo As FileInfo)
        m_fileinfo = t_fileinfo
        'sensor_2018-04-02
        Dim year As Integer = CInt(m_fileinfo.Name.Substring(7, 4))
        Dim month As Integer = CInt(m_fileinfo.Name.Substring(12, 2))
        Dim day As Integer = CInt(m_fileinfo.Name.Substring(15, 2))
        time = New DateTime(year, month, day)
        data_min_dict = New Dictionary(Of DateTime, historySunrayData_min)

    End Sub

    Public Sub loadFile()
        '時間(min),車道@資料,車道@資料


        Dim file As New OnlyReadFile2(m_fileinfo.FullName)
        Dim data_string As String() = file.load()
        'Dim data() As historySunrayData_min
        'data = Nothing
        'ReDim data(data_string.Length - 1) 'as historySunrayData_min
        For index As Integer = 0 To data_string.Length - 1
            Dim c_data() As String = data_string(index).Split(",")

            Dim t_historySunrayData_min As New historySunrayData_min

            t_historySunrayData_min.time = Convert.ToDateTime(c_data(0)).AddHours(-8)
            'MsgBox(t_historySunrayData_min.time.ToString)
            Dim lanedata(c_data.Length - 2) As String
            For index2 As Integer = 0 To lanedata.Length - 1
                lanedata(index2) = c_data(index2 + 1).Split("@")(1)
            Next
            t_historySunrayData_min.setlaneData(lanedata)
            data_min_dict.Add(t_historySunrayData_min.time, t_historySunrayData_min)
        Next
    End Sub
    Public Sub saveFile()

        'If File.Exists(m_fileinfo.FullName) Then
        '    File.Delete(m_fileinfo.FullName)
        'End If

        Dim temp_text As String = ""
        Dim data() As historySunrayData_min = Me.getData
        Dim data_list As New List(Of String)
        For index As Integer = 0 To data.Length - 1
            temp_text = ""
            temp_text = data(index).time.ToString("u")
            '時間(min),車道@資料,車道@資料
            For index2 As Integer = 0 To data(index).LaneNumber - 1
                temp_text = temp_text + "," + index2.ToString + "@" + data(index).laneData(index2)
            Next
            data_list.Add(temp_text)
            'WriteFile.Writte(temp_text)
        Next
        Dim WriteFile As New OnlyWriteFile2(m_fileinfo.FullName)
        WriteFile.WriteAll(data_list.ToArray)
    End Sub

    Public Sub addMinData(t_time As DateTime, t_datastring As String)
        'Me.addData(t_time, t_data)
        If data_min_dict.ContainsKey(t_time) Then
            'data_dict(t_time) = t_data
            data_min_dict(t_time).addlaneData(t_datastring)
        Else

            Dim min_data As New historySunrayData_min
            min_data.time = t_time
            min_data.addlaneData(t_datastring)
            data_min_dict.Add(min_data.time, min_data)
        End If
    End Sub
#Region "dict"

    Private Function getData() As historySunrayData_min()
        Dim ValueColl As Dictionary(Of DateTime, historySunrayData_min).ValueCollection = data_min_dict.Values
        'keyColl.ToArray() 'test

        Dim ValueList As New List(Of historySunrayData_min)
        For Each s As historySunrayData_min In ValueColl
            ''Console.WriteLine("Key = {0}", s)
            ValueList.Add(s)
        Next s
        Return ValueList.ToArray
    End Function
    'Private Sub addData(key As DateTime, value As historySunrayData_min)

    '    If data_dict.ContainsKey(key) Then
    '        data_dict(key) = value
    '    Else
    '        data_dict.Add(key, value)
    '    End If
    'End Sub
#End Region
End Class

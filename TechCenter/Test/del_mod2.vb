'Public Class edit_del_set
'    '1名稱
'    Public del_file_path As String
'    Public del_dir_path As String
'    Public del_dir_keyword As String
'    '2刪除條件
'    Public if_type As String
'    Public if_value As String
'    'time
'    '3刪除執行時間
'    Public count As String '執行次數
'    Public time_value As String
'    Public time_type As String
'    '循環cycle min 10
'    '每天day 1440
'    '定時timing 20200431

'    Public err As String
'End Class
'1.撰寫log
'2.預設值
'3.狀態
'20200429
Public Class del_mod2

    Public index As String
    Public name As String
    Public enable As String
    Public file_type_name As String
    'file
    'dir
    Public file_path As String
    Public file_type_value As String

    Public file_delcount As String
    Public Timer_name As String
    '每天定時 timing
    '週期分 cycle min 
    Public Timer_value As String



    Public status_text As String
    Private Timer_fun As if_call
    Public err As Boolean
    Event log_text(text As String)
    'Private delete_file As delete_file

    Public D_File As IF_info
    Public D_Time As IF_info
    Sub New()
        'delete_file = New delete_file
    End Sub
    Sub init()

    End Sub
    'Sub stop_()
    '    If enable = False Then
    '        Exit Sub
    '    End If
    'End Sub
    Function displayForm_text() As String
        Return Me.index + "," + enable.ToString + "," + Me.name
    End Function
    Sub close()
        Write_Status("mod " + name + " close")
        'RaiseEvent log_text("mod " + name + " close")
        If Timer_fun IsNot Nothing Then
            Timer_fun.close()
        End If

    End Sub
    Sub check_err()

    End Sub
    Protected Sub Write_Status(Text As String)
        Me.status_text = Now.ToString("u") + " " + Text
        RaiseEvent log_text(Text)
    End Sub

#Region "log"

#End Region
#Region "file"
    Sub testdel()
        Write_Status("mod " + name + " testdel")
        'RaiseEvent log_text("mod " + name + " testdel")
        Select Case file_type_name
            Case "F001"
                F001()
                'Case File_002.name
                '    F002()
            Case "F003"
                F003()
            Case "F999"

                'Case Else
                '    F001()
        End Select
    End Sub
    Protected Sub AddressOf_del()
        'RaiseEvent log_text("mod " + name + " del")
        Write_Status("mod " + name + " del")
        Select Case file_type_name
            Case "F001"
                F001()
                'Case File_002.name
                '    F002()
            Case "F003"
                F003()
            Case "F999"

                'Case Else
                '    F001()
        End Select


    End Sub
    Protected Sub AddressOf_del2()


        Try
            If Not System.IO.Directory.Exists(file_path) Then
                Write_Status("路徑不存在 " + file_path)
                Exit Sub
            End If
            'Select Case aa

            'End Select
            DirectCast(D_File, IF_delFile).max_del_count = 1
            Select Case True
                Case TypeOf D_File Is File_001
                    Dim files_info() As System.IO.FileInfo = DirectCast(D_File, File_001).getFiles(file_path, file_type_value)
                    DirectCast(D_File, File_001).del_file(file_type_value, files_info)

                Case TypeOf D_File Is File_003

                    Dim dir_info() As System.IO.DirectoryInfo = DirectCast(D_File, File_003).getFiles(file_path, file_type_value)
                    DirectCast(D_File, File_003).del_file(file_type_value, dir_info)


            End Select


        Catch ex As Exception
            Write_Status("mod " + name + " ex " + ex.ToString)
            'RaiseEvent log_text("mod " + name + " ex " + ex.ToString)
        End Try
    End Sub
    '刪除資料夾內特定檔案
    Protected Sub F001()
        'https://docs.microsoft.com/zh-tw/dotnet/api/system.io.directory.getfiles?view=netframework-4.8#System_IO_Directory_GetFiles_System_String_System_String_
        'searchPattern中允許下列萬用字元規範
        '*該位置中的零或多個字元
        '?該位置中的零或一個字元
        Try
            Dim temp() As String = file_type_value.Split(",")
            Dim dir As String = temp(0)
            Dim searchPattern As String = temp(1)

            If Not System.IO.Directory.Exists(file_path) Then
                Exit Sub
            End If
            'Dim files() As String = System.IO.Directory.GetFiles(dir, searchPattern)

            'For index As Integer = 0 To files.Length - 1
            '    Me.deletefile(files.Length - 1)

            'Next
            Dim files_info() As System.IO.FileInfo = DirectCast(D_File, File_001).getFiles(file_path, file_type_value)
            DirectCast(D_File, File_001).del_file(file_type_value, files_info)
            'For index As Integer = 0 To files_info.Length - 1
            '    'If files_info(index).CreationTime.AddDays(time_day) < Now Then
            '    '    files_info(index).Delete()
            '    'End If
            '    files_info(index).Delete()
            'Next
        Catch ex As Exception
            Write_Status("mod " + name + " ex " + ex.ToString)
            'RaiseEvent log_text("mod " + name + " ex " + ex.ToString)
        End Try
    End Sub
    '刪除一定時間的檔案 天
    Protected Sub F002()
        Try
            If Not System.IO.Directory.Exists(file_path) Then
                Exit Sub
            End If
            Dim temp() As String = file_type_value.Split(",")
            'Dim dir As String = temp(0)
            'Dim searchPattern As String = temp(1)
            Dim time_day As String = temp(2)
            'If Not System.IO.Directory.Exists(dir) Then
            '    Exit Sub
            'End If
            ''Dim files() As String = System.IO.Directory.GetDirectories()
            'Dim dir_info As New System.IO.DirectoryInfo(dir)
            'Dim files_info() As System.IO.FileInfo = dir_info.GetFiles(searchPattern)
            Dim files_info() As System.IO.FileInfo = File_002.getFiles(file_path, file_type_value)
            For index As Integer = 0 To files_info.Length - 1
                If files_info(index).CreationTime.AddDays(time_day) < Now Then
                    files_info(index).Delete()
                End If

            Next
            'For index As Integer = 0 To files.Length - 1
            '    Me.deletefile(files.Length - 1)

            'Next

        Catch ex As Exception
            Write_Status("mod " + name + " ex " + ex.ToString)
            'RaiseEvent log_text("mod " + name + " ex " + ex.ToString)
        End Try
    End Sub
    Protected Sub F003()
        Try
            If Not System.IO.Directory.Exists(file_path) Then
                Exit Sub
            End If
            'Dim temp() As String = file_type_value.Split(",")
            ''Dim dir As String = temp(0)
            ''Dim searchPattern As String = temp(1)
            'Dim time_day As String = temp(1)
            'If Not System.IO.Directory.Exists(dir) Then
            '    Exit Sub
            'End If
            ''Dim files() As String = System.IO.Directory.GetDirectories()
            'Dim dir_info As New System.IO.DirectoryInfo(dir)
            'Dim files_info() As System.IO.FileInfo = dir_info.GetFiles(searchPattern)
            Dim dir_info() As System.IO.DirectoryInfo = DirectCast(D_File, File_003).getFiles(file_path, file_type_value)
            DirectCast(D_File, File_003).del_file(file_type_value, dir_info)
            'For index As Integer = 0 To dir_info.Length - 1
            '    If dir_info(index).CreationTime.AddDays(time_day) < Now Then
            '        dir_info(index).Delete()
            '    End If

            'Next
            'For index As Integer = 0 To files.Length - 1
            '    Me.deletefile(files.Length - 1)

            'Next

        Catch ex As Exception
            Write_Status("mod " + name + " ex " + ex.ToString)
            'RaiseEvent log_text("mod " + name + " ex " + ex.ToString)
        End Try
    End Sub


    Protected Sub F_test()

    End Sub
#End Region
#Region "call"
    Sub run()

        'If err Then
        '    Exit Sub
        'End If
        If enable = "false" Then
            Exit Sub
        End If
        Me.Write_Status("mod " + name + " run")
        RaiseEvent log_text("mod " + name + " run")
        Select Case Timer_name
            Case "T001"
                Timer_fun = New morningcall
                Try
                    Dim t As DateTime = Convert.ToDateTime(Timer_value)
                    t = New DateTime(Now.Year, Now.Month, Now.Day, t.Hour, t.Minute, t.Second)

                    DirectCast(Timer_fun, morningcall).call_time = t
                Catch ex As Exception
                    Exit Sub
                End Try
                Me.D_Time = New Time_001(Timer_fun)

                'Case "T002"
                'Case "T999"
            Case Else
                'Timer_fun = New morningcall
                'DirectCast(Timer_fun, morningcall).call_time = Now
                Exit Sub
        End Select

        AddHandler Timer_fun.Mcall, AddressOf AddressOf_del2
        Timer_fun.strat()

    End Sub

#End Region


    'Private Function deletefile(path As String) As Boolean
    '    If Not System.IO.File.Exists(path) Then
    '        Return False
    '    End If
    '    System.IO.File.Delete(path)
    '    Return True
    'End Function
End Class

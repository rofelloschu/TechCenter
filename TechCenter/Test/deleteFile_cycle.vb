'20191025
'20200206
'20200214
Public Class deleteFile_cycle
    'Private syste
    Public path_list As List(Of System.IO.FileInfo)
    Protected t_deletefile_thrad As Threading.Thread
    Protected m_delete_hours As Integer
    Protected AutoResetEvent As System.Threading.AutoResetEvent = New System.Threading.AutoResetEvent(True)
    Protected m_checktime As DateTime

    'Private ErrlogFile As ErrlogFile
    Protected p_filename As String
    Sub New(t_hours As Integer, Optional filename As String = "_deleteFile_cycle.txt")
        'ErrlogFile = New ErrlogFile("deleteFile_cycle_err.txt", False)
        p_filename = filename
        m_delete_hours = t_hours
        path_list = New List(Of System.IO.FileInfo)
        'checktime = New DateTime(2019, 9, 2, 0, 0, 0)
        checktime = Now.AddMinutes(30)
        t_deletefile_thrad = New Threading.Thread(AddressOf AddressOf_deletefile_thrad)
        t_deletefile_thrad.Start()
        'ErrlogFile.Write("start")
        'ErrlogFile.Write("checktime " + checktime.ToString("u"))
    End Sub
    Sub close()
        Try
            saveP()
            t_deletefile_thrad.Abort()
            path_list.Clear()
        Catch ex As Exception

        End Try
    End Sub
    '多餘
    Public Sub firstadd(ByVal path As String)
        AutoResetEvent.WaitOne()
        If System.IO.File.Exists(path) Then Return
        Dim file_info As System.IO.FileInfo = New System.IO.FileInfo(path)

        If file_info.CreationTime.AddHours(m_delete_hours) < DateTime.Now Then
            file_info.Delete()
        Else
            path_list.Add(file_info)
        End If
        AutoResetEvent.Set()
    End Sub
    Sub loadP()
        AutoResetEvent.WaitOne()
        If Not System.IO.File.Exists(p_filename) Then
            AutoResetEvent.Set()
            Exit Sub
        End If
        Dim Data As String() = System.IO.File.ReadAllLines(p_filename)

        For index As Integer = 0 To Data.Length - 1
            If System.IO.File.Exists(Data(index)) Then
                path_list.Add(New System.IO.FileInfo(Data(index)))
            End If

        Next
        'path_list.AddRange(Data)
        AutoResetEvent.Set()
    End Sub
    Sub saveP()
        AutoResetEvent.WaitOne()
        Dim temp_filename As String = "_" + p_filename
        For index As Integer = 0 To path_list.Count - 1
            System.IO.File.AppendAllText(temp_filename, path_list(index).FullName + vbNewLine)
        Next
        If System.IO.File.Exists(temp_filename) Then
            System.IO.File.Copy(temp_filename, p_filename, True)
            System.IO.File.Delete(temp_filename)
        End If



        AutoResetEvent.Set()
    End Sub
    Sub addpath(path As String)
        AutoResetEvent.WaitOne()
        Try

            If System.IO.File.Exists(path) Then
                path_list.Add(New System.IO.FileInfo(path))
                'ErrlogFile.Write("add " + path)
            End If
        Catch ex As Exception
        Finally
            AutoResetEvent.Set()
        End Try


    End Sub


    Overridable Sub AddressOf_deletefile_thrad()
        While True
            Threading.Thread.Sleep(1000)

            'Dim t_file() As System.IO.FileInfo = t_dirinfo.GetFiles(Me.m_filename + "*.txt")
            If check_time() Then
                Me.deleteFile()
                checktime = Now.AddMinutes(30)
                Me.saveP()


            End If

        End While
    End Sub
    Protected Sub deleteFile()
        AutoResetEvent.WaitOne()
        Try

            For index As Integer = 0 To path_list.Count - 1


                '刪除用

                'If path_list.Count > 0 Then
                If path_list(0).CreationTime.AddHours(m_delete_hours) < Now Then
                    'ErrlogFile.Write("Delete " + path_list(0).FullName)
                    If path_list(0).Exists Then
                        path_list(0).Delete()
                    End If
                    path_list.RemoveAt(0)
                Else
                    Exit For
                End If
                'End If


            Next

        Catch ex As Exception
        Finally
            'path_list.Clear()
            'ErrlogFile.Write("clear")

            'ErrlogFile.Write("checktime " + checktime.ToString("u"))
            AutoResetEvent.Set()

        End Try
    End Sub

    Property checktime As DateTime
        Get
            Return m_checktime
        End Get
        Set(value As DateTime)
            'AutoResetEvent.WaitOne()
            m_checktime = value
            'AutoResetEvent.Set()
        End Set
    End Property

    Overridable Function check_time() As Boolean
        If checktime.Second = Now.Second AndAlso checktime.Minute = Now.Minute AndAlso checktime.Hour = Now.Hour Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
Public Class use_deleteFile_cycle
    Sub New()

    End Sub
    Sub run()
        Dim deleteFile_cycle As New deleteFile_cycle(1)
        deleteFile_cycle.loadP()
        deleteFile_cycle.saveP()

    End Sub
End Class
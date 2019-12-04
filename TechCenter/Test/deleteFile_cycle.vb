'20191002
Public Class deleteFile_cycle
    'Private syste
    Public path_list As List(Of System.IO.FileInfo)
    Private t_deletefile_thrad As Threading.Thread
    Private m_delete_hours As Integer
    Private AutoResetEvent As System.Threading.AutoResetEvent = New System.Threading.AutoResetEvent(True)
    Public checktime As DateTime

    'Private ErrlogFile As ErrlogFile
    Sub New(t_hours As Integer)
        'ErrlogFile = New ErrlogFile("deleteFile_cycle_err.txt", False)
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
            t_deletefile_thrad.Abort()
            path_list.Clear()
        Catch ex As Exception

        End Try
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
                AutoResetEvent.WaitOne()
                Try

                    For index As Integer = 0 To path_list.Count - 1


                        '刪除用

                        'If path_list.Count > 0 Then
                        If path_list(0).CreationTime.AddHours(m_delete_hours) < Now Then
                            'ErrlogFile.Write("Delete " + path_list(0).FullName)
                            path_list(0).Delete()
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
                    checktime = Now.AddMinutes(30)
                    'ErrlogFile.Write("checktime " + checktime.ToString("u"))
                    AutoResetEvent.Set()
                End Try

            End If

        End While
    End Sub

    Overridable Function check_time() As Boolean
        If checktime.Second = Now.Second AndAlso checktime.Minute = Now.Minute AndAlso checktime.Hour = Now.Hour Then
            Return True
        Else
            Return False
        End If
    End Function
End Class

Namespace IO
    Public Class WaitReviceCC
        Private mutexList As mutexList_V02(Of Byte())

        Private t_Revice As Threading.Thread
        Private t_Revice_enabled As Boolean
        Private Comm As IF_Comm
        Sub New(t_Comm As IF_Comm)
            Comm = t_Comm
            mutexList = New mutexList_V02(Of Byte())
            t_Revice_enabled = True
            t_Revice = New Threading.Thread(AddressOf AddressOf_Revice)
            t_Revice.Start()
            AddHandler Comm.RecvDataFromCC, AddressOf AddressOf_RecvData
        End Sub
        Public Sub close()
            Try
                t_Revice_enabled = False
                t_Revice.Abort()
            Catch ex As Exception

            End Try
            RemoveHandler Comm.RecvDataFromCC, AddressOf AddressOf_RecvData

        End Sub
        Private Sub AddressOf_RecvData(data() As Byte)
            mutexList.Add(data)
        End Sub
        Event event_ReviceDdata(data() As Byte)
        Private Sub AddressOf_Revice()
            While t_Revice_enabled
                Threading.Thread.Sleep(1)
                If mutexList.Count > 0 Then
                    Try
                        Dim waitReviceData() As Byte = mutexList.getFirstValue
                        RaiseEvent event_ReviceDdata(waitReviceData)
                    Catch ex As Exception

                    End Try
                End If

            End While
        End Sub

    End Class
End Namespace


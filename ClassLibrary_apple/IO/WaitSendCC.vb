Namespace IO
    Public Class WaitSendCC
        Private mutexList As mutexList_V02(Of Byte())

        Private t_Send As Threading.Thread
        Private t_Send_enabled As Boolean
        Private Comm As IF_Comm
        Sub New(t_Comm As IF_Comm)
            Comm = t_Comm
            mutexList = New mutexList_V02(Of Byte())
            t_Send_enabled = True
            t_Send = New Threading.Thread(AddressOf AddressOf_Send)
            t_Send.Start()
        End Sub
        Public Sub close()
            Try
                t_Send_enabled = False
                t_Send.Abort()
            Catch ex As Exception

            End Try
        End Sub
        Public Sub send(data() As Byte)
            mutexList.Add(data)
        End Sub
        Private Sub AddressOf_Send()
            While t_Send_enabled
                Threading.Thread.Sleep(1)
                If mutexList.Count > 0 Then
                    Try
                        Dim waitSendData() As Byte = mutexList.getFirstValue
                        Comm.SendDataToCC(waitSendData)
                    Catch ex As Exception

                    End Try

                End If
            End While
        End Sub

    End Class
End Namespace


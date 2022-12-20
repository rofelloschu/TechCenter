'20211012
Public Module M_ClientReadWork2

    Private SPcount As Integer = 0

    Private modCount As Integer = 3
    Private lastDataStr As String = ""
    '測是連線
    Function TestConnected(ByVal client As System.Net.Sockets.TcpClient) As Boolean
        Dim testRecByte(0) As Byte
        Try
            If client.Connected And client.Client.Poll(0, System.Net.Sockets.SelectMode.SelectRead) Then
                If client.Client.Receive(testRecByte, System.Net.Sockets.SocketFlags.Peek) = 0 Then
                    M_WriteLineMaster.WriteLine("斷線" + testRecByte.Length.ToString)
                    Return False
                Else
                    M_WriteLineMaster.WriteLine("連線" + testRecByte.Length.ToString)
                    Return True
                End If
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try


    End Function
    Function auto_readWork(ByVal stream As System.Net.Sockets.NetworkStream, Buffer_len As Integer) As Byte()



        'Dim returnData As Byte() = Nothing
        Dim myReadBuffer(Buffer_len) As Byte
        'Dim textstring As String = ""
        Dim numberOfBytesRead As Integer = 0
        Dim isReadFull As Boolean = False
        Dim all_ReadBuffer_list As New List(Of Byte)
        Do
            Try
                numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length)
                ' Console.WriteLine("numberOfBytesRead " + numberOfBytesRead.ToString)
                If numberOfBytesRead = 0 Then
                    '   Console.WriteLine("numberOfBytesRead " + numberOfBytesRead.ToString + " end3")
                    System.Threading.Thread.Sleep(1)
                    Exit Do
                End If

                If numberOfBytesRead = myReadBuffer.Length Then
                    '快速讀取
                    isReadFull = True
                    all_ReadBuffer_list.AddRange(myReadBuffer)
                Else
                    isReadFull = False
                    '2
                    Dim tempdata(numberOfBytesRead - 1) As Byte
                    Array.Copy(myReadBuffer, 0, tempdata, 0, numberOfBytesRead)
                    all_ReadBuffer_list.AddRange(tempdata)
                    '1
                    'For index As Integer = 0 To numberOfBytesRead - 1
                    '    all_ReadBuffer_list.Add(myReadBuffer(index))
                    'Next


                End If
                '讀滿過濾x
                If Not isReadFull Then
                    System.Threading.Thread.Sleep(1)
                    Exit Do
                End If


            Catch ex As Exception
                Exit Do
            End Try
        Loop While True

        Return all_ReadBuffer_list.ToArray
    End Function

    'Function readWork(ByVal stream As System.Net.Sockets.NetworkStream, Optional ByVal Encode As System.Text.Encoding = Nothing)
    '    If stream.DataAvailable Then
    '        Return M_ClientReadWork.auto_readWork(stream, Encode)
    '    Else
    '        Return Nothing
    '    End If
    'End Function

End Module

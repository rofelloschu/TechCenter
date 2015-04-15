<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Public Module M_ClientReadWork

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
    Function auto_readWork(ByVal stream As System.Net.Sockets.NetworkStream, Optional ByVal Encode As System.Text.Encoding = Nothing) As Byte()
        Dim mEncoding As System.Text.Encoding = Encode


        Dim returnData As Byte() = Nothing
        Dim myReadBuffer(1023) As Byte
        Dim textstring As String = ""
        Dim numberOfBytesRead As Integer = 0
        Dim isReadFull As Boolean = False
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
                    'Exit Do
                Else
                    'isReadFull = False
                End If
                '讀滿過濾
                If Not isReadFull Then
                    If mEncoding Is Nothing Then
                        mEncoding = M_EncodingDefault.bom(myReadBuffer)
                    End If

                    textstring = mEncoding.GetString(myReadBuffer, 0, numberOfBytesRead)
                    returnData = mEncoding.GetBytes(textstring)
                    '判斷重複過濾
                    'If lastDataStr.Equals(textstring) Then
                    '    SPcount = SPcount + 1
                    '    If SPcount Mod modCount = 0 Then
                    '        If SPcount > 1000 Then
                    '            SPcount = 0
                    '        End If
                    '        ' RaiseEvent ReadByte(Me.Encode.GetBytes(textstring))
                    '    End If
                    'Else
                    '    lastDataStr = textstring
                    '    'RaiseEvent ReadByte(Me.Encode.GetBytes(textstring))
                    'End If
                End If

                System.Threading.Thread.Sleep(1)
                Exit Do

            Catch ex As Exception
                Exit Do
            End Try
        Loop While True

        Return returnData
    End Function
End Module

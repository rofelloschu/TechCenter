'20140227
'連線工具
Imports System.Threading
Imports System.Net.Sockets
Imports System.Text
Imports classLibrary_bang
'Namespace tools.socket
'Interface LineModes
'    Sub connect()
'    Sub connect(ByVal ip As String, ByVal port As String)
'    Sub disconnect()
'    Function readData(ByVal data() As Byte) As Integer
'    Function autoRead(ByVal data() As Byte) As Integer
'    Function autoRead() As Byte
'    Function getData() As Byte()
'    Sub writeData(ByVal data() As Byte)
'    Sub createServer()


'End Interface
'綜合client server  用於two way
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Class tcpSocket

    Private tcpClientSocket As tcpClientSocket
    Private tcpServerSocket As tcpServerSocket
    Sub New()
        tcpClientSocket = New tcpClientSocket
        tcpServerSocket = New tcpServerSocket
    End Sub
    Sub clientConnect(ByVal ip As String, ByVal port As String)
        tcpClientSocket.connect(ip, port)
    End Sub

    Function clientAutoRead() As Byte()

        Return tcpClientSocket.autoRead()
    End Function
    Function isTcpConnected() As Boolean
        Return tcpClientSocket.getConnected()
    End Function
    Sub clientDisconnect()
        tcpClientSocket.disconnect()
    End Sub
    Sub clientSendToServer(ByVal data As Byte())
        tcpClientSocket.writeData(data)
    End Sub
    Function isServerConnected() As Boolean
        Return tcpServerSocket.getConnected()
    End Function
    Sub createServer()
        tcpServerSocket.start()
    End Sub
    Sub createServer(ByVal port As String)
        tcpServerSocket.start(port)
    End Sub
    Function ServerAutoRead() As Byte()
        Return tcpServerSocket.autoRead()
    End Function
    Function ServerRead() As Byte()
        Dim data As Byte() = Nothing
        ServerRead(data)
        Return data
    End Function
    Function ServerRead(ByVal data As Byte()) As Integer

        Return tcpServerSocket.readData(data)
    End Function
    Sub CloseServer()
        tcpServerSocket.close()
    End Sub
    Sub serverSendToClient(ByVal data As Byte())
        tcpServerSocket.writeData(data)
    End Sub
    Function getServerPort() As String
        Return tcpServerSocket.getPort
    End Function
    Function getClientIpFromServer() As String
        Return tcpServerSocket.getClientIP
    End Function

End Class
'tcpClient
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Public Class tcpClientSocket
    Implements tcpScoket_IF
    Private stream As NetworkStream
    Private data() As Byte
    Private TcpIp As System.Net.IPAddress
    Private TcpPort As String
    Private Client As TcpClient
    Private Connected As Boolean
    Private dataLength As Integer
    Private auto As Boolean
    Private testConnected(4) As Byte
    Private isQuick As Boolean

    Private t_read_start As Boolean
    Private t_read As Thread
    Private Encode As Encoding
    Sub New()
        Encode = Encoding.Default
        testConnected(0) = CByte(0)
        testConnected(1) = CByte(1)
        testConnected(2) = CByte(2)
        testConnected(3) = CByte(3)
        testConnected(4) = CByte(4)
        Connected = False
        auto = False
        isQuick = False

        t_read_start = True
        t_read = New Thread(AddressOf auto_read)
        t_read_start = True
        t_read.Start()
    End Sub
    '資料讀入
    Function readData(ByVal data() As Byte) As Integer
        Dim dataCount As Integer
        Try
            If Connected Then
                If stream.DataAvailable Then
                    'If stream.CanRead Then
                    '(資料,起始位置,資料數),
                    dataCount = stream.Read(data, 0, data.Length)

                    Dim data2(dataCount - 1) As Byte
                    For i As Integer = 0 To dataCount - 1
                        data2(i) = data(i)
                    Next
                    data = data2
                    Me.data = data2
                End If

            End If
        Catch ex As Exception
            M_WriteLineMaster.WriteLine("[tcpClientSocket.readData]" + ex.ToString)
            'stream.Close()

        End Try
        Return dataCount
    End Function
    '資料讀入
    Function readData() As Integer
        Dim data(1023) As Byte
        Return readData(data)
    End Function
    '自動讀取
    Function autoRead(ByVal data() As Byte) As Integer


        Dim flag As Boolean = True
        While flag

            dataLength = readData(data)
            If (dataLength = 0) Then
            Else
                flag = False
            End If

            Thread.Sleep(1)
        End While

        Return dataLength
    End Function
    '自動讀取-無0資料
    Function autoRead() As Byte()
        Dim data(1023) As Byte
        dataLength = autoRead(data)
        Dim data2(dataLength - 1) As Byte
        If (dataLength = 0) Then
            data2 = Nothing
        Else

            For j As Integer = 0 To data2.Length - 1
                data2(j) = data(j)
            Next
            data = data2
            GC.Collect()
        End If
        Return data
    End Function
    Function autoRead2() As Byte()

        Dim rData As Byte() = Nothing
        Try
            If Connected Then

                If stream.CanRead Then
                    '(資料,起始位置,資料數),
                    stream.ReadTimeout = 2000
                    ' Dim myReadBuffer(1023) As Byte
                    Dim myReadBuffer(Me.Client.ReceiveBufferSize) As Byte
                    Dim myCompleteMessage As StringBuilder = New StringBuilder()
                    Dim numberOfBytesRead As Integer = 0
                    Do
                        numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length)
                        ' Console.WriteLine("numberOfBytesRead " + numberOfBytesRead.ToString)
                        '

                        myCompleteMessage.AppendFormat("{0}", Me.Encode.GetString(myReadBuffer, 0, numberOfBytesRead))

                        If Not numberOfBytesRead = myReadBuffer.Length Then
                            'Console.WriteLine("numberOfBytesRead " + numberOfBytesRead.ToString)
                            Exit Do
                        End If

                    Loop While True

                    'Dim data2(numberOfBytesRead - 1) As Byte
                    'For i As Integer = 0 To numberOfBytesRead - 1
                    '    data2(i) = data(i)
                    'Next

                    Me.data = Encoding.ASCII.GetBytes(myCompleteMessage.ToString)

                    rData = data

                End If
            End If
        Catch ex As Exception

            'stream.Close()

            If TypeOf ex Is Exception Then
                Dim ex2 As Exception = ex.GetBaseException
                If TypeOf ex2 Is SocketException Then
                    Dim ex3 As SocketException = ex2
                    Select Case ex3.ErrorCode
                        Case 10060, 10035
                            'WriteLineMaster.WriteLine("[tcpClientSocket.autoRead2] time out")
                            Thread.Sleep(250)
                        Case Else
                            M_WriteLineMaster.WriteLine("[tcpClientSocket.autoRead2]" + ex.ToString)
                            disconnect()
                    End Select
                End If
            Else
                M_WriteLineMaster.WriteLine("[tcpClientSocket.autoRead2]" + ex.ToString)
                disconnect()
                Thread.Sleep(1000)
            End If

        End Try
        Return rData
    End Function
    '-無0資料
    Function readDataQ() As Byte()
        Dim data(1023) As Byte
        dataLength = readData(data)
        If dataLength > 0 Then
            Dim data2(dataLength - 1) As Byte
            If (dataLength = 0) Then
                data2 = Nothing
                M_WriteLineMaster.WriteLine("a")
            Else
                M_WriteLineMaster.WriteLine("b")
                For j As Integer = 0 To data2.Length - 1
                    data2(j) = data(j)
                Next
                data = data2
                GC.Collect()
            End If
            Return data
        Else
            M_WriteLineMaster.WriteLine("c")
            Return Nothing
        End If

    End Function
    '資料寫出
    Sub writeData(ByVal data() As Byte) Implements tcpScoket_IF.Write
        Try
            If Connected Then

                stream.Write(data, 0, data.Length)

            End If
        Catch ex As Exception

            M_WriteLineMaster.WriteLine("[CatchData.writedData]" + ex.ToString)

            ' stream.Close()
            disconnect()
        End Try

    End Sub
    Sub writeData(ByVal text As String) Implements tcpScoket_IF.Write
        Me.writeData(Me.Encode.GetBytes(text))
    End Sub
    '設定stream
    Sub setStream(ByVal stream As NetworkStream)
        Me.stream = stream
    End Sub
    '連線
    Public Sub connect(ByVal ip As String, ByVal port As String)
        '待改
        connect(ip, port, False)
    End Sub
    Public Sub connect(ByVal ip As String, ByVal port As Integer) Implements tcpScoket_IF.connect
        '待改
        connect(ip, port.ToString, False)
    End Sub


    '連線 第三個參數控制是否自動連線
    Public Sub connect(ByVal ip As String, ByVal port As String, ByVal auto As Boolean)
        setIp(ip)
        setPort(port)
        Me.auto = auto
        Do
            Try
                Thread.Sleep(1000)
                If Connected Then
                    writeData(testConnected)
                Else
                    Client = New TcpClient()
                    Client.Connect(TcpIp, CInt(TcpPort))
                    stream = Client.GetStream
                    Connected = True

                End If

                '   Thread.Sleep(1000) Thread.Sleep(2000)
            Catch ex As Exception
                Thread.Sleep(100)
                M_WriteLineMaster.WriteLine("[TcpConnect]" + ex.ToString)
                'MsgBox("[TcpConnect]" + ex.Message)
                Connected = False
                Client.Close()
                Throw ex
            Finally
                GC.Collect()
            End Try
        Loop While auto
    End Sub
    '斷線
    Public Sub disconnect() Implements tcpScoket_IF.close
        '待改
        Try
            Connected = False
            t_read_start = False
            Client.Close()

        Catch ex As Exception
            M_WriteLineMaster.WriteLine("[TcpDisconnect]" + ex.ToString)
            'MsgBox("[TcpDisconnect]" + ex.Message)
            Client.Close()
            Throw ex
            '  Exit Try
        Finally
            GC.Collect()
        End Try
    End Sub
    '設定ip
    Sub setIp(ByVal ip As String)
        Dim hostname As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(ip)
        Dim t_ip As System.Net.IPAddress() = hostname.AddressList
        TcpIp = t_ip(0)
        ' TcpIp = System.Net.IPAddress.Parse(ip)
    End Sub
    '設定port
    Sub setPort(ByVal port As String)
        TcpPort = port
    End Sub
    '取得stream
    Function getStream() As NetworkStream
        Return stream
    End Function
    '是否連線狀態
    Function getConnected() As Boolean
        Return Connected
    End Function

    '關閉
    Sub close()
        auto = False
        disconnect()
        If stream Is Nothing Then
        Else
            stream.Close()
        End If

    End Sub
    '取得port
    Function getPort() As String
        Return TcpPort
    End Function
    '取得目標IPIP
    Function getIP() As System.Net.IPAddress
        Return TcpIp
    End Function
    '取得讀到的資料
    Function getData() As Byte()
        Return Me.data
    End Function
    '取得Client端IP
    Function getClientIP() As String
        Dim ClientIp As String = "null"
        If Client Is Nothing Then

        Else
            Dim ipend As Net.IPEndPoint = Client.Client.RemoteEndPoint
            ClientIp = ipend.Address.ToString + ipend.Port.ToString

        End If

        Return ClientIp
    End Function
    Sub quick()
        If Not Client.Client Is Nothing Then
            Client.NoDelay = True
        End If
    End Sub

    Public Event DisplayStatus(ByVal Status As String) Implements tcpScoket_IF.DisplayStatus

    Public Event ReadByte(ByVal ReadTe() As Byte) Implements tcpScoket_IF.ReadByte

    Public Event ReadString(ByVal Text As String) Implements tcpScoket_IF.ReadString
    Sub auto_read()
        'Dim testWT As New watchTimer
        While t_read_start
            Try

                If Not Connected Then
                    Thread.Sleep(1000)
                    Continue While
                End If

                Dim readbyte() As Byte = Me.autoRead2


                If readbyte.Length > 0 Then
                    'testWT.timeStop()
                    RaiseEvent ReadByte(readbyte)
                    RaiseEvent ReadString(Me.Encode.GetString(readbyte))
                    ' testWT.timeStart()
                End If


                Thread.Sleep(1)
            Catch ex As Exception
                Thread.Sleep(1000)
                M_WriteLineMaster.WriteLine("auto_read:" + ex.ToString)
            End Try
        End While
    End Sub
End Class
'tcpServer
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Class tcpServerSocket
    Private stream As NetworkStream
    Private mydata() As Byte
    Private TcpIp As System.Net.IPAddress
    Private TcpPort As String
    Private Server As TcpListener
    Private client As TcpClient
    Private dataLength As Integer
    Private Connected As Boolean
    Private auto_Connected As Boolean

    Private lastTime As DateTime
    Private LastMessage As String

    Sub New()
        TcpIp = System.Net.IPAddress.Any
        TcpPort = "9999"
        Connected = False
        auto_Connected = True
        lastTime = Now
        LastMessage = "Null"
    End Sub
    Sub New(ByVal serverIP As String, ByVal serverPort As String)
        setIp(serverIP)
        setPort(serverPort)
        Connected = False
        auto_Connected = True
        lastTime = Now
        LastMessage = "Null"
    End Sub
    Sub New(ByVal serverIP As String, ByVal serverPort As String, ByVal isAuto As Boolean)
        setIp(serverIP)
        setPort(serverPort)
        Connected = False
        setAuto(isAuto)
        lastTime = Now
        LastMessage = "Null"
    End Sub
    Sub start()
        Dim tempClient As TcpClient
        Dim first As Boolean = True
        Dim second As Boolean = True
        Do
            Thread.Sleep(1000)
            Try

                Server = New TcpListener(TcpIp, CInt(TcpPort))
                M_WriteLineMaster.WriteLine("Server Listener" + " " + "Ip:" + TcpIp.ToString + " " + "port:" + TcpPort)
                Server.Start()
                While auto_Connected
                    Thread.Sleep(1000)
                    Try
                        tempClient = Server.AcceptTcpClient()
                        Connected = False
                        If Server.Pending Then
                            If (client Is Nothing) Then
                            Else
                                client.Close()
                            End If
                            If (stream Is Nothing) Then
                            Else
                                stream.Close()
                            End If
                        End If
                        client = tempClient

                        M_WriteLineMaster.WriteLine("Server 建立" + " " + "Ip:" + TcpIp.ToString + " " + "port:" + TcpPort)

                        stream = client.GetStream()
                        Connected = True
                        Thread.Sleep(1)
                        GC.Collect()
                        If first Then
                            first = False
                        End If
                        'Catch ex As SocketException
                        '    Thread.Sleep(1000)
                        '    catchException.printExceptionInFile(TcpPort.ToString + "[tcpServerSocket.start]TcpPort Connected:" + Connected.ToString, ex)

                        '    WriteLineMaster.WriteLine(TcpPort.ToString + "[tcpServerSocket.start]" & ex.ErrorCode.ToString)
                    Catch ex As Exception
                        Thread.Sleep(1000)
                        If first Then

                            auto_Connected = False
                            M_catchException.printExceptionInFile(TcpPort.ToString + "[tcpServerSocket.start]TcpPort Create Fail", ex)
                            M_WriteLineMaster.WriteLine(TcpPort.ToString + "[tcpServerSocket.start]TcpPort Create Fail")

                        End If
                        M_catchException.printExceptionInFile(TcpPort.ToString + "[tcpServerSocket.start]TcpPort Connected:" + Connected.ToString, ex)

                        M_WriteLineMaster.WriteLine(TcpPort.ToString + "[tcpServerSocket.start]" & ex.ToString)

                    End Try
                End While



            Catch ex As Exception
                Thread.Sleep(1000)
                M_catchException.printExceptionInFile(TcpPort.ToString + "[tcpServerSocket.start2]TcpPort Connected:" + Connected.ToString, ex)

                M_WriteLineMaster.WriteLine(TcpPort.ToString + "[tcpServerSocket.start2]" & ex.ToString)
            Finally
                Server.Stop()
            End Try
        Loop While True

    End Sub
    Sub start_o()
        Dim tempClient As TcpClient
        Dim first As Boolean = True
        Dim second As Boolean = True
        Do
            Try
                Server = New TcpListener(TcpIp, CInt(TcpPort))
                M_WriteLineMaster.WriteLine("Server Listener" + " " + "Ip:" + TcpIp.ToString + " " + "port:" + TcpPort)
                Server.Start()

                tempClient = Server.AcceptTcpClient()

                If (client Is Nothing) Then
                Else
                    Me.close()
                End If
                client = tempClient
                Server.Stop()
                M_WriteLineMaster.WriteLine("Server 建立" + " " + "Ip:" + TcpIp.ToString + " " + "port:" + TcpPort)
                stream = client.GetStream()

                Connected = True

                Thread.Sleep(1)
                GC.Collect()
                If first Then
                    first = False
                End If
                'Catch ex As SocketException
                '    Thread.Sleep(1000)
                '    catchException.printExceptionInFile(TcpPort.ToString + "[tcpServerSocket.start]TcpPort Connected:" + Connected.ToString, ex)

                '    WriteLineMaster.WriteLine(TcpPort.ToString + "[tcpServerSocket.start]" & ex.ErrorCode.ToString)
            Catch ex As Exception
                Thread.Sleep(1000)
                If first Then

                    auto_Connected = False
                    M_catchException.printExceptionInFile(TcpPort.ToString + "[tcpServerSocket.start]TcpPort Create Fail", ex)
                    M_WriteLineMaster.WriteLine(TcpPort.ToString + "[tcpServerSocket.start]TcpPort Create Fail")
                End If
                M_catchException.printExceptionInFile(TcpPort.ToString + "[tcpServerSocket.start]TcpPort Connected:" + Connected.ToString, ex)

                M_WriteLineMaster.WriteLine(TcpPort.ToString + "[tcpServerSocket.start]" & ex.ToString)
            Finally

            End Try
        Loop While auto_Connected

    End Sub
    Sub start(ByVal port As String)
        setPort(port)
        start()
    End Sub
    Sub start(ByVal ip As String, ByVal port As String)
        setIp(ip)
        setPort(port)
        start()
    End Sub
    Sub start(ByVal isAuto As Boolean)
        setAuto(isAuto)
        start()
    End Sub

    Sub createServer()
        While True
            Try
                Server = New TcpListener(TcpIp, CInt(TcpPort))
                Server.Start()
                If (client Is Nothing) Then
                Else
                    Connected = False
                    stream.Close()
                    client.Close()
                End If
                client = Server.AcceptTcpClient()
                stream = client.GetStream()
                Server.Stop()
                Connected = True
                Thread.Sleep(1)
            Catch ex As Exception
                M_WriteLineMaster.WriteLine("[tcpServerSocket.start]" & ex.ToString)
            End Try
        End While
    End Sub
    Function readData(ByVal data() As Byte) As Integer
        Dim x As Integer
        Try
            If Connected Then
                If stream.DataAvailable Then

                    '(資料,起始位置,資料數),
                    x = stream.Read(data, 0, data.Length)
                    If x > 0 Then
                        lastTime = Now
                    End If
                    Dim data2(x - 1) As Byte
                    For i As Integer = 0 To x - 1
                        data2(i) = data(i)
                    Next
                    data = data2
                    mydata = data2
                End If
                If Now > lastTime.AddMinutes(10) Then

                End If
            End If
        Catch ex As Exception
            M_WriteLineMaster.WriteLine(TcpPort.ToString + "[CatchData.readData]" + ex.ToString)

        End Try
        Return x
    End Function
    '輸入byte(),轉換成十六進位string寫入檔案
    Public Sub writeString(ByVal data() As Byte)
        If True Then
            Dim text As String = Nothing
            For i As Integer = 0 To data.Length - 1
                text = text + data(i).ToString("X2") + " "
            Next
            LastMessage = text
            'Console.WriteLine(TcpPort + ":" + text)
        End If




    End Sub
    Function autoRead(ByVal data() As Byte) As Integer


        Dim flag As Boolean = True
        While flag
            dataLength = readData(data)
            If (dataLength = 0) Then
            Else
                flag = False
            End If
            Thread.Sleep(1)
        End While

        Return dataLength
    End Function
    Function autoRead() As Byte()
        Dim data(1023) As Byte
        dataLength = autoRead(data)
        Dim data2(dataLength - 1) As Byte
        If (dataLength = 0) Then
            data2 = Nothing
        Else

            For j As Integer = 0 To data2.Length - 1
                data2(j) = data(j)
            Next
            data = data2
            writeString(data)
            GC.Collect()
        End If

        Return data
    End Function

    Sub writeData(ByVal data() As Byte)
        Try
            If Connected Then
                stream.Write(data, 0, data.Length)

            End If
        Catch ex As Exception
            M_WriteLineMaster.WriteLine(TcpPort.ToString + "[tcpServerSocket.writedData]" + ex.ToString)

            close()
        End Try

    End Sub
    Sub setIp(ByVal ip As String)
        TcpIp = System.Net.IPAddress.Parse(ip)
    End Sub
    Sub setPort(ByVal port As String)
        TcpPort = port
    End Sub
    Sub close()
        If Connected Then
            Connected = False
            If (client Is Nothing) Then
            Else
                client.Close()
            End If
            If (stream Is Nothing) Then
            Else
                stream.Close()
            End If

        End If

    End Sub
    Sub returnControlBackToOS()
        Thread.Sleep(0)
    End Sub
    Function getConnected() As Boolean
        Return Connected
    End Function
    Function getPort() As String
        Return TcpPort
    End Function

    Function getClientIP() As String
        Dim ClientIp As String = "null"
        If client Is Nothing Then

        Else
            Dim ipend As Net.IPEndPoint = client.Client.RemoteEndPoint
            ClientIp = ipend.Address.ToString + ipend.Port.ToString

        End If

        Return ClientIp
    End Function
    Sub setAuto(ByVal isAuto As Boolean)
        Me.auto_Connected = isAuto
    End Sub
    Function getLastMessage() As String
        Return lastTime.ToString + " port:" + TcpPort + " " + LastMessage
    End Function
End Class
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Class simpleTcpClient
    Private Client As TcpClient
    Private isConnect As Boolean

    Private ARE As AutoResetEvent = New AutoResetEvent(True)
    Sub New()
        Client = New TcpClient
        isConnect = False
    End Sub
    Public ReadOnly Property isTcpConnected() As Boolean
        Get
            Return Me.isConnect
        End Get
    End Property
    Sub connect(ByVal ip As String, ByVal port As String)
        Try
            Client.Connect(ip, CInt(port))
            isConnect = True
        Catch ex As Exception
            isConnect = False
        End Try

    End Sub
    Sub Disconnect()
        isConnect = False
        Client.Close()
    End Sub
    Sub AsyncRead()
        Dim myNetworkStream As NetworkStream = Client.GetStream
        Dim state As New StateObject(Client)

        If myNetworkStream.CanRead Then

            'Dim numberOfBytesRead As Integer
            'numberOfBytesRead = myNetworkStream.EndRead(myNetworkStream)
            Dim myReadBuffer(1024) As Byte
            myNetworkStream.BeginRead(state.Buffer, 0, state.BufferSize, New AsyncCallback(AddressOf EndReadCallback), state)
            'myNetworkStream.BeginRead(myReadBuffer, 0, myReadBuffer.Length, New AsyncCallback(AddressOf myReadCallBack), myNetworkStream)
            '  ARE.WaitOne()
            'Console.WriteLine(Encoding.ASCII.GetString(myReadBuffer, 0, 15))
        Else
            M_WriteLineMaster.WriteLine("Sorry.  You cannot read from this NetworkStream.")
        End If

        '  myReadCallBack(Client.GetStream)
    End Sub

    Protected Sub myReadCallBack(ByVal ar As IAsyncResult)
        Try
            Dim myNetworkStream As NetworkStream = CType(ar.AsyncState, NetworkStream)
            Dim myReadBuffer(1024) As Byte
            Dim myCompleteMessage As String = ""
            Dim numberOfBytesRead As Integer

            numberOfBytesRead = myNetworkStream.EndRead(ar)

            myCompleteMessage = String.Concat(myCompleteMessage, Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead))


            ' message received may be larger than buffer size so loop through until you have it all.
            While myNetworkStream.DataAvailable

                myNetworkStream.BeginRead(myReadBuffer, 0, myReadBuffer.Length, New AsyncCallback(AddressOf myReadCallBack), myNetworkStream)

            End While

            ' Print out the received message to the console.
            M_WriteLineMaster.WriteLine(("You received the following message : " + myCompleteMessage))
        Catch ex As Exception
            M_WriteLineMaster.WriteLine(ex.ToString)
        End Try

        ' ARE.Set()
    End Sub 'myReadCallBack

    '以非同步方式從用戶端通訊端讀取資料
    Private Sub EndReadCallback(ByVal ar As IAsyncResult)
        Try
            Dim state As StateObject = DirectCast(ar.AsyncState, StateObject)
            Dim client As TcpClient = state.Client
            Dim stream As NetworkStream = client.GetStream()

            Dim bytesRead As Integer = stream.EndRead(ar)

            If bytesRead > 0 Then
                state.Data.Append(Encoding.UTF8.GetString(state.Buffer, 0, bytesRead))
                stream.BeginRead(state.Buffer, 0, state.BufferSize, New AsyncCallback(AddressOf EndReadCallback), state)
            Else
                client.Close()
                ' DisplayResults(state.Data.ToString())
                M_WriteLineMaster.WriteLine(("You received the following message : " + state.Data.ToString()))
            End If
        Catch ex As Exception

        End Try

    End Sub
    Sub AsyncWrite()
        'myWriteCallBack(Client.GetStream)
    End Sub
    Protected Sub myWriteCallBack(ByVal ar As IAsyncResult)

        Dim myNetworkStream As NetworkStream = CType(ar.AsyncState, NetworkStream)
        myNetworkStream.EndWrite(ar)

    End Sub 'myWriteCallBack
End Class

<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Class tcpClientSocket2
    Private stream As NetworkStream
    Private data() As Byte
    Private TcpIp As System.Net.IPAddress
    Private TcpPort As String
    Private Client As TcpClient
    Private Connected As Boolean
    Private dataLength As Integer
    Private auto As Boolean
    Private testConnected(4) As Byte

    Private isQuick As Boolean
    '範例
    Shared Sub Connect(ByVal server As [String], ByVal message As [String])
        Try
            ' Create a TcpClient.
            ' Note, for this client to work you need to have a TcpServer 
            ' connected to the same address as specified by the server, port
            ' combination.
            Dim port As Int32 = 13000
            Dim client As New TcpClient(server, port)

            ' Translate the passed message into ASCII and store it as a Byte array.
            Dim data As [Byte]() = System.Text.Encoding.ASCII.GetBytes(message)

            ' Get a client stream for reading and writing.
            '  Stream stream = client.GetStream();
            Dim stream As NetworkStream = client.GetStream()

            ' Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length)

            Console.WriteLine("Sent: {0}", message)

            ' Receive the TcpServer.response.
            ' Buffer to store the response bytes.
            data = New [Byte](256) {}

            ' String to store the response ASCII representation.
            Dim responseData As [String] = [String].Empty

            ' Read the first batch of the TcpServer response bytes.
            Dim bytes As Int32 = stream.Read(data, 0, data.Length)
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes)
            Console.WriteLine("Received: {0}", responseData)

            ' Close everything.
            stream.Close()
            client.Close()
        Catch e As ArgumentNullException
            Console.WriteLine("ArgumentNullException: {0}", e)
        Catch e As SocketException
            Console.WriteLine("SocketException: {0}", e)
        End Try

        Console.WriteLine(ControlChars.Cr + " Press Enter to continue...")
        Console.Read()
    End Sub 'Connect
    Sub a()
        '同步
        Using client As New TcpClient("www.msn.com", 80)
            Using stream As NetworkStream = client.GetStream()
                Dim send As Byte() = Encoding.UTF8.GetBytes("GET HTTP/1.0 " & vbCr & vbLf & vbCr & vbLf)
                stream.Write(send, 0, send.Length)
                Dim bytes As Byte() = New Byte(client.ReceiveBufferSize - 1) {}
                Dim bytesRead As Integer = stream.Read(bytes, 0, CInt(client.ReceiveBufferSize))
                Dim data As [String] = Encoding.UTF8.GetString(bytes)
                Dim unused As Char() = {CChar(data(bytesRead))}
                M_WriteLineMaster.WriteLine(data.TrimEnd(unused))
            End Using
        End Using
    End Sub
    ''非同步 
    ''先建立用來在非同步呼叫間傳送狀態資訊的狀態物件
    'Sub bb01()
    '    Dim client As New TcpClient()
    '    Dim state As New StateObject(client)
    '    state.Data.AppendFormat("GET {0} HTTP/1.0" & vbCr & vbLf, url)
    '    state.Data.AppendFormat("Host:{0}" & vbCr & vbLf & vbCr & vbLf, hostName)

    '    client.BeginConnect(hostName, 0, New AsyncCallback(EndConnectCallback), state)
    'End Sub
    ''呼叫 EndConnect 方法來結束擱置的非同步連接要求
    'Private Sub EndConnectCallback(ByVal ar As IAsyncResult)
    '    Dim state As StateObject = DirectCast(ar.AsyncState, StateObject)
    '    Dim client As TcpClient = state.Client

    '    Try
    '        client.EndConnect(ar)

    '        If client.Connected Then
    '            Dim stream As NetworkStream = client.GetStream()
    '            If stream.CanWrite Then
    '                Dim send As Byte() = Encoding.UTF8.GetBytes(state.Data.ToString())
    '                stream.BeginWrite(send, 0, send.Length, New AsyncCallback(EndWriteCallback), state)
    '            End If
    '        Else
    '            DisplayStatus(String.Format("Ready (last error: {0})", "Connect Failed!"))
    '        End If
    '    Catch ex As Exception
    '        client.Close()
    '        DisplayStatus(String.Format("Ready (last error: {0})", ex.Message))
    '    End Try
    'End Sub
    ''透過 Stream 通訊端的 BeginWrite 方法以非同步方式將 HTTP 請求字串寫入網路資料流
    'Private Sub EndWriteCallback(ByVal ar As IAsyncResult)
    '    Dim state As StateObject = DirectCast(ar.AsyncState, StateObject)
    '    Dim client As TcpClient = state.Client

    '    Try
    '        Dim stream As NetworkStream = client.GetStream()
    '        stream.EndWrite(ar)

    '        state.Data.Length = 0
    '        If stream.CanRead Then
    '            stream.BeginRead(state.Buffer, 0, state.BufferSize, New AsyncCallback(EndReadCallback), state)
    '        End If
    '    Catch ex As Exception
    '        client.Close()
    '        DisplayStatus(String.Format("Ready (last error: {0})", ex.Message))
    '    End Try
    'End Sub
    ''以非同步方式從用戶端通訊端讀取資料
    'Private Sub EndReadCallback(ByVal ar As IAsyncResult)
    '    Dim state As StateObject = DirectCast(ar.AsyncState, StateObject)
    '    Dim client As TcpClient = state.Client
    '    Dim stream As NetworkStream = client.GetStream()

    '    Dim bytesRead As Integer = stream.EndRead(ar)

    '    If bytesRead > 0 Then
    '        state.Data.Append(Encoding.UTF8.GetString(state.Buffer, 0, bytesRead))
    '        stream.BeginRead(state.Buffer, 0, state.BufferSize, New AsyncCallback(AddressOf EndReadCallback), state)
    '    Else
    '        client.Close()
    '        DisplayResults(state.Data.ToString())
    '    End If
    'End Sub
End Class
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Class tcpClient_AutoCRW
    Sub connect()
        ' Dim s As NetworkStream

    End Sub

End Class

<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Public Interface tcpScoket_IF
    Sub connect(ByVal m_hostName As String, ByVal m_port As Integer)
    Sub Write(ByVal text As String)
    Sub Write(ByVal byteData As Byte())
    Event ReadByte(ByVal ReadTe As Byte())
    Event ReadString(ByVal Text As String)
    Event DisplayStatus(ByVal Status As String)
    Sub close()

End Interface
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Public Class newtcpScoket3
    Implements tcpScoket_IF

    Private tcpClient As TcpClient
    Private state As StateObject
    Private t_read As Thread
    Private t_write As Thread
    Private t_connect As Thread
    Private t_read2 As Thread
    Private t_read_Start As Boolean
    Private t_write_Start As Boolean
    Private t_connect_Start As Boolean
    Private t_read2_Start As Boolean

    Private hostName As String
    Private port As Integer
    Private Connected As Boolean
    Private TextType As System.Text.Encoding
    Friend Class newtcpScoket_p

    End Class
    Friend Class StateObject
        Public Client As TcpClient


        Public ReadOnly readData2 As mutexList_V02(Of Byte())
        Public ReadOnly writeDataByte As mutexList_V02(Of Byte())
        Public ReadOnly readDataByte As mutexList_V02(Of Byte())
        Public ReadOnly Buffer As Byte()
        Public ReadOnly BufferSize As Integer = 1024
        Public ReadOnly ReadMaxSize As Integer = 1024
        Public Sub New()


            Me.writeDataByte = New mutexList_V02(Of Byte())
            Me.readDataByte = New mutexList_V02(Of Byte())
            Me.readData2 = New mutexList_V02(Of Byte())
            Me.Buffer = New Byte(Me.BufferSize - 1) {}
        End Sub
        Public Function getNextWriteData() As Byte()
            If Me.writeDataByte.Count > 0 Then
                Return Me.writeDataByte.getFirstValue
            Else
                Return Nothing
            End If

        End Function
        Public Sub AddWriteData(ByVal Data As Byte())
            Me.writeDataByte.Add(Data)
        End Sub
        Public Function getWriteDataCount() As Integer
            Return Me.writeDataByte.Count
        End Function
        Public Sub AddReadData(ByVal Data As Byte())
            Me.readDataByte.Add(Data)
        End Sub
        Public Function getreadDataCount() As Integer
            Return Me.readDataByte.Count
        End Function
        Public Function getNextReadData() As Byte()
            If Me.readDataByte.Count > 0 Then
                Return Me.readDataByte.getFirstValue
            Else
                Return Nothing
            End If

        End Function
    End Class

    Sub New()
        Connected = False
        state = New StateObject()
        Me.hostName = "127.0.0.1"
        Me.port = 60000
        Me.TextType = System.Text.Encoding.UTF8

        t_read = New Thread(AddressOf auto_read)
        t_read_Start = True
        t_read.Start()
        t_write = New Thread(AddressOf auto_Write)
        t_write_Start = True
        t_write.Start()
        t_read2 = New Thread(AddressOf auto_read2)
        t_read2_Start = True
        t_read2.Start()
    End Sub
    Public Sub close() Implements tcpScoket_IF.close
        Me.t_connect_Start = False
        Me.t_read_Start = False
        Me.t_write_Start = False
        Me.t_read2_Start = False
        Me.tcpClient.GetStream.Close()
        Me.tcpClient.Close()
    End Sub



#Region "connect"

    Sub connect(ByVal m_hostName As String, ByVal m_port As Integer) Implements tcpScoket_IF.connect
        Me.hostName = m_hostName
        Me.port = m_port
        Connected = False
        If t_connect Is Nothing Then
            t_connect = New Thread(AddressOf auto_Connect)
            t_connect_Start = True
            t_connect.Start()

        End If
        ' t_connect = New Thread(AddressOf auto_Connect)
        't_connect_Start = True
        't_connect.Start()
        GC.Collect()
        '非同步 連線
        ' tcpClient.BeginConnect(hostName, port, New AsyncCallback(AddressOf EndConnectCallback), state)
    End Sub
    Sub auto_Connect()

        While Me.t_connect_Start
            Try
                If Not Connected Then
                    tcpClient = New TcpClient
                    state.Client = tcpClient
                    Connected = True
                End If
                If Not tcpClient.Connected Then

                    tcpClient.BeginConnect(hostName, port, New AsyncCallback(AddressOf EndConnectCallback), state)

                End If
                Thread.Sleep(1000)
            Catch ex As Exception
                Thread.Sleep(5000)
                M_WriteLineMaster.WriteLine("auto_Connect:" + ex.ToString)
            End Try

        End While
    End Sub
    '終止非同步連線
    Private Sub EndConnectCallback(ByVal ar As IAsyncResult)
        Dim state As StateObject = DirectCast(ar.AsyncState, StateObject)
        Dim client As TcpClient = state.Client

        Try
            client.EndConnect(ar)

            If client.Connected Then


            Else
                RaiseEvent DisplayStatus(String.Format("Ready (last error: {0})", "Connect Failed!"))
            End If
        Catch ex As Exception
            Connected = False
            RaiseEvent DisplayStatus(String.Format("Ready (last error: {0})", ex.Message))
        End Try
    End Sub
#End Region
#Region "write"
    Sub auto_Write()
        While t_write_Start
            Try
                If Not Connected Then
                    Thread.Sleep(1000)
                    Continue While
                End If

                If Me.state.getWriteDataCount > 0 Then
                    If Me.tcpClient.Connected Then
                        If Me.tcpClient.GetStream.CanWrite Then
                            Dim send As Byte() = state.getNextWriteData
                            Me.tcpClient.GetStream.BeginWrite(send, 0, send.Length, New AsyncCallback(AddressOf EndWriteCallback), state)
                        End If
                    Else
                        RaiseEvent DisplayStatus(String.Format("Ready (last error: {0})", "Connect Failed!"))
                    End If
                End If
                Thread.Sleep(100)
            Catch ex As Exception
                Thread.Sleep(1000)
                M_WriteLineMaster.WriteLine("auto_Write:" + ex.ToString)
            End Try


        End While
    End Sub
    Sub Write(ByVal byteData As Byte()) Implements tcpScoket_IF.Write
        Me.state.AddWriteData(byteData)
    End Sub
    Sub Write(ByVal text As String) Implements tcpScoket_IF.Write
        Me.state.AddWriteData(Me.TextType.GetBytes(text))

    End Sub
    '終止非同步Write
    Private Sub EndWriteCallback(ByVal ar As IAsyncResult)
        Dim state As StateObject = DirectCast(ar.AsyncState, StateObject)
        Dim client As TcpClient = state.Client

        Try
            Dim stream As NetworkStream = client.GetStream()
            stream.EndWrite(ar)

        Catch ex As Exception
            Connected = False
            RaiseEvent DisplayStatus(String.Format("Ready (last error: {0})", ex.Message))
        End Try
    End Sub


#End Region

#Region "read"
    Sub auto_read()
        While t_read_Start
            Try

                If Not Connected Then
                    Thread.Sleep(1000)
                    Continue While
                End If

                If Me.tcpClient.GetStream.CanRead Then
                    'state.readData.Length = 0
                    If state.readData2.Count = 0 Then
                        '  Console.WriteLine("1")
                        If Me.tcpClient.GetStream.DataAvailable Then
                            Me.tcpClient.GetStream.BeginRead(state.Buffer, 0, state.BufferSize, New AsyncCallback(AddressOf EndReadCallback), state)

                        End If
                        ' Thread.Sleep(1)
                        ' Console.WriteLine("2")
                    End If
                End If
                Thread.Sleep(100)
            Catch ex As Exception
                Thread.Sleep(1000)
                M_WriteLineMaster.WriteLine("auto_read:" + ex.ToString)
            End Try


        End While

    End Sub
    '終止非同步Read
    Private Sub EndReadCallback(ByVal ar As IAsyncResult)
        '   Console.WriteLine("EndReadCallback")
        Dim state As StateObject = DirectCast(ar.AsyncState, StateObject)
        Dim client As TcpClient = state.Client
        Dim stream As NetworkStream = client.GetStream()

        Dim bytesRead As Integer = stream.EndRead(ar)

        If bytesRead > 0 Then

            '   state.readData.Append(Me.TextType.GetString(state.Buffer, 0, bytesRead))
            Dim readByte(state.BufferSize - 1) As Byte
            state.Buffer.CopyTo(readByte, 0)
            state.readData2.Add(readByte)


            stream.BeginRead(state.Buffer, 0, state.BufferSize, New AsyncCallback(AddressOf EndReadCallback), state)

        Else
            'client.Close()
            If state.readData2.Count > 0 Then
                'RaiseEvent DisplayResults(state.readData.ToString())
                'RaiseEvent ReadByte(Encoding.UTF8.GetBytes(state.readData.ToString()
                Dim sumByte As Byte() = Nothing
                Dim count As Integer = state.readData2.Count
                Console.WriteLine("count " + count.ToString)
                For index As Integer = 0 To count - 1
                    Try
                        If index = 0 Then
                            sumByte = state.readData2.getFirstValue

                        Else
                            sumByte = M_ArraySum.Sum(sumByte, state.readData2.getFirstValue)

                        End If
                    Catch ex As Exception
                        M_WriteLineMaster.WriteLine(index.ToString)
                        Exit For
                    End Try

                Next
                state.AddReadData(sumByte)
                ' state.readData.Length = 0
            End If


        End If
    End Sub

#End Region
#Region "read2"
    Sub auto_read2()
        While t_read2_Start
            Try

                If state.getreadDataCount > 0 Then
                    Dim t_byte As Byte() = state.getNextReadData
                    RaiseEvent DisplayResults((Me.TextType.GetString(t_byte)))
                    RaiseEvent ReadByte(t_byte)
                End If
                Thread.Sleep(100)
            Catch ex As Exception
                Thread.Sleep(1000)
                M_WriteLineMaster.WriteLine("auto_read:" + ex.ToString)
            End Try


        End While

    End Sub
#End Region

    Event DisplayResults(ByVal Results As String) Implements tcpScoket_IF.ReadString
    Event DisplayStatus(ByVal Status As String) Implements tcpScoket_IF.DisplayStatus
    Event ReadByte(ByVal ReadTe() As Byte) Implements tcpScoket_IF.ReadByte






End Class

'End Namespace

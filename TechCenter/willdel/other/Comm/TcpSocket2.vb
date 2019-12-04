'20140304
'Imports tools_nf2
Imports System.Text
Imports System.Net.Sockets
Imports System.Threading
Imports classLibrary_bang
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Public Class TcpSocket2

End Class
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Public Class TcpClient2
    Implements IF_Communication

    Private stream As NetworkStream
    Private Client As TcpClient

    Private enabled As Boolean
    Private Encode As Encoding
    Private m_Connected As Boolean
    Private isrun As Boolean

    Private m_ip As String
    Private m_port As String


    Private t_read As Thread
    Private t_connect As Thread
    Sub New()
        isrun = True
        enabled = False
        Me.m_Connected = False
        Encode = Encoding.Default
    End Sub
    Public Sub start() Implements IF_Communication.start

        t_connect = New Thread(AddressOf Me.auto_connect)
        t_connect.Start()
        t_read = New Thread(AddressOf Me.auto_read)
        t_read.Start()
    End Sub
    Public Sub close() Implements IF_Communication.close

        isrun = False
        ' Me.Client.Close()
        If Not (Client Is Nothing) Then
            Client.Close()
        End If
        If Not (stream Is Nothing) Then

            stream.Close()
        End If
        Me.Connected = False
    End Sub

    Public Function connect(ByVal ip As String, ByVal port As String) As Boolean Implements IF_Communication.connect
        Me.m_ip = ip
        Me.m_port = port
        enabled = True
    End Function

    Public Sub disconnect() Implements IF_Communication.disconnect
        Me.Connected = False
        'enabled = False
        If Not (Client Is Nothing) Then
            Client.Close()
        End If

    End Sub
    Sub auto_connect()
        While Me.isrun
            If Me.enabled Then
                Try
                    Thread.Sleep(1000)
                    If Me.m_Connected Then
                        'Thread.Sleep(5000)
                        If Not TestConnected() Then
                            Me.Connected = False
                        End If
                    Else
                        Me.Client = New TcpClient()
                        Me.Client.Connect(Me.m_ip, CInt(Me.m_port))
                        Me.stream = Client.GetStream
                        Me.Connected = True
                        Dim First As String = "ID=001"
                        'Me.writeData(System.Text.Encoding.Default.GetBytes(First))
                    End If

                Catch ex As Exception
                    'Thread.Sleep(100)
                    M_WriteLineMaster.WriteLine("[TcpClient2.auto_connect]" + ex.ToString)
                    If Me.Connected Then
                        Me.Connected = False
                    End If

                    Thread.Sleep(5000)
                    'Client.Close()
                    'Throw ex
                Finally
                    GC.Collect()
                End Try

            End If
        End While
    End Sub

    Public Event readData(ByVal data() As Byte) Implements IF_Communication.readData
    Sub auto_read()

        While Me.isrun
            Thread.Sleep(1)
            If Me.enabled And Me.m_Connected Then
                If stream.CanRead Then
                    '  Me.TestConnected()
                    Dim dataByte As Byte()
                    dataByte = M_ClientReadWork.auto_readWork(Me.stream)
                    If dataByte IsNot Nothing Then
                        RaiseEvent readData(dataByte)
                    End If
                End If

            Else
                'Thread.Sleep(1000)
            End If

        End While
    End Sub
    Function TestConnected() As Boolean
        Dim testRecByte(0) As Byte
        Try
            If Client.Connected And Client.Client.Poll(0, SelectMode.SelectRead) Then
                If Client.Client.Receive(testRecByte, SocketFlags.Peek) = 0 Then
                    ' Console.WriteLine("斷線" + testRecByte.Length.ToString)
                    Return False
                Else
                    '  Console.WriteLine("連線" + testRecByte.Length.ToString)
                    Return True
                End If
            End If
            Return Client.Connected
        Catch ex As Exception
            Return False
        End Try


    End Function


    Public Sub writeData(ByVal data() As Byte) Implements IF_Communication.writeData
        Try
            If m_Connected Then

                stream.Write(data, 0, data.Length)

            End If
        Catch ex As Exception

            M_WriteLineMaster.WriteLine("[CatchData.writedData]" + ex.ToString)

            ' stream.Close()
            Me.disconnect()
        End Try

    End Sub

    Protected Property Connected() As Boolean
        Get
            Return m_Connected
        End Get
        Set(ByVal value As Boolean)
            m_Connected = value
            RaiseEvent e_Connected(value)
        End Set
    End Property
    Public Function getConnected() As Boolean Implements IF_Communication.getConnected
        Return Me.m_Connected
    End Function

    Public Event e_Connected(ByVal Connected As Boolean) Implements IF_Communication.e_Connected
End Class
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Public Class TcpServer2
    Implements IF_Communication

    Private stream As NetworkStream
    'Private mydata() As Byte

    Private Server As TcpListener
    Private client As TcpClient

    Private m_Ip As String
    Private m_Port As String

    Private enabled As Boolean
    Private Encode As Encoding
    Private m_Connected As Boolean
    Private isrun As Boolean

    Private t_read As Thread
    Private t_connect As Thread
    Sub New()
        isrun = True
        enabled = False
        Me.Connected = False
        Encode = Encoding.Default
    End Sub

    Public Sub start() Implements IF_Communication.start
        t_connect = New Thread(AddressOf Me.auto_connect)
        t_connect.Start()
        t_read = New Thread(AddressOf Me.auto_read)
        t_read.Start()
    End Sub
    Public Sub close() Implements IF_Communication.close

        isrun = False
        enabled = False
        If Not client Is Nothing Then
            client.Close()
        End If
        If Not Server Is Nothing Then
            Server.Stop()
        End If
        If Not stream Is Nothing Then
            stream.Close()
        End If
    End Sub

    Public Function connect(ByVal ip As String, ByVal port As String) As Boolean Implements IF_Communication.connect
        Me.m_Ip = ip
        Me.m_Port = port
        enabled = True
    End Function

    Public Sub disconnect() Implements IF_Communication.disconnect

        Me.Connected = False
        enabled = False
        client.Close()
    End Sub
    Sub auto_connect()
        Dim temp_client As TcpClient
        While Me.isrun
            If Me.enabled Then
                Try

                    Server = New TcpListener(System.Net.IPAddress.Parse(m_Ip), CInt(m_Port))
                    Server.Start()
                    temp_client = Server.AcceptTcpClient()

                    If (client Is Nothing) Then
                    Else

                        Me.Connected = False
                        stream.Close()
                        client.Close()
                    End If
                    client = temp_client
                    stream = client.GetStream()
                    Server.Stop()
                    Me.Connected = True

                    Thread.Sleep(1)

                Catch ex As Exception
                    M_WriteLineMaster.WriteLine("[tcpServer.start]" & ex.ToString)
                End Try
            End If

        End While
    End Sub
    Public Event readData(ByVal data() As Byte) Implements IF_Communication.readData
    Sub auto_read()

        While Me.isrun
            Thread.Sleep(1000)
            If Me.enabled And Me.m_Connected Then
                If stream.CanRead Then
                    Dim dataByte As Byte()
                    dataByte = M_ClientReadWork.auto_readWork(Me.stream)
                    If dataByte IsNot Nothing Then
                        RaiseEvent readData(dataByte)
                    End If
                End If

            Else
                'Thread.Sleep(1000)
            End If

        End While
    End Sub

    Protected Property Connected() As Boolean
        Get
            Return m_Connected
        End Get
        Set(ByVal value As Boolean)
            m_Connected = value
            RaiseEvent e_Connected(value)
        End Set
    End Property
    Public Sub writeData(ByVal data() As Byte) Implements IF_Communication.writeData
        Try
            If m_Connected Then

                stream.Write(data, 0, data.Length)

            End If
        Catch ex As Exception

            M_WriteLineMaster.WriteLine("[CatchData.writedData]" + ex.ToString)
            Me.Connected = False

            client.Close()
            ' stream.Close()
            'Me.disconnect()
        End Try
    End Sub

    Public Function getConnected() As Boolean Implements IF_Communication.getConnected
        Return Me.m_Connected
    End Function

    Public Event e_Connected(ByVal Connected As Boolean) Implements IF_Communication.e_Connected

    Function TestConnected() As Boolean
        Dim testRecByte(0) As Byte
        Try
            If client.Connected And client.Client.Poll(0, SelectMode.SelectRead) Then
                If client.Client.Receive(testRecByte, SocketFlags.Peek) = 0 Then
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
End Class


Imports System.Text
Imports System.Net.Sockets
Imports System.Threading
'20181217
'<ObsoleteAttribute("未做", False)> _
Public Class serverSocket
    Implements IF_Communication2


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
    Private t_isConnecte As Thread
    Sub New()
        isrun = True
        enabled = False
        Me.m_Connected = False
        Encode = Encoding.Default
        t_connect = New Thread(AddressOf Me.auto_connect)
        t_connect.Start()
        t_read = New Thread(AddressOf Me.auto_read)
        t_read.Start()
        t_isConnecte = New Thread(AddressOf Me.auto_isConnecte)
        t_isConnecte.Start()
    End Sub
    Public Sub setConnect(ByVal ip As String, ByVal port As String)
        Me.m_Ip = ip
        Me.m_Port = port

    End Sub
    Public Sub setConnect(ByVal port As String)
        'Me.m_Ip = ip
        Me.m_Port = port
        'Console.WriteLine(Me.m_Port)
    End Sub
    Sub auto_connect()
        Dim temp_client As TcpClient
        While Me.isrun
            Thread.Sleep(1000)
            If Me.enabled Then
                Try
                    If m_Ip = "" Then
                        Server = New TcpListener(System.Net.IPAddress.Any, CInt(m_Port))
                    Else

                        Server = New TcpListener(System.Net.IPAddress.Parse(m_Ip), CInt(m_Port))
                    End If



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
                    'M_WriteLineMaster.WriteLine("[tcpServer.start]" & ex.ToString)
                    RaiseEvent err("[tcpServer.start]", ex)
                End Try
            End If

        End While
    End Sub

    Sub auto_read()

        While Me.isrun
            Thread.Sleep(1000)
            If Me.enabled And Me.Connected Then
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


    Function TestConnected() As Boolean
        Dim testRecByte(0) As Byte
        Try

            If client.Connected And client.Client.Poll(0, SelectMode.SelectRead) Then
                If client.Client.Receive(testRecByte, SocketFlags.Peek) = 0 Then
                    'M_WriteLineMaster.WriteLine("斷線" + testRecByte.Length.ToString)

                    Return False
                Else
                    'M_WriteLineMaster.WriteLine("連線" + testRecByte.Length.ToString)
                    Return True
                End If
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try


    End Function
    Sub auto_isConnecte()
        Dim count As Integer = 1
        While Me.isrun
            Thread.Sleep(1000)
            count = count + 1
            If count > 60 Then
                count = 1
                Connected = TestConnected()
            End If

            'Thread.Sleep(10000)
            'Console.WriteLine(Now.ToString("u") + " " + TestConnected().ToString)
        End While
    End Sub
#Region "IF_Communication2"

    Public Event e_Connected(ByVal Connected As Boolean) Implements IF_Communication2.e_commState
    'Public Event e_commState(status As Boolean)
    Protected Property Connected() As Boolean Implements IF_Communication2.commState
        Get
            Return m_Connected
        End Get
        Set(ByVal value As Boolean)
            m_Connected = value
            RaiseEvent e_Connected(value)
        End Set
    End Property
    Public Sub writeData(ByVal data() As Byte) Implements IF_Communication2.SendData
        Try
            If Connected Then

                stream.Write(data, 0, data.Length)

            End If
        Catch ex As Exception

            'M_WriteLineMaster.WriteLine("[CatchData.writedData]" + ex.ToString)
            RaiseEvent err("[CatchData.writedData]", ex)
            Me.Connected = False

            client.Close()
            ' stream.Close()
            'Me.disconnect()
        End Try
    End Sub
    Public Event readData(ByVal data() As Byte) Implements IF_Communication2.RecvData

    Public Sub start() Implements IF_Communication2.StartProc

        enabled = True
    End Sub
    Public Sub disconnect() Implements IF_Communication2.StopProc

        Me.Connected = False
        enabled = False
        Me.close()
    End Sub
    Public Sub close() Implements IF_Communication2.close
        'Console.WriteLine("close " + Me.m_Port)
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


    Public Event err(text As String, ex As Exception) Implements IF_Communication2.err
#End Region

End Class
Public Class serverSocket_use
    Sub New()

    End Sub

    Sub test01()
        Dim serverSocket As serverSocket = New serverSocket
        serverSocket.setConnect("127.0.0.1", 10001)
        serverSocket.start()
    End Sub
End Class
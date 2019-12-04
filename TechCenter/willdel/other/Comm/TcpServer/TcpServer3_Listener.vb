Imports System.Text
Imports System.Net.Sockets
Imports System.Threading
Imports classLibrary_bang
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Public Class TcpServer3_Listener
    Implements IF_Communication


    'Private stream As NetworkStream
    'Private mydata() As Byte

    Private Server As TcpListener
    '  Private client As TcpClient
    Private ClientList As List(Of TcpServer3_Client)
    Private m_Ip As String
    Private m_Port As String

    Private enabled As Boolean
    Private Encode As Encoding

    Private isrun As Boolean

    '   Private t_read As Thread
    Private t_connect As Thread
    Private t_ListTestConnected As Thread
    Private AutoResetEvent As AutoResetEvent = New AutoResetEvent(True)
    Sub New()
        isrun = True
        enabled = False
        ClientList = New List(Of TcpServer3_Client)
        Encode = Encoding.Default
    End Sub

    Public Sub start() Implements IF_Communication.start
        t_connect = New Thread(AddressOf Me.auto_connect)
        t_connect.Start()
        t_ListTestConnected = New Thread(AddressOf Me.List_TestConnected)
        t_ListTestConnected.Start()
        't_read = New Thread(AddressOf Me.auto_read)
        't_read.Start()
    End Sub
    Public Sub close() Implements IF_Communication.close

        isrun = False
        enabled = False
        'If Not client Is Nothing Then
        '    client.Close()
        'End If
        If Not Server Is Nothing Then
            Server.Stop()
        End If
        'If Not stream Is Nothing Then
        '    stream.Close()
        'End If


        ' Me.ClientList(index).ToString()
        While Me.ClientList.Count > 0
            Me.ClientList(0).close()
            RemoveHandler Me.ClientList(0).readData, AddressOf Me.List_Read
            RemoveHandler Me.ClientList(0).e_Connected, AddressOf Me.List_connected
            Me.ClientList.RemoveAt(0)
        End While




    End Sub

    Public Function connect(ByVal ip As String, ByVal port As String) As Boolean Implements IF_Communication.connect
        Me.m_Ip = ip
        Me.m_Port = port
        enabled = True
    End Function

    Public Sub disconnect() Implements IF_Communication.disconnect

        'Me.Connected = False
        enabled = False
        '  client.Close()
    End Sub
    Event ClientConnect(ByVal TcpServer3_Client As TcpServer3_Client)
    Sub auto_connect()
        Dim last_client As TcpClient
        While Me.isrun
            If Me.enabled Then
                Try

                    Server = New TcpListener(System.Net.IPAddress.Parse(m_Ip), CInt(m_Port))
                    Server.Start()
                    last_client = Server.AcceptTcpClient()
                    Dim t_TcpServer3_Client As New TcpServer3_Client(last_client)
                    Me.ClientList.Add(t_TcpServer3_Client)
                    AddHandler t_TcpServer3_Client.readData, AddressOf Me.List_Read
                    AddHandler t_TcpServer3_Client.e_Connected, AddressOf Me.List_connected
                    'If (client Is Nothing) Then
                    'Else

                    '    Me.Connected = False
                    '    ' stream.Close()
                    '    client.Close()
                    'End If
                    ' client = temp_client
                    'stream = client.GetStream()
                Catch ex As Exception
                    M_WriteLineMaster.WriteLine("[tcpServer.start]" & ex.ToString)
                Finally
                    Server.Stop()
                    ' Me.Connected = True
                    'Dim readIndex As Integer
                    'Dim t_count As Integer = Me.ClientList.Count - 1
                    'For index As Integer = 0 To t_count
                    '    ' Me.ClientList(index).ToString()
                    '    readIndex = t_count - index
                    '    If Not Me.ClientList(readIndex).TestConnected Then
                    '        Me.ClientList(readIndex).close()
                    '        Me.ClientList.RemoveAt(readIndex)
                    '    End If
                    'Next
                    Thread.Sleep(1)
                End Try


            End If

        End While
    End Sub
    'Public Event readData(ByVal data() As Byte) Implements IF_Communication.readData
    'Sub auto_read()

    '    While Me.isrun
    '        Thread.Sleep(1000)
    '        If Me.enabled And Me.m_Connected Then
    '            If stream.CanRead Then
    '                Dim dataByte As Byte()
    '                dataByte = M_ClientReadWork.auto_readWork(Me.stream)
    '                If dataByte IsNot Nothing Then
    '                    RaiseEvent readData(dataByte)
    '                End If
    '            End If

    '        Else
    '            'Thread.Sleep(1000)
    '        End If

    '    End While
    'End Sub

    'Protected Property Connected() As Boolean
    '    Get
    '        Return m_Connected
    '    End Get
    '    Set(ByVal value As Boolean)
    '        m_Connected = value
    '        RaiseEvent e_Connected(value)
    '    End Set
    'End Property
    'Public Sub writeData(ByVal data() As Byte) Implements IF_Communication.writeData
    '    Try
    '        If m_Connected Then

    '            stream.Write(data, 0, data.Length)

    '        End If
    '    Catch ex As Exception

    '        M_WriteLineMaster.WriteLine("[CatchData.writedData]" + ex.ToString)

    '        ' stream.Close()
    '        Me.disconnect()
    '    End Try
    'End Sub

    'Public Function getConnected() As Boolean Implements IF_Communication.getConnected
    '    Return Me.m_Connected
    'End Function

    'Public Event e_Connected(ByVal Connected As Boolean) Implements IF_Communication.e_Connected

    'Function TestConnected() As Boolean
    '    Dim testRecByte(0) As Byte
    '    Try
    '        If client.Connected And client.Client.Poll(0, SelectMode.SelectRead) Then
    '            If client.Client.Receive(testRecByte, SocketFlags.Peek) = 0 Then
    '                Console.WriteLine("斷線" + testRecByte.Length.ToString)
    '                Return False
    '            Else
    '                Console.WriteLine("連線" + testRecByte.Length.ToString)
    '                Return True
    '            End If
    '        End If
    '        Return True
    '    Catch ex As Exception
    '        Return False
    '    End Try


    'End Function

    Public Event readData(ByVal data() As Byte) Implements IF_Communication.readData

    Public Sub writeData(ByVal data() As Byte) Implements IF_Communication.writeData

    End Sub

    Public Event e_Connected(ByVal Connected As Boolean) Implements IF_Communication.e_Connected

    Public Function getConnected() As Boolean Implements IF_Communication.getConnected

    End Function

    Sub List_TestConnected()
        While Me.isrun
            Thread.Sleep(100)
            Try
                If Now.Second = 0 Then
                    Dim readIndex As Integer
                    Dim t_count As Integer = Me.ClientList.Count - 1
                    For index As Integer = 0 To t_count
                        ' Me.ClientList(index).ToString()
                        readIndex = t_count - index
                        If Not Me.ClientList(readIndex).TestConnected Then
                            Me.ClientList(readIndex).close()
                            RemoveHandler Me.ClientList(readIndex).readData, AddressOf Me.List_Read
                            RemoveHandler Me.ClientList(readIndex).e_Connected, AddressOf Me.List_connected
                            Me.ClientList.RemoveAt(readIndex)
                        End If
                    Next
                    Thread.Sleep(1000)
                End If
            Catch ex As Exception
                M_WriteLineMaster.WriteLine("[tcpServer.ListTestConnected]" & ex.ToString)
            End Try

        End While
    End Sub
    Sub List_Read(ByVal data As Byte())
        AutoResetEvent.WaitOne()
        RaiseEvent readData(data)
        ' Console.WriteLine(System.Text.Encoding.Default.GetString(data))
        AutoResetEvent.Set()
    End Sub
    Sub List_connected(ByVal connected As Boolean)
        AutoResetEvent.WaitOne()


        AutoResetEvent.Set()
    End Sub
End Class
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
'自回測試
Public Class udpserver_lock_sp
    Implements IF_Portbase

    'Private m_clientIp As String
    'Private m_clientPort As Integer
    'Private m_serverPort As Integer
    Protected Udp_par As Udp_par

    Protected m_UDPClient As UdpClient
    Protected m_Client_IPEndPoint As IPEndPoint
    Protected m_UDPServer As UdpClient
    Protected m_Server_IPEndPoint As IPEndPoint
    Protected thread_ServerReceive As Threading.Thread
    Protected ServerReceive_Enabled As Boolean
    'Private Class CSState
    '    Public RemoteIpEndPoint As IPEndPoint
    '    Public myUDPClient As UdpClient
    '    Public ReceiveBytes() As Byte
    'End Class
    'Private m_UDPType As UDPType
    'Private Enum UDPType
    '    client
    '    server
    'End Enum
    Protected otherPortBase As IF_Portbase
    Protected m_syncPoint As Integer
    Protected m_id As String
    Protected m_name As String
    Protected m_gprs As en_gprs_type

    'Protected gprs_filte As gprs_filte
    Sub New(t_Udp_par As Udp_par)

        'gprs_filte = New gprs_filte(Me)
        Udp_par = t_Udp_par
        ServerReceive_Enabled = True
        'udp無連線狀態
        m_syncPoint = enum_WorkStep.CLIENT_CONNECTED
        'Me.m_id = Udp_par.server_port
        Me.m_id = Udp_par.ID
        Me.m_name = t_Udp_par.ID
        RaiseEvent event_syncPoint(Udp_par.client_port, m_syncPoint)
    End Sub
    Sub New(t_Udp_par As Udp_par, t_gprs As en_gprs_type)
        m_gprs = t_gprs
        'gprs_filte = New gprs_filte(Me)
        Udp_par = t_Udp_par
        ServerReceive_Enabled = True
        'udp無連線狀態
        m_syncPoint = enum_WorkStep.CLIENT_CONNECTED
        'Me.m_id = Udp_par.server_port
        Me.m_id = Udp_par.ID
        Me.m_name = t_Udp_par.ID
        RaiseEvent event_syncPoint(Udp_par.client_port, m_syncPoint)
    End Sub
    Sub New(t_clientIp As String, t_clientPort As Integer, t_serverPort As Integer)
        'gprs_filte = New gprs_filte(Me)
        Udp_par = New Udp_par
        Udp_par.client_ip = t_clientIp
        Udp_par.client_port = t_clientPort
        Udp_par.server_port = t_serverPort

        'Me.m_clientIp = t_clientIp
        'Me.m_clientPort = t_clientPort
        'Me.m_serverPort = t_serverPort

        ServerReceive_Enabled = True
        'udp無連線狀態
        m_syncPoint = enum_WorkStep.CLIENT_CONNECTED
        'Me.m_id = Udp_par.server_port
        Me.m_id = Udp_par.ID
        RaiseEvent event_syncPoint(Udp_par.client_port, m_syncPoint)
    End Sub
    'Sub New(t_serverPort As Integer)
    '    'server
    '    'Me.m_clientIp = t_clientIp
    '    'Me.m_clientPort = t_clientPort
    '    Me.m_serverPort = t_serverPort
    '    m_UDPType = UDPType.server
    'End Sub
    'Sub New(t_clientIp As String, t_clientPort As Integer)
    '    'client
    '    Me.m_clientIp = t_clientIp
    '    Me.m_clientPort = t_clientPort
    '    'Me.m_serverPort = t_serverPort
    '    'm_UDPType = UDPType.client
    'End Sub

    Sub AddressOf_ServerReceive()
        Dim ReceiveBytes As Byte()
        While ServerReceive_Enabled

            Try
                ReceiveBytes = m_UDPServer.Receive(m_Server_IPEndPoint)
                '取得對應的ReceiveBytes
                Me.PassMsg(ReceiveBytes)
                'Dim textString As String = System.Text.Encoding.Default.GetString(ReceiveBytes)
                'Console.WriteLine("ServerReceive " + m_serverPort.ToString + " : " + textString)
            Catch scex As System.Net.Sockets.SocketException

            Catch ex As Exception

                MessageBox.Show(ex.ToString)
            End Try
        End While
    End Sub

    Shared Sub test()
        Dim myUDPClient As New UdpClient()

        Dim ServerIpAddress As IPAddress
        Try
            ServerIpAddress = IPAddress.Parse("127.0.0.1")
        Catch ex As Exception
            MessageBox.Show("Server IP設定錯誤")
            Exit Sub
        End Try
        Dim iPort As Integer
        iPort = 34567
        Dim RemoteIpEndPoint As New IPEndPoint(ServerIpAddress, iPort)
        Dim myBytes As Byte()
        myBytes = Encoding.GetEncoding(950).GetBytes(Trim("test"))
        If myBytes.Length > 0 Then
            myUDPClient.Send(myBytes, myBytes.Length, RemoteIpEndPoint)
        Else
            MessageBox.Show("無資料可傳送!!")
        End If
    End Sub
#Region "IF_Portbase"
    '斷線
    Public Sub Disconnect() Implements IF_Portbase.Disconnect
        'Me.ServerReceive_Enabled = False
        'If Me.m_UDPClient IsNot Nothing Then
        '    Me.m_UDPClient.Close()
        'End If

        'If Me.m_UDPServer IsNot Nothing Then
        '    Me.m_UDPServer.Close()
        'End If
        '無連線狀態

    End Sub

    Public Sub FormShowExit() Implements IF_Portbase.FormShowExit
        Me.StopProc(True)
        ' Me.ServerReceive_Enabled = False
    End Sub
    '程式啟動
    Public Function StartProc() As Boolean Implements IF_Portbase.StartProc
        '建立udpClient

        m_UDPClient = New UdpClient
        Dim ServerIpAddress As IPAddress
        Try
            ServerIpAddress = IPAddress.Parse(Udp_par.client_ip)
        Catch ex As Exception
            MessageBox.Show("Server IP設定錯誤")
            Return False
        End Try
        m_Client_IPEndPoint = New IPEndPoint(ServerIpAddress, Udp_par.client_port)


        m_UDPServer = New UdpClient(Udp_par.server_port)
        'm_UDPServer = New UdpClient()
        m_Server_IPEndPoint = New IPEndPoint(IPAddress.Any, 0)
        thread_ServerReceive = New Threading.Thread(AddressOf AddressOf_ServerReceive)
        thread_ServerReceive.Start()
        m_syncPoint = enum_WorkStep.CLIENT_CONNECTED
        RaiseEvent event_syncPoint(Udp_par.client_port, m_syncPoint)
        Return True
    End Function
    '程式停止
    Public Sub StopProc(dispose As Boolean) Implements IF_Portbase.StopProc

        Me.ServerReceive_Enabled = False
        If Me.m_UDPClient IsNot Nothing Then
            Me.m_UDPClient.Close()
            Me.m_UDPClient = Nothing
        End If

        If Me.m_UDPServer IsNot Nothing Then
            Me.m_UDPServer.Close()
            Me.m_UDPServer = Nothing
        End If
        m_syncPoint = enum_WorkStep.STOPED
        RaiseEvent event_syncPoint(Udp_par.client_port, m_syncPoint)

        'If Me.syncPoint <> enum_TcpServerWorkStep.STOPED Then
        '    If Me.m_client IsNot Nothing Then
        '        Me.m_client.Close()
        '    End If
        '    Me.m_server.Stop()
        '    ' Start the control thread that shuts off the timer.
        '    Dim t As New Thread(AddressOf StopThreadProc)
        '    t.Start(dispose)
        'ElseIf dispose Then
        '    Me.syncPoint = enum_TcpServerWorkStep.DISPOSED
        '    Me.InvokeStopMsg(Me.listenPort & " Finished." & vbCrLf)
        'End If
    End Sub

    Protected Overridable Sub PassMsg(data() As Byte) Implements IF_Portbase.PassMsg

        If Me.otherPortBase IsNot Nothing Then
            Me.otherPortBase.SendMsg(data)
        End If
        Dim newdata As Byte() = data

        SendMsg(newdata)
        'Dim newdata As Byte() = gprs_filte.filte(data)
        '  If False Then
        ' Dim info As String = System.Text.Encoding.Default.GetString(data)
        Dim info As String = "[Recv]" & vbCrLf & BitConverter.ToString(newdata, 0, newdata.Length)
        Dim msg As String = String.Format("{0} - {1} - {2} - {3}", Udp_par.server_port.ToString, DateTime.Now.ToShortDateString(), DateTime.Now.ToString("HH:mm:ss"), info)
        RaiseEvent event_PassMsg_toForm_id(Udp_par.ID, msg)
        '  End If
    End Sub
    Public Overridable Sub SendMsg(data() As Byte) Implements IF_Portbase.SendMsg
        '新北中心需求
        'Dim newdata(data.Length + 1) As Byte
        'Try
        '    For index As Integer = 0 To data.Length - 1
        '        newdata(2 + index) = data(index)
        '    Next
        'Catch ex As Exception

        'End Try

        'newdata(0) = data(3)
        'newdata(1) = data(4)
        'If m_UDPClient IsNot Nothing Then
        '    m_UDPClient.Send(data, data.Length, m_Client_IPEndPoint)

        'End If
        'If m_UDPServer.Client.LocalEndPoint Is Nothing Then
        '    m_UDPServer.Send(data, data.Length, m_UDPServer.Client.LocalEndPoint)
        'End If
        'If m_UDPServer.Client.RemoteEndPoint Is Nothing Then
        '    m_UDPServer.Send(data, data.Length, m_UDPServer.Client.RemoteEndPoint)
        'End If
        'If m_UDPServer IsNot Nothing Then
        '    'm_UDPServer.Send(data, data.Length, m_Server_IPEndPoint)
        '    m_UDPServer.Send(data, data.Length)
        'End If
        m_UDPServer.Send(data, data.Length, m_Server_IPEndPoint)
        '  If False Then
        Dim info As String = "[Send]" & vbCrLf & BitConverter.ToString(data, 0, data.Length)
        Dim msg As String = String.Format("{0} - {1} - {2} - {3}", Udp_par.server_port.ToString, DateTime.Now.ToShortDateString(), DateTime.Now.ToString("HH:mm:ss"), info)
        RaiseEvent event_SendMsg_toForm_id(Udp_par.ID, msg)
        'End If
    End Sub
    Public Sub setOtherPortbase(t_otherPortbase As IF_Portbase) Implements IF_Portbase.setOtherPortbase
        Me.otherPortBase = t_otherPortbase
    End Sub
    Public ReadOnly Property syncPoint As enum_WorkStep Implements IF_Portbase.syncPoint
        Get
            Return m_syncPoint
        End Get
    End Property
    Public Event event_syncPoint(index As Integer, syncPoint As enum_WorkStep) Implements IF_Portbase.event_syncPoint

    Public Event event_PassMsg(data() As Byte) Implements IF_Portbase.event_PassMsg

    Public Event event_SendMsg(data() As Byte) Implements IF_Portbase.event_SendMsg

    Public Event event_PassMsg_toForm(index As Integer, Form_Test As String) Implements IF_Portbase.event_PassMsg_toForm

    Public Event event_SendMsg_toForm(index As Integer, Form_Test As String) Implements IF_Portbase.event_SendMsg_toForm

    Public Property ID As String Implements IF_Portbase.index
        Get
            Return m_id
        End Get
        Set(value As String)
            m_id = value
        End Set
    End Property
#End Region


    Public Property name As String Implements IF_Portbase.name
        Get
            Return m_name
        End Get
        Set(value As String)
            m_name = value
        End Set
    End Property
    Public ReadOnly Property gprstype As en_gprs_type 'Implements IF_msgpass_plus.gprstype
        Get
            Return m_gprs
        End Get
    End Property
    Protected Function filte(data() As Byte) As Byte() 'Implements IF_msgpass_plus.filte
        Select Case Me.gprstype
            Case en_gprs_type.none
                Return data
            Case en_gprs_type.ma89i_Client_03

                'Case en_gprs_type.MLB_G1101_02
                '    Dim MLB_G1101 As New MLB_G1101
                '    Return MLB_G1101.readData(data)
            Case en_gprs_type.ma89i_Client_03
                Return data
            Case Else
                Return data
        End Select
    End Function

#Region "event"
    Protected Sub raise_event_PassMsg_toForm(index As Integer, Form_Test As String)
        RaiseEvent event_PassMsg_toForm(index, Form_Test)
    End Sub
    Protected Sub RaiseEvent_event_SendMsg_toForm(server_port As Integer, msg As String)
        RaiseEvent event_SendMsg_toForm(server_port, msg)
    End Sub
    Protected Sub raise_event_PassMsg_toForm_id(id As String, Form_Test As String)
        RaiseEvent event_PassMsg_toForm_id(id, Form_Test)
    End Sub
    Protected Sub RaiseEvent_event_SendMsg_toForm_id(id As String, msg As String)
        RaiseEvent event_SendMsg_toForm_id(id, msg)
    End Sub


#End Region

    Public Event event_PassMsg_toForm_id(id As String, Form_Test As String) Implements IF_Portbase.event_PassMsg_toForm_id

    Public Event event_SendMsg_toForm_id(id As String, Form_Test As String) Implements IF_Portbase.event_SendMsg_toForm_id

    Public ReadOnly Property gprstype1 As en_gprs_type Implements IF_Portbase.gprstype
        Get
            Return m_gprs
        End Get
    End Property
End Class
Public Class Udp_par
    Public ID As String
    Public client_ip As String
    Public client_port As Integer
    Public server_ip As String
    Public server_port As Integer
End Class
Public Enum en_comm_type
    tcpserver
    tcpserver_two
    tcpclient
    tcpclient_two
    udp
    udp_two
    udpclient
    udpserver
End Enum
Public Enum en_gprs_type
    none
    ma89i_01
    MLB_G1101_02
    ma89i_Client_03
    itri '偵測器id分辨
    ceci_04 '增加設備編號
    temp
End Enum
Public Enum enum_WorkStep
    STOPED
    STARTED
    CLIENT_BREAK
    CLIENT_CONNECTED
    DISPOSED
End Enum
Public Interface IF_Portbase
    Property index As String
    Property name As String
    Sub setOtherPortbase(t_otherPortbase As IF_Portbase)
    '程式啟動
    Function StartProc() As Boolean
    '程式停止
    Sub StopProc(ByVal dispose As Boolean)

    Sub FormShowExit()

    '斷線
    Sub Disconnect()

    'passmsg
    Sub PassMsg(data As Byte())
    Sub SendMsg(data As Byte())
    Event event_PassMsg(data As Byte())
    Event event_SendMsg(data As Byte())
    Event event_PassMsg_toForm(index As Integer, Form_Test As String)
    Event event_SendMsg_toForm(index As Integer, Form_Test As String)
    Event event_PassMsg_toForm_id(id As String, Form_Test As String)
    Event event_SendMsg_toForm_id(id As String, Form_Test As String)
    ReadOnly Property syncPoint As enum_WorkStep
    Event event_syncPoint(index As Integer, syncPoint As enum_WorkStep)
    ReadOnly Property gprstype As en_gprs_type
End Interface
 
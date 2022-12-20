Imports System.IO
Imports System.IO.Ports
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports System.Text


'Imports IF_TC
' Original Copy form 'WinCE_Com2TCP'
Public Class EndTCPClient_0

    Public Event RecvDataFromTCPClient(ByVal commData() As Byte)
    Public Event ShowCommState(ByVal connected As Boolean, ByVal msg As String)
    ' Communication ports
    Private m_Socket As Socket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp)
    Public remoteEP As IPEndPoint

    Private manualExitFlag As Boolean = False

    Protected AutoResetEvent As AutoResetEvent = New AutoResetEvent(True)
    Dim comm_state As Boolean = False
    Public Property commState() As Boolean
        Get
            Return Me.comm_state
        End Get
        Set(ByVal value As Boolean)
            Me.comm_state = value
        End Set
    End Property

    Dim active_comm_state As Boolean = False
    Public Property activeCommState() As Boolean
        Get
            Return Me.active_comm_state
        End Get
        Set(ByVal value As Boolean)
            Me.active_comm_state = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Function StartProc(ByVal remote_ip As String, ByVal remote_port As Integer) As Boolean
        SyncLock lockStartStop
            Me.remoteEP = New IPEndPoint(IPAddress.Parse(remote_ip), remote_port)
            Me.manualExitFlag = False
            Dim ConcRecv_Thread As Thread = New Thread(AddressOf SocketProcess)
            ConcRecv_Thread.Start()

            Me.startResult = True
        End SyncLock

        Return Me.startResult
    End Function

    Dim startResult As Boolean = False
    Private Shared lockStartStop As New Object

    Public Sub StopProc()
        SyncLock lockStartStop
            If Me.startResult Then
                Me.manualExitFlag = True
                ' Release the socket.
                If Me.m_Socket.Connected Then
                    Me.m_Socket.Shutdown(SocketShutdown.Both)
                End If
                Me.m_Socket.Close()
                AutoResetEvent.WaitOne()

                Me.startResult = False
            End If
        End SyncLock
    End Sub

    Private Sub SocketProcess()
        While Not Me.manualExitFlag
            ' Create a TCP/IP socket.
            Me.m_Socket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp)
            ' m_Socket.ReceiveTimeout = 180000
            Dim connectErrFlag As Boolean
            While Not Me.m_Socket.Connected
                Try
                    ' Connect to the remote endpoint.
                    Me.m_Socket.Connect(Me.remoteEP)
                Catch es As SocketException
                    Me.InvokeCommState(False, "m_Socket.Connect ErrorCode:" + es.ErrorCode.ToString() + es.Message)
                    ' 10038嘗試操作的對象不是通訊端。
                    ' 10053連線已被您主機上的軟體中止。
                    ' 10060連線嘗試失敗，因為連線對象有一段時間並未正確回應，或是連線建立失敗，因為連線的主機無法回應
                    ' 10061無法連線，因為目標電腦拒絕連線
                    ' 10065通訊端操作無法連線到主機
                    Thread.Sleep(1000)
                    If Not manualExitFlag Then
                        ' Connect to the remote endpoint.
                        Continue While
                    End If
                Catch eo As ObjectDisposedException
                    Me.InvokeCommState(False, "m_Socket.Connect Fail:" & eo.Message) ' 無法存取已處置的物件, manualExitFlag = True
                    Exit While
                Catch ex As Exception
                    connectErrFlag = True
                    ' 發生不明錯誤???
                    Dim errMsg As String = "m_Socket.Connect Fail:" & ex.ToString()
                    Me.InvokeCommState(False, errMsg)
                    MessageBox.Show(errMsg & vbCrLf & "SocketProcess Stoped! Please Disconnect and Reconnect.")
                    Me.LogErr(errMsg)
                    Exit While
                End Try
            End While

            If Me.manualExitFlag Or connectErrFlag Then
                Me.InvokeCommState(False, "Me.manualExitFlag Or connectErrFlag")
                Exit While
            End If
            Me.InvokeCommState(True, "Connect To " & Me.remoteEP.ToString())
            'ii_lib.M_DeviceServer_VDC.
            Dim recvBuffer(1023) As Byte
            Dim recvErrFlag As Boolean = False
            While True
                Dim bytesRead As Integer = -1
                Try
                    ' Begin receiving the data from the remote device.
                    bytesRead = m_Socket.Receive(recvBuffer)
                Catch es As SocketException
                    Select Case es.ErrorCode
                        Case 10004, 10054
                            ' 10004-中止操作被 WSACancelBlockingCall 呼叫打斷, manualExitFlag = True
                            ' 10054-遠端主機已強制關閉一個現存的連線
                            Me.InvokeCommState(False, "m_Socket.Receive ErrorCode:" + es.ErrorCode.ToString() + es.Message)
                        Case 10060, 10035 ' 連線嘗試失敗，因為連線對象有一段時間並未正確回應，或是連線建立失敗，因為連線的主機無法回應。
                            ' 10060 無法從傳輸連接讀取資料: (m_Socket.ReceiveTimeout)
                            ' 連線嘗試失敗, 因為連線對象有一段時間並未正確回應, 或是連線建立失敗, 因為連線的主機無法回應
                            ' 10035 無法從傳輸連接讀取資料: (m_clientStream.ReadTimeout)
                            ' 無法立即完成通訊端操作，而且無法停止。                            
                            Me.InvokeCommState(False, "m_Socket.Receive TIME_OUT ErrorCode:" + es.ErrorCode.ToString() + es.Message)
                        Case Else
                            recvErrFlag = True
                            ' 發生不明錯誤???
                            Dim errMsg As String = "m_Socket.Receive Fail:" & es.ToString()
                            Me.InvokeCommState(False, errMsg)
                            MessageBox.Show(errMsg & vbCrLf & "SocketProcess Stoped! Please Disconnect and Reconnect.")
                            Me.LogErr(errMsg)
                    End Select
                Catch eo As ObjectDisposedException
                    Me.InvokeCommState(False, "m_Socket.Receive:" & eo.Message) ' 無法存取已處置的物件, manualExitFlag = True            
                Catch ex As Exception
                    recvErrFlag = True
                    ' 發生不明錯誤???
                    Dim errMsg As String = "m_Socket.Receive Fail:" & ex.ToString()
                    Me.InvokeCommState(False, errMsg)
                    MessageBox.Show(errMsg & vbCrLf & "SocketProcess Stoped! Please Disconnect and Reconnect.")
                    Me.LogErr(errMsg)
                End Try

                If Me.manualExitFlag Or recvErrFlag Then
                    Me.InvokeCommState(False, "Me.manualExitFlag Or recvErrFlag")
                    Exit While
                End If

                If bytesRead > 0 Then
                    Dim recvBuf(bytesRead - 1) As Byte
                    Array.Copy(recvBuffer, recvBuf, bytesRead)
                    RaiseEvent RecvDataFromTCPClient(recvBuf)

                    Continue While
                Else
                    Me.InvokeCommState(False, "Connection Break!")
                    ' Release the socket.
                    Me.m_Socket.Close()
                    Exit While
                End If
            End While
        End While

        AutoResetEvent.Set()
    End Sub

    Public Sub SendData(ByVal commdata() As Byte)
        If Me.m_Socket.Connected Then
            Dim bytesSendDone As Integer = 0
            While bytesSendDone < commdata.Length
                Dim bytesSend As Integer = 0
                Try
                    ' Begin sending the data to the remote device.
                    bytesSend = Me.m_Socket.Send(commdata, bytesSendDone, (commdata.Length - bytesSendDone), SocketFlags.None)
                Catch ex As Exception
                    Me.InvokeCommState(False, "m_Socket.Send Fail:" & ex.ToString())
                    Exit Sub
                End Try
                bytesSendDone += bytesSend
            End While

            Me.InvokeCommState(True, "m_Socket.Send " & commdata.Length.ToString() & " Bytes")
        End If
    End Sub
    'Public Sub SendDataToTCPClient(Data As Byte())
    '    If Me.m_Socket.Connected Then

    '        m_Socket.Send(Data)

    '        Me.InvokeCommState(True, "m_Socket.Send " & commdata.Length.ToString() & " Bytes")
    '    End If
    'End Sub

    Private Sub InvokeCommState(ByVal state As Boolean, ByVal info As String)
        Me.commState = state

        If Me.activeCommState Or Me.activeLogMsg Then
            Dim logTime As DateTime = DateTime.Now
            Dim msg As String = String.Format("{0} - {1} - {2} - {3}{4}", _
                                            Me.remoteEP.ToString(), logTime.ToShortDateString(), logTime.ToString("HH:mm:ss"), info, vbCrLf)
            If Me.activeCommState Then
                RaiseEvent ShowCommState(state, msg)
            End If
            If Me.activeLogMsg Then
                Me.LogMsg(msg, logTime)
            End If
        End If
    End Sub
#Region "temp"
    Dim active_log_msg As Boolean = False
    Public Property activeLogMsg() As Boolean
        Get
            Return Me.active_log_msg
        End Get
        Set(ByVal value As Boolean)
            Me.active_log_msg = value
        End Set
    End Property

    Private Sub LogMsg(ByVal msg As String, ByVal logTime As DateTime)

    End Sub
    Private Sub LogErr(ByVal info As String)

    End Sub

#End Region


End Class

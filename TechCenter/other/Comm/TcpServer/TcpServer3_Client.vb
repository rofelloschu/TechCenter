Imports System.Text
Imports System.Net.Sockets
Imports System.Threading
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Public Class TcpServer3_Client
    Private client As TcpClient
    Private stream As NetworkStream

    Private t_read As Thread

    Private m_Connected As Boolean
    Private isrun As Boolean
    Private enabled As Boolean

    Private Remote_IP As String
    Private Remote_Port As String
    Sub New(ByVal t_client As TcpClient)
        client = t_client
        stream = client.GetStream
        Connected = True
        isrun = True
        enabled = True

        t_read = New Thread(AddressOf Me.auto_read)
        t_read.Start()
    End Sub
    Sub close()

        isrun = False
        enabled = False
        Connected = False
        If Not client Is Nothing Then
            client.Close()
        End If

        If Not stream Is Nothing Then
            stream.Close()
        End If
    End Sub
    Public Event readData(ByVal data() As Byte)
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
    Public Event e_Connected(ByVal Connected As Boolean)

    Public Sub writeData(ByVal data() As Byte)
        Try
            If m_Connected Then

                stream.Write(data, 0, data.Length)

            End If
        Catch ex As Exception

            M_WriteLineMaster.WriteLine("[CatchData.writedData]" + ex.ToString)

            stream.Close()
            'Me.disconnect()
        End Try
    End Sub
    Protected Property Connected() As Boolean
        Get
            Return m_Connected
        End Get
        Set(ByVal value As Boolean)
            Me.Remote_IP = Me.client.Client.RemoteEndPoint.ToString.Split(":")(0)
            Me.Remote_Port = Me.client.Client.RemoteEndPoint.ToString.Split(":")(1)
            M_WriteLineMaster.WriteLine(Me.Remote_IP + " " + Me.Remote_Port)
            m_Connected = value
            RaiseEvent e_Connected(value)
        End Set
    End Property
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
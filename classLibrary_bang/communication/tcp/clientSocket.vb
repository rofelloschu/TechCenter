Imports System.Text
Imports System.Net.Sockets
Imports System.Threading
'20180816
Imports classLibrary_bang

Public Class clientSocket
    Implements IF_Communication2


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
        t_connect = New Thread(AddressOf Me.auto_connect)
        t_connect.Start()
        t_read = New Thread(AddressOf Me.auto_read)
        t_read.Start()

    End Sub


    Public Sub setConnect(ByVal ip As String, ByVal port As String)
        Me.m_ip = ip
        Me.m_port = port

    End Sub

    Protected Sub auto_connect()
        While Me.isrun
            Thread.Sleep(1000)
            If Me.enabled Then
                Try
                    Thread.Sleep(1000)
                    If Me.Connected Then
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
                    'M_WriteLineMaster.WriteLine("[TcpClient2.auto_connect]" + ex.ToString)
                    RaiseEvent err("[TcpClient2.auto_connect]", ex)
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
    Protected Sub auto_read()

        While Me.isrun
            Thread.Sleep(1)
            If Me.enabled And Me.Connected Then
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

#Region ""

    Public Event e_Connected(ByVal Connected As Boolean) Implements IF_Communication2.e_commState


    Property Connected() As Boolean Implements IF_Communication2.commState
        Get
            Return m_Connected
        End Get
        Set(ByVal value As Boolean)
            m_Connected = value
            RaiseEvent e_Connected(value)
        End Set
    End Property


    Public Event readData(ByVal data() As Byte) Implements IF_Communication2.RecvData
    Public Sub writeData(ByVal data() As Byte) Implements IF_Communication2.SendData
        Try
            If Connected Then

                stream.Write(data, 0, data.Length)

            End If
        Catch ex As Exception

            'M_WriteLineMaster.WriteLine("[CatchData.writedData]" + ex.ToString)
            RaiseEvent err("[CatchData.writedData]", ex)
            ' stream.Close()
            Me.StopProc()
        End Try

    End Sub



    Public Sub StartProc() Implements IF_Communication2.StartProc
        enabled = True

    End Sub

    Public Sub StopProc() Implements IF_Communication2.StopProc
        Me.Connected = False
        enabled = False
        If Not (Client Is Nothing) Then
            Client.Close()
        End If
    End Sub
    Public Sub close() Implements IF_Communication2.close

        isrun = False
        enabled = False
        ' Me.Client.Close()
        If Not (Client Is Nothing) Then
            Client.Close()
        End If
        If Not (stream Is Nothing) Then
            stream.Close()
        End If
        Me.Connected = False
    End Sub
#End Region

    Public Event err(text As String, ex As Exception) Implements IF_Communication2.err

End Class

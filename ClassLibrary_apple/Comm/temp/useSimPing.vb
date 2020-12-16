'20150525
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Public Class useSimPing

    Private Thread_ping As Thread
    Private Thread_ping_timeout As Thread
    Private pingip As String
    Private Ping_while As Boolean = True
    Private PingEndMessage As String
    Event PingEventMessage(ByVal msg As String, ByVal sender As useSimPing)
    Sub New(ByVal ip As String)
        pingip = ip

    End Sub
    Sub startPing()
        Thread_ping_timeout = New Thread(AddressOf AddressOf_ping_timeout)
        Thread_ping = New Thread(AddressOf AddressOf_Ping)
        Thread_ping.Start()
    End Sub
    Sub AddressOf_Ping()
        Try

            PingEndMessage = ""
            Dim data As Byte() = New Byte(1023) {}
            Dim recv As Integer
            'Dim host As New Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp)

            Dim packet As New comm.Custom_Pingpacket()

            packet.Type = &H8
            packet.Code = &H0
            packet.Checksum = 0
            Buffer.BlockCopy(BitConverter.GetBytes(CShort(1)), 0, packet.Message, 0, 2)
            Buffer.BlockCopy(BitConverter.GetBytes(CShort(1)), 0, packet.Message, 2, 2)
            data = Encoding.ASCII.GetBytes("Ping Test")
            Buffer.BlockCopy(data, 0, packet.Message, 4, data.Length)
            packet.MessageSize = data.Length + 4
            Dim packetsize As Integer = packet.MessageSize + 4

            Dim chcksum As UInt16 = packet.getChecksum()
            packet.Checksum = chcksum

            Dim iep As New IPEndPoint(IPAddress.Parse(pingip), 0)
            Dim ep As EndPoint = DirectCast(iep, EndPoint)
            'host.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000)
            Thread_ping_timeout.Start()
            '  Console.WriteLine("Ping " + Me.pingip + " ...")
            Dim host As New Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp)
            Dim sendData As Byte() = packet.getBytes()
            ' host.SendTo(packet.getBytes(), packetsize, SocketFlags.None, iep)
            host.SendTo(sendData, sendData.Length, SocketFlags.None, iep)


            Ping_while = True
            Dim Starttime As Integer = System.Environment.TickCount
            While Ping_while
                If host.Available Then

                    Try

                        data = New Byte(1023) {}
                        recv = host.ReceiveFrom(data, ep)
                        Ping_while = False
                    Catch generatedExceptionName As SocketException
                        Console.WriteLine("No response from remote host")
                        Console.WriteLine(pingip + " time Out")
                        PingEndMessage = pingip + " time out"
                        RaiseEvent PingEventMessage(PingEndMessage, Me)
                        Return
                    Catch ex As Exception
                        Console.WriteLine(ex.ToString)
                        PingEndMessage = pingip + " Scoket ERROR"
                        RaiseEvent PingEventMessage(PingEndMessage, Me)
                        Return
                    End Try
                End If

            End While
            Starttime = System.Environment.TickCount - Starttime
            'Timer3.Enabled = False
            If data.Length < 22 Then

                'Console.WriteLine(pingip + " time out")
                PingEndMessage = pingip + " time out"
            Else
                Dim response As New comm.Custom_Pingpacket(data, recv)
                '  Console.WriteLine("response from: {0}", ep.ToString())
                'Console.WriteLine("  Type {0}", response.Type)
                ' Console.WriteLine("  Code: {0}", response.Code)
                Dim Identifier As Integer = BitConverter.ToInt16(response.Message, 0)
                Dim Sequence As Integer = BitConverter.ToInt16(response.Message, 2)
                ' Console.WriteLine("  Identifier: {0}", Identifier)
                ' Console.WriteLine("  Sequence: {0}", Sequence)
                '  Console.WriteLine("  MS: {0}", Starttime.ToString + " MS")
                Dim stringData As String = Encoding.ASCII.GetString(response.Message, 4, response.MessageSize - 4)
                '  Console.WriteLine("  data: {0}", stringData)
                '  Console.WriteLine("  data size: {0}", recv.ToString)
                PingEndMessage = pingip + " time " + Starttime.ToString + " MS"
            End If


            host.Close()

        Catch ex As Exception
            PingEndMessage = pingip + " ERROR"

        End Try
        Console.WriteLine(PingEndMessage)
        RaiseEvent PingEventMessage(PingEndMessage, Me)
    End Sub
    Sub AddressOf_ping_timeout()
        Dim nowtime As DateTime = Now
        While True

            If nowtime.AddSeconds(5) < Now Then
                Ping_while = False
                Exit While
            End If
        End While
    End Sub
    'Shared Function PingOne(ByVal ip As String) As String

    '    Return PingEndMessage
    'End Function
    'Sub x()
    '    RaiseEvent PingEventMessage(PingEndMessage, Me)
    'End Sub
    Sub close()
        Ping_while = False
    End Sub
End Class

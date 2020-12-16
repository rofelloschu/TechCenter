'20150525
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Namespace temp
    Public Class usePing
        Private host As New Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp)
        Private t As Thread
        Private t_time As Thread
        Private pingip As String
        Private testPing_while As Boolean = True
        Sub New(ByVal ip As String)
            pingip = ip
            t_time = New Thread(AddressOf time)
            t = New Thread(AddressOf testPing)
            t.Start()
        End Sub
        Sub testPing()
            Dim data As Byte() = New Byte(1023) {}
            Dim recv As Integer
            'Dim host As New Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp)
            Dim iep As New IPEndPoint(IPAddress.Parse(pingip), 0)
            Dim ep As EndPoint = DirectCast(iep, EndPoint)
            Dim packet As New comm.Custom_Pingpacket()

            packet.Type = &H8
            packet.Code = &H0
            packet.Checksum = 0
            Buffer.BlockCopy(BitConverter.GetBytes(CShort(1)), 0, packet.Message, 0, 2)
            Buffer.BlockCopy(BitConverter.GetBytes(CShort(1)), 0, packet.Message, 2, 2)
            data = Encoding.ASCII.GetBytes("test packet")
            Buffer.BlockCopy(data, 0, packet.Message, 4, data.Length)
            packet.MessageSize = data.Length + 4
            Dim packetsize As Integer = packet.MessageSize + 4

            Dim chcksum As UInt16 = packet.getChecksum()
            packet.Checksum = chcksum

            host.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000)
            ' t_time.Start()
            Console.WriteLine("Ping " + Me.pingip + " ...")
            host.SendTo(packet.getBytes(), packetsize, SocketFlags.None, iep)
            ' testPing_while = True
            'While testPing_while
            '    If host.Available Then
            Dim Starttime As Integer = System.Environment.TickCount
            Try

                data = New Byte(1023) {}
                recv = host.ReceiveFrom(data, ep)
            Catch generatedExceptionName As SocketException
                Console.WriteLine("No response from remote host")
                Console.WriteLine(pingip + " time Out")
                Return
            Catch ex As Exception
                Console.WriteLine(ex.ToString)
                Return
            End Try
            '    End If

            'End While
            Starttime = System.Environment.TickCount - Starttime
            'Timer3.Enabled = False
            Dim response As New comm.Custom_Pingpacket(data, recv)
            Console.WriteLine("response from: {0}", ep.ToString())
            Console.WriteLine("  Type {0}", response.Type)
            Console.WriteLine("  Code: {0}", response.Code)
            Dim Identifier As Integer = BitConverter.ToInt16(response.Message, 0)
            Dim Sequence As Integer = BitConverter.ToInt16(response.Message, 2)
            Console.WriteLine("  Identifier: {0}", Identifier)
            Console.WriteLine("  Sequence: {0}", Sequence)
            Console.WriteLine("  delay: {0}", Starttime.ToString + " MS")
            Dim stringData As String = Encoding.ASCII.GetString(response.Message, 4, response.MessageSize - 4)
            Console.WriteLine("  data: {0}", stringData)
            Console.WriteLine("  data size: {0}", recv.ToString)


            host.Close()

        End Sub

        Sub time()
            Dim nowtime As DateTime = Now
            While True

                If nowtime.AddSeconds(5) < Now Then
                    testPing_while = False
                    Exit While
                End If
            End While
        End Sub

    End Class
End Namespace


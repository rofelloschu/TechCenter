'20180105
Imports System.Net
Public Class Work_Ping2
    'Inherits Work_base
    'Public Event reportPing(ByVal IsEcho As Boolean)

    Sub New()
        'MyBase.new()
    End Sub
    Sub close()

    End Sub
    Public Shared Function IP_Parse(ByVal ip As String) As Boolean
        Try
            Dim IPAddress As System.Net.IPAddress = System.Net.IPAddress.Parse(ip)
            Return True
        Catch ex As Exception
            Return False
        End Try


    End Function

    Public Function Ping(ByVal ip As String) As Boolean
        Try
            Dim myReply As System.Net.NetworkInformation.PingReply
            Using myPing As New System.Net.NetworkInformation.Ping
                myReply = myPing.Send(ip, 2000)
            End Using

            Select Case myReply.Status
                Case System.Net.NetworkInformation.IPStatus.Success
                    '   Me.message.Add(Now.ToString + " ping " + ip + " " + "Success")
                    'Me.message.Add(Now.ToString + " ping " + ip + " " + myReply.Status.ToString)
                    Return True
                Case System.Net.NetworkInformation.IPStatus.TimedOut
                    'Console.WriteLine(Now.ToString + " ping " + ip + " " + myReply.Status.ToString)
                    'Me.message.Add(Now.ToString + " ping " + ip + " " + myReply.Status.ToString)
                    Return False
                Case System.Net.NetworkInformation.IPStatus.TimeExceeded
                    'Console.WriteLine(Now.ToString + " ping " + ip + " " + myReply.Status.ToString)
                    'Me.message.Add(Now.ToString + " ping " + ip + " " + myReply.Status.ToString)
                    Return False
                Case Else
                    'Me.message.Add(Now.ToString + " ping " + ip + " " + myReply.Status.ToString)
                    Return False

            End Select

        Catch ex As Exception
            '  System.ArgumentNullException: hostNameOrAddress 是 null 或空字串 (""
            'System.Net.Sockets.SocketException() : hostNameOrAddress()
            'Console.WriteLine(Now.ToString + " " + ip + " " + "IP設定值錯誤")
            ''Me.message.Add(Now.ToString + " " + ip + " " + "IP設定值錯誤")
            'Console.WriteLine(ex.ToString)
            Return False
        End Try

    End Function

    Public Shared Function GetDeviceAddress() As System.Net.IPAddress()
        Dim Hostname As String = Dns.GetHostName()
        Dim HostInfo As IPHostEntry
        ' HostInfo = Dns.GetHostByName(Hostname)
        HostInfo = Dns.GetHostEntry(Hostname)


        Return HostInfo.AddressList
    End Function



End Class


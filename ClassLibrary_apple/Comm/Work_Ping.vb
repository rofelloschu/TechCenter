'20130530
Imports System.Net
Namespace comm
    Public Class Work_Ping
        Inherits Work_base
        Public Event reportPing(ByVal IsEcho As Boolean)

        Sub New()
            MyBase.New()
        End Sub
        Overrides Sub close()

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
                        Me.message.Add(Now.ToString + " ping " + ip + " " + myReply.Status.ToString)
                        Return True
                    Case System.Net.NetworkInformation.IPStatus.TimedOut
                        ' Console.WriteLine(Now.ToString + " ping " + ip + " " + myReply.Status.ToString)
                        Me.message.Add(Now.ToString + " ping " + ip + " " + myReply.Status.ToString)
                        Return False
                    Case System.Net.NetworkInformation.IPStatus.TimeExceeded
                        '  Console.WriteLine(Now.ToString + " ping " + ip + " " + myReply.Status.ToString)
                        Me.message.Add(Now.ToString + " ping " + ip + " " + myReply.Status.ToString)
                        Return False
                    Case Else
                        Me.message.Add(Now.ToString + " ping " + ip + " " + myReply.Status.ToString)
                        Return False

                End Select

            Catch ex As Exception
                '  System.ArgumentNullException: hostNameOrAddress 是 null 或空字串 (""
                'System.Net.Sockets.SocketException() : hostNameOrAddress()
                Console.WriteLine(Now.ToString + " " + ip + " " + "IP設定值錯誤")
                Me.message.Add(Now.ToString + " " + ip + " " + "IP設定值錯誤")
                Console.WriteLine(ex.ToString)
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
End Namespace

Public MustInherit Class Work_base
    Implements IDisposable
    Protected message As List(Of String)
    Sub New()
        message = New List(Of String)
    End Sub
#Region "IDisposable Support"
    Private disposedValue As Boolean ' 偵測多餘的呼叫

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: 處置 Managed 狀態 (Managed 物件)。
            End If
            Me.message.Clear()
            Me.message = Nothing
            close()
            ' TODO: 釋放 Unmanaged 資源 (Unmanaged 物件) 並覆寫下面的 Finalize()。
            ' TODO: 將大型欄位設定為 null。
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: 只有當上面的 Dispose(ByVal disposing As Boolean) 有可釋放 Unmanaged 資源的程式碼時，才覆寫 Finalize()。
    'Protected Overrides Sub Finalize()
    '    ' 請勿變更此程式碼。在上面的 Dispose(ByVal disposing As Boolean) 中輸入清除程式碼。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' 由 Visual Basic 新增此程式碼以正確實作可處置的模式。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' 請勿變更此程式碼。在以上的 Dispose 置入清除程式碼 (ByVal 視為布林值處置)。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
    Public Function getMessageString() As String()
        Return Me.message.ToArray
    End Function
    'Dispose
    MustOverride Sub close()



End Class

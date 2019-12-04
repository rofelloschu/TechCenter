Imports System.Net.Sockets
Imports System.Text

'http://msdn.microsoft.com/zh-tw/library/system.net.sockets.networkstream.endread(v=vs.80).aspx
'我們需要定義一個能儲存非同步作業相關資訊的狀態物件
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Friend Class StateObject
    Public ReadOnly Client As TcpClient
    Public ReadOnly Buffer As Byte()
    Public ReadOnly Data As StringBuilder
    Public ReadOnly BufferSize As Integer = 1024

    Public Sub New(ByVal client As TcpClient)
        Me.Client = client
        Me.Data = New StringBuilder()
        Me.Buffer = New Byte(Me.BufferSize - 1) {}
    End Sub
End Class
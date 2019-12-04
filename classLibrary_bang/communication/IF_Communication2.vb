''20180816
'20190424
Public Interface IF_Communication2


    Sub StartProc()
    Sub StopProc()


    Property commState() As Boolean
    Event e_commState(status As Boolean)

    Event RecvData(ByVal Data() As Byte)
    Sub SendData(ByVal commData() As Byte)
    Sub close()
    Event err(text As String, ex As Exception)
End Interface
Public Class ex_use_Communication2
    Private Communication As IF_Communication2
    Sub New(t_comm As IF_Communication2)
        Communication = t_comm
        AddHandler Communication.RecvData, AddressOf AddressOf_RecvData
        Communication.StartProc()
    End Sub
    Sub close()
        RemoveHandler Communication.RecvData, AddressOf AddressOf_RecvData
        Communication.close()
    End Sub
    Private Sub AddressOf_RecvData(data As Byte())

    End Sub
    Sub send(data As Byte())
        Communication.SendData(data)
    End Sub
    Sub _Stop()
        Communication.StopProc()
    End Sub
    Public Function isConnect() As Boolean
        Return Communication.commState
    End Function
End Class

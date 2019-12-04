
Public Class think01
    Implements IF_think
    Protected rule As List(Of Object)
    Protected work As List(Of Object)
    Private auto_think As System.Threading.Thread
    Sub New()
        rule = New List(Of Object)
        work = New List(Of Object)
        rememberRule()
        auto_think = New System.Threading.Thread(AddressOf AddressOf_think)
        auto_think.Start()

    End Sub
    Private Sub rememberRule()
        '記憶規則

    End Sub
    Protected sleeptime As Integer = 1
    Protected Sub AddressOf_think()
        System.Threading.Thread.Sleep(sleeptime)

    End Sub
    Sub creareRule()

    End Sub
    Sub crearework()

    End Sub
#Region "if"
    Public Event command(comm As Object) Implements IF_think.command
    Public Sub recvdata(data As Object) Implements IF_think.recvdata

    End Sub
    Public Event remember(comm As Object) Implements IF_think.remember
    Public Event save(data As Object) Implements IF_think.save
#End Region

End Class

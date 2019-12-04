Imports System.Threading
Public Class code_i
    Implements IF_body

    'Public body_F As IF_body '接收 執行
    Public database_F As IF_memory '資料  資訊
    Public thing_F As IF_think '思考

    Sub New()
        'AddHandler Me.recv, AddressOf thing_F.recvdatay
        AddHandler thing_F.remember, AddressOf AddressOf_read
        AddHandler thing_F.save, AddressOf database_F.write
    End Sub

    Protected Sub AddressOf_command(comm As Object)
        Me.run(comm)
    End Sub

    Protected Sub AddressOf_read(comm As Object)
        thing_F.recvdata(database_F.read(comm))
    End Sub
#Region ""
    Public Event recv(data As Object) Implements IF_body.recv

    Public Sub run(data As Object) Implements IF_body.run

    End Sub
#End Region

End Class

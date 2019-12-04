Public Class vddata
    Public address As String
    Public time As String
    Public lanedata As List(Of lanedata)
    Sub New()
        lanedata = New List(Of lanedata)
    End Sub
End Class

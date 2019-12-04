Public Class ThreadFunction
    Delegate Function F() As Object
    Private x As F
    Sub New(ByVal tF As F)
        x = tF
    End Sub
    Sub start()

    End Sub
End Class

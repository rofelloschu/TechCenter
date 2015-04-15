Class OnlyOne
    Private Shared WeAreIsOne As New OnlyOne("John")
    Private myName As String
    Sub New()

    End Sub
    Sub New(ByRef a As OnlyOne)
        a = WeAreIsOne
    End Sub
    Sub New(ByVal name As String)
        setName(name)
    End Sub
    Shared Function getOnlyOne() As OnlyOne
        Return WeAreIsOne
    End Function

    Sub setName(ByVal name As String)
        Me.myName = name
    End Sub
    Function getName() As String
        Return WeAreIsOne.myName

    End Function
End Class
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Class csvWrite_undone
    Inherits APFile
    Sub New(ByVal name As String)
        MyBase.New(name)
    End Sub
    Sub wrtite(ByVal text As String())
        Dim newText As String = String.Empty
        For index As Integer = 0 To text.Length - 1
            If index = 0 Then
                newText = text(index)
            Else
                newText = newText + "," + text(index)
            End If
        Next
        MyBase.write(newText)
    End Sub
End Class
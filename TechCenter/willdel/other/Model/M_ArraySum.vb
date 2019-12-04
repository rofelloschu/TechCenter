<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Public Module M_ArraySum
    Function Sum(Of T)(ByVal array01 As T(), ByVal array02 As T()) As T()

        Dim Array00 As New List(Of T)
        For index As Integer = 0 To array01.Length - 1
            Array00.Add(array01(index))
        Next
        For index As Integer = 0 To array02.Length - 1
            Array00.Add(array02(index))
        Next
        Return Array00.ToArray
    End Function


End Module

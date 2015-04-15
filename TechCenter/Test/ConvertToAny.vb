Class ConvertToAny
    Sub New()
        Dim a1 As String = "1111"
        Console.WriteLine(Convert.ToByte(a1, 2).ToString)
        Console.WriteLine(Convert.ToInt16(a1, 10).ToString)
        Console.WriteLine(Convert.ToInt16(a1, 16).ToString)

        Dim x(0) As Byte
        x(0) = Convert.ToByte(a1, 2).ToString()
        Dim a2 As New BitArray(Convert.ToByte(a1, 2).ToString())
        Console.WriteLine("a2")
        Console.WriteLine("   Count:    {0}", a2.Count)
        Console.WriteLine("   Length:   {0}", a2.Length)
        Console.WriteLine("   Values:")
        PrintValues(a2, 8)
        Dim a3 As New BitArray(x)
        Console.WriteLine("a3")
        Console.WriteLine("   Count:    {0}", a3.Count)
        Console.WriteLine("   Length:   {0}", a3.Length)
        Console.WriteLine("   Values:")
        PrintValues(a3, 8)
    End Sub
    Public Shared Sub PrintValues(ByVal myList As IEnumerable, ByVal myWidth As Integer)
        Dim i As Integer = myWidth
        Dim obj As [Object]
        For Each obj In myList
            If i <= 0 Then
                i = myWidth
                Console.WriteLine()
            End If
            i -= 1
            Console.Write("{0,8}", obj)
        Next obj
        Console.WriteLine()
    End Sub 'PrintValues
End Class
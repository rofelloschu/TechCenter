Public Class ClassC
    Implements IF_A

    Public Sub AA() Implements IF_A.AA
        Console.WriteLine("AA")
    End Sub

    Public Sub BB() Implements IF_B.BB
        Console.WriteLine("BB")
    End Sub
  
End Class

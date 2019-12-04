Public Class TestAttribute
    <ConditionalAttribute("CONDITION1")> _
    Shared Sub Method1(x As Integer)
        Console.WriteLine("CONDITION1 is defined")
    End Sub

    <ConditionalAttribute("CONDITION1"), ConditionalAttribute("Condition2")> _
    Shared Sub Method2()
        Console.WriteLine("CONDITION1 or Condition2 is defined")
    End Sub
    ' This attribute will only be included if DEBUG is defined.

    '<DocumentationAttribute("dd")> _
    'Sub c()

    'End Sub
    Sub use_DEBUG()
#If DEBUG Then
        ' DEB = 1
#Else
       'DEB = 0
#End If
    End Sub
End Class

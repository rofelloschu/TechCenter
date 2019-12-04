Class avg
    Private total_avg As Integer
    Public total_count As Integer
    Sub New()
        total_avg = 0
        total_count = 0
    End Sub
    Sub add(ByVal avg_value As Integer, ByVal count As Integer)
        If avg_value = 0 And count = 0 Then
            Exit Sub
        End If
        total_avg = avg_value * count
        total_count = total_count + count
    End Sub
    Sub clear()
        total_avg = 0
        total_count = 0
    End Sub
    Function getAvg() As Integer
        Return total_avg \ total_count
    End Function
End Class
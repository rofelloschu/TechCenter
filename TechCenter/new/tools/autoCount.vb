Public Class autoCount
    Private m_count As Integer
    Private m_indexcount As Integer
    Sub New(setCount As Integer)
        Me.m_count = setCount
        Me.m_indexcount = Me.m_count
    End Sub
    Function getCount() As Integer
        Dim r_count As Integer = Me.m_indexcount
        Me.m_indexcount = Me.m_indexcount - 1
        If Me.m_indexcount < 0 Then
            Me.m_indexcount = 0
        End If

        Return r_count
    End Function

End Class

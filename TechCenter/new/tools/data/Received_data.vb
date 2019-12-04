
'20180321
Class Received_data
    Private m_databyte() As Byte
    Sub New()

    End Sub
    Overridable Sub add(data() As Byte)
        If m_databyte Is Nothing Then
            m_databyte = data
        Else
            Dim tempdata(m_databyte.Length + data.Length - 1) As Byte
            Array.Copy(m_databyte, tempdata, m_databyte.Length)
            Array.Copy(data, 0, tempdata, m_databyte.Length, data.Length)
            m_databyte = tempdata
        End If
    End Sub

    Overridable Sub removeData(remove_count As Integer)
        If remove_count > 0 Then
            Dim tempBuf(m_databyte.Length - remove_count - 1) As Byte
            Array.Copy(m_databyte, remove_count, tempBuf, 0, m_databyte.Length - remove_count)
            m_databyte = tempBuf


        End If
    End Sub
    Overridable Function getCountData(get_count As Integer) As Byte()
        If get_count > 0 Then
            Dim tempBuf(get_count - 1) As Byte
            Array.Copy(m_databyte, 0, tempBuf, 0, get_count)
            Return tempBuf

        Else
            Return Nothing
        End If
    End Function
    Overridable Function getData() As Byte()

        Return m_databyte
    End Function
End Class
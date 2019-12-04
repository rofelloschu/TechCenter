Public Class sunray_format_FFF9
    Public m_data As Byte()
    Public Timestamp(3) As Byte
    Public LaneID As Byte
    Public time As DateTime
    Sub New()

    End Sub
    Sub setByteData(data() As Byte)
        Me.m_data = data
        timestamp(0) = 0
        timestamp(1) = 0
        timestamp(2) = 0
        Timestamp(3) = 0

        Dim index As Integer = 3
        Array.Copy(Me.m_data, index, Timestamp, 0, 4)
        LaneID = Me.m_data(index + 4)



        Dim timestamp_int As Integer
        timestamp_int += CInt(Me.Timestamp(0))
        timestamp_int += (CInt(Me.Timestamp(1)) << 8)
        timestamp_int += (CInt(Me.Timestamp(2)) << 16)
        timestamp_int += (CInt(Me.Timestamp(3)) << 24)
        time = New DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local).AddSeconds(timestamp_int)


    End Sub
    Function to_string() As String
        Return time.ToString("u") + " lane " + LaneID.ToString
    End Function
    Function byteToString() As String
        Dim r_text As String = ""
        For index As Integer = 0 To m_data.Length - 1
            r_text = r_text + m_data(index).ToString("X2")
        Next
        Return r_text
    End Function
End Class

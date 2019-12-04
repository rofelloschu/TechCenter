Public Class sunray_format_FFF3


    ' 讀資料 比對
    'Dim a As New List(Of Byte)
    '    a.Add(&HFF)
    '    a.Add(&HF3)
    '    a.Add(&H3)
    '    a.Add(&H1)
    '    a.Add(&H0)
    '    a.Add(&H0)
    'Dim check As Byte = Me.Calcchecksum(a.ToArray, 3, 3)
    '    a.Add(check)
    'Dim a_text As String = ""
    '    For index As Integer = 0 To a.Count - 1
    '        a_text = a_text + "$" + a(index).ToString("X2")
    '    Next
    '    Console.WriteLine(a_text)
    Private m_data() As Byte
 
    Sub setIndex(index As Integer)
        Dim a As New List(Of Byte)
        a.Add(&HFF)
        a.Add(&HF3)
        a.Add(&H3)

        Convert.ToByte("0XFF", 16)
        Dim index_string As String = index.ToString("X6")
        Dim num01 As String = index_string.Substring(0, 2)

        Dim num02 As String = index_string.Substring(2, 2)

        Dim num03 As String = index_string.Substring(4, 2)
        a.Add(Convert.ToByte("0x" + num01, 16))
        a.Add(Convert.ToByte("0x" + num02, 16))
        a.Add(Convert.ToByte("0x" + num03, 16))
        Dim check As Byte = Me.Calcchecksum(a.ToArray, 3, 3)
        a.Add(check)

        m_data = a.ToArray
        'Console.WriteLine(index.ToString)
        'Console.WriteLine(index.ToString("X6"))
        'Console.WriteLine(a(3).ToString("X2") + a(4).ToString("X2") + a(5).ToString("X2"))
    End Sub

    Private Function Calcchecksum(ByRef procBuf As Byte(), ByVal shift As Integer, ByVal length As Integer) As Byte
        Dim checksum As Integer = 0
        Dim i As Integer = 0
        While (i < length)
            checksum = checksum + procBuf(shift + i)
            i += 1
        End While
        checksum = checksum And &HFF

        Return CByte(checksum)
    End Function
    Function getdata() As Byte()
        Return m_data
    End Function
End Class

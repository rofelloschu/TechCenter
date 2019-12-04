Public Class IcmpPacket
    Private _type As [Byte]
    ' 类型
    Private _subCode As [Byte]
    ' 代码
    Private _checkSum As UInt16
    ' 校验和
    Private _identifier As UInt16
    ' 识别符
    Private _sequenceNumber As UInt16
    ' 序列号 
    Private _data As [Byte]()
    '选项数据
    Public Sub New(ByVal type As [Byte], ByVal subCode As [Byte], ByVal checkSum As UInt16, ByVal identifier As UInt16, ByVal sequenceNumber As UInt16, ByVal dataSize As Integer)
        _type = type
        _subCode = subCode
        _checkSum = checkSum
        _identifier = identifier
        _sequenceNumber = sequenceNumber
        _data = New [Byte](dataSize - 1) {}
        '在数据中，写入指定的数据大小
        For i As Integer = 0 To dataSize - 1
            '由于选项数据在此命令中并不重要，所以你可以改换任何你喜欢的字符 
            _data(i) = CByte(AscW("#"c))
        Next
    End Sub
    Public Property CheckSum() As UInt16
        Get
            Return _checkSum
        End Get
        Set(ByVal value As UInt16)
            _checkSum = value
        End Set
    End Property
    '初始化ICMP报文
    Public Function CountByte(ByVal buffer As [Byte]()) As Integer
        Dim b_type As [Byte]() = New [Byte](0) {_type}
        Dim b_code As [Byte]() = New [Byte](0) {_subCode}
        Dim b_cksum As [Byte]() = BitConverter.GetBytes(_checkSum)
        Dim b_id As [Byte]() = BitConverter.GetBytes(_identifier)
        Dim b_seq As [Byte]() = BitConverter.GetBytes(_sequenceNumber)
        Dim i As Integer = 0
        Array.Copy(b_type, 0, buffer, i, b_type.Length)
        i += b_type.Length
        Array.Copy(b_code, 0, buffer, i, b_code.Length)
        i += b_code.Length
        Array.Copy(b_cksum, 0, buffer, i, b_cksum.Length)
        i += b_cksum.Length
        Array.Copy(b_id, 0, buffer, i, b_id.Length)
        i += b_id.Length
        Array.Copy(b_seq, 0, buffer, i, b_seq.Length)
        i += b_seq.Length
        Array.Copy(_data, 0, buffer, i, _data.Length)
        i += _data.Length
        Return i
    End Function
    '将整个ICMP报文信息和数据转化为Byte数据包 
    Public Shared Function SumOfCheck(ByVal buffer As UInt16()) As UInt16
        Dim cksum As Integer = 0
        For i As Integer = 0 To buffer.Length - 1
            cksum += CInt(buffer(i))
        Next
        cksum = (cksum >> 16) + (cksum And &HFFFF)
        cksum += (cksum >> 16)
        Dim chcksmx As UInt16 = (Not cksum) And UInt16.MaxValue
        Return DirectCast(chcksmx, UInt16)

        'Return DirectCast(Not cksum, UInt16)
    End Function

End Class

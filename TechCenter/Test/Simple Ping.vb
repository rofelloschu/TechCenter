Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class Simple_Ping
    Public Type As Byte
    Public Code As Byte
    Public Checksum As UInt16
    Public MessageSize As Integer
    Public Message As Byte() = New Byte(1023) {}
    Public Sub New()
    End Sub
    Public Sub New(ByVal data As Byte(), ByVal size As Integer)
        Type = data(20)
        Code = data(21)
        Checksum = BitConverter.ToUInt16(data, 22)
        MessageSize = size - 24
        Buffer.BlockCopy(data, 24, Message, 0, MessageSize)
    End Sub

    Public Function getBytes() As Byte()
        Dim data As Byte() = New Byte(MessageSize + 8) {}
        Buffer.BlockCopy(BitConverter.GetBytes(Type), 0, data, 0, 1)
        Buffer.BlockCopy(BitConverter.GetBytes(Code), 0, data, 1, 1)
        Buffer.BlockCopy(BitConverter.GetBytes(Checksum), 0, data, 2, 2)
        Buffer.BlockCopy(Message, 0, data, 4, MessageSize)
        Return data
    End Function
    Public Function getChecksum() As UInt16
        Dim chcksm As UInt32 = 0
        Dim data As Byte() = getBytes()
        Dim packetsize As Integer = MessageSize + 8
        Dim index As Integer = 0

        While index < packetsize
            chcksm += Convert.ToUInt32(BitConverter.ToUInt16(data, index))
            index += 2
        End While
        chcksm = (chcksm >> 16) + (chcksm And &HFFFF)
        chcksm += (chcksm >> 16)
        Dim chcksmx As UInt16 = (Not chcksm) And UInt16.MaxValue
        Return DirectCast(chcksmx, UInt16)
    End Function

End Class

'20180502
'閱20180315
'20190530
Public Module M_Math
#Region "亂數"
    '使用NewGuid作亂數種子
    Public Function getRandomSeed() As Integer
        Return Guid.NewGuid().GetHashCode()
    End Function
    Public Function getRandomObj() As Random
        Return New Random(Guid.NewGuid().GetHashCode())
    End Function
 
#End Region

#Region "數字文字轉換"
    Private Sub test_01()
        MsgBox(Convert.ToString(Convert.ToInt32("1111", 2))) '//2進制轉10進制
        MsgBox(Convert.ToString(Convert.ToInt32("11", 8))) '//8進制轉10進制
        MsgBox(Convert.ToString(Convert.ToInt32("0XFF", 16))) '//16進制轉10進制
    End Sub

    Public Function ToHexString(ByVal value As Integer) As String
        Return value.ToString("X2")
    End Function
    Public Function ToHexString(ByVal value As Integer, ByVal len As Integer) As String
        Return value.ToString("X" + len.ToString)

    End Function
    
    Public Function ToHexBytes(ByVal value As Integer) As Byte()
        Dim HexString As String = value.ToString("X")
        Dim index As Integer = 0

        If HexString.Length Mod 2 = 0 Then
            index = HexString.Length \ 2
        Else
            HexString = "0" + HexString
            index = HexString.Length \ 2
        End If

        Dim return_bytes(index - 1) As Byte
        For index = 0 To return_bytes.Length - 1
            return_bytes(index) = Convert.ToInt32("0X" + HexString.Substring(index * 2, 2), 16)
        Next
        Return return_bytes
    End Function
    Public Function ToHexBytes(ByVal value As Integer, ByVal len As Integer) As Byte()
        Dim HexString As String = value.ToString("X" + (len * 2).ToString)
        If len > 0 Then
            HexString = value.ToString("X" + (len * 2).ToString)
        Else
            HexString = value.ToString("X")
        End If
        Dim index As Integer = 0

        If HexString.Length Mod 2 = 0 Then
            index = HexString.Length \ 2
        Else
            HexString = "0" + HexString
            index = HexString.Length \ 2
        End If

        Dim return_bytes(index - 1) As Byte
        For index = 0 To return_bytes.Length - 1
            return_bytes(index) = Convert.ToInt32("0X" + HexString.Substring(index * 2, 2), 16)
        Next
        Return return_bytes
    End Function
 
    Function chrToHexString(text As String) As String
        Dim bytesString As String() = text.Split(" ")
        Dim stringbyteslist As New List(Of Byte)
        For index As Integer = 0 To bytesString.Length - 1
            Try
                stringbyteslist.Add(Convert.ToInt32("0X" + bytesString(index), 16))
            Catch ex As Exception
                Console.WriteLine("err " + bytesString(index))
                stringbyteslist.Add(0)
            End Try

        Next
        Return System.Text.Encoding.ASCII.GetString(stringbyteslist.ToArray)
    End Function
    <ObsoleteAttribute("感覺有問題", False)> _
    Function HexStringToChr(text As String)
        Dim stringbytes As Byte() = System.Text.Encoding.ASCII.GetBytes(text)
        Dim bytesString As String = ""
        For index As Integer = 0 To stringbytes.Length - 1
            If index = 0 Then
                bytesString = stringbytes(index).ToString("x2")
            Else
                bytesString = bytesString + " " + stringbytes(index).ToString("x2")
            End If

        Next
        Return bytesString
    End Function

    Function HexStringToBytes(text As String) As Byte()
        Dim len As Integer = text.Length / 2
        Dim Hex_Byte(len - 1) As Byte
        For index As Integer = 0 To len - 1
            Hex_Byte(index) = HEXString_to_DEC(text.Substring(index * 2, 2))
        Next
        Return Hex_Byte
    End Function

    '16轉10
    Public Function HEXString_to_DEC(ByVal Hex As String) As Long
        Dim i As Long
        Dim B As Long

        Hex = UCase(Hex)
        For i = 1 To Len(Hex)
            Select Case Mid(Hex, Len(Hex) - i + 1, 1)
                Case "0" : B = B + 16 ^ (i - 1) * 0
                Case "1" : B = B + 16 ^ (i - 1) * 1
                Case "2" : B = B + 16 ^ (i - 1) * 2
                Case "3" : B = B + 16 ^ (i - 1) * 3
                Case "4" : B = B + 16 ^ (i - 1) * 4
                Case "5" : B = B + 16 ^ (i - 1) * 5
                Case "6" : B = B + 16 ^ (i - 1) * 6
                Case "7" : B = B + 16 ^ (i - 1) * 7
                Case "8" : B = B + 16 ^ (i - 1) * 8
                Case "9" : B = B + 16 ^ (i - 1) * 9
                Case "A" : B = B + 16 ^ (i - 1) * 10
                Case "B" : B = B + 16 ^ (i - 1) * 11
                Case "C" : B = B + 16 ^ (i - 1) * 12
                Case "D" : B = B + 16 ^ (i - 1) * 13
                Case "E" : B = B + 16 ^ (i - 1) * 14
                Case "F" : B = B + 16 ^ (i - 1) * 15
            End Select
        Next i
        HEXString_to_DEC = B
    End Function
#End Region
#Region "數字文字轉換 test"
    ' 10進制轉成2、8、16進制
    '1
    'j=Convert.ToString(10, 2)        '10進制轉2進制     j="1010"
    '2
    'j=Convert.ToString(11, 8)        '10進制轉8進制     j="13"
    '3
    'j=Convert.ToString(254, 16)      '10進制轉16進制    j="FE"

    '2、8、16進制轉10進制
    'view my source(檢視程式碼)print source(列印程式碼)版權說明
    '1
    'i=Convert.ToInt32("1010", 2)     '2進制轉10進制  i=10
    '2
    'i=Convert.ToInt32("13", 8)       '8進制轉10進制  i=11
    '3
    'i=Convert.ToInt32("0XFE", 16)    '16進制轉10進制 i=254




    'test
    '10轉2
    <ObsoleteAttribute("未測試", False)> _
    Public Function DEC_to_BINString(ByVal Dec As Long) As String
        DEC_to_BINString = ""
        Do While Dec > 0
            DEC_to_BINString = Dec Mod 2 & DEC_to_BINString
            Dec = Dec \ 2
        Loop
    End Function
    '2轉10
    <ObsoleteAttribute("未測試", False)> _
    Public Function BINString_to_DEC(ByVal Bin As String) As Long
        Dim i As Long
        For i = 1 To Len(Bin)
            BINString_to_DEC = BINString_to_DEC * 2 + Val(Mid(Bin, i, 1))
        Next i
    End Function
    '16轉2
    <ObsoleteAttribute("未測試", False)> _
    Public Function HEX_to_BIN(ByVal Hex As String) As String
        Dim i As Long
        Dim B As String = ""

        Hex = UCase(Hex)
        For i = 1 To Len(Hex)
            Select Case Mid(Hex, i, 1)
                Case "0" : B = B & "0000"
                Case "1" : B = B & "0001"
                Case "2" : B = B & "0010"
                Case "3" : B = B & "0011"
                Case "4" : B = B & "0100"
                Case "5" : B = B & "0101"
                Case "6" : B = B & "0110"
                Case "7" : B = B & "0111"
                Case "8" : B = B & "1000"
                Case "9" : B = B & "1001"
                Case "A" : B = B & "1010"
                Case "B" : B = B & "1011"
                Case "C" : B = B & "1100"
                Case "D" : B = B & "1101"
                Case "E" : B = B & "1110"
                Case "F" : B = B & "1111"
            End Select
        Next i
        While Left(B, 1) = "0"
            B = Right(B, Len(B) - 1)
        End While
        HEX_to_BIN = B

    End Function
    '2轉16
    'Public Function BIN_to_HEX(ByVal Bin As String) As String
    '    Dim i As Long
    '    Dim H As String
    '    If Len(Bin) Mod 4 <> 0 Then
    '        Bin =   String(4 - Len(Bin) Mod 4, "0") & Bin
    '    End If

    '    For i = 1 To Len(Bin) Step 4
    '        Select Case Mid(Bin, i, 4)
    '            Case "0000" : H = H & "0"
    '            Case "0001" : H = H & "1"
    '            Case "0010" : H = H & "2"
    '            Case "0011" : H = H & "3"
    '            Case "0100" : H = H & "4"
    '            Case "0101" : H = H & "5"
    '            Case "0110" : H = H & "6"
    '            Case "0111" : H = H & "7"
    '            Case "1000" : H = H & "8"
    '            Case "1001" : H = H & "9"
    '            Case "1010" : H = H & "A"
    '            Case "1011" : H = H & "B"
    '            Case "1100" : H = H & "C"
    '            Case "1101" : H = H & "D"
    '            Case "1110" : H = H & "E"
    '            Case "1111" : H = H & "F"
    '        End Select
    '    Next i
    '    While Left(H, 1) = "0"
    '        H = Right(H, Len(H) - 1)
    '    End While
    '    BIN_to_HEX = H
    'End Function



    
    '10轉16
    <ObsoleteAttribute("未測試", False)> _
    Public Function DEC_to_HEXString(ByVal Dec As Long) As String
        Dim a As String
        DEC_to_HEXString = ""
        Do While Dec > 0
            a = CStr(Dec Mod 16)
            Select Case a
                Case "10" : a = "A"
                Case "11" : a = "B"
                Case "12" : a = "C"
                Case "13" : a = "D"
                Case "14" : a = "E"
                Case "15" : a = "F"
            End Select
            DEC_to_HEXString = a & DEC_to_HEXString
            Dec = Dec \ 16
        Loop
    End Function
    '10轉8
    <ObsoleteAttribute("未測試", False)> _
    Public Function DEC_to_OCT(ByVal Dec As Long) As String
        DEC_to_OCT = ""
        Do While Dec > 0
            DEC_to_OCT = Dec Mod 8 & DEC_to_OCT
            Dec = Dec \ 8
        Loop
    End Function
    '8轉10
    <ObsoleteAttribute("未測試", False)> _
    Public Function OCT_to_DEC(ByVal Oct As String) As Long
        Dim i As Long
        Dim B As Long

        For i = 1 To Len(Oct)
            Select Case Mid(Oct, Len(Oct) - i + 1, 1)
                Case "0" : B = B + 8 ^ (i - 1) * 0
                Case "1" : B = B + 8 ^ (i - 1) * 1
                Case "2" : B = B + 8 ^ (i - 1) * 2
                Case "3" : B = B + 8 ^ (i - 1) * 3
                Case "4" : B = B + 8 ^ (i - 1) * 4
                Case "5" : B = B + 8 ^ (i - 1) * 5
                Case "6" : B = B + 8 ^ (i - 1) * 6
                Case "7" : B = B + 8 ^ (i - 1) * 7
            End Select
        Next i
        OCT_to_DEC = B
    End Function
    '2轉8
    'Public Function BIN_to_OCT(ByVal Bin As String) As String
    '    Dim i As Long
    '    Dim H As String
    '    If Len(Bin) Mod 3 <> 0 Then
    '    Bin = String(3 - Len(Bin) Mod 3, "0") & Bin
    '    End If

    '    For i = 1 To Len(Bin) Step 3
    '        Select Case Mid(Bin, i, 3)
    '            Case "000" : H = H & "0"
    '            Case "001" : H = H & "1"
    '            Case "010" : H = H & "2"
    '            Case "011" : H = H & "3"
    '            Case "100" : H = H & "4"
    '            Case "101" : H = H & "5"
    '            Case "110" : H = H & "6"
    '            Case "111" : H = H & "7"
    '        End Select
    '    Next i
    '    While Left(H, 1) = "0"
    '        H = Right(H, Len(H) - 1)
    '    End While
    '    BIN_to_OCT = H
    'End Function
    '8轉2
    <ObsoleteAttribute("未測試", False)> _
    Public Function OCT_to_BIN(ByVal Oct As String) As String
        Dim i As Long
        Dim B As String = ""

        For i = 1 To Len(Oct)
            Select Case Mid(Oct, i, 1)
                Case "0" : B = B & "000"
                Case "1" : B = B & "001"
                Case "2" : B = B & "010"
                Case "3" : B = B & "011"
                Case "4" : B = B & "100"
                Case "5" : B = B & "101"
                Case "6" : B = B & "110"
                Case "7" : B = B & "111"
            End Select
        Next i
        While Left(B, 1) = "0"
            B = Right(B, Len(B) - 1)
        End While
        OCT_to_BIN = B
    End Function

    '8轉16
    'Public Function OCT_to_HEX(ByVal Oct As String) As String
    '    Dim Bin As String
    '    Bin = OCT_to_BIN(Oct)
    '    OCT_to_HEX = BIN_to_HEX(Bin)
    'End Function

    '16轉8
    'Public Function HEX_to_OCT(ByVal Hex As String) As String
    '    Dim Bin As String
    '    Hex = UCase(Hex)
    '    Bin = HEX_to_BIN(Hex)
    '    HEX_to_OCT = BIN_to_OCT(Bin)
    'End Function
#End Region

    '\u uUTF 轉文字 未測
    Function UTF_c_string_toString(UTF_c_string As String) As String
        Dim text01 As String() = UTF_c_string.Split("\u")
        Dim text02 As String = ""
        For index As Integer = 0 To text01.Length - 1
            If text01(index).Length = 4 Then
                text02 = text02 + Convert.ToInt32("0X" + text01(index), 16)
            End If
            If text01(index).Length > 4 Then
                Dim chr1 As String = Convert.ToInt32("0X" + text01(index).Substring(0, 4), 16)
                Dim chr2 As String = text01(index).Substring(4, text01(index).Length - 4)
                text02 = text02 + chr1 + chr2

            End If
        Next

    End Function
    Class HexToD
        Public Shared Function IntToHexString(ByVal value As Integer) As String

            Return "0x" & String.Format("{0:X}", value)

        End Function
        Public Shared Function HexStringToInt(ByVal value As String) As Integer
            If value.ToUpper().StartsWith("0X") Then
                value = value.Substring(2)
            End If
            Return Int32.Parse(value, System.Globalization.NumberStyles.HexNumber)
        End Function

    End Class
    '轉boolean
    Function getByteToboolean(data As Byte, index As Integer) As Boolean
        Dim bit_text As String = Convert.ToString(data, 2)
        If bit_text.Substring(index, 1) = "1" Then
            Return True
        Else
            Return False
        End If
    End Function
End Module

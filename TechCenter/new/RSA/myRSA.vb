'Imports System
'Imports System.Collections.Generic
'Imports System.Linq
Imports System.Text
Imports System.Security.Cryptography
Imports System.IO
Imports System.Xml
Imports System.IO.IsolatedStorage

'20200211
'1創造A RSA KEy
'2加密
Public Class myRSA
    

    Private Shared Sub writeToFile(filename As String, ByVal data As String)
        'Dim isoStore As IsolatedStorageFile = _SunSkyActivator._isoStore
        Dim writer As StreamWriter = Nothing
        writer = New StreamWriter(filename, True, Encoding.UTF8)
        writer.Write(data)
        writer.Close()
    End Sub

    Private Shared Function readFromFile(filename As String ) As String
        Dim data As String = ""
        'Dim isoStore As IsolatedStorageFile = _SunSkyActivator._isoStore
        'Dim fileNames As String() = isoStore.GetFileNames(_SunSkyActivator.fileName)
        Dim reader As StreamReader = New StreamReader(filename, Encoding.UTF8)
        data = reader.ReadToEnd()
        reader.Close()
        'For Each file As String In fileNames

        '    If file = _SunSkyActivator.fileName Then
        '        Dim reader As StreamReader = New StreamReader(New IsolatedStorageFileStream(file, FileMode.Open, isoStore), Encoding.UTF8)
        '        data = reader.ReadToEnd()
        '        reader.Close()
        '    End If
        'Next

        Return data
    End Function

    'Private Function _GetRSA(ByVal IsPrivateKey As Boolean) As String
    '    If Not IsPrivateKey Then
    '        Return PublicKey
    '    Else
    '        Return PrivateKey
    '    End If
    'End Function
    '加密文字上限117
    Shared Function _RSAEncrypt(ByVal strOriginal As String, pubKey As String, Optional buff_len As Integer = 64) As String

        Dim m_buff_len As Integer = buff_len
        If m_buff_len > 117 Then
            m_buff_len = 117
        End If
        Try
            Dim objByteConverter As UTF8Encoding = New UTF8Encoding()
            Dim objEncryptRSA As RSACryptoServiceProvider = New RSACryptoServiceProvider()
            objEncryptRSA.FromXmlString(pubKey)
            Dim bitOriginalSegment As Byte()
            Dim bitEncryptedSegment As Byte()
            Dim strEncryptedData As StringBuilder = New StringBuilder()
            'strEncryptedData.AppendLine("<EncryptData>")
            Dim bitEncryptedSegment_list As New List(Of Byte)
            Dim data_count As Integer = strOriginal.Length \ m_buff_len
            Dim data_last_len As Integer = strOriginal.Length Mod m_buff_len
            If data_last_len <> 0 Then
                data_count = data_count + 1
            End If

            For i As Integer = 0 To data_count - 1
                Dim intStringLength As Integer

                'If (strOriginal.Length - i * m_buff_len) < m_buff_len Then
                '    intStringLength = strOriginal.Length - i * m_buff_len
                'Else
                '    intStringLength = m_buff_len
                'End If
                If i = data_count - 1 Then
                    If data_last_len <> 0 Then
                        intStringLength = data_last_len
                    Else
                        intStringLength = m_buff_len
                    End If
                Else
                    intStringLength = m_buff_len
                End If



                bitOriginalSegment = objByteConverter.GetBytes(strOriginal.Substring(i * m_buff_len, intStringLength))
                bitEncryptedSegment = myRSA._RSAEncrypt(bitOriginalSegment, objEncryptRSA.ExportParameters(False), True)
                'strEncryptedData.AppendLine(String.Format("<p{0}>{1}</p{0}>", i, Convert.ToBase64String(bitEncryptedSegment)))
                'strEncryptedData.AppendLine(String.Format("{0}", Convert.ToBase64String(bitEncryptedSegment)))
                bitEncryptedSegment_list.AddRange(bitEncryptedSegment)
            Next

            'strEncryptedData.AppendLine(String.Format("{0}", Convert.ToBase64String(bitEncryptedSegment_list.ToArray)))
            strEncryptedData.Append(String.Format("{0}", Convert.ToBase64String(bitEncryptedSegment_list.ToArray)))
            bitEncryptedSegment_list.Clear()
            'strEncryptedData.AppendLine("</EncryptData>")
            objEncryptRSA.Clear()
            objEncryptRSA = Nothing
            Return strEncryptedData.ToString()
        Catch ex As Exception
            Return ""
        End Try
    End Function
    'RSA 預設長度128
    Shared Function _RSADecrypt(ByVal strEncrypted As String, priKey As String, Optional buff_len As Integer = 128) As String
        Dim m_buff_len As Integer = buff_len
        'If m_buff_len > 117 Then
        '    m_buff_len = 117
        'End If
        Try

            If strEncrypted.Length = 0 Then
                Return ""
            End If

            Dim objByteConverter As UTF8Encoding = New UTF8Encoding()
            Dim objDecryptRSA As RSACryptoServiceProvider = New RSACryptoServiceProvider()
            objDecryptRSA.FromXmlString(priKey)
            Dim strDecryptedData As StringBuilder = New StringBuilder()
            'Dim objXml As XmlDocument = New XmlDocument()
            'objXml.LoadXml(strEncrypted)

            'If objXml.DocumentElement.ChildNodes.Count > 0 Then

            '    For i As Integer = 0 To objXml.DocumentElement.ChildNodes.Count - 1
            '        Dim bitEncryptedSegment As Byte()
            '        Dim bitDecryptedSegment As Byte()
            '        bitEncryptedSegment = Convert.FromBase64String(objXml.DocumentElement.ChildNodes(i).InnerText)

            '        If bitEncryptedSegment.Length > 0 Then
            '            bitDecryptedSegment = myRSA._RSADecrypt(bitEncryptedSegment, objDecryptRSA.ExportParameters(True), True)
            '            strDecryptedData.Append(objByteConverter.GetString(bitDecryptedSegment))
            '        End If
            '    Next
            'Else
            '    Return ""
            'End If
            Dim allbitEncryptedSegment As Byte() = Convert.FromBase64String(strEncrypted)

            Dim data_count As Integer = allbitEncryptedSegment.Length \ m_buff_len
            Dim data_last_len As Integer = allbitEncryptedSegment.Length Mod m_buff_len
            If data_last_len <> 0 Then
                data_count = data_count + 1
            End If

            For index As Integer = 0 To data_count - 1
                Dim bitEncryptedSegment(m_buff_len - 1) As Byte
                Dim bitDecryptedSegment As Byte()


                If index = data_count - 1 Then
                    If data_last_len <> 0 Then
                        Array.Copy(allbitEncryptedSegment, index * m_buff_len, bitEncryptedSegment, 0, data_last_len)
                    Else
                        Array.Copy(allbitEncryptedSegment, index * m_buff_len, bitEncryptedSegment, 0, m_buff_len)
                    End If
                Else
                    Array.Copy(allbitEncryptedSegment, index * m_buff_len, bitEncryptedSegment, 0, m_buff_len)
                End If


                If bitEncryptedSegment.Length > 0 Then
                    bitDecryptedSegment = myRSA._RSADecrypt(bitEncryptedSegment, objDecryptRSA.ExportParameters(True), True)
                    strDecryptedData.Append(objByteConverter.GetString(bitDecryptedSegment))
                End If
            Next


            Return strDecryptedData.ToString()
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Shared Function _RSAEncrypt_xml(ByVal strOriginal As String, pubKey As String) As String
        Try
            Dim objByteConverter As UTF8Encoding = New UTF8Encoding()
            Dim objEncryptRSA As RSACryptoServiceProvider = New RSACryptoServiceProvider()
            objEncryptRSA.FromXmlString(pubKey)
            Dim bitOriginalSegment As Byte()
            Dim bitEncryptedSegment As Byte()
            Dim strEncryptedData As StringBuilder = New StringBuilder()
            strEncryptedData.AppendLine("<EncryptData>")

            For i As Integer = 0 To (strOriginal.Length / 39) + 1 - 1
                'fix 修正39倍數時會多1次
                If (strOriginal.Length Mod 39) = 0 Then
                    Exit For
                End If

                Dim intStringLength As Integer

                If (strOriginal.Length - i * 39) < 39 Then
                    intStringLength = strOriginal.Length - i * 39
                Else
                    intStringLength = 39
                End If

                bitOriginalSegment = objByteConverter.GetBytes(strOriginal.Substring(i * 39, intStringLength))
                bitEncryptedSegment = myRSA._RSAEncrypt(bitOriginalSegment, objEncryptRSA.ExportParameters(False), True)
                strEncryptedData.AppendLine(String.Format("<p{0}>{1}</p{0}>", i, Convert.ToBase64String(bitEncryptedSegment)))
            Next

            strEncryptedData.AppendLine("</EncryptData>")
            objEncryptRSA.Clear()
            objEncryptRSA = Nothing
            Return strEncryptedData.ToString()
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Shared Function _RSADecrypt_xml(ByVal strEncrypted As String, priKey As String) As String
        Try

            If strEncrypted.Length = 0 Then
                Return ""
            End If

            Dim objByteConverter As UTF8Encoding = New UTF8Encoding()
            Dim objDecryptRSA As RSACryptoServiceProvider = New RSACryptoServiceProvider()
            objDecryptRSA.FromXmlString(priKey)
            Dim strDecryptedData As StringBuilder = New StringBuilder()
            Dim objXml As XmlDocument = New XmlDocument()
            objXml.LoadXml(strEncrypted)

            If objXml.DocumentElement.ChildNodes.Count > 0 Then

                For i As Integer = 0 To objXml.DocumentElement.ChildNodes.Count - 1
                    Dim bitEncryptedSegment As Byte()
                    Dim bitDecryptedSegment As Byte()
                    bitEncryptedSegment = Convert.FromBase64String(objXml.DocumentElement.ChildNodes(i).InnerText)

                    If bitEncryptedSegment.Length > 0 Then
                        bitDecryptedSegment = myRSA._RSADecrypt(bitEncryptedSegment, objDecryptRSA.ExportParameters(True), True)
                        strDecryptedData.Append(objByteConverter.GetString(bitDecryptedSegment))
                    End If
                Next
            Else
                Return ""
            End If

            Return strDecryptedData.ToString()
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Shared Function _RSAEncrypt(ByVal DataToEncrypt As Byte(), ByVal RSAKeyInfo As RSAParameters, ByVal DoOAEPPadding As Boolean) As Byte()
        Dim RSA As RSACryptoServiceProvider = New RSACryptoServiceProvider()
        RSA.ImportParameters(RSAKeyInfo)
        Return RSA.Encrypt(DataToEncrypt, DoOAEPPadding)
    End Function

    Shared Function _RSADecrypt(ByVal DataToDecrypt As Byte(), ByVal RSAKeyInfo As RSAParameters, ByVal DoOAEPPadding As Boolean) As Byte()
        Dim RSA As RSACryptoServiceProvider = New RSACryptoServiceProvider()
        RSA.ImportParameters(RSAKeyInfo)
        Return RSA.Decrypt(DataToDecrypt, DoOAEPPadding)
    End Function

    Shared Function createRSA_CSP(name As String) As RSACryptoServiceProvider

        Dim _cspParameters_A As New CspParameters()
        _cspParameters_A.KeyContainerName = name
        Dim RSA_A As RSACryptoServiceProvider = New RSACryptoServiceProvider(_cspParameters_A)

        Return RSA_A

    End Function
    Public Function GetSignature(ByVal OriginalString As String, PrivateKey As String) As String
        If String.IsNullOrEmpty(OriginalString) Then
            Throw New ArgumentNullException()
        End If
        Dim _rsaCrypto As RSACryptoServiceProvider = New RSACryptoServiceProvider
        _rsaCrypto.FromXmlString(PrivateKey)
        Dim Encode As System.Text.Encoding = Encoding.Unicode
        Dim originalData = Encode.GetBytes(OriginalString)
        Dim singData = _rsaCrypto.SignData(originalData, New SHA1CryptoServiceProvider())
        Dim SignatureString = Convert.ToBase64String(singData)
        Return SignatureString
    End Function
    Public Function VerifySignature(ByVal OriginalString As String, ByVal SignatureString As String, PublicKey As String) As Boolean
        If String.IsNullOrEmpty(OriginalString) Then
            Throw New ArgumentNullException()
        End If
        Dim _rsaCrypto As RSACryptoServiceProvider = New RSACryptoServiceProvider
        _rsaCrypto.FromXmlString(PublicKey)
        Dim Encode As System.Text.Encoding = Encoding.Unicode
        Dim originalData = Encode.GetBytes(OriginalString)
        Dim signatureData = Convert.FromBase64String(SignatureString)
        Dim isVerify = _rsaCrypto.VerifyData(originalData, New SHA1CryptoServiceProvider(), signatureData)
        Return isVerify
    End Function
    Shared Sub test()
        Dim a_RSA_CSP As RSACryptoServiceProvider = myRSA.createRSA_CSP("CC")

        Dim b_RSA_CSP As RSACryptoServiceProvider = myRSA.createRSA_CSP("Bb")
        Dim data As String = "123456789"

        'Dim data01 As String = myRSA._RSAEncrypt(data, a_RSA_CSP.ToXmlString(False))
        'Dim data02 As String = myRSA._RSADecrypt(data01, a_RSA_CSP.ToXmlString(True))


        '公key只能加密 私key加密解密
        'Dim data01 As String = myRSA._RSAEncrypt(data, a_RSA_CSP.ToXmlString(True))
        'Console.WriteLine(data01)
        'Dim data02 As String = myRSA._RSADecrypt(data01, a_RSA_CSP.ToXmlString(True))
        'Console.WriteLine(data02)


        'Dim data01 As String = myRSA._RSAEncrypt(data, a_RSA_CSP.ToXmlString(False))
        'Console.WriteLine(data01)
        'Dim data02 As String = myRSA._RSAEncrypt(data01, b_RSA_CSP.ToXmlString(False))
        'Console.WriteLine(data02)
        'Dim data03 As String = myRSA._RSADecrypt(data02, b_RSA_CSP.ToXmlString(True))
        'Console.WriteLine(data03)
        'Dim data04 As String = myRSA._RSADecrypt(data03, a_RSA_CSP.ToXmlString(True))
        'Console.WriteLine(data04)





    End Sub
#Region "2"
    '    /// <summary>
    '/// UTF-8編碼加密函式，加密結果以base-64編碼輸出
    '/// 超過117位元組的內容(相當於39個UTF-8編碼的中文字)將自動分段處理
    '/// 但是合併輸出。
    '/// </summary>
    '/// <param name="strOriginal">原始資料(string)</param>
    '/// <returns>加密後的資料(string)</returns>
    Shared Function _RSAEncrypt_2(ByVal strOriginal As String, getRSA As String) As String
        Try
            Dim objByteConverter As UTF8Encoding = New UTF8Encoding()
            Dim objEncryptRSA As RSACryptoServiceProvider = New RSACryptoServiceProvider()
            objEncryptRSA.FromXmlString(getRSA)
            '//暫存轉碼過的位元組
            Dim bitOriginalSegment As Byte()
            '//暫存加密過的位元組
            Dim bitEncryptedSegment As Byte()
            '//暫存加密結果
            Dim strEncryptedData As StringBuilder = New StringBuilder()
            strEncryptedData.AppendLine("<EncryptData>")
            '           //RSA演算僅支援117位元內容。
            '//UTF-8編碼的內容一個繁體字佔3位元組，所以保險的字串長度為39
            '//每次僅處理39各字元
            For i As Integer = 0 To (strOriginal.Length / 39) + 1 - 1
                Dim intStringLength As Integer

                If (strOriginal.Length - i * 39) < 39 Then
                    intStringLength = strOriginal.Length - i * 39
                Else
                    intStringLength = 39
                End If
                '//將字串轉為base64編碼陣列
                bitOriginalSegment = objByteConverter.GetBytes(strOriginal.Substring(i * 39, intStringLength))
                '//取得以RSA公開金鑰加密後的二進位資料
                bitEncryptedSegment = myRSA._RSAEncrypt_2(bitOriginalSegment, objEncryptRSA.ExportParameters(False), True)
                '//將二進位資料轉為base-64編碼字串
                strEncryptedData.AppendLine(String.Format("<p{0}>{1}</p{0}>", i, Convert.ToBase64String(bitEncryptedSegment)))
            Next

            strEncryptedData.AppendLine("</EncryptData>")
            objEncryptRSA.Clear()
            objEncryptRSA = Nothing
            Return strEncryptedData.ToString()
        Catch ex As Exception
            '_WriteLog(String.Format("{0}._RSAEncrypt()", Me._ClassName), String.Format("Source:{0}" & vbCrLf & "Message:{1}", ex.Source, ex.Message), LogLevel.[Error])
            Return ""
        End Try
    End Function
    '/// <summary>
    '   /// RSA加密函式，長度限制為117位元組
    '/// </summary>
    '/// <param name="DataToEncrypt">待加密資料</param>
    '/// <param name="RSAKeyInfo">指定的RSA金鑰</param>
    '/// <param name="DoOAEPPadding">是否採高度加解密演算</param>
    '/// <returns>加密後的二進位陣列</returns>
    Shared Function _RSAEncrypt_2(ByVal DataToEncrypt As Byte(), ByVal RSAKeyInfo As RSAParameters, ByVal DoOAEPPadding As Boolean) As Byte()
        Dim RSA As RSACryptoServiceProvider = New RSACryptoServiceProvider()
        RSA.ImportParameters(RSAKeyInfo)
        Return RSA.Encrypt(DataToEncrypt, DoOAEPPadding)
    End Function

    '    /// <summary>
    '/// UTF-8編碼解密函式
    '/// 支援多段內容合併解密，並自動重組輸出結果。
    '/// </summary>
    '/// <param name="strEncrypted">待解密的資料</param>
    '/// <returns>解密後的資料</returns>
    Shared Function _RSADecrypt_2(ByVal strEncrypted As String, getRSA As String) As String
        Try

            If strEncrypted.Length = 0 Then
                Return ""
            End If

            Dim objByteConverter As UTF8Encoding = New UTF8Encoding()
            Dim objDecryptRSA As RSACryptoServiceProvider = New RSACryptoServiceProvider()
            objDecryptRSA.FromXmlString(getRSA)
            Dim strDecryptedData As StringBuilder = New StringBuilder()
            Dim objXml As XmlDocument = New XmlDocument()
            objXml.LoadXml(strEncrypted)

            If objXml.DocumentElement.ChildNodes.Count > 0 Then

                For i As Integer = 0 To objXml.DocumentElement.ChildNodes.Count - 1
                    '//暫存待解密的二進位資料
                    Dim bitEncryptedSegment As Byte()
                    '//暫存解密後的二進位資料
                    Dim bitDecryptedSegment As Byte()
                    '//將base64編碼字串還原為二進位陣列
                    bitEncryptedSegment = Convert.FromBase64String(objXml.DocumentElement.ChildNodes(i).InnerText)
                    '//判斷位元組長度
                    If bitEncryptedSegment.Length > 0 Then
                        '//取得解密後的二進位陣列
                        bitDecryptedSegment = myRSA._RSADecrypt_2(bitEncryptedSegment, objDecryptRSA.ExportParameters(True), True)
                        '//將二進位資料還原為Unicode編碼字串，並依序重組
                        strDecryptedData.Append(objByteConverter.GetString(bitDecryptedSegment))
                    End If
                Next
            Else
                Return ""
            End If

            Return strDecryptedData.ToString()
        Catch ex As Exception
            '_WriteLog(String.Format("{0}._RSADecrypt()", Me._ClassName), String.Format("Source:{0}" & vbCrLf & "Message:{1}", ex.Source, ex.Message), LogLevel.[Error])
            Return ""
        End Try
    End Function
    '/// <summary>
    '  /// RSA解密函式
    '  /// </summary>
    '  /// <param name="DataToDecrypt">待解密資料</param>
    '  /// <param name="RSAKeyInfo">指定的RSA金鑰</param>
    '  /// <param name="DoOAEPPadding">是否採高度加解密演算</param>
    '  /// <returns>解密後的二進位陣列</returns>
    Shared Function _RSADecrypt_2(ByVal DataToDecrypt As Byte(), ByVal RSAKeyInfo As RSAParameters, ByVal DoOAEPPadding As Boolean) As Byte()
        Dim RSA As RSACryptoServiceProvider = New RSACryptoServiceProvider()
        RSA.ImportParameters(RSAKeyInfo)
        Return RSA.Decrypt(DataToDecrypt, DoOAEPPadding)
    End Function
#End Region

    Shared Sub test2()
        Dim a_RSA_CSP As RSACryptoServiceProvider = myRSA.createRSA_CSP("CC")

        Dim b_RSA_CSP As RSACryptoServiceProvider = myRSA.createRSA_CSP("Bb")
        'Dim data As String = "123456789"
        Dim data As String = "123456781234567899"

        Dim data01 As String = myRSA._RSAEncrypt(data, a_RSA_CSP.ToXmlString(False))
        Console.WriteLine(data01)
        Dim data02 As String = myRSA._RSADecrypt(data01, a_RSA_CSP.ToXmlString(True))
        Console.WriteLine(data02)

        '公key只能加密 私key加密解密
        Dim data03 As String = myRSA._RSAEncrypt(data, b_RSA_CSP.ToXmlString(True))
        Console.WriteLine(data03)
        Dim data04 As String = myRSA._RSADecrypt(data03, b_RSA_CSP.ToXmlString(True))
        Console.WriteLine(data04)


        Dim data05 As String = myRSA._RSAEncrypt(data, a_RSA_CSP.ToXmlString(False))
        Console.WriteLine(data05)
        Dim data06 As String = myRSA._RSAEncrypt(data05, b_RSA_CSP.ToXmlString(False))
        Console.WriteLine(data06)
        Dim data07 As String = myRSA._RSADecrypt(data06, b_RSA_CSP.ToXmlString(True))
        Console.WriteLine(data07)
        Dim data08 As String = myRSA._RSADecrypt(data07, a_RSA_CSP.ToXmlString(True))
        Console.WriteLine(data08)



        'Dim data11 As String = myRSA._RSAEncrypt(b_RSA_CSP.ToXmlString(False), a_RSA_CSP.ToXmlString(False))
        'Console.WriteLine(data11)
        'Dim data12 As String = myRSA._RSADecrypt(data11, b_RSA_CSP.ToXmlString(True))
        'Console.WriteLine(data12)
        'Dim data13 As String = myRSA._RSADecrypt(data12, a_RSA_CSP.ToXmlString(True))
        'Console.WriteLine(data13)
    End Sub

End Class
Public Class myRSA_file
    Public Sub EncryptFile(ByVal OriginalFile As String, ByVal EncrytpFile As String, PublicKey As String)
        Using originalStream As FileStream = New FileStream(OriginalFile, FileMode.Open, FileAccess.Read)

            Using encrytpStream As FileStream = New FileStream(EncrytpFile, FileMode.Create, FileAccess.Write)
                Dim dataByteArray = New Byte(originalStream.Length - 1) {}
                originalStream.Read(dataByteArray, 0, dataByteArray.Length)
                Dim encryptData = encryptor(dataByteArray, PublicKey)
                encrytpStream.Write(encryptData, 0, encryptData.Length)
            End Using
        End Using
    End Sub
    Public Sub DecryptFile(ByVal EncrytpFile As String, ByVal DecrytpFile As String, PrivateKey As String)
        Using encrytpStream As FileStream = New FileStream(EncrytpFile, FileMode.Open, FileAccess.Read)

            Using decrytpStream As FileStream = New FileStream(DecrytpFile, FileMode.Create, FileAccess.Write)
                Dim dataByteArray = New Byte(encrytpStream.Length - 1) {}
                encrytpStream.Read(dataByteArray, 0, dataByteArray.Length)
                Dim decryptData = Me.decryptor(dataByteArray, PrivateKey)
                decrytpStream.Write(decryptData, 0, decryptData.Length)
            End Using
        End Using
    End Sub

    Private Function decryptor(ByVal EncryptDada As Byte(), PrivateKey As String) As Byte()
        If EncryptDada Is Nothing OrElse EncryptDada.Length <= 0 Then
            Throw New NotSupportedException()
        End If
        Dim _rsaCrypto As RSACryptoServiceProvider = createRSA_CSP("sunsky")
        _rsaCrypto.FromXmlString(PrivateKey)
        Dim decrtpyData = _rsaCrypto.Decrypt(EncryptDada, False)
        Return decrtpyData
    End Function
    Private Function encryptor(ByVal OriginalData As Byte(), PublicKey As String) As Byte()
        If OriginalData Is Nothing OrElse OriginalData.Length <= 0 Then
            Throw New NotSupportedException()
        End If
        Dim _rsaCrypto As RSACryptoServiceProvider = createRSA_CSP("sunsky")
        If _rsaCrypto Is Nothing Then
            Throw New ArgumentNullException()
        End If

        _rsaCrypto.FromXmlString(PublicKey)
        Dim encryptData = _rsaCrypto.Encrypt(OriginalData, False)
        Return encryptData
    End Function

     
     
    Shared Function createRSA_CSP(name As String) As RSACryptoServiceProvider

        Dim _cspParameters_A As New CspParameters()
        _cspParameters_A.KeyContainerName = name
        Dim RSA_A As RSACryptoServiceProvider = New RSACryptoServiceProvider(_cspParameters_A)

        Return RSA_A

    End Function
End Class
Imports System.Text
Imports System.Security.Cryptography
Imports System.Xml
'Imports System.Text.Encoder
Imports System.IO
'20200923
Public Class SunSkyActivator2
    Private PrivateKey As String = "<RSAKeyValue><Modulus>tLPbXyqAIrPKEEpxSZ5MK9N3MPzmdCWx5lsVSErph2uOvJoXn8sLRkmL2SaYBa/wGa0kdKZhgfzoSe4trYhZC7eHKaYSc6NrfFXs4CIGYkwFv4H4RqCKK4UeIDqck5PajpLVVbG30zc6P5BYv9fPMEJEMzgcYPCeFbcxJhEuEppL4+QOYS3599XSh3iQr2i7oFXMZ+xzp91BvQsF4Kk+0O623CwFoKrGsnRkhO1JYAE07B1gkUaM2ic9fkOpn4VfCIH5GtwlrfphV7tc8HwI+4odGYpeRq7M/czp1PZlAEdHFcaIo1AehYP0PtBb3M/bE1YIkK+kVjiDm2mHDI1YIQ==</Modulus><Exponent>AQAB</Exponent><P>476o8LZGCRxGN3KV71c5WTzIoxGIL5vf/rCcqNfFVFs2kNlrXbYtDWhSmj+K3YUMJslHoOseOpcF9avH+0dAoiaT1jh19EH58ULOXp+xSvdQgcmYHfN1cdmD4tm/XU6vp2hm5yLTUZhWkm7wpSj4IXpTv3FqMWhnP/JGSBmFIDs=</P><Q>yx8aNnsEUHA2wqmgzLcRLIvxso2qTQ/tU9Z8zJimrA73qDetAZQDhEwOvSpUclcdBykl5ihafAyJA5k0wZlBI6VrKLliJiiZGqdMIRzHjXyjzFlmHjslmYi3FJrTU9DWTsIlgfRdRz0SCzxNnA1OS+G7vOZUna3iur3Dy6VqX1M=</Q><DP>pl5KRYWxxcf811aChxP91d2cZ9tP1A+XYxObbZAqG8SCKPBbCVsisC+sX/fZNpeR1+ejxr7bF0vp05yIe1yCr7Fkv9IBAM0NjBwUa3VW63+dNSKSWBbYjbGrMZWFwODRWobe3SxImMujOleGvfAeyz30Xd65B5zQCBuxEcxqvq8=</DP><DQ>n1sPXZ61i8X9sEsUdIdLWf+Q59xst3i/YP7tejZozKQReE/10z8kYy6ogZAsIGhnxa5qpV8TXi8Xb1NLKHfruuOUZqbKcdV4CIkoGPJTPJWEjFW24BDXNtUjjW7KTP+Sosd+Va45YCJxfY8Z9EwcGTxH5bNuvyYksw0eBy8HfTE=</DQ><InverseQ>FEzr0pLhXtoL5il0P34HfWpo7nDjF4jl3FoJXVMF3Z/UrmbTkgGz1sl0OtOcjxJDDeoqg9SDN9E6291BUDM4EYpPtWbrilP8dF0sSsyWAFZYpUqNX83NNdBpasCnAhO6+b4Ex8bZgpnm2c+oV63dKhzAWjdTsW/qCl1BLNFur2k=</InverseQ><D>TzBCgoHthUek1V7KJqjoWIxjP2SU2XFrdLeVrAioLmBEPQKKN8yTNEomrxLqrBNd9OKbh0MimARtB3kJK3MFEs1qviW5EFQdVm+RTueaRJlsK3CZjNntswSzHwyQcVvp+3D9bIBlPnqFXDW1rty0P0HLAWcliJXpsTqGn1gICL44keb5aVMnieN1dAt1UuYaUlIey/gy9GNHQSyLT6SHJCBXABesXVlQv8c5hxGots6EwAcAGNGLfoFb5H3gYhUBojMDzdWn2fH+zfwg9Boxujk8aW8fwc1mmvZqcTNAMM6G146xJIbr3zEbz6/HXhP7sMWJzao8ybeO6Rb7QJhbwQ==</D></RSAKeyValue>"
    Private PublicKey As String = "<RSAKeyValue><Modulus>tLPbXyqAIrPKEEpxSZ5MK9N3MPzmdCWx5lsVSErph2uOvJoXn8sLRkmL2SaYBa/wGa0kdKZhgfzoSe4trYhZC7eHKaYSc6NrfFXs4CIGYkwFv4H4RqCKK4UeIDqck5PajpLVVbG30zc6P5BYv9fPMEJEMzgcYPCeFbcxJhEuEppL4+QOYS3599XSh3iQr2i7oFXMZ+xzp91BvQsF4Kk+0O623CwFoKrGsnRkhO1JYAE07B1gkUaM2ic9fkOpn4VfCIH5GtwlrfphV7tc8HwI+4odGYpeRq7M/czp1PZlAEdHFcaIo1AehYP0PtBb3M/bE1YIkK+kVjiDm2mHDI1YIQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"

    Private Function _GetRSA(ByVal IsPrivateKey As Boolean) As String
        If Not IsPrivateKey Then
            Return PublicKey
        Else
            Return PrivateKey
        End If
    End Function
    Private Function _RSAEncrypt(ByVal strOriginal As String) As String
        Try
            Dim objByteConverter As UTF8Encoding = New UTF8Encoding()
            Dim objEncryptRSA As RSACryptoServiceProvider = New RSACryptoServiceProvider()
            objEncryptRSA.FromXmlString(Me._GetRSA(False))
            Dim bitOriginalSegment As Byte()
            Dim bitEncryptedSegment As Byte()
            Dim strEncryptedData As StringBuilder = New StringBuilder()
            strEncryptedData.AppendLine("<EncryptData>")

            For i As Integer = 0 To (strOriginal.Length / 39) + 1 - 1
                Dim intStringLength As Integer

                If (strOriginal.Length - i * 39) < 39 Then
                    intStringLength = strOriginal.Length - i * 39
                Else
                    intStringLength = 39
                End If

                bitOriginalSegment = objByteConverter.GetBytes(strOriginal.Substring(i * 39, intStringLength))
                bitEncryptedSegment = Me._RSAEncrypt(bitOriginalSegment, objEncryptRSA.ExportParameters(False), True)
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

    Private Function _RSADecrypt(ByVal strEncrypted As String) As String
        Try

            If strEncrypted.Length = 0 Then
                Return ""
            End If

            Dim objByteConverter As UTF8Encoding = New UTF8Encoding()
            Dim objDecryptRSA As RSACryptoServiceProvider = New RSACryptoServiceProvider()
            objDecryptRSA.FromXmlString(Me._GetRSA(True))
            Dim strDecryptedData As StringBuilder = New StringBuilder()
            Dim objXml As XmlDocument = New XmlDocument()
            objXml.LoadXml(strEncrypted)

            If objXml.DocumentElement.ChildNodes.Count > 0 Then

                For i As Integer = 0 To objXml.DocumentElement.ChildNodes.Count - 1
                    Dim bitEncryptedSegment As Byte()
                    Dim bitDecryptedSegment As Byte()
                    bitEncryptedSegment = Convert.FromBase64String(objXml.DocumentElement.ChildNodes(i).InnerText)

                    If bitEncryptedSegment.Length > 0 Then
                        bitDecryptedSegment = Me._RSADecrypt(bitEncryptedSegment, objDecryptRSA.ExportParameters(True), True)
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

    Private Function _RSAEncrypt(ByVal DataToEncrypt As Byte(), ByVal RSAKeyInfo As RSAParameters, ByVal DoOAEPPadding As Boolean) As Byte()
        Dim RSA As RSACryptoServiceProvider = New RSACryptoServiceProvider()
        RSA.ImportParameters(RSAKeyInfo)
        Return RSA.Encrypt(DataToEncrypt, DoOAEPPadding)
    End Function

    Private Function _RSADecrypt(ByVal DataToDecrypt As Byte(), ByVal RSAKeyInfo As RSAParameters, ByVal DoOAEPPadding As Boolean) As Byte()
        Dim RSA As RSACryptoServiceProvider = New RSACryptoServiceProvider()
        RSA.ImportParameters(RSAKeyInfo)

        Return RSA.Decrypt(DataToDecrypt, DoOAEPPadding)
    End Function

    'https://dotblogs.com.tw/yc421206/archive/2012/06/25/73041.aspx
    Public Sub GenerateKey()
        '建立金鑰容器
             'Dim BLOCK_SIZE As Integer = 1
        Dim _cspParameters As New CspParameters()
        '表示 CspParameters 的金鑰容器名稱
        _cspParameters.KeyContainerName = "john"

        'https://docs.microsoft.com/zh-tw/dotnet/api/system.security.cryptography.cspproviderflags?view=netframework-4.8
        '_cspParameters.Flags = CspProviderFlags.UseMachineKeyStore
        '表示 CspParameters 的提供者名稱 不能自訂
        '_cspParameters.ProviderName = "Microsoft Strong Cryptographic Provider"

        '若要指定與 RSA 演算法相容的提供者 1
        '若要指定與 DSA 演算法相容的提供者 13
        'https://docs.microsoft.com/zh-tw/dotnet/api/system.security.cryptography.cspparameters.providertype?view=netframework-4.8

        '_cspParameters.ProviderType = 1


        Dim RSA As RSACryptoServiceProvider = New RSACryptoServiceProvider(_cspParameters)
        'PrivateKey 
        Console.WriteLine(RSA.ToXmlString(True))
        PrivateKey = RSA.ToXmlString(True)
        'PublicKey 
        Console.WriteLine(RSA.ToXmlString(False))
        PublicKey = RSA.ToXmlString(False)
    End Sub
    Public Function get_GenerateKey() As RSACryptoServiceProvider
        '建立金鑰容器
        'Dim BLOCK_SIZE As Integer = 1
        Dim _cspParameters As New CspParameters()
        '表示 CspParameters 的金鑰容器名稱
        _cspParameters.KeyContainerName = "john"

        'https://docs.microsoft.com/zh-tw/dotnet/api/system.security.cryptography.cspproviderflags?view=netframework-4.8
        '_cspParameters.Flags = CspProviderFlags.UseMachineKeyStore
        '表示 CspParameters 的提供者名稱 不能自訂
        '_cspParameters.ProviderName = "Microsoft Strong Cryptographic Provider"

        '若要指定與 RSA 演算法相容的提供者 1
        '若要指定與 DSA 演算法相容的提供者 13
        'https://docs.microsoft.com/zh-tw/dotnet/api/system.security.cryptography.cspparameters.providertype?view=netframework-4.8

        '_cspParameters.ProviderType = 1
        Dim RSA As RSACryptoServiceProvider = New RSACryptoServiceProvider(_cspParameters)

        Return RSA
    End Function
    Public Function get_GenerateKey(prikey As String, pubKey As String) As RSACryptoServiceProvider
        '建立金鑰容器
        'Dim BLOCK_SIZE As Integer = 1
        Dim _cspParameters As New CspParameters()
        '表示 CspParameters 的金鑰容器名稱
        _cspParameters.KeyContainerName = "john"

        'https://docs.microsoft.com/zh-tw/dotnet/api/system.security.cryptography.cspproviderflags?view=netframework-4.8
        '_cspParameters.Flags = CspProviderFlags.UseMachineKeyStore
        '表示 CspParameters 的提供者名稱 不能自訂
        '_cspParameters.ProviderName = "Microsoft Strong Cryptographic Provider"

        '若要指定與 RSA 演算法相容的提供者 1
        '若要指定與 DSA 演算法相容的提供者 13
        'https://docs.microsoft.com/zh-tw/dotnet/api/system.security.cryptography.cspparameters.providertype?view=netframework-4.8

        '_cspParameters.ProviderType = 1
        Dim RSA As RSACryptoServiceProvider = New RSACryptoServiceProvider(_cspParameters)
        RSA.FromXmlString(prikey)
        RSA.FromXmlString(pubKey)
        Return RSA
    End Function
    Private Function encryptor(ByVal OriginalData As Byte()) As Byte()
        If OriginalData Is Nothing OrElse OriginalData.Length <= 0 Then
            Throw New NotSupportedException()
        End If
        Dim _rsaCrypto As RSACryptoServiceProvider = get_GenerateKey()
        If _rsaCrypto Is Nothing Then
            Throw New ArgumentNullException()
        End If

        _rsaCrypto.FromXmlString(Me.PublicKey)
        Dim encryptData = _rsaCrypto.Encrypt(OriginalData, False)
        Return encryptData
    End Function
    Private Function decryptor(ByVal EncryptDada As Byte()) As Byte()
        If EncryptDada Is Nothing OrElse EncryptDada.Length <= 0 Then
            Throw New NotSupportedException()
        End If
        Dim _rsaCrypto As RSACryptoServiceProvider = get_GenerateKey()
        _rsaCrypto.FromXmlString(Me.PrivateKey)
        Dim decrtpyData = _rsaCrypto.Decrypt(EncryptDada, False)
        Return decrtpyData
    End Function

    Public Function GetSignature(ByVal OriginalString As String) As String
        If String.IsNullOrEmpty(OriginalString) Then
            Throw New ArgumentNullException()
        End If
        Dim _rsaCrypto As RSACryptoServiceProvider = New RSACryptoServiceProvider
        _rsaCrypto.FromXmlString(Me.PrivateKey)
        Dim Encode As System.Text.Encoding = Encoding.Unicode
        Dim originalData = Encode.GetBytes(OriginalString)
        Dim singData = _rsaCrypto.SignData(originalData, New SHA1CryptoServiceProvider())
        Dim SignatureString = Convert.ToBase64String(singData)
        Return SignatureString
    End Function
    Public Function VerifySignature(ByVal OriginalString As String, ByVal SignatureString As String) As Boolean
        If String.IsNullOrEmpty(OriginalString) Then
            Throw New ArgumentNullException()
        End If
        Dim _rsaCrypto As RSACryptoServiceProvider = New RSACryptoServiceProvider
        _rsaCrypto.FromXmlString(Me.PublicKey)
        Dim Encode As System.Text.Encoding = Encoding.Unicode
        Dim originalData = Encode.GetBytes(OriginalString)
        Dim signatureData = Convert.FromBase64String(SignatureString)
        Dim isVerify = _rsaCrypto.VerifyData(originalData, New SHA1CryptoServiceProvider(), signatureData)
        Return isVerify
    End Function
    Public Function EncryptString(ByVal OriginalString As String) As String
        If String.IsNullOrEmpty(OriginalString) Then
            Throw New NotSupportedException()
        End If
        Dim Encode As System.Text.Encoding = Encoding.Unicode
        Dim originalData = Encode.GetBytes(OriginalString)
        Dim encryptData = Me.encryptor(originalData)
        Dim base64 = Convert.ToBase64String(encryptData)
        Return base64
    End Function
    Public Sub EncryptFile(ByVal OriginalFile As String, ByVal EncrytpFile As String)
        Using originalStream As FileStream = New FileStream(OriginalFile, FileMode.Open, FileAccess.Read)

            Using encrytpStream As FileStream = New FileStream(EncrytpFile, FileMode.Create, FileAccess.Write)
                Dim dataByteArray = New Byte(originalStream.Length - 1) {}
                originalStream.Read(dataByteArray, 0, dataByteArray.Length)
                Dim encryptData = encryptor(dataByteArray)
                encrytpStream.Write(encryptData, 0, encryptData.Length)
            End Using
        End Using
    End Sub
    Public Function DecryptString(ByVal EncryptString As String) As String
        If String.IsNullOrEmpty(EncryptString) Then
            Throw New NotSupportedException()
        End If

        Dim encryptData = Convert.FromBase64String(EncryptString)
        Dim decryptData = Me.decryptor(encryptData)
        Dim Encode As System.Text.Encoding = Encoding.Unicode
        Dim decryptString_r = Encode.GetString(decryptData)
        Return decryptString_r
    End Function
    Public Sub DecryptFile(ByVal EncrytpFile As String, ByVal DecrytpFile As String)
        Using encrytpStream As FileStream = New FileStream(EncrytpFile, FileMode.Open, FileAccess.Read)

            Using decrytpStream As FileStream = New FileStream(DecrytpFile, FileMode.Create, FileAccess.Write)
                Dim dataByteArray = New Byte(encrytpStream.Length - 1) {}
                encrytpStream.Read(dataByteArray, 0, dataByteArray.Length)
                Dim decryptData = Me.decryptor(dataByteArray)
                decrytpStream.Write(decryptData, 0, decryptData.Length)
            End Using
        End Using
    End Sub

    'https://blog.darkthread.net/blog/rsa-notes/
    Sub test()
        'RSA加密明文最大长度117字节，解密要求密文最大长度为128字节
        '建立金鑰容器
        'Dim BLOCK_SIZE As Integer = 1
        Dim _cspParameters_A As New CspParameters()

        '表示 CspParameters 的金鑰容器名稱
        _cspParameters_A.KeyContainerName = "john"
        'https://docs.microsoft.com/zh-tw/dotnet/api/system.security.cryptography.cspproviderflags?view=netframework-4.8
        '_cspParameters.Flags = CspProviderFlags.UseMachineKeyStore
        '表示 CspParameters 的提供者名稱 不能自訂
        '_cspParameters.ProviderName = "Microsoft Strong Cryptographic Provider"
        '若要指定與 RSA 演算法相容的提供者 1
        '若要指定與 DSA 演算法相容的提供者 13
        'https://docs.microsoft.com/zh-tw/dotnet/api/system.security.cryptography.cspparameters.providertype?view=netframework-4.8
        '_cspParameters_A.ProviderType = 1

        '假設兩家廠商有各自的Key
        'Dim RSA_A As RSACryptoServiceProvider = New RSACryptoServiceProvider(1280, _cspParameters_A)
        Dim RSA_A As RSACryptoServiceProvider = New RSACryptoServiceProvider(1280)
        'RSA_A.
        Dim RSA_A_key As String = RSA_A.ToXmlString(False)
        Dim _cspParameters_B As New CspParameters()
        _cspParameters_B.KeyContainerName = "julia"
        '_cspParameters_B.ProviderType = 1
        'Dim RSA_B As RSACryptoServiceProvider = New RSACryptoServiceProvider(1280, _cspParameters_B)
        Dim RSA_B As RSACryptoServiceProvider = New RSACryptoServiceProvider(1280)
        Dim RSA_B_key As String = RSA_B.ToXmlString(False)
        Dim word As String = "www."
        Dim Encode As System.Text.Encoding = Encoding.Unicode

        'B：拿著A發佈的公開金鑰加密，並用自己的私密金鑰產生簽章
        Dim o_data_1 = Encode.GetBytes(word)
        RSA_B.FromXmlString(RSA_B.ToXmlString(True))
        Dim data_1 = RSA_B.Encrypt(o_data_1, False)
        Dim base64_1 As String = Convert.ToBase64String(data_1)
        'A：拿著B加密後的資料，用自己的私密金鑰解密。
        Dim o_data_2 = Convert.FromBase64String(base64_1)
        RSA_A.FromXmlString(RSA_A.ToXmlString(True))
        Dim data_2 = RSA_A.Decrypt(o_data_2, False)
        Dim base64_2 As String = Convert.ToBase64String(data_2)
        'A：拿著B發佈的公開金鑰

        'A：用B給的公開金鑰驗証簽章資料，驗証成功表示該A拿到的資料確實是B給的。
        If base64_2.Equals(word) Then
            Console.WriteLine("pass")
        Else
            Console.WriteLine("fail")
        End If

    End Sub
    Sub test2()
        Dim _cspParameters_A As New CspParameters()

        '表示 CspParameters 的金鑰容器名稱
        _cspParameters_A.KeyContainerName = "john"
 
        'Dim RSA_A As RSACryptoServiceProvider = New RSACryptoServiceProvider(1280, _cspParameters_A)
        'Dim RSA_A As RSACryptoServiceProvider = New RSACryptoServiceProvider(1280)
        For index As Integer = 0 To 9
            Dim tt As New CspParameters()
            tt.KeyContainerName = "john" + index.ToString
            Dim RSA_A As RSACryptoServiceProvider = New RSACryptoServiceProvider(1001, tt)
            Console.WriteLine(RSA_A.ToXmlString(False))
        Next


    End Sub
    Sub test3()
        Dim data As String = "test03"
        Dim Signature As String = Me.GetSignature(data)

        Dim v As Boolean = VerifySignature(data, Signature)

        Dim sha As New SHA1CryptoServiceProvider
        Dim Encode As System.Text.Encoding = Encoding.Unicode
        Dim originalData = Encode.GetBytes(data)
        Console.WriteLine(Convert.ToBase64String(sha.ComputeHash(originalData)))
    End Sub
End Class

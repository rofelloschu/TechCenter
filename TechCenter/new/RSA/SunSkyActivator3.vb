Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Security.Cryptography
Imports System.IO
Imports System.Xml
Imports System.IO.IsolatedStorage
Public NotInheritable Class SunSkyActivator3
    Private Shared _SunSkyActivator As SunSkyActivator3 = New SunSkyActivator3()
    Private PrivateKey As String = "<RSAKeyValue><Modulus>tLPbXyqAIrPKEEpxSZ5MK9N3MPzmdCWx5lsVSErph2uOvJoXn8sLRkmL2SaYBa/wGa0kdKZhgfzoSe4trYhZC7eHKaYSc6NrfFXs4CIGYkwFv4H4RqCKK4UeIDqck5PajpLVVbG30zc6P5BYv9fPMEJEMzgcYPCeFbcxJhEuEppL4+QOYS3599XSh3iQr2i7oFXMZ+xzp91BvQsF4Kk+0O623CwFoKrGsnRkhO1JYAE07B1gkUaM2ic9fkOpn4VfCIH5GtwlrfphV7tc8HwI+4odGYpeRq7M/czp1PZlAEdHFcaIo1AehYP0PtBb3M/bE1YIkK+kVjiDm2mHDI1YIQ==</Modulus><Exponent>AQAB</Exponent><P>476o8LZGCRxGN3KV71c5WTzIoxGIL5vf/rCcqNfFVFs2kNlrXbYtDWhSmj+K3YUMJslHoOseOpcF9avH+0dAoiaT1jh19EH58ULOXp+xSvdQgcmYHfN1cdmD4tm/XU6vp2hm5yLTUZhWkm7wpSj4IXpTv3FqMWhnP/JGSBmFIDs=</P><Q>yx8aNnsEUHA2wqmgzLcRLIvxso2qTQ/tU9Z8zJimrA73qDetAZQDhEwOvSpUclcdBykl5ihafAyJA5k0wZlBI6VrKLliJiiZGqdMIRzHjXyjzFlmHjslmYi3FJrTU9DWTsIlgfRdRz0SCzxNnA1OS+G7vOZUna3iur3Dy6VqX1M=</Q><DP>pl5KRYWxxcf811aChxP91d2cZ9tP1A+XYxObbZAqG8SCKPBbCVsisC+sX/fZNpeR1+ejxr7bF0vp05yIe1yCr7Fkv9IBAM0NjBwUa3VW63+dNSKSWBbYjbGrMZWFwODRWobe3SxImMujOleGvfAeyz30Xd65B5zQCBuxEcxqvq8=</DP><DQ>n1sPXZ61i8X9sEsUdIdLWf+Q59xst3i/YP7tejZozKQReE/10z8kYy6ogZAsIGhnxa5qpV8TXi8Xb1NLKHfruuOUZqbKcdV4CIkoGPJTPJWEjFW24BDXNtUjjW7KTP+Sosd+Va45YCJxfY8Z9EwcGTxH5bNuvyYksw0eBy8HfTE=</DQ><InverseQ>FEzr0pLhXtoL5il0P34HfWpo7nDjF4jl3FoJXVMF3Z/UrmbTkgGz1sl0OtOcjxJDDeoqg9SDN9E6291BUDM4EYpPtWbrilP8dF0sSsyWAFZYpUqNX83NNdBpasCnAhO6+b4Ex8bZgpnm2c+oV63dKhzAWjdTsW/qCl1BLNFur2k=</InverseQ><D>TzBCgoHthUek1V7KJqjoWIxjP2SU2XFrdLeVrAioLmBEPQKKN8yTNEomrxLqrBNd9OKbh0MimARtB3kJK3MFEs1qviW5EFQdVm+RTueaRJlsK3CZjNntswSzHwyQcVvp+3D9bIBlPnqFXDW1rty0P0HLAWcliJXpsTqGn1gICL44keb5aVMnieN1dAt1UuYaUlIey/gy9GNHQSyLT6SHJCBXABesXVlQv8c5hxGots6EwAcAGNGLfoFb5H3gYhUBojMDzdWn2fH+zfwg9Boxujk8aW8fwc1mmvZqcTNAMM6G146xJIbr3zEbz6/HXhP7sMWJzao8ybeO6Rb7QJhbwQ==</D></RSAKeyValue>"
    Private PublicKey As String = "<RSAKeyValue><Modulus>tLPbXyqAIrPKEEpxSZ5MK9N3MPzmdCWx5lsVSErph2uOvJoXn8sLRkmL2SaYBa/wGa0kdKZhgfzoSe4trYhZC7eHKaYSc6NrfFXs4CIGYkwFv4H4RqCKK4UeIDqck5PajpLVVbG30zc6P5BYv9fPMEJEMzgcYPCeFbcxJhEuEppL4+QOYS3599XSh3iQr2i7oFXMZ+xzp91BvQsF4Kk+0O623CwFoKrGsnRkhO1JYAE07B1gkUaM2ic9fkOpn4VfCIH5GtwlrfphV7tc8HwI+4odGYpeRq7M/czp1PZlAEdHFcaIo1AehYP0PtBb3M/bE1YIkK+kVjiDm2mHDI1YIQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"
    Private origLicence As String = "CubicianKeyForSunSky22761931"
    Private CpuID As String = ""
    Private fileName As String
    Private _isoStore As IsolatedStorageFile

    Private Sub New()

    End Sub

    Public Shared Function GetApplyCode() As String
        If _SunSkyActivator.CpuID = "unknow" Then
            Throw New Exception("錯誤代碼1：獲取申請碼發生錯誤！")
        End If

        Dim origCode As String = _SunSkyActivator.CpuID
        Dim ApplyCode As String = _SunSkyActivator._RSAEncrypt(origCode)

        If ApplyCode = "" Then
            Throw New Exception("錯誤代碼3：獲取申請碼發生錯誤！")
        End If

        Return ApplyCode
    End Function

    Public Shared Sub ReadAuthentication()
    End Sub

    Public Shared Function SaveAuthentication(ByVal data As String) As Boolean
        Try

            If _SunSkyActivator._IsAuthenticated(data) Then
                writeToFile(data)
            End If

            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function IsAuthenticated() As Boolean
        Try
            Dim data As String = readFromFile()
            Return _SunSkyActivator._IsAuthenticated(data)
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function _IsAuthenticated(ByVal data As String) As Boolean
        If CpuID = "unknow" Then
            Throw New Exception("錯誤代碼1：驗證啟動檔發生錯誤！")
        End If

        Dim decryptData As String = _SunSkyActivator._RSADecrypt(data)

        If decryptData = "" Then
            Throw New Exception("錯誤代碼3:驗證啟動檔發生錯誤！")
        End If

        Dim origCode As String = CpuID

        If Not decryptData.StartsWith(origCode) Then
            Throw New Exception("錯誤代碼4：驗證啟動檔發生錯誤！")
        End If

        Dim licence As String = decryptData.Substring(origCode.Length)

        If origLicence <> _RSADecrypt(licence) Then
            Throw New Exception("錯誤代碼5：驗證啟動檔發生錯誤！")
        End If

        Return True
    End Function

    Private Shared Sub writeToFile(ByVal data As String)
        Dim isoStore As IsolatedStorageFile = _SunSkyActivator._isoStore
        Dim writer As StreamWriter = Nothing
        writer = New StreamWriter(New IsolatedStorageFileStream(_SunSkyActivator.fileName, FileMode.Create, isoStore), Encoding.UTF8)
        writer.Write(data)
        writer.Close()
    End Sub

    Private Shared Function readFromFile() As String
        Dim data As String = ""
        Dim isoStore As IsolatedStorageFile = _SunSkyActivator._isoStore
        Dim fileNames As String() = isoStore.GetFileNames(_SunSkyActivator.fileName)

        For Each file As String In fileNames

            If file = _SunSkyActivator.fileName Then
                Dim reader As StreamReader = New StreamReader(New IsolatedStorageFileStream(file, FileMode.Open, isoStore), Encoding.UTF8)
                data = reader.ReadToEnd()
                reader.Close()
            End If
        Next

        Return data
    End Function

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
End Class
Public Class use_key
    '1取得key 儲存
    '2取得M-key
    '3
End Class

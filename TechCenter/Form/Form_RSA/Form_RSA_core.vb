'讀key
'1加密 
'2存檔
Imports System.IO
Imports System.Text
Imports System.Security.Cryptography
Imports System.Management '加入參考 
Public Class Form_RSA_core
    'Public rsa As myRSA
    Sub New()

    End Sub
    Sub create()
        Dim rsa As RSACryptoServiceProvider = myRSA.createRSA_CSP(Now.ToString)
        Dim data As String = rsa.ToXmlString(False) + rsa.ToXmlString(True)
        Me.saveFile("key.txt", data)
    End Sub
    Sub create(name As String)
        Dim rsa As RSACryptoServiceProvider = myRSA.createRSA_CSP(name)
        Dim data As String = rsa.ToXmlString(False) + rsa.ToXmlString(True)
        Me.saveFile("key.txt", data)
    End Sub
    Sub create(name As String, filename As String)
        Dim rsa As RSACryptoServiceProvider = myRSA.createRSA_CSP(name)
        Dim data As String = rsa.ToXmlString(False) + rsa.ToXmlString(True)
        Me.saveFile(filename + ".txt", data)
    End Sub
    Function readFile(filepath As String) As String
        Dim data As String = ""
        If Not System.IO.File.Exists(filepath) Then
            Return data
        End If
        Dim reader As StreamReader = New StreamReader(filepath, Encoding.UTF8)
        data = reader.ReadToEnd()
        reader.Close()


        Return data
    End Function

    Sub saveFile(filepath As String, data As String)

        Dim writer As StreamWriter = Nothing
        writer = New StreamWriter(filepath, False, Encoding.UTF8)
        writer.Write(data)
        writer.Close()
    End Sub
    Function run_typeA(key As String) As String
        Dim data As String = Decrypt_typea.GetCpuID
        Return myRSA._RSAEncrypt(data, key)

    End Function
    Function dec_typeA(filepath As String, key As String) As Boolean
        Dim data As String = readFile(filepath)
        Return Decrypt_typea.IsAuthenticated(data, key)
    End Function
     

    Function run_type(key As String, text As String) As String
        'Dim data As String = GetCpuID()
        Return myRSA._RSAEncrypt(text, key)

    End Function
    Function dec_type(filepath As String, key As String) As Boolean
        Dim data As String = readFile(filepath)
        Return Decrypt_typea.IsAuthenticated(data, key)
    End Function
End Class

Class Decrypt_typea
 

    Shared Function IsAuthenticated(data As String, key As String) As Boolean
        Try
            Dim a As String = myRSA._RSADecrypt(data, key)
            Dim b As String = GetCpuID()
            Return a.Equals(b)
        Catch ex As Exception
            Return False
        End Try

    End Function
    Public Shared Function GetCpuID() As String
        Try
            Dim cpuInfo As String = ""
            Dim mc As ManagementClass = New ManagementClass("Win32_Processor")
            Dim moc As ManagementObjectCollection = mc.GetInstances()

            For Each mo As ManagementObject In moc
                cpuInfo = mo.Properties("ProcessorId").Value.ToString()
            Next

            moc = Nothing
            mc = Nothing
            Return cpuInfo
        Catch
            Return "unknow"
        Finally
        End Try
    End Function
End Class

Class Decrypt_type_all
    Shared Function IsAuthenticated(filename As String, key As String) As Boolean
        'Try
        '    Dim d As Byte() = System.IO.File.ReadAllBytes(filename)
        '    Dim a As String = myRSA._RSADecryp(d, key)
        '    'Dim b As String = GetCpuID()
        '    Return a.Equals(b)
        'Catch ex As Exception
        '    Return False
        'End Try

    End Function
End Class
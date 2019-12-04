'FTP上傳下載
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Net
Imports System.IO

'Imports OpenNETCF.Net
Imports OpenNETCF.Net.Ftp

Public Class SdfFtp


    '' FTP Server
    Public sFTPServer As String = "ftp://www.sunsky.com.tw"
    '' FTP User Account
    Public sUserName As String = "sunsky"
    ''FTP sPassword
    Public sPassword As String = "22761931"

    Public Sub UploadFile(ByVal LocalFile As String, ByVal RemoteFile As String)
        Try



            Dim FtpRequestCreator As New FtpRequestCreator
            WebRequest.RegisterPrefix("ftp:", FtpRequestCreator)
            Dim oFtpWebRequest As New FtpWebRequest(New Uri(sFTPServer))
            oFtpWebRequest.Credentials = New NetworkCredential(sUserName, sPassword)
            ' Getting the Request stream
            Dim oFtpRequestStream As Stream = oFtpWebRequest.GetRequestStream()


            Dim oStreamReader As New StreamReader(oFtpRequestStream)
            'Just ignore the result, but read it.
            Dim oReader As String = oStreamReader.ReadToEnd


            'Open the input file. If the file does not exist, it's an error.
            Dim oFileStream As New FileStream(LocalFile, FileMode.Open)
            'Create the reader for the local file data.
            Dim oFileReader As BinaryReader = New BinaryReader(oFileStream)


            'Opening the data connection, this must be done before we issue the command.
            Dim oFtpResponseStream As Stream = oFtpWebRequest.GetResponse().GetResponseStream()
            Dim oDataWriter As New BinaryWriter(oFtpResponseStream)


            'Prepare to send commands to the server.
            Dim cmdWriter As New StreamWriter(oFtpRequestStream)

            'Set transfer type to IMAGE (binary).
            cmdWriter.Write("TYPE I" & vbCrLf)
            cmdWriter.Flush()
            'Reading the request output
            Dim oStreamReader2 As New StreamReader(oFtpRequestStream)
            oReader = oStreamReader.ReadToEnd()

            'Reading the request output
            Dim sReader As String
            sReader = oStreamReader2.ReadToEnd()

            'Write the command to the request stream.
            cmdWriter.Write("STOR " + Path.GetFileName(RemoteFile) + vbCrLf)
            cmdWriter.Flush()

            sReader = oStreamReader2.ReadToEnd()


            'Allocate buffer for the data, which will be written in blocks.
            Dim bytes As Integer
            Dim bufsize As Integer = 1024
            Dim buffer(bufsize) As Byte

            While True
                bytes = oFileReader.Read(buffer, 0, bufsize)
                oDataWriter.Write(buffer, 0, bytes)
                If bytes <= 0 Then
                    Exit While
                End If
            End While

            oFileReader.Close()
            oFileStream.Close()
            oDataWriter.Close()
            oStreamReader.Close()
            GC.Collect()
            Console.WriteLine("Upload OK!")
        Catch ex As Exception
            Console.WriteLine("Upload error" + ex.InnerException.ToString + ex.Message)
            Throw ex

        End Try



    End Sub
    Public Sub DownloadFile(ByVal LocalFile As String, ByVal RemoteFile As String)
        Try


            Dim FtpRequestCreator As New FtpRequestCreator
            WebRequest.RegisterPrefix("ftp:", FtpRequestCreator)
            Dim oFtpWebRequest As New FtpWebRequest(New Uri(sFTPServer))
            oFtpWebRequest.Credentials = New NetworkCredential(sUserName, sPassword)
            ' Getting the Request stream
            Dim oFtpRequestStream As Stream = oFtpWebRequest.GetRequestStream()
            Dim oStreamReader As New StreamReader(oFtpRequestStream)
            'Just ignore the result, but read it.
            Dim oReader As String = oStreamReader.ReadToEnd


            'Open the input file. If the file does not exist, it's an error.
            Dim oFileStream As FileStream
            If File.Exists(LocalFile) Then
                'Open the input file. If the file does not exist, it's an error.
                oFileStream = New FileStream(LocalFile, FileMode.Open)
            Else
                oFileStream = File.Create(LocalFile)
            End If


            'Create the reader for the local file data.
            Dim oFileWriter As BinaryWriter = New BinaryWriter(oFileStream)
            'Opening the data connection, this must be done before we issue the command.
            Dim oFtpResponseStream As Stream = oFtpWebRequest.GetResponse().GetResponseStream()
            Dim oDataReader As New BinaryReader(oFtpResponseStream)

            'Prepare to send commands to the server.
            Dim cmdWriter As New StreamWriter(oFtpRequestStream)

            'Set transfer type to IMAGE (binary).
            cmdWriter.Write("TYPE I" & vbCrLf)
            cmdWriter.Flush()
            'Reading the request output
            Dim oStreamReader2 As New StreamReader(oFtpRequestStream)
            oReader = oStreamReader.ReadToEnd()

            'Reading the request output
            Dim sReader As String
            sReader = oStreamReader2.ReadToEnd()

            'Write the command to the request stream.
            cmdWriter.Write("RETR " + Path.GetFileName(RemoteFile) + vbCrLf)
            cmdWriter.Flush()

            sReader = oStreamReader2.ReadToEnd()

            'Allocate buffer for the data, which will be written in blocks.
            Dim bytes As Integer
            Dim bufsize As Integer = 1024
            Dim buffer(bufsize) As Byte

            While True
                bytes = oDataReader.Read(buffer, 0, bufsize)
                oFileWriter.Write(buffer, 0, bytes)
                ' bytes = oFileReader.Read(buffer, 0, bufsize)
                'oDataWriter.Write(buffer, 0, bytes)
                If bytes <= 0 Then
                    Exit While
                End If
            End While

            oFileWriter.Close()
            oFileStream.Close()
            oDataReader.Close()
            oStreamReader.Close()
            GC.Collect()
            Console.WriteLine("Download OK!")
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw ex
        End Try
    End Sub
End Class

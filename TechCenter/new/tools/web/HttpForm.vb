Imports System.IO
Imports System.Text
Imports System.Net
Imports System.Collections.Specialized
'20190822
Public Class HttpForm
    Private _files As Dictionary(Of String, String) = New Dictionary(Of String, String)()
    Private _values As Dictionary(Of String, String) = New Dictionary(Of String, String)()

    Public Sub New(ByVal url As String)
        Me.Url = url
        Me.Method = "POST"
    End Sub

    Public Property Method As String
    Public Property Url As String

    Public Function AttachFile(ByVal field As String, ByVal fileName As String) As HttpForm
        _files(field) = fileName
        Return Me
    End Function

    Public Function ResetForm() As HttpForm
        _files.Clear()
        _values.Clear()
        Return Me
    End Function

    Public Function SetValue(ByVal field As String, ByVal value As String) As HttpForm
        _values(field) = value
        Return Me
    End Function

    Public Function Submit() As String
        Return Me.UploadFiles(_files, _values)
    End Function

    Private Function UploadFiles(ByVal files As Dictionary(Of String, String), ByVal otherValues As Dictionary(Of String, String)) As String
        Dim statusdesc As String = ""
        Dim req = CType(WebRequest.Create(Me.Url), HttpWebRequest)
        req.Timeout = 100000
        req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
        req.AllowAutoRedirect = False
        req.KeepAlive = False
        Dim mimeParts = New List(Of MimePart)()

        Try

            If otherValues IsNot Nothing Then

                For Each fieldName In otherValues.Keys
                    Dim part = New MimePart()
                    part.Headers("Content-Disposition") = "form-data; name=""" & fieldName & """"
                    part.Data = New MemoryStream(Encoding.UTF8.GetBytes(otherValues(fieldName)))
                    mimeParts.Add(part)
                Next
            End If

            If files IsNot Nothing Then

                For Each fieldName In files.Keys
                    Dim part = New MimePart()
                    part.Headers("Content-Disposition") = "form-data; name=""" & fieldName & """; filename=""" + files(fieldName) & """"
                    part.Headers("Content-Type") = "application/octet-stream"
                    part.Data = File.OpenRead(files(fieldName))
                    mimeParts.Add(part)
                Next
            End If

            Dim boundary As String = "----------" & DateTime.Now.Ticks.ToString("x")
            req.ContentType = "multipart/form-data; boundary=" & boundary
            req.Method = Me.Method
            Dim contentLength As Long = 0
            Dim _footer As Byte() = Encoding.UTF8.GetBytes("--" & boundary & "--" & vbCrLf)

            For Each part As MimePart In mimeParts
                contentLength += part.GenerateHeaderFooterData(boundary)
            Next

            req.ContentLength = contentLength + _footer.Length
            Dim buffer As Byte() = New Byte(8191) {}
            Dim afterFile As Byte() = Encoding.UTF8.GetBytes(vbCrLf)
            Dim read As Integer

            Using s As Stream = req.GetRequestStream()

                For Each part As MimePart In mimeParts
                    s.Write(part.Header, 0, part.Header.Length)

                    While (CSharpImpl.__Assign(read, part.Data.Read(buffer, 0, buffer.Length))) > 0
                        s.Write(buffer, 0, read)
                    End While

                    part.Data.Dispose()
                    s.Write(afterFile, 0, afterFile.Length)
                Next

                s.Write(_footer, 0, _footer.Length)
                s.Close()
            End Using

            Dim res = CType(req.GetResponse(), HttpWebResponse)
            statusdesc = res.StatusDescription
            res.Close()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            statusdesc = ex.Message.ToString()

            For Each part As MimePart In mimeParts
                If part.Data IsNot Nothing Then part.Data.Dispose()
            Next
        End Try

        Return statusdesc
    End Function

    Private Class MimePart
        Private _headers As NameValueCollection = New NameValueCollection()

        Public ReadOnly Property Headers As NameValueCollection
            Get
                Return _headers
            End Get
        End Property

        Public Property Header As Byte()

        Public Function GenerateHeaderFooterData(ByVal boundary As String) As Long
            Dim sb As StringBuilder = New StringBuilder()
            sb.Append("--")
            sb.Append(boundary)
            sb.AppendLine()

            For Each key As String In _headers.AllKeys
                sb.Append(key)
                sb.Append(": ")
                sb.AppendLine(_headers(key))
            Next

            sb.AppendLine()
            Header = Encoding.UTF8.GetBytes(sb.ToString())
            Return Header.Length + Data.Length + 2
        End Function

        Public Property Data As Stream

        Private Class CSharpImpl
            <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
            Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
                target = value
                Return value
            End Function
        End Class
    End Class

    Private Class CSharpImpl
        <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
        Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function
    End Class
End Class
Class use_HttpForm
    Sub New()

    End Sub
    Sub send(url As String)
        Try
            Dim httpForm As New HttpForm(url)

            Dim retCode As Boolean = False
            'httpForm.AttachFile("image", fileImage)     ' after modification 0603
            httpForm.SetValue("Location", "data.Location").SetValue("dt", "data.dt").SetValue("text", " Data.text") _
            .SetValue("zone", " Data.zone").SetValue("camera_name", " Data.camera_name").SetValue("Plate_ID", "Data.Plate_ID")
            Dim statusdesc As String = httpForm.Submit()

            If statusdesc = "OK" OrElse statusdesc = "Ok" OrElse statusdesc = "ok" Then
                retCode = True
            Else
                retCode = False
            End If
            'data_log.Write(System.String.Format("Host response: {0}", statusdesc))
        Catch ex As Exception
            'data_log.Write(ex.ToString)
        End Try


    End Sub
End Class
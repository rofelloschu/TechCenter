Imports System.Net
Imports System.Text
'HttpWebRequest->HttpWebResponse
Public Class web_Request


    Private cookies As New CookieCollection
    Private m_Request As HttpWebRequest
    Sub New(Request As HttpWebRequest)
        'm_Request = Request
        'm_Request.CookieContainer = New CookieContainer()
        'm_Request.CookieContainer.Add(cookies) ' //recover cookies First request
        'Console.WriteLine(m_Request.Method)
        'Console.WriteLine("@" + Request.RequestUri.ToString)
        M_log.writeline("web.txt", "@" + Request.RequestUri.ToString)
    End Sub
    Function GetResponse() As HttpWebResponse
        Return m_Request.GetResponse
    End Function

    Function post(text As String) As HttpWebResponse
        m_Request.CookieContainer = New CookieContainer()
        m_Request.CookieContainer.Add(cookies) ' //recover cookies First request
        m_Request.Method = WebRequestMethods.Http.Post
        m_Request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2"
        m_Request.AllowWriteStreamBuffering = True
        m_Request.ProtocolVersion = HttpVersion.Version11
        m_Request.AllowAutoRedirect = True
        m_Request.ContentType = "application/x-www-form-urlencoded" '




        Dim postData As String = ""
        Dim byteArray() As Byte = Encoding.UTF8.GetBytes(postData)
        m_Request.ContentLength = byteArray.Length
        Dim newStream As System.IO.Stream = m_Request.GetRequestStream() 'open connection
        newStream.Write(byteArray, 0, byteArray.Length) ' Send the data.
        newStream.Close()

        Return m_Request.GetResponse
    End Function

    Sub close()

    End Sub
#Region "shared"
    Public Shared Function http_get(loginurl As String) As HttpWebResponse
        M_log.writeline("web.txt", "@get " + loginurl)
        Dim request As HttpWebRequest = WebRequest.Create(loginurl)
        Dim cookies As New CookieCollection
        request.CookieContainer = New CookieContainer()
        request.CookieContainer.Add(cookies) ' //recover cookies First request
        Return request.GetResponse()
    End Function

    Public Shared Function http_post(loginurl As String, postData As String) As HttpWebResponse
        M_log.writeline("web.txt", "@post " + loginurl + "@@" + postData)

        Dim cookie As CookieContainer
        Dim request As HttpWebRequest = WebRequest.Create(loginurl)
        request.Method = "POST"
        request.ContentType = "application/x-www-form-urlencoded"
        request.CookieContainer = New CookieContainer()
        Dim loginDataBytes() As Byte = Encoding.ASCII.GetBytes(postData)
        request.ContentLength = loginDataBytes.Length
        Dim stream As System.IO.Stream = request.GetRequestStream()
        stream.Write(loginDataBytes, 0, loginDataBytes.Length)
        stream.Close()
        'Dim response As HttpWebResponse = request.GetResponse()
        'Dim reader As System.IO.StreamReader = New System.IO.StreamReader(response.GetResponseStream(), Encoding.GetEncoding("big5"))
        'Dim returnString As String = reader.ReadToEnd()
        'response.Close()
        Return request.GetResponse()
    End Function
    Public Shared Function ResponseToStr(Response As HttpWebResponse) As String
        Dim sourceCode As String = ""
        Using sr As System.IO.StreamReader = New System.IO.StreamReader(Response.GetResponseStream())
            sourceCode = sr.ReadToEnd()
        End Using
        Return sourceCode
    End Function
#End Region


End Class

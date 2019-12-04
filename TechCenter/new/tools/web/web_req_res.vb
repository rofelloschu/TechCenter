Imports System.Net
Imports System.Text
Public Class web_req_res

    Private m_url As String
    Private isRun As Boolean
    Private m_HttpWebResponse As HttpWebResponse
    Private m_html_text As String
    Sub New(url As String)
        m_url = url
        isRun = False
    End Sub
    Sub New()

    End Sub
    Sub web_get()
        If Not isRun Then
            isRun = True
        End If
        m_HttpWebResponse = Me.http_get(m_url)
        m_html_text = Me.ResponseToStr(m_HttpWebResponse)
    End Sub
    Sub web_post(t As String)
        If Not isRun Then
            isRun = True
        End If
        m_HttpWebResponse = Me.http_post(m_url, t, Encoding.ASCII)
        m_html_text = Me.ResponseToStr(m_HttpWebResponse)
    End Sub
    Public ReadOnly Property html_text As String
        Get
            Return m_html_text
        End Get
    End Property

    Private Function http_get(loginurl As String) As HttpWebResponse
        'M_log.writeline("web.txt", "@get " + loginurl)
        Dim request As HttpWebRequest = WebRequest.Create(loginurl)
        Dim cookies As New CookieCollection
        request.CookieContainer = New CookieContainer()
        request.CookieContainer.Add(cookies) ' //recover cookies First request
        Return request.GetResponse()
    End Function
    Private Function http_post(loginurl As String, postData As String, ByVal t_Encoding As System.Text.Encoding) As HttpWebResponse
        'M_log.writeline("web.txt", "@post " + loginurl + "@@" + postData)

        'Dim cookie As CookieContainer
        Dim request As HttpWebRequest = WebRequest.Create(loginurl)
        request.Method = "POST"
        request.ContentType = "application/x-www-form-urlencoded"
        request.CookieContainer = New CookieContainer()
        Dim loginDataBytes() As Byte = t_Encoding.GetBytes(postData)
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
    Private Function ResponseToStr(Response As HttpWebResponse) As String
        Dim sourceCode As String = ""
        Using sr As System.IO.StreamReader = New System.IO.StreamReader(Response.GetResponseStream())
            sourceCode = sr.ReadToEnd()
        End Using
        Return sourceCode
    End Function
End Class

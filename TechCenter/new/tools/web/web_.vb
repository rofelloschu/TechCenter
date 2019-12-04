Imports System.Net
Imports System.Text
Public Class web_
#Region "shared"


    Public Shared Function http_get(loginurl As String) As HttpWebResponse
        'M_log.writeline("web.txt", "@get " + loginurl)
        Dim request As HttpWebRequest = WebRequest.Create(loginurl)
        Dim cookies As New CookieCollection
        request.CookieContainer = New CookieContainer()
        request.CookieContainer.Add(cookies) ' //recover cookies First request

        Return request.GetResponse()
    End Function
    Public Shared Function http_post(loginurl As String, postData As String, ByVal t_Encoding As System.Text.Encoding) As HttpWebResponse
        'M_log.writeline("web.txt", "@post " + loginurl + "@@" + postData)

        Dim cookie As CookieContainer
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
    Public Shared Function ResponseToStr(Response As HttpWebResponse) As String
        Dim sourceCode As String = ""
        Using sr As System.IO.StreamReader = New System.IO.StreamReader(Response.GetResponseStream())
            sourceCode = sr.ReadToEnd()
        End Using
        Return sourceCode
    End Function
    Public Shared Function ResponseToStr(Response As HttpWebResponse, ByVal t_Encoding As System.Text.Encoding) As String
        Dim sourceCode As String = ""
        Using sr As System.IO.StreamReader = New System.IO.StreamReader(Response.GetResponseStream(), t_Encoding)
            sourceCode = sr.ReadToEnd()
        End Using
        Return sourceCode
    End Function
#End Region

End Class

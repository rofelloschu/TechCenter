Imports System.Net
Imports System.Text
'20190717
Public Class web_C


    Sub New()

    End Sub
    Public Function http_get_BearerToken(loginurl As String, Token As String) As HttpWebResponse
        'M_log.writeline("web.txt", "@get " + loginurl)
        Dim request As HttpWebRequest = WebRequest.Create(loginurl)
        request.Method = "GET"
        'Dim encoded As String = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(user_pass))
        'request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(New ASCIIEncoding().GetBytes(user_pass)))
        request.Headers.Add("Authorization", "Bearer " + Token)

        'Dim cookies As New CookieCollection
        'request.CookieContainer = New CookieContainer()
        'request.CookieContainer.Add(cookies) ' //recover cookies First request
        Return request.GetResponse()
    End Function
    Public Function http_get_basicauth(loginurl As String, user_pass As String) As HttpWebResponse
        'M_log.writeline("web.txt", "@get " + loginurl)
        Dim request As HttpWebRequest = WebRequest.Create(loginurl)
        request.Method = "GET"
        Dim encoded As String = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(user_pass))
        'request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(New ASCIIEncoding().GetBytes(user_pass)))
        request.Headers.Add("Authorization", "Basic " + encoded)
        'Dim cookies As New CookieCollection
        'request.CookieContainer = New CookieContainer()
        'request.CookieContainer.Add(cookies) ' //recover cookies First request
        Return request.GetResponse()
    End Function
    Public Function http_get(loginurl As String) As HttpWebResponse
        'M_log.writeline("web.txt", "@get " + loginurl)
        Dim request As HttpWebRequest = WebRequest.Create(loginurl)
        request.Method = "GET"
        'Dim cookies As New CookieCollection
        'request.CookieContainer = New CookieContainer()
        'request.CookieContainer.Add(cookies) ' //recover cookies First request
        Return request.GetResponse()
    End Function
    Public Function http_post(loginurl As String, postData As String, ByVal t_Encoding As System.Text.Encoding) As HttpWebResponse
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
    Public Function ResponseToStr(Response As HttpWebResponse) As String
        Dim sourceCode As String = ""
        Using sr As System.IO.StreamReader = New System.IO.StreamReader(Response.GetResponseStream())
            sourceCode = sr.ReadToEnd()
        End Using
        Return sourceCode
    End Function
    Public Function ResponseToStr(Response As HttpWebResponse, ByVal t_Encoding As System.Text.Encoding) As String
        Dim sourceCode As String = ""
        Using sr As System.IO.StreamReader = New System.IO.StreamReader(Response.GetResponseStream(), t_Encoding)
            sourceCode = sr.ReadToEnd()
        End Using
        Return sourceCode
    End Function


    Public Function http_get_Digest(loginurl As String, user As String, password As String) As HttpWebResponse
        Dim request As HttpWebRequest = WebRequest.Create(loginurl)
        request.Method = "GET"
        'https://stackoverflow.com/questions/3172510/how-can-i-do-digest-authentication-with-httpwebrequest
        Dim credentialCache As New CredentialCache()
        Dim uriAddress As New Uri(loginurl)
        credentialCache.Add(New Uri(uriAddress.GetLeftPart(UriPartial.Authority)), "Digest", New NetworkCredential(user, password))
        request.Credentials = credentialCache

        Return request.GetResponse()
    End Function
End Class

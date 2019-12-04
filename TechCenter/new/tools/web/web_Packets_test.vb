Imports System.Net
Public Class web_Packets_test

    Private cookies As New CookieCollection

    Sub New(Request As HttpWebRequest)
        Request.CookieContainer = New CookieContainer()
        Request.CookieContainer.Add(cookies) ' //recover cookies First request

        'Console.WriteLine("@" + Request.RequestUri.ToString)
        M_log.writeline("web.txt", "@" + Request.RequestUri.ToString)
    End Sub


    Sub close()

    End Sub
End Class

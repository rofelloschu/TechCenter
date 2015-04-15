'20110807
Imports System.Net
Public Class SendMail
    Private newMail As System.Net.Mail.MailMessage
    Private ToAddressList As List(Of String())
    Private CCAddressList As List(Of String())
    Private BccAddressList As List(Of String())
    Private AttachFileList As List(Of String)
    Private smtpMail As System.Net.Mail.SmtpClient

    Sub New(ByVal sendAddress As String, ByVal nameAddress As String, ByVal host As String)
        newMail = New System.Net.Mail.MailMessage

        setsHost(host)

        With newMail
            '.From = New System.Net.Mail.MailAddress("john.sunsky@gmail.com", "john.sunsky") '寄件者 
            .From = New System.Net.Mail.MailAddress(sendAddress, nameAddress) '寄件者 
            .Body = "Hello Every Body!!" '內文 
            .Subject = "測試資料!!" '主旨 
            .BodyEncoding = System.Text.Encoding.GetEncoding("BIG5") '編碼方式 
            .IsBodyHtml = True '是否為HTML格式 
            .Priority = Net.Mail.MailPriority.High '優先權 
        End With
        ToAddressList = New List(Of String())
        CCAddressList = New List(Of String())
        BccAddressList = New List(Of String())
        AttachFileList = New List(Of String)
    End Sub
    Sub New()
        Me.New("john.sunsky@gmail.com", "john.sunsky", "gmail.com")
    End Sub
    '增加收件人
    Sub addToAddress(ByVal Address As String, ByVal name As String)

        Dim temp As String() = {Address, name}
        ToAddressList.Add(temp)
    End Sub
    '增加副本
    Sub addCCAddress(ByVal Address As String, ByVal name As String)
        Dim temp As String() = {Address, name}
        CCAddressList.Add(temp)
    End Sub
    '增加密件
    Sub addBccAddress(ByVal Address As String, ByVal name As String)
        Dim temp As String() = {Address, name}
        BccAddressList.Add(temp)
    End Sub
    Sub setSubject(ByVal Subject As String)
        newMail.Subject = Subject
    End Sub
    Sub setBody(ByVal Body As String)
        newMail.Body = Body
    End Sub
    Sub setBody(ByVal Line() As String)
        Dim temp As String = Nothing
        Dim br As String = "<br />"
        temp = Line(0)
        For index As Integer = 1 To Line.Length - 1
            temp = temp + br + Line(index)
            'temp = temp + vbCrlf + Line(index)
        Next
        newMail.Body = temp
    End Sub
    Sub send()

        Try
            With newMail
                For i As Int32 = 0 To ToAddressList.Count - 1 '收信人 
                    .To.Add(New System.Net.Mail.MailAddress(ToAddressList(i)(0), ToAddressList(i)(1)))
                Next
                For i As Int32 = 0 To CCAddressList.Count - 1 '副本 
                    .CC.Add(New System.Net.Mail.MailAddress(CCAddressList(i)(0), CCAddressList(i)(1)))
                Next
                For i As Int32 = 0 To BccAddressList.Count - 1 '密件副本 
                    .Bcc.Add(New System.Net.Mail.MailAddress(BccAddressList(i)(0), BccAddressList(i)(1)))
                Next
                For i As Int32 = 0 To AttachFileList.Count - 1 '夾檔 
                    .Attachments.Add(New System.Net.Mail.Attachment(AttachFileList(i)))
                Next
            End With


            smtpMail.SendAsync(newMail, newMail)
            'MsgBox("ok")
        Catch ex As Exception
            'MsgBox(ex.InnerException)
            Throw ex
        End Try

    End Sub
    Sub setsHost(ByVal host As String)
        smtpMail = New System.Net.Mail.SmtpClient
        If host = "gmail" Then
            smtpMail.Host = "smtp.gmail.com"
            smtpMail.Port = 587
            smtpMail.EnableSsl = True
            smtpMail.Credentials = New NetworkCredential("john.sunsky@gmail.com", "sunsky44")
        Else
            smtpMail.Host = "sunsky.com.tw"
            smtpMail.Port = 25
            smtpMail.EnableSsl = False
            smtpMail.Credentials = New NetworkCredential("john", "1qazxcv")
        End If

    End Sub
    Sub setHost(ByVal host As String, ByVal userName As String, ByVal password As String)
        smtpMail = New System.Net.Mail.SmtpClient
        If host = "gmail" Then
            smtpMail.Host = "smtp.gmail.com"
            smtpMail.Port = 587
            smtpMail.EnableSsl = True
            smtpMail.Credentials = New NetworkCredential("john.sunsky@gmail.com", "sunsky44")
        Else
            smtpMail.Host = host
            smtpMail.Port = 25
            smtpMail.EnableSsl = False
            smtpMail.Credentials = New NetworkCredential(userName, password)
        End If

    End Sub
End Class


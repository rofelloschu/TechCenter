Imports System.Net
Imports System.Text
Public Class web_Response

    Private m_Response As HttpWebResponse
    Private encoding As System.Text.Encoding
    Private html_text_list As List(Of html_text)
    Sub New(Response As HttpWebResponse)
        m_Response = Response
        html_text_list = New List(Of html_text)
    End Sub
    Function a(html As String) As String()
        Dim sourceCode As String = ""
        sourceCode = html
        'Using sr As System.IO.StreamReader = New System.IO.StreamReader(m_Response.GetResponseStream())
        '    sourceCode = sr.ReadToEnd()
        'End Using
        Dim text01() As String = sourceCode.Split("<")
        Dim text02 As New List(Of String)
        For index As Integer = 1 To text01.Length - 1
            '1.
            Dim text As String = "<" + text01(index)
            '< 60  > 62
            '2 
            Dim text_byte() As Byte = System.Text.Encoding.Default.GetBytes(text)
            '3
            Dim start_index As Integer = Me.get60_index(text_byte)
            Dim end_index As Integer = Me.get62_index(text_byte)
            If start_index = -1 Or end_index = -1 Then
                Continue For
            End If
            Dim new_byte(end_index - start_index) As Byte
            Array.Copy(text_byte, start_index, new_byte, 0, new_byte.Length)
            Dim new_text As String = System.Text.Encoding.Default.GetString(new_byte)
            '4
            Dim lastchr_index As Integer = Me.getLastChr_index(text_byte)
            Dim new_text2 As String = ""
            If lastchr_index <> -1 And lastchr_index <> end_index Then
                Dim new_byte2(lastchr_index - end_index - 1) As Byte
                Array.Copy(text_byte, end_index + 1, new_byte2, 0, new_byte2.Length)
                new_text2 = System.Text.Encoding.Default.GetString(new_byte2)
            End If
            '5
            If new_text.Length >= 2 AndAlso new_text.Substring(0, 2) = "</" Then
                html_text_list(html_text_list.Count - 1).endtag = html_text_list(html_text_list.Count - 1).endtag + new_text
                text02(text02.Count - 1) = text02(text02.Count - 1) + new_text

                Continue For
            End If
            If new_text.Length >= 4 AndAlso new_text.Substring(0, 4) = "<br>" Then
                html_text_list(html_text_list.Count - 1).text = html_text_list(html_text_list.Count - 1).text + new_text
                text02(text02.Count - 1) = text02(text02.Count - 1) + new_text
                Continue For
            End If
          
            Dim html_text As New html_text
            html_text.starttag = new_text
            html_text.text = new_text2
            html_text_list.Add(html_text)


            text02.Add(new_text + new_text2)
        Next
        Return text02.ToArray
    End Function
    Private Function getLastChr_index(Data() As Byte) As Integer
        For index As Integer = 0 To Data.Length - 1
            Dim new_index As Integer = Data.Length - 1 - index
            '9 水平定位符號  10 換行鍵 32(space)
            If Data(new_index) <> 9 And Data(new_index) <> 10 And Data(new_index) <> 32 Then
                Return new_index
            End If

        Next
        Return -1
    End Function
    Private Function get60_index(Data() As Byte) As Integer
        'Dim r_index As Integer = -1
        For index As Integer = 0 To Data.Length - 1
            If Data(index) = 60 Then
                Return index
            End If
        Next
        Return -1
    End Function
    Private Function get62_index(Data() As Byte) As Integer
        For index As Integer = 0 To Data.Length - 1
            If Data(index) = 62 Then
                Return index
            End If
        Next
        Return -1
    End Function

    Public Shared Function ResponseToStr(Response As HttpWebResponse) As String
        Dim sourceCode As String = ""
        Using sr As System.IO.StreamReader = New System.IO.StreamReader(Response.GetResponseStream())
            sourceCode = sr.ReadToEnd()
        End Using
        Return sourceCode
    End Function
End Class

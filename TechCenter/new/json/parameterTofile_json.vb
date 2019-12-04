Imports Newtonsoft.Json
Imports System.IO
Imports classLibrary_bang
Public Class parameterTofile_json(Of parameter_class)
    Public Parameters As parameter_class
    Public filename As String
    Public ErrlogFile As ErrlogFile
    Sub New(t_a As parameter_class, t_filename As String)
        filename = "p.txt"
        filename = t_filename
        Parameters = t_a
        ErrlogFile = New ErrlogFile("Errlog.txt", True)
    End Sub

    Sub init()
        onlyRead()
    End Sub
    Private Sub onlyRead()
        If System.IO.File.Exists(filename) Then
            load_p()
        Else
            save_p()
        End If
    End Sub

    Sub save_p()
        Dim text As String = JsonConvert.SerializeObject(Parameters)
        WritrFile(filename, text)
    End Sub
    Function load_p() As parameter_class
        If System.IO.File.Exists(filename) Then
            Dim text As String = System.IO.File.ReadAllText(filename)
            Parameters = JsonConvert.DeserializeObject(Of parameter_class)(text)
            Return Parameters

        End If
        Return Nothing
    End Function
    Private Sub WritrFile(t_filename As String, text As String)
        'MsgBox(t_filename + "W1")
        '清除檔案
        If System.IO.File.Exists(t_filename) Then
            System.IO.File.Delete(t_filename)

        End If
        '寫檔
        'MsgBox(t_filename + "W2")
        Dim sw As New StreamWriter(t_filename, True, System.Text.Encoding.Default)
        Try

            'For index As Integer = 0 To text_list.Count - 1

            '    sw.Write(text_list(index) & sw.NewLine)

            'Next
            sw.Write(text)
        Catch ex As Exception

            'MsgBox("寫檔失敗 " + t_filename)
            ErrlogFile.errWrite("寫檔失敗 ", ex)

        Finally
            sw.Close()
        End Try
    End Sub
    Private Sub WritrFile(t_filename As String, text_list As List(Of String))
        'MsgBox(t_filename + "W1")
        '清除檔案
        If System.IO.File.Exists(t_filename) Then
            System.IO.File.Delete(t_filename)

        End If
        '寫檔
        'MsgBox(t_filename + "W2")
        Dim sw As New StreamWriter(t_filename, True, System.Text.Encoding.Default)
        Try

            For index As Integer = 0 To text_list.Count - 1

                sw.Write(text_list(index) & sw.NewLine)

            Next
        Catch ex As Exception

            'MsgBox("寫檔失敗 " + t_filename)
            ErrlogFile.errWrite("寫檔失敗 ", ex)

        Finally
            sw.Close()
        End Try
    End Sub
End Class
Public Class test_parameter
    Public a As Integer
    Public b As Boolean
    Public c As String
    Sub New()
        a = 0
        b = True
        c = "C"
    End Sub
    Function text() As String
        Return "a=" + a.ToString + "b=" + b.ToString + "c=" + c.ToString
    End Function
End Class
Public Class parameterTofile_json_use
    Sub New()

    End Sub
    Sub run()
        Dim p As New test_parameter
        p.a = 11
        p.b = True
        p.c = "CC"
        Dim PTF_J As New parameterTofile_json(Of test_parameter)(p, "test2.txt")
        PTF_J.save_p()
        MsgBox(PTF_J.load_p().text)
    End Sub
End Class
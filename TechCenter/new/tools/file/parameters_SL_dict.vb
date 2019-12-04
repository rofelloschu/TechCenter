'20190528
Imports System.IO
Public Class parameters_SL_dict
    Implements IF_Parameter
    Protected m_Dictionary As Dictionary(Of String, String)
    Public split_word As String
    Protected Filepath As String

    '設定用
    Sub New(t_filename As String)
        Filepath = t_filename
        m_Dictionary = New Dictionary(Of String, String)
        Me.init()
    End Sub
    Protected Overridable Sub init()
        Me.split_word = "="

    End Sub
    Sub clsoe()
        m_Dictionary.Clear()
    End Sub
    Public Sub addParameters(tagname As String, value As String) Implements IF_Parameter.addParameters
        Dim t_value As String = ""
        If m_Dictionary.TryGetValue(tagname, t_value) Then

            m_Dictionary(tagname) = value
        Else
            m_Dictionary.Add(tagname, value)
        End If

    End Sub
    Public Function getValue(name As String) As String Implements IF_Parameter.getValue
        Dim t_value As String = ""
        If m_Dictionary.TryGetValue(name, t_value) Then
            Return t_value

        Else
            Return t_value
        End If

    End Function
    Public Sub readFile() Implements IF_Parameter.readFile
        If File.Exists(Me.Filepath) Then

        Else
            Me.saveFile()
            Exit Sub
        End If
        Try
            m_Dictionary.Clear()
            Dim ID_index As Integer = 0
            Using sr As StreamReader = New StreamReader(Me.Filepath, System.Text.Encoding.Default)
                Dim line As String = sr.ReadLine()
                While line IsNot Nothing
                    Try

                        'If line.StartsWith("#") Then
                        '    line = sr.ReadLine()
                        '    Continue While
                        'End If
                        'Console.WriteLine(line)
                        Dim str_url As String() = line.Split(split_word)


                        If str_url.Length >= 2 Then
                        Else
                            line = sr.ReadLine()
                            Continue While
                        End If

                        m_Dictionary.Add(str_url(0), str_url(1))
                        'Select Case str_url(0)
                        '    Case "Url_RiverString"
                        '        M_set_Parameters.Url_RiverString = str_url(1)
                        '    Case "sensor_ip"
                        '        M_set_Parameters.sensor_ip = str_url(1)
                        '    Case "sensor_port"
                        '        M_set_Parameters.sensor_port = str_url(1)
                        '    Case "RiverMaxHeight"
                        '        M_set_Parameters.RiverMaxHeight = str_url(1)
                        '    Case "deviceid"
                        '        M_set_Parameters.deviceid = str_url(1)
                        'End Select


                        line = sr.ReadLine()
                    Catch ex As Exception

                    End Try

                End While

            End Using
        Catch ex As Exception

        End Try
    End Sub
    Public Sub saveFile() Implements IF_Parameter.saveFile

        Dim text_list As New List(Of String)
        For Each kvp As KeyValuePair(Of String, String) In m_Dictionary
            text_list.Add(kvp.Key.ToString + split_word + kvp.Value.ToString)
        Next

        System.IO.File.WriteAllLines(Me.Filepath, text_list.ToArray, System.Text.Encoding.Default)

        text_list.Clear()
        GC.Collect()
    End Sub

    Public Function getKeyArray() As String()
        Dim keyColl As Dictionary(Of String, String).KeyCollection = m_Dictionary.Keys
        'keyColl.ToArray() 'test

        Dim keyList As New List(Of String)
        For Each s As String In keyColl
            ''Console.WriteLine("Key = {0}", s)
            keyList.Add(s)
        Next s
        Return keyList.ToArray
    End Function

    Protected Sub WritrFile(t_filename As String, text As String)
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
            'ErrlogFile.errWrite("寫檔失敗 ", ex)

        Finally
            sw.Close()
        End Try
    End Sub
    Protected Sub WritrFile(t_filename As String, text_list As List(Of String))
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
            'ErrlogFile.errWrite("寫檔失敗 ", ex)

        Finally
            sw.Close()
        End Try
    End Sub

End Class

Public Class parameters_SL_dict_use
    Sub New()

    End Sub
    Sub run()
        Dim parameters_SL As New parameters_SL_dict("aaa.txt")
        parameters_SL.split_word = "@"
        parameters_SL.addParameters("1", "http")
        parameters_SL.addParameters("12", "sun")
        parameters_SL.saveFile()

        parameters_SL.readFile()
        Console.WriteLine("count: " + parameters_SL.getKeyArray.Length.ToString())
    End Sub
End Class

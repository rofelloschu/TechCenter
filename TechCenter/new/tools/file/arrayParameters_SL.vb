'20180625
Imports System.IO
'讀陣列資料
Public Class arrayParameters_SL
    'Implements IF_Parameter
    Protected split_word As String
    Protected Filepath As String
    Protected item_count As Integer
    Private array_list As List(Of String())
    Sub New(t_filename As String, t_item_count As Integer)
        Filepath = t_filename
        item_count = t_item_count
        array_list = New List(Of String())
        Me.init()

    End Sub
    Protected Overridable Sub init()
        Me.split_word = ","

    End Sub


    Public Sub readFile() 'Implements IF_Parameter.readFile
        If File.Exists(Me.Filepath) Then

        Else
            Me.saveFile()
            Exit Sub
        End If
        Try
            'm_Dictionary.Clear()
            Dim ID_index As Integer = 0
            Using sr As StreamReader = New StreamReader(Me.Filepath, System.Text.Encoding.Default)
                Dim line As String = sr.ReadLine()
                While line IsNot Nothing
                    Try
                        Dim str_url As String() = line.Split(split_word)
                        If str_url.Length >= item_count Then
                            array_list.Add(str_url)
                        Else
                            line = sr.ReadLine()
                            Continue While
                        End If

                        'm_Dictionary.Add(str_url(0), str_url(1))
                        line = sr.ReadLine()
                    Catch ex As Exception

                    End Try

                End While

            End Using
        Catch ex As Exception

        End Try
    End Sub

    Public Sub saveFile() 'Implements IF_Parameter.saveFile
        Dim text_list As New List(Of String)
        For index As Integer = 0 To array_list.Count
            Dim text As String = ""
            Dim text_array As String = text_list(index)
            For index2 As Integer = 0 To item_count - 1
                If index2 = 0 Then
                    text = text_array(index2)
                Else
                    text = text + Me.split_word + text_array(index2)
                End If
            Next
            text_list.Add(text)
        Next
        'For Each kvp As KeyValuePair(Of String, String) In m_Dictionary
        '    text_list.Add(kvp.Key.ToString + split_word + kvp.Value.ToString)
        'Next

        System.IO.File.WriteAllLines(Me.Filepath, text_list.ToArray)

        text_list.Clear()
        GC.Collect()
    End Sub

    Public Function getarray_list() As List(Of String())
        Return array_list
    End Function

End Class

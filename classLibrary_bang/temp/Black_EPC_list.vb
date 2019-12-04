'20180307
Public Class Black_EPC_list
    Private EPC_Datalist As List(Of String)
    Private EPC_Timelist As List(Of DateTime)
    Private MaxCount As Integer
    Private RemoveTime As Integer
    Private AutoResetEvent As System.Threading.AutoResetEvent = New System.Threading.AutoResetEvent(True)

    Sub New(t_MaxCount As Integer, t_RemoveTime As Integer)
        Me.MaxCount = 3
        Me.RemoveTime = 0
        Me.MaxCount = t_MaxCount
        '0 不使用 單位 秒
        Me.RemoveTime = t_RemoveTime
        Me.EPC_Datalist = New List(Of String)
        Me.EPC_Timelist = New List(Of DateTime)
    End Sub
    '比對是否在名單內
    Function Compare(data As String) As Boolean
        '移除名單資料2
        AutoResetEvent.WaitOne()
        Dim exIndex As Integer
        Dim exback_index As Integer
        Try
            If Me.RemoveTime > 0 And EPC_Datalist.Count > 0 Then
                If Now > EPC_Timelist(0) Then
                    Dim listCount As Integer = EPC_Timelist.Count
                    For index As Integer = 0 To listCount - 1
                        Dim back_index As Integer = listCount - index - 1
                        exIndex = index
                        exback_index = back_index
                        If Now > EPC_Timelist(back_index) Then
                            'EPC_Datalist.RemoveAt(back_index)
                            'EPC_Timelist.RemoveAt(back_index)
                            Me.RemoveAtList(back_index)
                        End If
                    Next
                End If

            End If
        Catch ex As Exception

            'Console.WriteLine(ex.ToString)
            'Console.WriteLine("exIndex " + exIndex.ToString)
            'Console.WriteLine("exback_index " + exback_index.ToString)
            'Console.WriteLine("EPC_Timelist count " + EPC_Timelist.Count.ToString)
            'Console.WriteLine("EPC_Datalist count " + EPC_Datalist.Count.ToString)
            Me.CleartList()
        End Try

        Try
            If EPC_Datalist.Count = 0 Then
                Me.AddList(data)
                AutoResetEvent.Set()
                Return False
            Else
                For index As Integer = 0 To EPC_Datalist.Count - 1
                    'If data.(EPC_Datalist(index)) Then
                    '    AutoResetEvent.Set()
                    '    Return True
                    'End If
                    If iSTagEquals(data, EPC_Datalist(index)) Then
                        AutoResetEvent.Set()
                        Return True
                    End If
                Next
                Me.AddList(data)
                AutoResetEvent.Set()
                Return False
            End If
        Catch ex As Exception

        End Try

        AutoResetEvent.Set()
    End Function
    Private Sub AddList(Data As String)
        '   AutoResetEvent.WaitOne()
        ' Console.WriteLine(Now.ToString("u") + " add " + Data)
        Try
            EPC_Datalist.Add(Data)
            EPC_Timelist.Add(Now.AddSeconds(Me.RemoveTime))
            '移除名單資料1

            If EPC_Datalist.Count > Me.MaxCount Then
                '  Console.WriteLine(Now.ToString("u") + " remove 0 " + EPC_Datalist(0))
                EPC_Datalist.RemoveAt(0)
            End If
            If EPC_Timelist.Count > Me.MaxCount Then
                EPC_Timelist.RemoveAt(0)
            End If
        Catch ex As Exception
            ' Console.WriteLine(ex.ToString)
            Me.CleartList()
        End Try

        '  AutoResetEvent.Set()
    End Sub

    Private Sub RemoveAtList(index As Integer)
        Try
            '  Console.WriteLine(Now.ToString("u") + " remove " + EPC_Datalist(index))
            EPC_Datalist.RemoveAt(index)
            EPC_Timelist.RemoveAt(index)
        Catch ex As Exception
            '  Console.WriteLine(ex.ToString)
            Me.CleartList()
        End Try

    End Sub
    Private Sub CleartList()
        'Console.WriteLine(Now.ToString("u") + " clearList ")
        EPC_Datalist.Clear()
        EPC_Timelist.Clear()
    End Sub
    Private Function iSTagEquals(tag1 As String, tag2 As String) As Boolean
        Dim newTag1 As String = tag1.Split("T")(1)
        Dim newTag2 As String = tag2.Split("T")(1)
        Return newTag1.Equals(newTag2)
    End Function
End Class
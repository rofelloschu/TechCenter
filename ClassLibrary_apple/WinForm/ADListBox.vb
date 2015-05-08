'20120314
Namespace WinForm
    Class ADListBox

        Private ListBox As System.Windows.Forms.ListBox
        Private mList As IO.mutexList(Of String)
        Private fileName As String
        Sub New(ByVal ListBox As System.Windows.Forms.ListBox, ByVal fileName As String)
            Me.fileName = fileName
            mList = New IO.mutexList(Of String)
            Me.ListBox = ListBox
            readFile()

        End Sub
        Sub New(ByVal ListBox As System.Windows.Forms.ListBox, ByVal text As String())
            fileName = Nothing
            mList = New IO.mutexList(Of String)
            Me.ListBox = ListBox
            readFile(text)

        End Sub
        Function readFile() As Boolean
            Dim err As Boolean = True
            Try
                Using file As New OnlyReadFile(fileName)

                    ' mList.Add(file.load())
                    ListBox.Items.Clear()
                    ListBox.Items.AddRange(mList.ToArray)
                End Using

            Catch ex As Exception
                err = False
            End Try
            Return err
        End Function
        Function readFile(ByVal text As String()) As Boolean
            Dim err As Boolean = True
            Try
                'mList.Add(text)
                ListBox.Items.Clear()
                ListBox.Items.AddRange(mList.ToArray)

            Catch ex As Exception
                err = False
            End Try
            Return err
        End Function
        Sub close()
            mList.Clear()
        End Sub
        'Function writeFile() As Boolean

        'End Function
        Function toArray() As String()
            Dim SelectedItemsString(ListBox.SelectedIndices.Count - 1) As String
            For index As Integer = 0 To ListBox.SelectedIndices.Count - 1
                SelectedItemsString(index) = ListBox.SelectedItems.Item(index)
            Next

            Return SelectedItemsString
        End Function
    End Class
End Namespace

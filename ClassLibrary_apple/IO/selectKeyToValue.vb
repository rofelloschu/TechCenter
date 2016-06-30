'20160322
Namespace IO
    Public Class selectKeyToValue(Of inputType)
        Private StrToValueDict As Dictionary(Of String, inputType)
        Sub New()
            StrToValueDict = New Dictionary(Of String, inputType)
        End Sub
        Sub create(key As String, inputValue As inputType)
            StrToValueDict.Add(key, inputValue)
        End Sub
        Function getKeyArray() As String()
            Dim keyColl As Dictionary(Of String, inputType).KeyCollection = StrToValueDict.Keys
            'keyColl.ToArray() 'test

            Dim keyList As New List(Of String)
            For Each s As String In keyColl
                ''Console.WriteLine("Key = {0}", s)
                keyList.Add(s)
            Next s
            Return keyList.ToArray
        End Function
        Function getValueArray() As inputType()
            Dim ValueColl As Dictionary(Of String, inputType).ValueCollection = StrToValueDict.Values
            'keyColl.ToArray() 'test

            Dim ValueList As New List(Of inputType)
            For Each s As inputType In ValueColl
                ''Console.WriteLine("Key = {0}", s)
                ValueList.Add(s)
            Next s
            Return ValueList.ToArray
        End Function
        Public Function getValue(key As String) As inputType
            Dim Result As inputType

            If StrToValueDict.TryGetValue(key, Result) Then
                Return Result
            Else
                Return Nothing
            End If
        End Function
    End Class
End Namespace


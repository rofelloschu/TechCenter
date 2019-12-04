Module M_Dictionary_plus
    Function getKeyArray(dict As Dictionary(Of Object, Object)) As Object()
        Dim keyColl As Dictionary(Of Object, Object).KeyCollection = dict.Keys
        'keyColl.ToArray() 'test

        Dim keyList As New List(Of Object)
        For Each s As String In keyColl
            ''Console.WriteLine("Key = {0}", s)
            keyList.Add(s)
        Next s
        Return keyList.ToArray
    End Function
    Function getValueArray(dict As Dictionary(Of Object, Object)) As Object()
        Dim ValueColl As Dictionary(Of Object, Object).ValueCollection = dict.Values
        'keyColl.ToArray() 'test

        Dim ValueList As New List(Of Object)
        For Each s As Object In ValueColl
            ''Console.WriteLine("Key = {0}", s)
            ValueList.Add(s)
        Next s
        Return ValueList.ToArray
    End Function
End Module

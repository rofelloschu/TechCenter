Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
'20210910
Public Class json_seach
    Public Value As String
    Sub New(json_text As String)
        Value = json_text
        init()
    End Sub
    Public isDict As Boolean = False
    Public isList As Boolean = False
    Public isErr As Boolean = False
    Public Sub init()
        Dim r_object As Object = Nothing
        Try
            If Value.Substring(0, 1) = "[" Then

                r_object = JsonConvert.DeserializeObject(Of List(Of Object))(Value)

                isList = True
            End If
            If Value.Substring(0, 1) = "{" Then
                r_object = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(Value)
                isDict = True
            End If
        Catch ex As Exception
            isErr = True
        End Try
        'isErr = True
    End Sub
    'Public Function getObject(key As String) As Object
    '    Return JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(Value)(key)
    'End Function
    Public Function getObject(key As String) As json_seach
        Try
            Dim j_dict As Dictionary(Of String, Object) = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(Value)
            Dim r As New json_seach(j_dict(key).ToString)
            Return r
        Catch ex As Exception
            Return Nothing
        End Try

    End Function
    Public Function getObject(key As Integer) As json_seach
        Try
            Dim j_list As List(Of Object) = JsonConvert.DeserializeObject(Of List(Of Object))(Value)
            Dim r As New json_seach(j_list(key).ToString)
            Return r
        Catch ex As Exception
            Return Nothing
        End Try

    End Function
    Public Function getCount() As Integer
        Try
            Dim j_list As List(Of Object) = JsonConvert.DeserializeObject(Of List(Of Object))(Value)
            Return j_list.Count
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function getListarray() As json_seach()
        Try
            Dim j_list As List(Of Object) = JsonConvert.DeserializeObject(Of List(Of Object))(Value)
            Dim r2 As New List(Of json_seach)
            For Each t As Object In j_list
                r2.Add(New json_seach(t.ToString))
            Next
            'Dim r As New json_seach(j_list(key).ToString)
            Return r2.ToArray
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Private Function jsonToObject(text As String) As Object
        '[{"ID":"1","value":"100","time":"5"},{"ID":"7","value":["1","2","3"],"time":"5"}]
        Dim r_object As Object = Nothing
        Try
            If text.Substring(0, 1) = "[" Then

                r_object = JsonConvert.DeserializeObject(Of List(Of Object))(text)
                Return r_object
            End If
            If text.Substring(0, 1) = "{" Then
                r_object = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(text)
                Return r_object
            End If
        Catch ex As Exception

        End Try
        Return r_object
    End Function

    Public Function getValueBytes() As Byte()
        'Dim a As String = "[""" + Value + """]"
        'Dim b As Object = JsonConvert.DeserializeObject(Of List(Of Byte()))(a)
        'Return JsonConvert.DeserializeObject(Of List(Of Byte()))(a)(0)
        Dim a As String = """" + Value + """"
        Return JsonConvert.DeserializeObject(Of Byte())(a)
    End Function

    'Overrides Function Tostring() As String
    '    Return Value
    'End Function
    '20210719變短
    Public Function go(key As String) As json_seach
        Try
            Dim j_dict As Dictionary(Of String, Object) = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(Value)
            Dim r As New json_seach(j_dict(key).ToString)
            Return r
        Catch ex As Exception
            Return Nothing
        End Try

    End Function
    Public Function go(key As Integer) As json_seach
        Try
            Dim j_list As List(Of Object) = JsonConvert.DeserializeObject(Of List(Of Object))(Value)
            Dim r As New json_seach(j_list(key).ToString)
            Return r
        Catch ex As Exception
            Return Nothing
        End Try

    End Function
End Class
'20210719
Class use_json_seach
    Dim a As String = "[{""ID"":""1"",""value"":""100"",""time"":""5""},{""ID"":""7"",""value"":[""1"",""2"",""3""],""time"":""5""}]"
    Dim b As String = "{type:""event"",value:[{""ID"":""1"",""value"":""100"",""time"":""5""},{""ID"":""7"",""value"":[""1"",""2"",""3""],""time"":""5""}]}"
    Sub New()
        Dim a_json_seach As New json_seach(a)
        Console.WriteLine("a")
        Console.WriteLine(a_json_seach.getCount.ToString)
        Console.WriteLine(a_json_seach.getObject(0).Value)
        Console.WriteLine(a_json_seach.getObject(1).Value)
        Console.WriteLine(a_json_seach.getObject(0).getObject("ID").Value)
        Console.WriteLine(a_json_seach.go(0).go("value").Value)
        Console.WriteLine(a_json_seach.go(0).go("time").Value)
        Dim b_json_seach As New json_seach(b)
        Console.WriteLine("b")
        Console.WriteLine(b_json_seach.getCount.ToString)
        Console.WriteLine(b_json_seach.getObject("type").Value)
        Console.WriteLine(b_json_seach.getObject("value").Value)
        Console.WriteLine(b_json_seach.getObject("value").getCount.ToString)
        Console.WriteLine(b_json_seach.go("value").go(0).go("ID").Value)
        Console.WriteLine(b_json_seach.go("value").go(0).go("value").Value)
        Console.WriteLine(b_json_seach.go("value").go(0).go("time").Value)
    End Sub
End Class
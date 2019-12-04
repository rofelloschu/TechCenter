Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
'20190820
Public Class json_seach
    Public Value As String
    Sub New(json_text As String)
        Value = json_text

    End Sub
    'Public Function getObject(key As String) As Object
    '    Return JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(Value)(key)
    'End Function
    Public Function getObject(key As String) As json_seach
        Dim o As Dictionary(Of String, Object) = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(Value)
        Dim r As New json_seach(o(key).ToString)
        Return r
    End Function
    Public Function getObject(key As Integer) As json_seach
        Dim l As List(Of Object) = JsonConvert.DeserializeObject(Of List(Of Object))(Value)
        Dim r As New json_seach(l(key).ToString)
        Return r
    End Function
    Public Function getCount() As Integer
        Dim l As List(Of Object) = JsonConvert.DeserializeObject(Of List(Of Object))(Value)
        Return l.Count
    End Function
    'Overrides Function Tostring() As String
    '    Return Value
    'End Function
End Class
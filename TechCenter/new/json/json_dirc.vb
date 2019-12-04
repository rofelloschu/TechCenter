Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
'20190801
Class json_dict
    Public Value As String
    Sub New(json_text As String)
        Value = json_text
         
    End Sub
    'Public Function getObject(key As String) As Object
    '    Return JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(Value)(key)
    'End Function
    Public Function getjson_dict(key As String) As json_dict

        Dim r As New json_dict(JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(Value)(key).ToString)
        Return r
    End Function
    'Overrides Function Tostring() As String
    '    Return Value
    'End Function
End Class
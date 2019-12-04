'20180625
Public Interface IF_Parameter

    Sub addParameters(tagname As String, value As String)
    Sub readFile()
    Sub saveFile()
    Function getValue(name As String) As String
End Interface

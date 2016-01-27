<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Public Class ParameterArrayFile_undone
    Private TopNameList As List(Of String)
    Private ValueList As List(Of String)
    Private Path As String
    Private name As String
    Private data() As String
    Sub New(ByVal newPath As String, ByVal newName As String)
        data = Nothing
        setPath(newPath, newName)
        load()
        TopNameList = New List(Of String)
    End Sub
    'Sub readFile()

    'End Sub
    'Sub saveFile()
    '    Dim tempList As List(Of String)

    'End Sub
    '設定路徑檔名
    Public Sub setPath(ByVal filePath As String, ByVal fileName As String)
        Me.name = fileName
        Me.Path = filePath
    End Sub
    '讀取檔案內容放置data(string)
    Public Function load() As String()
        If System.IO.File.Exists(readPath) Then
            data = System.IO.File.ReadAllLines(Me.readPath)
            'data = read()
            Return data
        Else
            Return Nothing
        End If
    End Function
    '回傳完整路徑 string 
    Function readPath() As String
        Return Path + "\" + name
    End Function
    '將data儲存至檔案
    Public Sub save()
        isDataNull()
        System.IO.File.WriteAllLines(readPath, data)
    End Sub
    'data是否存在,不存在預設值"Null"
    Function isDataNull() As Boolean
        Dim isNull As Boolean = False
        If data Is Nothing Then
            Dim temp(0) As String
            temp(0) = "Null"
            data = temp
            isNull = True
        End If
        Return isNull
    End Function
#Region "TopName"
    Sub AddTopName(ByVal TopName As String)
        For index As Integer = 0 To TopNameList.Count - 1
            If TopNameList(index) = TopName Then
                Throw New Exception("TopName重複")
            End If
        Next
        TopNameList.Add(TopName)
    End Sub
    Function getTopName() As String()
        Return TopNameList.ToArray
    End Function
    Private Function getTopNameArrayValue() As String
        Dim text As String = ""
        For index As Integer = 0 To TopNameList.Count - 1
            If index = 0 Then
                text = TopNameList(0)
            Else
                text = text + "," + TopNameList(index)
            End If
        Next
        Return text
    End Function

    Function getTopNameIndex(ByVal TopName As String) As Integer
        For index As Integer = 0 To TopNameList.Count - 1
            If TopNameList(index) = TopName Then
                Return index
            End If
        Next
        Return -1
    End Function
#End Region
#Region "Value"

#End Region
    Function getNewLineArray() As String()
        Dim return_string(TopNameList.Count - 1) As String
        Return return_string
    End Function
    Function SetNewValue(ByVal value As String()) As Boolean
        If Not value.Length = TopNameList.Count Then
            Return False
        End If
        Dim line As String = ""
        For index As Integer = 0 To value.Length - 1
            If index = 0 Then
                line = value(0)
            Else
                line = line + "," + value(index)
            End If
        Next
        ValueList.Add(line)
        '待改
        Return True
    End Function

End Class

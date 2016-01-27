Imports System.IO

<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Class csvFile_undone
    Inherits APFile
    Private Table(,) As String
    Private TableHead() As String
    Private isHead As Boolean
    Private max_X As Integer
    Private max_Y As Integer
    'x橫向
    'y直向
    Sub New(ByVal newName As String, ByVal isHead As Boolean)
        MyBase.New(Directory.GetCurrentDirectory(), newName)

        Me.isHead = isHead
        ' Me.New(System.IO.Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly.GetName.CodeBase.ToString()), newName)
    End Sub

    Function readTable() As String(,)
        read()
        Dim tableIndex As Integer = 0
        If isHead Then
            TableHead = MyBase.getData(tableIndex).Split(",")
            tableIndex = tableIndex + 1
        End If
        Dim lines As String()
        max_Y = getData.Length - 1 - tableIndex
        max_X = getData(0).Split(",").Length - 1
        ReDim Table(max_X, max_Y)
        For index As Integer = tableIndex To getData.Length - 1
            'getData.CopyTo(TableHead, index, 0)
            lines = MyBase.getData(index).Split(",")

            For index2 As Integer = 0 To lines.Length - 1
                Table(index2, index - 1) = lines(index2)
            Next
        Next
        'max_Y = Table.GetLength(0)
        'max_X = Table.GetLength(1)
        Return getTable()
    End Function
    Function getTable() As String(,)
        Return Table
    End Function
    Function getTableHead() As String()
        Return TableHead
    End Function
End Class
'20150507
'ParameterFile_undone增加

'待改無需存取文字檔修改內容
'讀寫檔案
'暫時取代mutex
'專寫log檔 logFile class
'可讀可寫參數檔 ParameterFile class
'20140127增加logFile_undone
'20150507 修正警告
Imports System.IO
Imports System.Threading
'Namespace tools.file
<ObsoleteAttribute("DLL過時", False)> _
 Public Class APFile
    Implements IDisposable
    'Imports System.IO
    Private Path As String
    Private name As String
    Private data() As String
    ' Private output_mutex As Mutex = New Mutex(False)
    Private AutoResetEvent As AutoResetEvent = New AutoResetEvent(True)
#Region "New"
    '在程式同路徑下建立Parameter.txt
    Sub New()
        Me.New(Directory.GetCurrentDirectory(), "Parameter.txt")
        ' Me.New(System.IO.Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly.GetName.CodeBase.ToString()), "Parameter.txt")
    End Sub
    '輸入string路徑,string檔名依照輸入值建立
    Sub New(ByVal newPath As String, ByVal newName As String)
        data = Nothing
        setPath(newPath, newName)
        load()
    End Sub
    '輸入string檔名依照輸入值建立
    Sub New(ByVal newName As String)
        Me.New(Directory.GetCurrentDirectory(), newName)
        ' Me.New(System.IO.Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly.GetName.CodeBase.ToString()), newName)
    End Sub
#End Region

    '讀取檔案內容放置data(string)
    Public Function load() As String()
        If System.IO.File.Exists(readPath) Then
            data = System.IO.File.ReadAllLines(readPath)
            'data = read()
            Return data
        Else
            Return Nothing
        End If

    End Function
    '檔案是否存在
    Function Exists() As Boolean
        Return System.IO.File.Exists(readPath)
    End Function
    '輸入路徑檔案是否存在
    Function Exists(ByVal filePath As String) As Boolean
        Return System.IO.File.Exists(filePath)
    End Function

#Region "存檔"
    '將data儲存至檔案
    Public Sub save()
        'output_mutex.WaitOne()
        AutoResetEvent.WaitOne()
        isDataNull()
        System.IO.File.WriteAllLines(readPath, data)
        'For lineCount As Integer = 0 To data.Length - 1
        '    write(data(lineCount))
        'Next
        AutoResetEvent.Set()
        'output_mutex.ReleaseMutex()
    End Sub
    '輸入string檔名,將data儲存至檔案
    Public Sub save(ByVal fileName As String)
        isDataNull()
        System.IO.File.WriteAllLines((Path + "\" + fileName), data)
        'setName(fileName)
        'save()
    End Sub
    '輸入string()資料至data,將data儲存至檔案
    Public Sub save(ByVal data As String())
        setData(data)
        save()
    End Sub
    '輸入string檔名sting()資料至data,將data儲存至檔案
    Public Sub save(ByVal fileName As String, ByVal data As String())
        setData(data)
        save(fileName)
    End Sub
    '輸入string完整路徑,將data儲存至檔案
    Sub saveAs(ByVal allPath As String)
        setPathAndName(allPath)
        save()
    End Sub
#End Region
#Region "寫入(插入)"
    '輸入string,寫入檔案
    Public Sub write(ByVal text As String)
        'output_mutex.WaitOne()
        AutoResetEvent.WaitOne()
        Dim sw As New StreamWriter(readPath, True, System.Text.Encoding.Default)
        sw.Write(text & sw.NewLine)
        sw.Close()
        AutoResetEvent.Set()
        'output_mutex.ReleaseMutex()
    End Sub
    '輸入byte(),轉換成十六進位string寫入檔案
    Public Sub write(ByVal data() As Byte)
        'output_mutex.WaitOne()
        AutoResetEvent.WaitOne()
        Dim sw As New StreamWriter(readPath, True, System.Text.Encoding.Default)
        Dim text As String = Nothing
        For i As Integer = 0 To data.Length - 1
            text = text + data(i).ToString("X2")
        Next
        sw.Write(text & sw.NewLine)
        sw.Close()
        AutoResetEvent.Set()
        'output_mutex.ReleaseMutex()
    End Sub
    '?
    Public Sub write_Sp(ByVal data() As Byte)
        'output_mutex.WaitOne()
        AutoResetEvent.WaitOne()
        Dim sw As New StreamWriter(readPath, True, System.Text.Encoding.Default)
        Dim text As String = Nothing
        For i As Integer = 0 To data.Length - 1
            text = text + data(i).ToString("X2") + " "
        Next
        sw.Write(text & sw.NewLine)
        sw.Close()
        AutoResetEvent.Set()
        'output_mutex.ReleaseMutex()
    End Sub

    '輸入關鍵字string及要寫入值,將寫入值覆蓋關鍵字後面的值
    Function writeKey(ByVal keyword As String, ByVal writeText As String) As String
        'output_mutex.WaitOne()
        AutoResetEvent.WaitOne()
        Dim keyMessage As String = Nothing
        Try

            If keyword Is Nothing OrElse writeText Is Nothing Then
                Exit Try
            End If
            Dim keywordNumber As Integer
            read()
            For index As Integer = 0 To data.Length - 1
                keywordNumber = data(index).IndexOf(keyword)
                If keywordNumber = -1 Then
                Else
                    keyMessage = data(index).Substring(keywordNumber + keyword.Length)
                    data(index) = keyword + writeText
                    Exit For
                End If
            Next
            AutoResetEvent.Set()
            save()
            Return keyMessage
        Catch ex As Exception
            AutoResetEvent.Set()
            M_WriteLineMaster.WriteLine(ex.ToString)
            Return Nothing
        Finally
            ' AutoResetEvent.Set()
            'output_mutex.ReleaseMutex()
        End Try
        Return Nothing
    End Function
#End Region
#Region "讀出"
    '回傳讀到的內容string()
    Public Function read() As String()
        Return read(Nothing, Nothing)
    End Function
    '輸入開頭關鍵字string,回傳全部關鍵字之後的值
    Public Function read(ByVal keyword As String) As String()
        Return read(keyword, Nothing)
    End Function
    '輸入開頭關鍵字string結尾關鍵字string,回傳全部兩關鍵字之間的值
    Public Function read(ByVal keyword As String, ByVal endword As String) As String()
        Try
            Dim sr As New StreamReader(readPath, System.Text.Encoding.Default)
            Dim stringLineS As New List(Of String)
            Dim NextData As String
            Do
                '讀取一行字string
                NextData = sr.ReadLine()
                If NextData Is Nothing Then
                Else
                    stringLineS.Add(NextData)
                    Dim stringLine As String = stringLineS.Item(stringLineS.Count - 1)
                    If (keyword Is Nothing) OrElse (stringLine.IndexOf(keyword) = -1) Then
                        If (endword Is Nothing) OrElse (stringLine.IndexOf(endword) = -1) Then
                            stringLineS.Item(stringLineS.Count - 1) = stringLine.Substring(0)
                            If (keyword Is Nothing) Then
                            Else
                                stringLineS.RemoveAt(stringLineS.Count - 1)
                            End If
                        Else
                            Dim endAddr As Integer = stringLine.IndexOf(endword)
                            stringLineS.Item(stringLineS.Count - 1) = stringLine.Substring(0, endAddr)
                        End If
                        'Console.WriteLine(stringLineS.Item(stringLineS.Count - 1))
                    Else
                        Dim startAddr As Integer = stringLine.IndexOf(keyword) + keyword.Length
                        If (endword Is Nothing) OrElse (stringLine.IndexOf(endword) = -1) Then

                            stringLineS.Item(stringLineS.Count - 1) = stringLine.Substring(startAddr)
                        Else
                            Dim endAddr As Integer = stringLine.IndexOf(endword) - startAddr
                            stringLineS.Item(stringLineS.Count - 1) = stringLine.Substring(startAddr, endAddr)
                        End If


                        'Console.WriteLine(stringLineS.Item(stringLineS.Count - 1))
                    End If
                End If

            Loop Until NextData Is Nothing
            data = stringLineS.ToArray()
            sr.Close()
            GC.Collect()
            Return Me.data
        Catch ex As Exception
            M_WriteLineMaster.WriteLine(ex.ToString)
            Return Me.data
        Finally
        End Try
    End Function
    '輸入關鍵字string,回傳關鍵字之後的值(單一)
    Function readKey(ByVal keyword As String) As String

        'output_mutex.WaitOne()
        AutoResetEvent.WaitOne()
        Try
            Dim sr As New StreamReader(readPath, System.Text.Encoding.Default)
            Dim stringLineS As New List(Of String)
            Dim keyMessage As String = Nothing
            Do

                stringLineS.Add(sr.ReadLine())
                If stringLineS.Item(stringLineS.Count - 1) Is Nothing Then

                Else
                    If keyword Is Nothing Then

                        ' Console.WriteLine(stringLines.Item(stringLines.Count - 1))
                    Else
                        Dim stringLine As String = stringLineS.Item(stringLineS.Count - 1)
                        If stringLine.IndexOf(keyword) = -1 Then
                            'stringLines.Item(stringLines.Count - 1) = stringLine.Substring(stringLine.IndexOf(keyword) + keyword.Length)

                        Else
                            stringLineS.Item(stringLineS.Count - 1) = stringLine.Substring(stringLine.IndexOf(keyword) + keyword.Length)
                            keyMessage = stringLineS.Item(stringLineS.Count - 1)
                            '讀到關鍵字,跳出loop結束
                            Exit Do
                        End If
                    End If
                    'Console.WriteLine(stringLines.Item(stringLines.Count - 1))
                End If
            Loop Until stringLineS.Item(stringLineS.Count - 1) Is Nothing
            'data = stringLines.ToArray()
            sr.Close()
            Return keyMessage
        Catch ex As Exception
            M_WriteLineMaster.WriteLine(ex.ToString)
            Return Nothing
        Finally
            AutoResetEvent.Set()
            'output_mutex.ReleaseMutex()
        End Try

    End Function
    '    Function readArray() As Array

    '   End Function
#End Region
#Region "刪除"
    '刪除最後一行
    Sub deleteEndLine()
        load()
        Dim tempDate(data.Length - 2) As String
        For count As Integer = 0 To tempDate.Length - 1
            tempDate(count) = data(count)
        Next
        save(tempDate)
    End Sub
    '刪除keyword行
    Function deleteKeyLine(ByVal keywork As String) As Boolean
        Dim isdelete As Boolean = False
        Dim KeywordIndex As Integer
        Try
            KeywordIndex = Me.getKeywordIndex(keywork)
            If KeywordIndex = -1 Then
                isdelete = False
            Else
                load()
                Dim dataList As New List(Of String)
                For count As Integer = 0 To data.Length - 1
                    If Not count = KeywordIndex Then
                        dataList.Add(data(count))
                    Else
                        isdelete = True
                    End If
                Next
                save(dataList.ToArray)
            End If
            Return isdelete
        Catch ex As Exception
            Return False
        End Try
    End Function
    '刪除檔案
    Sub delete()
        If Exists() Then
            System.IO.File.Delete(readPath)
        End If

    End Sub
#End Region

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
    '取得keyword的索引
    Function getKeywordIndex(ByVal keyword As String) As Integer
        ' output_mutex.WaitOne()
        AutoResetEvent.WaitOne()
        Try
            Dim sr As New StreamReader(readPath, System.Text.Encoding.Default)
            Dim stringLineS As New List(Of String)
            Dim keywordIndex As Integer = -1
            Do

                stringLineS.Add(sr.ReadLine())
                If stringLineS.Item(stringLineS.Count - 1) Is Nothing Then

                Else
                    If keyword Is Nothing Then

                        ' Console.WriteLine(stringLines.Item(stringLines.Count - 1))
                    Else
                        Dim stringLine As String = stringLineS.Item(stringLineS.Count - 1)
                        If stringLine.IndexOf(keyword) = -1 Then
                            'stringLines.Item(stringLines.Count - 1) = stringLine.Substring(stringLine.IndexOf(keyword) + keyword.Length)

                        Else
                            stringLineS.Item(stringLineS.Count - 1) = stringLine.Substring(stringLine.IndexOf(keyword) + keyword.Length)
                            keywordIndex = stringLineS.Count - 1
                            '讀到關鍵字,跳出loop結束
                            Exit Do
                        End If
                    End If
                    'Console.WriteLine(stringLines.Item(stringLines.Count - 1))
                End If
            Loop Until stringLineS.Item(stringLineS.Count - 1) Is Nothing
            'data = stringLines.ToArray()
            sr.Close()
            Return keywordIndex
        Catch ex As Exception
            M_WriteLineMaster.WriteLine(ex.ToString)
            Return -1
        Finally
            AutoResetEvent.Set()
            'output_mutex.ReleaseMutex()
        End Try
    End Function
    '設定檔名
    Public Sub setName(ByVal fileName As String)
        Me.name = fileName
    End Sub
    '設定路徑檔名
    Public Sub setPath(ByVal filePath As String, ByVal fileName As String)
        Me.name = fileName
        Me.Path = filePath
    End Sub
    '回傳完整路徑 string 
    Function readPath() As String

        Return Path + "\" + name
    End Function
    '回傳data string()
    Function getData() As String()
        Return data
    End Function
    '設定data
    Sub setData(ByVal Data As String())
        Me.data = Data
    End Sub
    '???
    Function readValue(ByVal FilePath As String, ByVal ElementName As String) As String
        Try
            Dim sr As StreamReader = New StreamReader(FilePath, System.Text.Encoding.Default)
            Dim line As String = Nothing
            Do
                line = sr.ReadLine()
                If Not (line Is Nothing) Then
                    If line.Length > 0 Then
                        If line.Substring(0, line.IndexOf(":")) = ElementName Then
                            sr.Close()
                            Return line.Substring(line.IndexOf(":") + 1, line.Length - line.IndexOf(":") - 1)
                        End If
                    End If
                End If
            Loop Until line Is Nothing
            sr.Close()
        Catch ex As Exception
            Return Nothing
        End Try
        Return Nothing
    End Function

    '回傳檔名
    Function getName() As String
        Return name
    End Function
    '設定完整路徑
    Sub setPathAndName(ByVal allpath As String)

        Dim newPath() As String = allpath.Split("\")
        Me.name = newPath(newPath.Length - 1)
        Me.Path = allpath.ToString.Substring(0, allpath.Length - name.Length - 1)
    End Sub
    '解構
    Sub Dispose() Implements IDisposable.Dispose
        GC.SuppressFinalize(Me)
    End Sub

End Class
'<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
'<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _

'<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _


'讀csv檔表格


'End Namespace

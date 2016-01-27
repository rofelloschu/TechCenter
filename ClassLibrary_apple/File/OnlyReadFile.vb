Imports System.IO
Imports System.Threading
Class OnlyReadFile
    Implements IDisposable

    Private disposedValue As Boolean = False        ' 偵測多餘的呼叫
    Private data As String()
    Private name As String
    Private Path As String
    Private output_mutex As Mutex = New Mutex(False)
#Region "New"
    '輸入string路徑,string檔名依照輸入值建立
    Sub New(ByVal newPath As String, ByVal newName As String)
        data = Nothing
        setPath(newPath, newName)
        load()
    End Sub
    '輸入string檔名依照輸入值建立
    Sub New(ByVal newName As String)
        Me.New(Directory.GetCurrentDirectory(), newName)
    End Sub
    Sub New()

    End Sub
#End Region

    '讀取檔案內容放置data(string)
    Public Function load() As String()
        If System.IO.File.Exists(Me.readPath) Then

            data = System.IO.File.ReadAllLines(Me.readPath)
            'data = read()
            Return data
        Else
            Throw New Exception(readPath)
            Return Nothing
        End If

    End Function
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

        output_mutex.WaitOne()
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
            output_mutex.ReleaseMutex()
        End Try

    End Function
    '    Function readArray() As Array

    '   End Function
#End Region
    '設定路徑檔名
    Public Sub setPath(ByVal filePath As String, ByVal fileName As String)
        Me.name = fileName
        Me.Path = filePath
    End Sub

    '回傳完整路徑 string 
    Function readPath() As String

        Return Path + "\" + name
    End Function
    Sub close()

    End Sub
    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: 明確呼叫時釋放 Unmanaged 資源
            End If

            ' TODO: 釋放共用的 Unmanaged 資源
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' 由 Visual Basic 新增此程式碼以正確實作可處置的模式。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' 請勿變更此程式碼。在以上的 Dispose 置入清除程式碼 (ByVal 視為布林值處置)。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class

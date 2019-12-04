'20171002
'20180328閱
Imports System.IO
Imports System.Threading
Public Class logFile2
    Implements IDisposable

    Private isHaveTime As Boolean
    Private m_FileName As String
    Private m_Directory As String
    Private m_FileDate As DateTime

    Private Encoding As System.Text.Encoding
    Private m_FilePath As String

    Private errFile As ErrlogFile
    Private AutoResetEvent As AutoResetEvent = New AutoResetEvent(True)
    Sub New(ByVal t_fileNmae As String, ByVal t_isHaveTime As Boolean, Optional ByVal t_directory As String = "")
        '檔名
        Me.m_FileName = t_fileNmae
        '檔名加時間
        Me.isHaveTime = t_isHaveTime
        Me.Encoding = System.Text.Encoding.Default

        Me.m_FileMaxCount = 1

        '改儲存路徑 尾巴不加\
        If t_directory = "" Then
            Me.m_Directory = System.IO.Directory.GetCurrentDirectory
        Else

            Me.m_Directory = t_directory
            If Not System.IO.Directory.Exists(Me.m_Directory) Then
                System.IO.Directory.CreateDirectory(Me.m_Directory)
            End If
        End If
        Me.m_FileDate = Now
        If Me.isHaveTime Then
            Me.m_FilePath = m_Directory + "\" + m_FileName + "_" + Me.getFileNameTimeTag + ".txt"

        Else
            Me.m_FilePath = m_Directory + "\" + m_FileName + ".txt"
        End If

        errFile = New ErrlogFile(Me.m_FileName, t_isHaveTime, t_directory)
    End Sub
    '檔名時間
    Protected Function getFileNameTimeTag() As String
        Return m_FileDate.Year.ToString + "-" + m_FileDate.Month.ToString("D2") + "-" + m_FileDate.Day.ToString("D2")
    End Function
    '確認及修改檔名時間
    Protected Sub checktime()
        If Me.isHaveTime Then
            If Not m_FileDate.Day.Equals(Now.Day) Then
                m_FileDate = Now
                Me.m_FilePath = m_Directory + "\" + m_FileName + "_" + getFileNameTimeTag() + ".txt"

            End If
        Else
            Me.m_FilePath = m_Directory + "\" + m_FileName + ".txt"
        End If
    End Sub
    Sub Writte2(ByVal text As String)
        '寫入時間測試10,000行 12秒
        AutoResetEvent.WaitOne()
        System.IO.File.AppendAllText(Me.m_FilePath, text & vbCrLf)
        AutoResetEvent.Set()
    End Sub
    Sub Writte2_array(ByVal text As String())
        '寫入時間測試10,000行 0秒 需重測
        '寫入時間測試10,000,000行 4秒 檔案285MB 
        '不確定是否受硬碟寫入速度影響()
        AutoResetEvent.WaitOne()
        'System.IO.File.AppendAllLines(Me.m_FilePath, text)
        Dim temp As New List(Of String)
        If System.IO.File.Exists(Me.m_FilePath) Then
            temp.AddRange(System.IO.File.ReadAllLines(Me.m_FilePath))
        End If


        For index As Integer = 0 To text.Length - 1
            temp.Add(text(index))
        Next
        System.IO.File.WriteAllLines(Me.m_FilePath, temp.ToArray)
        AutoResetEvent.Set()
    End Sub
    Sub Writte(ByVal text As String)
        '寫入時間測試10,000行 9秒
        AutoResetEvent.WaitOne()
        Try
            Me.checktime()
            Dim sw As New StreamWriter(m_FilePath, True, Encoding)
            sw.Write(text & sw.NewLine)
            sw.Close()
        Catch ex As Exception

            Me.errFile.errWrite("[Writte] ", ex)

        Finally
            AutoResetEvent.Set()
            'M_catchException.exWritte(Now.ToString + " logFile_undone.Writte " + ex.ToString)

        End Try

        'AutoResetEvent.Set()
    End Sub
    Sub Writte_array(ByVal text() As String)
        '寫入時間測試10,000行 0秒 需重測
        '寫入時間測試10,000,000行 6秒 檔案285MB 
        '不確定是否受硬碟寫入速度影響()
        AutoResetEvent.WaitOne()
        Try
            Me.checktime()
            Dim sw As New StreamWriter(m_FilePath, True, Encoding)
            Dim temp As String = String.Join(sw.NewLine, text, 0, text.Length)
            sw.WriteLine(temp)
            sw.Close()
        Catch ex As Exception

            Me.errFile.errWrite("[Writte] ", ex)

        Finally
            AutoResetEvent.Set()
            'M_catchException.exWritte(Now.ToString + " logFile_undone.Writte " + ex.ToString)

        End Try

    End Sub
 
    'Sub time_Writte(ByVal t_time As DateTime, ByVal text As String)
    '    AutoResetEvent.WaitOne()
    '    Try
    '        Dim t_FilePath As String = m_Directory + "\" + FileName + "_" + t_time.Year.ToString + "-" + t_time.Month.ToString("D2") + "-" + t_time.Day.ToString("D2") + ".txt"
    '        Dim sw As New StreamWriter(t_FilePath, True, Encoding)
    '        sw.Write(text & sw.NewLine)
    '        sw.Close()
    '    Catch ex As Exception

    '        AutoResetEvent.Set()
    '        'M_catchException.exWritte(Now.ToString + " logFile_undone.time_Writte " + ex.ToString)
    '    End Try

    '    AutoResetEvent.Set()
    'End Sub


    '預計控制檔案數
    '重設資料數量 刪除多於資料
    Private m_FileMaxCount As Integer
    Property FileMaxCount() As Integer
        Set(ByVal value As Integer)
            ' 測試20,000,000行 7秒 完成
            '不確定是否受硬碟寫入速度影響()
            AutoResetEvent.WaitOne()

            Me.m_FileMaxCount = value

            Try
                '
                Dim sou_datastring As String()
                If System.IO.File.Exists(Me.m_FilePath) Then
                    sou_datastring = System.IO.File.ReadAllLines(Me.m_FilePath)

                    If sou_datastring.Length > Me.m_FileMaxCount Then
                        Console.WriteLine(Now.ToString("u") + " copy start")
                        Dim newMaxCount As Integer = Me.m_FileMaxCount \ 2
                        Dim des_datastring(newMaxCount - 1) As String
                        Dim sou_index As Integer = sou_datastring.Length - newMaxCount
                        Array.Copy(sou_datastring, sou_index, des_datastring, 0, newMaxCount)
                        '資料刪除
                        System.IO.File.Delete(Me.m_FilePath)
                        '資料重建
                        System.IO.File.WriteAllLines(Me.m_FilePath, des_datastring)
                        Console.WriteLine(Now.ToString("u") + " copy end")
                    End If

                End If


            Catch ex As Exception
            Finally
                AutoResetEvent.Set()

            End Try


        End Set
        Get
            Return Me.m_FileMaxCount
        End Get

    End Property
    Private disposedValue As Boolean = False        ' 偵測多餘的呼叫
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

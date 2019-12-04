'20180328
'待改 功能拆開
Imports System.IO
Imports System.Threading
Public Class OnlyWriteFile2
    Implements IDisposable
     

    Protected Encoding As System.Text.Encoding
    Protected errFile As ErrlogFile
    Protected AutoResetEvent As AutoResetEvent = New AutoResetEvent(True)
    Protected m_path As String
    Sub New(ByVal t_filepath As String)
        Me.m_path = t_filepath
        Me.Encoding = System.Text.Encoding.Default
    End Sub
    Sub setEncoding(t_Encoding As System.Text.Encoding)
        Me.Encoding = t_Encoding
    End Sub
#Region "寫入"
    Sub Writte2(ByVal text As String)
        '寫入時間測試10,000行 12秒
        AutoResetEvent.WaitOne()
        System.IO.File.AppendAllText(Me.m_path, text & vbCrLf)
        AutoResetEvent.Set()
    End Sub
    Sub Writte2_array(ByVal text As String())
        '寫入時間測試10,000行 0秒 需重測
        '寫入時間測試10,000,000行 4秒 檔案285MB 
        '不確定是否受硬碟寫入速度影響()
        AutoResetEvent.WaitOne()
        'System.IO.File.AppendAllLines(Me.m_FilePath, text)
        Dim temp As New List(Of String)
        If System.IO.File.Exists(Me.m_path) Then
            temp.AddRange(System.IO.File.ReadAllLines(Me.m_path))
        End If


        For index As Integer = 0 To text.Length - 1
            temp.Add(text(index))
        Next
        System.IO.File.WriteAllLines(Me.m_path, temp.ToArray)
        AutoResetEvent.Set()
    End Sub
    Sub Writte(ByVal text As String)
        '寫入時間測試10,000行 9秒
        AutoResetEvent.WaitOne()
        Try
            'Me.checktime()
            Dim sw As New StreamWriter(m_path, True, Encoding)
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
            'Me.checktime()
            Dim sw As New StreamWriter(m_path, True, Encoding)
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
    Sub cleanData()
        If System.IO.File.Exists(Me.m_path) Then

        End If
    End Sub
#End Region

#Region "複寫"
    Sub WriteAll(ByVal text() As String)
        AutoResetEvent.WaitOne()
        System.IO.File.WriteAllLines(Me.m_path, text)
        AutoResetEvent.Set()
    End Sub
#End Region
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
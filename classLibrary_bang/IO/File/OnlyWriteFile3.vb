'執行續版
'20211125
Imports System.IO
Imports System.Threading
'待改
Public Class OnlyWriteFile3

    Implements IDisposable


    Protected Encoding As System.Text.Encoding

    Protected AutoResetEvent As AutoResetEvent = New AutoResetEvent(True)
    Protected m_path As String
    Public temp_text_list As List(Of String)
    Protected t_auto_write As Thread
    Sub New(ByVal t_filepath As String)
        temp_text_list = New List(Of String)
        Me.m_path = t_filepath
        Me.Encoding = System.Text.Encoding.Default
        t_auto_write = New Thread(AddressOf auto_wrtie)
        t_auto_write.Start()
    End Sub
 
    Sub setEncoding(t_Encoding As System.Text.Encoding)

        Me.Encoding = t_Encoding
    End Sub
    Sub close()
        auto_wrtie_enable = False
    End Sub
    Public Property file_path() As String
        Get
            Return m_path
        End Get
        Set(ByVal value As String)
            m_path = value
        End Set
    End Property

#Region "寫入"
    'Sub Writte2(ByVal text As String)
    '    '寫入時間測試10,000行 12秒
    '    AutoResetEvent.WaitOne()
    '    System.IO.File.AppendAllText(Me.m_path, text & vbCrLf)
    '    AutoResetEvent.Set()
    'End Sub
    'Sub Writte2_array(ByVal text As String())
    '    '寫入時間測試10,000行 0秒 需重測
    '    '寫入時間測試10,000,000行 4秒 檔案285MB 
    '    '不確定是否受硬碟寫入速度影響()
    '    AutoResetEvent.WaitOne()
    '    'System.IO.File.AppendAllLines(Me.m_FilePath, text)
    '    Dim temp As New List(Of String)
    '    If System.IO.File.Exists(Me.m_path) Then
    '        temp.AddRange(System.IO.File.ReadAllLines(Me.m_path))
    '    End If


    '    For index As Integer = 0 To text.Length - 1
    '        temp.Add(text(index))
    '    Next
    '    System.IO.File.WriteAllLines(Me.m_path, temp.ToArray)
    '    AutoResetEvent.Set()
    'End 
    Sub Write(ByVal text As String)
        AutoResetEvent.WaitOne()
        temp_text_list.Add(text)
        AutoResetEvent.Set()
    End Sub
    Sub Write_array(ByVal text() As String)
        AutoResetEvent.WaitOne()
        temp_text_list.AddRange(text)
        AutoResetEvent.Set()
    End Sub
    Private auto_wrtie_enable As Boolean = True
    Protected Sub auto_wrtie()
        While auto_wrtie_enable
            Thread.Sleep(1)
            'While True
            If temp_text_list.Count > 0 Then

                AutoResetEvent.WaitOne()
                Try

                    Dim sw As New StreamWriter(m_path, True, Encoding)
                    Dim temp As String = String.Join(sw.NewLine, temp_text_list.ToArray, 0, temp_text_list.Count)
                    sw.WriteLine(temp)
                    sw.Close()
                    temp_text_list.Clear()
                Catch ex As Exception
                    Dim ex_text As String = ex.ToString + " temp clear"
                    Dim sw As New StreamWriter(m_path, True, Encoding)
                    sw.Write(ex_text & sw.NewLine)
                    sw.Close()

                    temp_text_list.Clear()
                End Try

                AutoResetEvent.Set()
            End If
            'End While
        End While
    End Sub
    Protected Sub Writte_t(ByVal text As String)

        Try
            'Me.checktime()
            Dim sw As New StreamWriter(m_path, True, Encoding)
            sw.Write(text & sw.NewLine)
            sw.Close()


        Catch ex As Exception

            'Me.errFile.errWrite("[Writte] ", ex)

        Finally

            'M_catchException.exWritte(Now.ToString + " logFile_undone.Writte " + ex.ToString)

        End Try

        'AutoResetEvent.Set()
    End Sub
    Protected Sub Writte_array_t(ByVal text() As String)

        Try
            'Me.checktime()
            Dim sw As New StreamWriter(m_path, True, Encoding)
            Dim temp As String = String.Join(sw.NewLine, text, 0, text.Length)
            sw.WriteLine(temp)
            sw.Close()
        Catch ex As Exception

            'Me.errFile.errWrite("[Writte] ", ex)

        Finally

            'M_catchException.exWritte(Now.ToString + " logFile_undone.Writte " + ex.ToString)

        End Try

    End Sub
     
     
#End Region

    '#Region "複寫"
    '    Sub WriteAll(ByVal text() As String)
    '        'AutoResetEvent.WaitOne()
    '        System.IO.File.WriteAllLines(Me.m_path, text)
    '        'AutoResetEvent.Set()
    '    End Sub
    '#End Region
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
'待改
Public Class use_OnlyWriteFile3
    Sub New()
        Dim OnlyWriteFile3 As New OnlyWriteFile3("tes_OnlyWriteFile3.txt")

    End Sub
End Class
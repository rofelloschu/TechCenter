Public Class FileReadAndWrite
    Implements IDisposable
    Private m_Path As String
    Sub New(ByVal t_Path As String)
        m_Path = t_Path
    End Sub
    Function readStringData() As String()
        Dim r_data() As String
        If System.IO.File.Exists(m_Path) Then
            r_data = System.IO.File.ReadAllLines(m_Path)
            'data = read()
            Return r_data
        Else
            Return Nothing
        End If

    End Function
    Function writeStringData(stringdata() As String) As Boolean
        'isDataNull()
        System.IO.File.WriteAllLines(m_Path, stringdata)
    End Function
    Function Exists() As Boolean
        Return System.IO.File.Exists(m_Path)
    End Function
 
#Region "IDisposable Support"
    Private disposedValue As Boolean ' 偵測多餘的呼叫

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO:  處置 Managed 狀態 (Managed 物件)。
            End If

            ' TODO:  釋放 Unmanaged 資源 (Unmanaged 物件) 並覆寫下面的 Finalize()。
            ' TODO:  將大型欄位設定為 null。
        End If
        Me.disposedValue = True
    End Sub

    ' TODO:  只有當上面的 Dispose(ByVal disposing As Boolean) 有可釋放 Unmanaged 資源的程式碼時，才覆寫 Finalize()。
    'Protected Overrides Sub Finalize()
    '    ' 請勿變更此程式碼。在上面的 Dispose(ByVal disposing As Boolean) 中輸入清除程式碼。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' 由 Visual Basic 新增此程式碼以正確實作可處置的模式。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' 請勿變更此程式碼。在以上的 Dispose 置入清除程式碼 (視為布林值處置)。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class

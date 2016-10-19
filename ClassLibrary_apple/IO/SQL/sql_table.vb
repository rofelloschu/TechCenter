Imports MySql.Data
Imports MySql.Data.MySqlClient
Public Class sql_table
    Implements IDisposable

    Private m_MySqlDataAdapter As MySqlDataAdapter
    Private m_DataSet As DataSet
    Private m_tableName As String = "temptable"
    Sub New(ByVal t_MySqlDataAdapter As MySqlDataAdapter)
        Try
            m_MySqlDataAdapter = t_MySqlDataAdapter
            m_DataSet = New DataSet
            m_MySqlDataAdapter.Fill(m_DataSet, m_tableName)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    Function RowsCount() As Integer
        Return m_DataSet.Tables(m_tableName).Rows.Count
    End Function
    Function Row_Item(ByVal Row_index As Integer, ByVal Column_name As String) As String
        Return m_DataSet.Tables(m_tableName).Rows(Row_index).Item(Column_name).ToString()
    End Function


    Private disposedValue As Boolean = False        ' 偵測多餘的呼叫

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: 明確呼叫時釋放 Managed 資源
            End If
            m_DataSet = Nothing
            m_MySqlDataAdapter = Nothing
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

'20130730
Imports System.Data
Public Class MyDataGridView
    Private mDataTable As System.Data.DataTable
    Private myDataGridView As System.Windows.Forms.DataGridView
    Sub New(ByVal tDataGridView As System.Windows.Forms.DataGridView, ByVal ColumnsHeadText() As String)
        If tDataGridView Is Nothing Then
            'Throw New Exception("DataGridView is nothing")
        End If
        mDataTable = New System.Data.DataTable
        If tDataGridView IsNot Nothing Then
            tDataGridView.DataSource = mDataTable
            myDataGridView = tDataGridView
            '設定標頭自動加寬
            For index As Integer = 0 To myDataGridView.Columns.Count - 1
                myDataGridView.Columns(index).AutoSizeMode = DataGridViewAutoSizeColumnsMode.AllCells

            Next
        End If


        Me.addHeadText(ColumnsHeadText)

        GC.Collect()
    End Sub
    Sub AutoSizeMode()
  
        For index As Integer = 0 To myDataGridView.Columns.Count - 1
            AutoSizeMode2(index, DataGridViewAutoSizeColumnsMode.Fill)
        Next
    End Sub
    Public Delegate Sub set_AutoSizeMode(index As Integer, id As Integer)
    Private Sub AutoSizeMode2(index As Integer, id As Integer)
        If Me.myDataGridView.FindForm.InvokeRequired Then
            Me.myDataGridView.FindForm.BeginInvoke(New set_AutoSizeMode(AddressOf Me.AutoSizeMode2), {index, id})
        Else
            myDataGridView.Columns(index).AutoSizeMode = id
        End If
    End Sub

    Sub New()
        mDataTable = New System.Data.DataTable
    End Sub

    Property data() As System.Data.DataTable
        Get
            Return Me.mDataTable
        End Get
        Set(ByVal value As System.Data.DataTable)
            Me.mDataTable = value
        End Set

    End Property
    Sub addHeadText(ByVal HeadText As String())
        mDataTable.Clear()
        For index As Integer = 0 To HeadText.Length - 1
            mDataTable.Columns.Add(HeadText(index))
        Next
    End Sub
    Function getColumnsCount() As Integer

        Return mDataTable.Columns.Count
    End Function
    Sub editData(ByVal edit_data_string As String(), ByVal row_index As Integer)
        Dim tData As System.Data.DataRow = mDataTable.NewRow

        For index As Integer = 0 To edit_data_string.Length - 1
            tData(index) = edit_data_string(index)
        Next
        Me.editData(tData, row_index)
    End Sub
    Public Delegate Sub editDataAction(ByVal c_data As System.Data.DataRow, ByVal row_index As Integer)
    Sub editData(ByVal c_row As System.Data.DataRow, ByVal row_index As Integer)
        If myDataGridView IsNot Nothing Then
            If Me.myDataGridView.FindForm.InvokeRequired Then
                Me.myDataGridView.FindForm.BeginInvoke(New editDataAction(AddressOf Me.editData), {c_row, row_index})
            Else
                For index As Integer = 0 To mDataTable.Rows(row_index).ItemArray.Length - 1
                    mDataTable.Rows(row_index)(index) = c_row(index)
                Next
            End If

        End If

    End Sub
    Sub addData(ByVal dataString() As String)
        Try

            Dim tData As System.Data.DataRow = mDataTable.NewRow

            For index As Integer = 0 To dataString.Length - 1

                tData(index) = dataString(index)
            Next

            Me.addData(tData)
        Catch ex As Exception

        End Try
    End Sub

    Sub addData(ByVal data As System.Data.DataRow)
        If myDataGridView IsNot Nothing Then
            If Me.myDataGridView.FindForm.InvokeRequired Then
                Me.myDataGridView.FindForm.BeginInvoke(New System.Action(Of System.Data.DataRow)(AddressOf Me.addData), data)
            Else
                mDataTable.Rows.Add(data)
            End If

        End If

    End Sub
    Function getrow() As System.Data.DataRow
        Return mDataTable.NewRow
    End Function
    Public Delegate Sub myAction()
    Sub clear()
        If myDataGridView IsNot Nothing Then
            If Me.myDataGridView.FindForm.InvokeRequired Then
                Me.myDataGridView.FindForm.BeginInvoke(New myAction(AddressOf Me.clear))
            Else
                mDataTable.Clear()
            End If
        End If
        Threading.Thread.Sleep(10) '延遲  字才會顯示 

    End Sub
End Class

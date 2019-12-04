Imports System.IO
Imports System.Threading
Public Class logFile_undone 
    Implements IDisposable, IF_logfile
    Private disposedValue As Boolean = False        ' 偵測多餘的呼叫
    Private isHaveTime As Boolean
    Private FileName As String
    Private Encoding As System.Text.Encoding
    Private m_FileMaxCount As Integer
    Private directory As String

    Private FilePath As String
    Private fileDate As DateTime

    Private AutoResetEvent As AutoResetEvent = New AutoResetEvent(True)
    Sub New(ByVal t_fileNmae As String, ByVal t_isHaveTime As Boolean, Optional ByVal t_directory As String = "")
        Me.FileName = t_fileNmae
        Me.isHaveTime = t_isHaveTime
        Me.Encoding = System.Text.Encoding.Default
        Me.m_FileMaxCount = 1


        If t_directory = "" Then
            Me.directory = System.IO.Directory.GetCurrentDirectory
        Else
            'If System.IO.Directory.Exists(t_directory) Then
            '    Me.directory = t_directory
            'Else
            '    Me.directory = System.IO.Directory.GetCurrentDirectory
            'End If
            Me.directory = t_directory
            If Not System.IO.Directory.Exists(Me.directory) Then
                System.IO.Directory.CreateDirectory(Me.directory)
            End If
        End If
        Me.fileDate = Now
        If Me.isHaveTime Then
            Me.FilePath = directory + "\" + FileName + "_" + fileDate.Year.ToString + "-" + fileDate.Month.ToString("D2") + "-" + fileDate.Day.ToString("D2") + ".txt"

        Else
            Me.FilePath = directory + "\" + FileName + ".txt"
        End If

        'Console.WriteLine("test")
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
    Public Function getDirectory() As String
        Return Me.directory
    End Function
    Public Function getPath() As String
        Return Me.FilePath
    End Function
    '預計控制檔案數
    Property FileMaxCount() As Integer
        Set(ByVal value As Integer)
            AutoResetEvent.WaitOne()

            Me.m_FileMaxCount = value

            Try
                '資料複製
                Dim sou_datastring As String() = System.IO.File.ReadAllLines(Me.FilePath)
                If sou_datastring.Length > Me.m_FileMaxCount Then
                    Dim newMaxCount As Integer = Me.m_FileMaxCount \ 2
                    Dim des_datastring(newMaxCount - 1) As String
                    Dim sou_index As Integer = sou_datastring.Length - newMaxCount
                    Array.Copy(sou_datastring, sou_index, des_datastring, 0, newMaxCount)
                    '資料刪除
                    System.IO.File.Delete(Me.FilePath)
                    '資料重建
                    System.IO.File.WriteAllLines(Me.FilePath, des_datastring)
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
    Private Sub checktime()
        If Me.isHaveTime Then
            If Not fileDate.Day.Equals(Now.Day) Then
                fileDate = Now
                Me.FilePath = directory + "\" + FileName + "_" + fileDate.Year.ToString + "-" + fileDate.Month.ToString("D2") + "-" + fileDate.Day.ToString("D2") + ".txt"

            End If
        Else
            Me.FilePath = directory + "\" + FileName + ".txt"
        End If
    End Sub


    Sub Writte(ByVal text As String) Implements IF_logfile.writeLine

        AutoResetEvent.WaitOne()
        Try
            Me.checktime()
            Dim sw As New StreamWriter(FilePath, True, Encoding)
            sw.Write(text & sw.NewLine)
            sw.Close()
        Catch ex As Exception

            AutoResetEvent.Set()
            M_catchException_APFile.exWritte(Now.ToString + " logFile_undone.Writte " + ex.ToString)
        End Try

        AutoResetEvent.Set()
    End Sub
    Sub time_Writte(ByVal t_time As DateTime, ByVal text As String)
        AutoResetEvent.WaitOne()
        Try
            Dim t_FilePath As String = directory + "\" + FileName + "_" + t_time.Year.ToString + "-" + t_time.Month.ToString("D2") + "-" + t_time.Day.ToString("D2") + ".txt"
            Dim sw As New StreamWriter(t_FilePath, True, Encoding)
            sw.Write(text & sw.NewLine)
            sw.Close()
        Catch ex As Exception

            AutoResetEvent.Set()
            M_catchException_APFile.exWritte(Now.ToString + " logFile_undone.time_Writte " + ex.ToString)
        End Try

        AutoResetEvent.Set()
    End Sub
End Class
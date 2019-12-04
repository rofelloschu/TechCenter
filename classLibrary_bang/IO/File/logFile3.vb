Public Class logFile3
    Inherits OnlyWriteFile2



    Private isHaveTime As Boolean
    Private m_FileName As String
    Private m_Directory As String
    Private m_FileDate As DateTime

    Private Encoding As System.Text.Encoding
    Private m_FilePath As String

    Private errFile As ErrlogFile
    Private AutoResetEvent As AutoResetEvent = New AutoResetEvent(True)
    Sub New(ByVal t_fileNmae As String, ByVal t_isHaveTime As Boolean, Optional ByVal t_directory As String = "")
        MyBase.New(t_fileNmae, t_isHaveTime)
    End Sub


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
End Class

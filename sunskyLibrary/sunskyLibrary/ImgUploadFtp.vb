'20140506 待測試
Imports Avi_freecam.Utilities.FTP
Imports System.Threading

'.暫時規定AVI圖檔一小時一次傳送至中心
'獨立執行
'將一個小時圖片按照檔按日期放進同個資料夾

'一個小時上傳一次資料夾圖片
'
Public Class ImgUploadFtp
    Public FTP_IP As String
    Public FTP_Port As String
    Public FTP_Username As String
    Public FTP_Password As String
    Private UpDirPath As String

    Private FTPclient As FTPclient_plus

    Private thread_HourUpload As Thread
    Private isStart_HourUploa As Boolean

    Private Image_DirectoryPath As String
    Private SmallImage_DirectoryPath As String

    Private thread_DeleteDataList As System.Threading.Timer
    Sub New()
        ''
        '========
        FTP_IP = M_FC_City3P.Ftp_Ip
        ' FTP_IP = "114.32.64.90"
        FTP_Port = "21"
        FTP_Username = M_FC_City3P.FTP_Username
        FTP_Password = M_FC_City3P.FTP_Password

        Image_DirectoryPath = M_FC_City3P.PlateTextImage_DirectoryPath
        SmallImage_DirectoryPath = M_FC_City3P.PlateTextSmallImage_DirectoryPath

        FTPclient = New FTPclient_plus(FTP_IP, FTP_Username, FTP_Password)
        isStart_HourUploa = True
        AddHandler FTPclient.ErrUploadFileName, AddressOf Me.recordErrUploadFile

        '資料夾建立
        UpDirPath = M_FC_City3P.FtpUpload_DirectoryPath
        If Not System.IO.Directory.Exists(UpDirPath) Then
            System.IO.Directory.CreateDirectory(UpDirPath)
        End If
        thread_HourUpload = New Thread(AddressOf Me.AddressOf_HourUpload)
        thread_HourUpload.Start()
        '定時刪檔
        Me.thread_DeleteDataList = New System.Threading.Timer(AddressOf AddressOf_deleteDataList, Nothing, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
        Me.thread_DeleteDataList.Change(100, 60000)
    End Sub
    Sub close()
        RemoveHandler FTPclient.ErrUploadFileName, AddressOf Me.recordErrUploadFile
        isStart_HourUploa = False
    End Sub
    Sub setIP_Username_Password(ByVal t_IP As String, ByVal t_Username As String, ByVal t_Password As String)
        Me.FTP_IP = t_IP
        Me.FTP_Username = t_Username
        Me.FTP_Password = t_Password


    End Sub
    Sub AddressOf_HourUpload()
        Dim HourDirectoryInfo As System.IO.DirectoryInfo
        While isStart_HourUploa
            Thread.Sleep(10)
            Try
                If Now.Second = 0 AndAlso Now.Minute = 0 Then
                    HourDirectoryInfo = Me.CreateHourDirectory(Now, Me.Image_DirectoryPath)
                    Me.ImgUploadFtp(HourDirectoryInfo.FullName, "Img_" + HourDirectoryInfo.Name)
                    HourDirectoryInfo = Me.CreateHourDirectory(Now, Me.SmallImage_DirectoryPath)
                    Me.ImgUploadFtp(HourDirectoryInfo.FullName, "SImg_" + HourDirectoryInfo.Name)

                    Thread.Sleep(1000)
                End If
                'If Now.Second = 0 Then
                '    Dim time As DateTime = Now
                '    Dim HourDirectoryName As String = time.Year.ToString("D4") + time.Month.ToString("D2") + time.Day.ToString("D2") + time.Hour.ToString("D2")
                '    Dim HourDirectoryInfo_test As System.IO.DirectoryInfo = System.IO.Directory.CreateDirectory(Me.SmallImage_DirectoryPath + "\" + HourDirectoryName)
                '    HourDirectoryInfo_test = Me.CreateHourDirectory(Now, Me.SmallImage_DirectoryPath)
                '    Me.ImgUploadFtp(HourDirectoryInfo_test.FullName, "SImg_" + HourDirectoryInfo_test.Name)
                'End If

            Catch ex As Exception

            End Try
        End While
    End Sub

    Sub ImgUploadFtp(ByVal DirectoryPth As String, ByVal targetDirectoryPathas As String)
        If M_FC_City3P.isUploadImg Then
            FTPclient.UploadDirectory(DirectoryPth, M_FC_City3P.Device_ID + "\" + targetDirectoryPathas + "\")
        End If

    End Sub
    Sub recordErrUploadFile(ByVal FileName As String)
        catchException.exWritte(Now + " " + "上傳失敗" + " " + FileName)
    End Sub
    Function CreateHourDirectory(ByVal time As DateTime, ByVal ImgDirectoryPath As String) As System.IO.DirectoryInfo
        Dim HourDirectoryName As String = time.Year.ToString("D4") + time.Month.ToString("D2") + time.Day.ToString("D2") + time.Hour.ToString("D2")
        Dim HourDirectoryInfo As System.IO.DirectoryInfo = System.IO.Directory.CreateDirectory(ImgDirectoryPath + "\" + HourDirectoryName)
        'Dim o_path As String = System.IO.Directory.GetCurrentDirectory + "\"
        Dim ImgDirectoryInfo As New System.IO.DirectoryInfo(ImgDirectoryPath)
        Dim ImgFiles() As System.IO.FileInfo = ImgDirectoryInfo.GetFiles()

        'Dim testDirectoryInfo As New System.IO.DirectoryInfo(System.IO.Directory.GetCurrentDirectory)
        'Dim testFile() As System.IO.FileInfo = testDirectoryInfo.GetFiles()
        'System.IO.File.Move(testFile(0).FullName, testDirectoryInfo.FullName + "\" + "test_" + testFile(0).Name)
        ''Console.WriteLine(testFile(0).Name)
        '檔案搬移
        Dim o_ImgFile As System.IO.FileInfo
        For index As Integer = 0 To ImgFiles.Length - 1
            o_ImgFile = ImgFiles(index)
            If o_ImgFile.CreationTime.Hour = time.AddHours(-1).Hour AndAlso o_ImgFile.CreationTime.Day = time.Day Then
                System.IO.File.Move(ImgFiles(index).FullName, HourDirectoryInfo.FullName + "\" + ImgFiles(index).Name)

            End If
        Next
        Return HourDirectoryInfo
    End Function


    '刪圖 預計一天刪一次
    Sub AddressOf_deleteDataList(ByVal stateInfo As Object)
        '設定seconde=30
        If Now.Second <> 30 Then
            Dim t_second As Integer = 60 - Now.Second + 30

            Me.thread_DeleteDataList.Change(t_second * 1000, 60000)
        End If
        If Now.Hour = 0 AndAlso Now.Minute = 0 Then

            Dim ImgDirectory() As String = System.IO.Directory.GetDirectories(Me.Image_DirectoryPath)
            Dim deleteTime As DateTime = Now.AddDays(-M_FC_City3P.PlateTextImage_SaveDay)
            For index As Integer = 0 To ImgDirectory.Length - 1
                If getHourDirectoryTime(ImgDirectory(index)) < deleteTime Then
                    System.IO.Directory.Delete(ImgDirectory(index), True)
                End If
            Next
            Dim SImgDirectory() As String = System.IO.Directory.GetDirectories(Me.SmallImage_DirectoryPath)
            deleteTime = Now.AddDays(-M_FC_City3P.PlateTextSmallImage_SaveDay)
            For index As Integer = 0 To SImgDirectory.Length - 1
                If getHourDirectoryTime(SImgDirectory(index)) < deleteTime Then
                    System.IO.Directory.Delete(SImgDirectory(index), True)
                End If
            Next
        End If

    End Sub
    Function getHourDirectoryTime(ByVal ImgDirectoryFullName As String) As DateTime
        Dim temp_str As String() = ImgDirectoryFullName.Split("\")
        Dim FileNmae As String = temp_str(temp_str.Length - 1)
        Dim year As Integer = CInt(FileNmae.Substring(0, 4))
        Dim month As Integer = CInt(FileNmae.Substring(4, 2))
        Dim day As Integer = CInt(FileNmae.Substring(6, 2))
        Dim hour As Integer = CInt(FileNmae.Substring(8, 2))
        Dim return_time As DateTime = New DateTime(year, month, day, hour, 0, 0)
        Return return_time
  
    End Function
End Class

'20150813

Imports System.Threading
Public Module M_LogManagement
    Private LogDir As String = System.IO.Directory.GetCurrentDirectory + "\log"
    Private FormLogName As String = "FormLog"
    Private AppStatusLogName As String = "AppLog"

    '紀錄操作
    Private FormLog As logFile_undone
    Private WriteLog_thread As Thread
    '紀錄與中心的通訊
    Private V3TCPServer_DictionaryLog As Dictionary(Of Integer, logFile_undone)
    '紀錄程式啟動結束
    Private AppStatusLog As logFile_undone
    ''紀錄協定
    'Private City3Log As logFile_undone

    Private WriteLog_ARE As AutoResetEvent = New AutoResetEvent(True)
    Sub New()
        V3TCPServer_DictionaryLog = New Dictionary(Of Integer, logFile_undone)
        If Not System.IO.Directory.Exists(LogDir) Then
            System.IO.Directory.CreateDirectory(LogDir)
        End If
        FormLog = New logFile_undone(FormLogName, True, LogDir)
        AppStatusLog = New logFile_undone(AppStatusLogName, True, LogDir)

    End Sub
    '通訊Server <->中心
    Sub addV3TCPServerLog(ByVal index As Integer, ByVal logFile As logFile_undone)
        If Not V3TCPServer_DictionaryLog.ContainsKey(index) Then
            V3TCPServer_DictionaryLog.Add(index, logFile)
        End If

    End Sub
    '紀錄按鍵
    Sub WriteFormLog(ByVal text As String)
        WriteLog_ARE.WaitOne()
        'If M_PrjParameter.isRecordLog Then
        WriteLog_thread = New Thread(AddressOf AddressOf_WriteFormLog)
        WriteLog_thread.Start(text)
        'End If
        WriteLog_ARE.Set()
    End Sub
    Sub AddressOf_WriteFormLog(ByVal text As Object)
        FormLog.Writte(Now.ToString + " " + text)
    End Sub
    '紀錄資料異常
    '資料異常開始
    '資料回復正常

    '程式運作紀錄
    Sub WriteAppStatusLog(ByVal text As String)
        WriteLog_ARE.WaitOne()
        'If M_PrjParameter.isRecordLog Then
        WriteLog_thread = New Thread(AddressOf AddressOf_WriteAppStatusLog)
        WriteLog_thread.Start(text)
        'End If
        WriteLog_ARE.Set()
    End Sub
    Sub AddressOf_WriteAppStatusLog(ByVal text As Object)
        AppStatusLog.Writte(text)
    End Sub
    Sub RecordProcessStartTime()
        Dim ProcessArray() As Process
        ProcessArray = Process.GetProcessesByName("VirtualTCSrv.vshost")
        If ProcessArray.Length > 0 Then
            WriteAppStatusLog("VirtualTCSrv.vshost程式啟動時間: " & ProcessArray(0).StartTime.ToString)
            Exit Sub
        End If
        ProcessArray = Process.GetProcessesByName("VirtualTCSrv")
        If ProcessArray.Length > 0 Then
            WriteAppStatusLog("VirtualTCSrv程式啟動時間: " & ProcessArray(0).StartTime.ToString)
            Exit Sub
        End If
    End Sub
    Sub RecordProcessEndTime()
        Dim ProcessArray() As Process
        ProcessArray = Process.GetProcessesByName("VirtualTCSrv.vshost")
        If ProcessArray.Length > 0 Then
            WriteAppStatusLog("VirtualTCSrv.vshost程式預估結束時間: " & Now.ToString)
            Exit Sub
        End If
        ProcessArray = Process.GetProcessesByName("VirtualTCSrv")
        If ProcessArray.Length > 0 Then
            WriteAppStatusLog("VirtualTCSrv程式預估結束時間: " & Now.ToString)
            Exit Sub
        End If
    End Sub
End Module

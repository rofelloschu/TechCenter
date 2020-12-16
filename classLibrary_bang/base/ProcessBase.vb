Imports System.Windows.Forms
'Imports System.Security.Permissions

'Imports System.Threading
'20150813
Public Class ProcessBase
    '避免被重覆執行
    Public Shared Sub ProcessOnly()

        If System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1 Then
            Dim myProces As System.Diagnostics.Process = System.Diagnostics.Process.GetCurrentProcess
            myProces.Kill()
        End If
    End Sub

    'https://msdn.microsoft.com/zh-tw/library/system.windows.forms.application.threadexception(v=vs.110).aspx
    '<SecurityPermission(SecurityAction.Demand, Flags:=SecurityPermissionFlag.ControlAppDomain)> _
    Public Shared Sub CatchApplicationException()
        ' Add the event handler for handling UI thread exceptions to the event.
        AddHandler Application.ThreadException, AddressOf ProcessBase.Form_UIThreadException
        ' Set the unhandled exception mode to force all Windows Forms errors to go through
        ' our handler.
        '  Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException)

        ' Add the event handler for handling non-UI thread exceptions to the event. 
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf ProcessBase.CurrentDomain_UnhandledException

        ' Runs the application.
        '  Application.Run(New ErrorHandlerForm())
    End Sub
    Private Shared logFileDir As String = System.IO.Directory.GetCurrentDirectory + "\appException\"
    Public Shared UIException_Write As New ExceptionWrite(logFileDir, "UIException")
    Shared Sub Form_UIThreadException(ByVal sender As Object, ByVal e As System.Threading.ThreadExceptionEventArgs)
        UIException_Write.writeLine("[sender]" + sender.ToString)
        UIException_Write.writeLine(e.Exception.ToString)
    End Sub
    Public Shared UnhandledException_Write As New ExceptionWrite(logFileDir, "UnhandledException")
    Shared Sub CurrentDomain_UnhandledException(ByVal sender As Object, ByVal e As System.UnhandledExceptionEventArgs)
        UnhandledException_Write.writeLine("[sender]" + sender.ToString)
        UnhandledException_Write.writeLine(e.ExceptionObject.ToString)
    End Sub

    '取得檔案版本
    Shared Function getVer() As System.Version

        Return System.Reflection.Assembly.GetExecutingAssembly.GetName.Version
    End Function


#Region "Msgbox自動關閉"
    Shared Sub MsgboxAndcloseMsg(ByVal text As String)
        closeMsg()
        MsgBox(text)
    End Sub
    Shared Sub closeMsg()
        Dim Thread_closeMsg As New Threading.Thread(AddressOf AddressOf_closeMsg)
        Dim proName As String
        Dim myAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly
        proName = myAssembly.FullName.Split(",")(0)
        Thread_closeMsg.Start(proName)
    End Sub
    Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
    Private Declare Function PostMessage Lib "user32" Alias "PostMessageA" (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Shared Sub AddressOf_closeMsg(ByVal text As Object)
        Threading.Thread.Sleep(5000)
        Dim hWnd As Integer
        hWnd = FindWindow(vbNullString, text.ToString)
        If hWnd Then
            PostMessage(hWnd, &H10, 0&, 0&)
        End If
    End Sub
#End Region

    
   
End Class


'M_UIException_Write


'轉INI.cs
'20181212
'https://www.pinvoke.net/default.aspx/kernel32.queryperformancecounter
'https://ithelp.ithome.com.tw/articles/10096533
'https://dotblogs.com.tw/chiajung/2009/11/05/11437
'https://ithelp.ithome.com.tw/articles/10096533
Imports System
Imports System.Runtime.InteropServices
Imports System.Text

'201801212
'20190729 增加使用方法 
Public Class INI
    Implements IDisposable

    Public Sub New(ByVal path As String)
        _FilePath = path
    End Sub
#Region "kernel32"
    '作用：指定Section, Key, Path,寫入Value
    <DllImport("kernel32")>
    Private Shared Function WritePrivateProfileString(ByVal section As String, ByVal key As String, ByVal val As String, ByVal filePath As String) As Long
        ' Leave the body of the function empty.
    End Function
    '作用：指定Section, Key, Path,讀取StringBuilder
    <DllImport("kernel32")>
    Private Shared Function GetPrivateProfileString(ByVal section As String, ByVal key As String, ByVal def As String, ByVal retVal As StringBuilder, ByVal size As Integer, ByVal filePath As String) As Integer
        ' Leave the body of the function empty.
    End Function
    '作用：指定Path, 讀取其中所有Section Name
    'uint > UInt32
    <DllImport("kernel32", EntryPoint:="GetPrivateProfileSectionNames", SetLastError:=True)>
    Private Shared Function GetPrivateProfileSectionNames(ByVal retVal As IntPtr, size As UInt32, filePath As String) As UInt32
        ' Leave the body of the function empty.
    End Function
    '作用：指定Section,路徑,取得Section內所有資料
    <DllImport("kernel32", EntryPoint:="GetPrivateProfileSection", SetLastError:=True)>
    Private Shared Function GetPrivateProfileSection(section As String, ByVal retVal As IntPtr, size As UInt32, filePath As String) As UInt32
        ' Leave the body of the function empty.
    End Function

    '作用：改寫整個Section
    <DllImport("kernel32")>
    Private Shared Function WritePrivateProfileSection(ByVal section As String, ByVal lpString As String, ByVal filePath As String) As Long
        ' Leave the body of the function empty.
    End Function
#End Region


    Private bDisposed As Boolean = False
    Private _FilePath As String = String.Empty

    Public Property FilePath As String
        Get

            If _FilePath Is Nothing Then
                Return String.Empty
            Else
                Return _FilePath
            End If
        End Get
        Set(ByVal value As String)
            If _FilePath <> value Then _FilePath = value
        End Set
    End Property


    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overridable Sub Dispose(ByVal IsDisposing As Boolean)
        If bDisposed Then
            Return
        End If

        If IsDisposing Then
        End If

        bDisposed = True
    End Sub


    Public Sub setKeyValue(ByVal IN_Section As String, ByVal IN_Key As String, ByVal IN_Value As String)
        WritePrivateProfileString(IN_Section, IN_Key, IN_Value, Me._FilePath)
    End Sub

    Public Function getKeyValue(ByVal IN_Section As String, ByVal IN_Key As String) As String
        Dim temp As StringBuilder = New StringBuilder(255)
        Dim i As Integer = GetPrivateProfileString(IN_Section, IN_Key, "", temp, 255, Me._FilePath)
        Return temp.ToString()
    End Function

    Public Function getKeyValue(ByVal Section As String, ByVal Key As String, ByVal DefaultValue As String) As String
        Dim sbResult As StringBuilder = Nothing

        Try
            sbResult = New StringBuilder(255)
            GetPrivateProfileString(Section, Key, "", sbResult, 255, Me._FilePath)
            'Return If((sbResult.Length > 0), sbResult.ToString(), DefaultValue)
            If (sbResult.Length > 0) Then
                Return sbResult.ToString()
            Else
                Return DefaultValue
            End If
        Catch
            Return String.Empty
        End Try
    End Function

    Function GetSectionName() As String()
        Dim MAX_BUFFER As UInt32 = 32767
        Dim pReturnedString As IntPtr = Marshal.AllocCoTaskMem(MAX_BUFFER)
        Dim bytesReturned As Integer = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, Me._FilePath)
        Return IntPtrToStringArray(pReturnedString, bytesReturned)
    End Function
    '未測
    Function GetSection(section As String) As String()
        Dim MAX_BUFFER As UInt32 = 32767
        Dim pReturnedString As IntPtr = Marshal.AllocCoTaskMem(MAX_BUFFER)
        Dim bytesReturned As Integer = GetPrivateProfileSection(section, pReturnedString, MAX_BUFFER, Me._FilePath)
        Return IntPtrToStringArray(pReturnedString, bytesReturned)
    End Function
    Private Function IntPtrToStringArray(pReturnedString As IntPtr, bytesReturned As UInt32) As String()
        If bytesReturned = 0 Then
            Marshal.FreeCoTaskMem(pReturnedString)
            Return Nothing
        End If
        Dim local As String = Marshal.PtrToStringAnsi(pReturnedString, bytesReturned).ToString
        Marshal.FreeCoTaskMem(pReturnedString)
        Return local.Substring(0, local.Length - 1).Split("\0")
    End Function
    Public Shared Sub example()
        ' System.IO.Directory.GetCurrentDirectory()
        Console.WriteLine(Application.StartupPath)
        Using oTINI As New INI(System.IO.Path.Combine(Application.StartupPath, "config.ini"))
            oTINI.setKeyValue("test", "a01", "aa")
            oTINI.setKeyValue("CONFIG", "Folder", "a1")
            oTINI.setKeyValue("CONFIG", "Days", "a2")
            oTINI.setKeyValue("CONFIG", "Hour", "a3")

        End Using

        Using oTINI As New INI(System.IO.Path.Combine(Application.StartupPath, "config.ini"))
            ''//查看的資料夾
            '    Folder = oTINI.getKeyValue("CONFIG", "Folder");
            Console.WriteLine("CONFIG.Folder " + oTINI.getKeyValue("CONFIG", "Folder"))
            ''//要留下的天數
            '    DateLong = int.Parse(oTINI.getKeyValue("CONFIG", "Days"));
            Console.WriteLine("CONFIG.Days " + oTINI.getKeyValue("CONFIG", "Days"))
            ''//幾點開始移除
            '    starthour = oTINI.getKeyValue("CONFIG", "Hour");
            Console.WriteLine("CONFIG.Hour " + oTINI.getKeyValue("CONFIG", "Hour"))

            Console.WriteLine("test " + oTINI.getKeyValue("test", "a01"))
            Console.WriteLine("GetSection " + oTINI.GetSectionName.Count.ToString)
        End Using


    End Sub
End Class
Public Class use_INI
    Sub New()
        Using oTINI As New INI(System.IO.Path.Combine(Application.StartupPath, "config.ini"))
            oTINI.setKeyValue("test", "a01", "aa")
            oTINI.setKeyValue("CONFIG", "Folder", "a1")
            oTINI.setKeyValue("CONFIG", "Days", "a2")
            oTINI.setKeyValue("CONFIG", "Hour", "a3")
        End Using

        Using oTINI As New INI(System.IO.Path.Combine(Application.StartupPath, "config.ini"))
            ''//查看的資料夾
            '    Folder = oTINI.getKeyValue("CONFIG", "Folder");
            Console.WriteLine("CONFIG.Folder " + oTINI.getKeyValue("CONFIG", "Folder"))
            ''//要留下的天數
            '    DateLong = int.Parse(oTINI.getKeyValue("CONFIG", "Days"));
            Console.WriteLine("CONFIG.Days " + oTINI.getKeyValue("CONFIG", "Days"))
            ''//幾點開始移除
            '    starthour = oTINI.getKeyValue("CONFIG", "Hour");
            Console.WriteLine("CONFIG.Hour " + oTINI.getKeyValue("CONFIG", "Hour"))

            Console.WriteLine("test " + oTINI.getKeyValue("test", "a01"))
        End Using

    End Sub
End Class

Imports System.Runtime.InteropServices

Public Class cFTP

    ' Internet Session Handle (InternetOpen)
    Private _hInternet As Long
    ' Internet Service Handle (InternetConnect)
    Public _hConnect As Long

    Private _ServerName As String = "www.sunsky.com.tw"
    Private _UserName As String = "sunsky"
    Private _Password As String = "22761931"
    ''儲存是否需要密碼驗證的狀態
    Private _IsNeedPassword As Boolean = True
    ''儲存是否需要via Proxy
    Private _IsNeedProxy As Boolean = False
    ''儲存傳輸模式(ASCII or Binary (default to Binary)
    Private _TransferMode As Long = INTERNET_FLAG_TRANSFER_BINARY

    Public Sub New()

    End Sub

#Region "Const"
    ' dwAccessType: 設定Internet連結的存取方式
    Private Const INTERNET_OPEN_TYPE_PRECONFIG As Short = 0
    Private Const INTERNET_OPEN_TYPE_DIRECT As Short = 1
    Private Const INTERNET_OPEN_TYPE_PROXY As Short = 3
    Private Const INTERNET_OPEN_TYPE_PRECONFIG_WITH_NO_AUTOPROXY As Short = 4

    ' dwFlags
    Private Const INTERNET_FLAG_FROM_CACHE As Integer = &H1000000
    Private Const INTERNET_FLAG_OFFLINE As Integer = &H1000000
    Private Const INTERNET_FLAG_MAKE_PERSISTENT As Integer = &H2000000
    Private Const INTERNET_FLAG_NO_CACHE_WRITE As Integer = &H4000000
    Private Const INTERNET_FLAG_ASYNC As Integer = &H10000000
    Private Const INTERNET_FLAG_EXISTING_CONNECT As Integer = &H20000000
    Private Const INTERNET_FLAG_PASSIVE As Integer = &H8000000

    ' nServerPort：設定Internet服務之通訊埠（port）
    Private Const INTERNET_DEFAULT_FTP_PORT As Short = 21
    Private Const INTERNET_DEFAULT_GOPHER_PORT As Short = 70
    Private Const INTERNET_DEFAULT_HTTP_PORT As Short = 80
    Private Const INTERNET_DEFAULT_HTTPS_PORT As Short = 443
    Private Const INTERNET_DEFAULT_SOCKS_PORT As Short = 1080
    Private Const INTERNET_INVALID_PORT_NUMBER As Short = 0

    ' dwService：設定Internet服務類型
    Private Const INTERNET_SERVICE_FTP As Short = 1
    Private Const INTERNET_SERVICE_GOPHER As Short = 2
    Private Const INTERNET_SERVICE_HTTP As Short = 3

    ' dwInternetFlags
    Private Const FTP_TRANSFER_TYPE_ASCII As Short = &H1S ' ASCII 模式
    Private Const FTP_TRANSFER_TYPE_BINARY As Short = &H2S ' Binary模式
    Private Const FTP_TRANSFER_TYPE_UNKNOWN As Short = &H0S ' 預設為Binary模式
    Private Const INTERNET_FLAG_TRANSFER_ASCII As Short = &H1S ' ASCII 模式
    Private Const INTERNET_FLAG_TRANSFER_BINARY As Short = &H2S ' Binary模式

    Private Const FILE_ATTRIBUTE_DIRECTORY As Short = &H10S
    Private Const FILE_ATTRIBUTE_ARCHIVE As Short = &H20S
#End Region

#Region "Win32API"


    <DllImport("wininet.dll", EntryPoint:="InternetOpenA")> _
    Private Shared Function InternetOpen( _
        ByVal lpszAgent As String, ByVal dwAccessType As Integer, _
        ByVal lpszProxyName As String, ByVal lpszProxyBypass As String, _
        ByVal dwFlags As Integer) As Integer
    End Function


    <DllImport("wininet.dll", EntryPoint:="InternetConnectA")> _
    Private Shared Function InternetConnect(ByVal hInternetSession As Integer, _
        ByVal lpszServerName As String, ByVal nServerPort As Short, _
        ByVal lpszUsername As String, ByVal lpszPassword As String, _
        ByVal dwService As Integer, ByVal dwFlags As Integer, _
        ByVal dwContext As Integer) As Integer
    End Function

    <DllImport("wininet.dll", EntryPoint:="InternetCloseHandle")> _
    Private Shared Function InternetCloseHandle(ByVal hInternet As Integer) As Boolean
    End Function

    <DllImport("wininet.dll", EntryPoint:="FtpGetCurrentDirectoryA")> _
    Private Shared Function FtpGetCurrentDirectory(ByVal hFtpSession As Integer, _
        ByVal lpszCurrentDirectory As String, _
        ByRef lpdwCurrentDirectory As Integer) As Boolean
    End Function

    <DllImport("wininet.dll", EntryPoint:="FtpSetCurrentDirectoryA")> _
    Private Shared Function FtpSetCurrentDirectory(ByVal hConnect As Integer, _
        ByVal lpszDirectory As String) As Boolean
    End Function

    <DllImport("wininet.dll", EntryPoint:="FtpGetFileA")> _
    Private Shared Function FtpGetFile(ByVal hFtpSession As Integer, _
        ByVal lpszRemoteFile As String, ByVal lpszNewFile As String, _
        ByVal fFailIfExists As Boolean, ByVal dwLocalFlagsAndAttributes As Integer, _
        ByVal dwInternetFlags As Integer, ByVal dwContext As Integer) As Boolean
    End Function

    <DllImport("wininet.dll", EntryPoint:="FtpPutFileA")> _
    Private Shared Function FtpPutFile(ByVal hFtpSession As Integer, _
        ByVal lpszLocalFile As String, ByVal lpszNewRemoteFile As String, _
        ByVal dwInternetFlags As Integer, ByVal dwContext As Integer) As Boolean
    End Function

    <DllImport("wininet.dll", EntryPoint:="FtpCreateDirectory")> _
    Private Shared Function FtpCreateDirectory(ByVal hConnect As Integer, _
        ByVal lpszDirectory As String) As Boolean
    End Function

    <DllImport("wininet.dll", EntryPoint:="FtpRemoveDirectory")> _
    Private Shared Function FtpRemoveDirectory(ByVal hConnect As Integer, _
        ByVal lpszDirectory As String) As Boolean
    End Function
#End Region

#Region "Property"
    ''設定遠端主機名稱
    Public Property ServerName() As String
        Get
            Return _ServerName
        End Get
        Set(ByVal Value As String)
            _ServerName = Value
        End Set
    End Property

    ''設定使用者名稱
    Public Property UserName() As String
        Get
            Return _UserName
        End Get
        Set(ByVal Value As String)
            _UserName = Value
        End Set
    End Property

    ''設定密碼
    Public Property Password() As String
        Get
            Return _Password
        End Get
        Set(ByVal Value As String)
            _Password = Value
        End Set
    End Property

    ''設定連線時是否需要驗證密碼
    Public Property NeedPassword() As Boolean
        Get
            Return _IsNeedPassword
        End Get
        Set(ByVal Value As Boolean)
            _IsNeedPassword = Value
        End Set
    End Property

    ''設定是否需要 Proxy
    Public Property NeedProxy() As Boolean
        Get
            Return _IsNeedProxy
        End Get
        Set(ByVal Value As Boolean)
            _IsNeedProxy = Value
        End Set
    End Property

    ''設定傳輸模式,1->ASCII 2->Binary
    Public Property TransMode() As Integer
        Get
            Return _TransferMode
        End Get
        Set(ByVal Value As Integer)
            _TransferMode = Value
        End Set
    End Property
#End Region

#Region "Function"
    ''連線
    Public Function Connect() As Boolean
        ' INTERNET_FLAG_NO_CACHE_WRITE = &H04000000
        ' {不將Internet連結之資訊寫入Cache中}

        If _IsNeedProxy Then
            _hInternet = InternetOpen( _
            "FTP Application", INTERNET_OPEN_TYPE_PROXY, _
            Nothing, Nothing, INTERNET_FLAG_NO_CACHE_WRITE)
        Else
            _hInternet = InternetOpen( _
            "FTP Application", INTERNET_OPEN_TYPE_DIRECT, _
            Nothing, Nothing, INTERNET_FLAG_NO_CACHE_WRITE)
        End If

        ' If Internet Session Handle created OK
        If (_hInternet <> 0) Then
            ' FTP Service Handle
            If _IsNeedPassword Then
                _hConnect = InternetConnect( _
                                _hInternet, _ServerName, INTERNET_DEFAULT_FTP_PORT, _
                                _UserName, _Password, INTERNET_SERVICE_FTP, _
                                INTERNET_FLAG_NO_CACHE_WRITE, 0)
            Else
                ' anonymous
                _hConnect = InternetConnect( _
                                _hInternet, _ServerName, INTERNET_DEFAULT_FTP_PORT, _
                                Nothing, Nothing, INTERNET_SERVICE_FTP, _
                                INTERNET_FLAG_EXISTING_CONNECT Or INTERNET_FLAG_PASSIVE, 0)
            End If

        Else
            ''錯誤處理
            Return False
        End If

        Return True
    End Function

    ''關閉連線
    Public Function DisConnect() As Boolean
        ' close internet service connection
        If (_hConnect <> 0) Then
            InternetCloseHandle(_hConnect)
            _hConnect = 0
        End If

        ' close internet session
        If (_hInternet <> 0) Then
            InternetCloseHandle(_hInternet)
            _hInternet = 0
        End If
        Return True
    End Function

    ''變更FTP目錄
    Public Function SetCurrentDir(ByVal DirectoryName As String) As Boolean
        Return FtpSetCurrentDirectory(_hConnect, DirectoryName)
    End Function

    ''傳送檔案
    Public Function PutFile(ByVal LocalFile As String, ByVal RemoteFile As String) As Boolean
        'Dim dwInternetFlags As Integer
        Return FtpPutFile(_hConnect, LocalFile, RemoteFile, _TransferMode, 0)
    End Function

    ''下載檔案
    Public Function GetFile(ByVal LocalFile As String, ByVal RemoteFile As String) As Boolean
        'Dim dwInternetFlags As Integer
        Return FtpGetFile(_hConnect, RemoteFile, LocalFile, _
                            False, FILE_ATTRIBUTE_ARCHIVE, _TransferMode, 0)
    End Function

    ''建立新目錄
    Public Function CreateDirectory(ByVal DirName As String) As Boolean
        Return FtpCreateDirectory(_hConnect, DirName)
    End Function

    ''刪除目錄
    ''當資料夾內有檔案存在時刪除會失敗傳回False
    Public Function RemoveDirectory(ByVal DirName As String) As Boolean
        Return FtpRemoveDirectory(_hConnect, DirName)
    End Function

    ''取得FTP Server目前目錄
    Public Function GetFTPCurrentPath(ByRef hConnect As Long) As String
        Dim sDir As String
        Dim bReturn As Boolean

        sDir = New String(Chr(0), 1024)

        ' FtpGetCurrentDirectory
        ' Retrieves the current directory for the connection.
        bReturn = FtpGetCurrentDirectory(hConnect, sDir, Len(sDir))

        ' returns True if successful
        If bReturn Then
            Return sDir.Substring(0, sDir.Length - 1)
        Else
            Return ""
        End If
    End Function
#End Region

End Class
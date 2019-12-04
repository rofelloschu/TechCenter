'20150312
Imports MySql.Data.MySqlClient
Public Class useSqlDatabase2

    'Protected mysql_ip As String
    'Protected mysql_port As String
    'Protected mysql_username As String
    'Protected mysql_password As String
    'Protected mysql_database As String
    'Protected mysql_table As String
    Protected m_MySqlConnection As MySqlConnection
    '  Private m_sqlCommand As New MySqlCommand
    '    Private AutoResetEvent As Threading.AutoResetEvent = New Threading.AutoResetEvent(True)
    Private Parameters As Parameters_usesqldatabase
    'Sub New()

    '    Me.Parameters.mysql_ip = "192.168.1.30"
    '    Me.Parameters.mysql_port = "3306"
    '    Me.Parameters.mysql_username = "tDV"
    '    Me.Parameters.mysql_password = "tdv1234"
    '    Me.Parameters.mysql_database = "tdvdb"
    '    Me.Parameters.mysql_table = "tdvevent"
    'End Sub
    Private m_isConnect As Boolean
    Private auto_Connect As Threading.Thread
    Sub New(ByVal t_Parameters As Parameters_usesqldatabase)

        Me.Parameters = t_Parameters
        '   Me.Parameters.mysql_table = "tdvevent" '無作用
        m_isConnect = False
    End Sub
    Function testconnect() As Boolean
        Dim results As Boolean = False
        Try
            Dim sConnStr As String = String.Format("Server={0};database={1};User Id={2};Password={3};pooling=false;Connection Timeout=3;Port={4};", _
                                                   Me.Parameters.mysql_ip, Me.Parameters.mysql_database, Me.Parameters.mysql_username, Me.Parameters.mysql_password, Me.Parameters.mysql_port)
            Me.m_MySqlConnection = New MySqlConnection(sConnStr)
            Console.WriteLine(sConnStr)
            Me.m_MySqlConnection.Open()


        Catch ex As Exception
            Me.m_MySqlConnection.Close()
            Return False
        Finally

        End Try
        Me.m_MySqlConnection.Close()
        Return True

        Return results
    End Function

    ReadOnly Property isConnect() As Boolean
        Get
            Return m_isConnect
        End Get

    End Property
    Sub start_auto_Connect()
        auto_Connect = New Threading.Thread(AddressOf AddressOf_auto_Connect)
        auto_Connect.Start()
    End Sub
    Sub AddressOf_auto_Connect()
        If auto_testconnect() Then
            m_isConnect = True
        Else
            m_isConnect = False
        End If

        While True

            If m_isConnect Then
                System.Threading.Thread.Sleep(300000)
            Else
                System.Threading.Thread.Sleep(60000)
            End If
            If auto_testconnect() Then
                m_isConnect = True
            Else
                m_isConnect = False
            End If

        End While
    End Sub
    Private Function auto_testconnect() As Boolean

        Try
            Dim sConnStr As String = String.Format("Server={0};database={1};User Id={2};Password={3};pooling=false;Connection Timeout=3;Port={4};", _
                                                   Me.Parameters.mysql_ip, Me.Parameters.mysql_database, Me.Parameters.mysql_username, Me.Parameters.mysql_password, Me.Parameters.mysql_port)
            Me.m_MySqlConnection = New MySqlConnection(sConnStr)
            '   Console.WriteLine(sConnStr)
            Me.m_MySqlConnection.Open()

            Me.m_MySqlConnection.Close()
        Catch ex As Exception
            Me.m_MySqlConnection.Close()
            Return False
        Finally

        End Try

        Return True


    End Function
    'Public Sub connect()
    '    Try
    '        If m_MySqlConnection IsNot Nothing Then
    '            m_MySqlConnection.Close()
    '        End If
    '        Dim sConnStr As String = String.Format("Server={0};database={1};User Id={2};Password={3};pooling=false;", _
    '                                               Me.Parameters.mysql_ip, Me.Parameters.mysql_database, Me.Parameters.mysql_username, Me.Parameters.mysql_password)
    '        Me.m_MySqlConnection = New MySqlConnection(sConnStr)
    '        ' AddHandler m_MySqlConnection.StateChange, AddressOf Me.AddressOf_StateChange
    '        Me.m_MySqlConnection.Open()

    '    Catch ex As Exception

    '    End Try

    '    '    Dim sConnStr As String = "Server=192.168.1.30;database=tdvdb;User Id=tDV;Password=tdv1234;pooling=false;"
    'End Sub
    'Public Sub connect(ip As String, username As String, passwords As String)
    '    Me.Parameters.mysql_ip = ip

    '    Me.Parameters.mysql_username = username
    '    Me.Parameters.mysql_password = passwords
    '    'Me.mysql_database = "tdvdb"
    '    ' Me.mysql_table = "tdvevent"

    '    Me.connect()
    'End Sub
    'Public Event ReaderStatus(value As Boolean) 'Implements sunsky.sensorIF.AVI.Status
    Sub AddressOf_StateChange(ByVal sender As Object, ByVal e As System.Data.StateChangeEventArgs)
        Console.WriteLine(e.OriginalState.ToString + "->>" + e.CurrentState.ToString)
        'Select Case e.CurrentState
        '    Case ConnectionState.Open
        '        RaiseEvent ReaderStatus(True)
        '    Case Else
        '        RaiseEvent ReaderStatus(False)
        'End Select
    End Sub
    'Protected Sub RaiseEvent_ReaderStatus(value As Boolean)
    '    RaiseEvent ReaderStatus(value)
    'End Sub
    'ReadOnly Property Status As Boolean
    '    Get
    '        If Me.m_MySqlConnection Is Nothing Then
    '            Return False
    '        End If
    '        Select Case Me.m_MySqlConnection.State
    '            Case ConnectionState.Open
    '                Return True
    '            Case Else
    '                Return False
    '        End Select
    '    End Get
    'End Property
    Sub close()
        If auto_Connect IsNot Nothing Then
            Try
                auto_Connect.Abort()
            Catch ex As Exception

            End Try

        End If
        If Me.m_MySqlConnection IsNot Nothing Then
            Me.m_MySqlConnection.Close()
        End If
        'If m_MySqlConnection IsNot Nothing Then
        '    RemoveHandler m_MySqlConnection.StateChange, AddressOf Me.AddressOf_StateChange
        'End If

    End Sub
    Function use_SELECT_comm(ByVal SELECT_text As String, ByVal TableName As String) As DataSet


        Dim sConnStr As String = String.Format("Server={0};database={1};User Id={2};Password={3};pooling=false;Port={4};", _
                                             Me.Parameters.mysql_ip, Me.Parameters.mysql_database, Me.Parameters.mysql_username, Me.Parameters.mysql_password, Me.Parameters.mysql_port)
        Me.m_MySqlConnection = New MySqlConnection(sConnStr)
        ' AddHandler m_MySqlConnection.StateChange, AddressOf Me.AddressOf_StateChange
        Try
            Me.m_MySqlConnection.Open()
        Catch ex As Exception

        End Try


        Dim oset As New DataSet
        'If Me.m_MySqlConnection.State = ConnectionState.Open Then
        If Me.m_MySqlConnection.State = ConnectionState.Connecting Or Me.m_MySqlConnection.State = ConnectionState.Open Then
            Try
                Dim oDbAdapter_1 As New MySqlDataAdapter(SELECT_text, Me.m_MySqlConnection)

                oDbAdapter_1.Fill(oset, TableName)
                oDbAdapter_1.Dispose()
            Catch ex As Exception

            End Try

        End If
        Me.m_MySqlConnection.Close()
        Return oset
    End Function
    'Function use_SELECT_comm(SELECT_text As String, TableName As String) As DataSet

    '    If m_MySqlConnection IsNot Nothing Then
    '        m_MySqlConnection.Close()
    '    End If
    '    Dim sConnStr As String = String.Format("Server={0};database={1};User Id={2};Password={3};pooling=false;", _
    '                                           Me.Parameters.mysql_ip, Me.Parameters.mysql_database, Me.Parameters.mysql_username, Me.Parameters.mysql_password)
    '    Me.m_MySqlConnection = New MySqlConnection(sConnStr)
    '    ' AddHandler m_MySqlConnection.StateChange, AddressOf Me.AddressOf_StateChange
    '    Me.m_MySqlConnection.Open()

    '    Dim oset As New DataSet
    '    'If Me.m_MySqlConnection.State = ConnectionState.Open Then
    '    If Me.Status Then
    '        Try
    '            Dim oDbAdapter_1 As New MySqlDataAdapter(SELECT_text, Me.m_MySqlConnection)

    '            oDbAdapter_1.Fill(oset, TableName)
    '            oDbAdapter_1.Dispose()
    '        Catch ex As Exception

    '        End Try

    '    End If
    '    Me.m_MySqlConnection.Close()
    '    Return oset
    'End Function
    Function use_Delete_comm(ByVal t_deletecmd As String, ByVal TableName As String) As Integer
        Dim sConnStr As String = String.Format("Server={0};database={1};User Id={2};Password={3};pooling=false;Port={4};", _
                                          Me.Parameters.mysql_ip, Me.Parameters.mysql_database, Me.Parameters.mysql_username, Me.Parameters.mysql_password, Me.Parameters.mysql_port)
        Me.m_MySqlConnection = New MySqlConnection(sConnStr)
        ' AddHandler m_MySqlConnection.StateChange, AddressOf Me.AddressOf_StateChange
        Me.m_MySqlConnection.Open()
        '  Dim oset As New DataSet
        Dim count As Integer
        If Me.m_MySqlConnection.State = ConnectionState.Connecting Then
            Try

                'Dim oDbAdapter_1 As New MySqlDataAdapter(SELECT_text, Me.m_MySqlConnection)
                Dim oDbAdapter_1 As New MySqlDataAdapter
                Dim delete_cmd As New MySqlCommand(t_deletecmd, Me.m_MySqlConnection)
                ' oDbAdapter_1.Fill(oset, TableName)
                oDbAdapter_1.DeleteCommand = delete_cmd
                count = oDbAdapter_1.DeleteCommand.ExecuteNonQuery()

                'oDbAdapter_1.Update(oset, TableName)
                'oDbAdapter_1.Update()
                'oDbAdapter_1.Fill(oset, TableName)
                'oDbAdapter_1.Dispose()
            Catch ex As Exception

            End Try

        End If
        Me.m_MySqlConnection.Close()
        Return count
    End Function

    '範例
    Private Sub test()
        ' Dim sConnStr As String = "Server=192.168.1.30;database=tdvdb;User Id=tDV;Password=tdv1234;pooling=false;"
        Dim sConnStr As String = String.Format("Server={0};database={1};User Id={2};Password={3};pooling=false;", _
                                              Me.Parameters.mysql_ip, Me.Parameters.mysql_database, Me.Parameters.mysql_username, Me.Parameters.mysql_password)
        Dim oConn1 As New MySqlConnection(sConnStr)
        oConn1.Open()
        Dim sqlCommand As New MySqlCommand
        'oDbAdapter_1 = New MySqlDataAdapter("SELECT * FROM parking_history where DATE_TIME like '" & dd & "%'", oConn1)
        Dim oDbAdapter_1 As New MySqlDataAdapter("SELECT * FROM tdvevent  ", oConn1)

        Dim set1 As New DataSet
        oDbAdapter_1.Fill(set1, "oTable")
        Console.WriteLine(set1.Tables("oTable").Rows.Count.ToString)
    End Sub


End Class


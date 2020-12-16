Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.IO.Pipes
Imports System.Linq
Imports System.Text
Imports System.Threading
'Imports System.Threading.Tasks
'20200605
'Namespace PipeServer
'20200820
Class PipeServer
    Private t As Thread
    Private m_pipeServer As NamedPipeServerStream
    Sub start()
        t = New Thread(AddressOf Main2)
        t.Start(Nothing)
    End Sub
    Event data()
    Private Sub cloesServer()
        Using pipeClient As NamedPipeClientStream = New NamedPipeClientStream(".", "testpipe", PipeDirection.InOut)
            pipeClient.Connect()

        End Using
    End Sub

    Sub close()
        Try
            server_read_enabale = False
            server_enabale = False
            If m_pipeServer IsNot Nothing Then
                'pipeServer.re()

                If m_pipeServer.IsConnected Then
                    m_pipeServer.Disconnect()
                Else
                    cloesServer()
                End If

                m_pipeServer.Close()
                m_pipeServer.Dispose()

            End If
            't.Abort()
        Catch ex As Exception

        End Try
    End Sub
    Sub Main(ByVal args As String())
        Console.WriteLine("Waiting client to connect..." & vbLf)

        m_pipeServer = New NamedPipeServerStream("testpipe", PipeDirection.InOut)
        m_pipeServer.WaitForConnection()

        Console.WriteLine("[server] a client connected")
        Dim reader As StreamReader = New StreamReader(m_pipeServer)
        Dim writer As StreamWriter = New StreamWriter(m_pipeServer)
        writer.AutoFlush = True
        Dim line As String = Nothing

        'While (CSharpImpl.__Assign(line, reader.ReadLine())) IsNot Nothing
        '    Console.WriteLine("[server] receive message:" & line)
        '    writer.WriteLine("I'm fine")
        '    pipeServer.WaitForPipeDrain()

        '    If "line" = "EXIT" Then
        '        Exit While
        '    End If
        'End While
        line = reader.ReadLine()
        While line IsNot Nothing
            Thread.Sleep(100)
            RaiseEvent data()
            Console.WriteLine("[server] receive message:" & line)
            writer.WriteLine("I'm fine")
            m_pipeServer.WaitForPipeDrain()

            If "line" = "EXIT" Then
                Exit While
            End If
            line = reader.ReadLine()
        End While


        Console.ReadLine()
    End Sub
    Private server_enabale As Boolean = True
    Private server_read_enabale As Boolean = True
    Sub Main2(ByVal args As String())
        server_enabale = True
        While server_enabale
            Thread.Sleep(100)
            Console.WriteLine("Waiting client to connect..." & vbLf)
            Try
                m_pipeServer = New NamedPipeServerStream("testpipe", PipeDirection.InOut)
                m_pipeServer.WaitForConnection()
            Catch ex As Exception
                Console.WriteLine("[server] err " + ex.ToString)
                Continue While
            End Try
            Console.WriteLine("[server] a client connected")
            Dim reader As StreamReader = New StreamReader(m_pipeServer)
            Dim writer As StreamWriter = New StreamWriter(m_pipeServer)
            Dim line As String = Nothing
            Try

                writer.AutoFlush = True

                server_read_enabale = True
            Catch ex As Exception
                Console.WriteLine("[server] err " + ex.ToString)
                server_read_enabale = False
                Continue While
            End Try


            While server_read_enabale
                Thread.Sleep(100)
                Try

                    line = reader.ReadLine()
                    If line IsNot Nothing Then
                        RaiseEvent data()
                        Console.WriteLine("[server] receive message:" & line)
                        writer.WriteLine("I'm fine")
                        m_pipeServer.WaitForPipeDrain()

                        If line = "EXIT" Then
                            server_enabale = False
                        End If
                        server_read_enabale = False
                    End If
                Catch ex As Exception
                    server_read_enabale = False
                End Try

            End While


            m_pipeServer.Close()
        End While

    End Sub
    Private Class CSharpImpl
        <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
        Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function
    End Class
    Sub writeData(text As String)
        If m_pipeServer Is Nothing Then
            Exit Sub
        End If
        Try
            Dim writer As StreamWriter = New StreamWriter(m_pipeServer)
            writer.AutoFlush = True
            writer.WriteLine(text)
        Catch ex As Exception

        End Try

    End Sub
End Class
Class PipeServer_onlyRead
    Protected t As Thread
    Protected m_pipeServer As NamedPipeServerStream

    Protected server_enabale As Boolean
    Protected server_read_enabale As Boolean
    Public PipeName As String
    Public R_Text As String
    Public exit_keyWord As String
    Sub New(t_PipeName As String)
        PipeName = "testpipe"
        If t_PipeName = "" Then
        Else
            PipeName = t_PipeName
        End If
        exit_keyWord = "EXIT"
        R_Text = "I'm fine"
    End Sub
    Sub start()
        server_enabale = True
        server_read_enabale = True
        t = New Thread(AddressOf Main2)
        t.Start(Nothing)
    End Sub
    Event data(text As String)
    Event exit_thread()

    Protected Sub cloesServer()
        Using pipeClient As NamedPipeClientStream = New NamedPipeClientStream(".", PipeName, PipeDirection.InOut)
            pipeClient.Connect()

        End Using
    End Sub

    Sub close()
        Try
            server_read_enabale = False
            server_enabale = False
            If Not t.IsAlive Then
                m_pipeServer.Close()
                m_pipeServer.Dispose()
                Exit Sub
            End If

            If m_pipeServer IsNot Nothing Then
                'pipeServer.re()
                cloesServer()
                If m_pipeServer.IsConnected Then
                    m_pipeServer.Disconnect()
                End If

                m_pipeServer.Close()
                m_pipeServer.Dispose()

            End If
            't.Abort()
        Catch ex As Exception

        End Try
    End Sub

    Sub Main2(ByVal args As String())
        server_enabale = True
        While server_enabale
            Thread.Sleep(100)
            Console.WriteLine("Waiting client to connect..." & vbLf)
            Try
                m_pipeServer = New NamedPipeServerStream(PipeName, PipeDirection.InOut)
                m_pipeServer.WaitForConnection()
            Catch ex As Exception
                Console.WriteLine("[server] errtime " + Now.ToString)
                Console.WriteLine("[server] err " + ex.ToString)
                Thread.Sleep(1000)
                Continue While
            End Try
            Console.WriteLine("[server] a client connected")
            Dim reader As StreamReader = New StreamReader(m_pipeServer)
            Dim writer As StreamWriter = New StreamWriter(m_pipeServer)
            Dim line As String = Nothing
            Try

                writer.AutoFlush = True

                server_read_enabale = True
            Catch ex As Exception
                Console.WriteLine("[server] err " + ex.ToString)
                server_read_enabale = False
                Continue While
            End Try


            While server_read_enabale
                Thread.Sleep(100)
                Try

                    line = reader.ReadLine()
                    If line IsNot Nothing Then
                        RaiseEvent data(line)
                        Console.WriteLine("[server] receive message:" & line)
                        writer.WriteLine(R_Text)
                        m_pipeServer.WaitForPipeDrain()

                        If line = exit_keyWord Then
                            server_enabale = False
                            RaiseEvent exit_thread()
                        End If
                        server_read_enabale = False
                    End If
                Catch ex As Exception
                    server_read_enabale = False
                End Try

            End While


            m_pipeServer.Close()
        End While

    End Sub
    Private Class CSharpImpl
        <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
        Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function
    End Class
End Class
'End Namespace
Class PipeServer_onlywrite
    'Protected t As Thread
    'Protected m_pipeServer As NamedPipeServerStream

    'Protected server_enabale As Boolean
    'Protected server_read_enabale As Boolean
    'Public PipeName As String
    'Public R_Text As String
    'Public exit_keyWord As String
    'Sub New(t_PipeName As String)
    '    PipeName = "testpipe"
    '    If t_PipeName = "" Then
    '    Else
    '        PipeName = t_PipeName
    '    End If
    '    exit_keyWord = "EXIT"
    '    R_Text = "I'm fine"
    'End Sub
    'Sub start()
    '    server_enabale = True
    '    server_read_enabale = True
    '    t = New Thread(AddressOf Main2)
    '    t.Start(Nothing)
    'End Sub
    'Event data(text As String)
    'Event exit_thread()

    'Protected Sub cloesServer()
    '    Using pipeClient As NamedPipeClientStream = New NamedPipeClientStream(".", PipeName, PipeDirection.InOut)
    '        pipeClient.Connect()

    '    End Using
    'End Sub

    'Sub close()
    '    Try
    '        server_read_enabale = False
    '        server_enabale = False
    '        If Not t.IsAlive Then
    '            m_pipeServer.Close()
    '            m_pipeServer.Dispose()
    '            Exit Sub
    '        End If

    '        If m_pipeServer IsNot Nothing Then
    '            'pipeServer.re()
    '            cloesServer()
    '            If m_pipeServer.IsConnected Then
    '                m_pipeServer.Disconnect()
    '            End If

    '            m_pipeServer.Close()
    '            m_pipeServer.Dispose()

    '        End If
    '        't.Abort()
    '    Catch ex As Exception

    '    End Try
    'End Sub

    'Sub Main2(ByVal args As String())
    '    server_enabale = True
    '    While server_enabale
    '        Thread.Sleep(100)
    '        Console.WriteLine("Waiting client to connect..." & vbLf)
    '        Try
    '            m_pipeServer = New NamedPipeServerStream(PipeName, PipeDirection.InOut)
    '            m_pipeServer.WaitForConnection()
    '        Catch ex As Exception
    '            Console.WriteLine("[server] errtime " + Now.ToString)
    '            Console.WriteLine("[server] err " + ex.ToString)
    '            Thread.Sleep(1000)
    '            Continue While
    '        End Try
    '        Console.WriteLine("[server] a client connected")
    '        Dim reader As StreamReader = New StreamReader(m_pipeServer)
    '        Dim writer As StreamWriter = New StreamWriter(m_pipeServer)
    '        Dim line As String = Nothing
    '        Try

    '            writer.AutoFlush = True

    '            server_read_enabale = True
    '        Catch ex As Exception
    '            Console.WriteLine("[server] err " + ex.ToString)
    '            server_read_enabale = False
    '            Continue While
    '        End Try


    '        While server_read_enabale
    '            Thread.Sleep(100)
    '            Try

    '                line = reader.ReadLine()
    '                If line IsNot Nothing Then
    '                    RaiseEvent data(line)
    '                    Console.WriteLine("[server] receive message:" & line)
    '                    writer.WriteLine(R_Text)
    '                    m_pipeServer.WaitForPipeDrain()

    '                    If line = exit_keyWord Then
    '                        server_enabale = False
    '                        RaiseEvent exit_thread()
    '                    End If
    '                    server_read_enabale = False
    '                End If
    '            Catch ex As Exception
    '                server_read_enabale = False
    '            End Try

    '        End While


    '        m_pipeServer.Close()
    '    End While

    'End Sub
    'Private Class CSharpImpl
    '    <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
    '    Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
    '        target = value
    '        Return value
    '    End Function
    'End Class

    Sub write(pipename As String, ByVal text As String)
        'Using pipeClient As NamedPipeClientStream = New NamedPipeClientStream("host path", "testpipe", PipeDirection.InOut)
        Using pipeClient As NamedPipeClientStream = New NamedPipeClientStream(".", pipename, PipeDirection.InOut)
            Console.WriteLine("[client] Attemping to connect to pipe...")
            Try
                pipeClient.Connect(500)
            Catch ex As Exception
                'Console.WriteLine("[client] connected Err" + ex.ToString)
                'RaiseEvent ConnectErr()
                'pipe_ErrlogFile.Write("[client] connected Err " + ex.ToString)
                'Me.re_write(pipename, text, 0)
                Exit Sub
            End Try

            Console.WriteLine("[client] connected to pipe")
            Console.WriteLine("[client] There are currently {0} pipe server instances open.", pipeClient.NumberOfServerInstances)
            Dim writer As StreamWriter = New StreamWriter(pipeClient)
            writer.AutoFlush = True
            Dim reader As StreamReader = New StreamReader(pipeClient)
            Dim temp As String = ""

            'While True
            temp = text
            Try
                writer.WriteLine(temp)
                pipeClient.WaitForPipeDrain()
                Console.WriteLine("[client] read message from server:" & reader.ReadLine())
            Catch ex As Exception
                'Console.WriteLine("[client] read message Err" + ex.ToString)
                ''pipe_ErrlogFile.Write("[client] read message Err" + ex.ToString)
                'pipe_ErrlogFile.Write("[client] read message Err1 " + pipename + " " + text)
                'pipe_ErrlogFile.Write("[client] read message Err2 " + ex.ToString)
            End Try



            '    If temp = "EXIT" Then
            '        Exit While
            '    End If
            'End While
        End Using

        'Console.ReadLine()
    End Sub

End Class

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.IO.Pipes
Imports System.Linq
Imports System.Text
'Imports System.Threading.Tasks
'https://www.cnblogs.com/HDK2016/p/9840989.html
'Namespace PipeClient
'20200605
Class PipeClient
    Shared Sub Main(ByVal args As String())
        Using pipeClient As NamedPipeClientStream = New NamedPipeClientStream("host path", "testpipe", PipeDirection.InOut)
            Console.WriteLine("[client] Attemping to connect to pipe...")
            pipeClient.Connect()
            Console.WriteLine("[client] connected to pipe")
            Console.WriteLine("[client] There are currently {0} pipe server instances open.", pipeClient.NumberOfServerInstances)
            Dim writer As StreamWriter = New StreamWriter(pipeClient)
            writer.AutoFlush = True
            Dim reader As StreamReader = New StreamReader(pipeClient)
            Dim temp As String = ""

            While True
                Threading.Thread.Sleep(100)
                temp = Console.ReadLine()
                writer.WriteLine(temp)
                pipeClient.WaitForPipeDrain()
                Console.WriteLine("[client] read message from server:" & reader.ReadLine())

                If temp = "EXIT" Then
                    Exit While
                End If
            End While
        End Using

        Console.ReadLine()
    End Sub
    Shared Sub Main2(ByVal text As String)
        'Using pipeClient As NamedPipeClientStream = New NamedPipeClientStream("host path", "testpipe", PipeDirection.InOut)
        Using pipeClient As NamedPipeClientStream = New NamedPipeClientStream(".", "testpipe", PipeDirection.InOut)
            Console.WriteLine("[client] Attemping to connect to pipe...")
            Try

                pipeClient.Connect(500)
            Catch ex As Exception
                Console.WriteLine("[client] connected Err" + ex.ToString)

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
                Console.WriteLine("[client] read message Err" + ex.ToString)
            End Try



            '    If temp = "EXIT" Then
            '        Exit While
            '    End If
            'End While
        End Using

        'Console.ReadLine()
    End Sub
    Sub input(text As String)

    End Sub
End Class
'End Namespace
Class PipeClient_onlyWrite

    Shared Sub write(pipename As String, ByVal text As String)
        'Using pipeClient As NamedPipeClientStream = New NamedPipeClientStream("host path", "testpipe", PipeDirection.InOut)
        Using pipeClient As NamedPipeClientStream = New NamedPipeClientStream(".", pipename, PipeDirection.InOut)
            Console.WriteLine("[client] Attemping to connect to pipe...")
            Try
                pipeClient.Connect(500)
            Catch ex As Exception
                Console.WriteLine("[client] connected Err" + ex.ToString)

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
                Console.WriteLine("[client] read message Err" + ex.ToString)
            End Try



            '    If temp = "EXIT" Then
            '        Exit While
            '    End If
            'End While
        End Using

        'Console.ReadLine()
    End Sub

End Class
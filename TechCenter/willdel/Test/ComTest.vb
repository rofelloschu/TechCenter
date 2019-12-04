Imports System.Threading
Imports System.IO.Ports
'20180509 閱
Class ComTest
    Private t As Thread
    Private com As SerialPort
    Private run As Boolean
    Private name As String
    Private file As APFile
    Private file_err As APFile

    Private timeout As Integer
    Private ComPortSocket As ComPortSocket
    Sub New(ByVal comName As String)
        run = True
        name = comName
        Console.WriteLine(name + " new")
        file_err = New APFile(name + "_err.txt")
        Try


            t = New Thread(AddressOf AutoRead)
            t.Start()
        Catch ex As Exception
            run = False
            Console.WriteLine(name + " err")
            Console.WriteLine(ex)
            file_err.write(Now.ToString)
            file_err.write(ex.ToString)
        End Try

    End Sub
    Sub New(ByVal comName As String, ByVal timeout As Integer)
        run = True
        name = comName
        Console.WriteLine(name + " new")
        file_err = New APFile(name + "_err.txt")
        Me.timeout = timeout
        Console.WriteLine(name + " Tetimeoutst:" + timeout.ToString + "ms")
        Console.WriteLine(name + " Test")

        Try
            ComPortSocket = New ComPortSocket()
            ComPortSocket.connect(name, 9600)
            Console.WriteLine(name + " open")

            'test for
            Dim testText As String = ""
            Dim isSuccess As Boolean = False
            For index As Integer = 0 To 9
                testText = testText + index.ToString
                If WriteRead(testText) Then
                    Console.WriteLine("Success")
                    isSuccess = True
                Else
                    Console.WriteLine("Failure")
                    isSuccess = False
                    Exit For
                End If
            Next
            If isSuccess Then
                Console.WriteLine("TestSuccess")
            Else
                Console.WriteLine("TestFailure")
            End If
        Catch ex As Exception
            run = False

            Console.WriteLine(name + " err")
            Console.WriteLine(ex)
            file_err.write(Now.ToString)
            file_err.write(ex.ToString)
        End Try
        Console.ReadLine()

    End Sub
    Function WriteRead(ByVal text As String) As Boolean
        Dim readText As String = ""

        ComPortSocket.writeData(text)
        Console.WriteLine("send:        " + text)
        'While (True)
        '    Thread.Sleep(1)
        'End While
        For index As Integer = 0 To 9
            Thread.Sleep(100)
            readText = ComPortSocket.ReadText
            If readText.Length > 0 Then
                Console.WriteLine("Receiver:    " + readText)
                Exit For

            End If
        Next

        Return readText.Equals(text)
    End Function


    Sub AutoRead()
        Dim string_data As String = ""
        Console.WriteLine(name + " start")
        file = New APFile(name + ".txt")
        Try
            com = New SerialPort(name, 9600)
            com.Open()
        Catch ex As Exception
            run = False
            Console.WriteLine(name + " err")
            Console.WriteLine(ex)
            file_err.write(Now.ToString)
            file_err.write(ex.ToString)
        End Try

        Console.WriteLine(name + " run")
        While run

            Thread.Sleep(1000)
            Try
                string_data = com.ReadExisting()
            Catch ex As Exception
                run = False
                Console.WriteLine(name + " err")
                Console.WriteLine(ex)
                file_err.write(Now.ToString)
                file_err.write(ex.ToString)
            End Try
            If string_data <> "" Then
                Console.WriteLine(name + " read: " + string_data)
                file.write(Now.ToString)
                file.write(string_data)
            End If

        End While
        Console.WriteLine(name + " end")
    End Sub
End Class
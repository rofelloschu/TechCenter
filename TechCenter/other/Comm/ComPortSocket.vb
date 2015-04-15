'20121004
Imports System.IO
Imports System.Threading
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Public Class ComPortSocket

    Private comport As New IO.Ports.SerialPort
    Private Connected As Boolean
    Private log As APFile
    Private mydata As Byte()
    Private RWlog As APFile
    Private a_read As Thread
    Private ID As Integer


    Sub New()
        ID = 0
        Connected = False
        log = New APFile("comlog" + ID.ToString + ".txt")
        RWlog = New APFile("comRWlog" + ID.ToString + ".txt")
    End Sub


    Public Sub connect(ByVal text As String, ByVal i As Integer)
        Try

            comport = New IO.Ports.SerialPort(text, i, IO.Ports.Parity.None, 8, IO.Ports.StopBits.One)

            ' comport = New IO.Ports.SerialPort(text, i)
            comport.ReadTimeout = 1000
            comport.Open()
            M_WriteLineMaster.WriteLine(text + " connect")
            Connected = True
        Catch ex As Exception
            M_WriteLineMaster.WriteLine("comport ERROR")
            log.write("connect error")
            'log.write(ex.ToString)
            Connected = False
            Thread.Sleep(10000)
        Finally
            GC.Collect()
        End Try
    End Sub
    Public Sub disconnect()
        Try
            If Connected Then
                comport.Close()
                Connected = False

            End If

        Catch ex As Exception
            log.write("disconnect error")
            log.write(ex.ToString)
            Connected = False
        Finally
            GC.Collect()
        End Try
    End Sub
    Function getConnected() As Boolean
        Return Connected
    End Function
    Function getComport() As IO.Ports.SerialPort
        Return comport
    End Function
    Function readData(ByVal data() As Byte) As Integer
        Dim x As Integer = 0
        Try
            If Me.Connected Then


                x = comport.Read(data, 0, data.Length)
                'RWlog.write("R<< ")
                'RWlog.write(Now.Hour.ToString + ":" + Now.Minute.ToString + ":" + Now.Second.ToString + ",R<< ", data)


            End If
        Catch ex As Exception
            log.write("readData error")
            log.write(ex.ToString)
            comport.Close()
            Connected = False
        End Try
        Return x
    End Function
    Function autoRead(ByVal data() As Byte) As Integer

        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim flag As Boolean = True
        While flag
            Thread.Sleep(1)
            j = i
            i = comport.BytesToRead
            If (i = j And i > 0) Then


                i = readData(data)
                flag = False
                mydata = data
            Else
                Thread.Sleep(50)
            End If

        End While
        Return i
    End Function

    Function autoRead() As Integer
        Dim data(1023) As Byte
        Return autoRead(data)
    End Function
    Function ReadText() As String
        Dim text As String = ""
        Try
            If Me.Connected Then


                text = comport.ReadExisting
                'RWlog.write("R<< ")
                'RWlog.write(Now.Hour.ToString + ":" + Now.Minute.ToString + ":" + Now.Second.ToString + ",R<< ", data)


            End If
        Catch ex As Exception
            log.write("ReadText error")
            log.write(ex.ToString)
            comport.Close()
            Connected = False
        End Try
        Return text
    End Function
    Function getData() As Byte()
        Return mydata
    End Function
    Sub writeData(ByVal data() As Byte)
        Try
            If Me.Connected Then
                comport.Write(data, 0, data.Length)
                'log.write("write data")
                'log.write(data)

            End If
        Catch ex As Exception
            RWlog.write("writeData error")
            RWlog.write(ex.ToString)
            Connected = False
        End Try

    End Sub
    Sub writeData(ByVal data As String)
        Try
            If Me.Connected Then
                comport.Write(data)
                'log.write("write data")
                'log.write(data)

            End If
        Catch ex As Exception
            RWlog.write("writeDataString error")
            RWlog.write(ex.ToString)
            Connected = False
        End Try

    End Sub
    Function getReadByte() As Integer
        Return comport.BytesToRead
    End Function
    Sub close()
        disconnect()
        log.Dispose()
        RWlog.Dispose()
    End Sub
End Class

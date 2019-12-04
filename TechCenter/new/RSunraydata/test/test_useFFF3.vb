Imports System.Threading
Imports classLibrary_bang
Public Class test_useFFF3
    Private t As Thread

    Private com As System.IO.Ports.SerialPort

    Private sensorlog_file As APFile
    Private log_file As APFile
    Private log2_file As APFile

    Private save_historySunrayData As save_historySunrayData

    Private t_B As Thread

    Private seachSunrayData As seachSunrayData
    'Private guessSunrayData_list As List(Of guessSunrayData)
    Sub New()

        sensorlog_file = New APFile("sensorlog.txt")
        log_file = New APFile("log.txt")
        log2_file = New APFile("log2.txt")
        save_historySunrayData = New save_historySunrayData
        save_historySunrayData.loadAllFile()
        'guessSunrayData_list = New List(Of guessSunrayData)
        seachSunrayData = New seachSunrayData
        AddHandler seachSunrayData.WriteFFF3Data, AddressOf b3_writeData
        Try
            com = New System.IO.Ports.SerialPort("COM2", 9600, IO.Ports.Parity.None, 8, IO.Ports.StopBits.One)
            com.Open()
            AddHandler com.DataReceived, AddressOf com_DataReceived
        Catch ex As Exception
            MsgBox("com 設定錯誤")
            Exit Sub
        End Try
        't = New Thread(AddressOf a)
        't.Start()

    End Sub
    Sub New(o As Object)
        sensorlog_file = New APFile("sensorlog.txt")
        log_file = New APFile("log.txt")
        save_historySunrayData = New save_historySunrayData
        save_historySunrayData.loadAllFile()
    End Sub
    Sub close()
        Try

            t.Abort()


        Catch ex As Exception

        End Try

        Try
            If t_B IsNot Nothing Then
                t_B.Abort()
            End If
        Catch ex As Exception

        End Try
        com.Close()
        seachSunrayData.close()
    End Sub
    Sub a()
        While True
            Thread.Sleep(1000)
            If Now.Second = 10 Then
                Try
                    Dim fff3 As New sunray_format_FFF3
                    fff3.setIndex(1)
                    Dim data As Byte() = fff3.getdata
                    Me.com.Write(data, 0, data.Length)
                Catch ex As Exception
                    MsgBox("a " + ex.ToString)
                End Try


            End If
        End While
    End Sub

    Sub start_B()
        If t_B Is Nothing Then

        Else

            If Not t_B.IsAlive Then

                MsgBox("執行過")
                Exit Sub

            End If

        End If
        t_B = New Thread(AddressOf b3)
        t_B.Start(Now.AddDays(-1))

    End Sub
    Sub b(parameter As Object)
        '延遲500不能用
        Dim count As Integer = parameter

        Dim count2 As Integer = Now.Minute + Now.Hour * 60
        Dim a As Integer = count2 + 1
        Dim b As Integer = count + count2
        M_dubug.debugfile.Write("start " + a.ToString + "~~" + b.ToString)
        For index As Integer = a To b

            Thread.Sleep(1000)

            Try
                Dim fff3 As New sunray_format_FFF3
                fff3.setIndex(index)

                Dim data As Byte() = fff3.getdata
                Me.com.Write(data, 0, data.Length)
            Catch ex As Exception
                MsgBox("b " + ex.ToString)
            End Try

            M_dubug.debugfile.Write(index.ToString)

        Next

        'MsgBox("完成")

    End Sub
    Sub b2(parameter As Object)
        '延遲500不能用
        Dim nowtime As DateTime = parameter
        'Dim count2 As Integer = Now.Minute + Now.Hour * 60
        Dim start_time As DateTime = New DateTime(nowtime.Year, nowtime.Month, nowtime.Day, 0, 0, 0)
        Dim end_time As DateTime = New DateTime(nowtime.Year, nowtime.Month, nowtime.Day, 23, 59, 49)
        M_dubug.debugfile.Write("start " + start_time.ToString + "~~" + end_time.ToString)
        Dim index_time As DateTime = start_time
        Dim count As Integer = 0
        While end_time > index_time
            Thread.Sleep(1000)
            If Now.Second = 59 Then
                '避免時間查錯
                Thread.Sleep(1000)
            End If
            M_dubug.debugfile.Write("搜尋 " + index_time.ToString("u"))
            Me.b2_2(index_time)

            index_time = index_time.AddMinutes(1)
            '避免迴圈
            count = count + 1
            If count >= 2000 Then

                M_dubug.debugfile.Write(Now.ToString("u") + " 避免迴圈b2 " + index_time.AddMinutes(-1).ToString("u"))
                Exit While
            End If
        End While
        M_dubug.debugfile.Write("搜尋完成 ")


    End Sub
    Private now_deviation As Integer = 0
    Private AutoResetEvent2 As AutoResetEvent = New AutoResetEvent(True)
    Private guessSunrayData As guessSunrayData
    Sub b2_2(TargetDataTime As DateTime)

        'Dim TargetDataTime As DateTime = Now
        'Dim guessSunrayData As guessSunrayData = New guessSunrayData(now_deviation)
        guessSunrayData = New guessSunrayData(TargetDataTime, now_deviation, 10)
        'AutoResetEvent2.WaitOne()
        'guessSunrayData_list.Add(guessSunrayData)
        'AutoResetEvent2.Set()

        'Dim temp_count As Integer = now_deviation
        While guessSunrayData.getResult = guessresult.notfind
            Thread.Sleep(1000)
            Dim data As Byte() = guessSunrayData.getNewFFF3()
            Me.com.Write(data, 0, data.Length)
            log2_file.write(Now.ToString + " send")
            log2_file.write(data)

        End While
        If guessSunrayData.getResult = guessresult.close Then
            M_dubug.debugfile.Write("close " + TargetDataTime.ToString("u"))
        End If
        If guessSunrayData.getResult = guessresult.nothings Then
            M_dubug.debugfile.Write("找不到 " + TargetDataTime.ToString("u") + " count " + guessSunrayData.getdeviation.ToString)
        End If
        If guessSunrayData.getResult = guessresult.find Then

            'now_deviation = guessSunrayData.getdeviation
            M_dubug.debugfile.Write("找到 " + TargetDataTime.ToString("u") + "  count " + guessSunrayData.getdeviation.ToString)
        End If
    End Sub
    Sub b3(parameter As Object)
        '延遲500不能用
        Dim nowtime As DateTime = parameter
        'Dim count2 As Integer = Now.Minute + Now.Hour * 60
        Dim start_time As DateTime = New DateTime(nowtime.Year, nowtime.Month, nowtime.Day, 0, 0, 0)
        Dim end_time As DateTime = New DateTime(nowtime.Year, nowtime.Month, nowtime.Day, 23, 59, 49)
        M_dubug.debugfile.Write("start " + start_time.ToString + "~~" + end_time.ToString)
        Dim index_time As DateTime = start_time
        Dim count As Integer = 0
        While end_time > index_time
            Thread.Sleep(1000)

            M_dubug.debugfile.Write("搜尋 " + index_time.ToString("u"))
            seachSunrayData.addDate(index_time)
            'Me.b2_2(index_time)

            index_time = index_time.AddMinutes(1)
            '避免迴圈
            count = count + 1
            If count >= 2000 Then

                M_dubug.debugfile.Write(Now.ToString("u") + " 避免迴圈b2 " + index_time.AddMinutes(-1).ToString("u"))
                Exit While
            End If
        End While
        M_dubug.debugfile.Write("搜尋完成 ")
    End Sub
    Sub b3_writeData(data() As Byte)
        Me.com.Write(data, 0, data.Length)
        log2_file.write(Now.ToString + " send")
        log2_file.write(data)
    End Sub
    Private test_file As New APFile("test.txt")
    Private f_temp_data() As Byte
    Private Received_data As New Received_data
    Private AutoResetEvent As AutoResetEvent = New AutoResetEvent(True)

    Sub com_DataReceived(sender As Object, e As System.IO.Ports.SerialDataReceivedEventArgs)
        AutoResetEvent.WaitOne()
        Try
            Dim sp As System.IO.Ports.SerialPort = CType(sender, System.IO.Ports.SerialPort)
            Dim size As Integer = sp.BytesToRead()
            Dim data(size - 1) As Byte
            sp.Read(data, 0, data.Length)



            '合併
            log_file.write(Now.ToString)
            If data.Length > 0 Then
                Received_data.add(data)

                log_file.write(data)
            Else
                log_file.write("null")
                AutoResetEvent.Set()
                Exit Sub
            End If

            'test_file.write(Now.ToString("u") + " 1 ")
            ' 尋找FF
            Dim index_ff As Integer = -1
            For index As Integer = 0 To Received_data.getData.Length - 1
                If Received_data.getData(index) = &HFF Then
                    index_ff = index
                    Exit For
                End If
            Next

            'test_file.write(Now.ToString("u") + " 2 ")
            '移除FF之前資料
            If index_ff > 0 Then

                Received_data.removeData(index_ff)
                'test_file.write(Now.ToString("u") + " remove " + index_ff.ToString)
            End If
            'test_file.write(Now.ToString("u") + " 3 ")
            '判斷FF資料
            'test_file.write(Now.ToString("u") + " " + Received_data.getData(0).ToString("X2") + Received_data.getData(1).ToString("X2"))
            log2_file.write(Now.ToString)
            log2_file.write(Received_data.getData())
            If Received_data.getData().Length > 3 Then
                If Received_data.getData(0) = &HFF Then

                    Select Case Received_data.getData(1)
                        Case &HF9
                            'test_file.write(Now.ToString("u") + " FFF9 ")
                            If Received_data.getData.Length >= 27 Then
                                Dim data_fff9 As New sunray_format_FFF9
                                data_fff9.setByteData(Received_data.getCountData(27))
                                Me.ReceivedFFF9(data_fff9)
                                seachSunrayData.ReceivedFFF9(data_fff9)
                                Received_data.removeData(27)
                            End If
                        Case Else
                            sensorlog_file.write(Now.ToString("u") + "@notFFF9")
                            Received_data.removeData(1)
                    End Select

                Else
                    sensorlog_file.write(Now.ToString("u") + "@notFF")
                End If

            Else
                sensorlog_file.write(Now.ToString("u") + "@not>3")
            End If



        Catch ex As Exception
            test_file.write(Now.ToString("u") + " err " + ex.ToString)
            MsgBox("com_DataReceived " + ex.ToString)

        End Try
        AutoResetEvent.Set()
    End Sub
    Private Sub ReceivedFFF9(data_fff9 As sunray_format_FFF9)
        sensorlog_file.write(Now.ToString("u") + "@" + data_fff9.to_string)

        '1
        Dim nowData As Boolean = False
        If Not nowData And Me.isDataNow(data_fff9.time, Now) Then
            save_historySunrayData.addFFF9(data_fff9)
            nowData = True
        End If
        If Not nowData And Me.isDataNow(data_fff9.time.AddMinutes(-1), Now) Then
            save_historySunrayData.addFFF9(data_fff9)
            nowData = True
        End If
        If Not nowData And Me.isDataNow(data_fff9.time.AddMinutes(1), Now) Then
            save_historySunrayData.addFFF9(data_fff9)
            nowData = True
        End If
        If nowData Then
            M_dubug.debugfile.Write("現在資料 " + data_fff9.time.ToString("u"))
        End If
        '2
        If Not nowData And guessSunrayData IsNot Nothing Then

            If guessSunrayData.setReturnData(data_fff9) = guessresult.find Then
                save_historySunrayData.addFFF9(data_fff9)
            End If
            M_dubug.debugfile.Write("收到資料 " + data_fff9.time.ToString("u"))
        End If

    End Sub

    Private Function isDataNow(data_time As DateTime, now_time As DateTime) As Boolean
        ''Dim isNowData As Boolean = True
        If (data_time.Minute = now_time.Minute) Then
            'isNowData = False
        Else

            Return False

        End If
     
        If (data_time.Hour = now_time.Hour) Then
            'isNowData = False
        Else
            Return False
        End If
        If (data_time.Day = now_time.Day) Then
            'isNowData = False
        Else
            Return False

        End If
        If (data_time.Month = now_time.Month) Then
            'isNowData = False
        Else
            Return False

        End If
        If (data_time.Year = now_time.Year) Then
            'isNowData = 
        Else
            Return False

        End If
        Return True

    End Function
    Sub saveHisData()

    End Sub
End Class

Imports System.Windows
'Imports ClassLibrary_apple
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
'Imports classLibrary_bang
Imports classLibrary_bang
Public Class Form1



    Sub New()

        ' 此為設計工具所需的呼叫。
        InitializeComponent()

        ' 在 InitializeComponent() 呼叫之後加入任何初始設定。
        'testFF = New test_useFFF3()
    End Sub
    ' Private TC_FaultDetection As New TC_FaultDetection("test01", True)
    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        'If testFF Is Nothing Then
        '    testFF.close()
        'End If

    End Sub
    Private differentValueRecord As New differentValueRecord("test02", True)
    Private testFF As test_useFFF3
     
 
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Dim appDiary As New appDiary
        Dim p As New usePing
        p.testPing("8.8.8.8")
        p.testPing("1.1.1.1")
        p.testPing("61.219.246.61")
        p.testPing("168.95.192.1")
        'Dim a As New Dictionary(Of String, Object)
        'a.Add("123", "123")
        'Dim b As New Dictionary(Of String, Object)
        'b.Add("b123", "b123")
        'b.Add("b234", "b234")

        'Dim c As New List(Of Object)
        'c.Add(b)
        'a.Add("234", c)
        'Dim r As String = JsonConvert.SerializeObject(a)
        'Console.WriteLine(r)


        'Dim jd As New json_dict(r)
        ''Console.WriteLine(jd.getObject("123").ToString)
        'Console.WriteLine(jd.getjson_dict("123").Value)
        ''Console.WriteLine(jd.getObject("234").ToString)
        'Console.WriteLine(jd.getjson_dict("234").Value)

        'Console.WriteLine(jd.getjson_dict("234").getjson_dict("b123").Value)
        ''Dim aa As Dictionary(Of String, Object) = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(r)

        ''Console.WriteLine(aa("123"))
        ''Console.WriteLine(aa("234"))
        'Dim use_jsonToXml As New use_jsonToXml
        'testFF = New test_useFFF3("123")

        'testFF.start_B()
        'Dim c As Single = 44.705
        'Dim fv As Byte() = BitConverter.GetBytes(c)
        'Console.WriteLine(fv(3).ToString("X2") + fv(2).ToString("X2") + fv(1).ToString("X2") + fv(0).ToString("X2"))

        'Dim num2 As Single = BitConverter.ToSingle(fv, 0)
        'Console.WriteLine(num2.ToString)

        'c = 17
        'fv = BitConverter.GetBytes(c)
        'Console.WriteLine(fv(3).ToString("X2") + fv(2).ToString("X2") + fv(1).ToString("X2") + fv(0).ToString("X2"))

        'num2 = BitConverter.ToSingle(fv, 0)
        'Console.WriteLine(num2.ToString)


        'c = 0.625
        'fv = BitConverter.GetBytes(c)
        'Console.WriteLine(fv(3).ToString("X2") + fv(2).ToString("X2") + fv(1).ToString("X2") + fv(0).ToString("X2"))

        'num2 = BitConverter.ToSingle(fv, 0)
        'Console.WriteLine(num2.ToString)



        'json_test()
        'Dim d As New logFile2("test2", True)
        'Console.WriteLine(Now.ToString("u") + " write2")
        'For index As Integer = 0 To 10000
        '    d.Writte(Now.ToString("u") + " " + index.ToString)
        '    'System.Threading.Thread.Sleep(1)
        'Next
        'Console.WriteLine(Now.ToString("u") + " write2 end")

        'Dim d2 As New logFile2("test3", True)
        'Console.WriteLine(Now.ToString("u") + " write3")
        'For index As Integer = 0 To 10000
        '    d2.Writte2(Now.ToString("u") + " " + index.ToString)
        '    'System.Threading.Thread.Sleep(1)
        'Next
        'Console.WriteLine(Now.ToString("u") + " write3 end")
        'd2.FileMaxCount = 100000


        'Dim d4 As New logFile2("test4", True)
        'Console.WriteLine(Now.ToString("u") + " write4")
        'Dim temp As New List(Of String)
        'For index As Integer = 0 To 10000000
        '    temp.Add(Now.ToString("u") + " " + index.ToString)
        '    'd2.Writte2(Now.ToString("u") + " " + index.ToString)
        '    'System.Threading.Thread.Sleep(1)
        'Next
        'Console.WriteLine(Now.ToString("u") + " add4")
        'd4.Writte2_array(temp.ToArray)
        'Console.WriteLine(Now.ToString("u") + " write4 end")
        'd4.FileMaxCount = 20000000


        'Dim d5 As New logFile2("test5", True)
        'Console.WriteLine(Now.ToString("u") + " write5")
        'Dim temp2 As New List(Of String)
        'For index As Integer = 0 To 10000000
        '    temp2.Add(Now.ToString("u") + " " + index.ToString)
        '    'd2.Writte2(Now.ToString("u") + " " + index.ToString)
        '    'System.Threading.Thread.Sleep(1)
        'Next
        'Console.WriteLine(Now.ToString("u") + " add5")
        'd5.Writte_array(temp.ToArray)
        'Console.WriteLine(Now.ToString("u") + " write5 end")
        'd5.FileMaxCount = 200000000
        'Dim test1_value As String = "01100001"
        'Dim t As Byte = Convert.ToByte(test1_value, 2)
        'Dim myOtherBits As BitArray = Me.createBitArray(t)
        'Console.WriteLine(myOtherBits.Get(0).ToString)
        'Console.WriteLine(myOtherBits.Get(7).ToString)
        '    Test.M_test.OpenDir(System.IO.Directory.GetCurrentDirectory)
        'TC_FaultDetection.setValue("new", "true")
        'TC_FaultDetection.setValue("new", "false")
        'TC_FaultDetection.thanValue("new", "true")
        'TC_FaultDetection.thanValue("new", "false")
        'TC_FaultDetection.thanValue("new2", "true")
        'differentValueRecord.setValue("new03", "true")
        'System.Threading.Thread.Sleep(10000)
        'differentValueRecord.setValue("new03", "true")
        'System.Threading.Thread.Sleep(5000)
        'differentValueRecord.setValue("new03", "false
        'Dim ClassC As New ClassC
        'Dim classA As IF_A = ClassC
        'classA.AA()
        'classA.BB()
        'Dim classB As IF_B = ClassC
        'classB.BB()    

        'Dim del As MyDelSub
        'del = New MyDelSub(AddressOf WriteToDebug)
        'del.Invoke()
        'Console.WriteLine(Asc("A"))
        'Dim a As Integer = &H1

        'Console.WriteLine(Now.GetHashCode.ToString)
        'Dim r As New Random()
        'Console.WriteLine(r.Next(0, 10))
        'Dim o As New Object
        'Console.WriteLine(o.GetHashCode.ToString)
        'Console.WriteLine(Guid.NewGuid().GetHashCode())
        'Console.WriteLine(Me.Controls.Count.ToString)
        'Console.WriteLine(Form2.Controls.Count.ToString)

        'Me.Controls("OpenDir_Button").Visible = False
        ''Form2.Container()
        'getControlNames(Me)
        'getControlNames(Form2)
        'Dim timing_timer As New timing_timer
        'Dim timeNow As DateTime = DateTime.Now
        'Dim a As New DateTime(timeNow.Year, timeNow.Month, timeNow.Day, timeNow.Hour, timeNow.Minute, 0)
        'Dim b As New DateTime(timeNow.Year, timeNow.Month, timeNow.Day, timeNow.Hour, timeNow.Minute, 30)
        'Console.WriteLine(b.Date.ToString)
        'If a.Date.Equals(b.Date) Then
        '    MsgBox("true")
        'Else
        '    MsgBox("false")
        'End If
        'Dim deviceID_sequence(4, 2) As Byte

        'Console.WriteLine(deviceID_sequence.Length.ToString)
        'Console.WriteLine(deviceID_sequence.GetLength(0).ToString)
        'Console.WriteLine(deviceID_sequence.GetLength(1).ToString)
        'Console.WriteLine(deviceID_sequence.Rank.ToString)

        'Dim ss As New delayRun
        'ss.run(AddressOf Me.testnow, 5)
        'Console.WriteLine(Now.ToString)
        'classLibrary_bang.delayRun.run(AddressOf Me.testnow, 5)
        'Console.WriteLine(Now.ToString)

    End Sub
    
    Function Dec3(n As String) As Single
        Dim num As UInt32 = UInt32.Parse(n, System.Globalization.NumberStyles.AllowHexSpecifier)
        Dim floatVals As Byte() = BitConverter.GetBytes(num)
        Dim f As Single = BitConverter.ToSingle(floatVals, 0)
        Return f

    End Function
    Private Delegate Sub MyDelSub()
    Private Sub WriteToDebug()
        Console.WriteLine("Delegate Wrote To Debug Window")
    End Sub

    Private Sub OpenDir_Button_Click(sender As Object, e As EventArgs) Handles OpenDir_Button.Click
        test.M_test.OpenDir(System.IO.Directory.GetCurrentDirectory)

    End Sub

    Function getControlNames(t_form As System.Windows.Forms.Form) As String()
        't_form.Controls.Item
        Dim return_list As New List(Of String)
        Dim ctl As Control
        For Each ctl In t_form.Controls
            Console.WriteLine(ctl.Name)
            return_list.Add(ctl.Name)
        Next
        Return return_list.ToArray
    End Function
    Function getFormControl(t_form As System.Windows.Forms.Form, name As String) As System.Windows.Forms.Form

        Dim ctl As Control = Nothing
        For Each ctl In t_form.Controls
            If ctl.Name = name Then
                Exit For
            End If
        Next
        Return ctl
    End Function
    Dim testTime As DateTime = New DateTime(2016, 12, 29, 23, 59, 0)
    Public test_Reader_ANT_WDT As Reader_ANT_WDT
    Public Property aa As String
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' System.IO.Directory.GetCurrentDirectory()
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

            'Dim a1 As Integer = 9999
            'Console.WriteLine(a1.ToString)
            'Console.WriteLine(a1.ToString("X2"))
            'Dim a2 As Integer = classLibrary_bang.M_Math.HexToD.HexStringToInt(a1.ToString)
            'Console.WriteLine(a2.ToString)
            'Console.WriteLine(a2.ToString("X2"))

            'Dim nowtime As DateTime = Now
            ''Dim nowttostring As String = nowtime.ToString("u")
            'Dim nowttostring As String = nowtime.ToString("yyyy-MM-dd")

            'Console.WriteLine(nowttostring)
            'Dim nowttostring2 As String = Convert.ToDateTime(nowttostring).ToString("u")
            'Console.WriteLine(nowttostring2)

            'Dim s As String() = System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory)
            'For index As Integer = 0 To s.Length - 1
            '    Dim f_info As New System.IO.FileInfo(s(index))
            '    Console.WriteLine(s(index))
            '    Console.WriteLine(f_info.Name.Length.ToString("D2") + f_info.Name)
            'Next
            ''Dim xdStr As String = "FF-F9-17-1C-B7-DF-1F"
            'Dim temp_procBuf(6) As Byte
            'temp_procBuf(0) = &HFF
            'temp_procBuf(1) = &HF9
            'temp_procBuf(2) = &H17
            'temp_procBuf(3) = &H1C
            'temp_procBuf(4) = &HB7
            'temp_procBuf(5) = &HDF
            'temp_procBuf(6) = &H1F
            'Dim shift As Integer = 0
            'Dim timestamp As Integer
            'timestamp += CInt(temp_procBuf(shift + 3))
            'timestamp += (CInt(temp_procBuf(shift + 4)) << 8)
            'timestamp += (CInt(temp_procBuf(shift + 5)) << 16)
            'timestamp += (CInt(temp_procBuf(shift + 6)) << 24)
            'Dim a As DateTime = New DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local).AddSeconds(timestamp)
            'MsgBox(a.ToString)
            ''Dim logFileName As String = System.IO.Directory.GetCurrentDirectory & "\" & CInt(testTime.DayOfWeek).ToString() & testTime.DayOfWeek.ToString() & ".log"
            'Console.WriteLine(Now.ToString("yyyy-MM-dd" + "T" + "HH:mm:ss"))

            'Console.WriteLine("xmlns:xsd=""http://www.w3.org/2001/XMLSchema""")
            'Exit Sub
            ''Try
            ''    Using sw As System.IO.StreamWriter = New System.IO.StreamWriter(logFileName)
            ''        sw.WriteLine(testTime.ToShortDateString)
            ''        sw.Close()
            '    End Using
            'Catch ex As Exception
            '    ' Let the user know what went wrong.
            'End Try
            'test_Reader_ANT_WDT = New Reader_ANT_WDT(1)
            'AddHandler test_Reader_ANT_WDT.event_Status, AddressOf add_event_Status
            'a.resetTime()

            'Dim a As String = "10013000160016004801020000001710180000000075"
            'Dim a As String = "1001370000000d0d480100000000171014000000007c"
            'Dim len As Integer = a.Length / 2
            'Dim a_byte() As Byte

            ''a_byte = ClassLibrary_apple.M_Math.HexStringToBytes(a)
            ''xor

            'Dim xor_value As Byte = a_byte(0)
            'For index As Integer = 1 To a_byte.Length - 2
            '    xor_value = xor_value Xor a_byte(index)

            'Next
            'Console.WriteLine(xor_value.ToString)
            'Console.WriteLine(a_byte(a_byte.Length - 1).ToString)
    End Sub
    Sub add_event_Status(value As Boolean)
        Console.WriteLine(Now.ToString + " " + value.ToString)
    End Sub
    'Dim a As eventClock_sec
    Public udpclient As udpserver_lock_sp
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Console.WriteLine("now " + Now.ToString)
        Dim a As New cycle_run(AddressOf time)

        'Dim udp_par As New Udp_par
        'udp_par.client_ip = "127.0.0.1"
        'udp_par.client_port = 10001
        'udp_par.server_ip = "127.0.0.1"
        'udp_par.server_port = 40002
        'udpclient = New udpserver_lock_sp(udp_par)
        'udpclient.StartProc()

        'delAllTxt()
        'Console.WriteLine(CInt("4.33").ToString)
        'Dim a As New test_seachweb

        'test_Reader_ANT_WDT.Set_getData()
        ''ResetCurrentLog2(testTime)
        'Dim tmp_st As String = ""
        'Dim tmp_end As String = ""
        'Dim play_lst As String = "[4-9-7][10-15-35]"
        'tmp_st = play_lst.Substring(1, play_lst.IndexOf("-") - 1)
        'Console.WriteLine("tmp_st " + tmp_st)
        'play_lst = play_lst.Substring(play_lst.IndexOf("-") + 1, play_lst.Length - play_lst.IndexOf("-") - 1)
        'Console.WriteLine("now play_lst " + play_lst)
        'tmp_end = play_lst.Substring(0, play_lst.IndexOf("-"))
        'Console.WriteLine("tmp_end " + tmp_end)
        'a = New eventClock_sec(60)


        'Dim parameters_SL As New parameters_SL
        'parameters_SL.init()
        'parameters_SL.readFile()
        'parameters_SL.addParameters("a", Now.ToString("u"))
        'parameters_SL.addParameters("b", Now.ToString())
        'parameters_SL.saveFile()
        'parameters_SL.readFile()

        'Dim a As New List(Of Byte)
        'a.Add(&HFF)
        'a.Add(&HF3)
        'a.Add(&H3)
        'a.Add(&H1)
        'a.Add(&H0)
        'a.Add(&H0)
        'Dim check As Byte = Me.Calcchecksum(a.ToArray, 3, 3)
        'a.Add(check)
        'Dim a_text As String = ""
        'For index As Integer = 0 To a.Count - 1
        '    a_text = a_text + "$" + a(index).ToString("X2")
        'Next
        'Console.WriteLine(a_text)

        'Dim a As New sunray_format_FFF3
        'a.setIndex(65534)
        'a.setIndex(3365534)
    End Sub
    Sub time()
        Console.WriteLine(Now.ToString)
    End Sub
    Sub delAllTxt()
        Dim t As String = "D:\專案160126\showroom\北市\VirtualTCSrv\"
        Dim a As New System.IO.DirectoryInfo(t)
        Dim a_DirectoryInfo() As System.IO.DirectoryInfo = a.GetDirectories

        For index As Integer = 0 To a_DirectoryInfo.Length - 1
            'Console.WriteLine(a_DirectoryInfo(index).FullName)
            Me.delTXT(a_DirectoryInfo(index))
        Next
    End Sub

    Sub delTXT(t_DirectoryInfo As System.IO.DirectoryInfo)
        Console.WriteLine(t_DirectoryInfo.FullName)
        Dim file_info() As System.IO.FileInfo = t_DirectoryInfo.GetFiles("*.txt")
        For index As Integer = 0 To file_info.Length - 1
            'Console.WriteLine(file_info(index).FullName)
            file_info(index).Delete()
        Next

        Dim dir_infeo() As System.IO.DirectoryInfo = t_DirectoryInfo.GetDirectories
        For index As Integer = 0 To dir_infeo.Length - 1
            'Console.WriteLine(file_info(index).FullName)
            Me.delTXT(dir_infeo(index))
        Next
    End Sub
    Private Function Calcchecksum(ByRef procBuf As Byte(), ByVal shift As Integer, ByVal length As Integer) As Byte
        Dim checksum As Integer = 0
        Dim i As Integer = 0
        While (i < length)
            checksum = checksum + procBuf(shift + i)
            i += 1
        End While
        checksum = checksum And &HFF

        Return CByte(checksum)
    End Function
    Private Sub ResetCurrentLog2(ByVal submitDate As DateTime)
        If submitDate.Hour = 23 And submitDate.Minute = 59 Then
            Dim delTime As DateTime = submitDate.AddMinutes(1)
            Dim logFileName As String = System.IO.Directory.GetCurrentDirectory & "\" & CInt(submitDate.DayOfWeek).ToString() & submitDate.DayOfWeek.ToString() & ".log"

            If System.IO.File.Exists(logFileName) Then
                System.IO.File.Delete(logFileName)
            End If
        End If

    End Sub

    Function createBitArray(ByVal number As Byte) As BitArray
        Dim tByte() As Byte = {number}
        Dim r_BitArray As New BitArray(tByte)

        Return r_BitArray
    End Function

    Sub json_test()
        '物件(object)用大括號 { }

        '陣列(array)用中括號 [ ]
        Dim s As New Studeent
        s.Id = 12883
        s.Name = "john"
        s.Scores = {87.5, 92, 76.2}

        'Dim text As String = JsonConvert.SerializeObject(s)
        Console.WriteLine(text)
        '{"Id":12883,"Name":"john","Scores":[87.5,92.0,76.2]}
        'Dim Product As deserializedProduct
        'Dim deserializedProduct As Studeent = JsonConvert.DeserializeObject(Of Studeent)(text)
    End Sub
    Class Studeent

        Public Property Id As Integer

        Public Property Name As String

        Public Property Scores As Double()
    End Class
    Sub testnow()
        Console.WriteLine(Now.ToString)
    End Sub

   
End Class

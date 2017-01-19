Imports System.Windows
Imports ClassLibrary_apple
Public Class Form1



    Sub New()

        ' 此為設計工具所需的呼叫。
        InitializeComponent()

        ' 在 InitializeComponent() 呼叫之後加入任何初始設定。
   
    End Sub
    ' Private TC_FaultDetection As New TC_FaultDetection("test01", True)

    Private differentValueRecord As New differentValueRecord("test02", True)
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
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


    End Sub

    Private Delegate Sub MyDelSub()
    Private Sub WriteToDebug()
        Console.WriteLine("Delegate Wrote To Debug Window")
    End Sub

    Private Sub OpenDir_Button_Click(sender As Object, e As EventArgs) Handles OpenDir_Button.Click
        Test.M_test.OpenDir(System.IO.Directory.GetCurrentDirectory)
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
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
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


        ''Try
        ''    Using sw As System.IO.StreamWriter = New System.IO.StreamWriter(logFileName)
        ''        sw.WriteLine(testTime.ToShortDateString)
        ''        sw.Close()
        '    End Using
        'Catch ex As Exception
        '    ' Let the user know what went wrong.
        'End Try
        test_Reader_ANT_WDT = New Reader_ANT_WDT(1)
        AddHandler test_Reader_ANT_WDT.event_Status, AddressOf add_event_Status
    End Sub
    Sub add_event_Status(value As Boolean)
        Console.WriteLine(Now.ToString + " " + value.ToString)
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        test_Reader_ANT_WDT.Set_getData()
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

    End Sub
     
    Private Sub ResetCurrentLog2(ByVal submitDate As DateTime)
        If submitDate.Hour = 23 And submitDate.Minute = 59 Then
            Dim delTime As DateTime = submitDate.AddMinutes(1)
            Dim logFileName As String = System.IO.Directory.GetCurrentDirectory & "\" & CInt(submitDate.DayOfWeek).ToString() & submitDate.DayOfWeek.ToString() & ".log"

            If System.IO.File.Exists(logFileName) Then
                System.IO.File.Delete(logFileName)
            End If
        End If

    End Sub


End Class

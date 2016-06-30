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

        Console.WriteLine(Now.GetHashCode.ToString)
        Dim r As New Random()
        Console.WriteLine(r.Next(0, 10))
        Dim o As New Object
        Console.WriteLine(o.GetHashCode.ToString)
        Console.WriteLine(Guid.NewGuid().GetHashCode())
    End Sub

    Private Delegate Sub MyDelSub()
    Private Sub WriteToDebug()
        Console.WriteLine("Delegate Wrote To Debug Window")
    End Sub

    Private Sub OpenDir_Button_Click(sender As Object, e As EventArgs) Handles OpenDir_Button.Click
        Test.M_test.OpenDir(System.IO.Directory.GetCurrentDirectory)
    End Sub
End Class

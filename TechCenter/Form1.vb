Imports System.Windows
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
        differentValueRecord.setValue("new03", "true")
        System.Threading.Thread.Sleep(10000)
        differentValueRecord.setValue("new03", "true")
        System.Threading.Thread.Sleep(5000)
        differentValueRecord.setValue("new03", "false")
    End Sub


    Private Sub OpenDir_Button_Click(sender As Object, e As EventArgs) Handles OpenDir_Button.Click
        Test.M_test.OpenDir(System.IO.Directory.GetCurrentDirectory)
    End Sub
End Class

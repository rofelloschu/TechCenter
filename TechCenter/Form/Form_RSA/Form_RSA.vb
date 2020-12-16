'20200213
Public Class Form_RSA
    Public core As Form_RSA_core

    Sub New()

        ' 此為設計工具所需的呼叫。
        InitializeComponent()

        ' 在 InitializeComponent() 呼叫之後加入任何初始設定。

    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        core = New Form_RSA_core
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.InitialDirectory = System.IO.Directory.GetCurrentDirectory
        If (OpenFileDialog1.ShowDialog() = DialogResult.OK) Then
            TextBox_key_path.Text = OpenFileDialog1.FileName
        Else

        End If

    End Sub

    Private Sub Button_TypeA_Click(sender As Object, e As EventArgs) Handles Button_TypeA.Click

        Dim key As String = core.readFile(TextBox_key_path.Text)
        Dim data As String = core.run_typeA(key)
        core.saveFile("typea.txt", data)
 
        Dim key2 As String = "<RSAKeyValue><Modulus>rSJmV+aq6BR/InQ0H+qXtLwov8UGB57m+86Umeb+IRPYQ5CAFGY+cttrnqv8agWn8+PbYywXjq/JAsu4uMaqg2R5CnHu79IGbKO2qCu6KNn53gloER3nz2Kk/cJ5C6IcN3eKEYSGEJXKPtwdY7c4ClTY0cEE/oUiRfcZSBViqHE=</Modulus><Exponent>AQAB</Exponent><P>zMEV+gwhZIgI3CVysNM+BKPdz9vrjROrKE4uO8OLOw50X+zsKRe3xSMvLpd16qVgQrQbeGWQun5mkrA3d6GcLw==</P><Q>2HdhlDdRd7tn1Hge2stJe7dvyQYiu1wgCBqRoMc0vR8DVVlV7WqT0+hHI4/t6ay/s6IZcwrKkAIAfPqhI9W9Xw==</Q><DP>QZDK8skToFeXTreHJGxgfafjjX4EzaYwtrViRKz3Vq/oQBdkADyiEjSUFT1W+w595p459bHASJfVTL+041AS7Q==</DP><DQ>rTRA0M6+khZTCzAeSCV3I9XwTSJqsg4R10ojEkmzCkyBZ053MeQgauOl4G+vB1XhgkOHJ4UP9dCUQWxteXXx8w==</DQ><InverseQ>yQ4PtPspYQgeORDe2tFmcgDRo7t0A6qZRyx2t6yKGVUx/dXQvt4M2d2m5Pv0iG3pwHwA05bPiIkbPyT0xqGngw==</InverseQ><D>IOPiEVzOptwN8tukc1O7kigUjam+JYB4XOm3rS0Gpf4BHYS78CQkROmOHkyJ3RJJ/7kPml1r9D1MsG1HIEMNbSyJ9d2uXETLz9U0tOmhlhtSVhoqOyuWayWn8BJYHKOhqWgcPbGmoatqCuq6guv1xXpC0cx5OjEPQysv/ZlprEk=</D></RSAKeyValue>"
        Console.WriteLine("typeA " + core.dec_typeA("typea.txt", key2).ToString)
    End Sub

    Private Sub Button_create_Click(sender As Object, e As EventArgs) Handles Button_create.Click
        core.create()
        MsgBox("創造完成")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        core.create(TextBox_key_name.Text)
        MsgBox("創造完成")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        '轉換依照UTC
        Dim a As String = Now.ToUniversalTime.ToString("u")
        Console.WriteLine(a)
        Dim t As DateTime = Convert.ToDateTime(a)
        t = t.AddHours(-8)
        Console.WriteLine(t.ToString("u"))

    End Sub
End Class


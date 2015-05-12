'20150512
Namespace WinForm
    Public Class FormPlus
        Private form As System.Windows.Forms.Form
        Sub New(t_form As System.Windows.Forms.Form)
            form = t_form
        End Sub


        Sub StartHide()
            '啟動時隱藏 內容設定順序重要 執行必須擺最後
            form.WindowState = System.Windows.Forms.FormWindowState.Minimized
            form.ShowInTaskbar = False
        End Sub

        Sub NotifyIcon()

        End Sub
    End Class
End Namespace


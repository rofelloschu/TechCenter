'20161122
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


        Public Shared Function getControlNames(t_Control As System.Windows.Forms.Control, Optional test As Boolean = False) As String()

            Dim return_list As New List(Of String)
            Dim ctl As System.Windows.Forms.Control
            For Each ctl In t_Control.Controls
                If test Then
                    Console.WriteLine(ctl.Name)
                End If
                return_list.Add(ctl.Name)
            Next
            Return return_list.ToArray
        End Function
        Public Shared Function getFormControl(t_Control As System.Windows.Forms.Control, name As String) As System.Windows.Forms.Control
            Dim return_Control As System.Windows.Forms.Control = Nothing

            For Each ctl As System.Windows.Forms.Control In t_Control.Controls
                If ctl.Name = name Then
                    return_Control = ctl
                    Exit For
                End If
            Next
            Return return_Control
        End Function
    End Class
End Namespace


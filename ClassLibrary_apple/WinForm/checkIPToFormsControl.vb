'20150508
'驗證IP用
Namespace WinForm
    Public Class checkIPToFormsControl
        Private m_FormsControl As System.Windows.Forms.Control
        Private m_checkButton As System.Windows.Forms.Button
        'IP格式是否正確
        Private isCheckIP As Boolean
        '  Private m_ip As String
        Sub New(t_FormsControl As System.Windows.Forms.Control, m_button As System.Windows.Forms.Button)
            'Dim a As System.Windows.Forms.TextBox = New System.Windows.Forms.TextBox
            '' directcast(a
            ''   DirectCast(a, System.Windows.Forms.Control)
            'DirectCast(a, System.Windows.Forms.Control).Text = ""
            Me.m_FormsControl = t_FormsControl
            Me.m_checkButton = m_button
            Me.isCheckIP = False
            AddHandler m_checkButton.Click, AddressOf CheckIP_Button_Click
            AddHandler m_FormsControl.TextChanged, AddressOf IP_Text_TextChanged



        End Sub
        Function getIP() As String
            If Me.isCheckIP Then
                Return Me.m_FormsControl.Text
            Else
                MsgBox("IP未驗證")
                Return Nothing
            End If
        End Function
        Function check() As Boolean
            Return Me.isCheckIP
        End Function
        Private Sub CheckIP_Button_Click(sender As Object, e As EventArgs) 'Handles m_checkButton.Click
            DirectCast(sender, System.Windows.Forms.Button).Enabled = False
            Try
                If IsIP(m_FormsControl.Text) Then
                    'Me.m_ip = m_FormsControl.Text
                    Me.isCheckIP = True
                Else

                    DirectCast(sender, System.Windows.Forms.Button).Enabled = True
                    MsgBox("輸入的IP格式錯誤")
                End If
            Catch ex As Exception
                DirectCast(sender, System.Windows.Forms.Button).Enabled = True
            End Try

        End Sub

        Private Sub IP_Text_TextChanged(sender As Object, e As EventArgs) 'Handles IP_TextBox.TextChanged
            Me.m_checkButton.Enabled = True
            Me.isCheckIP = False

        End Sub

        Protected Overridable Function IsIP(ByVal IP As String) As Boolean
            Return System.Text.RegularExpressions.Regex.IsMatch(IP, "\b((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$\b")

        End Function
    End Class
End Namespace


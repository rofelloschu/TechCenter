Public Class PictureBox_plus
    Public PictureBox As System.Windows.Forms.PictureBox
    Public number As Integer = 0
    Public pic_path As String = ""
    Sub New(t_PictureBox As System.Windows.Forms.PictureBox, t_index As Integer)
        PictureBox = t_PictureBox
        number = t_index
        'AddHandler PictureBox.Click, AddressOf PictureBox_Click
        AddHandler PictureBox.DoubleClick, AddressOf PictureBox_Click
    End Sub

#Region "圖"
    Private Sub PictureBox_Click(sender As Object, e As EventArgs)
        If pic_path = "" Then
            'Dim filePath As String
            Using ofd As New OpenFileDialog
                ofd.InitialDirectory = System.IO.Directory.GetCurrentDirectory
                'ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
                ofd.Filter = "bmp|*.bmp|All files|*.*"
                ofd.FilterIndex = 2
                ofd.RestoreDirectory = True
                If ofd.ShowDialog = DialogResult.OK Then
                    pic_path = ofd.FileName
                End If
            End Using

            ''Console.WriteLine(filePath)
            RaiseEvent updata_picpath(Me)
        Else
            pic_path = ""
            RaiseEvent clear_picpath(Me)
        End If

    End Sub
    Event updata_picpath(t_p As PictureBox_plus)
    Event clear_picpath(t_p As PictureBox_plus)
#End Region

End Class
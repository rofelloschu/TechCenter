'20150422
Namespace WinForm
    Public Class LogTextBox_V02
        Delegate Sub SetTextArrayCallback(ByVal textbox As System.Windows.Forms.TextBox, ByVal text As String())
        Delegate Sub SetTextBoxSelectionStart_MaxCallback()
        Private t_textbox As System.Windows.Forms.TextBox
        Private mList As IO.mutexList_V02(Of String)
        Private gsMaxCount As Integer
        Private form As System.Windows.Forms.Form
        Private log As IF_logfile
        Private m_isWrtieLog As Boolean
        Sub New(ByVal textbox As System.Windows.Forms.TextBox)
            m_isWrtieLog = False
            MaxCount = 255
            mList = New IO.mutexList_V02(Of String)
            Me.form = textbox.FindForm
            Me.t_textbox = textbox
            Me.log = New logFile_undone(Me.t_textbox.Name + "_log", True)
            If Me.t_textbox.Lines.Length > 0 Then
                For index As Integer = 0 To Me.t_textbox.Lines.Length - 1
                    mList.Add(Me.t_textbox.Lines(index))
                Next

            End If
        End Sub

        Public Property isWrtieLog() As Boolean
            Get
                Return Me.m_isWrtieLog
            End Get
            Set(ByVal value As Boolean)
                Me.m_isWrtieLog = value
            End Set
        End Property
        Public Sub setLogDirectory(ByVal DirectoryPath As String)
            ' Me.log = New IF_logfile(Me.t_textbox.Name + "_log", True, DirectoryPath)
        End Sub


        Sub add(ByVal text As String)
            mList.Add(text)
            ' Console.WriteLine("LogTextBox " + mList.Count.ToString)
            If m_isWrtieLog Then
                ' Me.log.Writte(Now.ToString + " " + text)
                Me.log.writeLine(Now.ToString + " " + text)
            End If

            updata()
        End Sub
        Sub updata()
            '訊息顯示在視窗
            't_textbox.Lines = mList.ToArray
            setTestboxArray(t_textbox, mList.ToArray)
            'form.setTestboxArray(mList.ToArray)
            '最多顯示255行
            While mList.Count > MaxCount
                mList.getFirstValue()
            End While
            '捲軸置底方式
            Me.setTextBoxSelectionStart_Max()
            ' t_textbox.SelectionStart = t_textbox.Text.Length
            't_textbox.ScrollToCaret()
        End Sub
        Sub clear()
            mList.Clear()
            updata()
        End Sub
        Sub close()
            Me.Finalize()
        End Sub
        Public Property MaxCount() As Integer
            Get
                Return gsMaxCount
            End Get
            Set(ByVal value As Integer)
                gsMaxCount = value
            End Set

        End Property

        Sub setTextBoxSelectionStart_Max()
            If t_textbox.InvokeRequired Then

                form.Invoke(New SetTextBoxSelectionStart_MaxCallback(AddressOf setTextBoxSelectionStart_Max), New Object() {})
            Else
                t_textbox.SelectionStart = t_textbox.Text.Length
                t_textbox.ScrollToCaret()
            End If
        End Sub
        Sub setTestboxArray(ByVal textbox As System.Windows.Forms.TextBox, ByVal text As String())
            If textbox.InvokeRequired Then

                form.Invoke(New SetTextArrayCallback(AddressOf setTestboxArray), New Object() {textbox, text})
            Else
                textbox.Lines = text
            End If

        End Sub
        Function getLogLines() As String()
            Return mList.ToArray
        End Function
    End Class
End Namespace


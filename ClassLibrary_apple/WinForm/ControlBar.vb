'20150507
'Public Class ControlToolStripProgressBar
'    Private Shared Instance As ControlToolStripProgressBar
'    Private myBar As System.Windows.Forms.ToolStripProgressBar
'    Private mylabel As System.Windows.Forms.ToolStripLabel
'    ' Private FormMain As FormMain


'    Sub New()

'    End Sub
'    Sub New(ByVal tbar As System.Windows.Forms.ToolStripProgressBar, ByVal tlabel As System.Windows.Forms.ToolStripLabel, ByVal tFormMain As FormMain)
'        If Instance Is Nothing Then
'            Instance = New ControlToolStripProgressBar
'            Instance.myBar = tbar
'            Instance.mylabel = tlabel
'            Instance.FormMain = tFormMain

'            Instance.myBar.Minimum = 0
'            Instance.myBar.Maximum = 0
'            Instance.myBar.Value = 0
'            Instance.myBar.Step = 1
'        End If
'    End Sub
'    Shared Function getInstance() As ControlToolStripProgressBar

'        Return Instance

'    End Function
'    Private Sub setObject(ByVal tbar As System.Windows.Forms.ToolStripProgressBar, ByVal tlabel As System.Windows.Forms.ToolStripLabel)
'        Instance.myBar = tbar
'        Instance.mylabel = tlabel
'    End Sub

'    '設定bar運作
'    Shared Sub BarPerformStep()
'        ' Bar+1
'        Instance.myBar.PerformStep()
'        Instance.mylabel.Text = Instance.myBar.Value & "/" & Instance.myBar.Maximum
'        If Instance.myBar.Value = Instance.myBar.Maximum Then
'            FormMain.ToolStrip1.Enabled = True
'            FormMain.TopDGV.UnlockButten()
'        Else
'            FormMain.ToolStrip1.Enabled = False
'            FormMain.TopDGV.LockButten()
'        End If
'    End Sub
'    Shared Sub setBarDefault(ByVal max As Integer)
'        Try
'            Instance.myBar.Minimum = 0
'            Instance.myBar.Maximum = max
'            Instance.myBar.Value = 0
'            Instance.myBar.Step = 1
'        Catch ex As Exception
'            Instance.myBar.Minimum = 0
'            Instance.myBar.Maximum = 0
'            Instance.myBar.Value = 0
'            Instance.myBar.Step = 1
'        End Try
'        Instance.mylabel.Text = Instance.myBar.Value & "/" & Instance.myBar.Maximum
'    End Sub

'    Shared Sub setBarValue(ByVal value As Integer)
'        Instance.myBar.Value = value
'        Instance.mylabel.Text = Instance.myBar.Value & "/" & Instance.myBar.Maximum
'    End Sub
'    Shared Sub AddBarMax()
'        Try
'            Instance.myBar.Maximum = Instance.myBar.Maximum + 1
'            FormMain.ToolStrip1.Enabled = False
'        Catch ex As Exception

'        End Try
'        Instance.mylabel.Text = Instance.myBar.Value & "/" & Instance.myBar.Maximum
'    End Sub
'    Shared Sub ClearBar()
'        Instance.myBar.Minimum = 0
'        Instance.myBar.Maximum = 0
'        Instance.myBar.Value = 0
'    End Sub
'    Shared Function isBarMaximum() As Boolean
'        If Instance.myBar.Value = Instance.myBar.Maximum Then
'            Return True
'        Else
'            Return False
'        End If

'    End Function
'End Class

Namespace WinForm


    Public Class ControlProgressBar
        Implements Bar_IF

        Private Bar As System.Windows.Forms.ProgressBar
        Private Form As System.Windows.Forms.Form
        Sub New(ByVal tform As System.Windows.Forms.Form, ByVal tBar As System.Windows.Forms.ProgressBar)
            Me.Form = tform
            Me.Bar = tBar

        End Sub
        Delegate Sub setMaximumCallback(ByVal Maximum As Integer)
        Public Sub setMaximum(ByVal Maximum As Integer) Implements Bar_IF.RestartMaximum

            If Me.Bar.InvokeRequired Then
                Me.Form.Invoke(New setMaximumCallback(AddressOf setMaximum), New Object() {Maximum})
            Else
                Me.Bar.Maximum = Maximum
                Me.Bar.Minimum = 0
                Me.Bar.Value = Me.Bar.Minimum
            End If
        End Sub
        Delegate Function getMinimumCaretCallback() As Integer
        Public Function getValue() As Integer Implements Bar_IF.getValue

            If Me.Bar.InvokeRequired Then
                '不確定是否可return
                Return Me.Form.Invoke(New getMinimumCaretCallback(AddressOf getValue), New Object() {})
            Else
                Return Me.Bar.Value
            End If



        End Function
        Delegate Sub BarPerformStepCaretCallback()
        Public Sub BarPerformStep() Implements Bar_IF.BarPerformStep
            If Me.Bar.InvokeRequired Then
                Me.Form.Invoke(New BarPerformStepCaretCallback(AddressOf BarPerformStep), New Object() {})
            Else
                Me.Bar.Value = Me.Bar.Value + 1
            End If

        End Sub

        'Public Sub BarPerformStep1() Implements Bar_IF.BarPerformStep

        'End Sub
    End Class
    Interface Bar_IF
        Sub BarPerformStep()
        Function getValue() As Integer
        Sub RestartMaximum(ByVal Max As Integer)
    End Interface
End Namespace
'20150511
'偵測錯誤
Public Class TC_FaultDetection
    Private m_isRunwrite As Boolean
    Private m_name As String
    Private ValueDiction As Dictionary(Of String, String)

    Private AutoResetEvent As New System.Threading.AutoResetEvent(True)

    Sub New(t_name As String, t_isrunwrite As Boolean)
        m_name = t_name
        m_isRunwrite = t_isrunwrite
        ValueDiction = New Dictionary(Of String, String)
    End Sub
    Sub close()
        ValueDiction.Clear()
        ValueDiction = Nothing
    End Sub
    '設定值
    Function setValue(name As String, t_setValue As String) As String
        AutoResetEvent.WaitOne()
        If ValueDiction.ContainsKey(name) Then
            ValueDiction(name) = t_setValue
            write_setValue(name + " = " + t_setValue)
            AutoResetEvent.set()
            Return "setValue"
        Else
            ValueDiction.Add(name, t_setValue)
            write_newValue(name + " = " + t_setValue)
            AutoResetEvent.set()
            Return "newValue"
        End If
    End Function
    '比較值
    Function thanValue(name As String, t_getvalue As String) As String
        AutoResetEvent.WaitOne()
        If ValueDiction.ContainsKey(name) Then
            If ValueDiction(name) = t_getvalue Then
                write_thesame(name + " 相同")
                AutoResetEvent.set()
                Return "thesame"
            Else
                write_different(name + " 不相同" + " 設定=" + ValueDiction(name) + " " + "比較=" + t_getvalue)
                AutoResetEvent.set()
                Return "different"
            End If
        Else
            write_nothing(name + " 無值")
            AutoResetEvent.set()
            Return "nothing"
        End If
    End Function
    Private Sub wtiteFile(Data As String)
        If m_isRunwrite Then
            System.IO.File.AppendAllText(m_name + ".txt", Data)
        End If

    End Sub
    Protected Overridable Sub write_different(msg As String)
        Me.wtiteFile(Now.ToString("u") + " " + "[than different] " + msg.ToString + vbCrLf)
    End Sub
    Protected Overridable Sub write_thesame(msg As String)
        Me.wtiteFile(Now.ToString("u") + " " + "[than thesame] " + msg.ToString + vbCrLf)
    End Sub
    Protected Overridable Sub write_nothing(msg As String)
        Me.wtiteFile(Now.ToString("u") + " " + "[than nothing] " + msg.ToString + vbCrLf)
    End Sub
    Protected Overridable Sub write_setValue(msg As String)
        Me.wtiteFile(Now.ToString("u") + " " + "[setValue] " + msg.ToString + vbCrLf)
    End Sub
    Protected Overridable Sub write_newValue(msg As String)
        Me.wtiteFile(Now.ToString("u") + " " + "[newValue] " + msg.ToString + vbCrLf)
    End Sub
End Class

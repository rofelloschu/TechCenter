'20150511
'偵測錯誤
Public Class TC_FaultDetection
    Private m_isRun As Boolean
    Private m_name As String
    Private ValueDiction As Dictionary(Of String, String)

    Sub New(t_name As String, t_isrun As Boolean)
        m_name = t_name
        m_isRun = t_isrun
        ValueDiction = New Dictionary(Of String, String)
    End Sub
    Sub close()
        ValueDiction.Clear()
        ValueDiction = Nothing
    End Sub
    '設定值
    Sub setValue(name As String, setValue As String)
        If ValueDiction.ContainsKey(name) Then
            ValueDiction(name) = setValue
            write_setValue(name + " = " + setValue)
        Else
            ValueDiction.Add(name, setValue)
            write_newValue(name + " = " + setValue)
        End If
    End Sub
    '比較值
    Sub thanValue(name As String, getvalue As String)
        If ValueDiction.ContainsKey(name) Then
            If ValueDiction(name) = getvalue Then
                write_thesame(name + " 相同")
            Else
                write_different(name + " 不相同" + " 設定=" + ValueDiction(name) + " " + "比較=" + getvalue)
            End If
        Else
            write_nothing(name + " 無值")
        End If
    End Sub
    Private Sub wtiteFile(Data As String)
        If m_isRun Then
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

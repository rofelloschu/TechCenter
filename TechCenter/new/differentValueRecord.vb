'20150511
'紀錄不一樣的值
Public Class differentValueRecord
    Private m_isRunwrite As Boolean
    Private m_name As String
    Private ValueDiction As Dictionary(Of String, String)
    Private settimeDiction As Dictionary(Of String, DateTime)

    Private AutoResetEvent As New System.Threading.AutoResetEvent(True)
    Sub New(t_name As String, t_isrunwrite As Boolean)
        m_name = t_name
        m_isRunwrite = t_isrunwrite
        ValueDiction = New Dictionary(Of String, String)
        settimeDiction = New Dictionary(Of String, DateTime)
    End Sub
    Sub close()
        ValueDiction.Clear()
        ValueDiction = Nothing
        settimeDiction.Clear()
        settimeDiction = Nothing
    End Sub
    '相同值回傳true 
    Function setValue(name As String, t_setValue As String) As Boolean
        AutoResetEvent.WaitOne()
        Dim results As Boolean = False

        If ValueDiction.ContainsKey(name) Then
            If ValueDiction(name) = t_setValue Then
                results = True
            Else
                '1
                Dim endtime As DateTime = Now
                write_End(name + " = " + ValueDiction(name) + " " + settimeDiction(name).ToString("u") + "~~~" + endtime.ToString("u"))

                '2
                ValueDiction(name) = t_setValue
                write_Start(name + " = " + t_setValue)
                settimeDiction(name) = Now
            End If
            ValueDiction(name) = t_setValue


        Else
            ValueDiction.Add(name, t_setValue)
            settimeDiction.Add(name, Now)
            write_Start(name + " = " + t_setValue)


        End If
        AutoResetEvent.Set()
        Return results
    End Function


    Private Sub wtiteFile(Data As String)
        If m_isRunwrite Then
            System.IO.File.AppendAllText(m_name + ".txt", Data)
        End If

    End Sub
    Protected Overridable Sub write_Start(msg As String)
        Me.wtiteFile(Now.ToString("u") + " " + "[Start] " + msg.ToString + vbCrLf)
    End Sub
    Protected Overridable Sub write_End(msg As String)
        Me.wtiteFile(Now.ToString("u") + " " + "[End] " + msg.ToString + vbCrLf)
    End Sub
End Class

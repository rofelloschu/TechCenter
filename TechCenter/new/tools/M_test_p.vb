Public Module M_test_p


    'test
    Public writeLog As Boolean

    Sub writeTolog(filename As String, text As String)
        If M_log.debug_log Then
            M_log.writeline(filename, text)
        End If

    End Sub
End Module

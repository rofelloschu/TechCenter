'20180316
'ex delayRun.run(AddressOf Me.SetSunRayTime, 30)
'設定30秒後執行
Public Class delayRun
    Delegate Sub run_sub()
    Sub New()

    End Sub
    Shared Sub run(t_sub As run_sub, t_delay_sec As Integer)
    
        Dim r_thread As New System.Threading.Timer(AddressOf AddressOf_DRun, t_sub, t_delay_sec * 1000, System.Threading.Timeout.Infinite)

    End Sub
    Shared Sub AddressOf_DRun(ByVal stateInfo As Object)

        DirectCast(stateInfo, run_sub)

    End Sub
End Class

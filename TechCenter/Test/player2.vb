Public Class player2
    '拉出wmp
    '參考多AxInterop.WMPLib.dll
    'Interop.WMPLib.dll
    ' AxWindowsMediaPlayer1 As AxWMPLib.AxWindowsMediaPlayer
    Public AWMP As AxWMPLib.AxWindowsMediaPlayer
    Sub New(t_AWMP As AxWMPLib.AxWindowsMediaPlayer)
        AWMP = t_AWMP
        AWMP.settings.volume = 100

    End Sub
    '可使用自動循環 來保證撥放秒數'axWindowsMediaPlayer1.Ctlcontrols.AutoRewind
    Sub play(path As String)
        AWMP.URL = path
        'AxWindowsMediaPlayer1.URL = "D:\專案190617\公總\公總Event_Alarm\Event_Alarm\test\我可能不會愛你.mp3"
        'Console.WriteLine(AWMP.isOnline.ToString)
        'Console.WriteLine(AWMP.IsAccessible.ToString)
        'Console.WriteLine(AWMP.Ctlcontrols.currentPositionString)
        'Console.WriteLine(AWMP.currentMedia.getItemInfo("Duration"))

        If AWMP.playState = 3 Then
            AWMP.Ctlcontrols.stop()

        End If
        AWMP.currentPlaylist.clear()
        AWMP.Ctlcontrols.play()
        Console.WriteLine(AWMP.currentMedia.durationString.ToString)

    End Sub
    Sub play(path_array As String())

        If AWMP.playState = 3 Then
            AWMP.Ctlcontrols.stop()

         
        End If
        AWMP.currentPlaylist.clear()
        Dim m_path As String
        For index As Integer = 0 To path_array.Length - 1
            m_path = path_array(index)
            If System.IO.File.Exists(m_path) Then
                AWMP.currentPlaylist.appendItem(AWMP.newMedia(m_path))
            Else

            End If

        Next
        AWMP.Ctlcontrols.play()

        'AWMP.URL = path
        ''AxWindowsMediaPlayer1.URL = "D:\專案190617\公總\公總Event_Alarm\Event_Alarm\test\我可能不會愛你.mp3"
        ''Console.WriteLine(AWMP.isOnline.ToString)
        ''Console.WriteLine(AWMP.IsAccessible.ToString)
        ''Console.WriteLine(AWMP.Ctlcontrols.currentPositionString)
        ''Console.WriteLine(AWMP.currentMedia.getItemInfo("Duration"))
        'If AWMP.playState = 3 Then
        '    AWMP.Ctlcontrols.stop()
        'End If
        'AWMP.Ctlcontrols.play()
        'Console.WriteLine(AWMP.currentMedia.durationString.ToString)


    End Sub
    Sub s_stop()
        AWMP.Ctlcontrols.stop()
    End Sub
    Private start_AutoRewind_max_sec As Integer = 60
    Private t_AutoRewind As System.Threading.Thread
    Public Function isPlay() As Boolean
        If AWMP.playState = WMPPlayState.wmppsPlaying Then
            Return True
        Else
            Return False
        End If




    End Function

#Region "重複撥放?待測"
    Sub start_AutoRewind(sec As Integer)
        If t_AutoRewind.IsAlive Then
            Try
                t_AutoRewind.Abort()
            Catch ex As Exception

            End Try
        End If
        t_AutoRewind = New System.Threading.Thread(AddressOf AddressOf_AutoRewind)
        t_AutoRewind.Start(sec)
        GC.Collect()
    End Sub
    Sub AddressOf_AutoRewind(sec As Integer)
        AWMP.Ctlcontrols.AutoRewind = True
        If sec > start_AutoRewind_max_sec Then
            Threading.Thread.Sleep(1000 * start_AutoRewind_max_sec)
        Else
            Threading.Thread.Sleep(1000 * sec)
        End If

        AWMP.Ctlcontrols.AutoRewind = False
    End Sub
#End Region

End Class
'https://sites.google.com/site/willsnote/123
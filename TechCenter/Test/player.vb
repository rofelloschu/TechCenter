Imports System.Windows.Media
Imports System.Windows
'參考PresentationCore.dll
'參考WindowsBase.dll
'https://docs.microsoft.com/zh-tw/dotnet/framework/wpf/graphics-multimedia/how-to-play-media-using-a-videodrawing
Imports WMPLib
'20200108測試失敗
Public Class player


    Sub test()
        Dim player As New MediaPlayer
        'Dim player As New Media.SoundPlayer

        player.Open(New Uri("D:\專案190617\子彈列車拉麵\子彈列車加盟影片.mp4", UriKind.RelativeOrAbsolute))
        Dim aVideoDrawing As New VideoDrawing()
        aVideoDrawing.Rect = New System.Windows.Rect(0, 0, 100, 100)

        aVideoDrawing.Player = player

        '/ Play the video once.
        player.Play()

    End Sub
    Sub test2()
        My.Computer.Audio.Play("D:\專案190617\子彈列車拉麵\子彈列車加盟影片.mp4", AudioPlayMode.Background)
    End Sub
    Sub test3()
        Dim WMP1 As New WindowsMediaPlayer
        'WMPLib.URL = "D:\專案190617\子彈列車拉麵\子彈列車加盟影片.mp4"
    End Sub
End Class

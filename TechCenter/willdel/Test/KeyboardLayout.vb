'20160321
'輸入法偵測
Public Class KeyboardLayout
    'http://tc.wangchao.net.cn/bbs/detail_21544.html
    Private Declare Function GetKeyboardLayoutList Lib "user32" (ByVal nBuff As Long, lpList As Long) As Long

    Private Declare Function ImmGetDescription Lib "imm32.dll" Alias "ImmGetDescriptionA" (ByVal hkl As Long, ByVal lpsz As String, ByVal uBufLen As Long) As Long

    Private Declare Function ImmIsIME Lib "imm32.dll" (ByVal hkl As Long) As Long

    Private Declare Function ActivateKeyboardLayout Lib "user32" (ByVal hkl As Long, ByVal flags As Long) As Long

    Private Declare Function GetKeyboardLayout Lib "user32" (ByVal dwLayout As Long) As Long

    Private Declare Function GetKeyboardLayoutName Lib "user32" Alias "GetKeyboardLayoutNameA" (ByVal pwszKLID As String) As Long

    Private Declare Function LoadKeyboardLayout Lib "user32" Alias "LoadKeyboardLayoutA" (ByVal pwszKLID As String, ByVal flags As Long) As Long


    'http://www.vincent.com.tw/vb/SVBR2.0/winapi/SVBR.WINAPI.activatekeyboardlayout.html
    'Private Declare Function ActivateKeyboardLayout2 Lib "user32" (ByVal HKL As Integer, ByVal flags As Integer) As Integer
    'http://www.programmer-club.com.tw/ShowSameTitleN/csharp/6268.html
    'https://dotblogs.com.tw/optimist9266/2011/06/06/27194
    'http://www.blueshop.com.tw/board/FUM20050124191756KKC/BRD200608161501231EA.html
    Sub New()

    End Sub
    Shared Sub setKeyboardLayoutEng()

    End Sub
    Private Declare Function ImmGetContext Lib "imm32.dll" (ByVal hwnd As Integer) As Integer
    Private Declare Function ImmGetConversionStatus Lib "imm32.dll" (ByVal himc As Integer, ByRef lpdw As Integer, ByRef lpdw2 As Integer) As Integer
    Private Declare Function ImmSimulateHotKey Lib "imm32.dll" (ByVal hwnd As Integer, ByVal dw As Integer) As Integer

    Public Shared Sub ControlIME(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lngStatus As Long, lngSt As Long, h As IntPtr = sender.Handle
        ImmGetConversionStatus(ImmGetContext(h), lngStatus, lngSt)
        If lngStatus = 9 Then ImmSimulateHotKey(h, 113)
    End Sub

End Class

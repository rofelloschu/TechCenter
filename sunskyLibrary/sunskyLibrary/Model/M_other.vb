'20140227
Public Module M_other
    'private string AbolutePath(string relativePath, string absoluteTo) {
    '    string[] relativeDir = relativePath.Split('\\');
    '    string[] absoluteDir = absoluteTo.Split('\\');
    '    string absolutePath = "";
    '    /* 把 ..\ 配對到絕對路徑中 */
    '    for (int index = 0; index < relativeDir.Length; index++) {
    '        if (relativeDir[index].CompareTo("..") != 0) {
    '            int i = absoluteDir.Length - index; // 停止點
    '            for (int j = 0; j < i; j++)
    '                absolutePath += absoluteDir[j] + "\\";

    '            /* 把剩下的路徑填滿 */
    '            while (index < relativeDir.Length) {
    '                absolutePath += relativeDir[index] + "\\";
    '                index++;
    '            }
    '            absolutePath = absolutePath.Substring(0, absolutePath.Length - 1); // 去除最後的 \
    '            break;
    '        }
    '    }
    '    return absolutePath;
    <ObsoleteAttribute("20140725 不再使用", True)> _
    Sub TestObsoleteAttribute()

    End Sub
    '相對路徑轉絕對路徑
    '相對路徑relative
    '絕對路徑absoluteTo
    Public Function AbolutePath(ByVal relativePath As String, ByVal absoluteTo As String) As String

        Dim relativeDir As String() = relativePath.Split("\")
        Dim absoluteDir As String() = absoluteTo.Split("\")
        Dim absolutePath As String = relativePath
        '/* 把 ..\ 配對到絕對路徑中 */
        For index As Integer = 0 To relativeDir.Length - 1
            If Not relativeDir(index).Equals("..") Then
                absolutePath = ""
                Dim i As Integer = absoluteDir.Length - index '// 停止點
                For index2 As Integer = 0 To i - 1
                    absolutePath += absoluteDir(index2) + "\"
                Next
                '/* 把剩下的路徑填滿 */
                While index < relativeDir.Length
                    absolutePath += relativeDir(index) + "\"
                    index += 1

                End While
                absolutePath = absolutePath.Substring(0, absolutePath.Length - 1) '// 去除最後的 \
                Exit For
            End If
        Next
        Return absolutePath
    End Function


    '絕對路徑轉相對路徑
    '    private string RelativePath(string absolutePath, string relativeTo)
    '{
    '    string[] absoluteDirectories = absolutePath.Split('\\');
    '    string[] relativeDirectories = relativeTo.Split('\\');//Get the shortest of the two paths 
    '    int length = absoluteDirectories.Length < relativeDirectories.Length ? absoluteDirectories.Length : relativeDirectories.Length;//Use to determine where in the loop we exited 
    '    int lastCommonRoot = -1;
    '    int index; //Find common root 
    '    for (index = 0; index < length; index++)
    '        if (absoluteDirectories[index] == relativeDirectories[index])
    '            lastCommonRoot = index;
    '        else
    '            break;//If we didn't find a common prefix then throw 
    '    if (lastCommonRoot == -1)
    '        throw new ArgumentException("Paths do not have a common base");//Build up the relative path 
    '    StringBuilder relativePath = new StringBuilder(); //Add on the .. 
    '    for (index = lastCommonRoot + 1; index < absoluteDirectories.Length; index++)
    '        if (absoluteDirectories[index].Length > 0)
    '            relativePath.Append("..\\"); //Add on the folders 
    '    for (index = lastCommonRoot + 1; index < relativeDirectories.Length - 1; index++)
    '        relativePath.Append(relativeDirectories[index] + "\\");
    '    relativePath.Append(relativeDirectories[relativeDirectories.Length - 1]);
    '    return relativePath.ToString();
    '}

    Public Function getZWSP() As Char()
        Dim all_whitespaces As Char() = {Chr(&H20), Chr(&H1680), Chr(&H180E), Chr(&H2000), Chr(&H2001), Chr(&H2002), Chr(&H2003), _
       Chr(&H2004), Chr(&H2005), Chr(&H2006), Chr(&H2007), Chr(&H2008), Chr(&H2009), Chr(&H200A), _
        Chr(&H202F), Chr(&H205F), Chr(&H3000), _
 Chr(&H2028), _
 Chr(&H2029), _
  Chr(&H9), Chr(&HA), Chr(&HB), Chr(&HC), Chr(&HD), Chr(&H85), Chr(&HA0), _
  Chr(&H200B), Chr(&HFEFF)}
        Return all_whitespaces

        '// SpaceSeparator category
        '       '\u0020', '\u1680', '\u180E', '\u2000', '\u2001', '\u2002', '\u2003', 
        '       '\u2004', '\u2005', '\u2006', '\u2007', '\u2008', '\u2009', '\u200A', 
        '       '\u202F', '\u205F', '\u3000',
        '   // LineSeparator category
        '       '\u2028',
        '   // ParagraphSeparator category
        '       '\u2029',
        '   // Latin1 characters
        '       '\u0009', '\u000A', '\u000B', '\u000C', '\u000D', '\u0085', '\u00A0',
        '   // ZERO WIDTH SPACE (U+200B) & ZERO WIDTH NO-BREAK SPACE (U+FEFF)
        '       '\u200B', '\uFEFF'

    End Function
#Region "clossMsg"
    Sub MsgboxAndcloseMsg(ByVal text As String)
        closeMsg()
        MsgBox(text)
    End Sub
    Sub closeMsg()
        Dim Thread_closeMsg As New Threading.Thread(AddressOf M_other.AddressOf_closeMsg)
        Dim proName As String
        Dim myAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly
        proName = myAssembly.FullName.Split(",")(0)
        Thread_closeMsg.Start(proName)
    End Sub
    Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
    Private Declare Function PostMessage Lib "user32" Alias "PostMessageA" (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Private Sub AddressOf_closeMsg(ByVal text As Object)
        Threading.Thread.Sleep(5000)
        Dim hWnd As Integer
        hWnd = FindWindow(vbNullString, text.ToString)
        If hWnd Then
            PostMessage(hWnd, &H10, 0&, 0&)
        End If
    End Sub
#End Region

End Module
Public Module ManagedThread
    Sub Id()
        M_WriteLineMaster.WriteLine("ManagedThreadId " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString)
    End Sub
End Module
'計算FPS
Public Module myImage
    Private LastTick As Long
    Private upDateTime As Integer
    Private Frames As Integer
    '1 sec= 10000000 Tick
    Sub New()
        LastTick = Now.Ticks
        upDateTime = 10000000
        Frames = 0
    End Sub

    Function getFPS() As Integer
        Frames = Frames + 1
        Dim FPS As Integer = 255
        Dim subTick As Long = Now.Ticks - LastTick
        If subTick > upDateTime Then
            FPS = (Frames * upDateTime) \ subTick
            LastTick = Now.Ticks
            Frames = 0
        End If
        Return FPS
    End Function
End Module
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Module Version
    Function GetVersionInfo(ByVal filePath As String) As System.Diagnostics.FileVersionInfo
        Return System.Diagnostics.FileVersionInfo.GetVersionInfo(filePath)
    End Function
    Function GetMeVersionInfo() As String
        Return My.Application.Info.Version.ToString()
    End Function
End Module

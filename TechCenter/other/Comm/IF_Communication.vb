'20140227
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Public Interface IF_Communication
    Sub start()
    Sub close()


    Function connect(ByVal ip As String, ByVal port As String) As Boolean
    Sub disconnect()
    '  Function readData(ByVal data() As Byte) As Integer
    Sub writeData(ByVal data() As Byte)
    Event readData(ByVal data() As Byte)
    Function getConnected() As Boolean
    Event e_Connected(ByVal Connected As Boolean)
End Interface

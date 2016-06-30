'通訊 server client comport
Public Interface IF_Comm
    Sub SendDataToCC(ByVal commData() As Byte)
    Event RecvDataFromCC(ByVal commData() As Byte)
    'Event ShowCommState(ByVal connected As Boolean, ByVal msg As String)
    Property commState() As Boolean

    Function StartProc() As Boolean
    Sub StopProc()




    'Property activeCommState() As Boolean ' Set by FormMain
    'Property activeLogMsg() As Boolean ' Set by CityV


End Interface
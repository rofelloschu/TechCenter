'20180509

Public Interface IF_Communication2


    Function StartProc() As Boolean
    Sub StopProc()


    Property commState() As Boolean

    Event RecvData(ByVal Data() As Byte)
    Sub SendData(ByVal commData() As Byte)
End Interface

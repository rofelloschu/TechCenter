'20180510
Imports System.IO.Ports
Imports System.Threading
Imports classLibrary_bang
Public Class ComPortSocket2
    Implements IF_Communication2

    'Public comport As New SerialPort

    Public Event RecvData(ByVal commData() As Byte) Implements IF_Communication2.RecvData
    ' Communication ports
    Public comPort As SerialPort = New SerialPort()
    'Public m_CurrentPath As String
    Public comm_state As Boolean = False

    Public Sub New(ByVal portNameStr As String)
        Me.comPort.PortName = portNameStr

        '   Me.m_CurrentPath = Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)
        'Me.m_CurrentPath = M_TC2.CurrentPath
    End Sub
    Public Property commState() As Boolean Implements IF_Communication2.commState

        Get
            Return Me.comm_state
        End Get

        Set(ByVal value As Boolean)
            Me.comm_state = value
        End Set

    End Property
    Public Sub StartProc() Implements IF_Communication2.StartProc
        ' Get a list of serial port names.
        Dim ports As String() = SerialPort.GetPortNames()
        Dim portNameComparer As StringComparer = StringComparer.OrdinalIgnoreCase
        Dim initResult As Boolean = False
        For Each port As String In ports
            If portNameComparer.Equals(Me.comPort.PortName, port) Then
                Try
                    Me.comPort.Open()
                    initResult = True
                    Exit For
                Catch ex As Exception
                    'Return False
                End Try
            End If
        Next port
        If initResult Then
            AddHandler Me.comPort.DataReceived, AddressOf CommPort_DataReceived
        End If
        Me.commState = True

        'Return initResult
    End Sub
    'Public Function StartProc() As Boolean Implements IF_Communication2.StartProc
    '    ' Get a list of serial port names.
    '    Dim ports As String() = SerialPort.GetPortNames()
    '    Dim portNameComparer As StringComparer = StringComparer.OrdinalIgnoreCase
    '    Dim initResult As Boolean = False
    '    For Each port As String In ports
    '        If portNameComparer.Equals(Me.comPort.PortName, port) Then
    '            Try
    '                Me.comPort.Open()
    '                initResult = True
    '                Exit For
    '            Catch ex As Exception
    '                Return False
    '            End Try
    '        End If
    '    Next port
    '    If initResult Then
    '        AddHandler Me.comPort.DataReceived, AddressOf CommPort_DataReceived
    '    End If
    '    Me.commState = True

    '    Return initResult
    'End Function

    Public Sub StopProc() Implements IF_Communication2.StopProc
        If Me.comPort IsNot Nothing Then
            If Me.comPort.IsOpen Then
                Me.comPort.Close()
            End If
        End If
    End Sub

    Protected Sub CommPort_DataReceived(ByVal sender As System.Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs)
        ' Thread.Sleep(50) ' Wait for more bytes to read
        Dim sp As System.IO.Ports.SerialPort = CType(sender, System.IO.Ports.SerialPort)
        Dim countReceive As Integer = sp.BytesToRead
        Dim serialRecvBuf(countReceive - 1) As Byte
        sp.Read(serialRecvBuf, 0, countReceive)

        ' Pass Receive Data
        Try
            ' Recv done
            'Me.InvokeCommState(COMM_DIRECTION.RECV, serialRecvBuf)

            RaiseEvent RecvData(serialRecvBuf)
        Catch ex As Exception
            ' 發生不明錯誤
            'Me.LogErr("{RaiseEvent RecvDataFromCC:exMsg}" & ex.Message)
        End Try
    End Sub


    Public Sub SendDataToCC(ByVal commData() As Byte) Implements IF_Communication2.SendData  ', IF_CommToForm.SendDataToCC
        Me.comPort.Write(commData, 0, commData.Length)
        ' Send done
        'Me.InvokeCommState(COMM_DIRECTION.SEND, commData)
    End Sub

    'Private Sub InvokeCommState(ByVal state As Boolean, ByVal info As String)
    '    Me.commState = state

    '    If Me.activeCommState Or Me.activeLogMsg Then
    '        Dim logTime As DateTime = DateTime.Now
    '        Dim msg As String = String.Format("{0} - {1} - {2} - {3},,{4}", _
    '                                        Me.commPort.PortName, logTime.ToShortDateString(), logTime.ToString("HH:mm:ss"), info, vbCrLf)
    '        If Me.activeCommState Then
    '            RaiseEvent ShowCommState(state, msg)
    '        End If
    '        If Me.activeLogMsg Then
    '            Me.LogMsg(msg, logTime)
    '        End If
    '    End If
    'End Sub

    'Private Sub InvokeCommState(ByVal info As String)
    '    Me.InvokeCommState(Me.commState, info)
    'End Sub

    'Private Sub InvokeCommState(ByVal direction As Integer, ByVal commData() As Byte)
    '    If Me.activeCommState Or Me.activeLogMsg Then
    '        Dim info As String = ""
    '        Dim msg As String = ""
    '        Dim logTime As DateTime = DateTime.Now
    '        Select Case direction
    '            Case COMM_DIRECTION.RECV
    '                info = "[Recv]"
    '                msg = String.Format("{0} - {1} - {2} - {3},,{4},{5},{6}", _
    '                                        Me.commPort.PortName, logTime.ToShortDateString(), logTime.ToString("HH:mm:ss"), info, vbCrLf, _
    '                                        BitConverter.ToString(commData, 0, commData.Length), vbCrLf)
    '            Case COMM_DIRECTION.SEND
    '                info = "[Send]"
    '                msg = String.Format("{0} - {1} - {2} - {3},,{4},,{5}{6}", _
    '                                        Me.commPort.PortName, logTime.ToShortDateString(), logTime.ToString("HH:mm:ss"), info, vbCrLf, _
    '                                        BitConverter.ToString(commData, 0, commData.Length), vbCrLf)
    '        End Select
    '        If Me.activeCommState Then
    '            RaiseEvent ShowCommState(Me.commState, msg)
    '        End If
    '        If Me.activeLogMsg Then
    '            Me.LogMsg(msg, logTime)
    '        End If
    '    End If
    'End Sub 


    Public Sub close() Implements IF_Communication2.close

    End Sub

    Public Event e_commState(status As Boolean) Implements IF_Communication2.e_commState

    Public Event err(text As String, ex As Exception) Implements IF_Communication2.err

    
End Class

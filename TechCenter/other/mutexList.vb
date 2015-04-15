'20120621
'暫時取代mutex
Imports System.Threading
'Namespace tools
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Public Class mutexList(Of T)
    Private List As List(Of T)

    Private mutex As Mutex
    Private AutoResetEvent As AutoResetEvent

    Sub New()
        'mutex = New Mutex(False)
        AutoResetEvent = New AutoResetEvent(True)
        List = New List(Of T)

    End Sub
    Sub Add(ByVal text As T)
        'Mutex.WaitOne()
        AutoResetEvent.WaitOne()
        List.Add(text)
        AutoResetEvent.Set()
        'Mutex.ReleaseMutex()
    End Sub
    Sub addFirst(ByVal text As T)
        ' Mutex.WaitOne()
        AutoResetEvent.WaitOne()
        List.Insert(0, text)
        AutoResetEvent.Set()
        'Mutex.ReleaseMutex()
    End Sub
    Function Count() As Integer
        Return List.Count
    End Function
    Sub removeAt(ByVal index As Integer)
        'mutex.WaitOne()
        AutoResetEvent.WaitOne()
        List.RemoveAt(index)
        AutoResetEvent.Set()
        'Mutex.ReleaseMutex()
    End Sub
    Function getFirstValue() As T
        Try

            Dim resText As T = Nothing
            If List.Count = 0 Then
            Else
                resText = List.Item(0)
                removeAt(0)
            End If


            Return resText
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function ToArray() As T()
        Return List.ToArray()
    End Function
    Sub Clear()
        'Mutex.WaitOne()
        AutoResetEvent.WaitOne()
        List.Clear()
        GC.Collect()
        AutoResetEvent.Set()
        'Mutex.ReleaseMutex()
    End Sub


    Public Property Item(ByVal index As Integer) As T
        Get
            Return List.Item(index)
        End Get
        Set(ByVal value As T)
            ' Mutex.WaitOne()
            AutoResetEvent.WaitOne()
            List.Item(index) = value
            AutoResetEvent.Set()
            ' Mutex.ReleaseMutex()
        End Set
    End Property


    Public Sub test(ByVal count As Integer)
        Dim testmutexList As New mutexList(Of String)
        Dim startTime As DateTime
        Dim endTime As DateTime
        Dim ts As TimeSpan
        startTime = Now
        For index As Integer = 0 To count
            testmutexList.Add(index.ToString)
        Next
        For index As Integer = 0 To count
            testmutexList.getFirstValue()
        Next
        endTime = Now
        ts = endTime - startTime
        M_WriteLineMaster.WriteLine(testmutexList.GetType.ToString)
        M_WriteLineMaster.WriteLine("Test TotalSeconds:" + ts.TotalSeconds.ToString)
    End Sub
End Class
<ObsoleteAttribute("DLL過時，改用sunskyLibrary", False)> _
Class mutexList_V02(Of T)

    Private Queue As Queue(Of T)
    Private mutex As Mutex
    Private AutoResetEvent As AutoResetEvent

    Sub New()
        'mutex = New Mutex(False)
        AutoResetEvent = New AutoResetEvent(True)
        Queue = New Queue(Of T)

    End Sub
    Sub Add(ByVal text As T)
        'Mutex.WaitOne()
        AutoResetEvent.WaitOne()
        Queue.Enqueue(text)
        AutoResetEvent.Set()
        'Mutex.ReleaseMutex()
    End Sub
    'Sub addFirst(ByVal text As T)
    '    ' Mutex.WaitOne()
    '    AutoResetEvent.WaitOne()
    '    List.Insert(0, text)
    '    AutoResetEvent.Set()
    '    'Mutex.ReleaseMutex()
    'End Sub
    Function Count() As Integer
        Return Queue.Count
    End Function
    'Sub removeAt(ByVal index As Integer)
    '    'mutex.WaitOne()
    '    AutoResetEvent.WaitOne()
    '    List.RemoveAt(index)

    '    AutoResetEvent.Set()
    '    'Mutex.ReleaseMutex()
    'End Sub
    Function getFirstValue() As T
        Try



            Return Queue.Dequeue
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function ToArray() As T()
        Return Queue.ToArray()
    End Function
    Sub Clear()
        'Mutex.WaitOne()
        AutoResetEvent.WaitOne()
        Queue.Clear()
        GC.Collect()
        AutoResetEvent.Set()
        'Mutex.ReleaseMutex()
    End Sub


    Public Property Item(ByVal index As Integer) As T
        Get
            Return Queue.ToArray(index)
        End Get
        Set(ByVal value As T)
            '' Mutex.WaitOne()
            'AutoResetEvent.WaitOne()
            'List.Item(index) = value
            'AutoResetEvent.Set()
            ' Mutex.ReleaseMutex()

        End Set
    End Property

    Public Sub test(ByVal count As Integer)
        Dim testmutexList As New mutexList(Of String)
        Dim startTime As DateTime
        Dim endTime As DateTime
        Dim ts As TimeSpan
        startTime = Now
        For index As Integer = 0 To count
            testmutexList.Add(index.ToString)
        Next
        For index As Integer = 0 To count
            testmutexList.getFirstValue()
        Next
        endTime = Now
        ts = endTime - startTime
        M_WriteLineMaster.WriteLine(testmutexList.GetType.ToString)
        M_WriteLineMaster.WriteLine("Test TotalSeconds:" + ts.TotalSeconds.ToString)
    End Sub
End Class
'End Namespace

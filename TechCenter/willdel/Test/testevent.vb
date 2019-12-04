Imports System.Threading
Imports classLibrary_bang
Public Class testevent

End Class
#Region "testevent"

Public Class loopThread
    Private Shared SleepTime As Integer = 1
    Private Shared LoopAlive As Boolean = True
    Private Shared ThreadStart As System.Threading.ThreadStart
    Private t_run As Thread
    Private t_run2 As Thread
    Private param As Object


    Sub New(ByVal start As System.Threading.ThreadStart, ByVal p As Object)

        ThreadStart = start
        param = p
        t_run2 = New Thread(AddressOf loopThread.run2)
        t_run2.Start()

    End Sub
    Sub New(ByVal start As System.Threading.ThreadStart)
        ThreadStart = start
        t_run = New Thread(AddressOf loopThread.run)
        t_run.Start()

    End Sub
    Shared Sub run2()
        Dim t_Thread As System.Threading.Thread
        t_Thread = New Thread(ThreadStart)
        t_Thread.Start()
        While LoopAlive
            Thread.Sleep(SleepTime)
            If Not t_Thread.IsAlive Then
                t_Thread = New Thread(ThreadStart)
                t_Thread.Start()
            End If
            GC.Collect()
        End While
    End Sub
    Shared Sub run()
        While LoopAlive
            Thread.Sleep(SleepTime)
            ThreadStart()
        End While

    End Sub

    Sub close()
        LoopAlive = False
        GC.Collect()
    End Sub
End Class
Class eventest
    Sub New()
        Dim Receiver As New Receiver
        Dim Sender As New Sender
        Dim R2 As New Receiver(Sender)

        Console.ReadKey()
    End Sub
    Public Class StartEventArgs
        Inherits System.EventArgs
        Sub New()

        End Sub
        'Provide one or more constructors, as well as fields and
        'accessors for the arguments.

    End Class

    Public Class Sender
        Sub New()

        End Sub
        Sub New(ByVal e As StartEventArgs)
            OnStart(e)
        End Sub
        Public Event Start(ByVal sender As Object, ByVal e As StartEventArgs)

        Protected Overridable Sub OnStart(ByVal e As StartEventArgs)
            RaiseEvent Start(Me, e)
        End Sub
        Sub test()
            OnStart(New StartEventArgs)
        End Sub
        '...

    End Class

    Public Class Receiver

        Friend WithEvents MySender As Sender
        Private MySender2 As New Sender
        Sub New(ByVal Sender As Sender)
            MySender = Sender
            MySender.test()
        End Sub
        Sub New()
            AddHandler MySender2.Start, AddressOf Me.HandleStart
            MySender2.test()
            Dim MemoryOptimizedBaseControl As New MemoryOptimizedBaseControl
        End Sub
        Private Sub MySender_Start(ByVal sender As Object, _
              ByVal e As StartEventArgs) Handles MySender.Start
            '...
            Console.WriteLine("a")
        End Sub
        Public Sub HandleStart(ByVal sender As Object, _
                     ByVal e As StartEventArgs)
            Console.WriteLine("b")
        End Sub
    End Class
    '?
    '宣告自訂事件以避免封鎖 (Visual Basic)
    Public NotInheritable Class ReliabilityOptimizedControl
        'Defines a list for storing the delegates
        Private EventHandlerList As New ArrayList

        'Defines the Click event using the custom event syntax.
        'The RaiseEvent always invokes the delegates asynchronously
        Public Custom Event Click As EventHandler
            AddHandler(ByVal value As EventHandler)
                EventHandlerList.Add(value)
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                EventHandlerList.Remove(value)
            End RemoveHandler
            RaiseEvent(ByVal sender As Object, ByVal e As EventArgs)
                For Each handler As EventHandler In EventHandlerList
                    If handler IsNot Nothing Then
                        handler.BeginInvoke(sender, e, Nothing, Nothing)
                    End If
                Next
            End RaiseEvent
        End Event
    End Class
    '宣告自訂事件以節省記憶體 (Visual Basic)
    Public Class MemoryOptimizedBaseControl
        ' Define a delegate store for all event handlers.
        Private Events As New System.ComponentModel.EventHandlerList
        Sub New()

        End Sub

        '自創event
        ' Define the Click event to use the delegate store.
        Public Custom Event Click As EventHandler
            AddHandler(ByVal value As EventHandler)
                Events.AddHandler("ClickEvent", value)
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                Events.RemoveHandler("ClickEvent", value)
            End RemoveHandler
            RaiseEvent(ByVal sender As Object, ByVal e As EventArgs)
                CType(Events("ClickEvent"), EventHandler).Invoke(sender, e)
            End RaiseEvent
        End Event

        ' Define the DoubleClick event to use the same delegate store.
        Public Custom Event DoubleClick As EventHandler
            AddHandler(ByVal value As EventHandler)
                Events.AddHandler("DoubleClickEvent", value)
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                Events.RemoveHandler("DoubleClickEvent", value)
            End RemoveHandler
            RaiseEvent(ByVal sender As Object, ByVal e As EventArgs)
                CType(Events("DoubleClickEvent"), EventHandler).Invoke(sender, e)
            End RaiseEvent
        End Event

        ' Define additional events to use the same delegate store.
        ' ...
    End Class


End Class
Class LH
    Private a As loopHandles(Of Object)
    Sub New()
        a = New loopHandles(Of Object)
    End Sub
    Class loopHandles(Of T)

        Private myHandler As System.EventHandler
        Private WithEvents meEvent As New meEvent
        Private a As Boolean
        Private loopHandlesEventArgs As loopHandlesEventArgs(Of T)
        Sub New()

        End Sub
        Sub New(ByVal Handler As System.Delegate)
            Me.myHandler = Handler

        End Sub
        Sub Handless(ByVal sender As Object, ByVal d As T) Handles Me.event_Read

        End Sub
        Sub Handless2(ByVal sender As Object, ByVal d As EventArgs) Handles meEvent.TestClick

        End Sub
        Event event_Read(ByVal sender As Object, ByVal d As T)
        Protected Overridable Sub OnRead(ByVal d As T)
            RaiseEvent event_Read(Me, d)

        End Sub
        Sub setEvent(ByVal meEvent As meEvent)
            meEvent = meEvent
            GC.Collect()
        End Sub


    End Class
    Public Class meEvent

        Private Events As New System.ComponentModel.EventHandlerList
        Sub New()

        End Sub
        Public Custom Event TestClick As EventHandler
            AddHandler(ByVal value As EventHandler)
                Events.AddHandler("DoubleClickEvent", value)
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                Events.RemoveHandler("TestClickEvent", value)
            End RemoveHandler
            RaiseEvent(ByVal sender As Object, ByVal e As EventArgs)
                CType(Events("TestClickEvent"), EventHandler).Invoke(sender, e)
            End RaiseEvent
        End Event
    End Class
    Public Class loopHandlesEventArgs(Of T)
        Inherits EventArgs
        Private mutexList As mutexList(Of T)
        Sub New()

        End Sub
    End Class
End Class



#End Region
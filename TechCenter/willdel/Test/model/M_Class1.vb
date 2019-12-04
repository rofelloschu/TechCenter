Public Module M_Class1
    Sub testp(ByVal myProcess As Process)

        Dim peakPagedMem As Long = 0
        Dim peakWorkingSet As Long = 0
        Dim peakVirtualMem As Long = 0


        ' Refresh the current process property values.
        myProcess.Refresh()

        Console.WriteLine()

        ' Display current process statistics.

        Console.WriteLine("{0} -", myProcess.ToString())
        Console.WriteLine("-------------------------------------")

        Console.WriteLine("  physical memory usage: {0}", _
             myProcess.WorkingSet64)
        Console.WriteLine("  base priority: {0}", _
             myProcess.BasePriority)
        Console.WriteLine("  priority class: {0}", _
             myProcess.PriorityClass)
        Console.WriteLine("  user processor time: {0}", _
             myProcess.UserProcessorTime)
        Console.WriteLine("  privileged processor time: {0}", _
             myProcess.PrivilegedProcessorTime)
        Console.WriteLine("  total processor time: {0}", _
             myProcess.TotalProcessorTime)
        Console.WriteLine("  PagedSystemMemorySize64: {0}", _
            myProcess.PagedSystemMemorySize64)
        Console.WriteLine("  PagedMemorySize64: {0}", _
           myProcess.PagedMemorySize64)

        ' Update the values for the overall peak memory statistics.
        peakPagedMem = myProcess.PeakPagedMemorySize64
        peakVirtualMem = myProcess.PeakVirtualMemorySize64
        peakWorkingSet = myProcess.PeakWorkingSet64

        If myProcess.Responding Then
            Console.WriteLine("Status = Running")
        Else
            Console.WriteLine("Status = Not Responding")
        End If

    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim NotepadMemory As Integer
        Dim component1() As Process
        component1 = Process.GetProcessesByName("VirtualTCSrv.exe")

        If component1.Length > 0 Then
            NotepadMemory = component1(0).PrivateMemorySize64
            Console.WriteLine("Memory used: " & NotepadMemory & ".")
            Exit Sub
        End If
        component1 = Process.GetProcessesByName("VirtualTCSrv.vshost")

        If component1.Length > 0 Then
            Console.WriteLine("Id: " & component1(0).Id.ToString & ".")
            Console.WriteLine("StartTime: " & component1(0).StartTime.ToString & ".")
            Console.WriteLine("Threads Count: " & component1(0).Threads.Count.ToString & ".")
            Console.WriteLine("TotalProcessorTime: " & component1(0).TotalProcessorTime.ToString & ".")
            Console.WriteLine("MaxWorkingSet used: " & component1(0).MaxWorkingSet.ToString & ".")
            Console.WriteLine("MinWorkingSet used: " & component1(0).MinWorkingSet.ToString & ".")

            Console.WriteLine("NonpagedSystemMemorySize used: " & component1(0).NonpagedSystemMemorySize64.ToString & ".")
            Console.WriteLine("PagedSystemMemorySize used: " & component1(0).PagedSystemMemorySize64.ToString & ".")

            Console.WriteLine("PeakPagedMemorySize used: " & component1(0).PeakPagedMemorySize64.ToString & ".")
            Console.WriteLine("PrivateMemorySize used: " & component1(0).PrivateMemorySize64.ToString & ".")
            Console.WriteLine("PagedMemorySize used: " & component1(0).PagedMemorySize64.ToString & ".")

            Console.WriteLine("PeakVirtualMemorySize used: " & component1(0).PeakVirtualMemorySize64.ToString & ".")
            Console.WriteLine("PeakWorkingSet used: " & component1(0).PeakWorkingSet64.ToString & ".")
            Console.WriteLine("VirtualMemory used: " & component1(0).VirtualMemorySize64.ToString & ".")
            Console.WriteLine("WorkingSet used: " & component1(0).WorkingSet64.ToString & ".")
            Console.WriteLine("")
            Dim PCounter1 As New PerformanceCounter
            PCounter1.CategoryName = "Process"
            PCounter1.CounterName = "% Processor Time"
            PCounter1.InstanceName = component1(0).ProcessName
            Console.WriteLine(PCounter1.NextValue.ToString())
            '   testp(component1(0))
            Exit Sub
        End If

    End Sub
    Sub test03(ByVal myProcess As Process)
        Dim PCounter1 As New PerformanceCounter
        PCounter1.CategoryName = "Process"
        '   PCounter1.CounterName = "% Processor Time"
        PCounter1.CounterName = "Private Bytes"
        PCounter1.InstanceName = myProcess.ProcessName
        Console.WriteLine(CInt(PCounter1.NextValue / 1024).ToString())
    End Sub
    Sub testThrowEX()
        testThrowEX2()
    End Sub
    Sub testThrowEX2()
        Dim a(3) As String
        a(2) = ""
        Console.WriteLine(a(4))
    End Sub
End Module

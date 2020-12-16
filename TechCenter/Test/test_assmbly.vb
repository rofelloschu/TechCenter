Imports System.Reflection
'Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Linq
Public Class test_assmbly


    Sub New()

    End Sub
    Sub test()
        'Dim Assembly As Assembly = Assembly.LoadFrom(System.IO.Directory.GetCurrentDirectory() + "\20200416_test\" + "CP5200.dll")
        'Dim Assembly As Assembly = Assembly.LoadFrom(System.IO.Directory.GetCurrentDirectory() + "\20200416_test\" + "classLibrary_bang.dll")
        'https://dotblogs.com.tw/kinanson/2015/07/03/151717
        'https://blog.csdn.net/AAA123524457/article/details/53242877
        'http://www.365jz.com/article/24377
        'https://www.cnblogs.com/zhaoqingqing/p/3944497.html
        'https://dotblogs.com.tw/kinanson/2015/07/03/151717
        Console.WriteLine("len " + AppDomain.CurrentDomain.GetAssemblies().Length.ToString)
        For index As Integer = 0 To AppDomain.CurrentDomain.GetAssemblies().Length - 1
            Console.WriteLine(AppDomain.CurrentDomain.GetAssemblies(index).GetName)
            Console.WriteLine(AppDomain.CurrentDomain.GetAssemblies(index).CodeBase)
        Next


        'Dim Assembly As Assembly = Assembly.LoadFile(System.IO.Directory.GetCurrentDirectory() + "\20200416_test\" + "Cpower.dll")
        Dim Assembly As Assembly = Assembly.LoadFile(System.IO.Directory.GetCurrentDirectory() + "\20200416_test\" + "CP5200.dll")

        Console.WriteLine(Assembly.FullName)
        Console.WriteLine(Assembly.GetName.CodeBase)

        Console.WriteLine("len " + AppDomain.CurrentDomain.GetAssemblies().Length.ToString)
        Exit Sub

        'Dim t As Type = Assembly.GetType("Cpower.use_cps5200")
        Dim t As Type = Assembly.GetType("Cpower.use_cps5200")
        Dim o As Object = Activator.CreateInstance(t)

        Dim m() As MethodInfo = o.GetType().GetMethods()

        'o.GetType().GetMethod("setDirPath").Invoke(o, {System.IO.Directory.GetCurrentDirectory()})
        'o.GetType().GetMethod("Net_Init").Invoke(o, {"127.0.0.1", 3333})
        m.ToString()
        '猜測FirstOrDefault需要Linq
        m.FirstOrDefault(Function(x) x.Name = "Net_Init" AndAlso x.GetParameters().Count() = 2).Invoke(o, {"127.0.0.1", 3333})
        'Console.WriteLine(r.ToString)
        Dim r As Object = o.GetType().GetMethod("testConnect").Invoke(o, {})

        Dim Assembly2 As Assembly = Assembly.LoadFrom(System.IO.Directory.GetCurrentDirectory() + "\20200416_test\2\" + "Cpower.dll")

        Dim t2 As Type = Assembly.GetType("Cpower.use_cps5200")
        Dim o2 As Object = Activator.CreateInstance(t2)
        Dim m2() As MethodInfo = o.GetType().GetMethods()

        m2.FirstOrDefault(Function(x) x.Name = "Net_Init" AndAlso x.GetParameters().Count() = 2).Invoke(o2, {"127.0.0.1", 4444})

        Dim r2 As Object = o2.GetType().GetMethod("testConnect").Invoke(o, {})
        r = o.GetType().GetMethod("testConnect").Invoke(o, {})
    End Sub
End Class

Public Class if_call_check
    Sub New()

    End Sub
    Sub test(if_call As if_call)

        AddHandler if_call.test_text, AddressOf AddressOf_test_text
        AddHandler if_call.Mcall, AddressOf AddressOf_Mcall

        if_call.test_start()
    End Sub
    Sub AddressOf_test_text(text As String)
        Console.WriteLine(text)
    End Sub
    Sub AddressOf_Mcall()

    End Sub
End Class

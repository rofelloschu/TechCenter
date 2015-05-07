Imports Microsoft.VisualBasic.ApplicationServices
Namespace My
    ' MyApplication 可以使用下列事件: 
    ' 
    ' Startup:  在應用程式啟動時，但尚未建立啟動表單之前引發。
    ' Shutdown:  在所有應用程式表單關閉之後引發。如果應用程式不正常終止，就不會引發此事件。
    ' UnhandledException:  在應用程式發生未處理的例外狀況時引發。
    ' StartupNextInstance:  在啟動單一執行個體應用程式且應用程式已於使用中時引發。
    ' NetworkAvailabilityChanged:  在連接或中斷網路連接時引發。
    Partial Friend Class MyApplication


        Private Sub Application_UnhandledException(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs) Handles Me.UnhandledException

            'If TypeOf e.ExceptionObject Is FileNotFoundException Then
            '    ' Inform the user 

            '    ' Recover from the error 
            '    e.Handled = True
            '    Return
            'End If
            Console.WriteLine("")
            ' Exception is unrecoverable so stop the application and allow the 
            ' Silverlight plug-in control to detect and process the exception. 

        End Sub

        Private Sub Application_Startup(ByVal o As Object, ByVal e As StartupEventArgs) Handles Me.Startup

            ' Detect when the application starts up.

        End Sub
    End Class

End Namespace


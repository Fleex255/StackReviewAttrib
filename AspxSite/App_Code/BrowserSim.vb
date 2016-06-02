Imports System.Windows.Forms
Public Class BrowserSim
    Public Shared Function GetPageHtml(URL As String) As String
        Dim html As String
        Dim t As New Threading.Thread(Sub()
                                          Dim wb As New WebBrowser With {.ScrollBarsEnabled = False}
                                          wb.AllowNavigation = True
                                          AddHandler wb.DocumentCompleted, Sub()
                                                                               Dim s As New Diagnostics.Stopwatch()
                                                                               s.Start()
                                                                               Do Until s.ElapsedMilliseconds > 1500
                                                                                   Application.DoEvents() ' Give time for the JS to work
                                                                               Loop
                                                                               s.Stop()
                                                                               html = wb.Document.GetElementsByTagName("html")(0).OuterHtml
                                                                               Application.Exit()
                                                                           End Sub
                                          wb.Navigate(URL)
                                          Application.Run()
                                      End Sub)
        t.SetApartmentState(Threading.ApartmentState.STA)
        t.Start()
        t.Join()
        Return html
    End Function
End Class

Imports Microsoft.VisualBasic
Public Class AttribDoc
    Shared Stream As IO.StreamWriter
    Public Shared AddedUrls As List(Of String)
    Public Shared ReadOnly Property IsStarted As Boolean
        Get
            Return Stream IsNot Nothing
        End Get
    End Property
    Public Shared Sub StartList()
        If Stream IsNot Nothing Then Stream.Close()
        AddedUrls = New List(Of String)
        Dim fileTitle = "SEReview-" & Now.ToString("MM-dd-yyyy-HH-mm") & ".html"
        Stream = New IO.StreamWriter(IO.Path.Combine(HttpContext.Current.Server.MapPath("~\App_Data"), fileTitle), False)
        Stream.WriteLine("<html><head><title>Attribution for reviewing at Stack Exchange</title><meta charset=""UTF-8""></head>")
        Stream.WriteLine("<body>Content in the accompanying work(s) originated in the Stack Exchange network. Neither Stack Exchange nor the posters endorse the work accompanied by this document.<br />")
        Stream.WriteLine("This page gives <a href=""http://blog.stackoverflow.com/2009/06/attribution-required/"">appropriate attribution</a> for each post shown.<br />")
        Stream.WriteLine("The code for the program that generated this page is <a href=""https://github.com/Fleex255/StackReviewAttrib"">available on GitHub</a>.<br /><br />")
        Stream.Write("Attribution recording session started at ")
        Stream.Write(Now.ToString)
        Stream.WriteLine(".<br />")
        Stream.WriteLine("<ul>")
    End Sub
    Public Shared Sub AddLine(Text As String)
        Stream.Write("<li>")
        Stream.Write(Text)
        Stream.Write("</li>")
    End Sub
    Public Shared Sub StopList()
        Stream.Write("</ul>Attribution recording session ended at ")
        Stream.Write(Now.ToString)
        Stream.WriteLine(".</body></html>")
        Stream.Close()
        Stream = Nothing
        AddedUrls = Nothing
    End Sub
End Class

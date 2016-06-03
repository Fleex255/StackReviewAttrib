Imports HtmlAgilityPack
Partial Class Add
    Inherits System.Web.UI.Page
    Private Sub PageLoad(sender As Object, e As EventArgs) Handles Me.Load
        If Not AttribDoc.IsStarted Then Exit Sub
        ' Get the page
        Dim reviewPage = Request.QueryString("page")
        If reviewPage = "" Or Not reviewPage Like "*/review/*/#*" Then Exit Sub
        If AttribDoc.AddedUrls.Contains(reviewPage) Then Exit Sub
        Dim html As New HtmlDocument
        Dim htmlString = BrowserSim.GetPageHtml(reviewPage)
        html.LoadHtml(htmlString)
        ' Find the site root
        Dim fullUri As New Uri(reviewPage)
        Dim siteRoot = "http://" & fullUri.Host
        ' Find the important data
        Dim linkQ, linkA As String ' Question and answer links
        Dim userQ, userA, userE As String ' User names
        Dim userlinkQ, userlinkA, userlinkE As String ' User links
        Dim titleQA As String ' Post title
        For Each node In html.DocumentNode.Descendants
            If node.InnerHtml = "link" AndAlso node.Name = "a" Then
                Dim title = node.Attributes("title").Value
                Dim href = siteRoot & node.Attributes("href").Value
                If title = "view answer" Then
                    linkA = href
                ElseIf title = "view question" Then
                    linkQ = href
                End If
            ElseIf node.Attributes.Contains("class") Then
                Dim classname = node.Attributes("class").Value
                If classname = "question-link" OrElse classname.Contains("question-hyperlink") Then
                    titleQA = node.InnerText
                    linkQ = siteRoot & node.Attributes("href").Value
                ElseIf classname = "answer-link" OrElse classname.Contains("answer-hyperlink") Then ' For suggested edits
                    titleQA = node.InnerText
                    linkA = siteRoot & node.Attributes("href").Value
                ElseIf classname.Contains("user-info") And Not classname.Contains("-actions") Then
                    Dim userinfoNode = node.ChildNodes.FirstOrDefault(Function(n) n.Attributes.Contains("class") AndAlso n.Attributes("class").Value.Contains("user-details"))
                    If userinfoNode Is Nothing Then Continue For Else userinfoNode = userinfoNode.ChildNodes.FindFirst("a")
                    If userinfoNode Is Nothing Then Continue For ' If somebody edits their own post
                    Dim href = siteRoot & userinfoNode.Attributes("href").Value
                    Dim name = userinfoNode.InnerHtml
                    Dim uatNode = node.ChildNodes.First(Function(n) n.Attributes.Contains("class") AndAlso n.Attributes("class").Value = "user-action-time")
                    If uatNode.InnerHtml.Contains("answered") Then
                        userlinkA = href
                        userA = name
                    ElseIf uatNode.InnerHtml.Contains("asked") Then
                        userlinkQ = href
                        userQ = name
                    ElseIf uatNode.InnerHtml.Contains("proposed") Then
                        userlinkE = href
                        userE = name
                    End If
                ElseIf classname = "started" Then ' Suggested edits by anonymous users
                    If node.InnerHtml.Contains("an anonymous user") Then
                        userE = "an anonymous user"
                        userlinkE = "#"
                    End If
                End If
            End If
        Next
        ' Put together the log entry
        Dim urlParts = Split(reviewPage, "/")
        Dim reviewId = urlParts(urlParts.Length - 2) & " #" & urlParts.Last
        Dim entry As New StringBuilder
        entry.Append("<a href=""")
        entry.Append(reviewPage)
        entry.Append(""">")
        entry.Append(reviewId)
        entry.Append("</a>: ")
        If userE <> "" Then
            entry.Append("Edit proposed by <a href=""")
            entry.Append(userlinkE)
            entry.Append(""">")
            entry.Append(userE)
            entry.Append("</a> on <a href=""")
            entry.Append(IIf(linkA = "", linkQ, linkA))
            entry.Append(""">""")
            entry.Append(titleQA)
            entry.Append("""</a> (written by <a href=""")
            If userA <> "" Then
                entry.Append(userlinkA)
                entry.Append(""">")
                entry.Append(userA)
            Else
                entry.Append(userlinkQ)
                entry.Append(""">")
                entry.Append(userQ)
            End If
            entry.Append("</a>)")
        ElseIf userA <> "" Then
            entry.Append("<a href=""")
            entry.Append(linkA)
            entry.Append(""">Answer</a> by <a href=""")
            entry.Append(userlinkA)
            entry.Append(""">")
            entry.Append(userA)
            entry.Append("</a> to <a href=""")
            entry.Append(linkQ)
            entry.Append(""">""")
            entry.Append(titleQA)
            entry.Append("""</a> (asked by <a href=""")
            entry.Append(userlinkQ)
            entry.Append(""">")
            entry.Append(userQ)
            entry.Append("</a>)")
        Else
            entry.Append("Question by <a href=""")
            entry.Append(userlinkQ)
            entry.Append(""">")
            entry.Append(userQ)
            entry.Append("</a>: <a href=""")
            entry.Append(linkQ)
            entry.Append(""">""")
            entry.Append(titleQA)
            entry.Append("""</a>")
        End If
        AttribDoc.AddLine(entry.ToString)
        AttribDoc.AddedUrls.Add(reviewPage)
    End Sub
End Class

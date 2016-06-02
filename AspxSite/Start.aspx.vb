Partial Class Start
    Inherits System.Web.UI.Page
    Private Sub Start_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not AttribDoc.IsStarted Then AttribDoc.StartList()
    End Sub
End Class

Partial Class [Stop]
    Inherits System.Web.UI.Page
    Private Sub Stop_Load(sender As Object, e As EventArgs) Handles Me.Load
        Response.AppendHeader("Access-Control-Allow-Origin", "*")
        If AttribDoc.IsStarted Then AttribDoc.StopList()
    End Sub
End Class

<%@ Application Language="C#" %>
<%@ Import Namespace="System.Security.Principal" %>
<%@ Import Namespace="System.Web.Security" %>
<script runat="server">
    void Application_Start(object sender, EventArgs e)
    {
        try { Db.EnsureDatabase(); }
        catch (Exception ex) { Application["DbError"] = ex.Message; }
    }

    // Restore the user's role from the forms-authentication ticket.
    void Application_PostAuthenticateRequest(object sender, EventArgs e)
    {
        if (Request.IsAuthenticated && User.Identity is FormsIdentity)
        {
            FormsIdentity id = (FormsIdentity)User.Identity;
            string role = id.Ticket.UserData;
            Context.User = new GenericPrincipal(id, new string[] { role });
        }
    }
</script>

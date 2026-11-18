using System;
using System.Web.Security;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Cache-bust the stylesheet so edits show up without a hard refresh.
        cssLink.Href = ResolveUrl("~/Content/site.css") + "?v=" +
            System.IO.File.GetLastWriteTimeUtc(Server.MapPath("~/Content/site.css")).Ticks;

        bool auth = Request.IsAuthenticated;
        phGuest.Visible = !auth;
        phMember.Visible = auth;
        lnkLogout.Visible = auth;
        phAdmin.Visible = auth && Page.User.IsInRole("Admin");

        object err = Application["DbError"];
        if (err != null)
        {
            lblDbError.Text = "Database setup error: " + err + " (see ReadMe.html)";
            lblDbError.Visible = true;
            return;
        }

        if (auth)
        {
            int uid = Db.CurrentUserId();
            object n = Db.Scalar("SELECT ISNULL(SUM(Quantity),0) FROM CartItems WHERE UserId=@u", Db.P("@u", uid));
            int count = Convert.ToInt32(n);
            lblCount.Visible = count > 0;
            lblCount.Text = count.ToString();
        }
    }

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();
        Session.Clear();
        Response.Redirect("~/Default.aspx");
    }
}

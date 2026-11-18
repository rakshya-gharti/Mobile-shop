using System;
using System.Data;
using System.Web;
using System.Web.Security;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && Request.QueryString["registered"] == "1")
        {
            lblMsg.Text = "Registration successful. Please login.";
            lblMsg.CssClass = "alert ok";
            lblMsg.Visible = true;
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        DataTable dt = Db.Table("SELECT Username, Role FROM Users WHERE Username=@u AND PasswordHash=@p",
                                Db.P("@u", txtUser.Text.Trim()), Db.P("@p", Db.Hash(txtPass.Text)));
        if (dt.Rows.Count == 0)
        {
            lblMsg.Text = "Invalid username or password.";
            lblMsg.CssClass = "alert error";
            lblMsg.Visible = true;
            return;
        }

        string username = dt.Rows[0]["Username"].ToString();
        string role = dt.Rows[0]["Role"].ToString();
        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, username, DateTime.Now,
            DateTime.Now.AddMinutes(chkRemember.Checked ? 60 * 24 * 7 : 30), chkRemember.Checked, role, FormsAuthentication.FormsCookiePath);
        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
        cookie.HttpOnly = true;
        if (chkRemember.Checked) cookie.Expires = ticket.Expiration;
        Response.Cookies.Add(cookie);

        string ret = Request.QueryString["ReturnUrl"];
        if (!string.IsNullOrEmpty(ret) && ret.StartsWith("/") && !ret.StartsWith("//") && !ret.StartsWith("/\\")) Response.Redirect(ret);
        else Response.Redirect(role == "Admin" ? "~/Admin/Default.aspx" : "~/Default.aspx");
    }
}

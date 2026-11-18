using System;
using System.Data;

public partial class Member_Profile : System.Web.UI.Page
{
    int uid;

    protected void Page_Load(object sender, EventArgs e)
    {
        uid = Db.CurrentUserId();
        if (!IsPostBack) Load_();
    }

    private void Load_()
    {
        DataRow r = Db.Table("SELECT * FROM Users WHERE UserId=@u", Db.P("@u", uid)).Rows[0];
        txtUser.Text = r["Username"].ToString();
        txtName.Text = r["FullName"].ToString();
        txtEmail.Text = r["Email"].ToString();
        txtPhone.Text = r["Phone"].ToString();
        txtAddress.Text = r["Address"].ToString();
        int pts = Convert.ToInt32(r["Points"]);
        litPoints.Text = pts.ToString();
        litBadge.Text = pts >= 500 ? "Gold member" : pts >= 150 ? "Silver member" : "Bronze member";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        Db.Exec("UPDATE Users SET FullName=@n, Email=@e, Phone=@p, Address=@a WHERE UserId=@u",
            Db.P("@n", txtName.Text.Trim()), Db.P("@e", txtEmail.Text.Trim()), Db.P("@p", txtPhone.Text.Trim()),
            Db.P("@a", txtAddress.Text.Trim()), Db.P("@u", uid));
        lblMsg.Text = "Profile updated.";
        lblMsg.CssClass = "alert ok";
        lblMsg.Visible = true;
    }

    protected void btnPw_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        int n = Db.Exec("UPDATE Users SET PasswordHash=@new WHERE UserId=@u AND PasswordHash=@old",
            Db.P("@new", Db.Hash(txtNew.Text)), Db.P("@u", uid), Db.P("@old", Db.Hash(txtOld.Text)));
        lblPw.Text = n > 0 ? "Password changed." : "Current password is incorrect.";
        lblPw.CssClass = n > 0 ? "alert ok" : "alert error";
        lblPw.Visible = true;
    }
}

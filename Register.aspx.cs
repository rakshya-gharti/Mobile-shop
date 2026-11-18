using System;

public partial class Register : System.Web.UI.Page
{
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        string user = txtUser.Text.Trim();

        int dup = Convert.ToInt32(Db.Scalar("SELECT COUNT(*) FROM Users WHERE Username=@u", Db.P("@u", user)));
        if (dup > 0)
        {
            Show("That username is already taken.", false);
            return;
        }

        Db.Exec(@"INSERT INTO Users (Username, PasswordHash, FullName, Email, Phone, Address, Role, Points)
                  VALUES (@u,@p,@n,@e,@ph,@a,'Member',50)",
            Db.P("@u", user), Db.P("@p", Db.Hash(txtPass.Text)), Db.P("@n", txtName.Text.Trim()),
            Db.P("@e", txtEmail.Text.Trim()), Db.P("@ph", txtPhone.Text.Trim()), Db.P("@a", txtAddress.Text.Trim()));

        int uid = Convert.ToInt32(Db.Scalar("SELECT UserId FROM Users WHERE Username=@u", Db.P("@u", user)));
        Db.Notify(uid, "Welcome to GlowUp! You received 50 bonus points. Use code WELCOME10 for 10% off.");
        Db.SendMail(txtEmail.Text.Trim(), "Welcome to GlowUp", "Hi " + txtName.Text.Trim() + ", your account has been created.");
        Response.Redirect("~/Login.aspx?registered=1");
    }

    private void Show(string msg, bool ok)
    {
        lblMsg.Text = msg;
        lblMsg.CssClass = "alert " + (ok ? "ok" : "error");
        lblMsg.Visible = true;
    }
}

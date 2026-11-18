using System;

public partial class FeedbackPage : System.Web.UI.Page
{
    protected void btnSend_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        string msg = txtMessage.Text.Trim();
        if (msg.Length > 500) msg = msg.Substring(0, 500);
        Db.Exec("INSERT INTO Feedback (Name, Email, Message) VALUES (@n,@e,@m)",
            Db.P("@n", txtName.Text.Trim()), Db.P("@e", txtEmail.Text.Trim()), Db.P("@m", msg));
        lblMsg.Text = "Thank you! Your feedback has been received.";
        lblMsg.Visible = true;
        txtName.Text = txtEmail.Text = txtMessage.Text = "";
    }
}

using System;
using System.Data;

// Demo payment gateway that mimics the redirect flow of eSewa / Khalti / Connect IPS.
public partial class Member_PayGateway : System.Web.UI.Page
{
    int uid, orderId;
    string method;
    decimal total;

    protected void Page_Load(object sender, EventArgs e)
    {
        uid = Db.CurrentUserId();
        DataTable t = null;
        if (int.TryParse(Request.QueryString["order"], out orderId))
            t = Db.Table("SELECT Total, Status, PaymentMethod FROM Orders WHERE OrderId=@o AND UserId=@u", Db.P("@o", orderId), Db.P("@u", uid));
        if (t == null || t.Rows.Count == 0 || t.Rows[0]["Status"].ToString() != "Unpaid")
        {
            pnlGw.Visible = false;
            lblMsg.Text = "This payment link is invalid or the order is already paid. <a href='Orders.aspx'>View my orders</a>";
            lblMsg.CssClass = "alert error";
            lblMsg.Visible = true;
            return;
        }
        method = t.Rows[0]["PaymentMethod"].ToString();
        total = Convert.ToDecimal(t.Rows[0]["Total"]);
        litProvider.Text = method == "Connect IPS" ? "connectIPS" : method;
        pnlGw.CssClass = "gw " + (method == "eSewa" ? "esewa" : method == "Khalti" ? "khalti" : "ips");
        litOrder.Text = litOrder2.Text = orderId.ToString();
        litAmt.Text = total.ToString("N2");
        pnlBank.Visible = method == "Connect IPS";
        litPinLabel.Text = method == "eSewa" ? "eSewa MPIN" : method == "Khalti" ? "Khalti PIN" : "Connect IPS password";
        if (!IsPostBack) txtMobile.Text = Session["PayMobile"] as string ?? "";
    }

    protected void btnStep1_Click(object sender, EventArgs e)
    {
        if (txtPin.Text.Trim() != "1122") { Fail("Incorrect PIN/password. Please try again."); return; }
        lblMsg.Visible = false;
        litMobile2.Text = txtMobile.Text;
        pnlStep1.Visible = false;
        pnlStep2.Visible = true;
    }

    protected void btnPay_Click(object sender, EventArgs e)
    {
        if (txtOtp.Text.Trim() != "123456") { Fail("Invalid verification code."); return; }
        // Only an order still Unpaid can be paid (guards against double submit)
        int n = Db.Exec("UPDATE Orders SET Status='Pending', PaymentMethod=@m WHERE OrderId=@o AND UserId=@u AND Status='Unpaid'",
            Db.P("@m", method + " (Paid)"), Db.P("@o", orderId), Db.P("@u", uid));
        if (n == 0) { Fail("Order is no longer payable."); return; }
        string txn = "TXN" + DateTime.Now.ToString("yyMMddHHmmss") + orderId;
        int points = (int)(total / 10m);
        Db.Exec("UPDATE Users SET Points = Points + @p WHERE UserId=@u", Db.P("@p", points), Db.P("@u", uid));
        Db.Notify(uid, "Payment of Rs. " + total.ToString("N2") + " received via " + method + " for order #" + orderId + " (" + txn + "). You earned " + points + " points.");
        object email = Db.Scalar("SELECT Email FROM Users WHERE UserId=@u", Db.P("@u", uid));
        Db.SendMail(email.ToString(), "GlowUp order #" + orderId + " payment received",
            "Thank you. We received Rs. " + total.ToString("N2") + " via " + method + ". Transaction ID: " + txn + ". Status: Pending.");
        litTxn.Text = txn;
        pnlStep2.Visible = false;
        pnlOk.Visible = true;
        btnCancel.Visible = false;
        demoNote.Visible = false;
        lblMsg.Visible = false;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Db.Notify(uid, "Payment for order #" + orderId + " was not completed. You can retry from My Orders.");
        Response.Redirect("Orders.aspx?payfail=" + orderId);
    }

    private void Fail(string msg)
    {
        lblMsg.Text = msg;
        lblMsg.CssClass = "alert error";
        lblMsg.Visible = true;
    }
}

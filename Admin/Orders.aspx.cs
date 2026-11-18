using System;
using System.Web.UI.WebControls;

public partial class Admin_Orders : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) Bind();
    }

    private void Bind()
    {
        gvOrders.DataSource = Db.Table(@"SELECT o.OrderId, u.FullName, o.OrderDate, o.Total, o.Status FROM Orders o
              JOIN Users u ON u.UserId=o.UserId WHERE (@s='' OR o.Status=@s) ORDER BY o.OrderDate DESC",
              Db.P("@s", ddlStatus.SelectedValue));
        gvOrders.DataBind();
    }

    protected void ddlStatus_Changed(object sender, EventArgs e)
    {
        gvOrders.PageIndex = 0;
        Bind();
    }

    protected void gvOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOrders.PageIndex = e.NewPageIndex;
        Bind();
    }

    protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "Set") return;
        string[] parts = e.CommandArgument.ToString().Split('|');
        int orderId = int.Parse(parts[0]);
        string status = parts[1];
        string from = status == "Delivery" ? "Pending" : status == "Delivered" ? "Delivery" : null;

        int n;
        if (from != null)
            n = Db.Exec("UPDATE Orders SET Status=@s WHERE OrderId=@o AND Status=@f", Db.P("@s", status), Db.P("@o", orderId), Db.P("@f", from));
        else // cancel (only Pending or Delivery)
            n = Db.Exec("UPDATE Orders SET Status='Cancelled' WHERE OrderId=@o AND Status IN ('Unpaid','Pending','Delivery')", Db.P("@o", orderId));

        if (n > 0)
        {
            if (status == "Cancelled")
                Db.Exec(@"UPDATE Products SET Stock = Stock + (SELECT SUM(oi.Quantity) FROM OrderItems oi WHERE oi.ProductId=Products.ProductId AND oi.OrderId=@o)
                      WHERE ProductId IN (SELECT ProductId FROM OrderItems WHERE OrderId=@o)", Db.P("@o", orderId));

            int uid = Convert.ToInt32(Db.Scalar("SELECT UserId FROM Orders WHERE OrderId=@o", Db.P("@o", orderId)));
            string text = status == "Delivery" ? "is out for delivery" : status == "Delivered" ? "has been delivered" : "was cancelled";
            Db.Notify(uid, "Your order #" + orderId + " " + text + ".");
            string email = Db.Scalar("SELECT Email FROM Users WHERE UserId=@u", Db.P("@u", uid)).ToString();
            Db.SendMail(email, "GlowUp order #" + orderId + " update", "Your order #" + orderId + " " + text + ".");
            lblMsg.Text = "Order #" + orderId + " updated to " + status + ".";
            lblMsg.Visible = true;
        }
        Bind();
    }
}

using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class Member_Orders : System.Web.UI.Page
{
    int uid;

    protected void Page_Load(object sender, EventArgs e)
    {
        uid = Db.CurrentUserId();
        if (!IsPostBack)
        {
            if (Request.QueryString["payfail"] != null)
            {
                lblMsg.Text = "Payment was not completed for order #" + Server.HtmlEncode(Request.QueryString["payfail"]) + ". Click \"Pay now\" to try again.";
                lblMsg.CssClass = "alert error";
                lblMsg.Visible = true;
            }
            BindAll();
        }
    }

    private void BindAll()
    {
        DataTable dt = Db.Table("SELECT * FROM Orders WHERE UserId=@u ORDER BY OrderDate DESC", Db.P("@u", uid));
        rptOrders.DataSource = dt;
        rptOrders.DataBind();
        lblEmpty.Visible = dt.Rows.Count == 0;
        rptNotes.DataSource = Db.Table("SELECT TOP 10 * FROM Notifications WHERE UserId=@u ORDER BY CreatedAt DESC", Db.P("@u", uid));
        rptNotes.DataBind();
    }

    protected void rptOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
        DataRowView row = (DataRowView)e.Item.DataItem;
        DataTable items = Db.Table(@"SELECT oi.OrderId, oi.ProductId, p.Name, oi.Quantity, oi.Quantity*oi.UnitPrice AS LineTotal,
              CAST(CASE WHEN @st='Delivered' AND NOT EXISTS (SELECT 1 FROM Ratings r WHERE r.OrderId=oi.OrderId AND r.ProductId=oi.ProductId AND r.UserId=@u) THEN 1 ELSE 0 END AS INTEGER) AS CanRate
              FROM OrderItems oi JOIN Products p ON p.ProductId=oi.ProductId WHERE oi.OrderId=@o",
              Db.P("@o", row["OrderId"]), Db.P("@st", row["Status"]), Db.P("@u", uid));
        Repeater inner = (Repeater)e.Item.FindControl("rptItems");
        inner.DataSource = items;
        inner.DataBind();
    }

    protected void rptOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Cancel") return;
        int orderId = int.Parse(e.CommandArgument.ToString());
        int n = Db.Exec("UPDATE Orders SET Status='Cancelled' WHERE OrderId=@o AND UserId=@u AND Status IN ('Pending','Unpaid')",
                        Db.P("@o", orderId), Db.P("@u", uid));
        if (n > 0)
        {
            Db.Exec(@"UPDATE Products SET Stock = Stock + (SELECT SUM(oi.Quantity) FROM OrderItems oi WHERE oi.ProductId=Products.ProductId AND oi.OrderId=@o)
                      WHERE ProductId IN (SELECT ProductId FROM OrderItems WHERE OrderId=@o)", Db.P("@o", orderId));
            Db.Notify(uid, "Order #" + orderId + " was cancelled.");
            lblMsg.Text = "Order #" + orderId + " cancelled.";
            lblMsg.CssClass = "alert ok";
            lblMsg.Visible = true;
        }
        BindAll();
    }
}

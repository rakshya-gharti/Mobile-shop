using System;

public partial class Admin_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        litUsers.Text = Db.Scalar("SELECT COUNT(*) FROM Users WHERE Role='Member'").ToString();
        litOrders.Text = Db.Scalar("SELECT COUNT(*) FROM Orders").ToString();
        litPending.Text = Db.Scalar("SELECT COUNT(*) FROM Orders WHERE Status='Pending'").ToString();
        litSales.Text = Convert.ToDecimal(Db.Scalar("SELECT ISNULL(SUM(Total),0) FROM Orders WHERE Status<>'Cancelled'")).ToString("N2");

        gvLow.DataSource = Db.Table("SELECT Name, Stock FROM Products WHERE Stock <= 5 ORDER BY Stock");
        gvLow.DataBind();
        gvSales.DataSource = Db.Table(@"SELECT c.Name, SUM(oi.Quantity*oi.UnitPrice) AS Sales FROM OrderItems oi
              JOIN Orders o ON o.OrderId=oi.OrderId AND o.Status<>'Cancelled'
              JOIN Products p ON p.ProductId=oi.ProductId JOIN Categories c ON c.CategoryId=p.CategoryId GROUP BY c.Name");
        gvSales.DataBind();
        gvFeedback.DataSource = Db.Table("SELECT TOP 20 * FROM Feedback ORDER BY CreatedAt DESC");
        gvFeedback.DataBind();
    }
}

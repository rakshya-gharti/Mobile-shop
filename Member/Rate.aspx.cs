using System;

public partial class Member_Rate : System.Web.UI.Page
{
    int uid, orderId, productId;

    protected void Page_Load(object sender, EventArgs e)
    {
        uid = Db.CurrentUserId();
        if (!int.TryParse(Request.QueryString["order"], out orderId) || !int.TryParse(Request.QueryString["product"], out productId))
        { Deny(); return; }

        // Only the buyer of a delivered order may rate the product.
        object ok = Db.Scalar(@"SELECT COUNT(*) FROM Orders o JOIN OrderItems oi ON oi.OrderId=o.OrderId
              WHERE o.OrderId=@o AND o.UserId=@u AND o.Status='Delivered' AND oi.ProductId=@p",
              Db.P("@o", orderId), Db.P("@u", uid), Db.P("@p", productId));
        if (Convert.ToInt32(ok) == 0) { Deny(); return; }
        litProduct.Text = Server.HtmlEncode(Db.Scalar("SELECT Name FROM Products WHERE ProductId=@p", Db.P("@p", productId)).ToString());
    }

    private void Deny()
    {
        pnlForm.Visible = false;
        lblMsg.Text = "You can only rate products from your delivered orders.";
        lblMsg.CssClass = "alert error";
        lblMsg.Visible = true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid || !pnlForm.Visible) return;
        int exists = Convert.ToInt32(Db.Scalar("SELECT COUNT(*) FROM Ratings WHERE UserId=@u AND OrderId=@o AND ProductId=@p",
            Db.P("@u", uid), Db.P("@o", orderId), Db.P("@p", productId)));
        if (exists > 0)
            Db.Exec("UPDATE Ratings SET Stars=@s, Comment=@c WHERE UserId=@u AND OrderId=@o AND ProductId=@p",
                Db.P("@s", int.Parse(rblStars.SelectedValue)), Db.P("@c", txtComment.Text.Trim()), Db.P("@u", uid), Db.P("@o", orderId), Db.P("@p", productId));
        else
            Db.Exec("INSERT INTO Ratings (UserId, ProductId, OrderId, Stars, Comment) VALUES (@u,@p,@o,@s,@c)",
                Db.P("@u", uid), Db.P("@p", productId), Db.P("@o", orderId), Db.P("@s", int.Parse(rblStars.SelectedValue)), Db.P("@c", txtComment.Text.Trim()));
        Response.Redirect("Orders.aspx");
    }
}

using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class Member_Cart : System.Web.UI.Page
{
    int uid;

    protected void Page_Load(object sender, EventArgs e)
    {
        uid = Db.CurrentUserId();
        if (!IsPostBack)
        {
            int add;
            if (int.TryParse(Request.QueryString["add"], out add))
            {
                AddToCart(add);
                Response.Redirect("Cart.aspx");
            }
            BindCart();
        }
    }

    private void AddToCart(int productId)
    {
        object stock = Db.Scalar("SELECT Stock FROM Products WHERE ProductId=@p", Db.P("@p", productId));
        if (stock == null || Convert.ToInt32(stock) <= 0) return;
        int updated = Db.Exec(@"UPDATE CartItems SET Quantity = CASE WHEN Quantity < @s THEN Quantity + 1 ELSE Quantity END
                                WHERE UserId=@u AND ProductId=@p", Db.P("@u", uid), Db.P("@p", productId), Db.P("@s", stock));
        if (updated == 0)
            Db.Exec("INSERT INTO CartItems (UserId, ProductId, Quantity) VALUES (@u,@p,1)", Db.P("@u", uid), Db.P("@p", productId));
    }

    private void BindCart()
    {
        DataTable dt = Db.Table(@"SELECT c.CartItemId, c.Quantity, p.Name, p.Price, p.ImageUrl
                                  FROM CartItems c JOIN Products p ON p.ProductId=c.ProductId WHERE c.UserId=@u", Db.P("@u", uid));
        rptCart.DataSource = dt;
        rptCart.DataBind();
        pnlSummary.Visible = dt.Rows.Count > 0;
        lblEmpty.Visible = dt.Rows.Count == 0;

        decimal sub = 0;
        foreach (DataRow r in dt.Rows) sub += Convert.ToDecimal(r["Price"]) * Convert.ToInt32(r["Quantity"]);
        decimal disc = Math.Round(sub * CouponPercent() / 100m, 2);
        litSub.Text = sub.ToString("N2");
        litDisc.Text = disc.ToString("N2");
        litTotal.Text = (sub - disc).ToString("N2");
    }

    private int CouponPercent()
    {
        string code = Session["Coupon"] as string;
        if (code == null) return 0;
        object p = Db.Scalar("SELECT Percent FROM Coupons WHERE Code=@c", Db.P("@c", code));
        return p == null ? 0 : Convert.ToInt32(p);
    }

    protected void rptCart_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            Db.Exec("DELETE FROM CartItems WHERE CartItemId=@i AND UserId=@u", Db.P("@i", e.CommandArgument), Db.P("@u", uid));
            BindCart();
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        foreach (RepeaterItem item in rptCart.Items)
        {
            int id = int.Parse(((HiddenField)item.FindControl("hfId")).Value);
            int qty;
            if (int.TryParse(((TextBox)item.FindControl("txtQty")).Text, out qty) && qty > 0)
                Db.Exec(@"UPDATE CartItems SET Quantity = CASE WHEN @q > (SELECT Stock FROM Products WHERE ProductId=CartItems.ProductId)
                          THEN (SELECT Stock FROM Products WHERE ProductId=CartItems.ProductId) ELSE @q END
                          WHERE CartItemId=@i AND UserId=@u", Db.P("@q", qty), Db.P("@i", id), Db.P("@u", uid));
        }
        BindCart();
        lblMsg.Text = "Cart updated.";
        lblMsg.CssClass = "alert ok";
        lblMsg.Visible = true;
    }

    protected void btnCoupon_Click(object sender, EventArgs e)
    {
        string code = txtCoupon.Text.Trim().ToUpper();
        object p = Db.Scalar("SELECT Percent FROM Coupons WHERE Code=@c", Db.P("@c", code));
        if (p == null)
        {
            Session["Coupon"] = null;
            lblMsg.Text = "Invalid coupon code.";
            lblMsg.CssClass = "alert error";
        }
        else
        {
            Session["Coupon"] = code;
            lblMsg.Text = "Coupon applied: " + p + "% off.";
            lblMsg.CssClass = "alert ok";
        }
        lblMsg.Visible = true;
        BindCart();
    }
}

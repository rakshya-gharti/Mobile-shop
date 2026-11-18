using System;
using System.Data;

public partial class ProductDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int id;
        if (!int.TryParse(Request.QueryString["id"], out id)) { Missing(); return; }

        DataTable dt = Db.Table(@"SELECT p.*, c.Name AS CategoryName,
              ISNULL((SELECT AVG(CAST(Stars AS FLOAT)) FROM Ratings r WHERE r.ProductId=p.ProductId),0.0) AS AvgRating,
              (SELECT COUNT(*) FROM Ratings r WHERE r.ProductId=p.ProductId) AS RatingCount
              FROM Products p JOIN Categories c ON c.CategoryId=p.CategoryId WHERE p.ProductId=@id", Db.P("@id", id));
        if (dt.Rows.Count == 0) { Missing(); return; }

        DataRow r = dt.Rows[0];
        Title = r["Name"].ToString();
        imgProduct.ImageUrl = r["ImageUrl"].ToString();
        litName.Text = Server.HtmlEncode(r["Name"].ToString());
        litPrice.Text = "Rs. " + Convert.ToDecimal(r["Price"]).ToString("N2");
        litDesc.Text = Server.HtmlEncode(r["Description"].ToString());
        litCat.Text = Server.HtmlEncode(r["CategoryName"].ToString());
        int stock = Convert.ToInt32(r["Stock"]);
        litStock.Text = stock > 0 ? stock + " in stock" : "Out of stock";
        stars.Value = Convert.ToDouble(r["AvgRating"]);
        stars.Count = Convert.ToInt32(r["RatingCount"]);
        lnkAdd.NavigateUrl = "~/Member/Cart.aspx?add=" + id;
        lnkAdd.Visible = stock > 0;
        lnkWish.NavigateUrl = "~/Member/Wishlist.aspx?add=" + id;

        DataTable rev = Db.Table(@"SELECT u.FullName, r.Stars, r.Comment, r.CreatedAt FROM Ratings r
                                   JOIN Users u ON u.UserId=r.UserId WHERE r.ProductId=@id ORDER BY r.CreatedAt DESC", Db.P("@id", id));
        rptReviews.DataSource = rev;
        rptReviews.DataBind();
        lblNoReviews.Visible = rev.Rows.Count == 0;

        rptRelated.DataSource = Db.Table(@"SELECT TOP 4 ProductId, Name, Price, ImageUrl FROM Products
                                           WHERE CategoryId=@c AND ProductId<>@id ORDER BY NEWID()",
                                         Db.P("@c", r["CategoryId"]), Db.P("@id", id));
        rptRelated.DataBind();
    }

    private void Missing()
    {
        pnlProduct.Visible = false;
        lblMissing.Visible = true;
    }
}

using System;

public partial class Controls_ProductCard : System.Web.UI.UserControl
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public int Stock { get; set; }
    public double Rating { get; set; }
    public int RatingCount { get; set; }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        string detail = "~/ProductDetail.aspx?id=" + ProductId;
        lnkImg.NavigateUrl = detail;
        lnkName.NavigateUrl = detail;
        lnkName.Text = Server.HtmlEncode(ProductName);
        img.ImageUrl = ImageUrl;
        img.AlternateText = ProductName;
        litPrice.Text = "Rs. " + Price.ToString("N2");
        stars.Value = Rating;
        stars.Count = RatingCount;
        lnkAdd.NavigateUrl = "~/Member/Cart.aspx?add=" + ProductId;
        lnkWish.NavigateUrl = "~/Member/Wishlist.aspx?add=" + ProductId;
        if (Stock <= 0)
        {
            lblStock.Text = "Out of stock";
            lblStock.ForeColor = System.Drawing.ColorTranslator.FromHtml("#a8473f");
            lnkAdd.Visible = false;
        }
        else if (Stock <= 5)
        {
            lblStock.Text = "Only " + Stock + " left";
            lblStock.ForeColor = System.Drawing.ColorTranslator.FromHtml("#b98a2e");
        }
    }
}

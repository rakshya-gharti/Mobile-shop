using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                DataTable cats = Db.Table("SELECT CategoryId, Name FROM Categories ORDER BY Name");
                ddlCategory.DataSource = cats;
                ddlCategory.DataTextField = "Name";
                ddlCategory.DataValueField = "CategoryId";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("All Categories", "0"));
                BindProducts();
            }
            catch (Exception) { lblEmpty.Text = "Database is not ready."; lblEmpty.Visible = true; }
        }
    }

    private void BindProducts()
    {
        string order = "p.CreatedAt DESC";
        switch (ddlSort.SelectedValue)
        {
            case "low": order = "p.Price ASC"; break;
            case "high": order = "p.Price DESC"; break;
            case "rate": order = "AvgRating DESC"; break;
        }
        string sql = @"SELECT p.ProductId, p.Name, p.Price, p.ImageUrl, p.Stock,
                         ISNULL((SELECT AVG(CAST(Stars AS FLOAT)) FROM Ratings r WHERE r.ProductId=p.ProductId),0.0) AS AvgRating,
                         (SELECT COUNT(*) FROM Ratings r WHERE r.ProductId=p.ProductId) AS RatingCount
                       FROM Products p
                       WHERE (@cat=0 OR p.CategoryId=@cat) AND (p.Name LIKE @q OR p.Description LIKE @q)
                       ORDER BY " + order;
        DataTable dt = Db.Table(sql, Db.P("@cat", int.Parse(ddlCategory.SelectedValue)),
                                     Db.P("@q", "%" + txtSearch.Text.Trim() + "%"));
        rptProducts.DataSource = dt;
        rptProducts.DataBind();
        lblEmpty.Visible = dt.Rows.Count == 0;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindProducts();
    }
}

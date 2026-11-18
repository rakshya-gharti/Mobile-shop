using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class Member_Wishlist : System.Web.UI.Page
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
                Db.Exec(@"INSERT OR IGNORE INTO Wishlist (UserId, ProductId)
                          SELECT @u, ProductId FROM Products WHERE ProductId=@p", Db.P("@u", uid), Db.P("@p", add));
                Response.Redirect("Wishlist.aspx");
            }
            Bind();
        }
    }

    private void Bind()
    {
        DataTable dt = Db.Table(@"SELECT p.ProductId, p.Name, p.Price, p.ImageUrl FROM Wishlist w
                                  JOIN Products p ON p.ProductId=w.ProductId WHERE w.UserId=@u", Db.P("@u", uid));
        gvWish.DataSource = dt;
        gvWish.DataBind();
        lblEmpty.Visible = dt.Rows.Count == 0;
    }

    protected void gvWish_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Del")
        {
            Db.Exec("DELETE FROM Wishlist WHERE UserId=@u AND ProductId=@p", Db.P("@u", uid), Db.P("@p", e.CommandArgument));
            Bind();
        }
    }
}

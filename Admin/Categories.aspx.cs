using System;
using System.Web.UI.WebControls;

public partial class Admin_Categories : System.Web.UI.Page
{
    protected void gvCats_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        if (e.Exception != null)
        {
            lblMsg.Text = "This category still contains products. Move or delete them first.";
            lblMsg.Visible = true;
            e.ExceptionHandled = true;
        }
    }

    protected void dvNew_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        gvCats.DataBind();
    }
}

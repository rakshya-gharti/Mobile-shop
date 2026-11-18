using System;
using System.Web.UI.WebControls;

public partial class Admin_Users : System.Web.UI.Page
{
    protected void gvUsers_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        if (e.Exception != null)
        {
            lblMsg.Text = "Could not delete this user: " + e.Exception.Message;
            lblMsg.Visible = true;
            e.ExceptionHandled = true;
        }
        else if (e.AffectedRows == 0)
        {
            lblMsg.Text = "The built-in admin account cannot be deleted.";
            lblMsg.Visible = true;
        }
    }
}

using System;
using System.IO;
using System.Web.UI.WebControls;

public partial class Admin_Products : System.Web.UI.Page
{
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        string imageUrl = "~/Images/products/p1.svg";
        if (fuImage.HasFile)
        {
            string ext = Path.GetExtension(fuImage.FileName).ToLowerInvariant();
            if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".svg")
            {
                lblAdd.Text = "Only JPG, PNG or SVG images are allowed.";
                lblAdd.CssClass = "alert error";
                lblAdd.Visible = true;
                return;
            }
            string file = Guid.NewGuid().ToString("N") + ext;
            fuImage.SaveAs(Server.MapPath("~/Images/products/" + file));
            imageUrl = "~/Images/products/" + file;
        }

        Db.Exec(@"INSERT INTO Products (CategoryId, Name, Description, Price, Stock, ImageUrl) VALUES (@c,@n,@d,@p,@s,@i)",
            Db.P("@c", int.Parse(ddlCat.SelectedValue)), Db.P("@n", txtName.Text.Trim()), Db.P("@d", txtDesc.Text.Trim()),
            Db.P("@p", decimal.Parse(txtPrice.Text)), Db.P("@s", int.Parse(txtStock.Text)), Db.P("@i", imageUrl));
        gvProducts.DataBind();
        lblAdd.Text = "Product added.";
        lblAdd.CssClass = "alert ok";
        lblAdd.Visible = true;
        txtName.Text = txtDesc.Text = txtPrice.Text = txtStock.Text = "";
    }

    protected void gvProducts_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        if (e.Exception != null)
        {
            lblMsg.Text = "This product has been ordered before and cannot be deleted. Set its stock to 0 instead.";
            lblMsg.Visible = true;
            e.ExceptionHandled = true;
        }
    }
}

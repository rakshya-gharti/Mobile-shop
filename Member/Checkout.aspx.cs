using System;
using System.Data;
using Mono.Data.Sqlite;

public partial class Member_Checkout : System.Web.UI.Page
{
    int uid;
    decimal subtotal, discount, shipping;

    static readonly string[] Districts = {
        "Kathmandu","Lalitpur","Bhaktapur","Taplejung","Panchthar","Ilam","Jhapa","Morang","Sunsari","Dhankuta","Terhathum","Sankhuwasabha","Bhojpur","Khotang","Solukhumbu","Okhaldhunga","Udayapur","Saptari","Siraha","Dhanusha","Mahottari","Sarlahi","Rautahat","Bara","Parsa","Chitwan","Makwanpur","Dolakha","Ramechhap","Sindhuli","Kavrepalanchok","Sindhupalchok","Nuwakot","Rasuwa","Dhading","Gorkha","Lamjung","Tanahun","Kaski","Syangja","Parbat","Baglung","Myagdi","Mustang","Manang","Nawalparasi (East)","Nawalparasi (West)","Rupandehi","Kapilvastu","Palpa","Arghakhanchi","Gulmi","Pyuthan","Rolpa","Rukum (East)","Rukum (West)","Dang","Banke","Bardiya","Salyan","Surkhet","Dailekh","Jajarkot","Dolpa","Jumla","Kalikot","Mugu","Humla","Bajura","Bajhang","Achham","Doti","Kailali","Kanchanpur","Dadeldhura","Baitadi","Darchula"
    };

    protected void Page_Load(object sender, EventArgs e)
    {
        uid = Db.CurrentUserId();
        if (!IsPostBack)
        {
            ddlDistrict.DataSource = Districts;
            ddlDistrict.DataBind();
            DataTable u = Db.Table("SELECT FullName, Phone, Address FROM Users WHERE UserId=@u", Db.P("@u", uid));
            if (u.Rows.Count > 0)
            {
                txtName.Text = u.Rows[0]["FullName"].ToString();
                txtPhone.Text = u.Rows[0]["Phone"].ToString();
                txtTole.Text = u.Rows[0]["Address"].ToString();
            }
            txtCoupon.Text = Session["Coupon"] as string ?? "";
            Calculate();
            if (subtotal == 0) Response.Redirect("Cart.aspx");
        }
        else Calculate();
    }

    private void Calculate()
    {
        object s = Db.Scalar("SELECT ISNULL(SUM(c.Quantity*p.Price),0) FROM CartItems c JOIN Products p ON p.ProductId=c.ProductId WHERE c.UserId=@u", Db.P("@u", uid));
        subtotal = Convert.ToDecimal(s);
        discount = 0;
        string code = Session["Coupon"] as string;
        if (code != null)
        {
            object pct = Db.Scalar("SELECT Percent FROM Coupons WHERE Code=@c", Db.P("@c", code));
            if (pct != null) discount = Math.Round(subtotal * Convert.ToInt32(pct) / 100m, 2);
        }
        // Delivery: Kathmandu Valley Rs. 100, rest of Nepal Rs. 150
        string d = ddlDistrict.SelectedValue;
        shipping = (d == "Kathmandu" || d == "Lalitpur" || d == "Bhaktapur") ? 100m : 150m;
        litSub.Text = subtotal.ToString("N2");
        litDisc.Text = discount.ToString("N2");
        litShip.Text = shipping.ToString("N2");
        litTotal.Text = (subtotal - discount + shipping).ToString("N2");
    }

    protected void Refresh_Totals(object sender, EventArgs e) { Calculate(); }

    protected void btnCoupon_Click(object sender, EventArgs e)
    {
        string code = txtCoupon.Text.Trim().ToUpperInvariant();
        lblCoupon.Visible = true;
        if (code == "") { Session["Coupon"] = null; lblCoupon.Text = "Coupon removed."; lblCoupon.CssClass = "alert"; }
        else if (Db.Scalar("SELECT Percent FROM Coupons WHERE Code=@c", Db.P("@c", code)) != null)
        { Session["Coupon"] = code; lblCoupon.Text = "Coupon " + code + " applied."; lblCoupon.CssClass = "alert success"; }
        else { Session["Coupon"] = null; lblCoupon.Text = "Invalid coupon code."; lblCoupon.CssClass = "alert error"; }
        Calculate();
    }

    protected void rblPay_Changed(object sender, EventArgs e)
    {
        string m = rblPay.SelectedValue;
        bool cod = m == "Cash on Delivery";
        pnlWallet.Visible = !cod;
        pnlCod.Visible = cod;
        rfvWallet.Enabled = revWallet.Enabled = !cod;
        litWalletLabel.Text = m == "eSewa" ? "eSewa ID (registered mobile number)"
                            : m == "Khalti" ? "Khalti ID (registered mobile number)"
                            : "Mobile number linked to your bank account";
    }

    protected void btnPlace_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        DataTable items = Db.Table(@"SELECT c.ProductId, c.Quantity, p.Price, p.Stock, p.Name
                                     FROM CartItems c JOIN Products p ON p.ProductId=c.ProductId WHERE c.UserId=@u", Db.P("@u", uid));
        if (items.Rows.Count == 0) { Response.Redirect("Cart.aspx"); return; }
        foreach (DataRow r in items.Rows)
            if (Convert.ToInt32(r["Quantity"]) > Convert.ToInt32(r["Stock"]))
            {
                lblMsg.Text = "Not enough stock for " + r["Name"] + ". Please update your cart.";
                lblMsg.CssClass = "alert error";
                lblMsg.Visible = true;
                return;
            }

        int orderId;
        using (SqliteConnection c = Db.Open())
        {
            SqliteTransaction tx = c.BeginTransaction();
            try
            {
                SqliteCommand cmd = new SqliteCommand(@"INSERT INTO Orders (UserId, Total, Discount, Status, PaymentMethod, ShippingAddress)
                    VALUES (@u,@t,@d,@st,@pm,@a); SELECT last_insert_rowid();", c, tx);
                cmd.Parameters.AddWithValue("@u", uid);
                cmd.Parameters.AddWithValue("@t", subtotal - discount + shipping);
                cmd.Parameters.AddWithValue("@d", discount);
                cmd.Parameters.AddWithValue("@pm", rblPay.SelectedValue);
                cmd.Parameters.AddWithValue("@st", rblPay.SelectedValue == "Cash on Delivery" ? "Pending" : "Unpaid");
                cmd.Parameters.AddWithValue("@a", txtName.Text.Trim() + " (" + txtPhone.Text.Trim() + "), " + txtTole.Text.Trim()
                    + (txtWard.Text.Trim() != "" ? ", Ward " + txtWard.Text.Trim() : "") + ", " + txtCity.Text.Trim() + ", " + ddlDistrict.SelectedValue);
                orderId = Convert.ToInt32(cmd.ExecuteScalar());

                foreach (DataRow r in items.Rows)
                {
                    SqliteCommand ic = new SqliteCommand(@"INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@o,@p,@q,@pr)", c, tx);
                    ic.Parameters.AddWithValue("@o", orderId);
                    ic.Parameters.AddWithValue("@p", r["ProductId"]);
                    ic.Parameters.AddWithValue("@q", r["Quantity"]);
                    ic.Parameters.AddWithValue("@pr", r["Price"]);
                    ic.ExecuteNonQuery();
                    SqliteCommand st = new SqliteCommand("UPDATE Products SET Stock = Stock - @q WHERE ProductId=@p", c, tx);
                    st.Parameters.AddWithValue("@p", r["ProductId"]);
                    st.Parameters.AddWithValue("@q", r["Quantity"]);
                    st.ExecuteNonQuery();
                }
                SqliteCommand del = new SqliteCommand("DELETE FROM CartItems WHERE UserId=@u", c, tx);
                del.Parameters.AddWithValue("@u", uid);
                del.ExecuteNonQuery();
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                lblMsg.Text = "Sorry, the order could not be completed. Please try again.";
                lblMsg.CssClass = "alert error";
                lblMsg.Visible = true;
                return;
            }
        }

        Session["Coupon"] = null;
        if (rblPay.SelectedValue != "Cash on Delivery")
        {
            // online payment: stock is reserved, order stays Unpaid until the gateway confirms
            Session["PayMobile"] = txtWallet.Text.Trim();
            Response.Redirect("PayGateway.aspx?order=" + orderId);
            return;
        }

        decimal total = subtotal - discount + shipping;
        int points = (int)(total / 10m);
        Db.Exec("UPDATE Users SET Points = Points + @p WHERE UserId=@u", Db.P("@p", points), Db.P("@u", uid));
        Db.Notify(uid, "Order #" + orderId + " placed successfully (Cash on Delivery). You earned " + points + " points.");
        object email = Db.Scalar("SELECT Email FROM Users WHERE UserId=@u", Db.P("@u", uid));
        Db.SendMail(email.ToString(), "GlowUp order #" + orderId + " confirmation",
            "Thank you for your purchase. Total to pay on delivery: Rs. " + total.ToString("N2") + ". Status: Pending.");

        litOrder.Text = orderId.ToString();
        litMethod.Text = rblPay.SelectedValue;
        pnlForm.Visible = false;
        pnlDone.Visible = true;
    }
}

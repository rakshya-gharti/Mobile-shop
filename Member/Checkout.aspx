<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Checkout.aspx.cs" Inherits="Member_Checkout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Checkout</h1>
    <asp:Label ID="lblMsg" runat="server" Visible="false" />
    <asp:Panel ID="pnlForm" runat="server">
        <div class="panel">
            <h2>1. Delivery address</h2>
            <div class="row">
                <div><label>Full name</label>
                    <asp:TextBox ID="txtName" runat="server" MaxLength="80" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" ErrorMessage="Name is required." CssClass="err" Display="Dynamic" /></div>
                <div><label>Mobile number</label>
                    <asp:TextBox ID="txtPhone" runat="server" MaxLength="10" placeholder="98XXXXXXXX" />
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPhone" ValidationExpression="^9[78]\d{8}$" ErrorMessage="Enter a valid Nepali mobile number (98/97 + 8 digits)." CssClass="err" Display="Dynamic" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPhone" ErrorMessage="Mobile number is required." CssClass="err" Display="Dynamic" /></div>
            </div>
            <div class="row">
                <div><label>District</label>
                    <asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Refresh_Totals" /></div>
                <div><label>Municipality / City</label>
                    <asp:TextBox ID="txtCity" runat="server" MaxLength="60" placeholder="e.g. Kathmandu Metropolitan City" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCity" ErrorMessage="City is required." CssClass="err" Display="Dynamic" /></div>
            </div>
            <div class="row">
                <div><label>Ward no.</label>
                    <asp:TextBox ID="txtWard" runat="server" MaxLength="2" placeholder="e.g. 4" />
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtWard" ValidationExpression="^\d{1,2}$" ErrorMessage="Ward must be a number." CssClass="err" Display="Dynamic" /></div>
                <div><label>Tole / Street / Nearest landmark</label>
                    <asp:TextBox ID="txtTole" runat="server" MaxLength="150" placeholder="e.g. Shantinagar, near Bhatbhateni" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTole" ErrorMessage="Tole / street is required." CssClass="err" Display="Dynamic" /></div>
            </div>
        </div>

        <div class="panel">
            <h2>2. Payment method</h2>
            <div class="paycards"><asp:RadioButtonList ID="rblPay" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblPay_Changed" RepeatLayout="UnorderedList">
                <asp:ListItem Value="eSewa" Selected="True">eSewa</asp:ListItem>
                <asp:ListItem Value="Khalti">Khalti</asp:ListItem>
                <asp:ListItem Value="Connect IPS">Connect IPS (bank transfer)</asp:ListItem>
                <asp:ListItem Value="Cash on Delivery">Cash on Delivery</asp:ListItem>
            </asp:RadioButtonList></div>
            <asp:Panel ID="pnlWallet" runat="server">
                <br />
                <label><asp:Literal ID="litWalletLabel" runat="server" Text="eSewa ID (registered mobile number)" /></label>
                <asp:TextBox ID="txtWallet" runat="server" MaxLength="10" placeholder="98XXXXXXXX" />
                <asp:RequiredFieldValidator ID="rfvWallet" runat="server" ControlToValidate="txtWallet" ErrorMessage="Enter your mobile number." CssClass="err" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revWallet" runat="server" ControlToValidate="txtWallet" ValidationExpression="^9[78]\d{8}$" ErrorMessage="Enter a valid Nepali mobile number." CssClass="err" Display="Dynamic" />
                <p class="muted">After you place the order you will be redirected to the payment page to confirm with your PIN and OTP (demo gateway &ndash; no real money is charged).</p>
            </asp:Panel>
            <asp:Panel ID="pnlCod" runat="server" Visible="false">
                <p class="muted">Pay in cash to the delivery rider when your order arrives.</p>
            </asp:Panel>
        </div>

        <div class="panel">
            <h2>3. Coupon &amp; order summary</h2>
            <div class="row">
                <div><asp:TextBox ID="txtCoupon" runat="server" MaxLength="30" placeholder="Coupon code (e.g. WELCOME10)" /></div>
                <div style="flex:0 0 auto"><asp:Button ID="btnCoupon" runat="server" Text="Apply" CausesValidation="false" OnClick="btnCoupon_Click" /></div>
            </div>
            <asp:Label ID="lblCoupon" runat="server" Visible="false" />
            <p>Subtotal: Rs. <asp:Literal ID="litSub" runat="server" /></p>
            <p>Discount: - Rs. <asp:Literal ID="litDisc" runat="server" /></p>
            <p>Delivery charge: Rs. <asp:Literal ID="litShip" runat="server" /></p>
            <h2>Total: Rs. <asp:Literal ID="litTotal" runat="server" /></h2>
            <asp:Button ID="btnPlace" runat="server" Text="Place Order" OnClick="btnPlace_Click" />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlDone" runat="server" Visible="false" CssClass="panel">
        <h2><svg class="ic" aria-hidden="true"><use href="#i-circle-check"/></svg> Order placed</h2>
        <p>Your order <b>#<asp:Literal ID="litOrder" runat="server" /></b> has been placed (<asp:Literal ID="litMethod" runat="server" />). A confirmation email/notification has been sent.</p>
        <a class="btn" href="Orders.aspx">View my orders</a>
    </asp:Panel>
</asp:Content>

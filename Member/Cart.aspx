<%@ Page Title="My Cart" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Cart.aspx.cs" Inherits="Member_Cart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Shopping Cart</h1>
    <asp:Label ID="lblMsg" runat="server" Visible="false" />
    <asp:Repeater ID="rptCart" runat="server" OnItemCommand="rptCart_ItemCommand">
        <ItemTemplate>
            <div class="cartrow">
                <img src='<%# ResolveUrl(Eval("ImageUrl").ToString()) %>' alt="" />
                <div style="flex:1">
                    <b><%# Eval("Name") %></b><br />
                    Rs. <%# Eval("Price", "{0:N2}") %> &times;
                    <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("CartItemId") %>' />
                    <asp:TextBox ID="txtQty" runat="server" CssClass="qty" MaxLength="2" Text='<%# Eval("Quantity") %>' />
                    <asp:RangeValidator runat="server" ControlToValidate="txtQty" Type="Integer" MinimumValue="1" MaximumValue="99" ErrorMessage="1-99" CssClass="err" Display="Dynamic" />
                </div>
                <asp:LinkButton runat="server" CommandName="Remove" CommandArgument='<%# Eval("CartItemId") %>' CssClass="btn danger small" CausesValidation="false">Remove</asp:LinkButton>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Panel ID="pnlSummary" runat="server" CssClass="panel">
        <div class="row">
            <asp:TextBox ID="txtCoupon" runat="server" placeholder="Coupon code" />
            <asp:Button ID="btnCoupon" runat="server" Text="Apply" OnClick="btnCoupon_Click" CausesValidation="false" />
        </div>
        <p>Subtotal: Rs. <asp:Literal ID="litSub" runat="server" /></p>
        <p>Discount: - Rs. <asp:Literal ID="litDisc" runat="server" /></p>
        <h2>Total: Rs. <asp:Literal ID="litTotal" runat="server" /></h2>
        <div class="row">
            <asp:Button ID="btnUpdate" runat="server" Text="Update Cart" OnClick="btnUpdate_Click" />
            <a class="btn alt" href="Checkout.aspx">Checkout</a>
        </div>
    </asp:Panel>
    <asp:Label ID="lblEmpty" runat="server" Text="Your cart is empty." Visible="false" />
    <p><a href="../Default.aspx">&larr; Continue shopping</a></p>
</asp:Content>

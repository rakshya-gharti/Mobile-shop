<%@ Page Title="Product" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProductDetail.aspx.cs" Inherits="ProductDetail" %>
<%@ Register Src="~/Controls/StarRating.ascx" TagPrefix="uc" TagName="Stars" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="pnlProduct" runat="server" CssClass="panel">
        <asp:Image ID="imgProduct" runat="server" />
        <h1><asp:Literal ID="litName" runat="server" /></h1>
        <uc:Stars ID="stars" runat="server" />
        <p class="price" style="font-size:1.4rem"><asp:Literal ID="litPrice" runat="server" /></p>
        <p><asp:Literal ID="litDesc" runat="server" /></p>
        <p><small>Category: <asp:Literal ID="litCat" runat="server" /> | <asp:Literal ID="litStock" runat="server" /></small></p>
        <div class="row">
            <asp:HyperLink ID="lnkAdd" runat="server" CssClass="btn">Add to Cart</asp:HyperLink>
            <asp:HyperLink ID="lnkWish" runat="server" CssClass="btn alt"><svg class="ic" aria-hidden="true"><use href="#i-heart"/></svg> Wishlist</asp:HyperLink>
        </div>
    </asp:Panel>
    <asp:Label ID="lblMissing" runat="server" Text="Product not found." Visible="false" CssClass="alert error" />
    <h2>Customer Reviews</h2>
    <asp:Repeater ID="rptReviews" runat="server">
        <ItemTemplate>
            <div class="panel">
                <b><%# Eval("FullName") %></b> <span class="stars"><%# Icons.Stars((int)Eval("Stars")) %></span>
                <div><%# Server.HtmlEncode(Convert.ToString(Eval("Comment"))) %></div>
                <small><%# Eval("CreatedAt", "{0:dd MMM yyyy}") %></small>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Label ID="lblNoReviews" runat="server" Text="No reviews yet." Visible="false" />
    <h2>Recommended for you</h2>
    <div class="grid">
        <asp:Repeater ID="rptRelated" runat="server">
            <ItemTemplate>
                <div class="card"><a href='ProductDetail.aspx?id=<%# Eval("ProductId") %>'>
                    <img src='<%# ResolveUrl(Eval("ImageUrl").ToString()) %>' alt="" /></a>
                    <div class="body"><b><%# Eval("Name") %></b><div class="price">Rs. <%# Eval("Price", "{0:N2}") %></div></div></div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>

<%@ Page Title="Wishlist" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Wishlist.aspx.cs" Inherits="Member_Wishlist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1><svg class="ic" aria-hidden="true"><use href="#i-heart"/></svg> My Wishlist</h1>
    <asp:Label ID="lblEmpty" runat="server" Text="Your wishlist is empty." Visible="false" />
    <asp:GridView ID="gvWish" runat="server" AutoGenerateColumns="false" CssClass="data" GridLines="None"
        DataKeyNames="ProductId" OnRowCommand="gvWish_RowCommand">
        <Columns>
            <asp:ImageField DataImageUrlField="ImageUrl" ControlStyle-Width="60px" HeaderText="" />
            <asp:HyperLinkField DataTextField="Name" DataNavigateUrlFields="ProductId" DataNavigateUrlFormatString="~/ProductDetail.aspx?id={0}" HeaderText="Product" />
            <asp:BoundField DataField="Price" HeaderText="Price (Rs.)" DataFormatString="{0:N2}" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# "Cart.aspx?add=" + Eval("ProductId") %>' CssClass="btn small">Add to cart</asp:HyperLink>
                    <asp:LinkButton runat="server" CommandName="Del" CommandArgument='<%# Eval("ProductId") %>' CssClass="btn small danger">Remove</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>

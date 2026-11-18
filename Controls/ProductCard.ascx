<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductCard.ascx.cs" Inherits="Controls_ProductCard" %>
<%@ Register Src="~/Controls/StarRating.ascx" TagPrefix="uc" TagName="Stars" %>
<div class="card">
    <asp:HyperLink ID="lnkImg" runat="server"><asp:Image ID="img" runat="server" /></asp:HyperLink>
    <div class="body">
        <h3><asp:HyperLink ID="lnkName" runat="server" /></h3>
        <uc:Stars ID="stars" runat="server" />
        <div class="price"><asp:Literal ID="litPrice" runat="server" /></div>
        <asp:Label ID="lblStock" runat="server" />
        <div class="row" style="margin-top:6px">
            <asp:HyperLink ID="lnkAdd" runat="server" CssClass="btn small">Add to Cart</asp:HyperLink>
            <asp:HyperLink ID="lnkWish" runat="server" CssClass="btn small alt"><svg class="ic" aria-hidden="true"><use href="#i-heart"/></svg> Save</asp:HyperLink>
        </div>
    </div>
</div>

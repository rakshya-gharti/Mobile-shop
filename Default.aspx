<%@ Page Title="Catalog" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Src="~/Controls/ProductCard.ascx" TagPrefix="uc" TagName="Card" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="banner">
      <div class="track">
        <div class="slide a active">
            <div class="pic" style="background-image:url('Images/hero1.jpg');background-position:50% 72%"></div>
            <div class="copy">
                <span class="eyebrow">New to GlowUp</span>
                <h2>Skincare that works as hard as you do</h2>
                <p>Thoughtfully formulated essentials for a calm, radiant complexion. Enjoy 10% off your first order with code WELCOME10.</p>
                <a href="#products" class="btn">Shop the collection</a>
            </div>
        </div>
        <div class="slide b">
            <div class="pic" style="background-image:url('Images/hero2.jpg');background-position:50% 60%"></div>
            <div class="copy">
                <span class="eyebrow">GlowUp Rewards</span>
                <h2>Be rewarded for every ritual</h2>
                <p>Earn 1 point for every RM10 spent and redeem it on your next order.</p>
                <a href="Register.aspx" class="btn alt">Join now</a>
            </div>
        </div>
      </div>
        <div class="progress"><i></i></div>
        <button type="button" class="arrow prev" aria-label="Previous slide"><svg class="ic" aria-hidden="true"><use href="#i-chevron-left"/></svg></button>
        <button type="button" class="arrow next" aria-label="Next slide"><svg class="ic" aria-hidden="true"><use href="#i-chevron-right"/></svg></button>
    </div>
    <div class="panel">
        <div class="row">
            <asp:TextBox ID="txtSearch" runat="server" placeholder="Search serums, moisturizers, makeup..." />
            <asp:DropDownList ID="ddlCategory" runat="server" />
        </div>
        <div class="row">
            <asp:DropDownList ID="ddlSort" runat="server">
                <asp:ListItem Value="new">Newest</asp:ListItem>
                <asp:ListItem Value="low">Price: Low to High</asp:ListItem>
                <asp:ListItem Value="high">Price: High to Low</asp:ListItem>
                <asp:ListItem Value="rate">Top Rated</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
        </div>
    </div>
    <h2 id="products">Our Collection</h2>
    <asp:Label ID="lblEmpty" runat="server" Text="No products found." Visible="false" />
    <div class="grid">
        <asp:Repeater ID="rptProducts" runat="server">
            <ItemTemplate>
                <uc:Card runat="server" ProductId='<%# Eval("ProductId") %>' ProductName='<%# Eval("Name") %>'
                    Price='<%# Eval("Price") %>' ImageUrl='<%# Eval("ImageUrl") %>' Stock='<%# Eval("Stock") %>'
                    Rating='<%# Eval("AvgRating") %>' RatingCount='<%# Eval("RatingCount") %>' />
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>

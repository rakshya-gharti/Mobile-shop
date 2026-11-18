<%@ Page Title="Rate Product" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Rate.aspx.cs" Inherits="Member_Rate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel">
        <h1>Rate: <asp:Literal ID="litProduct" runat="server" /></h1>
        <asp:Label ID="lblMsg" runat="server" Visible="false" />
        <asp:Panel ID="pnlForm" runat="server">
            <label>Your rating</label>
            <asp:RadioButtonList ID="rblStars" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="1">1 star</asp:ListItem>
                <asp:ListItem Value="2">2 stars</asp:ListItem>
                <asp:ListItem Value="3">3 stars</asp:ListItem>
                <asp:ListItem Value="4">4 stars</asp:ListItem>
                <asp:ListItem Value="5" Selected="True">5 stars</asp:ListItem>
            </asp:RadioButtonList>
            <label>Comment</label>
            <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Rows="3" MaxLength="300" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtComment" ErrorMessage="Please write a short comment." CssClass="err" Display="Dynamic" />
            <asp:Button ID="btnSave" runat="server" Text="Submit Rating" OnClick="btnSave_Click" />
        </asp:Panel>
        <p><a href="Orders.aspx">&larr; Back to orders</a></p>
    </div>
</asp:Content>

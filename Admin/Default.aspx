<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Admin Dashboard</h1>
    <div>
        <div class="stat"><b><asp:Literal ID="litUsers" runat="server" /></b>Members</div>
        <div class="stat"><b><asp:Literal ID="litOrders" runat="server" /></b>Orders</div>
        <div class="stat"><b><asp:Literal ID="litPending" runat="server" /></b>Pending</div>
        <div class="stat"><b>Rs. <asp:Literal ID="litSales" runat="server" /></b>Sales</div>
    </div>
    <h2><svg class="ic" aria-hidden="true"><use href="#i-triangle-alert"/></svg> Low stock alerts (5 or fewer)</h2>
    <div class="scroll">
    <asp:GridView ID="gvLow" runat="server" AutoGenerateColumns="false" CssClass="data" GridLines="None" EmptyDataText="All products are well stocked.">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Product" />
            <asp:BoundField DataField="Stock" HeaderText="Stock" />
        </Columns>
    </asp:GridView></div>
    <h2>Sales by category</h2>
    <div class="scroll">
    <asp:GridView ID="gvSales" runat="server" AutoGenerateColumns="false" CssClass="data" GridLines="None" EmptyDataText="No sales yet.">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Category" />
            <asp:BoundField DataField="Sales" HeaderText="Sales (Rs.)" DataFormatString="{0:N2}" />
        </Columns>
    </asp:GridView></div>
    <h2>Customer feedback</h2>
    <div class="scroll">
    <asp:GridView ID="gvFeedback" runat="server" AutoGenerateColumns="false" CssClass="data" GridLines="None" EmptyDataText="No feedback yet.">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="Message" HeaderText="Message" HtmlEncode="true" />
            <asp:BoundField DataField="CreatedAt" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" />
        </Columns>
    </asp:GridView></div>
</asp:Content>

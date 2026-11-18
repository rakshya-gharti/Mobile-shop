<%@ Page Title="Manage Orders" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Orders.aspx.cs" Inherits="Admin_Orders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Manage Orders</h1>
    <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="alert ok" />
    <div class="panel">
        <label>Filter by status</label>
        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_Changed">
            <asp:ListItem Value="">All</asp:ListItem>
            <asp:ListItem Value="Unpaid">Unpaid (awaiting payment)</asp:ListItem>
            <asp:ListItem Value="Pending">Pending</asp:ListItem>
            <asp:ListItem Value="Delivery">Sent out for delivery</asp:ListItem>
            <asp:ListItem Value="Delivered">Delivered</asp:ListItem>
            <asp:ListItem Value="Cancelled">Cancelled</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="scroll">
    <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" CssClass="data" GridLines="None"
        DataKeyNames="OrderId" OnRowCommand="gvOrders_RowCommand" EmptyDataText="No orders found." AllowPaging="true" PageSize="10" OnPageIndexChanging="gvOrders_PageIndexChanging">
        <Columns>
            <asp:BoundField DataField="OrderId" HeaderText="#" />
            <asp:BoundField DataField="FullName" HeaderText="Customer" />
            <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" />
            <asp:BoundField DataField="Total" HeaderText="Total (Rs.)" DataFormatString="{0:N2}" />
            <asp:TemplateField HeaderText="Status"><ItemTemplate><span class='status <%# Eval("Status") %>'><%# Eval("Status") %></span></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:LinkButton runat="server" CommandName="Set" CommandArgument='<%# Eval("OrderId") + "|Delivery" %>' CssClass="btn small" Visible='<%# Eval("Status").ToString() == "Pending" %>'>Ship</asp:LinkButton>
                    <asp:LinkButton runat="server" CommandName="Set" CommandArgument='<%# Eval("OrderId") + "|Delivered" %>' CssClass="btn small" Visible='<%# Eval("Status").ToString() == "Delivery" %>'>Mark delivered</asp:LinkButton>
                    <asp:LinkButton runat="server" CommandName="Set" CommandArgument='<%# Eval("OrderId") + "|Cancelled" %>' CssClass="btn small danger"
                        OnClientClick="return confirm('Cancel this order?');" Visible='<%# Eval("Status").ToString() == "Unpaid" || Eval("Status").ToString() == "Pending" || Eval("Status").ToString() == "Delivery" %>'>Cancel</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView></div>
</asp:Content>

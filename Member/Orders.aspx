<%@ Page Title="My Orders" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Orders.aspx.cs" Inherits="Member_Orders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Purchase History &amp; Delivery</h1>
    <asp:Label ID="lblMsg" runat="server" Visible="false" />
    <asp:Label ID="lblEmpty" runat="server" Text="You have no orders yet." Visible="false" />
    <asp:Repeater ID="rptOrders" runat="server" OnItemCommand="rptOrders_ItemCommand" OnItemDataBound="rptOrders_ItemDataBound">
        <ItemTemplate>
            <div class="panel">
                <div class="row">
                    <b>Order #<%# Eval("OrderId") %></b>
                    <span style="text-align:right"><span class='status <%# Eval("Status") %>'><%# Eval("Status") %></span></span>
                </div>
                <small><%# Eval("OrderDate", "{0:dd MMM yyyy HH:mm}") %> &middot; <%# Eval("PaymentMethod") %></small>
                <asp:Repeater ID="rptItems" runat="server">
                    <ItemTemplate>
                        <div class="row" style="margin:4px 0">
                            <span><%# Eval("Name") %> &times; <%# Eval("Quantity") %></span>
                            <span style="text-align:right">Rs. <%# Eval("LineTotal", "{0:N2}") %>
                                <asp:HyperLink runat="server" Visible='<%# Convert.ToInt32(Eval("CanRate")) == 1 %>' CssClass="btn small alt"
                                    NavigateUrl='<%# "Rate.aspx?order=" + Eval("OrderId") + "&product=" + Eval("ProductId") %>'><svg class="ic" aria-hidden="true"><use href="#i-star"/></svg> Rate</asp:HyperLink></span>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <p><b>Total: Rs. <%# Eval("Total", "{0:N2}") %></b></p>
                <%-- Delivery tracker --%>
                <p><small>Tracking: Pending &rarr; Sent out for delivery &rarr; Delivered</small></p>
                <asp:HyperLink runat="server" CssClass="btn small" Visible='<%# Eval("Status").ToString() == "Unpaid" %>'
                    NavigateUrl='<%# "PayGateway.aspx?order=" + Eval("OrderId") %>'>Pay now</asp:HyperLink>
                <asp:LinkButton runat="server" CommandName="Cancel" CommandArgument='<%# Eval("OrderId") %>'
                    Visible='<%# Eval("Status").ToString() == "Pending" || Eval("Status").ToString() == "Unpaid" %>' CssClass="btn danger small"
                    OnClientClick="return confirm('Cancel this order?');" CausesValidation="false">Cancel order</asp:LinkButton>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <h2>Notifications</h2>
    <asp:Repeater ID="rptNotes" runat="server">
        <ItemTemplate><div class="panel"><%# Server.HtmlEncode(Convert.ToString(Eval("Message"))) %><br /><small><%# Eval("CreatedAt", "{0:dd MMM HH:mm}") %></small></div></ItemTemplate>
    </asp:Repeater>
</asp:Content>

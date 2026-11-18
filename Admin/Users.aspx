<%@ Page Title="Manage Users" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="Admin_Users" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Manage Users</h1>
    <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="alert error" />
    <div class="scroll">
    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" DataKeyNames="UserId" DataSourceID="sdsUsers"
        CssClass="data" GridLines="None" AllowPaging="true" PageSize="10" OnRowDeleted="gvUsers_RowDeleted">
        <Columns>
            <asp:BoundField DataField="UserId" HeaderText="ID" ReadOnly="true" />
            <asp:BoundField DataField="Username" HeaderText="Username" ReadOnly="true" />
            <asp:BoundField DataField="FullName" HeaderText="Name" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="Phone" HeaderText="Phone" />
            <asp:TemplateField HeaderText="Role">
                <ItemTemplate><%# Eval("Role") %></ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlRole" runat="server" SelectedValue='<%# Bind("Role") %>'>
                        <asp:ListItem>Member</asp:ListItem><asp:ListItem>Admin</asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Points" HeaderText="Points" />
            <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
        </Columns>
    </asp:GridView></div>
    <asp:SqlDataSource ID="sdsUsers" runat="server" ConnectionString="<%$ ConnectionStrings:ShopDB %>" ProviderName="Mono.Data.Sqlite"
        SelectCommand="SELECT UserId, Username, FullName, Email, Phone, Role, Points FROM Users ORDER BY UserId"
        UpdateCommand="UPDATE Users SET FullName=@FullName, Email=@Email, Phone=@Phone, Role=@Role, Points=@Points WHERE UserId=@UserId"
        DeleteCommand="DELETE FROM Users WHERE UserId=@UserId AND Username<>'admin'" />
</asp:Content>

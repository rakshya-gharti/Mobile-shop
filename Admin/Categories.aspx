<%@ Page Title="Manage Categories" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Categories.aspx.cs" Inherits="Admin_Categories" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Manage Categories</h1>
    <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="alert error" />
    <div class="scroll">
    <asp:GridView ID="gvCats" runat="server" AutoGenerateColumns="false" DataKeyNames="CategoryId" DataSourceID="sdsCats"
        CssClass="data" GridLines="None" OnRowDeleted="gvCats_RowDeleted">
        <Columns>
            <asp:BoundField DataField="CategoryId" HeaderText="ID" ReadOnly="true" />
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
        </Columns>
    </asp:GridView></div>
    <h2>Add Category</h2>
    <asp:DetailsView ID="dvNew" runat="server" DataSourceID="sdsCats" DefaultMode="Insert" AutoGenerateRows="false"
        DataKeyNames="CategoryId" CssClass="data" GridLines="None" OnItemInserted="dvNew_ItemInserted">
        <Fields>
            <asp:TemplateField HeaderText="Name">
                <InsertItemTemplate>
                    <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>' MaxLength="60" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" ErrorMessage="Name required." CssClass="err" Display="Dynamic" />
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:CommandField ShowInsertButton="true" ButtonType="Button" />
        </Fields>
    </asp:DetailsView>
    <asp:SqlDataSource ID="sdsCats" runat="server" ConnectionString="<%$ ConnectionStrings:ShopDB %>" ProviderName="Mono.Data.Sqlite"
        SelectCommand="SELECT CategoryId, Name, Description FROM Categories ORDER BY Name"
        InsertCommand="INSERT INTO Categories (Name, Description) VALUES (@Name, @Description)"
        UpdateCommand="UPDATE Categories SET Name=@Name, Description=@Description WHERE CategoryId=@CategoryId"
        DeleteCommand="DELETE FROM Categories WHERE CategoryId=@CategoryId" />
</asp:Content>

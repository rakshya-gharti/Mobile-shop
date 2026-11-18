<%@ Page Title="Manage Products" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Products.aspx.cs" Inherits="Admin_Products" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Manage Products</h1>
    <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="alert error" />
    <div class="scroll">
    <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="false" DataKeyNames="ProductId" DataSourceID="sdsProducts"
        CssClass="data" GridLines="None" AllowPaging="true" PageSize="8" OnRowDeleted="gvProducts_RowDeleted">
        <Columns>
            <asp:BoundField DataField="ProductId" HeaderText="ID" ReadOnly="true" />
            <asp:ImageField DataImageUrlField="ImageUrl" ControlStyle-Width="50px" ReadOnly="true" />
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:0.00}" ApplyFormatInEditMode="true" />
            <asp:BoundField DataField="Stock" HeaderText="Stock" />
            <asp:TemplateField HeaderText="Category">
                <ItemTemplate><%# Eval("CategoryName") %></ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlCat" runat="server" DataSourceID="sdsCats" DataTextField="Name" DataValueField="CategoryId" SelectedValue='<%# Bind("CategoryId") %>' />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
        </Columns>
    </asp:GridView></div>

    <h2>Add New Product</h2>
    <div class="panel">
        <asp:Label ID="lblAdd" runat="server" Visible="false" />
        <label>Name</label><asp:TextBox ID="txtName" runat="server" MaxLength="100" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" ValidationGroup="add" ErrorMessage="Name required." CssClass="err" Display="Dynamic" />
        <label>Category</label><asp:DropDownList ID="ddlCat" runat="server" DataSourceID="sdsCats" DataTextField="Name" DataValueField="CategoryId" />
        <label>Description</label><asp:TextBox ID="txtDesc" runat="server" TextMode="MultiLine" Rows="2" MaxLength="500" />
        <div class="row">
            <div><label>Price (Rs.)</label><asp:TextBox ID="txtPrice" runat="server" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPrice" ValidationGroup="add" ErrorMessage="Price required." CssClass="err" Display="Dynamic" />
                <asp:RangeValidator runat="server" ControlToValidate="txtPrice" ValidationGroup="add" Type="Double" MinimumValue="0.01" MaximumValue="100000" ErrorMessage="Invalid price." CssClass="err" Display="Dynamic" /></div>
            <div><label>Stock</label><asp:TextBox ID="txtStock" runat="server" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtStock" ValidationGroup="add" ErrorMessage="Stock required." CssClass="err" Display="Dynamic" />
                <asp:RangeValidator runat="server" ControlToValidate="txtStock" ValidationGroup="add" Type="Integer" MinimumValue="0" MaximumValue="100000" ErrorMessage="Invalid stock." CssClass="err" Display="Dynamic" /></div>
        </div>
        <label>Image (JPG/PNG/SVG, optional)</label><asp:FileUpload ID="fuImage" runat="server" />
        <br /><br />
        <asp:Button ID="btnAdd" runat="server" Text="Add Product" ValidationGroup="add" OnClick="btnAdd_Click" />
    </div>

    <asp:SqlDataSource ID="sdsCats" runat="server" ConnectionString="<%$ ConnectionStrings:ShopDB %>" ProviderName="Mono.Data.Sqlite" SelectCommand="SELECT CategoryId, Name FROM Categories ORDER BY Name" />
    <asp:SqlDataSource ID="sdsProducts" runat="server" ConnectionString="<%$ ConnectionStrings:ShopDB %>" ProviderName="Mono.Data.Sqlite"
        SelectCommand="SELECT p.ProductId, p.Name, p.Description, p.Price, p.Stock, p.ImageUrl, p.CategoryId, c.Name AS CategoryName FROM Products p JOIN Categories c ON c.CategoryId=p.CategoryId ORDER BY p.ProductId"
        UpdateCommand="UPDATE Products SET Name=@Name, Description=@Description, Price=@Price, Stock=@Stock, CategoryId=@CategoryId WHERE ProductId=@ProductId"
        DeleteCommand="DELETE FROM Products WHERE ProductId=@ProductId" />
</asp:Content>

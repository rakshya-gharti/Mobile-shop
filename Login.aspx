<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel">
        <h1>Login</h1>
        <asp:Label ID="lblMsg" runat="server" Visible="false" />
        <label>Username</label>
        <asp:TextBox ID="txtUser" runat="server" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUser" ErrorMessage="Username is required." CssClass="err" Display="Dynamic" />
        <label>Password</label>
        <asp:TextBox ID="txtPass" runat="server" TextMode="Password" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPass" ErrorMessage="Password is required." CssClass="err" Display="Dynamic" />
        <asp:CheckBox ID="chkRemember" runat="server" Text="Remember me" /><br /><br />
        <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
        <p>New here? <a href="Register.aspx">Create an account</a></p>
    </div>
</asp:Content>

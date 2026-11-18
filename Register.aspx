<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel">
        <h1>Create Account</h1>
        <asp:Label ID="lblMsg" runat="server" Visible="false" />
        <asp:ValidationSummary ID="vs" runat="server" CssClass="err" />

        <label>Username</label>
        <asp:TextBox ID="txtUser" runat="server" MaxLength="50" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUser" ErrorMessage="Username is required." Display="Dynamic" CssClass="err" Text="*" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtUser" ValidationExpression="^[A-Za-z0-9_]{4,20}$" ErrorMessage="Username: 4-20 letters, digits or underscore." Display="Dynamic" CssClass="err" Text="*" />

        <label>Full name</label>
        <asp:TextBox ID="txtName" runat="server" MaxLength="100" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" ErrorMessage="Full name is required." Display="Dynamic" CssClass="err" Text="*" />

        <label>Email</label>
        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" MaxLength="100" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required." Display="Dynamic" CssClass="err" Text="*" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="Email format is invalid." Display="Dynamic" CssClass="err" Text="*" />

        <label>Phone</label>
        <asp:TextBox ID="txtPhone" runat="server" MaxLength="20" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPhone" ValidationExpression="^[0-9+\- ]{8,15}$" ErrorMessage="Phone must be 8-15 digits." Display="Dynamic" CssClass="err" Text="*" />

        <label>Delivery address</label>
        <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Rows="2" MaxLength="250" />

        <label>Password</label>
        <asp:TextBox ID="txtPass" runat="server" TextMode="Password" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPass" ErrorMessage="Password is required." Display="Dynamic" CssClass="err" Text="*" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPass" ValidationExpression="^(?=.*[A-Za-z])(?=.*\d).{6,}$" ErrorMessage="Password needs 6+ characters with letters and digits." Display="Dynamic" CssClass="err" Text="*" />

        <label>Confirm password</label>
        <asp:TextBox ID="txtPass2" runat="server" TextMode="Password" />
        <asp:CompareValidator runat="server" ControlToValidate="txtPass2" ControlToCompare="txtPass" ErrorMessage="Passwords do not match." Display="Dynamic" CssClass="err" Text="*" />

        <asp:Button ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click" />
        <p>Already a member? <a href="Login.aspx">Login</a></p>
    </div>
</asp:Content>

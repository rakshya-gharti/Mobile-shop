<%@ Page Title="My Profile" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Profile.aspx.cs" Inherits="Member_Profile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel">
        <h1>My Profile</h1>
        <p><svg class="ic" aria-hidden="true"><use href="#i-trophy"/></svg> Reward points: <b><asp:Literal ID="litPoints" runat="server" /></b> &nbsp; (<asp:Literal ID="litBadge" runat="server" />)</p>
        <asp:Label ID="lblMsg" runat="server" Visible="false" />
        <label>Username</label><asp:TextBox ID="txtUser" runat="server" ReadOnly="true" />
        <label>Full name</label><asp:TextBox ID="txtName" runat="server" MaxLength="100" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" ValidationGroup="p" ErrorMessage="Name required." CssClass="err" Display="Dynamic" />
        <label>Email</label><asp:TextBox ID="txtEmail" runat="server" TextMode="Email" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" ValidationGroup="p" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="Invalid email." CssClass="err" Display="Dynamic" />
        <label>Phone</label><asp:TextBox ID="txtPhone" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPhone" ValidationGroup="p" ValidationExpression="^[0-9+\- ]{8,15}$" ErrorMessage="Invalid phone." CssClass="err" Display="Dynamic" />
        <label>Address</label><asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Rows="2" />
        <asp:Button ID="btnSave" runat="server" Text="Save Profile" ValidationGroup="p" OnClick="btnSave_Click" />
    </div>
    <div class="panel">
        <h2>Change Password</h2>
        <asp:Label ID="lblPw" runat="server" Visible="false" />
        <label>Current password</label><asp:TextBox ID="txtOld" runat="server" TextMode="Password" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOld" ValidationGroup="pw" ErrorMessage="Required." CssClass="err" Display="Dynamic" />
        <label>New password</label><asp:TextBox ID="txtNew" runat="server" TextMode="Password" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtNew" ValidationGroup="pw" ValidationExpression="^(?=.*[A-Za-z])(?=.*\d).{6,}$" ErrorMessage="6+ chars with letters and digits." CssClass="err" Display="Dynamic" />
        <label>Confirm new password</label><asp:TextBox ID="txtNew2" runat="server" TextMode="Password" />
        <asp:CompareValidator runat="server" ControlToValidate="txtNew2" ControlToCompare="txtNew" ValidationGroup="pw" ErrorMessage="Does not match." CssClass="err" Display="Dynamic" />
        <asp:Button ID="btnPw" runat="server" Text="Change Password" ValidationGroup="pw" OnClick="btnPw_Click" />
    </div>
</asp:Content>

<%@ Page Title="Feedback" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Feedback.aspx.cs" Inherits="FeedbackPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel">
        <h1>Comments &amp; Suggestions</h1>
        <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="alert ok" />
        <label>Name</label>
        <asp:TextBox ID="txtName" runat="server" MaxLength="100" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" ErrorMessage="Name is required." CssClass="err" Display="Dynamic" />
        <label>Email</label>
        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" MaxLength="100" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required." CssClass="err" Display="Dynamic" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="Invalid email." CssClass="err" Display="Dynamic" />
        <label>Message</label>
        <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="4" MaxLength="500" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMessage" ErrorMessage="Message is required." CssClass="err" Display="Dynamic" />
        <asp:Button ID="btnSend" runat="server" Text="Submit Feedback" OnClick="btnSend_Click" />
    </div>
</asp:Content>

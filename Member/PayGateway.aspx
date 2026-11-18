<%@ Page Title="Secure Payment" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PayGateway.aspx.cs" Inherits="Member_PayGateway" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblMsg" runat="server" Visible="false" />
    <asp:Panel ID="pnlGw" runat="server" CssClass="gw">
        <div class="gw-head"><asp:Literal ID="litProvider" runat="server" /></div>
        <div class="gw-body">
            <div>Paying to <b>GlowUp</b> &middot; Order #<asp:Literal ID="litOrder" runat="server" /></div>
            <div class="amt">Rs. <asp:Literal ID="litAmt" runat="server" /></div>

            <asp:Panel ID="pnlStep1" runat="server">
                <asp:Panel ID="pnlBank" runat="server" Visible="false">
                    <label>Select your bank</label>
                    <asp:DropDownList ID="ddlBank" runat="server">
                        <asp:ListItem>Nepal Bank Limited</asp:ListItem><asp:ListItem>Nabil Bank</asp:ListItem>
                        <asp:ListItem>Global IME Bank</asp:ListItem><asp:ListItem>NIC Asia Bank</asp:ListItem>
                        <asp:ListItem>Siddhartha Bank</asp:ListItem><asp:ListItem>Himalayan Bank</asp:ListItem>
                    </asp:DropDownList>
                </asp:Panel>
                <label>Mobile number</label>
                <asp:TextBox ID="txtMobile" runat="server" MaxLength="10" ReadOnly="true" />
                <label><asp:Literal ID="litPinLabel" runat="server" /></label>
                <asp:TextBox ID="txtPin" runat="server" TextMode="Password" MaxLength="12" />
                <asp:Button ID="btnStep1" runat="server" Text="Continue" OnClick="btnStep1_Click" />
            </asp:Panel>

            <asp:Panel ID="pnlStep2" runat="server" Visible="false">
                <p>A 6-digit verification code was sent to <b><asp:Literal ID="litMobile2" runat="server" /></b>.</p>
                <label>Verification code (OTP)</label>
                <asp:TextBox ID="txtOtp" runat="server" MaxLength="6" />
                <asp:Button ID="btnPay" runat="server" Text="Confirm payment" OnClick="btnPay_Click" />
            </asp:Panel>

            <asp:Panel ID="pnlOk" runat="server" Visible="false">
                <h2><svg class="ic" aria-hidden="true"><use href="#i-circle-check"/></svg> Payment successful</h2>
                <p>Transaction ID: <b><asp:Literal ID="litTxn" runat="server" /></b><br />
                   Your order #<asp:Literal ID="litOrder2" runat="server" /> is confirmed.</p>
                <a class="btn" href="Orders.aspx">View my orders</a>
            </asp:Panel>

            <p><asp:LinkButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="false">Cancel payment</asp:LinkButton></p>
            <div class="demo-note" id="demoNote" runat="server">Demo gateway &ndash; no real money is charged. Use PIN/password <b>1122</b> and OTP <b>123456</b> to succeed.</div>
        </div>
    </asp:Panel>
</asp:Content>

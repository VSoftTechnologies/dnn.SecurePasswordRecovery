<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="ICG.Modules.SecurePasswordRecovery.Settings" %>

<div class="dnnForm">
    <div class="dnnFormItem">
        <label><asp:Label ID="lblExpiryTime" runat="server" resourcekey="lblExpiryTime" /></label>
        <asp:TextBox ID="txtExpiryTime" runat="server" MaxLength="5" CssClass="dnnFormRequired" />
        <asp:RequiredFieldValidator ID="ExpiryTimeRequired" runat="server" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtExpiryTime" resourcekey="RequiredField" />
        <asp:CompareValidator ID="ExpiryTimeFormat" runat="server" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtExpiryTime" resourcekey="NumberField" Type="Integer" Operator="DataTypeCheck" />
    </div>
    <div class="dnnFormItem">
        <label><asp:Label ID="lblEnableCaptcha" runat="server" resourcekey="lblEnableCaptcha" /></label>
        <asp:CheckBox ID="chkEnableCaptcha" runat="server" Checked="true" CssClass="dnnFormRadioButtons" />
    </div>
</div>

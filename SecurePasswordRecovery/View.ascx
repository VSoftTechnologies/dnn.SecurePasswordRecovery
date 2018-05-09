<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="ICG.Modules.SecurePasswordRecovery.View" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="icg" Namespace="ICG.Modules.SecurePasswordRecovery" Assembly="ICG.Modules.SecurePasswordRecovery" %>


<asp:Panel ID="pnlRequestPasswordReset" runat="server">

    <p>
        <asp:Label ID="RequestPrompt" runat="server" resourcekey="RequestPrompt" />
    </p>
    <div class="dnnForm">
        <div class="dnnFormItem">
            <label><asp:Label ID="lblUsernameOrEmail" runat="server" resourcekey="lblUsernameOrEmail" /></label>
            <asp:TextBox ID="txtUsernameOrEmail" runat="server" CssClass="ICG_SPR_Username dnnFormRequired" />
            <asp:RequiredFieldValidator ID="UsernameorEmailRequired" runat="server" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" controltovalidate="txtUsernameOrEmail" resourcekey="RequiredField" />
        </div>
         <icg:ReCaptchaControl ID="ctrlReCaptcha" runat="server" ></icg:ReCaptchaControl>
        <br/>
        <p>
            <asp:LinkButton ID="btnRequestPasswordReset" runat="server" CssClass="dnnPrimaryAction"  resourcekey="btnRequestPasswordReset" onclick="btnRequestPasswordReset_Click" />
        </p>
    </div>
    <div class="dnnClear">&nbsp;</div>
    <div class="dnnClear">&nbsp;</div>
    <asp:Literal ID="litAlreadyReceived" runat="server" />
</asp:Panel>

<asp:Panel ID="pnlPerformReset" runat='server' Visible="false">
    <p>
        <asp:Label ID="PerformReset" runat="server" resourcekey="PerformReset" />
    </p>
    <div class="dnnForm">
        <div class="dnnFormItem">
            <label><asp:Label ID="lblResetCode" runat="server" resourcekey="lblResetCode" /></label>
            <asp:TextBox ID="txtResetCode" runat="server" CssClass="ICG_SPR_ResetCode dnnFormRequired" />
            <asp:RequiredFieldValidator ID="ResetCodeRequired" runat="server" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" controltovalidate="txtResetCode" resourcekey="RequiredField" />
        </div>
        <div class="dnnFormItem">
            <label><asp:Label ID="lblNewPassword" runat="server" resourcekey="lblNewPassword" /></label>
            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="ICG_SPR_Password dnnFormRequired" />
            <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtNewPassword" resourckey="RequiredField" />
        </div>
        <div class="dnnFormItem">
            <label><asp:Label ID="lblConfirmPassword" runat="server" resourcekey="lblConfirmPassword" /></label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="ICG_SPR_Password dnnFormRequired" />
            <asp:CompareValidator ID="ConfirmPasswordMatch" runat="server" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" resourcekey="ConfirmMatch" ControlToValidate="txtConfirmPassword" ControlToCompare="txtNewPassword" />
        </div>
        <ul class="dnnActions">
            <li><asp:LinkButton ID="btnResetPassword" runat="server" CssClass="dnnPrimaryAction" resourcekey="btnResetPassword" onclick="btnResetPassword_Click" /></li>
        </ul>
    </div>
</asp:Panel>


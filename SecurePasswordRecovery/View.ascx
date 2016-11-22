<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="ICG.Modules.SecurePasswordRecovery.View" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>

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
        <div class="dnnFormItem" id="divCaptchaRequest" runat="server">
            <dnn:DnnCaptcha ID="ctlCaptcha" CaptchaWidth="300" EnableRefreshImage="True" CaptchaTextBoxLabel="Please type the characters you see in the image into the text box above."
                    runat="server" ErrorMessage="The typed code must match the image, please try again"/>
        </div>
        <ul class="dnnActions">
            <li><asp:LinkButton ID="btnRequestPasswordReset" runat="server" CssClass="dnnPrimaryAction" resourcekey="btnRequestPasswordReset" onclick="btnRequestPasswordReset_Click" /></li>
        </ul>
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
        <div class="dnnFormItem" id="divCaptchaReset" runat="server">
            <dnn:DnnCaptcha ID="ctlCaptchaReset" CaptchaWidth="300" EnableRefreshImage="True" CaptchaTextBoxLabel="Please type the characters you see in the image into the text box above."
                    runat="server" ErrorMessage="The typed code must match the image, please try again"/>
        </div>
        <ul class="dnnActions">
            <li><asp:LinkButton ID="btnResetPassword" runat="server" CssClass="dnnPrimaryAction" resourcekey="btnResetPassword" onclick="btnResetPassword_Click" /></li>
        </ul>
    </div>
</asp:Panel>


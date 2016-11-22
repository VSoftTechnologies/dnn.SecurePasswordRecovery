using System;
using System.Text;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Mail;
using DotNetNuke.UI.Skins;
using DotNetNuke.UI.Skins.Controls;
using ICG.Modules.SecurePasswordRecovery.Components.Controllers;
using ICG.Modules.SecurePasswordRecovery.Components.InfoObjects;
using ICG.Modules.SecurePasswordRecovery.Components.Utility;

namespace ICG.Modules.SecurePasswordRecovery
{
    public partial class View : PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    var oConfig = new SettingsManager(Settings);
                    if (oConfig.IsConfigured)
                    {
                        if (!oConfig.CaptchaEnabled)
                        {
                            divCaptchaRequest.Visible = false;
                            divCaptchaReset.Visible = false;
                        }
                        if (Request.QueryString["mode"] != null)
                        {
                            //Toggle view and pre-load code if needed
                            pnlPerformReset.Visible = true;
                            pnlRequestPasswordReset.Visible = false;

                            if (Request.QueryString["code"] != null)
                                txtResetCode.Text = Request.QueryString["code"];
                        }
                        else
                        {
                            //Localize the single display item needed
                            litAlreadyReceived.Text =
                                Localization.GetString("AlreadyReceived", LocalResourceFile).Replace("[RESETURL]",
                                                                                                     UrlUtility.
                                                                                                         GenerateResetUrl
                                                                                                         (
                                                                                                             TabId));
                        }
                    }
                    else
                    {
                        pnlRequestPasswordReset.Visible = false;
                        Skin.AddModuleMessage(this, "Not Configured",
                                              ModuleMessage.ModuleMessageType.YellowWarning);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnResetPassword control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (ctlCaptchaReset.IsValid || divCaptchaReset.Visible == false)
            {
                //STW new GUID 
                var recoveryCode = txtResetCode.Text;
                if (recoveryCode.Length >= 1)
                {
                    var code = PasswordRecoveryController.SelectByGUID(recoveryCode);
                    if (code != null && code.ExpirationDate >= DateTime.Now)
                    {
                        //Reset the password
                        PasswordRecoveryController.SetNewPassword(code.UserId, code.PortalId, txtNewPassword.Text);

                        //expire the recoverycode
                        PasswordRecoveryController.ExpireRecoveryCode(txtResetCode.Text);

                        //Setup for login
                        var SuccessMessage = new StringBuilder(Localization.GetString("ResetSuccess", LocalResourceFile));
                        SuccessMessage.Replace("[LOGIN]", (Globals.NavigateURL(PortalSettings.LoginTabId)));
                        Skin.AddModuleMessage(this, SuccessMessage.ToString(),
                                              ModuleMessage.ModuleMessageType.GreenSuccess);
                        //Hide the form
                        pnlPerformReset.Visible = false;
                    }
                    else
                    {
                        Skin.AddModuleMessage(this, Localization.GetString("ExpiredCode", LocalResourceFile),
                                              ModuleMessage.ModuleMessageType.YellowWarning);
                    }
                }
                else
                    Skin.AddModuleMessage(this, Localization.GetString("InvalidCode", LocalResourceFile),
                                          ModuleMessage.ModuleMessageType.RedError);
            }
            else
            {
                Skin.AddModuleMessage(this, Localization.GetString("CaptchaError", LocalResourceFile),
                                      ModuleMessage.ModuleMessageType.RedError);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnRequestPasswordReset control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnRequestPasswordReset_Click(object sender, EventArgs e)
        {
            //perform captcha check
            if (ctlCaptcha.IsValid || divCaptchaReset.Visible == false)
            {
                //find user by name
                var byName = UserController.GetUserByName(PortalId, txtUsernameOrEmail.Text);
                if (byName != null)
                {
                    RecordAndSendRequest(byName.UserID, byName.Username, byName.Email);
                }
                else
                {
                    //Try by e-mail
                    var totalRecords = 0;
                    var byEmail = UserController.GetUsersByEmail(PortalId, txtUsernameOrEmail.Text, 0, 10,
                                                                 ref totalRecords);
                    if (byEmail.Count > 0)
                    {
                        foreach (UserInfo currentUser in byEmail)
                        {
                            RecordAndSendRequest(currentUser.UserID, currentUser.Username, currentUser.Email);
                        }
                    }
                }

                //Regardless show success
                Skin.AddModuleMessage(this, Localization.GetString("RequestSent", LocalResourceFile),
                                      ModuleMessage.ModuleMessageType.GreenSuccess);
                pnlRequestPasswordReset.Visible = false;
            }
            else
            {
                //Show Captcha Error
                Skin.AddModuleMessage(this, Localization.GetString("CaptchaError", LocalResourceFile),
                                      ModuleMessage.ModuleMessageType.YellowWarning);
            }
        }

        /// <summary>
        /// Records the and send request.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="username">The username.</param>
        /// <param name="email">The email.</param>
        private void RecordAndSendRequest(int userId, string username, string email)
        {
            double ExpiryTime = 2;
            if (Settings["ExpiryTime"] != null)
            {
                ExpiryTime = Convert.ToDouble(Settings["ExpiryTime"]);
            }

            var myRequest = new PasswordResetRequest
                                {
                                    PortalId = PortalId,
                                    UserId = userId,
                                    ExpirationDate = DateTime.Now.AddHours(ExpiryTime),
                                    RecoveryCode = Guid.NewGuid().ToString()
                                };
            myRequest = PasswordRecoveryController.InsertRequest(myRequest);
            var notificationEmail = new StringBuilder(Localization.GetString("ResetEmail", LocalResourceFile));
            notificationEmail.Replace("[PORTALNAME]", PortalSettings.PortalName);
            notificationEmail.Replace("[USERNAME]", username);
            var url = UrlUtility.GenerateResetUrl(TabId, myRequest.RecoveryCode);
            if(!url.ToLower().StartsWith("http"))
                url = "http://" + PortalSettings.PortalAlias.HTTPAlias + url;

            notificationEmail.Replace("[RESETLINK]", url);
            notificationEmail.Replace("[CODE]", myRequest.RecoveryCode);
            try
            {
                Mail.SendMail(PortalSettings.Email, email, string.Empty,
                              Localization.GetString("ResetEmailSubject", LocalResourceFile),
                              notificationEmail.ToString(), string.Empty, "HTML", string.Empty, string.Empty,
                              string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
            }
        }
    }
}
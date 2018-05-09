using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Mail;
using DotNetNuke.UI.Skins;
using ICG.Modules.SecurePasswordRecovery.Components.Controllers;
using ICG.Modules.SecurePasswordRecovery.Components.InfoObjects;
using ICG.Modules.SecurePasswordRecovery.Components.Utility;
using System;
using System.Text;
using static DotNetNuke.UI.Skins.Controls.ModuleMessage;

namespace ICG.Modules.SecurePasswordRecovery
{
    public partial class View : PortalModuleBase
    {
        public virtual void ShowMessage(string msg, ModuleMessageType messageType)
        {
            clearMessagePlaceHolder();
            Skin.AddModuleMessage(this, msg, messageType);

        }

        private void clearMessagePlaceHolder()
        {
            var ctl = this.FindControl("dnnSkinMessage");
            if (ctl != null)
            {
                this.Controls.Remove(ctl);
            }
            Skin.AddModuleMessage(this, "", ModuleMessageType.BlueInfo);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                clearMessagePlaceHolder();

                var oConfig = new SettingsManager(Settings);
                if (oConfig.CaptchaEnabled)
                {
                    ctrlReCaptcha.Visible = true;
                    ctrlReCaptcha.SiteKey = oConfig.ReCaptchaSiteKey;
                    ctrlReCaptcha.SecretKey = oConfig.ReCaptchaSecretKey;
                }
                else
                {
                    ctrlReCaptcha.Visible = false;
                }



                if (!IsPostBack)
                {


                    if (oConfig.IsConfigured)
                    {
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
                        ShowMessage("Not Configured", ModuleMessageType.YellowWarning);
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
                    ShowMessage(SuccessMessage.ToString(), ModuleMessageType.GreenSuccess);
                    //Hide the form
                    pnlPerformReset.Visible = false;
                }
                else
                {
                    ShowMessage(Localization.GetString("ExpiredCode", LocalResourceFile), ModuleMessageType.YellowWarning);
                }
            }
            else
                ShowMessage(Localization.GetString("InvalidCode", LocalResourceFile), ModuleMessageType.RedError);
        }

        /// <summary>
        /// Handles the Click event of the btnRequestPasswordReset control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnRequestPasswordReset_Click(object sender, EventArgs e)
        {
            var oConfig = new SettingsManager(Settings);

            if (oConfig.CaptchaEnabled && !ctrlReCaptcha.Validate())
            {
                ShowMessage("Captcha not valid - please click on the captcha", ModuleMessageType.YellowWarning);
                ctrlReCaptcha.Visible = true;
                return;
            }

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
            ShowMessage(Localization.GetString("RequestSent", LocalResourceFile), ModuleMessageType.GreenSuccess);
            pnlRequestPasswordReset.Visible = false;
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
            if (!url.ToLower().StartsWith("http"))
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
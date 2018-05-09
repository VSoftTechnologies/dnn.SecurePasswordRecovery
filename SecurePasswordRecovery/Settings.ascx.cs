using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using System;

namespace ICG.Modules.SecurePasswordRecovery
{
    public partial class Settings : ModuleSettingsBase
    {

        /// <summary>
        /// handles the loading of the module setting for this
        /// control
        /// </summary>
        public override void LoadSettings()
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Settings["ExpiryTime"] != null)
                    {
                        txtExpiryTime.Text = Settings["ExpiryTime"].ToString();
                    }
                    else
                    {
                        txtExpiryTime.Text = "2";
                    }

                    if (Settings["EnableReCaptcha"] != null)
                        chkEnableReCaptcha.Checked = Convert.ToBoolean(Settings["EnableReCaptcha"].ToString());
                    if (Settings["ReCaptchaSiteKey"] != null)
                        txtReCaptchaSiteKey.Text = Settings["ReCaptchaSiteKey"].ToString();
                    if (Settings["ReCaptchaSecretKey"] != null)
                        txtReCaptchaSecretKey.Text = Settings["ReCaptchaSecretKey"].ToString();

                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        /// <summary>
        /// handles updating the module settings for this control
        /// </summary>
        public override void UpdateSettings()
        {
            try
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "ExpiryTime", txtExpiryTime.Text);
                modules.UpdateTabModuleSetting(this.TabModuleId, "EnableReCaptcha", chkEnableReCaptcha.Checked.ToString());
                modules.UpdateTabModuleSetting(this.TabModuleId, "ReCaptchaSiteKey", txtReCaptchaSiteKey.Text);
                modules.UpdateTabModuleSetting(this.TabModuleId, "ReCaptchaSecretKey", txtReCaptchaSecretKey.Text);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
    }
}
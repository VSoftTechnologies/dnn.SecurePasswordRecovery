using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;

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

                    if (Settings["EnableCaptcha"] != null)
                        chkEnableCaptcha.Checked = Convert.ToBoolean(Settings["EnableCaptcha"].ToString());
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
                modules.UpdateTabModuleSetting(this.TabModuleId, "ExpiryTime", txtExpiryTime.Text );
                modules.UpdateTabModuleSetting(this.TabModuleId, "EnableCaptcha", chkEnableCaptcha.Checked.ToString());
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
    }
}
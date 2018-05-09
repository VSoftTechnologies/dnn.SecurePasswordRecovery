using System;
using System.Collections;

namespace ICG.Modules.SecurePasswordRecovery.Components.Utility
{
    /// <summary>
    /// Manager class for viewing settings information
    /// </summary>
    public class SettingsManager
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the expire time.
        /// </summary>
        /// <value>
        /// The expire time.
        /// </value>
        public double ExpireTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [captcha enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [captcha enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool CaptchaEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is configured.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is configured; otherwise, <c>false</c>.
        /// </value>
        public bool IsConfigured { get; set; }

        public string ReCaptchaSiteKey { get; set; }

        public string ReCaptchaSecretKey { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsManager"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public SettingsManager(Hashtable settings)
        {
            if (settings["ExpiryTime"] != null)
            {
                ExpireTime = Convert.ToDouble(settings["ExpiryTime"]);
                IsConfigured = true;
            }

            if (settings["EnableCaptcha"] != null)
                CaptchaEnabled = Convert.ToBoolean(settings["EnableCaptcha"].ToString());
            else
                CaptchaEnabled = true;

            if (settings["ReCaptchaSiteKey"] != null)
                ReCaptchaSiteKey = settings["ReCaptchaSiteKey"].ToString();

            if (settings["ReCaptchaSecretKey"] != null)
                ReCaptchaSecretKey = settings["ReCaptchaSecretKey"].ToString();
        }

        #endregion
    }
}
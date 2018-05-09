using System;
using System.ComponentModel;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ICG.Modules.SecurePasswordRecovery
{
    public class GoogleVerificationResponseOutput
    {
        public bool success { get; set; }
    }


    [ToolboxData("<{0}:ReCaptchaControl runat=server></{0}:ReCaptchaControl>")]
    public class ReCaptchaControl : WebControl
    {
        readonly string ValidateResponseField = "g-recaptcha-response";
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string SiteKey
        {
            get
            {
                String s = (String)ViewState["_SiteKey"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["_SiteKey"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string SecretKey
        {
            get
            {
                String s = (String)ViewState["_SecretKey"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["_SecretKey"] = value;
            }
        }


        protected override void RenderContents(HtmlTextWriter output)
        {
            if (!string.IsNullOrEmpty(SiteKey) && !string.IsNullOrEmpty(SecretKey))
            {
                string reCaptchaHTML = string.Format("<div class='g-recaptcha' data-sitekey='{0}'></div>", SiteKey);

                string scriptToRenderCaptcha = @" <script src=""https://www.google.com/recaptcha/api.js"" async defer></script>";
                output.Write(reCaptchaHTML);
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "reCaptchaScript", scriptToRenderCaptcha, false);
            }
            else
            {
                //Public/Private Keys not provided
                string noKeyFound = "<div class='noKeyFound'>Public/Private keys not provided for Captcha control. You can get your keys from <a href='https://www.google.com/recaptcha' target='_blank'>Google Recaptcha</a></div>";
                output.Write(noKeyFound);
            }
        }

        public bool Validate()
        {
            string EncodedResponse = this.Page.Request.Form[ValidateResponseField];

            if (string.IsNullOrEmpty(EncodedResponse) || string.IsNullOrEmpty(SecretKey))
                return false;
            //by pass certificate validation
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var client = new System.Net.WebClient();

            var GoogleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", SecretKey, EncodedResponse));

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            GoogleVerificationResponseOutput gOutput = serializer.Deserialize<GoogleVerificationResponseOutput>(GoogleReply);

            return gOutput.success;
        }
    }
}
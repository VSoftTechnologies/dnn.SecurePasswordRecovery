using System;
using System.Collections.Generic;
using System.Web;

namespace ICG.Modules.SecurePasswordRecovery.Components.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public static class UrlUtility
    {
        public static string GenerateResetUrl(int tabId)
        {
            return DotNetNuke.Common.Globals.NavigateURL(tabId, string.Empty, "mode=reset");
        }

        public static string GenerateResetUrl(int tabId, string recoverycode)
        {
            /// STW changed to use GUID in url
            ///return DotNetNuke.Common.Globals.NavigateURL(tabId, string.Empty, "mode=reset", "code=" + code);
            return DotNetNuke.Common.Globals.NavigateURL(tabId, string.Empty, "mode=reset", "code=" + recoverycode);
        
        }

    }
}
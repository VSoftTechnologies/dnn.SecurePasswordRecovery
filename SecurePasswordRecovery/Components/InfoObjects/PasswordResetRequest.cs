using System;
using System.Collections.Generic;
using System.Web;

namespace ICG.Modules.SecurePasswordRecovery.Components.InfoObjects
{
    /// <summary>
    /// Represents a users request for a password
    /// </summary>
    public class PasswordResetRequest
    {
        /// <summary>
        /// Gets or sets the request id.
        /// </summary>
        /// <value>The request id.</value>
        public int RequestId { get; set; }
        /// <summary>
        /// Gets or sets the portal id.
        /// </summary>
        /// <value>The portal id.</value>
        public int PortalId { get; set; }
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>The expiration date.</value>
        public DateTime ExpirationDate { get; set; }

        /// public string Code { get { return string.Concat(RequestId.ToString(), "-", PortalId.ToString(), "-", UserId.ToString()); } }
        
        /// STW - added to implement GUID
        public string RecoveryCode { get; set; }


    }
}
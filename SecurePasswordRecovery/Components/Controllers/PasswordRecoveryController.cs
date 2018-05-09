using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;
using ICG.Modules.SecurePasswordRecovery.Components.InfoObjects;

namespace ICG.Modules.SecurePasswordRecovery.Components.Controllers
{
    /// <summary>
    /// Controller class for working with user password requests
    /// </summary>
    public class PasswordRecoveryController
    {
        /// <summary>
        /// Sets the new password for the 
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="portalId">The portal id.</param>
        /// <param name="newPassword">The new password.</param>
        public static void SetNewPassword(int userId, int portalId, string newPassword)
        {
            var toChange = UserController.GetUserById(portalId, userId);
            var password = MembershipProvider.Instance().ResetPassword(toChange, string.Empty);
            UserController.ChangePassword(toChange, password, newPassword);
        }

        /// <summary>
        /// Expires the recovery code.
        /// </summary>
        /// <param name="RecoveryCode">The recovery code.</param>
        public static void ExpireRecoveryCode(string RecoveryCode)
        {
            DataProvider.Instance().ExecuteNonQuery("ICG_SPR_RequestUpdateExpirationByGUID", RecoveryCode);
        }

        /// <summary>
        /// Inserts the request.
        /// </summary>
        /// <param name="oRequest">The o request.</param>
        /// <returns></returns>
        public static PasswordResetRequest InsertRequest(PasswordResetRequest oRequest)
        {
            var newId = int.Parse(DataProvider.Instance().ExecuteScalar<int>("ICG_SPR_RequestInsert", oRequest.PortalId, oRequest.UserId, oRequest.ExpirationDate, oRequest.RecoveryCode).ToString());
            oRequest.RequestId = newId;
            return oRequest;
        }

        /// <summary>
        /// Selects the Recovery Request by id.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <returns></returns>
        public static PasswordResetRequest SelectById(int requestId)
        {
            return CBO.FillObject<PasswordResetRequest>(DataProvider.Instance().ExecuteReader("ICG_SPR_RequestSelectById", requestId));
        }

        /// <summary>
        /// Selects Recovery Request the by GUID.
        /// </summary>
        /// <param name="RecoveryCode">The recovery code.</param>
        /// <returns></returns>
        public static PasswordResetRequest SelectByGUID(string RecoveryCode)
        {
            return CBO.FillObject<PasswordResetRequest>(DataProvider.Instance().ExecuteReader("ICG_SPR_RequestSelectByGUID", RecoveryCode));
        }
    }
}
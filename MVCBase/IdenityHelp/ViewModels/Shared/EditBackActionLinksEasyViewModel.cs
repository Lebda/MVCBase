using System;
using System.Linq;
using System.Security.Principal;
using IdenityHelp.Infrastructure;

namespace IdenityHelp.ViewModels.Shared
{
    public class EditBackActionLinksEasyViewModel : EditBackActionLinkEasyBase
    {
        public EditBackActionLinksEasyViewModel(
            IPrincipal user,
            string controllerName,
            string controllerName4Back,
            object modelBind,
            object modelBind4Back)
            : base (user, controllerName, controllerName4Back, modelBind, modelBind4Back)
        {
        }

        #region INTERFACE
        override public bool CanUserSeeEdit()
        {
            return RoleNames.AllowedRoleNames(RoleNames.EditOperationRoleNames, User);
        }
        #endregion
    }
}

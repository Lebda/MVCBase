using System;
using System.Linq;
using System.Security.Principal;
using IdenityHelp.Infrastructure;

namespace IdenityHelp.ViewModels.Shared
{
    public class CreateActionLinkEasyViewModel : CreateActionLinkBase
    {
        public CreateActionLinkEasyViewModel(
            string linkText,
            IPrincipal user,
            string controllerName,
            object modelBind)
            : base(linkText, user, controllerName, modelBind)
        {
        }

        #region INTERFACE
        override public bool CanUserSeeCreate()
        {
            return RoleNames.AllowedRoleNames(RoleNames.CreateOperationRoleNames, User);
        }
        #endregion
    }
}

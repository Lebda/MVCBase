using System;
using System.Linq;
using System.Security.Principal;
using IdenityHelp.Infrastructure;
using IdenityHelp.ViewModels.Shared;

namespace IdenityHelp.ViewModels.Roles
{
    public class CreateRoleActionLinkViewModel : CreateActionLinkBase
    {
        public CreateRoleActionLinkViewModel(
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
            return RoleNames.AllowedRoleNames(RoleNames.CreateRoleRoleNames, User);
        }
        #endregion
    }
}
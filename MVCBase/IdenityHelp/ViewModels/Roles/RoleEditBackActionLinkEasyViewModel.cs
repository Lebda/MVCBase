using System;
using System.Linq;
using System.Security.Principal;
using IdenityHelp.Infrastructure;
using IdenityHelp.ViewModels.Shared;

namespace IdenityHelp.ViewModels.Roles
{
    public class RoleEditBackActionLinkEasyViewModel : EditBackActionLinkEasyBase
    {
        public RoleEditBackActionLinkEasyViewModel(
            string roleName,
            IPrincipal user,
            string controllerName,
            string controllerName4Back,
            object modelBind,
            object modelBind4Back)
            : base(user, controllerName, controllerName4Back, modelBind, modelBind4Back)
        {
            m_roleName = roleName;
        }
        
        #region MEMBER
        readonly string m_roleName;
        #endregion
        
        #region INTERFACE
        override public bool CanUserSeeEdit()
        {
            return RoleNames.IsEditableRole(m_roleName) || User.IsInRole(RoleNames.c_architectRoleName);
        }
        #endregion
    }
}
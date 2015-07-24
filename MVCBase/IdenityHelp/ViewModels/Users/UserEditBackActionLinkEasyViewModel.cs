using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using IdenityHelp.Infrastructure;
using IdenityHelp.ViewModels.Shared;

namespace IdenityHelp.ViewModels.Users
{
    public class UserEditBackActionLinkEasyViewModel : EditBackActionLinkEasyBase
    {
        public UserEditBackActionLinkEasyViewModel(
            IEnumerable<string> role4User,
            IPrincipal user,
            string controllerName,
            string controllerName4Back,
            object modelBind,
            object modelBind4Back)
            : base(user, controllerName, controllerName4Back, modelBind, modelBind4Back)
        {
            m_role4User = role4User;
        }

        #region MEMBERS
        readonly IEnumerable<string> m_role4User;
        #endregion
        
        #region INTERFACE
        override public bool CanUserSeeEdit()
        {
            if (User.IsInRole(RoleNames.c_architectRoleName))
            {
                return true;
            }
            if (m_role4User.Contains(RoleNames.c_architectRoleName))
            {
                return false;
            }
            return RoleNames.AllowedRoleNames(RoleNames.EditUserRoleNames, User);
        }
        #endregion
    }
}
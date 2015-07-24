using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using IdenityHelp.Infrastructure;
using IdenityHelp.ViewModels.Shared;

namespace IdenityHelp.ViewModels.Users
{
    public class UsersCrudActionLinksEasyViewModel : CrudActionLinksEasyViewModel
    {
        public UsersCrudActionLinksEasyViewModel(
            IEnumerable<string> role4User,
            IPrincipal user,
            string controllerName,
            object modelBind)
            : base(user, controllerName, modelBind)
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
        override public bool CanUserSeeDelete()
        {
            if (User.IsInRole(RoleNames.c_architectRoleName))
            {
                return true;
            }
            if (m_role4User.Contains(RoleNames.c_architectRoleName) || m_role4User.Contains(RoleNames.c_adminRoleName))
            {
                return false;
            }
            return RoleNames.AllowedRoleNames(RoleNames.DeleteUserRoleNames, User);
        }
        override public bool CanUserSeeDetail()
        {
            if (User.IsInRole(RoleNames.c_architectRoleName))
            {
                return true;
            }
            return RoleNames.AllowedRoleNames(RoleNames.DetailUserRoleNames, User);
        }
        #endregion
    }
}

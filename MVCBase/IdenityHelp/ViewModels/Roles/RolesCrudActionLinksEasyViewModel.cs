using System;
using System.Linq;
using System.Security.Principal;
using IdenityHelp.Infrastructure;
using IdenityHelp.ViewModels.Shared;

namespace IdenityHelp.ViewModels.Roles
{
    public class RolesCrudActionLinksEasyViewModel : CrudActionLinksEasyViewModel
    {
        public RolesCrudActionLinksEasyViewModel(
            string roleName,
            IPrincipal user,
            string controllerName,
            object modelBind)
            : base(user, controllerName, modelBind)
        {
            m_roleName = roleName;
        }

        #region MEMBER
        readonly string m_roleName;
        #endregion

        #region INTERFACE
        override public bool CanUserSeeEdit()
        {
            return RoleNames.AllowedRoleNames(RoleNames.EditRoleRoleNames, User) || RoleNames.IsEditableRole(m_roleName);
        }
        override public bool CanUserSeeDelete()
        {
            return RoleNames.AllowedRoleNames(RoleNames.DeleteRoleRoleNames, User) || RoleNames.IsEditableRole(m_roleName);
        }
        override public bool CanUserSeeDetail()
        {
            return RoleNames.AllowedRoleNames(RoleNames.DetailRoleRoleNames, User);
        }
        #endregion
    }
}

using System;
using System.Linq;
using System.Security.Principal;
using IdenityHelp.Infrastructure;

namespace IdenityHelp.ViewModels.Shared
{
    public class CrudActionLinksEasyViewModel : CrudActionLinksBase
    {
        public CrudActionLinksEasyViewModel(
            IPrincipal user,
            string controllerName,
            object modelBind)
            : base(user, controllerName, modelBind)
        {
        }
        
        #region INTERFACE
        override public bool CanUserSeeEdit()
        {
            return RoleNames.AllowedRoleNames(RoleNames.EditOperationRoleNames, User);
        }
        override public bool CanUserSeeDelete()
        {
            return RoleNames.AllowedRoleNames(RoleNames.DeleteOperationRoleNames, User);
        }
        #endregion
    }
}
using System;
using System.Linq;

namespace IdenityHelp.Infrastructure.Atrributes
{
    public class AuthorizeRoles4EditAttribute : AuthorizeRolesAttribute
    {
        public AuthorizeRoles4EditAttribute()
            : base(RoleNames.EditOperationRoleNames)
        {           
        }
    }
}

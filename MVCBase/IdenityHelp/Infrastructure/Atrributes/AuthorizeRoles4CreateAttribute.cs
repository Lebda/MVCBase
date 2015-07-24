using System;
using System.Linq;

namespace IdenityHelp.Infrastructure.Atrributes
{
    public class AuthorizeRoles4CreateAttribute : AuthorizeRolesAttribute
    {
        public AuthorizeRoles4CreateAttribute()
            : base(RoleNames.CreateOperationRoleNames)
        {
        }
    }
}
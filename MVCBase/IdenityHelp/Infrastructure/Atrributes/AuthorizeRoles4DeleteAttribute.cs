using System;
using System.Linq;

namespace IdenityHelp.Infrastructure.Atrributes
{
    public class AuthorizeRoles4DeleteAttribute : AuthorizeRolesAttribute
    {
        public AuthorizeRoles4DeleteAttribute()
            : base(RoleNames.DeleteOperationRoleNames)
        {           
        }
    }
}

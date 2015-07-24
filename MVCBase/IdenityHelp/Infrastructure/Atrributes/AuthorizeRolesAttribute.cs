using System;
using System.Linq;
using System.Web.Mvc;

namespace IdenityHelp.Infrastructure.Atrributes
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles)
            : base()
        {
            Roles = string.Join(",", roles);
        }
    }
}

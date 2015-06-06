using System;
using System.Linq;
using System.Web.Mvc;

namespace MVCHelp.Concrete
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

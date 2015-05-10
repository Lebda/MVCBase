using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace MVCHelp.Concrete
{
    public class ContentControllerBase : Controller
    {
        public FileResult GetDeveloperLogoBase()
        {
            var path = Path.Combine("~/Content/Images", DeveloperLogo);
            return File(path, "image/png");
        }

        protected string DeveloperLogo { get; set; }
    }
}

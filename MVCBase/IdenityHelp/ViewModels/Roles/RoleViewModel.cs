using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IdenityHelp.ViewModels.Roles
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

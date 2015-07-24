using System;
using System.Linq;

namespace IdenityHelp.ViewModels.Roles
{
    public class InvalidRoleModificationViewModel
    {
        public InvalidRoleModificationViewModel()
        {
            Message = String.Empty;
        }

        #region PROPERTIES
        public string Message { get; set; }
        #endregion
    }
}

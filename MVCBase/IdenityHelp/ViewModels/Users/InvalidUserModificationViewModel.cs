using System;
using System.Linq;

namespace IdenityHelp.ViewModels.Users
{
    public class InvalidUserModificationViewModel
    {
        public InvalidUserModificationViewModel()
        {
            Message = String.Empty;
        }

        #region PROPERTIES
        public string Message { get; set; }
        #endregion
    }
}

using System;
using System.Linq;
using System.Security.Principal;

namespace IdenityHelp.ViewModels.Shared
{
    abstract public class CreateActionLinkBase : ICreateActionLinkBase
    {
        public CreateActionLinkBase(
            string linkText,
            IPrincipal user,
            string controllerName,
            object modelBind)
        {
            User = user;
            LinkText = linkText;
            ControllerName = controllerName;
            ModelBind = modelBind;
        }

        #region INTERFACE
        public abstract bool CanUserSeeCreate();
        #endregion

        #region PROPERTIES
        protected IPrincipal User { get; private set; }
        public string LinkText { get; private set; }
        public string ControllerName { get; private set; }
        public object ModelBind { get; private set; }
        #endregion
    }
}

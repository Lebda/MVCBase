using System;
using System.Linq;
using System.Security.Principal;

namespace IdenityHelp.ViewModels.Shared
{
    abstract public class EditBackActionLinkEasyBase : IEditBackActionLinkEasyBase
    {
        public EditBackActionLinkEasyBase(
            IPrincipal user,
            string controllerName,
            string controllerName4Back,
            object modelBind,
            object modelBind4Back)
        {
            User = user;
            ControllerName = controllerName;
            ControllerName4Back = controllerName4Back;
            ModelBind = modelBind;
            ModelBind4Back = modelBind4Back;
        }

        #region INTERFACE
        public abstract bool CanUserSeeEdit();
        public virtual bool ShowEditBackDivider()
        {
            return CanUserSeeEdit();
        }
        #endregion

        #region PROPERTIES
        protected IPrincipal User { get; private set; }
        public string ControllerName { get; private set; }
        public string ControllerName4Back { get; private set; }
        public object ModelBind { get; private set; }
        public object ModelBind4Back { get; private set; }
        #endregion
    }
}

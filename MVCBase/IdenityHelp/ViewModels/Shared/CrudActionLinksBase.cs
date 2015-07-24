using System;
using System.Linq;
using System.Security.Principal;

namespace IdenityHelp.ViewModels.Shared
{
    public abstract class CrudActionLinksBase
    {
        public CrudActionLinksBase(
            IPrincipal user,
            string controllerName,
            object modelBind)
        {
            User = user;
            ControllerName = controllerName;
            ModelBind = modelBind;
        }

        #region INTERFACE
        abstract public bool CanUserSeeEdit();
        abstract public bool CanUserSeeDelete();
        virtual public bool CanUserSeeDetail()
        {
            return true;
        }
        virtual public bool ShowEditDetailsDivider()
        {
            return CanUserSeeEdit();
        }
        virtual public bool ShowDetailsDeleteDivider()
        {
            return CanUserSeeDelete();
        }
        #endregion

        #region PROPERTIES
        protected IPrincipal User { get; private set; }
        public string ControllerName { get; private set; }
        public object ModelBind { get; private set; }
        #endregion
    }
}

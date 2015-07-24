using System;
using System.Linq;

namespace IdenityHelp.ViewModels.Shared
{
    public interface ICreateActionLinkBase
    {
        bool CanUserSeeCreate();
        string LinkText { get; }
        string ControllerName { get; }
        object ModelBind { get; }
    }
}

namespace IdenityHelp.ViewModels.Shared
{
    public interface IEditBackActionLinkEasyBase
    {
        bool CanUserSeeEdit();
        bool ShowEditBackDivider();
        string ControllerName { get; }
        string ControllerName4Back { get; }
        object ModelBind { get; }
        object ModelBind4Back { get; }
    }
}
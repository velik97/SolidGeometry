using Util.EventBusSystem;

namespace Runtime.Access.ApplicationModeManagement
{
    public interface IApplicationModeHandler : IGlobalSubscriber
    {
        void HandleApplicationModeChanged(ApplicationMode mode);
    }
}
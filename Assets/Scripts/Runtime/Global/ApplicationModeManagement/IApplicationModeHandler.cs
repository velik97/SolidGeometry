using Util.EventBusSystem;

namespace Runtime.Global.ApplicationModeManagement
{
    public interface IApplicationModeHandler : IGlobalSubscriber
    {
        void HandleApplicationModeChanged(ApplicationMode mode);
    }
}
using Util.EventBusSystem;

namespace Runtime.Core
{
    public interface IApplicationModeHandler : IGlobalSubscriber
    {
        void HandleChangeApplicationMode(ApplicationMode mode);
    }
}
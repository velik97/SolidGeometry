using Util.EventBusSystem;

namespace Runtime.Global.UI
{
    public interface IUIModeHandler : IGlobalSubscriber
    {
        void HandleUIModeChanged(UIMode mode);
    }
}
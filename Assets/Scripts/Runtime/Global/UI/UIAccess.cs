using UniRx;
using Util.EventBusSystem;

namespace Runtime.Global.UI
{
    public class UIAccess : CompositeDisposable
    {
        private UIMode m_CurrentUIMode;
        public UIMode CurrentUIMode => m_CurrentUIMode;

        public void ChangeMode(UIMode mode)
        {
            if (mode == m_CurrentUIMode)
            {
                return;
            }

            m_CurrentUIMode = mode;
            EventBus.RaiseEvent<IUIModeHandler>(h => h.HandleUIModeChanged(m_CurrentUIMode));
        }
    }

    public enum UIMode
    {
        Usual = 0,
        Popup = 1,
        RepositioningAR = 2
    }
}
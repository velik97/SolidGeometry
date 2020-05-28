using System;

namespace Util.CascadeUpdate
{
    public static class EditorUpdateInvokerBridge
    {
        public static event Action Update;

        public static void OnUpdate()
        {
            Update?.Invoke();
        }
    }
}
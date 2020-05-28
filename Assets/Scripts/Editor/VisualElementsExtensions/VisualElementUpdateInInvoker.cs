using System;
using UnityEngine.UIElements;
using Util.CascadeUpdate;

namespace Editor.VisualElementsExtensions
{
    public class VisualElementUpdateInInvoker : VisualElement
    {
        public VisualElementUpdateInInvoker()
        {
            IVisualElementScheduledItem updateScheduler = schedule.Execute(EditorUpdateInvokerBridge.OnUpdate);
            updateScheduler.Every(25);
        }
    }
}
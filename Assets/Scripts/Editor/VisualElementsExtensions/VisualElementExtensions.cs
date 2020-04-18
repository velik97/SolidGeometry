using UnityEngine.UIElements;

namespace Editor.VisualElementsExtensions
{
    public static class VisualElementExtensions
    {
        public static bool SwapElementsAt(this VisualElement visualElement, int a, int b)
        {
            if (a == b)
            {
                return false;
            }
            if (a < 0 || a >= visualElement.childCount)
            {
                return false;
            }
            if (b < 0 || b >= visualElement.childCount)
            {
                return false;
            }

            int lowerIndex = a;
            int higherIndex = b;

            if (a > b)
            {
                lowerIndex = b;
                higherIndex = a;
            }

            VisualElement higherElement = visualElement.ElementAt(higherIndex);
            visualElement.RemoveAt(higherIndex);
            visualElement.Insert(lowerIndex, higherElement);

            return true;
        }
    }
}
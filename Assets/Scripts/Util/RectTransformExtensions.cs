using UnityEngine;

namespace Util
{
    public static class RectTransformExtensions
    {
        public static Rect GetWorldSpaceRect(this RectTransform rectTransform)
        {
            Rect rect = rectTransform.rect;
            rect.center = rectTransform.TransformPoint(rect.center);
            rect.size = rectTransform.TransformVector(rect.size);
            return rect;
        }
    }
}
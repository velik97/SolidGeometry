using UnityEngine;

namespace Shapes.View
{
    public class LineView : MonoBehaviour, IShapeView
    {
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public void SetHighlight(HighlightType highlightType)
        {
            return;
        }
    }
}
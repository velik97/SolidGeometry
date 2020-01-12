using UnityEngine;

namespace Shapes.View
{
    public class PolygonView : MonoBehaviour, IShapeView
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
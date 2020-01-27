using Shapes.Data;
using UnityEngine;

namespace Shapes.View
{
    public class PolygonView : MonoBehaviourShapeView<PolygonData>
    {
        public override void SetHighlight(HighlightType highlightType)
        { }

        public override void UpdateName(PolygonData shapeData)
        { }

        public override void UpdateGeometry(PolygonData shapeData)
        { }
    }
}
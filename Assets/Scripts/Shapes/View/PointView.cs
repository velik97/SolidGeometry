using Shapes.Data;
using TMPro;
using UnityEngine;

namespace Shapes.View
{
    public class PointView : MonoBehaviourShapeView<PointData>
    {
        [SerializeField] private GameObject m_Sphere;
        [SerializeField] private TextMeshProUGUI m_NameLabel;

        public override void SetHighlight(HighlightType highlightType)
        { }

        public override void UpdateName(PointData shapeData)
        {
            m_NameLabel.text = shapeData.PointName;
        }

        public override void UpdateGeometry(PointData shapeData)
        {
            transform.position = shapeData.Position;
        }
    }
}
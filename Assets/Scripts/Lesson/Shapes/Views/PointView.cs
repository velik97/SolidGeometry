using Shapes.Data;
using TMPro;
using UnityEngine;

namespace Lesson.Shapes.Views
{
    public class PointView : MonoBehaviourShapeView<PointData>
    {
        [SerializeField] private GameObject m_Sphere;
        [SerializeField] private TextMeshProUGUI m_NameLabel;

        public override HighlightType Highlight { get; set; }

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
using Lesson.Shapes.Datas;
using TMPro;
using UnityEngine;

namespace Lesson.Shapes.Views
{
    public class PointView : MonoBehaviourShapeView<PointData>
    {
        [SerializeField] private GameObject m_Sphere;
        [SerializeField] private TextMeshProUGUI m_NameLabel;

        public override HighlightType Highlight { get; set; }

        public override void UpdateName()
        {
            m_NameLabel.text = ShapeData.PointName;
        }

        public override void UpdateGeometry()
        {
            transform.position = ShapeData.Position;
        }
    }
}
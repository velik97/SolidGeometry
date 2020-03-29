using Lesson.Shapes.Datas;
using TMPro;
using UnityEngine;

namespace Lesson.Shapes.Views
{
    public class PointView : MonoBehaviourShapeView<PointData>
    {
        [SerializeField] private GameObject m_Sphere;
        [SerializeField] private TextMeshProUGUI m_NameLabel;

        [SerializeField] private Material[] m_Materials = new Material[5];
        [SerializeField] private Renderer m_Renderer;
        private HighlightType m_Highlight;
        
        public override HighlightType Highlight 
        {
            get
            {
                return m_Highlight;
            }
            set
            {
                m_Highlight = value;
                UpdateHighlight();
            }
        }

        private void UpdateHighlight()
        {
            m_Renderer.material = m_Materials[(int)m_Highlight];
        }

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

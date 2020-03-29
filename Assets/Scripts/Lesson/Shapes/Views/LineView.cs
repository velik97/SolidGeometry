using Lesson.Shapes.Datas;
using UnityEngine;

namespace Lesson.Shapes.Views
{
    public class LineView : MonoBehaviourShapeView<LineData>
    {
        [SerializeField] private GameObject m_CylinderParent;
        [SerializeField] private GameObject m_Cylinder;

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
        { }

        public override void UpdateGeometry()
        {
            if (ShapeData.StartPoint == null || ShapeData.EndPoint == null)
            {
                return;
            }
            
            Vector3 start = ShapeData.StartPoint.Position;
            Vector3 end = ShapeData.EndPoint.Position;

            Vector3 middle = (start + end) / 2f;
            Vector3 direction = start - end;
            float length = direction.magnitude;

            transform.position = middle;

            Vector3 scale = m_Cylinder.transform.localScale;
            m_Cylinder.transform.localScale = new Vector3(scale.x, length / 2f, scale.z);
            
            if (length == 0f)
            {
                return;
            }
            
            m_CylinderParent.transform.localRotation = Quaternion.LookRotation(direction);
        }
    }
}
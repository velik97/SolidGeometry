using Shapes.Data;
using UnityEngine;

namespace Shapes.View
{
    public class LineView : MonoBehaviourShapeView<LineData>
    {
        [SerializeField] private GameObject m_CylinderParent;
        [SerializeField] private GameObject m_Cylinder;

        public override HighlightType Highlight { get; set; }

        public override void UpdateName(LineData shapeData)
        { }

        public override void UpdateGeometry(LineData shapeData)
        {
            if (shapeData.StartPoint == null || shapeData.EndPoint == null)
            {
                return;
            }
            
            Vector3 start = shapeData.StartPoint.Position;
            Vector3 end = shapeData.EndPoint.Position;

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
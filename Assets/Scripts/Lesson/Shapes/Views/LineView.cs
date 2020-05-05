using System;
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
        
        private static readonly int LineDirectionShaderProperty = Shader.PropertyToID("_LineDirection");

        public override HighlightType Highlight 
        {
            get => m_Highlight;
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

        private void Update()
        {
            if (m_Renderer.material.HasProperty("_LineDirection"))
            {
                float scale = transform.lossyScale.x;
                m_Renderer.material.SetVector(LineDirectionShaderProperty, m_Cylinder.transform.up / scale);
            }
        }

        protected override void UpdateGeometry()
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
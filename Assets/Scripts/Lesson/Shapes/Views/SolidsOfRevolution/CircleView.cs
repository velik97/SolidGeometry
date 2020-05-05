using System;
using Lesson.Shapes.Datas.SolidsOfRevolution;
using MeshGeneration;
using UnityEngine;

namespace Lesson.Shapes.Views.SolidsOfRevolution
{
    public class CircleView : MonoBehaviourShapeView<CircleData>, IHaveMeshSideVisibility
    {
        [SerializeField]
        private Transform m_CircleTransform;
        
        [SerializeField]
        [Range(3, 64)]
        private int m_SidesCount = 64;
        
        [Header("Edge")]
        [SerializeField]
        private GameObject m_EdgeObject;
        
        private MeshFilter m_EdgeMeshFilter;
        private Mesh m_EdgeMesh;
        
        [SerializeField]
        private Material[] m_EdgeMaterials = new Material[5];
        private MeshRenderer m_EdgeMeshRenderer;
        
        private ProceduralTorusGenerator m_EdgeTorusGenerator;
        [SerializeField]
        private float m_Radius2 = .1f;
        [SerializeField]
        [Range(3, 16)]
        private int m_EdgeTorusSegmentsCount = 6;

        [Header("Inside")]
        [SerializeField]
        private GameObject m_InsideObject;
        
        private MeshFilter m_InsideMeshFilter;
        private Mesh m_InsideMesh;
        
        [SerializeField]
        private Material[] m_InsideMaterials = new Material[5];
        private MeshRenderer m_InsideMeshRenderer;
        
        private ProceduralCircleGenerator m_InsideCircleGenerator;

        private HighlightType m_Highlight;
        private CircleMode m_CircleDrawMode;

        public MeshSideVisibilityType SideVisibility
        {
            get => m_InsideCircleGenerator.SideVisibility;
            set => m_InsideCircleGenerator.SideVisibility = value;
        }

        private bool m_IsInitialized = false;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (m_IsInitialized)
            {
                return;
            }
            // Edge
            m_EdgeMesh = new Mesh();
            m_EdgeMeshFilter = m_EdgeObject.GetComponent<MeshFilter>();
            m_EdgeMeshFilter.mesh = m_EdgeMesh;
            m_EdgeMeshRenderer = m_EdgeObject.GetComponent<MeshRenderer>();

            m_EdgeTorusGenerator = new ProceduralTorusGenerator
            {
                Radius2 = m_Radius2,
                SidesCount = m_SidesCount,
                SegmentsCount = m_EdgeTorusSegmentsCount
            };

            // Inside
            m_InsideMesh = new Mesh();
            m_InsideMeshFilter = m_InsideObject.GetComponent<MeshFilter>();
            m_InsideMeshFilter.mesh = m_InsideMesh;
            m_InsideMeshRenderer = m_InsideObject.GetComponent<MeshRenderer>();

            m_InsideCircleGenerator = new ProceduralCircleGenerator
            {
                SidesCount = m_SidesCount
            };

            m_IsInitialized = true;
        }
        
        public override HighlightType Highlight 
        {
            get => m_Highlight;
            set
            {
                m_Highlight = value;
                UpdateHighlight();
            }
        }

        public CircleMode CircleDrawMode
        {
            get => m_CircleDrawMode;
            set
            {
                m_CircleDrawMode = value;
                UpdateDrawMode();
            }
        }

        private void UpdateHighlight()
        {
            m_InsideMeshRenderer.material = m_InsideMaterials[(int) m_Highlight];
            m_EdgeMeshRenderer.material = m_EdgeMaterials[(int) m_Highlight];
        }

        private void UpdateDrawMode()
        {
            m_EdgeObject.gameObject.SetActive((m_CircleDrawMode & CircleMode.Edge) != 0);
            m_InsideObject.gameObject.SetActive((m_CircleDrawMode & CircleMode.Inside) != 0);
        }

        protected override void UpdateGeometry()
        {
            m_CircleTransform.localPosition = ShapeData.CenterPosition;
            m_CircleTransform.forward = ShapeData.Normal;

            m_EdgeTorusGenerator.Radius1 = ShapeData.Radius;
            m_EdgeTorusGenerator.UpdateMesh(m_EdgeMeshFilter.mesh);

            m_InsideCircleGenerator.Radius = ShapeData.Radius;
            m_InsideCircleGenerator.UpdateMesh(m_InsideMeshFilter.mesh);
        }
        
        [Flags]
        public enum CircleMode : byte
        {
            Everything = Edge | Inside,
            Inside = 1 << 0,
            Edge = 1 << 1,
            Nothing = 0,
        }
    }
}
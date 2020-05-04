using Lesson.Shapes.Datas.ShapesOfRevolution;
using MeshGeneration;
using UnityEngine;

namespace Lesson.Shapes.Views.SolidsOfRevolution
{
    public class SphereView : MonoBehaviourShapeView<SphereData>, IHaveMeshSideVisibility
    {
        [SerializeField]
        private Transform m_SphereTransform;
        
        private MeshFilter m_MeshFilter;
        private Mesh m_Mesh;
        
        [SerializeField]
        private Material[] m_Materials = new Material[5];
        private MeshRenderer m_MeshRenderer;
        
        private ProceduralSphereGenerator m_SphereGenerator;
        [SerializeField] private MeshSideVisibilityType m_SideVisibilityType = MeshSideVisibilityType.BothSides;
        
        [SerializeField]
        [Range(1, 4)]
        private int m_LevelOfDetail = 64;
        
        private HighlightType m_Highlight;
        
        public override HighlightType Highlight 
        {
            get => m_Highlight;
            set
            {
                m_Highlight = value;
                UpdateHighlight();
            }
        }
        
        public MeshSideVisibilityType SideVisibility
        {
            get => m_SphereGenerator.SideVisibility;
            set => m_SphereGenerator.SideVisibility = value;
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

            m_Mesh = new Mesh();
            m_MeshFilter = m_SphereTransform.GetComponent<MeshFilter>();
            m_MeshFilter.mesh = m_Mesh;
            m_MeshRenderer = m_SphereTransform.GetComponent<MeshRenderer>();

            m_SphereGenerator = new ProceduralSphereGenerator()
            {
                LevelOfDetail = m_LevelOfDetail
            };

            m_IsInitialized = true;
        }

        private void UpdateHighlight()
        {
            m_MeshRenderer.material = m_Materials[(int)m_Highlight];
        }

        protected override void UpdateGeometry()
        {
            m_SphereTransform.localPosition = ShapeData.CenterPosition;

            m_SphereGenerator.Radius = ShapeData.Radius;
            m_SphereGenerator.UpdateMesh(m_MeshFilter.mesh);
        }
    }
}
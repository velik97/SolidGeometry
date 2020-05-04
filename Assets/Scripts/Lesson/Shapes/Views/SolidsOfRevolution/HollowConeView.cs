using Lesson.Shapes.Datas.ShapesOfRevolution;
using MeshGeneration;
using UnityEngine;

namespace Lesson.Shapes.Views.SolidsOfRevolution
{
    public class HollowConeView : MonoBehaviourShapeView<HollowConeData>, IHaveMeshSideVisibility
    {
        [SerializeField]
        private Transform m_ConeTransform;
        
        private MeshFilter m_MeshFilter;
        private Mesh m_Mesh;
        
        [SerializeField]
        private Material[] m_Materials = new Material[5];
        private MeshRenderer m_MeshRenderer;
        
        private ProceduralHollowConeGenerator m_HollowConeGenerator;
        [SerializeField]
        private MeshSideVisibilityType m_SideVisibilityType = MeshSideVisibilityType.BothSides;
        
        [SerializeField]
        [Range(3, 64)]
        private int m_SidesCount = 64;
        
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
            get => m_HollowConeGenerator.SideVisibility;
            set => m_HollowConeGenerator.SideVisibility = value;
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
            m_MeshFilter = m_ConeTransform.GetComponent<MeshFilter>();
            m_MeshFilter.mesh = m_Mesh;
            m_MeshRenderer = m_ConeTransform.GetComponent<MeshRenderer>();

            m_HollowConeGenerator = new ProceduralHollowConeGenerator()
            {
                SidesCount = m_SidesCount
            };

            m_IsInitialized = true;
        }

        private void UpdateHighlight()
        {
            m_MeshRenderer.material = m_Materials[(int)m_Highlight];
        }

        protected override void UpdateGeometry()
        {
            m_ConeTransform.localPosition = ShapeData.OriginPosition;

            m_HollowConeGenerator.Height = ShapeData.Height;
            m_HollowConeGenerator.Radius = ShapeData.Radius;
            m_HollowConeGenerator.UpdateMesh(m_MeshFilter.mesh);
        }
    }
}
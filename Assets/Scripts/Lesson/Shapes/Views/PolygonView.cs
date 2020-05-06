using System.Linq;
using Lesson.Shapes.Datas;
using UnityEngine;

namespace Lesson.Shapes.Views
{
    public class PolygonView : MonoBehaviourShapeView<PolygonData>
    {
        [SerializeField]
        private MeshFilter m_MeshFilter;
        private Mesh m_PolygonMesh;

        [SerializeField] private Material[] m_Materials = new Material[5];
        [SerializeField] private Renderer m_Renderer;
        
        private HighlightType m_Highlight;
        
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
            m_PolygonMesh = new Mesh();
            m_MeshFilter.mesh = m_PolygonMesh;
            m_IsInitialized = true;
        }

        protected override void UpdateGeometry()
        {
            Initialize();

            if (ShapeData.Points == null || ShapeData.Points.Any(p => p == null))
            {
                return;
            }

            Vector3[] vertices = ShapeData.Points.Select(p => p.Position).ToArray();
            int[] triangles = new int[(vertices.Length - 2) * 6];

            int tr = 0;
            for (int v = 1; v < vertices.Length - 1; v++)
            {
                triangles[tr] = 0;
                tr++;
                triangles[tr] = v;
                tr++;
                triangles[tr] = v + 1;
                tr++;
                
                triangles[tr] = 0;
                tr++;
                triangles[tr] = v + 1;
                tr++;
                triangles[tr] = v;
                tr++;
            }
            
            Vector3 up = Vector3.Cross(vertices[1] - vertices[0], vertices[2] - vertices[0]);
            Vector3[] normals = new Vector3[vertices.Length];
            for (var i = 0; i < normals.Length; i++)
            {
                normals[i] = up;
            }

            m_PolygonMesh.Clear();
            m_PolygonMesh.vertices = vertices;
            m_PolygonMesh.triangles = triangles;
            m_PolygonMesh.normals = normals;
        }
        
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
    }
}
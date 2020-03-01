using System;
using System.Linq;
using Shapes.Data;
using UnityEngine;

namespace Shapes.View
{
    public class PolygonView : MonoBehaviourShapeView<PolygonData>
    {
        [SerializeField]
        private MeshFilter m_MeshFilter;

        private Mesh m_PolygonMesh;

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
            if (m_PolygonMesh == null)
            {
                m_PolygonMesh = new Mesh();
            }
            m_MeshFilter.mesh = m_PolygonMesh;
            m_IsInitialized = true;
        }

        public override HighlightType Highlight { get; set; }

        public override void UpdateName(PolygonData shapeData)
        { }

        public override void UpdateGeometry(PolygonData shapeData)
        {
            Initialize();

            if (shapeData.Points == null || shapeData.Points.Any(p => p == null))
            {
                return;
            }

            Vector3[] vertices = shapeData.Points.Select(p => p.Position).ToArray();
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
            
            m_PolygonMesh.Clear();
            m_PolygonMesh.vertices = vertices;
            m_PolygonMesh.triangles = triangles;
        }
    }
}
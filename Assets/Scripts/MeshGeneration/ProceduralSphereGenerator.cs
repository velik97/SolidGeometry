using System.Collections.Generic;
using UnityEngine;

namespace MeshGeneration
{
    public class ProceduralSphereGenerator : IProceduralMeshGenerator, IHaveMeshSideVisibility
    {
        private float m_Radius = 1;
        
        private int m_LevelOfDetail = 1;

        private MeshSideVisibilityType m_SideVisibility;

        public float Radius
        {
            get => m_Radius;
            set
            {
                if (value < float.Epsilon)
                {
                    return;
                }
                m_Radius = value;
            }
        }
        
        public int LevelOfDetail
        {
            get => m_LevelOfDetail;
            set
            {
                if (value < 1 || value > 4)
                {
                    return;
                }
                m_LevelOfDetail = value;
            }
        }

        public MeshSideVisibilityType SideVisibility
        {
            get => m_SideVisibility;
            set => m_SideVisibility = value;
        }

        public void UpdateMesh(Mesh mesh)
        {
            mesh.Clear();
            mesh.name = "ico sphere";
            if (m_SideVisibility == MeshSideVisibilityType.None)
            {
                return;
            }
     
            List<Vector3> vertList = new List<Vector3>();
            Dictionary<long, int> middlePointIndexCache = new Dictionary<long, int>();
            
            // create 12 vertices of a icosahedron
            float t = (1f + Mathf.Sqrt(5f)) / 2f;
     
            vertList.Add(new Vector3(-1f,  t,  0f).normalized * m_Radius);
            vertList.Add(new Vector3( 1f,  t,  0f).normalized * m_Radius);
            vertList.Add(new Vector3(-1f, -t,  0f).normalized * m_Radius);
            vertList.Add(new Vector3( 1f, -t,  0f).normalized * m_Radius);
     
            vertList.Add(new Vector3( 0f, -1f,  t).normalized * m_Radius);
            vertList.Add(new Vector3( 0f,  1f,  t).normalized * m_Radius);
            vertList.Add(new Vector3( 0f, -1f, -t).normalized * m_Radius);
            vertList.Add(new Vector3( 0f,  1f, -t).normalized * m_Radius);
     
            vertList.Add(new Vector3( t,  0f, -1f).normalized * m_Radius);
            vertList.Add(new Vector3( t,  0f,  1f).normalized * m_Radius);
            vertList.Add(new Vector3(-t,  0f, -1f).normalized * m_Radius);
            vertList.Add(new Vector3(-t,  0f,  1f).normalized * m_Radius);

            // create 20 triangles of the icosahedron
            List<TriangleIndices> faces = new List<TriangleIndices>();
     
            // 5 faces around point 0
            faces.Add(new TriangleIndices(0, 11, 5));
            faces.Add(new TriangleIndices(0, 5, 1));
            faces.Add(new TriangleIndices(0, 1, 7));
            faces.Add(new TriangleIndices(0, 7, 10));
            faces.Add(new TriangleIndices(0, 10, 11));
     
            // 5 adjacent faces 
            faces.Add(new TriangleIndices(1, 5, 9));
            faces.Add(new TriangleIndices(5, 11, 4));
            faces.Add(new TriangleIndices(11, 10, 2));
            faces.Add(new TriangleIndices(10, 7, 6));
            faces.Add(new TriangleIndices(7, 1, 8));
     
            // 5 faces around point 3
            faces.Add(new TriangleIndices(3, 9, 4));
            faces.Add(new TriangleIndices(3, 4, 2));
            faces.Add(new TriangleIndices(3, 2, 6));
            faces.Add(new TriangleIndices(3, 6, 8));
            faces.Add(new TriangleIndices(3, 8, 9));
     
            // 5 adjacent faces 
            faces.Add(new TriangleIndices(4, 9, 5));
            faces.Add(new TriangleIndices(2, 4, 11));
            faces.Add(new TriangleIndices(6, 2, 10));
            faces.Add(new TriangleIndices(8, 6, 7));
            faces.Add(new TriangleIndices(9, 8, 1));
     
     
            // refine triangles
            for (int i = 0; i < m_LevelOfDetail; i++)
            {
                List<TriangleIndices> faces2 = new List<TriangleIndices>();
                foreach (var tri in faces)
                {
                    // replace triangle by 4 triangles
                    int a = GetMiddlePoint(tri.v1, tri.v2, vertList, middlePointIndexCache, m_Radius);
                    int b = GetMiddlePoint(tri.v2, tri.v3, vertList, middlePointIndexCache, m_Radius);
                    int c = GetMiddlePoint(tri.v3, tri.v1, vertList, middlePointIndexCache, m_Radius);
     
                    faces2.Add(new TriangleIndices(tri.v1, a, c));
                    faces2.Add(new TriangleIndices(tri.v2, b, a));
                    faces2.Add(new TriangleIndices(tri.v3, c, b));
                    faces2.Add(new TriangleIndices(a, b, c));
                }
                faces = faces2;
            }
            
            List< int > triangles = new List<int>();
            if ((m_SideVisibility & MeshSideVisibilityType.Inverted) != 0)
            {
                for (int i = 0; i < faces.Count; i++)
                {
                    triangles.Add(faces[i].v3);
                    triangles.Add(faces[i].v2);
                    triangles.Add(faces[i].v1);
                }
            }
            if ((m_SideVisibility & MeshSideVisibilityType.Normal) != 0)
            {
                for (int i = 0; i < faces.Count; i++)
                {
                    triangles.Add(faces[i].v1);
                    triangles.Add(faces[i].v2);
                    triangles.Add(faces[i].v3);
                }
            }
     
            Vector3[] normals = new Vector3[ vertList.Count];
            for( int i = 0; i < normals.Length; i++ )
            {
                normals[i] = vertList[i].normalized;
            }

            mesh.vertices = vertList.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.normals = normals;
            mesh.Optimize();
        }
        
        // return index of point in the middle of p1 and p2
        private int GetMiddlePoint(int p1, int p2, List<Vector3> vertices, Dictionary<long, int> cache, float radius)
        {
            // first check if we have it already
            bool firstIsSmaller = p1 < p2;
            long smallerIndex = firstIsSmaller ? p1 : p2;
            long greaterIndex = firstIsSmaller ? p2 : p1;
            long key = (smallerIndex << 32) + greaterIndex;

            if (cache.TryGetValue(key, out int ret))
            {
                return ret;
            }
     
            // not in cache, calculate it
            Vector3 point1 = vertices[p1];
            Vector3 point2 = vertices[p2];
            Vector3 middle = new Vector3
            (
                (point1.x + point2.x) / 2f, 
                (point1.y + point2.y) / 2f, 
                (point1.z + point2.z) / 2f
            );
     
            // add vertex makes sure point is on unit sphere
            int i = vertices.Count;
            vertices.Add( middle.normalized * radius ); 
     
            // store it, return index
            cache.Add(key, i);
     
            return i;
        }
        
        private struct TriangleIndices
        {
            public int v1;
            public int v2;
            public int v3;

            public TriangleIndices(int v1, int v2, int v3)
            {
                this.v1 = v1;
                this.v2 = v2;
                this.v3 = v3;
            }
        }
    }
}
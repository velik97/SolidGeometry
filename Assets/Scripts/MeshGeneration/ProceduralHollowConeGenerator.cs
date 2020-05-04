using System.Collections.Generic;
using UnityEngine;
using Util;

namespace MeshGeneration
{
    public class ProceduralHollowConeGenerator : IProceduralMeshGenerator, IHaveMeshSideVisibility
    {
	    private float m_Radius;
	    private float m_Height;

	    private int m_SidesCount;

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
	    
	    public float Height
	    {
		    get => m_Height;
		    set
		    {
			    if (value < float.Epsilon)
			    {
				    return;
			    }
			    m_Height = value;
		    }
	    }

	    public int SidesCount
	    {
		    get => m_SidesCount;
		    set
		    {
			    if (value < 3 || value > 64)
			    {
				    return;
			    }
			    m_SidesCount = value;
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
			mesh.name = "circle";
			if (m_SideVisibility == MeshSideVisibilityType.None)
			{
				return;
			}

			int verticesCount = m_SidesCount + 1;
			int middleVertex = m_SidesCount;
			
			#region Vertices
			
			Vector3[] vertices = new Vector3[verticesCount];
			float _2pi = Mathf.PI * 2f;
			 
			for (int v = 0; v < m_SidesCount; v += 1)
			{
				float r1 = (float)v / m_SidesCount * _2pi;
				float cos = Mathf.Cos(r1);
				float sin = Mathf.Sin(r1);

				vertices[v] = new Vector3(cos * m_Radius, 0, sin * m_Radius);
			}

			vertices[middleVertex] = Vector3.up * m_Height;
			
			#endregion
			 
			#region Normales
			
			Vector3[] normals = new Vector3[vertices.Length];
			Vector3 middleVertexPosition = vertices[middleVertex];
			for (int v = 0; v < m_SidesCount; v += 1)
			{
				normals[v] = Vector3.zero.ProjectionOnLine(vertices[v], middleVertexPosition).normalized;
			}
			normals[middleVertex] = Vector3.up;

			#endregion
	        
			#region Triangles

			List<int> triangles = new List<int>(m_SidesCount * 3);
			
			if ((m_SideVisibility & MeshSideVisibilityType.Inverted) != 0)
			{
				for (int v = 0; v < m_SidesCount; v++)
				{
					triangles.Add(v);
					triangles.Add((v + 1) % m_SidesCount);
					triangles.Add(middleVertex);
				}
			}
			if ((m_SideVisibility & MeshSideVisibilityType.Normal) != 0)
			{
				for (int v = 0; v < m_SidesCount; v++)
				{
					triangles.Add(v);
					triangles.Add(middleVertex);
					triangles.Add((v + 1) % m_SidesCount);
				}
			}
			
			#endregion
			 
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.triangles = triangles.ToArray();
			 
			mesh.Optimize();
        }
    }
}
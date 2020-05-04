using System.Collections.Generic;
using UnityEngine;
using Util;

namespace MeshGeneration
{
    public class ProceduralHollowCylinderGenerator : IProceduralMeshGenerator, IHaveMeshSideVisibility
    {
	    private float m_TopRadius;
	    private float m_BottomRadius;
	    private float m_Height;
	    
	    private int m_SidesCount;
	    
	    private MeshSideVisibilityType m_SideVisibility;

	    public float TopRadius
	    {
		    get => m_TopRadius;
		    set
		    {
			    if (value < float.Epsilon)
			    {
				    return;
			    }
			    m_TopRadius = value;
		    }
	    }

	    public float BottomRadius
	    {
		    get => m_BottomRadius;
		    set
		    {
			    if (value < float.Epsilon)
			    {
				    return;
			    }
			    m_BottomRadius = value;
		    }
	    }
	    
	    public float CommonRadius
	    {
		    get => (m_BottomRadius + m_TopRadius) * .5f;
		    set
		    {
			    if (value < float.Epsilon)
			    {
				    return;
			    }
			    m_BottomRadius = value;
			    m_TopRadius = value;
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
			mesh.name = "cylinder";
			if (m_SideVisibility == MeshSideVisibilityType.None)
			{
				return;
			}

			int verticesCount = m_SidesCount * 2;
			
			#region Vertices
			
			Vector3[] vertices = new Vector3[verticesCount];
			float _2pi = Mathf.PI * 2f;
			 
			int sideCounter = 0;
			for (int v = 0; v < verticesCount; v += 2)
			{
				sideCounter = (sideCounter + 1) % m_SidesCount;
			 
				float r1 = (float)sideCounter / m_SidesCount * _2pi;
				float cos = Mathf.Cos(r1);
				float sin = Mathf.Sin(r1);

				vertices[v] = new Vector3(cos * m_TopRadius, m_Height, sin * m_TopRadius);
				vertices[v + 1] = new Vector3(cos * m_BottomRadius, 0, sin * m_BottomRadius);
			}
			
			#endregion
			 
			#region Normales
			
			Vector3[] normals = new Vector3[vertices.Length];
			 
			for (int v = 0; v < verticesCount; v += 2)
			{
				normals[v] = Vector3.zero.ProjectionOnLine(vertices[v], vertices[v + 1]).normalized;
				normals[v + 1] = normals[v];
			}
			
			#endregion
	        
			#region Triangles
			
			var triangles = new List<int>(m_SidesCount * 6);
			
			if ((m_SideVisibility & MeshSideVisibilityType.Inverted) != 0)
			{
				for (int v = 0; v < verticesCount; v += 2)
				{
					int bottomLeft = v;
					int topLeft = v + 1;
					int bottomRight = (v + 2) % verticesCount;
					int topRight = (v + 3) % verticesCount;

					triangles.Add(bottomLeft);
					triangles.Add(topLeft);
					triangles.Add(bottomRight);

					triangles.Add(bottomRight);
					triangles.Add(topLeft);
					triangles.Add(topRight);
				}
			}
			if ((m_SideVisibility & MeshSideVisibilityType.Normal) != 0)
			{
				for (int v = 0; v < verticesCount; v += 2)
				{
					int bottomLeft = v;
					int topLeft = v + 1;
					int bottomRight = (v + 2) % verticesCount;
					int topRight = (v + 3) % verticesCount;

					triangles.Add(bottomLeft);
					triangles.Add(bottomRight);
					triangles.Add(topLeft);

					triangles.Add(bottomRight);
					triangles.Add(topRight);
					triangles.Add(topLeft);
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
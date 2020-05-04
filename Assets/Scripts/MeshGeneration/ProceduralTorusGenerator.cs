using UnityEngine;

namespace MeshGeneration
{
    public class ProceduralTorusGenerator : IProceduralMeshGenerator
    {
	    private float m_Radius1 = 1f;
	    private float m_Radius2 = .1f;
	    
	    private int m_SidesCount = 32;
	    private int m_SegmentsCount = 16;
	    
	    public float Radius1
	    {
		    get => m_Radius1;
		    set
		    {
			    if (value < float.Epsilon || value < m_Radius2)
			    {
				    return;
			    }
			    m_Radius1 = value;
		    }
	    }

	    public float Radius2
	    {
		    get => m_Radius2;
		    set
		    {
			    if (value < float.Epsilon || value > m_Radius1)
			    {
				    return;
			    }
			    m_Radius2 = value;
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
	    
	    public int SegmentsCount
	    {
		    get => m_SegmentsCount;
		    set
		    {
			    if (value < 3 || value > 16)
			    {
				    return;
			    }
			    m_SegmentsCount = value;
		    }
	    }

	    public void UpdateMesh(Mesh mesh)
	    {
		    mesh.Clear();
			mesh.name = "torus";
			 
			#region Vertices
			
			Vector3[] vertices = new Vector3[(m_SidesCount + 1) * (m_SegmentsCount + 1)];
			float _2pi = Mathf.PI * 2f;

			for (int side = 0; side <= m_SidesCount; side++)
			{
				int currSeg = side == m_SidesCount ? 0 : side;

				float t1 = (float) currSeg / m_SidesCount * _2pi;
				Vector3 r1 = new Vector3(Mathf.Cos(t1) * m_Radius1, 0f, Mathf.Sin(t1) * m_Radius1);

				for (int seg = 0; seg <= m_SegmentsCount; seg++)
				{
					int currSide = seg == m_SegmentsCount ? 0 : seg;

					float t2 = (float) currSide / m_SegmentsCount * _2pi;
					Vector3 r2 = Quaternion.AngleAxis(-t1 * Mathf.Rad2Deg, Vector3.up) *
					             new Vector3(Mathf.Sin(t2) * m_Radius2, Mathf.Cos(t2) * m_Radius2);

					vertices[seg + side * (m_SegmentsCount + 1)] = r1 + r2;
				}
			}

			#endregion
			 
			#region Normales	
			
			Vector3[] normals = new Vector3[vertices.Length];
			for (int side = 0; side <= m_SidesCount; side++)
			{
				int currSeg = side == m_SidesCount ? 0 : side;

				float t1 = (float) currSeg / m_SidesCount * _2pi;
				Vector3 r1 = new Vector3(Mathf.Cos(t1) * m_Radius1, 0f, Mathf.Sin(t1) * m_Radius1);

				for (int seg = 0; seg <= m_SegmentsCount; seg++)
				{
					normals[seg + side * (m_SegmentsCount + 1)] = (vertices[seg + side * (m_SegmentsCount + 1)] - r1).normalized;
				}
			}

			#endregion
			 
			#region Triangles
			
			int[] triangles = new int[ vertices.Length * 6 ];
			 
			int i = 0;
			for (int side = 0; side <= m_SidesCount; side++)
			{
				for (int seg = 0; seg <= m_SegmentsCount - 1; seg++)
				{
					int current = seg + side * (m_SegmentsCount + 1);
					int next = seg + (side < (m_SidesCount) ? (side + 1) * (m_SegmentsCount + 1) : 0);

					if (i < triangles.Length - 6)
					{
						triangles[i++] = current;
						triangles[i++] = next;
						triangles[i++] = next + 1;

						triangles[i++] = current;
						triangles[i++] = next + 1;
						triangles[i++] = current + 1;
					}
				}
			}

			#endregion
			 
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.triangles = triangles;
			
			mesh.Optimize();
        }
    }
}
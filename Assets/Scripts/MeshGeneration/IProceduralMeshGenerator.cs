using UnityEngine;

namespace MeshGeneration
{
    public interface IProceduralMeshGenerator
    {
        void UpdateMesh(Mesh mesh);
    }
}
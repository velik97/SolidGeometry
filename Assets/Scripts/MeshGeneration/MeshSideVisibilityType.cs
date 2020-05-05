using System;

namespace MeshGeneration
{
    [Flags]
    public enum MeshSideVisibilityType : byte
    {
        BothSides = Normal | Inverted,
        Normal = 1 << 0,
        Inverted = 1 << 1,
        None = 0,
    }
}
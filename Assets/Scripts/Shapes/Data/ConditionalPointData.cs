using System;
using UnityEngine;

namespace Shapes.Data
{
    [Serializable]
    public class ConditionalPointData : PointData
    {
        public readonly Func<Vector3> PositionFunc;
    }
}
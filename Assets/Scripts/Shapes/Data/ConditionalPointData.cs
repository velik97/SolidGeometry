using System;
using UnityEngine;

namespace Shapes.Data
{
    public class ConditionalPointData : PointData
    {
        private readonly Func<Vector3> m_PositionFunc;

        public override Vector3 Position => m_PositionFunc();
    }
}
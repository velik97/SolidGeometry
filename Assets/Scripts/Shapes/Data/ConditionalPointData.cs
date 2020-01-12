using System;
using UnityEngine;

namespace Shapes.Data
{
    public class ConditionalPointData : PointData
    {
        private readonly Func<Vector3> m_PositionFunc;

        public override Vector3 Position => m_PositionFunc();

        public ConditionalPointData(Func<Vector3> positionFunc, string pointName, bool isAccessoryPoint)
            : base(pointName, isAccessoryPoint)
        {
            m_PositionFunc = positionFunc;
        }
    }
}
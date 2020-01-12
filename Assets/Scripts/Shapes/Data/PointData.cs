using Shapes.View;
using UnityEngine;

namespace Shapes.Data
{
    public class PointData : ShapeData
    {
        public PointView PointView => View as PointView;
        
        public virtual Vector3 Position => m_Position;
        public readonly string PointName;
        public readonly bool IsAccessoryPoint;

        private readonly Vector3 m_Position;

        public PointData(Vector3 position, string pointName, bool isAccessoryPoint)
            : this(pointName, isAccessoryPoint)
        {
            m_Position = position;
        }
        
        protected PointData(string pointName, bool isAccessoryPoint)
        {
            PointName = pointName;
            IsAccessoryPoint = isAccessoryPoint;
        }

        public override string ToString()
        {
            return $"Point: {PointName}";
        }
    }
}
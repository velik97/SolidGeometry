using System;
using Shapes.View;
using UniRx;
using UnityEngine;

namespace Shapes.Data
{
    [Serializable]
    public class PointData : ShapeData
    {
        public PointView PointView => View as PointView;
        
        public virtual Vector3 Position => m_Position;
        public string PointName => m_PointName;
        public bool IsAccessoryPoint => m_IsAccessoryPoint;

        protected Vector3 m_Position;
        private string m_PointName;
        private bool m_IsAccessoryPoint;

        public void SetName(string pointName)
        {
            m_PointName = pointName;
            OnNameUpdated();
        }

        public virtual void SetPosition(Vector3 position)
        {
            m_Position = position;
            OnGeometryUpdated();
        }

        public void SetIsAccessory(bool isAccessory)
        {
            m_IsAccessoryPoint = isAccessory;
        }

        public override string ToString()
        {
            return $"Point {PointName}";
        }
    }
}
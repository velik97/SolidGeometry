using System;
using Shapes.Data.Uniqueness;
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

        public PointNameUniquenessValidatable NameUniquenessValidatable;
        public PointPositionUniquenessValidatable PositionUniquenessValidatable;

        protected Vector3 m_Position = Vector3.zero;
        private string m_PointName = string.Empty;
        private bool m_IsAccessoryPoint = false;

        public PointData()
        {
            NameUniquenessValidatable = new PointNameUniquenessValidatable(this);
            PositionUniquenessValidatable = new PointPositionUniquenessValidatable(this);
        }

        public void SetName(string pointName)
        {
            if (pointName == m_PointName)
            {
                return;
            }
            m_PointName = pointName;
            OnNameUpdated();
        }

        public virtual void SetPosition(Vector3 position)
        {
            if (position == m_Position)
            {
                return;
            }
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
        
        public class PointNameUniquenessValidatable : IUniquenessValidatable<PointNameUniquenessValidatable>
        {
            public event Action UniqueDeterminingPropertyUpdated;
            public event Action UniquenessUpdated;

            private PointData m_PointData;
            
            private bool m_IsUnique;

            public bool IsUnique => m_IsUnique;

            public string PointName => m_PointData.PointName;

            public PointNameUniquenessValidatable(PointData pointData)
            {
                m_PointData = pointData;
                m_PointData.NameUpdated += OnUniqueDeterminingPropertyUpdated;
            }
            
            private void OnUniqueDeterminingPropertyUpdated()
            {
                UniqueDeterminingPropertyUpdated?.Invoke();
            }

            public void SetIsUnique(bool unique)
            {
                if (unique == m_IsUnique)
                {
                    return;
                }

                m_IsUnique = unique;
                UniquenessUpdated?.Invoke();
            }

            public int GetUniqueHashCode()
            {
                return PointName.GetHashCode();
            }

            public bool UniqueEquals(PointNameUniquenessValidatable validatable)
            {
                return PointName == validatable.PointName;
            }
        }
        
        public class PointPositionUniquenessValidatable : IUniquenessValidatable<PointPositionUniquenessValidatable>
        {
            public event Action UniqueDeterminingPropertyUpdated;
            public event Action UniquenessUpdated;

            private PointData m_PointData;
            
            private bool m_IsUnique;

            public bool IsUnique => m_IsUnique;

            public Vector3 Position => m_PointData.Position;

            public PointPositionUniquenessValidatable(PointData pointData)
            {
                m_PointData = pointData;
                m_PointData.GeometryUpdated += OnUniqueDeterminingPropertyUpdated;
            }
            
            private void OnUniqueDeterminingPropertyUpdated()
            {
                UniqueDeterminingPropertyUpdated?.Invoke();
            }

            public void SetIsUnique(bool unique)
            {
                if (unique == m_IsUnique)
                {
                    return;
                }

                m_IsUnique = unique;
                UniquenessUpdated?.Invoke();
            }

            public int GetUniqueHashCode()
            {
                return Position.GetHashCode();
            }

            public bool UniqueEquals(PointPositionUniquenessValidatable validatable)
            {
                return Position == validatable.Position;
            }
        }
    }
}
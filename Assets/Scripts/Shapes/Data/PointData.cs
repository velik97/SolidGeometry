using System;
using Shapes.Validators.Point;
using Shapes.Validators.Uniqueness;
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

        public PointNameUniquenessValidator NameUniquenessValidator;
        public PointNameNotEmptyValidator NameNotEmptyValidator;
        
        public PointPositionUniquenessValidator PositionUniquenessValidator;

        protected Vector3 m_Position = Vector3.zero;
        private string m_PointName = string.Empty;
        private bool m_IsAccessoryPoint = false;

        public PointData()
        {
            NameUniquenessValidator = new PointNameUniquenessValidator(this);
            PositionUniquenessValidator = new PointPositionUniquenessValidator(this);
            NameNotEmptyValidator = new PointNameNotEmptyValidator(this);
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

        public void SetPosition(Vector3 position)
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
    }
}
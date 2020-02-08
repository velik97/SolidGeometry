using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Shapes.Validators.Point;
using Shapes.Validators.Uniqueness;
using Shapes.View;
using UniRx;
using UnityEngine;

namespace Shapes.Data
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class PointData : ShapeData
    {
        public PointView PointView => View as PointView;
        
        public Vector3 Position => m_Position;
        public string PointName => m_PointName;
        public bool IsAccessoryPoint => m_IsAccessoryPoint;

        public PointNameUniquenessValidator NameUniquenessValidator;
        public PointNameNotEmptyValidator NameNotEmptyValidator;
        public PointPositionUniquenessValidator PositionUniquenessValidator;

        [JsonProperty]
        private Vector3 m_Position = Vector3.zero;
        [JsonProperty]
        private string m_PointName = string.Empty;
        [JsonProperty]
        private bool m_IsAccessoryPoint = false;

        public PointData()
        {
            OnDeserialized();
        }

        [JsonConstructor]
        public PointData(object _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            OnDeserialized();
        }
        
        private void OnDeserialized()
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
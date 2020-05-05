using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Serialization;
using UnityEngine;

namespace Lesson.Shapes.Datas.SolidsOfRevolution
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class CircleData : ShapeData
    {
        // View
        
        public Vector3 CenterPosition => m_CenterPosition;
        public Vector3 Normal => m_Normal;
        public float Radius => m_Radius;

        [JsonProperty]
        private Vector3 m_CenterPosition = Vector3.zero;
        [JsonProperty]
        private Vector3 m_Normal = Vector3.zero;
        [JsonProperty]
        private float m_Radius = 1f;

        public CircleData()
        {
            OnDeserialized();
        }

        [JsonConstructor]
        public CircleData(JsonConstructorMark _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            OnDeserialized();
        }
        
        private void OnDeserialized()
        {
            // Validators
        }

        public void SetCenterPosition(Vector3 position)
        {
            if (position == m_CenterPosition)
            {
                return;
            }
            m_CenterPosition = position;
            OnGeometryUpdated();
        }
        
        public void SetNormal(Vector3 axis)
        {
            if (axis == m_Normal)
            {
                return;
            }
            m_Normal = axis;
            OnGeometryUpdated();
        }
        
        public void SetRadius(float radius)
        {
            if (radius == m_Radius)
            {
                return;
            }
            m_Radius = radius;
            OnGeometryUpdated();
        }

        public override string ToString()
        {
            return $"Circle {GetHashCode() % 100}";
        }
    }
}
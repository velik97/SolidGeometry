using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Serialization;
using UnityEngine;

namespace Lesson.Shapes.Datas.SolidsOfRevolution
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class HollowConeData : ShapeData
    {
        // View
        
        public Vector3 OriginPosition => m_OriginPosition;
        public float Radius => m_Radius;
        public float Height => m_Height;

        [JsonProperty]
        private Vector3 m_OriginPosition = Vector3.zero;
        [JsonProperty]
        private float m_Radius = 1f;
        [JsonProperty]
        private float m_Height = 1f;

        public HollowConeData()
        {
            OnDeserialized();
        }

        [JsonConstructor]
        public HollowConeData(JsonConstructorMark _)
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

        public void SetOriginPosition(Vector3 position)
        {
            if (position == m_OriginPosition)
            {
                return;
            }
            m_OriginPosition = position;
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
        
        public void SetHeight(float height)
        {
            if (height == m_Height)
            {
                return;
            }
            m_Height = height;
            OnGeometryUpdated();
        }

        public override string ToString()
        {
            return $"Hollow Cone {GetHashCode() % 100}";
        }
    }
}
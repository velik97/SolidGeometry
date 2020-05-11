using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Serialization;
using UnityEngine;

namespace Lesson.Shapes.Datas.SolidsOfRevolution
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class HollowCylinderData : ShapeData
    {
        // View
        
        public Vector3 OriginPosition => m_OriginPosition;
        public float TopRadius => m_TopRadius;
        public float BottomRadius => m_BottomRadius;
        public float Height => m_Height;

        [JsonProperty]
        private Vector3 m_OriginPosition = Vector3.zero;
        [JsonProperty]
        private float m_TopRadius = 1f;
        [JsonProperty]
        private float m_BottomRadius = 1f;
        [JsonProperty]
        private float m_Height = 1f;

        public HollowCylinderData()
        {
            OnDeserialized();
        }

        [JsonConstructor]
        public HollowCylinderData(JsonConstructorMark _)
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
        
        public void SetTopRadius(float radius)
        {
            if (radius == m_TopRadius)
            {
                return;
            }
            m_TopRadius = radius;
            OnGeometryUpdated();
        }
        
        public void SetBottomRadius(float radius)
        {
            if (radius == m_BottomRadius)
            {
                return;
            }
            m_BottomRadius = radius;
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
            return $"Hollow Cylinder {GetHashCode() % 100}";
        }
    }
}
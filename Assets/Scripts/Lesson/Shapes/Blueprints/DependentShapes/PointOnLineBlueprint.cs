using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Datas;
using Lesson.Validators;
using Newtonsoft.Json;
using Serialization;
using UnityEngine;
using Util;

namespace Lesson.Shapes.Blueprints.DependentShapes
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class PointOnLineBlueprint : ShapeBlueprint
    {
        [JsonProperty] public readonly PointData PointData;

        [JsonProperty] private PointData m_FirstPoint;
        [JsonProperty] private PointData m_SecondPoint;

        [JsonProperty] private float m_Coefficient;
    
        public PointData FirstPoint => m_FirstPoint;
        public PointData SecondPoint => m_SecondPoint;
        public float Coefficient => m_Coefficient;

        public PointsNotSameValidator PointsNotSameValidator;

        public override ShapeData MainShapeData => PointData;

        public PointOnLineBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            PointData = ShapeDataFactory.CreatePointData();

            OnDeserialized();
        }

        [JsonConstructor]
        public PointOnLineBlueprint(JsonConstructorMark _)
        {
        }

        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            if (m_FirstPoint != null)
            {
                m_FirstPoint.GeometryUpdated.Subscribe(GeometryUpdated);
            }

            if (m_SecondPoint != null)
            {
                m_SecondPoint.GeometryUpdated.Subscribe(GeometryUpdated);
            }

            RestoreDependencies();
            OnDeserialized();
        }

        private void OnDeserialized()
        {
            AddToMyShapeDatas(PointData);


            PointsNotSameValidator = new PointsNotSameValidator(EnumeratePoints());
            PointData.NameUpdated.Subscribe(NameUpdated);

            GeometryUpdated.Invoke();
        }

        private IEnumerable<PointData> EnumeratePoints()
        {
            yield return m_FirstPoint;
            yield return m_SecondPoint;
        }

        public void SetFirstPoint(PointData pointData)
        {
            if (m_FirstPoint == pointData)
            {
                return;
            }

            if (m_FirstPoint != null)
            {
                m_FirstPoint.GeometryUpdated.Unsubscribe(GeometryUpdated);
            }

            m_FirstPoint = pointData;
            if (m_FirstPoint != null)
            {
                m_FirstPoint.GeometryUpdated.Subscribe(GeometryUpdated);
            }

            GeometryUpdated.Invoke();
        }

        public void SetSecondPoint(PointData pointData)
        {
            if (m_SecondPoint == pointData)
            {
                return;
            }

            if (m_SecondPoint != null)
            {
                m_SecondPoint.GeometryUpdated.Unsubscribe(GeometryUpdated);
            }

            m_SecondPoint = pointData;
            if (m_SecondPoint != null)
            {
                m_SecondPoint.GeometryUpdated.Subscribe(GeometryUpdated);
            }

            GeometryUpdated.Invoke();
        }
        
        public void SetCoefficient(float coef)
        {
            if (coef < 0 )
            {
                return;
            }

            m_Coefficient = coef;
            GeometryUpdated.Invoke();
        }
        
        protected override void UpdateGeometry()
        {
            PointsNotSameValidator.Update();
            if (!PointsNotSameValidator.IsValid())
            {
                return;
            }

            if (m_FirstPoint == null || m_SecondPoint == null || m_Coefficient == 0)
            {
                return;
            }

            Vector3 v = m_SecondPoint.Position - m_FirstPoint.Position;
            
            PointData.SetPosition(m_FirstPoint.Position + v*m_Coefficient);
        }
    }
}

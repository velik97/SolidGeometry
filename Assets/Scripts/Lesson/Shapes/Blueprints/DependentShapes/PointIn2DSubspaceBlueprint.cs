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
    public class PointIn2DSubspaceBlueprint : ShapeBlueprint
    {
        [JsonProperty] public readonly PointData PointData;

        [JsonProperty] private PointData m_ThirdPoint;
        [JsonProperty] private PointData m_SourcePoint;
        [JsonProperty] private PointData m_SecondPoint;

        [JsonProperty] private float m_Coefficient1;
        [JsonProperty] private float m_Coefficient2;
    
        public PointData SourcePoint => m_SourcePoint;
        public PointData SecondPoint => m_SecondPoint;
        public PointData ThirdPoint => m_ThirdPoint;
        public float Coefficient1 => m_Coefficient1;
        public float Coefficient2 => m_Coefficient2;


        public PointsNotSameValidator PointsNotSameValidator;

        public override ShapeData MainShapeData => PointData;

        public PointIn2DSubspaceBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            PointData = ShapeDataFactory.CreatePointData();

            OnDeserialized();
        }

        [JsonConstructor]
        public PointIn2DSubspaceBlueprint(JsonConstructorMark _)
        {
        }

        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            if (m_ThirdPoint != null)
            {
                m_ThirdPoint.GeometryUpdated += UpdatePosition;
            }

            if (m_SourcePoint != null)
            {
                m_SourcePoint.GeometryUpdated += UpdatePosition;
            }

            if (m_SecondPoint != null)
            {
                m_SecondPoint.GeometryUpdated += UpdatePosition;
            }

            RestoreDependences();
            OnDeserialized();
        }

        private void OnDeserialized()
        {
            PointData.SourceBlueprint = this;
            MyShapeDatas.Add(PointData);

            PointsNotSameValidator = new PointsNotSameValidator(EnumeratePoints());
            PointData.NameUpdated += OnNameUpdated;

            UpdatePosition();
        }

        private IEnumerable<PointData> EnumeratePoints()
        {
            yield return m_SourcePoint;
            yield return m_SecondPoint;
            yield return m_ThirdPoint;
        }

        public void SetSourcePoint(PointData pointData)
        {
            if (m_SourcePoint == pointData)
            {
                return;
            }

            if (m_SourcePoint != null)
            {
                m_SourcePoint.GeometryUpdated -= UpdatePosition;
            }

            m_SourcePoint = pointData;
            if (m_SourcePoint != null)
            {
                m_SourcePoint.GeometryUpdated += UpdatePosition;
            }

            UpdatePosition();
        }

        public void SetSecondPoint(PointData pointData)
        {
            if (m_SecondPoint == pointData)
            {
                return;
            }

            if (m_SecondPoint != null)
            {
                m_SecondPoint.GeometryUpdated -= UpdatePosition;
            }

            m_SecondPoint = pointData;
            if (m_SecondPoint != null)
            {
                m_SecondPoint.GeometryUpdated += UpdatePosition;
            }

            UpdatePosition();
        }

        public void SetThirdPoint(PointData pointData)
        {
            if (m_ThirdPoint == pointData)
            {
                return;
            }

            if (m_ThirdPoint != null)
            {
                m_ThirdPoint.GeometryUpdated -= UpdatePosition;
            }

            m_ThirdPoint = pointData;
            if (m_ThirdPoint != null)
            {
                m_ThirdPoint.GeometryUpdated += UpdatePosition;
            }

            UpdatePosition();
        }
        
        public void SetCoefficient1(float kof)
        {
            if (kof < 0 )
            {
                return;
            }

            m_Coefficient1 = kof;
            //NonZeroVolumeValidator.Update();
            UpdatePosition();
        }
        
        public void SetCoefficient2(float kof)
        {
            if (kof < 0 )
            {
                return;
            }

            m_Coefficient2 = kof;
            //NonZeroVolumeValidator.Update();
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            PointsNotSameValidator.Update();
            if (!PointsNotSameValidator.IsValid())
            {
                return;
            }

            if (m_ThirdPoint == null || m_SourcePoint == null || m_SecondPoint == null)
            {
                return;
            }

            Vector3 v2 = m_ThirdPoint.Position - m_SourcePoint.Position;
            Vector3 v1 = m_SecondPoint.Position - m_SourcePoint.Position;
            Vector3 sum = m_SourcePoint.Position + v1 * Coefficient1 + v2 * Coefficient2;
            
            PointData.SetPosition(sum);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Shapes.Data.Uniqueness;
using Shapes.View;
using UnityEngine;

namespace Shapes.Data
{
    [Serializable]
    public class ShapeDataFactory
    {
        [SerializeField] private readonly List<PointData> m_PointDatas = new List<PointData>();
        [SerializeField] private readonly List<LineData> m_LinetDatas = new List<LineData>();
        [SerializeField] private readonly List<PolygonData> m_PolygonDatas = new List<PolygonData>();
        [SerializeField] private readonly List<CompositeShapeData> m_CompositeShapeDatas = new List<CompositeShapeData>();

        private readonly UniquenessValidator<PointData.PointNameUniquenessValidatable> m_PointsByNameUniquenessValidator =
            new UniquenessValidator<PointData.PointNameUniquenessValidatable>(256);
        private readonly UniquenessValidator<PointData.PointPositionUniquenessValidatable> m_PointsByPositionUniquenessValidator =
            new UniquenessValidator<PointData.PointPositionUniquenessValidatable>(256);

        public event Action ShapesListUpdated;

        public IReadOnlyCollection<PointData> PointDatas => m_PointDatas;

        public IReadOnlyCollection<ShapeData> AllDatas =>
            m_PointDatas.Cast<ShapeData>()
                .Concat(m_LinetDatas)
                .Concat(m_PolygonDatas)
                .Concat(m_CompositeShapeDatas)
                .ToList();

        public PointData CreatePointData()
        {
            PointData pointData = new PointData();
            
            m_PointDatas.Add(pointData);
            m_PointsByNameUniquenessValidator.AddValidatable(pointData.NameUniquenessValidatable);
            m_PointsByPositionUniquenessValidator.AddValidatable(pointData.PositionUniquenessValidatable);
            ShapesListUpdated?.Invoke();
            pointData.NameUpdated += OnPointsListUpdated;
            return pointData;
        }
        
        public PointData CreateConditionalPointData()
        {
            PointData pointData = new ConditionalPointData();
            
            m_PointDatas.Add(pointData);
            m_PointsByNameUniquenessValidator.AddValidatable(pointData.NameUniquenessValidatable);
            m_PointsByPositionUniquenessValidator.AddValidatable(pointData.PositionUniquenessValidatable);
            ShapesListUpdated?.Invoke();
            pointData.NameUpdated += OnPointsListUpdated;
            return pointData;
        }

        public LineData CreateLineData()
        {
            LineData lineData = new LineData();
            
            m_LinetDatas.Add(lineData);
            ShapesListUpdated?.Invoke();
            lineData.NameUpdated += OnPointsListUpdated;
            return lineData;
        }

        public PolygonData CreatePolygonData()
        {
            PolygonData polygonData = new PolygonData();
            
            m_PolygonDatas.Add(polygonData);
            ShapesListUpdated?.Invoke();
            polygonData.NameUpdated += OnPointsListUpdated;
            return polygonData;
        }

        public CompositeShapeData CreateCompositeShapeData(
            PointData[] pointDatas,
            LineData[] lineDatas,
            PolygonData[] polygonDatas,
            string shapeName)
        {
            CompositeShapeData compositeShapeData = new CompositeShapeData(pointDatas, lineDatas, polygonDatas, shapeName);
            
            m_CompositeShapeDatas.Add(compositeShapeData);
            ShapesListUpdated?.Invoke();
            compositeShapeData.NameUpdated += OnPointsListUpdated;
            return compositeShapeData;
        }
        
        public void RemoveShapeData(ShapeData data)
        {
            data.NameUpdated -= OnPointsListUpdated;
            switch (data)
            {
                case PointData pointData:
                    m_PointDatas.Remove(pointData);
                    m_PointsByNameUniquenessValidator.RemoveValidatable(pointData.NameUniquenessValidatable);
                    m_PointsByPositionUniquenessValidator.RemoveValidatable(pointData.PositionUniquenessValidatable);
                    break;
                case LineData lineData:
                    m_LinetDatas.Remove(lineData);
                    break;
                case PolygonData polygonData:
                    m_PolygonDatas.Remove(polygonData);
                    break;
                case CompositeShapeData compositeShapeData:
                    m_CompositeShapeDatas.Remove(compositeShapeData);
                    break;
            }
            ShapesListUpdated?.Invoke();
        }

        private void OnPointsListUpdated()
        {
            ShapesListUpdated?.Invoke();
        }
    }
}
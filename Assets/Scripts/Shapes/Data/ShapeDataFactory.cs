using System;
using System.Collections.Generic;
using System.Linq;
using Shapes.Validators.Point;
using Shapes.Validators.Uniqueness;
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
        
        private ShapeDatasUniquenessValidators m_UniquenessValidators = new ShapeDatasUniquenessValidators();

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
            ShapesListUpdated?.Invoke();
            pointData.NameUpdated += OnPointsListUpdated;
            
            m_UniquenessValidators.AddShapeData(pointData);
            return pointData;
        }

        public LineData CreateLineData()
        {
            LineData lineData = new LineData();
            
            m_LinetDatas.Add(lineData);
            ShapesListUpdated?.Invoke();
            lineData.NameUpdated += OnPointsListUpdated;
            
            m_UniquenessValidators.AddShapeData(lineData);
            return lineData;
        }

        public PolygonData CreatePolygonData()
        {
            PolygonData polygonData = new PolygonData();
            
            m_PolygonDatas.Add(polygonData);
            ShapesListUpdated?.Invoke();
            polygonData.NameUpdated += OnPointsListUpdated;
            
            m_UniquenessValidators.AddShapeData(polygonData);
            return polygonData;
        }

        public CompositeShapeData CreateCompositeShapeData()
        {
            CompositeShapeData compositeShapeData = new CompositeShapeData();
            
            m_CompositeShapeDatas.Add(compositeShapeData);
            ShapesListUpdated?.Invoke();
            compositeShapeData.NameUpdated += OnPointsListUpdated;
            
            m_UniquenessValidators.AddShapeData(compositeShapeData);
            return compositeShapeData;
        }

        public void RemoveShapeData(ShapeData data)
        {
            data.NameUpdated -= OnPointsListUpdated;
            m_UniquenessValidators.RemoveShapeData(data);
            switch (data)
            {
                case PointData pointData:
                    m_PointDatas.Remove(pointData);
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
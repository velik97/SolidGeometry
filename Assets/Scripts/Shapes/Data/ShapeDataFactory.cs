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
        
        private readonly ShapeDatasUniquenessValidators m_UniquenessValidators = new ShapeDatasUniquenessValidators();

        private ShapeViewFactory m_ShapeViewFactory;

        public event Action ShapesListUpdated;

        public IReadOnlyCollection<PointData> PointDatas => m_PointDatas;

        public IReadOnlyCollection<ShapeData> AllDatas =>
            m_PointDatas.Cast<ShapeData>()
                .Concat(m_LinetDatas)
                .Concat(m_PolygonDatas)
                .Concat(m_CompositeShapeDatas)
                .ToList();

        public void SetViewFactory(ShapeViewFactory shapeViewFactory)
        {
            m_ShapeViewFactory = shapeViewFactory;
        }
        
        public PointData CreatePointData()
        {
            PointData pointData = new PointData();
            m_PointDatas.Add(pointData);
            pointData.NameUpdated += OnPointsListUpdated;
            ProcessNewShapeData(pointData);
            return pointData;
        }

        public LineData CreateLineData()
        {
            LineData lineData = new LineData();
            m_LinetDatas.Add(lineData);
            ProcessNewShapeData(lineData);
            return lineData;
        }

        public PolygonData CreatePolygonData()
        {
            PolygonData polygonData = new PolygonData();
            m_PolygonDatas.Add(polygonData);
            ProcessNewShapeData(polygonData);
            return polygonData;
        }

        public CompositeShapeData CreateCompositeShapeData()
        {
            CompositeShapeData compositeShapeData = new CompositeShapeData();
            m_CompositeShapeDatas.Add(compositeShapeData);
            ProcessNewShapeData(compositeShapeData);
            return compositeShapeData;
        }

        private void ProcessNewShapeData(ShapeData shapeData)
        {
            ShapesListUpdated?.Invoke();
            m_UniquenessValidators.AddShapeData(shapeData);
            IShapeView view = m_ShapeViewFactory?.RequestShapeView(shapeData);
            if (view != null)
            {
                shapeData.AttachView(view);
            }
        }

        public void RemoveShapeData(ShapeData shapeData)
        {
            m_UniquenessValidators.RemoveShapeData(shapeData);
            switch (shapeData)
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
            if (shapeData.View != null)
            {
                m_ShapeViewFactory?.ReleaseView(shapeData.View);
            }
            shapeData.DestroyData();
            ShapesListUpdated?.Invoke();
        }

        private void OnPointsListUpdated()
        {
            ShapesListUpdated?.Invoke();
        }
    }
}
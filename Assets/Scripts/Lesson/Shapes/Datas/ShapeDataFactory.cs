using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Views;
using Lesson.Validators.Uniqueness;
using Newtonsoft.Json;
using UnityEngine;
using Util.CascadeUpdate;

namespace Lesson.Shapes.Datas
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class ShapeDataFactory
    {
        public CascadeUpdateEvent BecameDirty = new CascadeUpdateEvent();

        [JsonProperty]
        private Vector3 m_Origin;
        
        [JsonProperty]
        private readonly List<PointData> m_PointDatas = new List<PointData>();
        [JsonProperty]
        private readonly List<LineData> m_LinetDatas = new List<LineData>();
        [JsonProperty]
        private readonly List<PolygonData> m_PolygonDatas = new List<PolygonData>();
        [JsonProperty]
        private readonly List<CompositeShapeData> m_CompositeShapeDatas = new List<CompositeShapeData>();
        
        private readonly ShapeDatasUniquenessValidators m_UniquenessValidators = new ShapeDatasUniquenessValidators();

        private IShapeViewFactory m_ShapeViewFactory;
        public IReadOnlyCollection<PointData> PointDatas => m_PointDatas;

        public Vector3 Center => m_Origin;

        public IReadOnlyCollection<ShapeData> AllDatas =>
            m_PointDatas.Cast<ShapeData>()
                .Concat(m_LinetDatas)
                .Concat(m_PolygonDatas)
                .Concat(m_CompositeShapeDatas)
                .ToList();

        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (ShapeData shapeData in AllDatas)
            {
                ProcessNewShapeData(shapeData);
            }
        }

        public IReadOnlyList<TShapeData> GetShapeDatasList<TShapeData>() where TShapeData : ShapeData
        {
            return AllDatas.Where(data => data is TShapeData).Cast<TShapeData>().ToList();
        }

        public void Clear()
        {
            foreach (ShapeData shapeData in AllDatas)
            {
                RemoveShapeData(shapeData);
            }
        }

        public void SetViewFactory(IShapeViewFactory shapeViewFactory)
        {
            if (m_ShapeViewFactory != null)
            {
                m_ShapeViewFactory.Dispose();
            }
            m_ShapeViewFactory = shapeViewFactory;
            if (m_ShapeViewFactory == null)
            {
                return;
            }
            foreach (ShapeData shapeData in AllDatas)
            {
                shapeData.AttachView(m_ShapeViewFactory.RequestShapeView(shapeData));
            }
        }
        
        public PointData CreatePointData()
        {
            PointData pointData = new PointData();
            m_PointDatas.Add(pointData);
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
            OnBecameDirty();
            shapeData.NameUpdated.Subscribe(OnBecameDirty);
            shapeData.GeometryUpdated.Subscribe(OnBecameDirty);
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
            OnBecameDirty();
        }

        public void SetOrigin(Vector3 origin)
        {
            m_Origin = origin;
            OnBecameDirty();
        }

        private void OnBecameDirty()
        {
            BecameDirty?.Invoke();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Lesson.Shapes.Datas;
using Runtime;
using Runtime.Core;
using UnityEngine;
using Util;

namespace Lesson.Shapes.Views
{
    public class ShapeViewFactory : MonoSingleton<ShapeViewFactory>, IShapeViewFactory, ISceneRunner
    {
        [SerializeField] private Transform m_DisposedContainer;

        [SerializeField] private PointView m_PointPrefab;
        [SerializeField] private LineView m_LinePrefab;
        [SerializeField] private PolygonView m_PolygonPrefab;

        private Stack<PointView> m_PointsPool = new Stack<PointView>();
        private Stack<LineView> m_LinesPool = new Stack<LineView>();
        private Stack<PolygonView> m_PolygonsPool = new Stack<PolygonView>();
        
        private readonly List<IShapeView> m_ViewsInUse = new List<IShapeView>();

        public void Initialize(GlobalData globalData)
        {
        }

        public IShapeView RequestShapeView(ShapeData data)
        {
            IShapeView view = null;
            switch (data)
            {
                case PointData pointData:
                    view = CreatePointView(pointData);
                    break;
                case LineData lineData:
                    view = CreateLineView(lineData);
                    break;
                case PolygonData polygonData:
                    view = CreatePolygonView(polygonData);
                    break;
                case CompositeShapeData compositeShapeData:
                    view = CreateCompositeShapeView(compositeShapeData);
                    break;
            }
            if (view != null)
            {
                m_ViewsInUse.Add(view);
            }
            return view;
        }
        
        private PointView CreatePointView(PointData data)
        {
            PointView pointView = m_PointsPool.Count > 0
                ? m_PointsPool.Pop()
                : Instantiate(m_PointPrefab, transform, false);

            pointView.SetShapeData(data);
            return pointView;
        }
        
        private LineView CreateLineView(LineData data)
        {
            LineView lineView = m_LinesPool.Count > 0
                ? m_LinesPool.Pop()
                : Instantiate(m_LinePrefab, transform, false);
            
            lineView.SetShapeData(data);
            return lineView;
        }
        
        private PolygonView CreatePolygonView(PolygonData data)
        {
            PolygonView polygonView = m_PolygonsPool.Count > 0
                ? m_PolygonsPool.Pop()
                : Instantiate(m_PolygonPrefab, transform, false);
            
            polygonView.SetShapeData(data);
            return polygonView;
        }

        private CompositeShapeView CreateCompositeShapeView(CompositeShapeData data)
        {
            return new CompositeShapeView(data);
        }

        public void Unload(Action callback)
        {
            Dispose();
            callback.Invoke();
        }

        public void Dispose()
        {
            while (m_ViewsInUse.Count > 0)
            {
                ReleaseView(m_ViewsInUse[m_ViewsInUse.Count - 1]);
            }
        }
        
        public void ReleaseView(IShapeView view)
        {
            if (m_ViewsInUse.Contains(view))
            {
                m_ViewsInUse.Remove(view);
            }
            switch (view)
            {
                case PointView pointView:
                    pointView.transform.parent = m_DisposedContainer;
                    pointView.transform.Reset();
                    pointView.SetShapeData(null);
                    m_PointsPool.Push(pointView);
                    break;
                case LineView lineView:
                    lineView.transform.parent = m_DisposedContainer;
                    lineView.transform.Reset();
                    lineView.SetShapeData(null);
                    m_LinesPool.Push(lineView);
                    break;
                case PolygonView polygonView:
                    polygonView.transform.parent = m_DisposedContainer;
                    polygonView.transform.Reset();
                    polygonView.SetShapeData(null);
                    m_PolygonsPool.Push(polygonView);
                    break;
            }
        }

        /// <summary>
        /// After recompiling project views might be lost
        /// </summary>
        public void CollectLostViews()
        {
            ClearNullViews();
            
            if (m_PointsPool.Count == 0)
            {
                foreach (PointView pointView in m_DisposedContainer.GetComponentsInChildren<PointView>())
                {
                    m_PointsPool.Push(pointView);
                }
            }
            
            if (m_LinesPool.Count == 0)
            {
                foreach (LineView lineView in m_DisposedContainer.GetComponentsInChildren<LineView>())
                {
                    m_LinesPool.Push(lineView);
                }
            }
            
            if (m_PolygonsPool.Count == 0)
            {
                foreach (PolygonView polygonView in m_DisposedContainer.GetComponentsInChildren<PolygonView>())
                {
                    m_PolygonsPool.Push(polygonView);
                }
            }
        }
        
        private void ClearNullViews()
        {
            IEnumerable<PointView> notNullPointsInPool = m_PointsPool.AsEnumerable().Where(view => view != null);
            m_PointsPool = new Stack<PointView>(notNullPointsInPool);
            
            IEnumerable<LineView> notNullLinesInPool = m_LinesPool.AsEnumerable().Where(view => view != null);
            m_LinesPool = new Stack<LineView>(notNullLinesInPool);
            
            IEnumerable<PolygonView> notNullPolygonsInPool = m_PolygonsPool.AsEnumerable().Where(view => view != null);
            m_PolygonsPool = new Stack<PolygonView>(notNullPolygonsInPool);
            
            m_ViewsInUse.RemoveAll(view => view == null);
        }
    }
}
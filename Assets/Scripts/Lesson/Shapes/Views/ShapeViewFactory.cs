using System.Collections.Generic;
using Lesson.Shapes.Datas;
using UnityEngine;

namespace Lesson.Shapes.Views
{
    public class ShapeViewFactory : MonoBehaviour, IShapeViewFactory
    {
        [SerializeField] private PointView m_PointPrefab;
        [SerializeField] private LineView m_LinePrefab;
        [SerializeField] private PolygonView m_PolygonPrefab;

        private readonly Stack<PointView> m_PointsPool = new Stack<PointView>();
        private readonly Stack<LineView> m_LinesPool = new Stack<LineView>();
        private readonly Stack<PolygonView> m_PolygonsPool = new Stack<PolygonView>();
        
        private readonly List<IShapeView> m_Views = new List<IShapeView>();

        private void Awake()
        {
            gameObject.SetActive(false);
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
                m_Views.Add(view);
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
            PolygonView polygonView = m_PointsPool.Count > 0
                ? m_PolygonsPool.Pop()
                : Instantiate(m_PolygonPrefab, transform, false);
            
            polygonView.SetShapeData(data);
            return polygonView;
        }

        private CompositeShapeView CreateCompositeShapeView(CompositeShapeData data)
        {
            return new CompositeShapeView(data);
        }

        public void Clear()
        {
            while (m_Views.Count > 0)
            {
                ReleaseView(m_Views[m_Views.Count - 1]);
            }
        }
        
        public void ReleaseView(IShapeView view)
        {
            if (!m_Views.Contains(view))
            {
                return;
            }
            m_Views.Remove(view);
            
            switch (view)
            {
                case PointView pointView:
                    pointView.transform.parent = transform;
                    m_PointsPool.Push(pointView);
                    break;
                case LineView lineView:
                    lineView.transform.parent = transform;
                    m_LinesPool.Push(lineView);
                    break;
                case PolygonView polygonView:
                    polygonView.transform.parent = transform;
                    m_PolygonsPool.Push(polygonView);
                    break;
            }
        }
    }
}
using System.Collections.Generic;
using Lesson.Shapes.Datas;
using Shapes.Data;
using UnityEngine;

namespace Lesson.Shapes.Views
{
    public class ShapeViewFactory : MonoBehaviour
    {
        [SerializeField] private PointView m_PointPrefab;
        [SerializeField] private LineView m_LinePrefab;
        [SerializeField] private PolygonView m_PolygonPrefab;

        private List<IShapeView> m_Views = new List<IShapeView>();

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
            PointView pointView = Instantiate(m_PointPrefab, transform, false);
            pointView.SetShapeData(data);
            return pointView;
        }
        
        private LineView CreateLineView(LineData data)
        {
            LineView lineView = Instantiate(m_LinePrefab, transform, false);
            lineView.SetShapeData(data);
            return lineView;
        }
        
        private PolygonView CreatePolygonView(PolygonData data)
        {
            PolygonView polygonView = Instantiate(m_PolygonPrefab, transform, false);
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
                ReleaseView(m_Views[0]);
            }
        }
        
        public void ReleaseView(IShapeView view)
        {
            if (!m_Views.Contains(view))
            {
                return;
            }
            m_Views.Remove(view);
            view.Release();
        }
    }
}
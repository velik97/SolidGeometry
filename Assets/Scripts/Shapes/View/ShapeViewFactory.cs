using System.Linq;
using Shapes.Data;
using UnityEngine;

namespace Shapes.View
{
    public class ShapeViewFactory : MonoBehaviour
    {
        [SerializeField] private PointView m_PointPrefab;
        [SerializeField] private LineView m_LinePrefab;
        [SerializeField] private PolygonView m_PolygonPrefab;

        public IShapeView RequestShapeView(ShapeData data)
        {
            switch (data)
            {
                case PointData pointData:
                    return CreatePointView(pointData);
                case LineData lineData:
                    return CreateLineView(lineData);
                case PolygonData polygonData:
                    return CreatePolygonView(polygonData);
                case CompositeShapeData compositeShapeData:
                    return CreateCompositeShapeView(compositeShapeData);
            }
            return null;
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
            return null;
        }

        public CompositeShapeView CreateCompositeShapeView(CompositeShapeData data)
        {
            return new CompositeShapeView(data);
        }
        
        public void ReleaseView(IShapeView view)
        {
            view.Release();
        }
    }
}
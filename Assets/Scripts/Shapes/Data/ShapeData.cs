using System;
using Shapes.Blueprint;
using Shapes.View;

namespace Shapes.Data
{
    public abstract class ShapeData
    {
        public event Action NameUpdated;
        public event Action GeometryUpdated;
        
        public IShapeView View { get; private set; }
        
        public ShapeBlueprint SourceBlueprint;

        public void AttachView(IShapeView view)
        {
            View = view;
        }
        
        protected void SubscribeOnPoint(PointData pointData)
        {
            if (pointData != null)
            {
                pointData.NameUpdated += OnNameUpdated;
                pointData.GeometryUpdated += OnGeometryUpdated;
            }
            OnNameUpdated();
            OnGeometryUpdated();
        }

        protected void UnsubscribeFromPoint(PointData pointData)
        {
            if (pointData != null)
            {
                pointData.NameUpdated -= OnNameUpdated;
                pointData.GeometryUpdated -= OnGeometryUpdated;
            }
        }

        protected void OnNameUpdated()
        {
            NameUpdated?.Invoke();
        }

        protected void OnGeometryUpdated()
        {
            GeometryUpdated?.Invoke();
        }
    }
}
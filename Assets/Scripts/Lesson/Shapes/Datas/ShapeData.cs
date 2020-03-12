using System;
using Lesson.Shapes.Blueprints;
using Lesson.Shapes.Views;
using Newtonsoft.Json;

namespace Lesson.Shapes.Datas
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
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

        public void DestroyData()
        {
            View = null;
            if (NameUpdated != null)
            {
                foreach (var d in NameUpdated.GetInvocationList())
                {
                    NameUpdated -= (d as Action);
                }
            }
            if (GeometryUpdated != null)
            {
                foreach (var d in GeometryUpdated.GetInvocationList())
                {
                    GeometryUpdated -= (d as Action);
                }
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
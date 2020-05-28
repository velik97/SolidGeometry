using System;
using Lesson.Shapes.Blueprints;
using Lesson.Shapes.Views;
using Newtonsoft.Json;
using Util.CascadeUpdate;

namespace Lesson.Shapes.Datas
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public abstract class ShapeData
    {
        public CascadeUpdateEvent NameUpdated = new CascadeUpdateEvent();
        public CascadeUpdateEvent GeometryUpdated = new CascadeUpdateEvent();
        
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
                pointData.NameUpdated.Subscribe(NameUpdated);
                pointData.GeometryUpdated.Subscribe(GeometryUpdated);
            }
            OnNameUpdated();
            OnGeometryUpdated();
        }

        protected void UnsubscribeFromPoint(PointData pointData)
        {
            if (pointData != null)
            {
                pointData.NameUpdated.Unsubscribe(NameUpdated);
                pointData.GeometryUpdated.Unsubscribe(GeometryUpdated);
            }
        }

        public void DestroyData()
        {
            View = null;
            NameUpdated?.Clear();
            GeometryUpdated.Clear();
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
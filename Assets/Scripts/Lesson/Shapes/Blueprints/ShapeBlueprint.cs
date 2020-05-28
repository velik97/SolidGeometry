using System;
using System.Collections.Generic;
using Lesson.Shapes.Datas;
using Newtonsoft.Json;
using Util.CascadeUpdate;

namespace Lesson.Shapes.Blueprints
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public abstract class ShapeBlueprint : CanDependOnShapeBlueprint
    {
        protected CascadeUpdateEvent GeometryUpdated = new CascadeUpdateEvent();
        public CascadeUpdateEvent NameUpdated = new CascadeUpdateEvent();
        public CascadeUpdateEvent DependenciesUpdated = new CascadeUpdateEvent();
        
        /// <summary>
        /// List of shapes, that depend on me
        /// </summary>
        private readonly List<CanDependOnShapeBlueprint> m_DependentOnMeShapes = new List<CanDependOnShapeBlueprint>();
        
        private readonly List<ShapeData> MyShapeDatas = new List<ShapeData>();
        
        public bool HaveDependencies => m_DependentOnMeShapes.Count > 0;
        
        public abstract ShapeData MainShapeData { get; }

        private ShapeDataFactory m_ShapeDataFactory;
        public ShapeDataFactory ShapeDataFactory => m_ShapeDataFactory;

        protected ShapeBlueprint(ShapeDataFactory dataFactory)
        {
            m_ShapeDataFactory = dataFactory;
            GeometryUpdated.Subscribe(UpdateGeometry);
        }

        protected ShapeBlueprint()
        {
            GeometryUpdated.Subscribe(UpdateGeometry);
        }
        
        public void SetShapeDataFactory(ShapeDataFactory shapeDataFactory)
        {
            m_ShapeDataFactory = shapeDataFactory;
        }

        public override void Destroy()
        {
            base.Destroy();
            
            foreach (ShapeData data in MyShapeDatas)
            {
                m_ShapeDataFactory.RemoveShapeData(data);
            }
            MyShapeDatas.Clear();
        }

        protected void AddToMyShapeDatas(ShapeData shapeData)
        {
            MyShapeDatas.Add(shapeData);
            GeometryUpdated.Subscribe(shapeData.GeometryUpdated);
            shapeData.SourceBlueprint = this;
        }
        
        protected void RemoveMyShapeDatas(ShapeData shapeData)
        {
            MyShapeDatas.Remove(shapeData);
            GeometryUpdated.Unsubscribe(shapeData.GeometryUpdated);
            shapeData.SourceBlueprint = null;
        }

        protected void ClearMyShapeDatas()
        {
            foreach (ShapeData shapeData in MyShapeDatas)
            {
                GeometryUpdated.Unsubscribe(shapeData.GeometryUpdated);
                shapeData.SourceBlueprint = null;
            }
            MyShapeDatas.Clear();
        }

        public void AddDependence(CanDependOnShapeBlueprint dependentOnMe)
        {
            m_DependentOnMeShapes.Add(dependentOnMe);
            DependenciesUpdated?.Invoke();
        }
        
        public void RemoveDependence(CanDependOnShapeBlueprint blueprint)
        {
            m_DependentOnMeShapes.Remove(blueprint);
            DependenciesUpdated?.Invoke();
        }

        protected virtual void UpdateGeometry() { }
    }
}
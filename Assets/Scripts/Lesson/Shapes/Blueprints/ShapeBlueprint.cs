using System;
using System.Collections.Generic;
using Lesson.Shapes.Datas;
using Newtonsoft.Json;

namespace Lesson.Shapes.Blueprints
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public abstract class ShapeBlueprint : CanDependOnShapeBlueprint
    {
        public event Action NameUpdated;
        public event Action DependencesUpdated;
        
        /// <summary>
        /// List of shapes, that depend on me
        /// </summary>
        private readonly List<CanDependOnShapeBlueprint> m_DependentOnMeShapes = new List<CanDependOnShapeBlueprint>();
        
        protected readonly List<ShapeData> MyShapeDatas = new List<ShapeData>();
        
        public bool HaveDependences => m_DependentOnMeShapes.Count > 0;
        
        public abstract ShapeData MainShapeData { get; }

        private ShapeDataFactory m_ShapeDataFactory;
        public ShapeDataFactory ShapeDataFactory => m_ShapeDataFactory;

        protected ShapeBlueprint(ShapeDataFactory dataFactory)
        {
            m_ShapeDataFactory = dataFactory;
        }
        
        protected ShapeBlueprint()
        { }
        
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

        public void AddDependence(CanDependOnShapeBlueprint dependentOnMe)
        {
            m_DependentOnMeShapes.Add(dependentOnMe);
            DependencesUpdated?.Invoke();
        }
        
        public void RemoveDependence(CanDependOnShapeBlueprint blueprint)
        {
            m_DependentOnMeShapes.Remove(blueprint);
            DependencesUpdated?.Invoke();
        }

        protected void OnNameUpdated()
        {
            NameUpdated?.Invoke();
        }
    }
}
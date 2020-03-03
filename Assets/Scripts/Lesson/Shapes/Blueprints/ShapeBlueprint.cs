using System;
using System.Collections.Generic;
using Lesson.Shapes.Blueprints;
using Lesson.Shapes.Data;
using Newtonsoft.Json;
using Shapes.Data;

namespace Shapes.Blueprint
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

        [JsonProperty(IsReference = true)]
        public ShapeDataFactory DataFactory;

        protected ShapeBlueprint(ShapeDataFactory dataFactory)
        {
            DataFactory = dataFactory;
        }
        
        protected ShapeBlueprint()
        { }

        public override void Destroy()
        {
            base.Destroy();
            
            foreach (ShapeData data in MyShapeDatas)
            {
                DataFactory.RemoveShapeData(data);
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
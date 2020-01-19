using System;
using System.Collections.Generic;
using Shapes.Data;

namespace Shapes.Blueprint
{
    public abstract class ShapeBlueprint
    {
        public event Action NameUpdated;
        public event Action DependencesUpdated;
        
        /// <summary>
        /// List of shapes, that depend on me
        /// </summary>
        private readonly List<ShapeBlueprint> m_DependentOnMeShapes = new List<ShapeBlueprint>();
        
        /// <summary>
        /// List of shapes, I depend on
        /// </summary>
        private readonly List<ShapeBlueprint> m_DependencesOnOtherShapes = new List<ShapeBlueprint>();

        protected readonly List<ShapeData> MyShapeDatas = new List<ShapeData>();
        
        public bool HaveDependences => m_DependentOnMeShapes.Count > 0;
        
        public abstract ShapeData MainShapeData { get; }

        public readonly ShapeDataFactory DataFactory;

        protected ShapeBlueprint(ShapeDataFactory dataFactory)
        {
            DataFactory = dataFactory;
        }
        
        public void Destroy()
        {
            foreach (ShapeBlueprint d in m_DependencesOnOtherShapes)
            {
                d.RemoveDependence(this);
            }
            m_DependencesOnOtherShapes.Clear();
            
            foreach (ShapeData data in MyShapeDatas)
            {
                DataFactory.RemoveShapeData(data);
            }
            MyShapeDatas.Clear();
        }

        public void CreateDependenceOn(ShapeData shapeData)
        {
            m_DependencesOnOtherShapes.Add(shapeData.SourceBlueprint);
            shapeData.SourceBlueprint.AddDependence(this);
        }

        public void RemoveDependenceOn(ShapeData shapeData)
        {
            m_DependencesOnOtherShapes.Remove(shapeData.SourceBlueprint);
            shapeData.SourceBlueprint.RemoveDependence(this);
        }

        protected void AddDependence(ShapeBlueprint blueprint)
        {
            m_DependentOnMeShapes.Add(blueprint);
            DependencesUpdated?.Invoke();
        }
        
        private void RemoveDependence(ShapeBlueprint blueprint)
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
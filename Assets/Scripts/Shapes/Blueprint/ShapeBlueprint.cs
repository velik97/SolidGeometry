using System.Collections.Generic;
using Shapes.Data;

namespace Shapes.Blueprint
{
    public abstract class ShapeBlueprint
    {
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

        protected ShapeDataFactory DataFactory;

        protected ShapeBlueprint(ShapeDataFactory dataFactory)
        {
            DataFactory = dataFactory;
        }
        
        public abstract void Create();

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

        protected void AddDependence(ShapeBlueprint blueprint)
        {
            m_DependentOnMeShapes.Add(blueprint);
        }
        
        private void RemoveDependence(ShapeBlueprint blueprint)
        {
            m_DependentOnMeShapes.Remove(blueprint);
        }
        
    }
}
using Shapes.Data;

namespace Shapes.Blueprint
{
    [System.Serializable]
    public class PointBlueprint : ShapeBlueprint
    {
        private PointData m_PointData;
        
        public PointBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
        }

        public override void Create()
        {
            m_PointData = DataFactory.CreatePointData();
            MyShapeDatas.Add(m_PointData);
        }
        
        
    }
}
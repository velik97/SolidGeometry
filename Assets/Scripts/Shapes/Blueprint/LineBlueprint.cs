using Shapes.Data;

namespace Shapes.Blueprint
{
    public class LineBlueprint : ShapeBlueprint
    {
        public readonly LineData LineData;
        
        public override ShapeData MainShapeData => LineData;
        
        public LineBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            LineData = dataFactory.CreateLineData();
            MyShapeDatas.Add(LineData);

            LineData.NameUpdated += OnNameUpdated;
            DataFactory.ShapesListUpdated += OnNameUpdated;
        }
    }
}
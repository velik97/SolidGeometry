using System;
using Shapes.Data;

namespace Shapes.Blueprint
{
    [Serializable]
    public class PolygonBlueprint : ShapeBlueprint
    {
        public readonly PolygonData PolygonData;
        
        public override ShapeData MainShapeData => PolygonData;
        
        public PolygonBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            PolygonData = dataFactory.CreatePolygonData();
            PolygonData.SourceBlueprint = this;
            MyShapeDatas.Add(PolygonData);

            PolygonData.NameUpdated += OnNameUpdated;
            DataFactory.ShapesListUpdated += OnNameUpdated;
        }
    }
}
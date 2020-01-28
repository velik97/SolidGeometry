using System;
using Shapes.Data;

namespace Shapes.Blueprint.BaseShapes
{
    [Serializable]
    public class PointBlueprint : ShapeBlueprint
    {
        public readonly PointData PointData;

        public override ShapeData MainShapeData => PointData;

        public PointBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            PointData = DataFactory.CreatePointData();
            PointData.SourceBlueprint = this;
            MyShapeDatas.Add(PointData);

            PointData.NameUpdated += OnNameUpdated;
        }
    }
}
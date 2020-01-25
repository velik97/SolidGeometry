using Shapes.Data;
using Shapes.Validators.Line;
using Shapes.Validators.Point;
using Shapes.Validators.Polygon;

namespace Shapes.Validators.Uniqueness
{
    public class ShapeDatasUniquenessValidators
    {
        private readonly UniquenessValidatorsSolver<PointNameUniquenessValidator> m_PointsByNameUniquenessValidatorsSolver =
            new UniquenessValidatorsSolver<PointNameUniquenessValidator>(256);
        private readonly UniquenessValidatorsSolver<PointPositionUniquenessValidator> m_PointsByPositionUniquenessValidatorsSolver =
            new UniquenessValidatorsSolver<PointPositionUniquenessValidator>(256);

        private readonly UniquenessValidatorsSolver<LineUniquenessValidator> m_LinesUniquenessValidatorsSolver =
            new UniquenessValidatorsSolver<LineUniquenessValidator>(256);

        private readonly UniquenessValidatorsSolver<PolygonUniquenessValidator> m_PolygonUniquenessValidatorsSolver =
            new UniquenessValidatorsSolver<PolygonUniquenessValidator>(256);

        public void AddShapeData(ShapeData shapeData)
        {
            switch (shapeData)
            {
                case PointData pointData:
                    m_PointsByNameUniquenessValidatorsSolver.AddValidatable(pointData.NameUniquenessValidator);
                    m_PointsByPositionUniquenessValidatorsSolver.AddValidatable(pointData.PositionUniquenessValidator);
                    break;
                case LineData lineData:
                    m_LinesUniquenessValidatorsSolver.AddValidatable(lineData.UniquenessValidator);
                    break;
                case PolygonData polygonData:
                    m_PolygonUniquenessValidatorsSolver.AddValidatable(polygonData.PolygonUniquenessValidator);
                    break;
                case CompositeShapeData compositeShapeData:
                    break;
            }
        }

        public void RemoveShapeData(ShapeData shapeData)
        {
            switch (shapeData)
            {
                case PointData pointData:
                    m_PointsByNameUniquenessValidatorsSolver.RemoveValidatable(pointData.NameUniquenessValidator);
                    m_PointsByPositionUniquenessValidatorsSolver.RemoveValidatable(pointData.PositionUniquenessValidator);
                    break;
                case LineData lineData:
                    m_LinesUniquenessValidatorsSolver.RemoveValidatable(lineData.UniquenessValidator);
                    break;
                case PolygonData polygonData:
                    m_PolygonUniquenessValidatorsSolver.RemoveValidatable(polygonData.PolygonUniquenessValidator);
                    break;
                case CompositeShapeData compositeShapeData:
                    break;
            }
        }
    }
}
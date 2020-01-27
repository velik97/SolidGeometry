using System;
using Shapes.Data;
using UnityEngine.UIElements;

namespace Shapes.Blueprint
{
    public class ShapeBlueprintFactory
    {
        private readonly ShapeDataFactory m_ShapeDataFactory;

        public ShapeBlueprintFactory(ShapeDataFactory shapeDataFactory)
        {
            m_ShapeDataFactory = shapeDataFactory;
        }

        public ShapeBlueprint CreateShapeBlueprint(ShapeBlueprintType type)
        {
            switch (type)
            {
                case ShapeBlueprintType.Point:
                    return new PointBlueprint(m_ShapeDataFactory);
                case ShapeBlueprintType.Line:
                    return new LineBlueprint(m_ShapeDataFactory);
                case ShapeBlueprintType.Polygon:
                    return new PolygonBlueprint(m_ShapeDataFactory);
                case ShapeBlueprintType.Parallelepiped:
                    return new ParallelepipedBlueprint(m_ShapeDataFactory);
            }

            return null;
        }

        public enum ShapeBlueprintType
        {
            Point,
            Line,
            Polygon,
            Parallelepiped
        }
    }
}
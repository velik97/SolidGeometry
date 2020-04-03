using System.Collections.Generic;
using Lesson.Shapes.Blueprints.BaseShapes;
using Lesson.Shapes.Blueprints.CompositeShapes;
using Lesson.Shapes.Blueprints.DependentShapes;
using Lesson.Shapes.Datas;
using Newtonsoft.Json;

namespace Lesson.Shapes.Blueprints
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class ShapeBlueprintFactory
    {
        private ShapeDataFactory m_ShapeDataFactory;
        
        [JsonProperty]
        private readonly List<ShapeBlueprint> m_ShapeBlueprints;

        public IReadOnlyList<ShapeBlueprint> ShapeBlueprints => m_ShapeBlueprints;

        public ShapeDataFactory ShapeDataFactory => m_ShapeDataFactory;

        [JsonConstructor]
        public ShapeBlueprintFactory(object _)
        { }

        public ShapeBlueprintFactory(ShapeDataFactory shapeDataFactory)
        {
            m_ShapeDataFactory = shapeDataFactory;
            m_ShapeBlueprints = new List<ShapeBlueprint>();
        }

        public void SetShapeDataFactory(ShapeDataFactory shapeDataFactory)
        {
            m_ShapeDataFactory = shapeDataFactory;
            foreach (ShapeBlueprint shapeBlueprint in m_ShapeBlueprints)
            {
                shapeBlueprint.SetShapeDataFactory(shapeDataFactory);
            }
        }

        public ShapeBlueprint CreateShapeBlueprint(ShapeBlueprintType type)
        {
            ShapeBlueprint blueprint = null;
            switch (type)
            {
                case ShapeBlueprintType.Point:
                    blueprint = new PointBlueprint(m_ShapeDataFactory);
                    break;
                case ShapeBlueprintType.Line:
                    blueprint = new LineBlueprint(m_ShapeDataFactory);
                    break;
                case ShapeBlueprintType.Polygon:
                    blueprint = new PolygonBlueprint(m_ShapeDataFactory);
                    break;
                case ShapeBlueprintType.Parallelepiped:
                    blueprint = new ParallelepipedBlueprint(m_ShapeDataFactory);
                    break;
                case ShapeBlueprintType.Cube:
                    blueprint = new CubeBlueprint(m_ShapeDataFactory);
                    break;
                case ShapeBlueprintType.Pyramid:
                    blueprint = new PyramidBlueprint(m_ShapeDataFactory);
                    break;
                case ShapeBlueprintType.RegularPyramid:
                    blueprint = new RegularPyramidBlueprint(m_ShapeDataFactory);
                    break;
                case ShapeBlueprintType.RegularPrism:
                    blueprint = new RegularPrismBlueprint(m_ShapeDataFactory);
                    break;
                case ShapeBlueprintType.PointPerpendicularProjection:
                    blueprint = new PointPerpendicularProjectionBlueprint(m_ShapeDataFactory);
                    break;
                case ShapeBlueprintType.PointOfIntersection:
                    blueprint = new PointOfIntersectionBlueprint(m_ShapeDataFactory);
                    break;
                case ShapeBlueprintType.PointIn2DSubspace:
                    blueprint = new PointIn2DSubspaceBlueprint(m_ShapeDataFactory);
                    break;
                case ShapeBlueprintType.PointProjectionContinuationOfPoint:
                    blueprint = new PointProjectionFromPointBlueprint(m_ShapeDataFactory);
                    break;
                case ShapeBlueprintType.PointProjectionAlongLine:
                    blueprint = new PointProjectionAlongLineBlueprint(m_ShapeDataFactory);
                    break;
                case ShapeBlueprintType.PointIn1DSubspace:
                    blueprint = new PointIn1DSubspaceBlueprint(m_ShapeDataFactory);
                    break;
            }

            if (blueprint != null)
            {
                m_ShapeBlueprints.Add(blueprint);
            }

            return blueprint;
        }
        
        public void Remove(ShapeBlueprint blueprint)
        {
            blueprint.Destroy();
            m_ShapeBlueprints.Remove(blueprint);
        }

        public void Clear()
        {
            m_ShapeBlueprints.Clear();
        }

        public enum ShapeBlueprintType
        {
            Point,
            Line,
            Polygon,
            Parallelepiped,
            Cube, 
            Pyramid,
            RegularPyramid,
            PointOfIntersection,
            PointIn1DSubspace,
            PointIn2DSubspace,
            PointProjectionContinuationOfPoint,
            PointProjectionAlongLine,
            RegularPrism,
            PointPerpendicularProjection,
        }
    }
}
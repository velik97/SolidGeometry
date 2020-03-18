using System.Collections.Generic;
using Lesson.Shapes.Blueprints.CompositeShapes;
using Lesson.Shapes.Data;
using Newtonsoft.Json;
using Shapes.Blueprint;
using Shapes.Blueprint.BaseShapes;
using Shapes.Blueprint.CompositeShapes;
using Shapes.Blueprint.DependentShapes;

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
                case ShapeBlueprintType.RegularNPolygon:
                    blueprint = new RegularNPolygonBlueprint(m_ShapeDataFactory);
                    break;
                case ShapeBlueprintType.PointProjectionOnLine:
                    blueprint = new PointProjectionOnLineBlueprint(m_ShapeDataFactory);
                    break;
                case ShapeBlueprintType.PointOfIntersection:
                    blueprint = new PointOfIntersectionBlueprint(m_ShapeDataFactory);
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
            RegularNPolygon,
            PointProjectionOnLine,
            PointOfIntersection
        }
    }
}
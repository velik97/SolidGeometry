using System.Collections.Generic;
using Shapes.Blueprint;
using Shapes.Data;
using UnityEngine;

namespace Shapes.Lesson
{
    [CreateAssetMenu]
    public class LessonBlueprint : ScriptableObject
    {
        public readonly List<ShapeBlueprint> ShapeBlueprints = new List<ShapeBlueprint>();
        public readonly ShapeDataFactory ShapeDataFactory = new ShapeDataFactory();
        private ShapeBlueprintFactory m_ShapeBlueprintFactory;

        private ShapeBlueprintFactory ShapeBlueprintFactory =>
            m_ShapeBlueprintFactory ?? (m_ShapeBlueprintFactory = new ShapeBlueprintFactory(ShapeDataFactory));

        public ShapeBlueprint AddBlueprint(ShapeBlueprintFactory.ShapeBlueprintType blueprintType)
        {
            ShapeBlueprint shapeBlueprint = ShapeBlueprintFactory.CreateShapeBlueprint(blueprintType);
            ShapeBlueprints.Add(shapeBlueprint);
            return shapeBlueprint;
        }

        public void DeleteBlueprint(ShapeBlueprint blueprint)
        {
            blueprint.Destroy();
            ShapeBlueprints.Remove(blueprint);
        }
    }
}
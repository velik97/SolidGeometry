using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Shapes.Blueprint;
using Shapes.Data;

namespace Lesson.Shapes.Blueprints
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CanDependOnShapeBlueprint
    {
        [JsonProperty]
        protected readonly List<ShapeBlueprint> DependencesOnOtherShapes = new List<ShapeBlueprint>();

        protected CanDependOnShapeBlueprint()
        { }

        protected void RestoreDependences()
        {
            foreach (ShapeBlueprint shapeBlueprint in DependencesOnOtherShapes)
            {
                shapeBlueprint.AddDependence(this);
            }
        }

        public void CreateDependenceOn(ShapeData shapeData)
        {
            DependencesOnOtherShapes.Add(shapeData.SourceBlueprint);
            shapeData.SourceBlueprint.AddDependence(this);
        }

        public void RemoveDependenceOn(ShapeData shapeData)
        {
            DependencesOnOtherShapes.Remove(shapeData.SourceBlueprint);
            shapeData.SourceBlueprint.RemoveDependence(this);
        }

        public virtual void Destroy()
        {
            foreach (ShapeBlueprint d in DependencesOnOtherShapes)
            {
                d.RemoveDependence(this);
            }
            DependencesOnOtherShapes.Clear();
        }
        
        public static bool CanCreateDependence(CanDependOnShapeBlueprint dependentBlueprint, ShapeData dependsOnData)
        {
            return !HasDependenceFromTo(dependsOnData.SourceBlueprint, dependentBlueprint, null);
        }

        private static bool HasDependenceFromTo(ShapeBlueprint fromBlueprint, CanDependOnShapeBlueprint toBlueprint,
            HashSet<ShapeBlueprint> visited)
        {
            if (fromBlueprint == toBlueprint)
            {
                return true;
            }

            if (visited == null)
            {
                visited = new HashSet<ShapeBlueprint>();
            }

            foreach (ShapeBlueprint blueprint in fromBlueprint.DependencesOnOtherShapes)
            {
                if (visited.Contains(blueprint))
                {
                    continue;
                }
                visited.Add(blueprint);

                if (blueprint == toBlueprint)
                {
                    return true;
                }

                return HasDependenceFromTo(blueprint, toBlueprint, visited);
            }

            return false;
        }
    }
}
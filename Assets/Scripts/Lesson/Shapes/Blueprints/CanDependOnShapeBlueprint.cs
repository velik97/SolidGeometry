using System.Collections.Generic;
using Lesson.Shapes.Datas;
using Newtonsoft.Json;

namespace Lesson.Shapes.Blueprints
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CanDependOnShapeBlueprint
    {
        [JsonProperty]
        protected readonly List<ShapeBlueprint> DependenciesOnOtherShapes = new List<ShapeBlueprint>();

        protected CanDependOnShapeBlueprint()
        { }

        protected void RestoreDependencies()
        {
            foreach (ShapeBlueprint shapeBlueprint in DependenciesOnOtherShapes)
            {
                shapeBlueprint.AddDependence(this);
            }
        }

        public void CreateDependenceOn(ShapeData shapeData)
        {
            DependenciesOnOtherShapes.Add(shapeData.SourceBlueprint);
            shapeData.SourceBlueprint.AddDependence(this);
        }

        public void RemoveDependenceOn(ShapeData shapeData)
        {
            DependenciesOnOtherShapes.Remove(shapeData.SourceBlueprint);
            shapeData?.SourceBlueprint.RemoveDependence(this);
        }

        public virtual void Destroy()
        {
            foreach (ShapeBlueprint d in DependenciesOnOtherShapes)
            {
                d.RemoveDependence(this);
            }
            DependenciesOnOtherShapes.Clear();
        }
        
        public static bool CanCreateDependence(CanDependOnShapeBlueprint @from, ShapeData to)
        {
            return !HasDependence(to.SourceBlueprint, @from, null);
        }

        private static bool HasDependence(ShapeBlueprint @from, CanDependOnShapeBlueprint to,
            HashSet<ShapeBlueprint> visited)
        {
            if (@from == to)
            {
                return true;
            }

            if (visited == null)
            {
                visited = new HashSet<ShapeBlueprint>();
            }

            foreach (ShapeBlueprint blueprint in @from.DependenciesOnOtherShapes)
            {
                if (visited.Contains(blueprint))
                {
                    continue;
                }
                visited.Add(blueprint);

                if (blueprint == to)
                {
                    return true;
                }

                return HasDependence(blueprint, to, visited);
            }

            return false;
        }
    }
}
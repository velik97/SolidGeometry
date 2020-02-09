using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Shapes.Blueprint;
using Shapes.Data;
using UnityEngine;

namespace Lesson
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FiguresSet
    {
        [JsonProperty(IsReference = true)]
        public readonly ShapeDataFactory ShapeDataFactory;
        [JsonProperty]
        public readonly List<ShapeBlueprint> ShapeBlueprints;
        private ShapeBlueprintFactory m_ShapeBlueprintFactory;

        public FiguresSet()
        {
            ShapeDataFactory = new ShapeDataFactory();
            ShapeBlueprints = new List<ShapeBlueprint>();
            OnDeserialized();
        }

        [JsonConstructor]
        public FiguresSet(object _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (ShapeBlueprint shapeBlueprint in ShapeBlueprints)
            {
                shapeBlueprint.DataFactory = ShapeDataFactory;
            }
            OnDeserialized();
        }

        private void OnDeserialized()
        {
            m_ShapeBlueprintFactory = new ShapeBlueprintFactory(ShapeDataFactory);
            var visualizer = Object.FindObjectOfType<FigureSetVisualizer>();
            if (visualizer)
            {
                visualizer.SetShapeDataFactory(ShapeDataFactory);
            }
        }
        
        public void Clear()
        {
            ShapeDataFactory.Clear();
        }

        public ShapeBlueprint AddBlueprint(ShapeBlueprintFactory.ShapeBlueprintType blueprintType)
        {
            ShapeBlueprint shapeBlueprint = m_ShapeBlueprintFactory.CreateShapeBlueprint(blueprintType);
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
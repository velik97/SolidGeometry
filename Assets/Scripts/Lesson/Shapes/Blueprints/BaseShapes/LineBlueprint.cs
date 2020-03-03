using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Data;
using Newtonsoft.Json;
using Shapes.Data;

namespace Shapes.Blueprint.BaseShapes
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class LineBlueprint : ShapeBlueprint
    {
        [JsonProperty]
        public readonly LineData LineData;
        
        public override ShapeData MainShapeData => LineData;
        
        public LineBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            LineData = dataFactory.CreateLineData();
            OnDeserialized();
        }
        
        [JsonConstructor]
        public LineBlueprint(object _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            RestoreDependences();
            OnDeserialized();
        }

        private void OnDeserialized()
        {
            LineData.SourceBlueprint = this;
            MyShapeDatas.Add(LineData);

            LineData.NameUpdated += OnNameUpdated;
        }
    }
}
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Datas;
using Newtonsoft.Json;
using Serialization;

namespace Lesson.Shapes.Blueprints.BaseShapes
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
        public LineBlueprint(JsonConstructorMark _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            RestoreDependencies();
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
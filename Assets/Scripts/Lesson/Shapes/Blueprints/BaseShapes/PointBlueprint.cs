using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Data;
using Newtonsoft.Json;
using Shapes.Data;

namespace Shapes.Blueprint.BaseShapes
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class PointBlueprint : ShapeBlueprint
    {
        [JsonProperty]
        public readonly PointData PointData;

        public override ShapeData MainShapeData => PointData;

        public PointBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            PointData = DataFactory.CreatePointData();
            OnDeserialized();
        }
        
        [JsonConstructor]
        public PointBlueprint(object _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            RestoreDependences();
            OnDeserialized();
        }

        private void OnDeserialized()
        {
            PointData.SourceBlueprint = this;
            MyShapeDatas.Add(PointData);

            PointData.NameUpdated += OnNameUpdated;
        }
    }
}
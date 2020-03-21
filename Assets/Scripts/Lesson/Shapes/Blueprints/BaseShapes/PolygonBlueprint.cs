using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Datas;
using Newtonsoft.Json;
using Serialization;

namespace Lesson.Shapes.Blueprints.BaseShapes
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class PolygonBlueprint : ShapeBlueprint
    {
        [JsonProperty]
        public readonly PolygonData PolygonData;
        
        public override ShapeData MainShapeData => PolygonData;
        
        public PolygonBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            PolygonData = dataFactory.CreatePolygonData();
            OnDeserialized();
        }
        
        [JsonConstructor]
        public PolygonBlueprint(JsonConstructorMark _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            RestoreDependencies();
            OnDeserialized();
        }

        private void OnDeserialized()
        {
            PolygonData.SourceBlueprint = this;
            MyShapeDatas.Add(PolygonData);

            PolygonData.NameUpdated += OnNameUpdated;
        }
    }
}
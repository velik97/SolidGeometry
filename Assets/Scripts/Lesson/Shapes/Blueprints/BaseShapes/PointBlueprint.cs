using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Datas;
using Newtonsoft.Json;
using Serialization;

namespace Lesson.Shapes.Blueprints.BaseShapes
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class PointBlueprint : ShapeBlueprint
    {
        [JsonProperty]
        public readonly PointData PointData;

        public override ShapeData MainShapeData => PointData;

        public PointBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            PointData = ShapeDataFactory.CreatePointData();
            OnDeserialized();
        }
        
        [JsonConstructor]
        public PointBlueprint(JsonConstructorMark _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            RestoreDependencies();
            OnDeserialized();
        }

        private void OnDeserialized()
        {
            AddToMyShapeDatas(PointData);

            PointData.NameUpdated.Subscribe(NameUpdated);
        }
    }
}
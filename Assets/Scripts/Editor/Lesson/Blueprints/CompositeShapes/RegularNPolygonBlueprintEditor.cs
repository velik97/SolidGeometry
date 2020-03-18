using System;
using Editor.VisualElementsExtensions;
using Lesson.Shapes.Blueprints.CompositeShapes;
using Shapes.Blueprint.CompositeShapes;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Util;

namespace Editor.Lesson.Blueprints.CompositeShapes
{
    public class RegularNPolygonBlueprintEditor: ShapeBlueprintEditor<RegularNPolygonBlueprint>
    {
        
        private Button m_CreateButton;

        public RegularNPolygonBlueprintEditor(RegularNPolygonBlueprint blueprint, Action<Shapes.Blueprint.ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
        {
        }
        
        protected override void SetBaseVisualElement(VisualElement visualElement)
        {
                IntegerField nField = new IntegerField("N: ") {value = Blueprint.N};
                nField.RegisterCallback<ChangeEvent<int>>(evt => Blueprint.SetN(evt.newValue));
                visualElement.Add(nField);
                
                Vector3Field originField = new Vector3Field("Origin: ") {value = Blueprint.Origin};
                originField.RegisterCallback<ChangeEvent<Vector3>>(evt => Blueprint.SetOrigin(evt.newValue));
                visualElement.Add(originField);

                FloatField radiusField = new FloatField("Radius: ") {value = Blueprint.Radius};
                radiusField.RegisterCallback<ChangeEvent<float>>(evt => Blueprint.SetRadius(evt.newValue));
                visualElement.Add(radiusField);

                FloatField heightField = new FloatField("Height: ") {value = Blueprint.Height};
                heightField.RegisterCallback<ChangeEvent<float>>(evt => Blueprint.SetHeight(evt.newValue));
                visualElement.Add(heightField);



                // visualElement.Add(new ValidatorField(Blueprint.NonZeroVolumeValidator));

                for (int i = 0; i < Blueprint.Points.Count; i++)
                {
                    int pointNum = i;
                    TextField nameField = new TextField((pointNum + 1).GetOrdinalForm() + " Point name: ")
                        {value = Blueprint.Points[i].PointName};
                    nameField.RegisterCallback<ChangeEvent<string>>(evt =>
                        Blueprint.SetPointName(pointNum, evt.newValue));
                    visualElement.Add(nameField);
                    visualElement.Add(new ValidatorField(Blueprint.Points[i].NameNotEmptyValidator));
                    //visualElement.Add(new ValidatorField(Blueprint.Points[i].NameUniquenessValidator));
                    visualElement.Add(new ValidatorField(Blueprint.Points[i].PositionUniquenessValidator));
                }
            
        }
    }
}
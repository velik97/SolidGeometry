using System;
using Editor.VisualElementsExtensions;
using Shapes.Blueprint;
using Shapes.Blueprint.Figures;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Util;

namespace Editor.Blueprints.Figures
{
    public class CubeBlueprintEditor: ShapeBlueprintEditor<CubeBlueprint>
    {
        public CubeBlueprintEditor(CubeBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
        {
        }
        
        protected override void SetBaseVisualElement(VisualElement visualElement)
        {
            Vector3Field originField = new Vector3Field("Origin: ") {value = Blueprint.Origin};
            originField.RegisterCallback<ChangeEvent<Vector3>>(evt => Blueprint.SetOrigin(evt.newValue));
            visualElement.Add(originField);

            FloatField lengthField = new FloatField("Length: ") {value = Blueprint.Length};
            lengthField.RegisterCallback<ChangeEvent<int>>(evt => Blueprint.SetLength(evt.newValue));
            visualElement.Add(lengthField);
            
            visualElement.Add(new ValidatorField(Blueprint.NonZeroVolumeValidator));
            
            for (int i = 0; i < Blueprint.Points.Count; i++)
            {
                int pointNum = i;
                VisualElement nameField = new TextField((pointNum + 1).GetOrdinalForm() + " Point name: ")
                    {value = Blueprint.Points[i].PointName};
                nameField.RegisterCallback<ChangeEvent<string>>(evt => Blueprint.SetPointName(pointNum, evt.newValue));
                visualElement.Add(nameField);
                visualElement.Add(new ValidatorField(Blueprint.Points[i].NameNotEmptyValidator));
                visualElement.Add(new ValidatorField(Blueprint.Points[i].NameUniquenessValidator));
                visualElement.Add(new ValidatorField(Blueprint.Points[i].PositionUniquenessValidator));
            }
        }
    }
}
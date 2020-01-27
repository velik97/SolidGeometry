using System;
using Editor.VisualElementsExtensions;
using Shapes.Blueprint;
using Shapes.Data;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Util;

namespace Editor.Blueprints
{
    public class ParallelepipedBlueprintEditor : ShapeBlueprintEditor<ParallelepipedBlueprint>
    {
        public ParallelepipedBlueprintEditor(ParallelepipedBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
        {
        }

        protected override void SetBaseVisualElement(VisualElement visualElement)
        {
            Vector3Field originField = new Vector3Field("Origin: ") {value = Blueprint.Origin};
            originField.RegisterCallback<ChangeEvent<Vector3>>(evt => Blueprint.SetOrigin(evt.newValue));
            visualElement.Add(originField);

            for (int i = 0; i < Blueprint.Axes.Count; i++)
            {
                int axisNum = i;
                Vector3Field axisField = new Vector3Field((axisNum + 1).GetOrdinalForm() + " Axis: ")
                    {value = Blueprint.Axes[i]};
                axisField.RegisterCallback<ChangeEvent<Vector3>>(evt => Blueprint.SetAxis(axisNum, evt.newValue));
                visualElement.Add(axisField);
            }
            
            foreach (PointData point in Blueprint.Points)
            {
                visualElement.Add(new ValidatorField(point.PositionUniquenessValidator));
            }
            
            for (int i = 0; i < Blueprint.Points.Count; i++)
            {
                int pointNum = i;
                VisualElement nameField = new TextField((pointNum + 1).GetOrdinalForm() + " Point name: ")
                    {value = Blueprint.Points[i].PointName};
                nameField.RegisterCallback<ChangeEvent<string>>(evt => Blueprint.SetPointName(pointNum, evt.newValue));
                visualElement.Add(nameField);
                visualElement.Add(new ValidatorField(Blueprint.Points[i].NameNotEmptyValidator));
                visualElement.Add(new ValidatorField(Blueprint.Points[i].NameUniquenessValidator));
            }
        }
    }
}
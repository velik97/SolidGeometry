using System;
using Editor.VisualElementsExtensions;
using Shapes.Blueprint;
using Shapes.Blueprint.DependentShapes;
using UnityEngine.UIElements;

namespace Editor.Blueprints
{
    public class PointOfIntersectionBlueprintEditor : ShapeBlueprintEditor<PointOfIntersectionBlueprint>
    {
        public PointOfIntersectionBlueprintEditor(PointOfIntersectionBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
        {
        }

        protected override void SetBaseVisualElement(VisualElement visualElement)
        {
            VisualElement nameField = new TextField("Name") {value = Blueprint.PointData.PointName};
            VisualElement isAccessoryField = new Toggle("Is Accessory") {value = Blueprint.PointData.IsAccessoryPoint};
            
            nameField.RegisterCallback<ChangeEvent<string>>(evt => Blueprint.PointData.SetName(evt.newValue));
            isAccessoryField.RegisterCallback<ChangeEvent<bool>>(evt => Blueprint.PointData.SetIsAccessory(evt.newValue));
            
            visualElement.Add(nameField);
            visualElement.Add(new ValidatorField(Blueprint.PointData.NameNotEmptyValidator));
            visualElement.Add(new ValidatorField(Blueprint.PointData.NameUniquenessValidator));
            visualElement.Add(new ValidatorField(Blueprint.PointData.PositionUniquenessValidator));
            visualElement.Add(isAccessoryField);

            int lineNum;
            int pointNum;
            void AddPointEditor()
            {
                int lineNumCopy = lineNum;
                int pointNumCopy = pointNum;
                visualElement.Add(new ChoosePointField(
                    Blueprint,
                    $"Line {lineNumCopy + 1}, Point {pointNumCopy + 1}",
                    () => Blueprint.PointsOnLines[lineNumCopy][pointNumCopy],
                    pointData => Blueprint.SetPoint(lineNumCopy, pointNumCopy, pointData)));
            }
            lineNum = 0;
            pointNum = 0;
            AddPointEditor();
            lineNum = 0;
            pointNum = 1;
            AddPointEditor();
            lineNum = 1;
            pointNum = 0;
            AddPointEditor();
            lineNum = 1;
            pointNum = 1;
            AddPointEditor();
            visualElement.Add(new ValidatorField(Blueprint.PointsNotSameValidator));
            visualElement.Add(new ValidatorField(Blueprint.LinesIntersectValidator));
        }
    }
}
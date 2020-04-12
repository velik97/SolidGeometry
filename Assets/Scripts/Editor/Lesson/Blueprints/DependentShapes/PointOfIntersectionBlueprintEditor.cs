using System;
using Editor.VisualElementsExtensions;
using Lesson.Shapes.Blueprints;
using Lesson.Shapes.Blueprints.DependentShapes;
using Lesson.Shapes.Datas;
using UnityEngine.UIElements;
using Util;

namespace Editor.Lesson.Blueprints.DependentShapes
{
    public class PointOfIntersectionBlueprintEditor : ShapeBlueprintEditor<PointOfIntersectionBlueprint>
    {
        public PointOfIntersectionBlueprintEditor(PointOfIntersectionBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
        {
        }

        protected override void SetBaseVisualElement(VisualElement visualElement)
        {
            VisualElement nameField = new TextField("Name") {value = Blueprint.PointData.PointName};
            
            nameField.RegisterCallback<ChangeEvent<string>>(evt => Blueprint.PointData.SetName(evt.newValue));
            
            visualElement.Add(nameField);
            visualElement.Add(new ValidatorField(Blueprint.PointData.NameNotEmptyValidator));
            visualElement.Add(new ValidatorField(Blueprint.PointData.NameUniquenessValidator));
            visualElement.Add(new ValidatorField(Blueprint.PointData.PositionUniquenessValidator));

            int lineNum;
            int pointNum;
            void AddPointEditor()
            {
                int lineNumCopy = lineNum;
                int pointNumCopy = pointNum;
                visualElement.Add(new ChoseShapeDataField<PointData>(
                    Blueprint.ShapeDataFactory,
                    Blueprint,
                    $"{(pointNumCopy + 1).GetOrdinalForm()} Point: ",
                    () => Blueprint.PointsOnLines[lineNumCopy][pointNumCopy],
                    pointData => Blueprint.SetPoint(lineNumCopy, pointNumCopy, pointData)));
            }
            
            // 1-st line
            Label line1Label = new Label("1-st Line");
            line1Label.AddToClassList("sub-header");
            visualElement.Add(line1Label);
            
            lineNum = 0;
            pointNum = 0;
            AddPointEditor();
            lineNum = 0;
            pointNum = 1;
            AddPointEditor();
            
            // 2-nd line
            Label line2Label = new Label("2-nd Line");
            line2Label.AddToClassList("sub-header");
            visualElement.Add(line2Label);
            
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
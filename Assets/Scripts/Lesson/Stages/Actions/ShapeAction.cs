using System;
using Shapes.Data;

namespace Lesson.Stages.Actions
{
    public abstract class ShapeAction
    {
        public event Action NameUpdated;

        protected ShapeData ShapeData;

        public abstract void PreservePreviousState();

        public abstract void ApplyAction();

        public abstract void RollbackAction();

        public virtual void SetShapeData(ShapeData shapeData)
        {
            if (ShapeData == shapeData)
            {
                return;
            }
            ShapeData = shapeData;
            OnNameUpdated();
        }

        public virtual bool HasConflictWith(ShapeAction other)
        {
            return other.ShapeData == this.ShapeData &&
                   other.GetType() == this.GetType();
        }

        protected void OnNameUpdated()
        {
            NameUpdated?.Invoke();
        }
    }
}
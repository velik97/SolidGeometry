using Shapes.Data;

namespace Stages.Actions
{
    public abstract class ShapeAction
    {
        protected ShapeData ShapeData;

        public abstract void PreservePreviousState();

        public abstract void ApplyAction();

        public abstract void RollbackAction();

        public virtual bool HasConflictWith(ShapeAction other)
        {
            return other.ShapeData == this.ShapeData &&
                   other.GetType() == this.GetType();
        }
    }
}
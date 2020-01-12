using Shapes.Blueprint;
using Shapes.View;

namespace Shapes.Data
{
    public abstract class ShapeData
    {
        public ShapeBlueprint SourceBlueprint;
        
        public IShapeView View { get; private set; }

        public void AttachView(IShapeView view)
        {
            View = view;
        }
    }
}
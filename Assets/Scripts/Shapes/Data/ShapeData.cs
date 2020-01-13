using Shapes.Blueprint;
using Shapes.View;

namespace Shapes.Data
{
    public abstract class ShapeData
    {
        public IShapeView View { get; private set; }
        
        public ShapeBlueprint SourceBlueprint;

        public void AttachView(IShapeView view)
        {
            View = view;
        }
    }
}
using Lesson.Shapes.Datas;

namespace Lesson.Shapes.Views
{
    public interface IShapeViewFactory
    {
        IShapeView RequestShapeView(ShapeData data);

        void ReleaseView(IShapeView view);

        void Clear();
    }
}
using System;
using Lesson.Shapes.Datas;

namespace Lesson.Shapes.Views
{
    public interface IShapeViewFactory : IDisposable
    {
        IShapeView RequestShapeView(ShapeData data);

        void ReleaseView(IShapeView view);
    }
}
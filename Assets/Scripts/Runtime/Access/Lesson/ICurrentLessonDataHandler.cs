using Lesson;
using Util.EventBusSystem;

namespace Runtime.Access.Lesson
{
    public interface ICurrentLessonDataChangedHandler : IGlobalSubscriber
    {
        void HandleCurrentLessonDataChanged(LessonData lessonData);
    }
}
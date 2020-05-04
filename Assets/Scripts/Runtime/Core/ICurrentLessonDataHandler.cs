using Lesson;
using Util.EventBusSystem;

namespace Runtime.Core
{
    public interface ICurrentLessonDataChangedHandler : IGlobalSubscriber
    {
        void HandleCurrentLessonDataChanged(LessonData lessonData);
    }
}
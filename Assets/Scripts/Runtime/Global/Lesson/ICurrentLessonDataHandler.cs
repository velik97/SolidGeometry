using Lesson;
using Util.EventBusSystem;

namespace Runtime.Global.LessonManagement
{
    public interface ICurrentLessonDataChangedHandler : IGlobalSubscriber
    {
        void HandleCurrentLessonDataChanged(LessonData lessonData);
    }
}
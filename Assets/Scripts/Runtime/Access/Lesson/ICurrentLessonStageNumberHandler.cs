using Util.EventBusSystem;

namespace Runtime.Access.Lesson
{
    public interface ICurrentLessonStageNumberHandler : IGlobalSubscriber
    {
        void HandleLessonStageNumberChanged(int number);
    }
}
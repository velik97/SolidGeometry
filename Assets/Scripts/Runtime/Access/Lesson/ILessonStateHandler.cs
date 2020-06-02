using Util.EventBusSystem;

namespace Runtime.Access.Lesson
{
    public interface ILessonStateHandler : IGlobalSubscriber
    {
        void HandlerLessonStateChanged(LessonState state);
    }
}
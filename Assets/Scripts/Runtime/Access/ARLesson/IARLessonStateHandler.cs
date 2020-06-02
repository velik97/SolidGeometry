using Util.EventBusSystem;

namespace Runtime.Access.ARLesson
{
    public interface IARLessonStateHandler : IGlobalSubscriber
    {
        void HandleARLessonStateChanged(ARLessonState state);
    }
}
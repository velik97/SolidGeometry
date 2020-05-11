using Util.EventBusSystem;

namespace Runtime.Global.LessonManagement
{
    public interface ICurrentLessonStageNumberHandler : IGlobalSubscriber
    {
        void HandleLessonStageNumberChanged(int number);
    }
}
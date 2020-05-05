using Util.EventBusSystem;

namespace Runtime.Core
{
    public interface ICurrentLessonStageNumberHandler : IGlobalSubscriber
    {
        void HandleLessonStageNumberChanged(int number);
    }
}
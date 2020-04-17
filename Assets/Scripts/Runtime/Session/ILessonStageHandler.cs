using Util.EventBusSystem;

namespace Runtime.Session
{
    public interface ILessonStageHandler : IGlobalSubscriber
    {
        void HandleGoToStage(int stageNumber);
    }
}
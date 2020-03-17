using Util.EventBusSystem;

namespace Session
{
    public interface ILessonStageHandler : IGlobalSubscriber
    {
        void HandleGoToStage(int stageNumber);
    }
}
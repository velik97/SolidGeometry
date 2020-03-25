using Lesson.Stages;
using UI.MVVM;

namespace UI.Session.LessonScrollBrowser
{
    public class LessonStageDescriptionVM : ViewModel
    {
        public int Number;
        public string Name;
        public string Description;

        public LessonStageDescriptionVM(LessonStage stage)
        {
            Number = stage.StageNum;
            Name = stage.StageName;
            Description = stage.StageDescription;
        }
    }
}
namespace UI.Lesson.ARRequirementsManual
{
    public interface IManualPageView
    { 
        void AppearImmediate();

        void Appear();

        void DisappearImmediate();
        
        void Disappear();

        void BindButtons();

        void UnbindButtons();
    }
}
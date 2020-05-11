namespace UI.Session.ARRequirementsManual
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
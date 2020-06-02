using Runtime.Access.ARLesson;
using UI.MVVM;

namespace UI.Lesson.ARLessonPlacingUI
{
    public class ARLessonPlacingVM : ViewModel
    {
        public void RequestReplace()
        {
            ARLessonAccess.Instance.RequestPlace();
        }

        public void ConfirmReplace()
        {
            ARLessonAccess.Instance.ConfirmReplace();
        }
    }
}
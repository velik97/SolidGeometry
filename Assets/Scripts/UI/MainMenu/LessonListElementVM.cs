using System;
using UI.MVVM;

namespace UI.MainMenu
{
    public class LessonListElementVM : ViewModel
    {
        public readonly string LessonName;
        private readonly Action<string> m_OnLessonChosen;

        public LessonListElementVM(string lessonName, Action<string> onLessonChosen)
        {
            LessonName = lessonName;
            m_OnLessonChosen = onLessonChosen;
        }

        public void ButtonPressed()
        {
            m_OnLessonChosen.Invoke(LessonName);
        }
    }
}
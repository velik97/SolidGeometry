using System;
using System.Collections.Generic;
using System.Linq;
using UI.MVVM;

namespace UI.MainMenu
{
    public class LessonListVM : ViewModel
    {
        private readonly Action<string> m_OnLessonChosen;
        private readonly List<LessonListElementVM> m_ElementVMs;

        public IReadOnlyList<LessonListElementVM> ElementVMs => m_ElementVMs;

        public LessonListVM(IEnumerable<string> lessonNames, Action<string> onLessonChosen)
        {
            m_OnLessonChosen = onLessonChosen;
            
            m_ElementVMs = new List<LessonListElementVM>();
            foreach (string lessonName in lessonNames)
            {
                LessonListElementVM elementVM = new LessonListElementVM(lessonName, onLessonChosen);
                Add(elementVM);
                m_ElementVMs.Add(elementVM);
            }
        }
    }
}
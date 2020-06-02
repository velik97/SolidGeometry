using System;
using UI.MVVM;
using UniRx;

namespace UI.Lesson.ARRequirementsManual
{
    public abstract class ManualPageVM : ViewModel
    {
        public abstract IReadOnlyReactiveProperty<bool> ManualIsCompleted { get; }

        public readonly int PageNumber;
        public readonly int PagesCount;

        private readonly Action m_CloseAction;
        private readonly Action m_GoFurtherAction;

        protected ManualPageVM(int pageNumber, int pagesCount, Action closeAction, Action goFurtherAction)
        {
            PagesCount = pagesCount;
            m_CloseAction = closeAction;
            m_GoFurtherAction = goFurtherAction;
            PageNumber = pageNumber;
            
        }

        public void Close()
        {
            m_CloseAction?.Invoke();
        }

        public virtual void GoFurther()
        {
            if (!ManualIsCompleted.Value)
            {
                return;
            }
            m_GoFurtherAction?.Invoke();
        }
    }
}
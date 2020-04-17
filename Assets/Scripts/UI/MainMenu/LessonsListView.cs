using UI.MVVM;
using UnityEngine;

namespace UI.MainMenu
{
    public class LessonsListView : View<LessonListVM>
    {
        [SerializeField]
        private RectTransform m_Container;
        
        [SerializeField]
        private LessonListElementView m_ElementViewPrefab;

        public override void Bind(LessonListVM viewModel)
        {
            base.Bind(viewModel);

            foreach (LessonListElementVM viewModelElementVM in ViewModel.ElementVMs)
            {
                LessonListElementView elementView = Instantiate(m_ElementViewPrefab, m_Container, false);
                elementView.Bind(viewModelElementVM);
            }
        }
    }
}
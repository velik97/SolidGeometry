using UI.MVVM;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Session.CloseLesson
{
    public class CloseLessonView : View<CloseLessonVM>
    {
        [SerializeField]
        private Button m_CloseButton;

        public override void Bind(CloseLessonVM viewModel)
        {
            base.Bind(viewModel);

            Add(m_CloseButton.OnClickAsObservable().Subscribe(_ => ViewModel.CloseLesson()));
        }
    }
}
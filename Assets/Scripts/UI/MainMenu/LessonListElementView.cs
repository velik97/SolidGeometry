using TMPro;
using UI.MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class LessonListElementView : View<LessonListElementVM>
    {
        [SerializeField]
        private Button m_ChoseLessonButton;

        [SerializeField]
        private TextMeshProUGUI m_LessonNameLabel;

        public override void Bind(LessonListElementVM viewModel)
        {
            base.Bind(viewModel);

            m_LessonNameLabel.text = ViewModel.LessonName;
            Add(m_ChoseLessonButton.OnClickAsObservable().Subscribe(_ => ViewModel.ButtonPressed()));
        }
    }
}
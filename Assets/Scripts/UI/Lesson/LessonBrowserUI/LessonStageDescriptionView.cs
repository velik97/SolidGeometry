using TMPro;
using UI.MVVM;
using UnityEngine;

namespace UI.Lesson.LessonBrowserUI
{
    public class LessonStageDescriptionView : View<LessonStageDescriptionVM>
    {
        [SerializeField] private TextMeshProUGUI m_NumberText;
        [SerializeField] private TextMeshProUGUI m_NameText;
        [SerializeField] private TextMeshProUGUI m_DescriptionText;

        public override void Bind(LessonStageDescriptionVM viewModel)
        {
            base.Bind(viewModel);
            
            SetNumber(viewModel.Number);
            SetName(viewModel.Name);
            SetDescription(viewModel.Description);
        }

        private void SetNumber(int number)
        {
            m_NumberText.text = (number + 1 + ".").ToString();
        }
        
        private void SetName(string nameText)
        {
            m_NameText.text = nameText;
        }
        
        private void SetDescription(string nameText)
        {
            m_DescriptionText.text = nameText;
        }
    }
}
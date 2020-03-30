using DanielLochner.Assets.SimpleScrollSnap;
using UI.MVVM;
using UnityEngine;
using Util;

namespace UI.Session.LessonBrowserUI
{
    public class LessonBrowserView : View<LessonBrowserVM>
    {
        [SerializeField] private SimpleScrollSnap m_Scroll;
        [SerializeField] private LessonStageDescriptionView m_DescriptionPrefab;

        public override void Bind(LessonBrowserVM viewModel)
        {
            base.Bind(viewModel);

            foreach (LessonStageDescriptionVM descriptionVM in viewModel.StagesVMs)
            {
                CreateNewPhaseDescription(descriptionVM);
            }
            
            m_Scroll.onPanelSelected.AddListener(() =>
            {
                viewModel.GoToStage(m_Scroll.TargetPanel);
            });
        }
        
        private void CreateNewPhaseDescription(LessonStageDescriptionVM vm)
        {
            LessonStageDescriptionView newDescription = m_Scroll.Add(m_DescriptionPrefab, vm.Number);
            newDescription.Bind(vm);
        }
    }
}
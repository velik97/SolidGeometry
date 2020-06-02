using DanielLochner.Assets.SimpleScrollSnap;
using Runtime.Access.Lesson;
using UI.MVVM;
using UniRx;
using UnityEngine;
using Util;
using Util.EventBusSystem;

namespace UI.Lesson.LessonBrowserUI
{
    public class LessonBrowserView : View<LessonBrowserVM>, ILessonStateHandler
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
            
            m_Scroll.onPanelSelected.AddListener(GoToStage);
            AddDisposable(Disposable.Create(() => m_Scroll.onPanelSelected.RemoveListener(GoToStage)));
            AddDisposable(EventBus.Subscribe(this));
        }

        public void HandlerLessonStateChanged(LessonState state)
        {
            gameObject.SetActive(state == LessonState.Running);
        }

        private void GoToStage()
        {
            ViewModel.GoToStage(m_Scroll.TargetPanel);
        }
        
        private void CreateNewPhaseDescription(LessonStageDescriptionVM vm)
        {
            LessonStageDescriptionView newDescription = m_Scroll.Add(m_DescriptionPrefab, vm.Number);
            newDescription.Bind(vm);
        }
    }
}
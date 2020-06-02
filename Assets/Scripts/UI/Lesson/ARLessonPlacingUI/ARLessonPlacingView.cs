using System;
using Runtime.Access.ARLesson;
using Runtime.Access.Lesson;
using UI.MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Util.EventBusSystem;

namespace UI.Lesson.ARLessonPlacingUI
{
    public class ARLessonPlacingView : View<ARLessonPlacingVM>, IARLessonStateHandler
    {
        [SerializeField]
        private GameObject m_PlacingHintObject;

        [SerializeField]
        private GameObject m_StartReplaceButtonObject;
        [SerializeField]
        private Button m_StartReplaceButton;
        
        [SerializeField]
        private GameObject m_ConfirmReplaceButtonObject;
        [SerializeField]
        private Button m_ConfirmReplaceButton;

        private void OnEnable()
        {
            if (ViewModel == null)
            {
                gameObject.SetActive(false);
            }
        }

        public override void Bind(ARLessonPlacingVM viewModel)
        {
            base.Bind(viewModel);
            
            AddDisposable(EventBus.Subscribe(this));
            HandleARLessonStateChanged(ARLessonAccess.Instance.ARLessonState);
            
            AddDisposable(m_StartReplaceButton.OnClickAsObservable().Subscribe(_ => ViewModel.RequestReplace()));
            AddDisposable(m_ConfirmReplaceButton.OnClickAsObservable().Subscribe(_ => ViewModel.ConfirmReplace()));

            gameObject.SetActive(true);
        }

        public void HandleARLessonStateChanged(ARLessonState state)
        {
            switch (state)
            {
                case ARLessonState.NotRunning:
                case ARLessonState.ExtractingFeaturePoints:
                    m_PlacingHintObject.SetActive(false);
                    m_StartReplaceButtonObject.SetActive(false);
                    m_ConfirmReplaceButtonObject.SetActive(false);
                    break;
                case ARLessonState.PlacingLesson:
                    m_PlacingHintObject.SetActive(false);
                    m_StartReplaceButtonObject.SetActive(false);
                    
                    m_ConfirmReplaceButtonObject.SetActive(true);
                    break;
                case ARLessonState.Running:
                    m_ConfirmReplaceButtonObject.SetActive(false);
                    
                    m_PlacingHintObject.SetActive(true);
                    m_StartReplaceButtonObject.SetActive(true);
                    break;
            }
        }

        protected override void UnbindViewAction()
        {
            base.UnbindViewAction();
            gameObject.SetActive(false);
        }
    }
}
using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Global.UI;
using TMPro;
using UI.MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Util.UniRxExtensions;

namespace UI.Session.ARRequirementsManual
{
    public abstract class ManualPageView<TViewModel> : View<TViewModel>, IManualPageView where TViewModel : ManualPageVM
    {
        [SerializeField] private RectTransform m_RectTransform;
        
        [SerializeField]
        private Button m_CompletedButton;
        [SerializeField]
        private TextMeshProUGUI m_CompletedButtonLabel;

        [SerializeField]
        private Button m_CloseButton;
        
        [SerializeField]
        private TextMeshProUGUI m_PageLabel;

        private CompositeDisposable m_ButtonsBindings;

        public override void Bind(TViewModel viewModel)
        {
            base.Bind(viewModel);
            
            SetPageLabel();
            Add(ViewModel.ManualIsCompleted.Subscribe(SetCompletedButton));

            gameObject.SetActive(true);
            SetPositionHiddenRight();
        }

        public void BindButtons()
        {
            if (m_ButtonsBindings != null)
            {
                return;
            }
            m_ButtonsBindings = new CompositeDisposable();
            m_ButtonsBindings.AddRange(GetButtonsBindings());
        }

        protected virtual IEnumerable<IDisposable> GetButtonsBindings()
        {
            yield return m_CompletedButton.OnClickAsObservable().Subscribe(_ => ViewModel.GoFurther());
            yield return m_CloseButton.OnClickAsObservable().Subscribe(_ => ViewModel.Close());
        }

        public void UnbindButtons()
        {
            if (m_ButtonsBindings == null)
            {
                return;
            }
            m_ButtonsBindings.Dispose();
            m_ButtonsBindings = null;
        }

        public void AppearImmediate()
        {
            SetPositionCenter();
        }

        public void Appear()
        {
            Sequence sequence = DOTween.Sequence();
            
            sequence.AppendCallback(SetPositionHiddenRight);
            sequence.Append(m_RectTransform.DOPivotX(1f, UIConsts.InteractionTime));
            sequence.AppendCallback(BindButtons);
            sequence.AppendCallback(SetPositionCenter);
        }
        
        public void DisappearImmediate()
        {
            SetPositionHiddenLeft();
            UnbindButtons();
        }
        
        public void Disappear()
        {
            Sequence sequence = DOTween.Sequence();
            
            sequence.AppendCallback(SetPositionCenter);
            sequence.Append(m_RectTransform.DOPivotX(1f, UIConsts.InteractionTime));
            sequence.AppendCallback(UnbindButtons);
            sequence.AppendCallback(SetPositionHiddenLeft);
        }
        
        private void SetPositionCenter()
        {
            float previousWidth = m_RectTransform.rect.width;
            
            m_RectTransform.anchorMin = new Vector2(0,0);
            m_RectTransform.anchorMax = new Vector2(0,1);
            m_RectTransform.pivot = new Vector2(0f, 0.5f);
            m_RectTransform.offsetMin = Vector2.zero;
            m_RectTransform.offsetMax = new Vector2(previousWidth, 0);
        }

        private void SetPositionHiddenLeft()
        {
            float previousWidth = m_RectTransform.rect.width;
            
            m_RectTransform.anchorMin = new Vector2(0,0);
            m_RectTransform.anchorMax = new Vector2(0,1);
            m_RectTransform.pivot = new Vector2(1, 0.5f);
            m_RectTransform.offsetMin = new Vector2(-previousWidth, 0);
            m_RectTransform.offsetMax = Vector2.zero;
        }
        
        private void SetPositionHiddenRight()
        {
            float previousWidth = m_RectTransform.rect.width;

            m_RectTransform.anchorMin = new Vector2(1,0);
            m_RectTransform.anchorMax = new Vector2(1,1);
            m_RectTransform.pivot = new Vector2(0, 0.5f);
            m_RectTransform.offsetMin = Vector2.zero;
            m_RectTransform.offsetMax = new Vector2(previousWidth, 0);
        }

        private void SetPageLabel()
        {
            m_PageLabel.text = $"{ViewModel.PageNumber + 1} из {ViewModel.PagesCount}";
        }

        private void SetCompletedButton(bool completed)
        {
            m_CompletedButton.interactable = completed;
            m_CompletedButtonLabel.text = completed ? "Готово!" : GetNotCompletedString();
        }

        protected abstract string GetNotCompletedString();

        protected override void UnbindViewAction()
        {
            gameObject.SetActive(false);
        }
    }
}
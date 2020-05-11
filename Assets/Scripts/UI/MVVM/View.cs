using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Util.UniRxExtensions;

namespace UI.MVVM
{
    public abstract class View<TViewModel> : MonoBehaviourCompositeDisposable, IDisposable where TViewModel : ViewModel
    {
        private readonly List<IDisposable> m_Disposables = new List<IDisposable>();
        
        protected TViewModel ViewModel;

        private IDisposable m_UnbindDisposable;

        public virtual void Bind(TViewModel viewModel)
        {
            if (ViewModel != null)
            {
                Unbind();
            }
            ViewModel = viewModel;
            ViewModel.Add(this);

            m_UnbindDisposable = Disposable.Create(Unbind);
            Add(m_UnbindDisposable);
        }

        private void Unbind()
        {
            ViewModel?.Remove(this);
            Remove(m_UnbindDisposable);
            UnbindViewAction();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        protected virtual void UnbindViewAction() { }
    }
}
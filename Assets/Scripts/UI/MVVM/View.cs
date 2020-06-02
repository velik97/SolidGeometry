using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Util.UniRxExtensions;

namespace UI.MVVM
{
    public abstract class View<TViewModel> : MonoBehaviourMultipleDisposable, IDisposable where TViewModel : ViewModel
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
            ViewModel.AddDisposable(this);

            m_UnbindDisposable = Disposable.Create(Unbind);
            AddDisposable(m_UnbindDisposable);
        }

        private void Unbind()
        {
            ViewModel?.RemoveDisposable(this);
            RemoveDisposable(m_UnbindDisposable);
            ViewModel = null;
            UnbindViewAction();
            Recover();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        protected virtual void UnbindViewAction() { }
    }
}
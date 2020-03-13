using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Util.UniRxExtensions;

namespace UI.MVVM
{
    public abstract class View<TViewModel> : MonoBehaviourCompositeDisposable, IDisposable where TViewModel : ViewModel, new()
    {
        private readonly List<IDisposable> m_Disposables = new List<IDisposable>();
        
        protected TViewModel ViewModel;

        private IDisposable m_UnbindDisposable;

        public TViewModel CreateViewModelAndBind()
        {
            TViewModel vm = new TViewModel();
            Bind(vm);
            return vm;
        }

        public virtual void Bind(TViewModel viewModel)
        {
            Unbind();
            ViewModel = viewModel;
            ViewModel.Add(this);

            m_UnbindDisposable = Disposable.Create(Unbind);
            Add(m_UnbindDisposable);
        }

        private void Unbind()
        {
            ViewModel?.Remove(this);
            Remove(m_UnbindDisposable);
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
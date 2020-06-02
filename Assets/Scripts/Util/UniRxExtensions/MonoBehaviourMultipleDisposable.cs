using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Util.UniRxExtensions
{
    public class MonoBehaviourMultipleDisposable : MonoBehaviour, IDisposable
    {
        private CompositeDisposable m_InnerDisposable = new CompositeDisposable();

        public void AddDisposable(IDisposable disposable)
        {
            m_InnerDisposable.Add(disposable);
        }

        public void RemoveDisposable(IDisposable disposable)
        {
            m_InnerDisposable.Remove(disposable);
        }
        
        public void AddDisposableRange(IEnumerable<IDisposable> disposables)
        {
            foreach (IDisposable disposable in disposables)
            {
                AddDisposable(disposable);
            }
        }
        
        public void Dispose()
        {
            m_InnerDisposable.Dispose();
        }

        public void Recover()
        {
            m_InnerDisposable.Recover();
        }
    }
}
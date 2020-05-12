using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace Util.UniRxExtensions
{
    public static class CompositeDisposableExtensions
    {
        public static void AddRange(this CompositeDisposable compositeDisposable, IEnumerable<IDisposable> disposables)
        {
            foreach (IDisposable disposable in disposables)
            {
                compositeDisposable.Add(disposable);
            }
        }
        
        public static void AddRange(this MonoBehaviourCompositeDisposable compositeDisposable, IEnumerable<IDisposable> disposables)
        {
            foreach (IDisposable disposable in disposables)
            {
                compositeDisposable.Add(disposable);
            }
        }
    }
}
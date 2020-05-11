using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Util
{
    public class CoroutineRunner : MonoSingleton<CoroutineRunner>
    {
        public static IDisposable Run(IEnumerator coroutine)
        {
            Coroutine routine = Instance.StartCoroutine(coroutine);
            return Disposable.Create(() => Instance.StopCoroutine(routine));
        }

        public static IDisposable Run(AsyncOperation asyncOperation)
        {
            return Run(AsyncOperationCoroutine(asyncOperation));
        }

        private static IEnumerator AsyncOperationCoroutine(AsyncOperation asyncOperation)
        {
            yield return asyncOperation;
        }

        public static void StopAll()
        {
            Instance.StopAllCoroutines();
        }
    }
}
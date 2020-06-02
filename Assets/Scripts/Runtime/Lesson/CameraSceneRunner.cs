using System;
using Runtime.Access.Camera;
using UnityEngine;
using Util.UniRxExtensions;

namespace Runtime.Lesson
{
    [RequireComponent(typeof(Camera))]
    public class CameraSceneRunner : MonoBehaviourMultipleDisposable, ISceneRunner
    {
        public void Initialize()
        {
            CameraAccess.Instance.SetCamera(GetComponent<Camera>());
        }

        public void Unload(Action callback)
        {
            CameraAccess.Instance.DeleteCamera(GetComponent<Camera>());
            callback.Invoke();
        }
    }
}
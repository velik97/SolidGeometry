using System;
using Runtime.CameraManagement;
using Runtime.Core;
using UnityEngine;
using Util.UniRxExtensions;

namespace Runtime.Session
{
    [RequireComponent(typeof(Camera))]
    public class CameraSceneRunner : MonoBehaviourCompositeDisposable, ISceneRunner
    {
        public void Initialize(GlobalData globalData)
        {
            CameraOwner.Instance.SetCamera(GetComponent<Camera>());
        }

        public void Unload(Action callback)
        {
            CameraOwner.Instance.DeleteCamera(GetComponent<Camera>());
            callback.Invoke();
        }
    }
}
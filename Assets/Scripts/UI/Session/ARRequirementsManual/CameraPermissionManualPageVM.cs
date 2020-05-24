using System;
using System.Collections;
using Runtime.Global.DeviceARRequirements;
using Runtime.Global.DeviceARRequirements.CameraPermission;
using UniRx;
using UnityEngine;
using Util;

namespace UI.Session.ARRequirementsManual
{
    public class CameraPermissionManualPageVM : ManualPageVM
    {
        private readonly ICameraPermissionProvider m_CameraPermissionProvider;
        
        private readonly BoolReactiveProperty m_ManualIsCompleted = new BoolReactiveProperty();
        public override IReadOnlyReactiveProperty<bool> ManualIsCompleted => m_ManualIsCompleted;

        public RuntimePlatform RunningPlatform => 
#if UNITY_IOS
            RuntimePlatform.IPhonePlayer;
#elif UNITY_ANDROID
            RuntimePlatform.Android;
#else
            RuntimePlatform.IPhonePlayer;
#endif

        public CameraPermissionManualPageVM(int pageNumber, int pagesCount, Action closeAction, Action goFurtherAction,
            ICameraPermissionProvider cameraPermissionProvider)
            : base(pageNumber, pagesCount, closeAction, goFurtherAction)
        {
            m_CameraPermissionProvider = cameraPermissionProvider;
            
            Add(CoroutineRunner.Run(UpdateIdCompletedPropertyCoroutine()));
        }

        public void GoToPermissionSettings()
        {
            m_CameraPermissionProvider.GoToPermissionSettings();
        }

        public override void GoFurther()
        {
            m_ManualIsCompleted.Value = m_CameraPermissionProvider.HaveCameraPermission();
            base.GoFurther();
        }

        private IEnumerator UpdateIdCompletedPropertyCoroutine()
        {
            while (true)
            {
                m_ManualIsCompleted.Value = m_CameraPermissionProvider.HaveCameraPermission();
                yield return new WaitForSeconds(.5f);
            }
        }
    }
}
using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Util;
using Util.UniRxExtensions;

namespace Runtime.Global.DeviceARRequirements.ARSupport
{
    public class TestARSupportProvider : MonoSingleton<TestARSupportProvider>, IARSupportProvider
    {
        private ReactiveProperty<bool> m_ARIsCheckedAndSupported = new ReactiveProperty<bool>(false);
        private ReactiveProperty<bool> m_NeedARInstall = new ReactiveProperty<bool>(true);
        private ReactiveProperty<bool> m_IsInstalling = new ReactiveProperty<bool>(false);

        public IReadOnlyReactiveProperty<bool> ARIsCheckedAndSupported => m_ARIsCheckedAndSupported;

        public IReadOnlyReactiveProperty<bool> ARIsReady => 
            m_ARIsCheckedAndSupported
                .And(m_IsInstalling.Not())
                .And(m_NeedARInstall.Not())
                .ToReadOnlyReactiveProperty();

        public IReadOnlyReactiveProperty<bool> NeedARInstall => m_NeedARInstall;

        public IReadOnlyReactiveProperty<bool> IsInstalling => m_IsInstalling;

        public void InstallARSoftware()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.AppendCallback(() =>
            {
                m_NeedARInstall.Value = false;
                m_IsInstalling.Value = true;
            });
            sequence.AppendInterval(1f);
            sequence.AppendCallback(() =>
            {
                m_NeedARInstall.Value = false;
                m_IsInstalling.Value = false;
            });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                m_ARIsCheckedAndSupported.Value = true;
            }
        }
    }
}
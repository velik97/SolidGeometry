using DG.Tweening;
using UniRx;
using UnityEngine;
using Util;
using Util.UniRxExtensions;

namespace Runtime.Access.DeviceARRequirements.ARSupport
{
    public class TestARSupportProvider : MultipleDisposable, IARSupportProvider
    {
        private ReactiveProperty<bool> m_ARIsCheckedAndSupported = new ReactiveProperty<bool>(false);
        private ReactiveProperty<bool> m_NeedARInstall = new ReactiveProperty<bool>(true);
        private ReactiveProperty<bool> m_IsInstalling = new ReactiveProperty<bool>(false);
        private ReadOnlyReactiveProperty<bool> m_ARIsReady;

        public IReadOnlyReactiveProperty<bool> ARIsCheckedAndSupported => m_ARIsCheckedAndSupported;
        
        public IReadOnlyReactiveProperty<bool> NeedInstall => m_NeedARInstall;

        public IReadOnlyReactiveProperty<bool> IsInstalling => m_IsInstalling;

        public IReadOnlyReactiveProperty<bool> ARIsReady => m_ARIsReady;

        public TestARSupportProvider()
        {
            AddDisposable(m_ARIsReady = m_ARIsCheckedAndSupported
                .And(m_IsInstalling.Not())
                .And(m_NeedARInstall.Not())
                .ToReadOnlyReactiveProperty());
            
            AddDisposable(MainThreadDispatcher.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Q))
                .Subscribe(_ => m_ARIsCheckedAndSupported.Value = true));
        }

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
    }
}
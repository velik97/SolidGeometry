using UnityEngine;
using Util.UniRxExtensions;

namespace Runtime.Core
{
    public class CoreRunner : MonoBehaviourCompositeDisposable
    {
        [SerializeField] private ApplicationConfig m_ApplicationConfig;

        private ApplicationModeManager m_ApplicationModeManager;

        private void Awake()
        {
            InitializeApplicationModeManager();

            m_ApplicationModeManager.GlobalData.RequestChangeApplicationMode(ApplicationMode.MainMenu);
        }

        private void InitializeApplicationModeManager()
        {
            Add(m_ApplicationModeManager = new ApplicationModeManager(m_ApplicationConfig));
        }

        private void OnDisable()
        {
            Dispose();
        }
    }

    
}
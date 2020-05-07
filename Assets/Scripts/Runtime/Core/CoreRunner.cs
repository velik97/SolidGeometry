using System;
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
            InitializeRootFolder();
            InitializeApplicationModeManager();

            m_ApplicationModeManager.GlobalData.RequestChangeApplicationMode(ApplicationMode.MainMenu);
        }

        private void InitializeRootFolder()
        {
            if (m_ApplicationConfig.RootFolder.HaveCycles())
            {
                throw new ArgumentException("Have cycles in folders!");
            }
            m_ApplicationConfig.RootFolder.AssignParentFolders();
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
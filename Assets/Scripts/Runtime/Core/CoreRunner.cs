using System;
using System.IO;
using Lesson;
using Serialization;
using UnityEngine;
using Util.EventBusSystem;
using Util.UniRxExtensions;

namespace Runtime.Core
{
    public class CoreRunner : MonoBehaviourCompositeDisposable
    {
        [SerializeField] private ApplicationConfig m_ApplicationConfig;

        private ApplicationModeManager m_ApplicationModeManager;

        private void Awake()
        {
            Initialize();

            // Will be changed to main menu
            EventBus.RaiseEvent<IApplicationModeHandler>(
                h => h.HandleChangeApplicationMode(ApplicationMode.MainMenu));
        }

        private void Initialize()
        {
            Add(m_ApplicationModeManager = new ApplicationModeManager(m_ApplicationConfig));
        }

        private void OnDisable()
        {
            Dispose();
        }
    }

    
}
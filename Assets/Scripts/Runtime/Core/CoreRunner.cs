using System;
using Runtime.Global;
using Runtime.Global.ApplicationModeManagement;
using UnityEngine;
using Util.UniRxExtensions;

namespace Runtime.Core
{
    public class CoreRunner : MonoBehaviourCompositeDisposable
    {
        [SerializeField] private ApplicationConfig m_ApplicationConfig;


        private void Awake()
        {
            InitializeRootFolder();

            Add(GlobalAccess.Create(m_ApplicationConfig));
            ApplicationModeAccess.Instance.RequestChangeApplicationMode(ApplicationMode.MainMenu);
        }

        private void InitializeRootFolder()
        {
            if (m_ApplicationConfig.RootFolder.HaveCycles())
            {
                throw new ArgumentException("Have cycles in folders!");
            }
            bool valid = true;
            m_ApplicationConfig.RootFolder.ValidateNullReferences(ref valid);
            if (!valid)
            {
                throw new ArgumentException("Have null references in assets!");
            }
            m_ApplicationConfig.RootFolder.AssignParentFolders();
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}
using System;
using Runtime.Access;
using Runtime.Access.ApplicationModeManagement;
using UnityEngine;
using Util.UniRxExtensions;

namespace Runtime.Start
{
    public class StartRunner : MonoBehaviourMultipleDisposable
    {
        [SerializeField] private ApplicationConfig m_ApplicationConfig;


        private void Awake()
        {
            InitializeRootFolder();

            AddDisposable(RootAccess.Create(m_ApplicationConfig));
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
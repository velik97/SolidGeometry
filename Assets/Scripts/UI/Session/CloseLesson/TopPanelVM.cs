using System;
using Runtime.Core;
using UI.MVVM;
using UniRx;
using UnityEngine;
using Util.EventBusSystem;

namespace UI.Session.CloseLesson
{
    public class TopPanelVM : ViewModel, IApplicationModeHandler
    {
        private GlobalData m_GlobalData;
        
        private StringReactiveProperty m_FunctionalButtonName = new StringReactiveProperty(string.Empty);
        public IReadOnlyReactiveProperty<string> FunctionalButtonName => m_FunctionalButtonName;

        public TopPanelVM(GlobalData globalData)
        {
            m_GlobalData = globalData;
            
            Add(EventBus.Subscribe(this));
        }

        public void HandleApplicationModeChanged(ApplicationMode mode)
        {
            switch (mode)
            {
                case ApplicationMode.Session3D:
                    m_FunctionalButtonName.Value = "AR";
                    break;
                case ApplicationMode.SessionAR:
                    m_FunctionalButtonName.Value = "Reset";
                    break;
            }
        }

        public void OnBackPressed()
        {
            switch (m_GlobalData.CurrentApplicationMode)
            {
                case ApplicationMode.Session3D:
                    m_GlobalData.RequestChangeApplicationMode(ApplicationMode.MainMenu);
                    break;
                case ApplicationMode.SessionAR:
                    m_GlobalData.RequestChangeApplicationMode(ApplicationMode.Session3D);
                    break;
            }
        }

        public void OnFunctionalPressed()
        {
            switch (m_GlobalData.CurrentApplicationMode)
            {
                case ApplicationMode.Session3D:
                    m_GlobalData.RequestChangeApplicationMode(ApplicationMode.SessionAR);
                    break;
                case ApplicationMode.SessionAR:
                    Debug.Log("Reset lesson position");
                    break;
            }
        }
    }
}
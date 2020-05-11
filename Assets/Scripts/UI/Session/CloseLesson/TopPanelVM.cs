using System;
using Runtime.Core;
using Runtime.Global;
using Runtime.Global.ApplicationModeManagement;
using UI.MVVM;
using UniRx;
using UnityEngine;
using Util.EventBusSystem;

namespace UI.Session.CloseLesson
{
    public class TopPanelVM : ViewModel, IApplicationModeHandler
    {
        private readonly StringReactiveProperty m_ChangeModeButtonName = new StringReactiveProperty(string.Empty);
        public IReadOnlyReactiveProperty<string> ChangeModeButtonName => m_ChangeModeButtonName;

        public readonly IReadOnlyReactiveProperty<bool> CanChangeMode;

        private Action m_GoBackAction;
        private Action m_ChangeModeAction;
        
        public TopPanelVM(Action goBackAction, Action changeModeAction, IReadOnlyReactiveProperty<bool> canChangeMode)
        {
            m_GoBackAction = goBackAction;
            m_ChangeModeAction = changeModeAction;

            CanChangeMode = canChangeMode;
            
            Add(EventBus.Subscribe(this));
        }

        public void HandleApplicationModeChanged(ApplicationMode mode)
        {
            switch (mode)
            {
                case ApplicationMode.Session3D:
                    m_ChangeModeButtonName.Value = "AR";
                    break;
                case ApplicationMode.SessionAR:
                    m_ChangeModeButtonName.Value = "3D";
                    break;
            }
        }

        public void OnBackPressed()
        {
            m_GoBackAction?.Invoke();
        }

        public void OnChangeModePressed()
        {
            if (!CanChangeMode.Value)
            {
                return;
            }
            m_ChangeModeAction?.Invoke();
        }
    }
}
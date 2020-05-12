using System.Collections.Generic;
using System.Linq;
using Runtime.Global.DeviceARRequirements;
using UniRx;
using UnityEngine;
using Util.EventBusSystem;
using Util.SceneUtils;

namespace Runtime.Global.ApplicationModeManagement
{
    public class ApplicationModeAccess : CompositeDisposable
    {
        public static ApplicationModeAccess Instance => GlobalAccess.Instance.ApplicationModeAccess;
        
        private readonly ApplicationConfig m_ApplicationConfig;
        
        private readonly Dictionary<ApplicationMode, List<SceneReference>> m_ModeScenes;
        private readonly List<SceneData> m_RunningScenes;
        
        private List<SceneReference> m_SceneReferencesToLoad;
        private List<SceneData> m_SceneDatasToInitialize;
        private List<SceneData> m_SceneDatasToUnload;

        private bool m_ChangingMode = false;

        private int m_ScenesToLoadCount;

        private ApplicationMode m_NewApplicationMode;
        
        private ReactiveProperty<ApplicationMode> m_CurrentApplicationModeProperty = new ReactiveProperty<ApplicationMode>();
        public IReadOnlyReactiveProperty<ApplicationMode> CurrentApplicationModeProperty => m_CurrentApplicationModeProperty;
        public ApplicationMode CurrentApplicationMode => m_CurrentApplicationModeProperty.Value;
        
        private const string LAST_SESSION_MODE_KEY = "LAST_SESSION_MODE";
        private ApplicationMode m_LastSessionMode = ApplicationMode.None;
        private ApplicationMode LastSessionMode
        {
            get
            {
                if (m_LastSessionMode != ApplicationMode.Session3D &&
                    m_LastSessionMode != ApplicationMode.SessionAR)
                {
                    if (PlayerPrefs.HasKey(LAST_SESSION_MODE_KEY))
                    {
                        m_LastSessionMode = (ApplicationMode)PlayerPrefs.GetInt(LAST_SESSION_MODE_KEY);
                        if (m_LastSessionMode != ApplicationMode.Session3D &&
                            m_LastSessionMode != ApplicationMode.SessionAR)
                        {
                            m_LastSessionMode = ApplicationMode.Session3D;
                        }
                    }
                    else
                    {
                        LastSessionMode = ApplicationMode.Session3D;
                    }
                }
                
                return m_LastSessionMode;
            }
            set
            {
                if (value != ApplicationMode.Session3D &&
                    value != ApplicationMode.SessionAR)
                {
                    return;
                }
                if (m_LastSessionMode == value)
                {
                    return;
                }
                PlayerPrefs.SetInt(LAST_SESSION_MODE_KEY, (int)value);
                m_LastSessionMode = value;
            }
        }
        
        public ApplicationModeAccess(ApplicationConfig applicationConfig)
        {
            m_ApplicationConfig = applicationConfig;
            m_ModeScenes = new Dictionary<ApplicationMode, List<SceneReference>>();
            m_RunningScenes = new List<SceneData>();
            
            FillModeScenesDictionary();
        }

        private void FillModeScenesDictionary()
        {
            RegisterSceneReference(m_ApplicationConfig.MainMenuMechanicsScene, ApplicationMode.MainMenu);
            
            RegisterSceneReference(m_ApplicationConfig.Session3DMechanicsScene, ApplicationMode.Session3D);
            RegisterSceneReference(m_ApplicationConfig.SessionARMechanicsScene, ApplicationMode.SessionAR);
            RegisterSceneReference(m_ApplicationConfig.SessionUIScene, ApplicationMode.Session3D, ApplicationMode.SessionAR);
            
            RegisterSceneReference(m_ApplicationConfig.ShapeViewFactoryScene, ApplicationMode.MainMenu, ApplicationMode.Session3D, ApplicationMode.SessionAR);
            RegisterSceneReference(m_ApplicationConfig.CameraScene3D, ApplicationMode.MainMenu, ApplicationMode.Session3D);
            RegisterSceneReference(m_ApplicationConfig.CameraSceneAR, ApplicationMode.SessionAR);
        }

        private void RegisterSceneReference(SceneReference sceneReference, params ApplicationMode[] modes)
        {
            if (sceneReference == null)
            {
                return;
            }
            foreach (ApplicationMode mode in modes)
            {
                if (!m_ModeScenes.ContainsKey(mode))
                {
                    m_ModeScenes[mode] = new List<SceneReference>();
                }
                m_ModeScenes[mode].Add(sceneReference);
            }
        }
        
        public void GoToLessonMode()
        {
            if (LastSessionMode == ApplicationMode.SessionAR && !DeviceARRequirementsAccess.Instance.ReadyForAR())
            {
                LastSessionMode = ApplicationMode.Session3D;
            }
            RequestChangeApplicationMode(LastSessionMode);
        }

        public void RequestChangeApplicationMode(ApplicationMode mode)
        {
            if (m_ChangingMode)
            {
                Debug.LogError("Can't change mode while changing other");
                return;
            }
            m_ChangingMode = true;
            m_NewApplicationMode = mode;
            
            List<SceneReference> sceneReferencesForMode = m_ModeScenes[mode];

            m_SceneReferencesToLoad = sceneReferencesForMode
                .Where(reference => !m_RunningScenes.Any(data => data.HasSameScene(reference)))
                .ToList();
            m_SceneDatasToInitialize = new List<SceneData>();

            m_SceneDatasToUnload = m_RunningScenes
                .Where(data => !sceneReferencesForMode.Any(data.HasSameScene))
                .ToList();

            m_ScenesToLoadCount = m_SceneReferencesToLoad.Count();
            
            foreach (SceneReference sceneReference in m_SceneReferencesToLoad)
            {
                SceneData.CreateAsync(sceneReference, OnSceneLoaded);
            }
        }

        private void OnSceneLoaded(SceneData sceneData)
        {
            m_SceneDatasToInitialize.Add(sceneData);
            if (m_SceneDatasToInitialize.Count < m_ScenesToLoadCount)
            {
                return;
            }
            
            foreach (SceneData data in m_SceneDatasToUnload)
            {
                data.Unload();
                m_RunningScenes.Remove(data);
            }
            
            foreach (SceneData data in m_SceneDatasToInitialize)
            {
                data.Initialize();
                m_RunningScenes.Add(data);
            }

            m_ChangingMode = false;
            OnApplicationModeChanged(m_NewApplicationMode);
        }
        
        private void OnApplicationModeChanged(ApplicationMode mode)
        {
            if (CurrentApplicationMode == ApplicationMode.Session3D ||
                CurrentApplicationMode == ApplicationMode.SessionAR)
            {
                LastSessionMode = CurrentApplicationMode;
            }
            
            m_CurrentApplicationModeProperty.Value = mode;
            EventBus.RaiseEvent<IApplicationModeHandler>(h => h.HandleApplicationModeChanged(mode));
        }
    }
    
    public enum ApplicationMode
    {
        None,
        MainMenu,
        Session3D,
        SessionAR
    }
}
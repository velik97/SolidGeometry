﻿using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Util.EventBusSystem;
using Util.SceneUtils;

namespace Runtime.Core
{
    public class ApplicationModeManager : CompositeDisposable, IApplicationModeHandler
    {
        private readonly ApplicationConfig m_ApplicationConfig;
        
        private readonly Dictionary<ApplicationMode, List<SceneReference>> m_ModeScenes;
        private readonly List<SceneData> m_RunningScenes;

        private readonly GlobalData m_GlobalData;

        private List<SceneReference> m_SceneReferencesToLoad;
        private List<SceneData> m_SceneDatasToInitialize;
        private List<SceneData> m_SceneDatasToUnload;

        private bool m_ChangingMode = false;

        private int m_ScenesToLoadCount;

        public ApplicationModeManager(ApplicationConfig applicationConfig)
        {
            m_ApplicationConfig = applicationConfig;
            
            m_GlobalData = new GlobalData();
            
            m_ModeScenes = new Dictionary<ApplicationMode, List<SceneReference>>();
            m_RunningScenes = new List<SceneData>();
            
            FillModeScenesDictionary();
            
            Add(EventBus.Subscribe(this));
        }

        private void FillModeScenesDictionary()
        {
            RegisterSceneReference(m_ApplicationConfig.MainMenuMechanicsScene, ApplicationMode.MainMenu);
            
            RegisterSceneReference(m_ApplicationConfig.Session3DMechanicsScene, ApplicationMode.Session3D);
            RegisterSceneReference(m_ApplicationConfig.SessionARMechanicsScene, ApplicationMode.SessionAR);
            RegisterSceneReference(m_ApplicationConfig.SessionUIScene, ApplicationMode.Session3D, ApplicationMode.SessionAR);
            
            RegisterSceneReference(m_ApplicationConfig.ShapeViewFactoryScene, ApplicationMode.MainMenu, ApplicationMode.Session3D, ApplicationMode.SessionAR);
            RegisterSceneReference(m_ApplicationConfig.CameraScene, ApplicationMode.MainMenu, ApplicationMode.Session3D, ApplicationMode.SessionAR);
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

        public void HandleChangeApplicationMode(ApplicationMode mode)
        {
            ChangeMode(mode);
        }

        private void ChangeMode(ApplicationMode mode)
        {
            if (m_ChangingMode)
            {
                Debug.LogError("Can't change mode while changing other");
                return;
            }
            m_ChangingMode = true;
            
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
                data.Initialize(m_GlobalData);
                m_RunningScenes.Add(data);
            }

            m_ChangingMode = false;
        }
    }
    
    public enum ApplicationMode
    {
        MainMenu,
        Session3D,
        SessionAR
    }
}
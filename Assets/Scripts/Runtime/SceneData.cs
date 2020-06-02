using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util.SceneUtils;

namespace Runtime
{
    public class SceneData
    {
        private Scene m_Scene;
        private ISceneRunner m_Runner;
        
        private SceneReference m_SceneReference;

        public static void CreateAsync(SceneReference sceneReference, Action<SceneData> callback)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneReference, LoadSceneMode.Additive);

            operation.completed += _ => callback.Invoke(Create(sceneReference));
        }

        private static SceneData Create(SceneReference sceneReference)
        {
            Scene scene = SceneManager.GetSceneByPath(sceneReference);
            
            var rootGameObjects = scene.GetRootGameObjects();
            ISceneRunner runner = null;

            for (int i = 0; i < rootGameObjects.Length; i++)
            {
                runner = rootGameObjects[i].GetComponent<ISceneRunner>();
                if (runner != null)
                    break;
            }

            return new SceneData(scene, runner, sceneReference);
        }

        private SceneData(Scene scene, ISceneRunner runner, SceneReference sceneReference)
        {
            m_Scene = scene;
            m_Runner = runner;
            m_SceneReference = sceneReference;
        }

        public bool HasSameScene(SceneReference sceneReference)
        {
            return sceneReference == m_SceneReference;
        }

        public void Initialize()
        {
            m_Runner?.Initialize();
        }

        public void Unload()
        {
            if (m_Runner != null)
            {
                m_Runner.Unload(UnloadScene);
            }
            else
            {
                UnloadScene();
            }
        }

        private void UnloadScene()
        {
            SceneManager.UnloadSceneAsync(m_Scene);
        }
    }
}
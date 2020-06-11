using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.CascadeUpdate
{
    public class CascadeUpdateQueueExecutor
    {
        private List<Action> m_ActionsQueue = new List<Action>();
        private int m_MaxActionsPerFrame = 200;

        private IDisposable m_RuntimeUpdateDisposable;
        
        public CascadeUpdateQueueExecutor()
        {
            UpdateTickSource();
        }

        public void UpdateTickSource()
        {
            m_RuntimeUpdateDisposable?.Dispose();
            EditorUpdateInvokerBridge.Update -= Tick;
            
            if (Application.isPlaying)
            {
                m_RuntimeUpdateDisposable = CoroutineRunner.Run(UpdateCoroutine());
            }
            else
            {
                EditorUpdateInvokerBridge.Update += Tick;
            }
        }

        public void AddActions(IEnumerable<Action> actions)
        {
            foreach (Action action in actions)
            {
                if (m_ActionsQueue.Contains(action))
                {
                    m_ActionsQueue.Remove(action);
                }

                m_ActionsQueue.Add(action);
            }
        }

        private IEnumerator UpdateCoroutine()
        {
            while (true)
            {
                Tick();
                yield return null;
            }
        }

        private void Tick()
        {
            if (m_ActionsQueue.Count == 0)
            {
                return;
            }
            
            for (int i = 0; i < m_MaxActionsPerFrame; i++)
            {
                if (m_ActionsQueue.Count == 0)
                {
                    break;
                }
                    
                using (CascadeUpdateEvent.SuppressCascadeInvokeScope())
                {
                    try
                    {
                        m_ActionsQueue[0]?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Error in cascade action: " + e);
                    }
                    m_ActionsQueue.RemoveAt(0);
                }
            }
        }
    }
}
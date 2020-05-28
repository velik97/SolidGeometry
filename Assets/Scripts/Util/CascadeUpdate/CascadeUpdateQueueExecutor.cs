using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Util.CascadeUpdate
{
    public class CascadeUpdateQueueExecutor
    {
        private Queue<Action> m_ActionsQueue = new Queue<Action>();
        private int m_MaxActionsPerFrame = 100;

        public CascadeUpdateQueueExecutor()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorApplication.update += Tick;
                return;
            }
#endif
            CoroutineRunner.Run(UpdateCoroutine());
        }

        public void AddActions(IEnumerable<Action> actions)
        {
            foreach (Action action in actions)
            {
                m_ActionsQueue.Enqueue(action);
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
                    m_ActionsQueue.Dequeue()?.Invoke();
                }
            }
        }
    }
}
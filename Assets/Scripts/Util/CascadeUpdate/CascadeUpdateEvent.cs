using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace Util.CascadeUpdate
{
    public class CascadeUpdateEvent
    {
        private static CascadeUpdateQueueExecutor s_Executor = new CascadeUpdateQueueExecutor();
        private static bool s_SuppressCascadeInvoke = false;

        private List<Action> m_UpdateActions = new List<Action>();

        private List<CascadeUpdateEvent> m_Subscribers;

        public CascadeUpdateEvent()
        {
            m_Subscribers = new List<CascadeUpdateEvent>();
        }

        public IDisposable Subscribe(CascadeUpdateEvent cascadeUpdateEvent)
        {
            m_Subscribers.Add(cascadeUpdateEvent);
            return Disposable.Create(() => Unsubscribe(cascadeUpdateEvent));
        }

        public void Unsubscribe(CascadeUpdateEvent cascadeUpdateEvent)
        {
            m_Subscribers.Remove(cascadeUpdateEvent);
        }
        
        public IDisposable Subscribe(Action updateAction)
        {
            m_UpdateActions.Add(updateAction);
            return Disposable.Create(() => Unsubscribe(updateAction));
        }

        public void Unsubscribe(Action updateAction)
        {
            m_UpdateActions.Remove(updateAction);
        }

        public void Clear()
        {
            m_UpdateActions.Clear();
            m_Subscribers.Clear();
        }

        public void Invoke()
        {
            if (s_SuppressCascadeInvoke)
            {
                return;
            }

            IEnumerable<Action> actionsQueue = ConstructActionsQueue(this);
            s_Executor.UpdateTickSource();
            s_Executor.AddActions(actionsQueue);
        }

        public static IDisposable SuppressCascadeInvokeScope()
        {
            s_SuppressCascadeInvoke = true;
            return Disposable.Create(() => s_SuppressCascadeInvoke = false);
        }

        private static IEnumerable<Action> ConstructActionsQueue(CascadeUpdateEvent cascadeUpdateEvent)
        {
            List<CascadeUpdateEvent> eventsQueue = new List<CascadeUpdateEvent>();
            Queue<CascadeUpdateEvent> eventsToVisit = new Queue<CascadeUpdateEvent>();

            eventsToVisit.Enqueue(cascadeUpdateEvent);

            while (eventsToVisit.Count > 0)
            {
                CascadeUpdateEvent evt = eventsToVisit.Dequeue();
                if (eventsQueue.Contains(evt))
                {
                    eventsQueue.Remove(evt);
                }
                eventsQueue.Add(evt);
                
                foreach (CascadeUpdateEvent subscriber in evt.m_Subscribers)
                {
                    eventsToVisit.Enqueue(subscriber);
                }
            }
            
            List<Action> actionsQueue = new List<Action>();
            
            foreach (CascadeUpdateEvent updateEvent in eventsQueue)
            {
                if (updateEvent.m_UpdateActions == null || updateEvent.m_UpdateActions.Count == 0)
                {
                    continue;
                }

                foreach (Action action in updateEvent.m_UpdateActions)
                {
                    if (actionsQueue.Contains(action))
                    {
                        actionsQueue.Remove(action);
                    }

                    actionsQueue.Add(action);
                }
            }

            return actionsQueue;
        }
    }
}
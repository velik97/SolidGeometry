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

        public void Invoke()
        {
            if (s_SuppressCascadeInvoke)
            {
                return;
            }

            IEnumerable<Action> actionsQueue = ConstructActionsQueue(this);
            s_Executor.AddActions(actionsQueue);
        }

        public static IDisposable SuppressCascadeInvokeScope()
        {
            s_SuppressCascadeInvoke = true;
            return Disposable.Create(() => s_SuppressCascadeInvoke = false);
        }

        private static IEnumerable<Action> ConstructActionsQueue(CascadeUpdateEvent cascadeUpdateEvent)
        {
            Queue<Action> actionsQueue = new Queue<Action>();
            Queue<CascadeUpdateEvent> eventsToVisit = new Queue<CascadeUpdateEvent>();
            HashSet<CascadeUpdateEvent> visitedEvents = new HashSet<CascadeUpdateEvent>();

            eventsToVisit.Enqueue(cascadeUpdateEvent);

            while (eventsToVisit.Count > 0)
            {
                CascadeUpdateEvent evt = eventsToVisit.Dequeue();
                if (visitedEvents.Contains(evt))
                {
                    continue;
                }

                visitedEvents.Add(evt);
                if (evt.m_UpdateActions != null)
                {
                    foreach (Action action in evt.m_UpdateActions)
                    {
                        actionsQueue.Enqueue(action);
                    }
                }
                foreach (CascadeUpdateEvent subscriber in evt.m_Subscribers)
                {
                    eventsToVisit.Enqueue(subscriber);
                }
            }

            return actionsQueue;
        }
    }
}
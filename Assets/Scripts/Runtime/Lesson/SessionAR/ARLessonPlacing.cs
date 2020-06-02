using System;
using System.Collections.Generic;
using Runtime.Access.ARLesson;
using Runtime.Access.Camera;
using UniRx;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Util.EventBusSystem;
using Util.UniRxExtensions;

namespace Runtime.Lesson.SessionAR
{
    public class ARLessonPlacing : MultipleDisposable, IARLessonStateHandler
    {
        private Transform m_LessonTransform;
        private GameObject m_PlacementIndicator;
        private ARRaycastManager m_ARRaycastManager;
        
        private Pose m_PlacementPose;
        private PositionUpdaterState m_State;

        private IDisposable m_UpdateSubscription;

        public ARLessonPlacing(Transform lessonTransform, GameObject placementIndicator, ARRaycastManager arRaycastManager)
        {
            m_LessonTransform = lessonTransform;
            m_PlacementIndicator = placementIndicator;
            m_ARRaycastManager = arRaycastManager;
            AddDisposable(EventBus.Subscribe(this));
        }

        public void HandleARLessonStateChanged(ARLessonState state)
        {
            if (state == ARLessonState.PlacingLesson)
            {
                StartPlacingLesson();
            }
            else
            {
                StopPlacingLesson();
            }
        }

        private void StartPlacingLesson()
        {
            if (m_UpdateSubscription != null)
            {
                return;
            }

            m_PlacementIndicator.SetActive(true);
            AddDisposable(m_UpdateSubscription = MainThreadDispatcher.UpdateAsObservable().Subscribe(_ => Tick()));
        }

        private void StopPlacingLesson()
        {
            if (m_UpdateSubscription == null)
            {
                return;
            }
            
            m_PlacementIndicator.SetActive(false);
            
            RemoveDisposable(m_UpdateSubscription);
            m_UpdateSubscription.Dispose();
            m_UpdateSubscription = null;
        }

        private void Tick()
        {
            UpdatePlacementPose();

            if (m_State != PositionUpdaterState.HaveHits)
            {
                return;
            }

            m_LessonTransform.position = m_PlacementPose.position;
            m_LessonTransform.rotation = m_PlacementPose.rotation;
        }

        private void UpdatePlacementPose()
        {
            if (m_State == PositionUpdaterState.NotActive)
            {
                return;
            }
            
            Vector2 screenCenter = CameraAccess.Instance.Camera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            m_ARRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

            m_State = hits.Count > 0
                ? PositionUpdaterState.HaveHits
                : PositionUpdaterState.HaveNoHits;
            if (m_State != PositionUpdaterState.HaveHits)
            {
                return;
            }
            
            m_PlacementPose = hits[0].pose;
            Vector3 cameraForward = CameraAccess.Instance.CameraTransform.forward;
            Vector3 cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            m_PlacementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    public enum PositionUpdaterState
    {
        NotActive,
        HaveNoHits,
        HaveHits
    }
}
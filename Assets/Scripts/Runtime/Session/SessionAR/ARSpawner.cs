// using System.Collections.Generic;
// using System.Linq;
// using PhaseControl;
// using UI.ResetPosition;
// using UniRx;
// using UnityEngine;
// using UnityEngine.XR.ARFoundation;
// using UnityEngine.XR.ARSubsystems;
// using Util;
// using Util.EventBusSystem;
// using Util.UniRxExtensions;
//
// namespace ARSpawn
// {
//     public class ARSpawner : MonoBehaviourCompositeDisposable//, IResetPositionHandler
//     {
//         public Transform ObjectToPlace;
//         public GameObject PlacementIndicator;
//         public ARRaycastManager ARRaycastManager;
//         
//         private Pose m_PlacementPose;
//         private bool m_PlacementPoseIsValid = false;
//         private Camera m_Camera;
//
//         private bool m_IsSpawning = true;
//         
//         private PhaseController m_PlacedObject;
//
//         private void Awake()
//         {
//             m_Camera = Camera.main;
//             Add(EventBus.Subscribe(this));
//         }
//
//         void Update()
//         {
//             if (!m_IsSpawning)
//             {
//                 return;
//             }
//             
//             UpdatePlacementPose();
//             UpdatePlacementIndicator();
//             
// #if UNITY_EDITOR
//             if (Input.GetMouseButtonDown(0))
//             {
//                 PlaceObject();
//             }
// #endif
//
//             if (m_PlacementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
//             {
//                 PlaceObject();
//             }
//         }
//
//         private void PlaceObject()
//         {
//             if (m_PlacedObject == null)
//             {
//                 m_PlacedObject = Instantiate(ObjectToPlace, m_PlacementPose.position, m_PlacementPose.rotation);
//             }
//             else
//             {
//                 m_PlacedObject.gameObject.SetActive(true);
//                 m_PlacedObject.transform.position = m_PlacementPose.position;
//                 m_PlacedObject.transform.rotation = m_PlacementPose.rotation;
//             }
//                 
//             // EventBus.RaiseEvent();<IPhaseControllerLifeCycleHandler>(h => h.HandlePhaseControllerInstantiated(m_PlacedObject));
//
//             PlacementIndicator.SetActive(false);
//             gameObject.SetActive(false);
//             m_IsSpawning = false;
//         }
//
//         private void UpdatePlacementIndicator()
//         {
//             if (m_PlacementPoseIsValid)
//             {
//                 PlacementIndicator.SetActive(true);
//                 PlacementIndicator.transform.SetPositionAndRotation(m_PlacementPose.position, m_PlacementPose.rotation);
//             }
//             else
//             {
//                 PlacementIndicator.SetActive(false);
//             }
//         }
//
//         private void UpdatePlacementPose()
//         {
//             Vector2 screenCenter = m_Camera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
//             List<ARRaycastHit> hits = new List<ARRaycastHit>();
//             ARRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
//
//             m_PlacementPoseIsValid = hits.Count > 0;
//             if (!m_PlacementPoseIsValid)
//             {
//                 return;
//             }
//             
//             m_PlacementPose = hits[0].pose;
//             var cameraForward = Camera.current.transform.forward;
//             var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
//             m_PlacementPose.rotation = Quaternion.LookRotation(cameraBearing);
//         }
//
//         public void HandleResetPosition()
//         {
//             PlacementIndicator.SetActive(true);
//             gameObject.SetActive(true);
//             m_IsSpawning = true;
//         }
//     }
// }
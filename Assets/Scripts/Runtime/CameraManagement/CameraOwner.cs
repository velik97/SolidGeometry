using System;
using UnityEngine;
using Util;

namespace Runtime.CameraManagement
{
    public class CameraOwner : MonoSingleton<CameraOwner>
    {
        private Camera m_Camera;
        private Transform m_CameraTransform;
        private bool m_HasCamera = false;

        public bool HasCamera
        {
            get
            {
                if (m_UpdatedCameraStatusInFrame)
                {
                    return m_HasCamera;
                }
                
                if (m_HasCamera && m_CameraTransform == null)
                {
                    m_HasCamera = false;
                    m_UpdatedCameraStatusInFrame = true;
                }
                
                return m_HasCamera;
            }
        }

        public Transform CameraTransform => m_CameraTransform;
        private bool m_UpdatedCameraStatusInFrame;

        private void Update()
        {
            m_UpdatedCameraStatusInFrame = false;
        }

        public void SetCamera(Camera cameraToSet)
        {
            if (cameraToSet == null)
            {
                return;
            }

            m_Camera = cameraToSet;
            m_CameraTransform = cameraToSet.transform;
            m_HasCamera = true;
        }

        public void DeleteCamera(Camera cameraToDelete)
        {
            if (cameraToDelete != m_Camera)
            {
                return;
            }

            m_Camera = null;
            m_CameraTransform = null;
            m_HasCamera = false;
        }
    }
}
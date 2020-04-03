using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatCamera : MonoBehaviour
{
    private Camera m_Camera;
    private void Awake()
    {
        m_Camera = Camera.main;
    }

    private void LateUpdate()
    {
        if (m_Camera == null)
        {
            return;
        }
        transform.forward = m_Camera.transform.forward;
    }

}

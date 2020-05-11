using System;
using DG.Tweening;
using UnityEngine;

namespace Test
{
    public class MoveUICanvasTest : MonoBehaviour
    {
        [SerializeField] private RectTransform m_RectTransform;

        private void Awake()
        {
            // m_RectTransform.DOPivotX(0f, 1f);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SetPositionHiddenLeft();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SetPositionHiddenRight();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SetPositionCenter();
            }
        }

        private void SetPositionHiddenLeft()
        {
            float previousWidth = m_RectTransform.rect.width;
            
            m_RectTransform.anchorMin = new Vector2(0,0);
            m_RectTransform.anchorMax = new Vector2(0,1);
            m_RectTransform.pivot = new Vector2(1, 0.5f);
            m_RectTransform.offsetMin = new Vector2(-previousWidth, 0);
            m_RectTransform.offsetMax = Vector2.zero;
        }
        
        private void SetPositionHiddenRight()
        {
            float previousWidth = m_RectTransform.rect.width;

            m_RectTransform.anchorMin = new Vector2(1,0);
            m_RectTransform.anchorMax = new Vector2(1,1);
            m_RectTransform.pivot = new Vector2(0, 0.5f);
            m_RectTransform.offsetMin = Vector2.zero;
            m_RectTransform.offsetMax = new Vector2(previousWidth, 0);
        }

        private void SetPositionCenter()
        {
            m_RectTransform.anchorMin = new Vector2(0,0);
            m_RectTransform.anchorMax = new Vector2(1,1);
            m_RectTransform.pivot = new Vector2(0.5f, 0.5f);
            m_RectTransform.offsetMin = Vector2.zero;
            m_RectTransform.offsetMax = Vector2.zero;
        }
    }
}
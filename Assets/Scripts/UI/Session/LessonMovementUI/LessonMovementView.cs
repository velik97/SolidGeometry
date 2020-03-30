using System;
using TouchScript;
using TouchScript.Examples.Checkers;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using TouchScript.Pointers;
using UI.MVVM;
using UniRx;
using UnityEngine;
using Util;

namespace UI.Session.LessonMovementUI
{
    public class LessonMovementView : View<LessonMovementVM>
    {
        // One finger gestures
        [SerializeField] private ScreenTransformGesture m_RotateXYGesture;
        
        // Two finger gestures
        [SerializeField] private ScreenTransformGesture m_RotateZGesture;
        [SerializeField] private ScreenTransformGesture m_ShiftGesture;
        [SerializeField] private ScreenTransformGesture m_ScaleGesture;
        
        // Tap gestures
        [SerializeField] private TapGesture m_ResetGesture;


        public override void Bind(LessonMovementVM viewModel)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            
            base.Bind(viewModel);
            
            m_RotateXYGesture.Delegate = new CanInteractInRectTransform(rectTransform);
            Add(m_RotateXYGesture.DeltaPositionAsObservable().Subscribe(viewModel.RotateAroundXY));
            
            m_RotateZGesture.Delegate = new CanInteractInRectTransform(rectTransform);
            Add(m_RotateZGesture.DeltaRotationAsObservable().Subscribe(viewModel.RotateAroundZ));
            m_ShiftGesture.Delegate = new CanInteractInRectTransform(rectTransform);
            Add(m_ShiftGesture.DeltaPositionAsObservable().Subscribe(viewModel.Shift));
            m_ScaleGesture.Delegate = new CanInteractInRectTransform(rectTransform);
            Add(m_ScaleGesture.DeltaScaleAsObservable().Subscribe(viewModel.Scale));
            
            m_ResetGesture.Delegate = new CanInteractInRectTransform(rectTransform);
            Add(m_ResetGesture.TapAsObservable().Subscribe(_ => viewModel.Reset()));
        }
        
        private class CanInteractInRectTransform : IGestureDelegate
        {
            private RectTransform m_RectTransform;

            public CanInteractInRectTransform(RectTransform rectTransform)
            {
                m_RectTransform = rectTransform;
            }

            public bool ShouldReceivePointer(Gesture gesture, Pointer pointer)
            {
                return m_RectTransform.GetWorldSpaceRect().Contains(pointer.Position);
            }

            public bool ShouldBegin(Gesture gesture)
            {
                return true;
            }

            public bool ShouldRecognizeSimultaneously(Gesture first, Gesture second)
            {
                return true;
            }
        }
    }
}
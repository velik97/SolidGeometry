using System;
using Runtime.Access.Lesson;
using TouchScript;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using TouchScript.Pointers;
using UI.MVVM;
using UniRx;
using UnityEngine;
using Util;

namespace UI.Lesson.LessonMovementUI
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
            
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
            AddDisposable(m_RotateXYGesture.DeltaPositionAsObservable()
                .Where(_ => ShouldRegister())
                .Subscribe(viewModel.RotateAroundXY));

            m_RotateZGesture.Delegate = new CanInteractInRectTransform(rectTransform);
            AddDisposable(m_RotateZGesture.DeltaRotationAsObservable()
                .Where(_ => ShouldRegister())
                .Subscribe(viewModel.RotateAroundZ));

            m_ShiftGesture.Delegate = new CanInteractInRectTransform(rectTransform);
            AddDisposable(m_ShiftGesture.DeltaPositionAsObservable()
                .Where(_ => ShouldRegister())
                .Subscribe(viewModel.Shift));

            m_ScaleGesture.Delegate = new CanInteractInRectTransform(rectTransform);
            AddDisposable(m_ScaleGesture.DeltaScaleAsObservable()
                .Where(_ => ShouldRegister())
                .Subscribe(viewModel.Scale));
#else
            AddDisposable(m_RotateXYGesture.DeltaPositionAsObservable()
                .Where(_ => ShouldRegister())
                .Where(_ => !Input.GetKey(KeyCode.LeftShift))
                .Subscribe(viewModel.RotateAroundXY));
            AddDisposable(m_RotateXYGesture.DeltaPositionAsObservable()
                .Where(_ => ShouldRegister())
                .Where(_ => Input.GetKey(KeyCode.LeftShift))
                .Subscribe(viewModel.Shift));
            AddDisposable(MainThreadDispatcher.UpdateAsObservable()
                .Where(_ => ShouldRegister())
                .Select(_ => Input.mouseScrollDelta.y)
                .Where(value => Math.Abs(value) > .01f)
                .Subscribe(viewModel.Scale));
#endif
            
            m_ResetGesture.Delegate = new CanInteractInRectTransform(rectTransform);
            AddDisposable(m_ResetGesture.TapAsObservable().Subscribe(_ => viewModel.Reset()));
        }

        private bool ShouldRegister()
        {
            return LessonAccess.Instance.LessonState == LessonState.Running;
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
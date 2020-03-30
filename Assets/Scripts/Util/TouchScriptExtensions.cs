using System;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using UniRx;
using UnityEngine;

namespace Util
{
    public static class TouchScriptExtensions
    {
        public static IObservable<Vector3> DeltaPositionAsObservable(this ScreenTransformGesture gesture)
        {
            return Observable
                .FromEvent<EventHandler<EventArgs>, EventArgs>(
                    h => (sender, e) => h(e),
                    h => gesture.Transformed += h,
                    h => gesture.Transformed -= h)
                .Select(_ => gesture.DeltaPosition);
        }
        
        public static IObservable<float> DeltaRotationAsObservable(this ScreenTransformGesture gesture)
        {
            return Observable
                .FromEvent<EventHandler<EventArgs>, EventArgs>(
                    h => (sender, e) => h(e),
                    h => gesture.Transformed += h,
                    h => gesture.Transformed -= h)
                .Select(_ => gesture.DeltaRotation);
        }
        
        public static IObservable<float> DeltaScaleAsObservable(this ScreenTransformGesture gesture)
        {
            return Observable
                .FromEvent<EventHandler<EventArgs>, EventArgs>(
                    h => (sender, e) => h(e),
                    h => gesture.Transformed += h,
                    h => gesture.Transformed -= h)
                .Select(_ => gesture.DeltaScale);
        }

        public static IObservable<Unit> TapAsObservable(this TapGesture gesture)
        {
            return Observable
                .FromEvent<EventHandler<EventArgs>, EventArgs>(
                    h => (sender, e) => h(e),
                    h => gesture.Tapped += h,
                    h => gesture.Tapped -= h)
                .Select(_ => Unit.Default);
        }
    }
}
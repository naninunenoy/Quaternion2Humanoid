using System;
using UnityEngine;
using UniRx;

namespace Quaternion2Humanoid {
    public static class UniRxRotationExtension {
        public static IDisposable SubscribeToRotation(this IObservable<Quaternion> source, Transform transform) {
            return source.SubscribeWithState(transform, (x, t) => t.rotation = x);
        }
        public static IDisposable SubscribeToLocalRotation(this IObservable<Quaternion> source, Transform transform) {
            return source.SubscribeWithState(transform, (x, t) => t.rotation = x);
        }
    }
}
using System;
using UnityEngine;

namespace UniRx {
    public static class UniRxRotaionExtension {
        public static IDisposable SubscribeToRotation(this IObservable<Quaternion> source, Transform transform) {
            return source.SubscribeWithState(transform, (x, t) => t.rotation = x);
        }
        public static IDisposable SubscribeToLocalRotation(this IObservable<Quaternion> source, Transform transform) {
            return source.SubscribeWithState(transform, (x, t) => t.rotation = x);
        }
    }
}
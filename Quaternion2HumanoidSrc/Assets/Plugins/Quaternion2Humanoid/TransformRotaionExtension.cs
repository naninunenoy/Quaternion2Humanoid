using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Quaternion2Humanoid {
    public static class TransformRotaionExtension {
        public static Quaternion GetRelativeQuaternion(this Transform baseTransform, Quaternion quaternion) {
            var baseQuaternion = baseTransform.rotation;
            return Quaternion.Inverse(baseQuaternion) * quaternion * baseQuaternion;
        }
        public static IObservable<Quaternion> ToRelativeQuaternionObservable(this IObservable<Quaternion> source, Transform baseTransform) {
            return source.Select(baseTransform.GetRelativeQuaternion);
        }
    }
}


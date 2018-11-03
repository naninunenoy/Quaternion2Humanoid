using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Quaternion2Humanoid {
    public static class QuaternionExtension {
        public static Quaternion ToRelativedQuaternion(this Quaternion quaternion, Quaternion baseQuaternion) {
            return baseQuaternion * quaternion;
        }
        public static IObservable<Quaternion> ToRelativedQuaternionObservable(this IObservable<Quaternion> source, Quaternion baseQuaternion) {
            return source.Select(q => q.ToRelativedQuaternion(baseQuaternion));
        }
    }
}

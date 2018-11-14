using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Quaternion2Humanoid {
    public static class QuaternionExtension {
        public static Quaternion ToRelativeQuaternion(this Quaternion quaternion, Quaternion baseQuaternion) {
            return quaternion * baseQuaternion;
        }
        public static IObservable<Quaternion> ToRelativeQuaternionObservable(this IObservable<Quaternion> source, Quaternion baseQuaternion) {
            return source.Select(q => q.ToRelativeQuaternion(baseQuaternion));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Assets.Quaternion2Humanoid.Scripts {
    public class UIReactiveQuaternion : MonoBehaviour {
        [SerializeField] ValueSlider sliderW;
        [SerializeField] ValueSlider sliderX;
        [SerializeField] ValueSlider sliderY;
        [SerializeField] ValueSlider sliderZ;

        ReactiveProperty<Quaternion> reactiveQuaternion = new ReactiveProperty<Quaternion>();
        public IReadOnlyReactiveProperty<Quaternion> ReactiveQuaternion { get { return reactiveQuaternion; } }
        bool isLockReactiveQuaternion = true;

        public void OverrideQuaternion(Quaternion quaternion) {
            // scriptからの上書きはReactivePropertyとして通知しない
            isLockReactiveQuaternion = false;
            sliderW.SetValue(quaternion.w);
            sliderX.SetValue(quaternion.x);
            sliderY.SetValue(quaternion.y);
            sliderZ.SetValue(quaternion.z);
            isLockReactiveQuaternion = true;
        }

        void Awake() {
            Observable.CombineLatest(
                sliderW.ReactiveValue,
                sliderX.ReactiveValue,
                sliderY.ReactiveValue,
                sliderZ.ReactiveValue
            )
            .Where(_ => { return isLockReactiveQuaternion; })
            .Subscribe(values => {
                var quat = new Quaternion(values[1], values[2], values[3], values[0]).normalized;
                reactiveQuaternion.Value = quat;
                OverrideQuaternion(quat);
                Debug.Log(quat);
            }).AddTo(this);
        }
    }
}

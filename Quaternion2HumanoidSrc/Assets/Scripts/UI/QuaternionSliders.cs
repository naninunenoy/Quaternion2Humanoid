using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Quaternion2Humanoid;

namespace Assets.Quaternion2Humanoid.Scripts.UI {
    public class QuaternionSliders : MonoBehaviour, IOverwritableReactiveQuaternion {
        [SerializeField] ValueSlider sliderW;
        [SerializeField] ValueSlider sliderX;
        [SerializeField] ValueSlider sliderY;
        [SerializeField] ValueSlider sliderZ;

        bool isLockReactiveQuaternion = true;
        ReactiveProperty<Quaternion> reactiveQuaternion = new ReactiveProperty<Quaternion>();
        public IReadOnlyReactiveProperty<Quaternion> ReactiveQuaternion { get { return reactiveQuaternion; } }

        void Awake() {
            // sliderによるQuaternion更新を通知
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
                OverwriteQuaternion(quat);
            }).AddTo(this);
        }

        public void OverwriteQuaternion(Quaternion quaternion) {
            // scriptからの上書きはReactivePropertyとして通知しない
            isLockReactiveQuaternion = false;
            SetQuaternionToSliders(quaternion);
            isLockReactiveQuaternion = true;
        }

        private void SetQuaternionToSliders(Quaternion quaternion) {
            sliderW.Slider.value = quaternion.w;
            sliderX.Slider.value = quaternion.x;
            sliderY.Slider.value = quaternion.y;
            sliderZ.Slider.value = quaternion.z;
        }

        public void ValidateAutoUpdate(Component addto) {
            ValidateAutoUpdateSafe(sliderW.GetComponent<ValueAutoUpdater>(), addto);
            ValidateAutoUpdateSafe(sliderX.GetComponent<ValueAutoUpdater>(), addto);
            ValidateAutoUpdateSafe(sliderY.GetComponent<ValueAutoUpdater>(), addto);
            ValidateAutoUpdateSafe(sliderZ.GetComponent<ValueAutoUpdater>(), addto);
        }

        private void ValidateAutoUpdateSafe(ValueAutoUpdater autoUpdater, Component addTo) {
            if (autoUpdater != null) autoUpdater.StartAutoUpdate(addTo);
        }
    }
}
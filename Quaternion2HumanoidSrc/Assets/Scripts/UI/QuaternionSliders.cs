using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Quaternion2Humanoid;

namespace Assets.Quaternion2Humanoid.Scripts.UI {
    public class QuaternionSliders : MonoBehaviour, IOverwritableReactiveQuaternion {
        [SerializeField]
        protected ValueSlider sliderW;
        [SerializeField]
        protected ValueSlider sliderX;
        [SerializeField]
        protected ValueSlider sliderY;
        [SerializeField]
        protected ValueSlider sliderZ;

        bool isLockReactiveQuaternion = false;
        ReactiveProperty<Quaternion> reactiveQuaternion = new ReactiveProperty<Quaternion>();
        public IReadOnlyReactiveProperty<Quaternion> ReactiveQuaternion { get { return reactiveQuaternion; } }

        public virtual void Validate(Component comp) {
            sliderW.Validate(comp);
            sliderX.Validate(comp);
            sliderY.Validate(comp);
            sliderZ.Validate(comp);
            // sliderによるQuaternion更新を通知
            SliderQuaternion.Where(_ => { return !isLockReactiveQuaternion; })
                            .Subscribe(quaternion => {
                                OverwriteQuaternion(quaternion, true);
                            })
                            .AddTo(comp);
            // 自動更新可能に
            ValidateAutoUpdate(comp);
        }

        protected IObservable<Quaternion> SliderQuaternion {
            get {
                return Observable.CombineLatest(
                    sliderW.ReactiveValue,
                    sliderX.ReactiveValue,
                    sliderY.ReactiveValue,
                    sliderZ.ReactiveValue
                ).Select(vals => { return new Quaternion(vals[1], vals[2], vals[3], vals[0]).normalized; });
            }
        }

        public void OverwriteQuaternion(Quaternion quaternion, bool notify = true) {
            if (notify) {
                reactiveQuaternion.Value = quaternion;
            }
            SetQuaternionToSliders(quaternion);
        }

        public virtual void SetDefaultQuaternion(Quaternion quaternion) {
            OverwriteQuaternion(Quaternion.identity, true);
        }

        protected void SetQuaternionToSliders(Quaternion quaternion) {
            isLockReactiveQuaternion = true;
            sliderW.Slider.value = quaternion.w;
            sliderX.Slider.value = quaternion.x;
            sliderY.Slider.value = quaternion.y;
            sliderZ.Slider.value = quaternion.z;
            isLockReactiveQuaternion = false;
        }

        private void ValidateAutoUpdate(Component addto) {
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
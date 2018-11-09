﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Assets.Quaternion2Humanoid.Scripts.UI {
    public class QuaternionSliders : MonoBehaviour {
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
                            .Subscribe(SetQuaternion)
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

        protected void SetQuaternion(Quaternion quaternion) {
            reactiveQuaternion.Value = quaternion;
            SetQuaternionToSliders(quaternion);
        }

        protected Quaternion GetQuaternion() {
            return reactiveQuaternion.Value;
        }

        private void SetQuaternionToSliders(Quaternion quaternion) {
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
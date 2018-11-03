﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Quaternion2Humanoid;

namespace Assets.Quaternion2Humanoid.Scripts.UI {
    public class UIReactiveQuaternion : MonoBehaviour, IOverwritableReactiveQuaternion {
        [SerializeField] Button resetButton;
        [SerializeField] ValueSlider sliderW;
        [SerializeField] ValueSlider sliderX;
        [SerializeField] ValueSlider sliderY;
        [SerializeField] ValueSlider sliderZ;

        ReactiveProperty<Quaternion> reactiveQuaternion = new ReactiveProperty<Quaternion>();
        public IReadOnlyReactiveProperty<Quaternion> ReactiveQuaternion { get { return reactiveQuaternion; } }
        bool isLockReactiveQuaternion = true;

        Quaternion defaultQuaternion = Quaternion.identity;
        public void SetDefaultQuaternion(Quaternion quaternion) {
            sliderW.Slider.value = quaternion.w;
            sliderX.Slider.value = quaternion.x;
            sliderY.Slider.value = quaternion.y;
            sliderZ.Slider.value = quaternion.z;
            defaultQuaternion = quaternion;
        }

        public void InitQuaternion() { SetQuaternion(defaultQuaternion); }

        public void OverwriteQuaternion(Quaternion quaternion) {
            // scriptからの上書きはReactivePropertyとして通知しない
            isLockReactiveQuaternion = false;
            SetQuaternion(quaternion);
            isLockReactiveQuaternion = true;
        }

        private void SetQuaternion(Quaternion quaternion) {
            sliderW.Slider.value = quaternion.w;
            sliderX.Slider.value = quaternion.x;
            sliderY.Slider.value = quaternion.y;
            sliderZ.Slider.value = quaternion.z;
        }

        void Awake() {
            // リセットボタン
            resetButton.onClick.AddListener(InitQuaternion);
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
                // scriptからの上書きはReactivePropertyとして通知しない
                isLockReactiveQuaternion = false;
                SetQuaternion(quat);
                isLockReactiveQuaternion = true;
            }).AddTo(this);
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
